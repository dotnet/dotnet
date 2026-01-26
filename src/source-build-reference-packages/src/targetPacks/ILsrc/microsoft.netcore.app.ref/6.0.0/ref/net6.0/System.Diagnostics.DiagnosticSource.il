


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
// Time-date stamp:            0x9eb77d08
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00005a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000784e
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
// Checksum:                       0x00011320
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000077fb [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x000004f4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00006400 [0x00002480] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000076f8 [0x00000054] address [size] of Debug Directory:           
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
//              0x00005854 Virtual Size
//              0x00002000 Virtual Address
//              0x00005a00 Size of Raw Data
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
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00005c00 Pointer to Raw Data
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
//              0x00006200 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000850
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000783d Import Name Table
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
// 0x00002484 [0x000051f4] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00007678 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000030d8 Size
//              '#~' Name
//    Stream 2:
//              0x00003144 Offset
//              0x0000140c Size
//              '#Strings' Name
//    Stream 3:
//              0x00004550 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00004554 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00004564 Offset
//              0x00000c90 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016090ba29f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 34944
// PE header size       : 512 (496 used)    ( 1.47%)
// PE additional info   : 10867             (31.10%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.21%)
// CLR meta-data size  : 20980              (60.04%)
// CLR additional info : 128                ( 0.37%)
// CLR method headers  : 299                ( 0.86%)
// Managed code         : 773               ( 2.21%)
// Data                 : 2048              ( 5.86%)
// Unaccounted          : -735              (-2.10%)

// Num.of PE sections   : 3
//   .text    - 23040
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 20980
//   Module        -    1 (10 bytes)
//   TypeDef       -   40 (560 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   58 (348 bytes)
//   MethodDef     -  311 (4354 bytes)    7 abstract, 0 native, 288 bodies
//   FieldDef      -   30 (180 bytes)     0 constant
//   MemberRef     -   31 (186 bytes)
//   ParamDef      -  406 (2436 bytes)
//   MethodImpl    -    6 (36 bytes)
//   Constant      -   65 (390 bytes)
//   CustomAttribute-  372 (2232 bytes)
//   ClassLayout   -    7 (56 bytes)
//   StandAloneSig -    1 (2 bytes)
//   InterfaceImpl -   27 (108 bytes)
//   PropertyMap   -   18 (72 bytes)
//   Property      -   84 (504 bytes)
//   MethodSemantic-  102 (612 bytes)
//   TypeSpec      -   14 (28 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   NestedClass   -    4 (16 bytes)
//   GenericParam  -   19 (152 bytes)
//   GenericParamConstraint-   16 (64 bytes)
//   Strings       -  5132 bytes
//   Blobs         -  3216 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   224 bytes

// CLR additional info : 128

// CLR method headers : 299
//   Num.of method bodies  - 288
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 287

// Managed code : 773
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class DiagnosticListener             (public) (auto) (ansi) 
// Class DiagnosticSource               (public) (abstract) (auto) (ansi) 
// Class Activity                       (public) (auto) (ansi) 
// Class ActivityTagsCollection         (public) (auto) (ansi) 
// Class ActivityStatusCode             (public) (auto) (ansi) (sealed) 
// Class ActivityIdFormat               (public) (auto) (ansi) (sealed) 
// Class ActivitySpanId                 (public) (sequential) (ansi) (sealed) 
// Class ActivitySource                 (public) (auto) (ansi) (sealed) 
// Class ActivityTraceFlags             (public) (auto) (ansi) (sealed) 
// Class ActivityTraceId                (public) (sequential) (ansi) (sealed) 
// Class ActivitySamplingResult         (public) (auto) (ansi) (sealed) 
// Class ActivityKind                   (public) (auto) (ansi) (sealed) 
// Class ActivityEvent                  (public) (sequential) (ansi) (sealed) 
// Class ActivityContext                (public) (sequential) (ansi) (sealed) 
// Class ActivityLink                   (public) (sequential) (ansi) (sealed) 
// Class ActivityCreationOptions`1      (public) (sequential) (ansi) (sealed) 
// Class SampleActivity`1               (public) (auto) (ansi) (sealed) 
// Class ActivityListener               (public) (auto) (ansi) (sealed) 
// Class DistributedContextPropagator   (public) (abstract) (auto) (ansi) 
// Class TagList                        (public) (sequential) (ansi) (sealed) 
// Class Counter`1                      (public) (auto) (ansi) (sealed) 
// Class Histogram`1                    (public) (auto) (ansi) (sealed) 
// Class Instrument                     (public) (abstract) (auto) (ansi) 
// Class Instrument`1                   (public) (abstract) (auto) (ansi) 
// Class Measurement`1                  (public) (sequential) (ansi) (sealed) 
// Class MeasurementCallback`1          (public) (auto) (ansi) (sealed) 
// Class Meter                          (public) (auto) (ansi) 
// Class MeterListener                  (public) (auto) (ansi) (sealed) 
// Class ObservableCounter`1            (public) (auto) (ansi) (sealed) 
// Class ObservableGauge`1              (public) (auto) (ansi) (sealed) 
// Class ObservableInstrument`1         (public) (abstract) (auto) (ansi) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
// Class PropagatorGetterCallback       (auto) (ansi) (sealed) (nested public) 
// Class PropagatorSetterCallback       (auto) (ansi) (sealed) (nested public) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Diagnostics.DiagnosticSource
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                        73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                        53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                       73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                       53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                 73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                 53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C000014:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                4B 86 C4 CB 78 54 9B 34 BA B6 1A 3B 18 00 E2 3B   // K...xT.4...;...;
                FE B5 B3 EC 39 00 74 04 15 36 A7 E3 CB D9 7F 5F   // ....9.t..6....._
                04 CF 0F 85 71 55 A8 92 8E AA 29 EB FD 11 CF BB   // ....qU....).....
                AD 3B A7 0E FE A7 BD A3 22 6C 6A 8D 37 0A 4C D3   // .;......"lj.7.L.
                03 F7 14 48 6B 6E BC 22 59 85 A6 38 47 1E 6E F5   // ...Hkn."Y..8G.n.
                71 CC 92 A4 61 3C 00 B8 FA 65 D6 1C CE E0 CB E5   // q...a<...e......
                F3 63 30 C9 A0 1F 41 83 55 9F 1B EF 24 CC 29 17   // .c0...A.U...$.).
                C6 D9 13 E3 A5 41 33 3A 1D 05 D9 BE D2 2B 38 CB ) // .....A3:.....+8.
  .hash algorithm 0x00008004
  .ver 6:0:0:0
}
.module System.Diagnostics.DiagnosticSource.dll
// MVID: {91f403a8-9c44-478a-9d60-f10f9569e60b}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C000016:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C00001A:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:0A000010*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000013*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000012*/) /* 0A000010 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00003A       */ newarr     [System.Runtime/*23000001*/]System.Byte/*0100003A*/
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
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C00001F:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000010*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000013*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000012*/) /* 0A000010 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C000024:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000010*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000013*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000012*/) /* 0A000010 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001C */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public auto ansi beforefieldinit System.Diagnostics.DiagnosticListener
       extends System.Diagnostics.DiagnosticSource/*02000007*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/,
                  class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000001*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000001*/
  .custom /*0C000018:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000015       */ call       instance void System.Diagnostics.DiagnosticSource/*02000007*/::.ctor() /* 06000015 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::.ctor

  .method /*06000007*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<class System.Diagnostics.DiagnosticListener/*02000006*/> 
          get_AllListeners() cil managed
  // SIG: 00 00 15 12 55 01 12 18
  {
    // Method begins at RVA 0x20a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::get_AllListeners

  .method /*06000008*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::get_Name

  .method /*06000009*/ public hidebysig newslot virtual 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ab
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::Dispose

  .method /*0600000A*/ public hidebysig instance bool 
          IsEnabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*0600000B*/ public hidebysig virtual 
          instance bool  IsEnabled(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*0600000C*/ public hidebysig virtual 
          instance bool  IsEnabled(string name,
                                   object arg1,
                                   [opt] object arg2) cil managed
  // SIG: 20 03 02 0E 1C 1C
  {
    .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000003*/ 
    .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000005*/  = nullref
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*0600000D*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000014*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*01000018*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> observer) cil managed
  // SIG: 20 01 12 51 15 12 61 01 15 11 59 02 0E 1C
  {
    .param [1]/*08000006*/ 
    .custom /*0C00002B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x20b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*0600000E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000014*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*01000018*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Func`4/*01000019*/<string,object,object,bool> isEnabled) cil managed
  // SIG: 20 02 12 51 15 12 61 01 15 11 59 02 0E 1C 15 12 65 04 0E 1C 1C 02
  {
    .param [1]/*08000007*/ 
    .custom /*0C00002F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*08000008*/ 
    .custom /*0C000034:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    // Method begins at RVA 0x20b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*0600000F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000014*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*01000018*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Predicate`1/*0100001A*/<string> isEnabled) cil managed
  // SIG: 20 02 12 51 15 12 61 01 15 11 59 02 0E 1C 15 12 69 01 0E
  {
    .param [1]/*08000009*/ 
    .custom /*0C000038:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*0800000A*/ 
    .custom /*0C00003B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*06000010*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::ToString

  .method /*06000011*/ public hidebysig virtual 
          instance void  Write(string name,
                               object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C000051:0A000011*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000011 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .param [2]/*0800000C*/ 
    .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::Write

  .method /*06000012*/ public hidebysig virtual 
          instance void  OnActivityExport(class System.Diagnostics.Activity/*02000008*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 20 1C
  {
    .param [2]/*0800000E*/ 
    .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::OnActivityExport

  .method /*06000013*/ public hidebysig virtual 
          instance void  OnActivityImport(class System.Diagnostics.Activity/*02000008*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 20 1C
  {
    .param [2]/*08000010*/ 
    .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::OnActivityImport

  .method /*06000014*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000014*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*01000018*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Func`4/*01000019*/<string,object,object,bool> isEnabled,
                    [opt] class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Activity/*02000008*/,object> onActivityImport,
                    [opt] class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Activity/*02000008*/,object> onActivityExport) cil managed
  // SIG: 20 04 12 51 15 12 61 01 15 11 59 02 0E 1C 15 12 65 04 0E 1C 1C 02 15 12 71 02 12 20 1C 15 12 71
  //      02 12 20 1C
  {
    .param [1]/*08000011*/ 
    .custom /*0C000052:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*08000012*/ 
    .custom /*0C000058:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    .param [3]/*08000013*/  = nullref
    .custom /*0C00005E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    .param [4]/*08000014*/  = nullref
    .custom /*0C000061:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x20c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .property /*17000001*/ callconv(8) class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<class System.Diagnostics.DiagnosticListener/*02000006*/>
          AllListeners()
  {
    .get class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<class System.Diagnostics.DiagnosticListener/*02000006*/> System.Diagnostics.DiagnosticListener/*02000006*/::get_AllListeners() /* 06000007 */
  } // end of property DiagnosticListener::AllListeners
  .property /*17000002*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.DiagnosticListener/*02000006*/::get_Name() /* 06000008 */
  } // end of property DiagnosticListener::Name
} // end of class System.Diagnostics.DiagnosticListener

.class /*02000007*/ public abstract auto ansi beforefieldinit System.Diagnostics.DiagnosticSource
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000015*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20cb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::.ctor

  .method /*06000016*/ public hidebysig newslot abstract virtual 
          instance bool  IsEnabled(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x0
  } // end of method DiagnosticSource::IsEnabled

  .method /*06000017*/ public hidebysig newslot virtual 
          instance bool  IsEnabled(string name,
                                   object arg1,
                                   [opt] object arg2) cil managed
  // SIG: 20 03 02 0E 1C 1C
  {
    .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000016*/ 
    .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000018*/  = nullref
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticSource::IsEnabled

  .method /*06000018*/ public hidebysig newslot abstract virtual 
          instance void  Write(string name,
                               object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C00006C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000011 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .param [2]/*0800001A*/ 
    .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DiagnosticSource::Write

  .method /*06000019*/ public hidebysig newslot virtual 
          instance void  OnActivityExport(class System.Diagnostics.Activity/*02000008*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 20 1C
  {
    .param [2]/*0800001C*/ 
    .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::OnActivityExport

  .method /*0600001A*/ public hidebysig newslot virtual 
          instance void  OnActivityImport(class System.Diagnostics.Activity/*02000008*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 20 1C
  {
    .param [2]/*0800001E*/ 
    .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::OnActivityImport

  .method /*0600001B*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          StartActivity(class System.Diagnostics.Activity/*02000008*/ activity,
                        object args) cil managed
  // SIG: 20 02 12 20 12 20 1C
  {
    .custom /*0C000075:0A000011*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000011 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .param [2]/*08000020*/ 
    .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticSource::StartActivity

  .method /*0600001C*/ public hidebysig instance void 
          StopActivity(class System.Diagnostics.Activity/*02000008*/ activity,
                       object args) cil managed
  // SIG: 20 02 01 12 20 1C
  {
    .custom /*0C000076:0A000011*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000011 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .param [2]/*08000022*/ 
    .custom /*0C000085:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20dd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::StopActivity

} // end of class System.Diagnostics.DiagnosticSource

.class /*02000008*/ public auto ansi beforefieldinit System.Diagnostics.Activity
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string operationName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20df
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Activity::.ctor

  .method /*0600001E*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ 
          get_ActivityTraceFlags() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ActivityTraceFlags

  .method /*0600001F*/ public hidebysig specialname 
          instance void  set_ActivityTraceFlags(valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ 'value') cil managed
  // SIG: 20 01 01 11 38
  {
    // Method begins at RVA 0x20ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_ActivityTraceFlags

  .method /*06000020*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> 
          get_Baggage() cil managed
  // SIG: 20 00 15 12 75 01 15 11 59 02 0E 0E
  {
    .param [0]/*08000025*/ 
    .custom /*0C000088:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Baggage

  .method /*06000021*/ public hidebysig specialname static 
          class System.Diagnostics.Activity/*02000008*/ 
          get_Current() cil managed
  // SIG: 00 00 12 20
  {
    .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Current

  .method /*06000022*/ public hidebysig specialname static 
          void  set_Current(class System.Diagnostics.Activity/*02000008*/ 'value') cil managed
  // SIG: 00 01 01 12 20
  {
    .custom /*0C000084:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_Current

  .method /*06000023*/ public hidebysig specialname static 
          valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ 
          get_DefaultIdFormat() cil managed
  // SIG: 00 00 11 2C
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_DefaultIdFormat

  .method /*06000024*/ public hidebysig specialname static 
          void  set_DefaultIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ 'value') cil managed
  // SIG: 00 01 01 11 2C
  {
    // Method begins at RVA 0x20f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_DefaultIdFormat

  .method /*06000025*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100001E*/ 
          get_Duration() cil managed
  // SIG: 20 00 11 79
  {
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Duration

  .method /*06000026*/ public hidebysig specialname static 
          bool  get_ForceDefaultIdFormat() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ForceDefaultIdFormat

  .method /*06000027*/ public hidebysig specialname static 
          void  set_ForceDefaultIdFormat(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x20ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_ForceDefaultIdFormat

  .method /*06000028*/ public hidebysig specialname 
          instance string  get_Id() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00008C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Id

  .method /*06000029*/ public hidebysig specialname 
          instance bool  get_IsAllDataRequested() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2104
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_IsAllDataRequested

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_IsAllDataRequested(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_IsAllDataRequested

  .method /*0600002B*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ 
          get_IdFormat() cil managed
  // SIG: 20 00 11 2C
  {
    // Method begins at RVA 0x210a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_IdFormat

  .method /*0600002C*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityKind/*02000011*/ 
          get_Kind() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Kind

  .method /*0600002D*/ public hidebysig specialname 
          instance string  get_OperationName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_OperationName

  .method /*0600002E*/ public hidebysig specialname 
          instance string  get_DisplayName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_DisplayName

  .method /*0600002F*/ public hidebysig specialname 
          instance void  set_DisplayName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_DisplayName

  .method /*06000030*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivitySource/*0200000D*/ 
          get_Source() cil managed
  // SIG: 20 00 12 34
  {
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Source

  .method /*06000031*/ public hidebysig specialname 
          instance class System.Diagnostics.Activity/*02000008*/ 
          get_Parent() cil managed
  // SIG: 20 00 12 20
  {
    .custom /*0C000091:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Parent

  .method /*06000032*/ public hidebysig specialname 
          instance string  get_ParentId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ParentId

  .method /*06000033*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ 
          get_ParentSpanId() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ParentSpanId

  .method /*06000034*/ public hidebysig specialname 
          instance bool  get_Recorded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Recorded

  .method /*06000035*/ public hidebysig specialname 
          instance string  get_RootId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000096:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_RootId

  .method /*06000036*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ 
          get_SpanId() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_SpanId

  .method /*06000037*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100001F*/ 
          get_StartTimeUtc() cil managed
  // SIG: 20 00 11 7D
  {
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_StartTimeUtc

  .method /*06000038*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityStatusCode/*0200000A*/ 
          get_Status() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Status

  .method /*06000039*/ public hidebysig specialname 
          instance string  get_StatusDescription() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00009B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_StatusDescription

  .method /*0600003A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 75 01 15 11 59 02 0E 0E
  {
    .param [0]/*0800002B*/ 
    .custom /*0C00008E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Tags

  .method /*0600003B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_TagObjects() cil managed
  // SIG: 20 00 15 12 75 01 15 11 59 02 0E 1C
  {
    .param [0]/*0800002C*/ 
    .custom /*0C00008F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TagObjects

  .method /*0600003C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityEvent/*02000012*/> 
          get_Events() cil managed
  // SIG: 20 00 15 12 75 01 11 48
  {
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Events

  .method /*0600003D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> 
          get_Links() cil managed
  // SIG: 20 00 15 12 75 01 11 50
  {
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Links

  .method /*0600003E*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 3C
  {
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceId

  .method /*0600003F*/ public hidebysig specialname 
          instance string  get_TraceStateString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceStateString

  .method /*06000040*/ public hidebysig specialname 
          instance void  set_TraceStateString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2149
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_TraceStateString

  .method /*06000041*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          AddBaggage(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 20 0E 0E
  {
    .param [2]/*0800002F*/ 
    .custom /*0C000090:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddBaggage

  .method /*06000042*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          AddEvent(valuetype System.Diagnostics.ActivityEvent/*02000012*/ e) cil managed
  // SIG: 20 01 12 20 11 48
  {
    // Method begins at RVA 0x214e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddEvent

  .method /*06000043*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          AddTag(string key,
                 string 'value') cil managed
  // SIG: 20 02 12 20 0E 0E
  {
    .param [2]/*08000032*/ 
    .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2151
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddTag

  .method /*06000044*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          AddTag(string key,
                 object 'value') cil managed
  // SIG: 20 02 12 20 0E 1C
  {
    .param [2]/*08000034*/ 
    .custom /*0C000095:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2154
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddTag

  .method /*06000045*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          SetTag(string key,
                 object 'value') cil managed
  // SIG: 20 02 12 20 0E 1C
  {
    .param [2]/*08000036*/ 
    .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2157
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetTag

  .method /*06000046*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          SetBaggage(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 20 0E 0E
  {
    .param [2]/*08000038*/ 
    .custom /*0C00009A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x215a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetBaggage

  .method /*06000047*/ public hidebysig instance string 
          GetBaggageItem(string key) cil managed
  // SIG: 20 01 0E 0E
  {
    .param [0]/*08000039*/ 
    .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x215d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetBaggageItem

  .method /*06000048*/ public hidebysig instance object 
          GetTagItem(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*0800003B*/ 
    .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2160
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetTagItem

  .method /*06000049*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          SetEndTime(valuetype [System.Runtime/*23000001*/]System.DateTime/*0100001F*/ endTimeUtc) cil managed
  // SIG: 20 01 12 20 11 7D
  {
    // Method begins at RVA 0x2163
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetEndTime

  .method /*0600004A*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<valuetype System.Diagnostics.ActivityTraceId/*0200000F*/> 
          get_TraceIdGenerator() cil managed
  // SIG: 00 00 15 12 80 81 01 11 3C
  {
    .custom /*0C0000A9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2166
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceIdGenerator

  .method /*0600004B*/ public hidebysig specialname static 
          void  set_TraceIdGenerator(class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<valuetype System.Diagnostics.ActivityTraceId/*0200000F*/> 'value') cil managed
  // SIG: 00 01 01 15 12 80 81 01 11 3C
  {
    .custom /*0C0000AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2169
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_TraceIdGenerator

  .method /*0600004C*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          SetIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ format) cil managed
  // SIG: 20 01 12 20 11 2C
  {
    // Method begins at RVA 0x216c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetIdFormat

  .method /*0600004D*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          SetParentId(valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ traceId,
                      valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ spanId,
                      [opt] valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ activityTraceFlags) cil managed
  // SIG: 20 03 12 20 11 3C 11 30 11 38
  {
    .param [3]/*08000042*/  = int32(0x00000000)
    // Method begins at RVA 0x216f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetParentId

  .method /*0600004E*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          SetParentId(string parentId) cil managed
  // SIG: 20 01 12 20 0E
  {
    // Method begins at RVA 0x2172
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetParentId

  .method /*0600004F*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          SetStartTime(valuetype [System.Runtime/*23000001*/]System.DateTime/*0100001F*/ startTimeUtc) cil managed
  // SIG: 20 01 12 20 11 7D
  {
    // Method begins at RVA 0x2175
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetStartTime

  .method /*06000050*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          SetStatus(valuetype System.Diagnostics.ActivityStatusCode/*0200000A*/ code,
                    [opt] string description) cil managed
  // SIG: 20 02 12 20 11 28 0E
  {
    .param [2]/*08000046*/  = nullref
    .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2178
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetStatus

  .method /*06000051*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          Start() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x217b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Start

  .method /*06000052*/ public hidebysig instance void 
          Stop() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x217e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Stop

  .method /*06000053*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2181
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Dispose

  .method /*06000054*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2184
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Dispose

  .method /*06000055*/ public hidebysig instance void 
          SetCustomProperty(string propertyName,
                            object propertyValue) cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000049*/ 
    .custom /*0C0000A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetCustomProperty

  .method /*06000056*/ public hidebysig instance object 
          GetCustomProperty(string propertyName) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*0800004A*/ 
    .custom /*0C0000AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetCustomProperty

  .method /*06000057*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityContext/*02000013*/ 
          get_Context() cil managed
  // SIG: 20 00 11 4C
  {
    // Method begins at RVA 0x218d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Context

  .property /*17000003*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/
          ActivityTraceFlags()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ System.Diagnostics.Activity/*02000008*/::get_ActivityTraceFlags() /* 0600001E */
    .set instance void System.Diagnostics.Activity/*02000008*/::set_ActivityTraceFlags(valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/) /* 0600001F */
  } // end of property Activity::ActivityTraceFlags
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>>
          Baggage()
  {
    .custom /*0C000023:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> System.Diagnostics.Activity/*02000008*/::get_Baggage() /* 06000020 */
  } // end of property Activity::Baggage
  .property /*17000005*/ callconv(8) class System.Diagnostics.Activity/*02000008*/
          Current()
  {
    .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get class System.Diagnostics.Activity/*02000008*/ System.Diagnostics.Activity/*02000008*/::get_Current() /* 06000021 */
    .set void System.Diagnostics.Activity/*02000008*/::set_Current(class System.Diagnostics.Activity/*02000008*/) /* 06000022 */
  } // end of property Activity::Current
  .property /*17000006*/ callconv(8) valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/
          DefaultIdFormat()
  {
    .get valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ System.Diagnostics.Activity/*02000008*/::get_DefaultIdFormat() /* 06000023 */
    .set void System.Diagnostics.Activity/*02000008*/::set_DefaultIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/) /* 06000024 */
  } // end of property Activity::DefaultIdFormat
  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100001E*/
          Duration()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100001E*/ System.Diagnostics.Activity/*02000008*/::get_Duration() /* 06000025 */
  } // end of property Activity::Duration
  .property /*17000008*/ callconv(8) bool
          ForceDefaultIdFormat()
  {
    .get bool System.Diagnostics.Activity/*02000008*/::get_ForceDefaultIdFormat() /* 06000026 */
    .set void System.Diagnostics.Activity/*02000008*/::set_ForceDefaultIdFormat(bool) /* 06000027 */
  } // end of property Activity::ForceDefaultIdFormat
  .property /*17000009*/ instance callconv(8) string
          Id()
  {
    .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000008*/::get_Id() /* 06000028 */
  } // end of property Activity::Id
  .property /*1700000A*/ instance callconv(8) bool
          IsAllDataRequested()
  {
    .get instance bool System.Diagnostics.Activity/*02000008*/::get_IsAllDataRequested() /* 06000029 */
    .set instance void System.Diagnostics.Activity/*02000008*/::set_IsAllDataRequested(bool) /* 0600002A */
  } // end of property Activity::IsAllDataRequested
  .property /*1700000B*/ instance callconv(8) valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/
          IdFormat()
  {
    .get instance valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ System.Diagnostics.Activity/*02000008*/::get_IdFormat() /* 0600002B */
  } // end of property Activity::IdFormat
  .property /*1700000C*/ instance callconv(8) valuetype System.Diagnostics.ActivityKind/*02000011*/
          Kind()
  {
    .get instance valuetype System.Diagnostics.ActivityKind/*02000011*/ System.Diagnostics.Activity/*02000008*/::get_Kind() /* 0600002C */
  } // end of property Activity::Kind
  .property /*1700000D*/ instance callconv(8) string
          OperationName()
  {
    .get instance string System.Diagnostics.Activity/*02000008*/::get_OperationName() /* 0600002D */
  } // end of property Activity::OperationName
  .property /*1700000E*/ instance callconv(8) string
          DisplayName()
  {
    .get instance string System.Diagnostics.Activity/*02000008*/::get_DisplayName() /* 0600002E */
    .set instance void System.Diagnostics.Activity/*02000008*/::set_DisplayName(string) /* 0600002F */
  } // end of property Activity::DisplayName
  .property /*1700000F*/ instance callconv(8) class System.Diagnostics.ActivitySource/*0200000D*/
          Source()
  {
    .get instance class System.Diagnostics.ActivitySource/*0200000D*/ System.Diagnostics.Activity/*02000008*/::get_Source() /* 06000030 */
  } // end of property Activity::Source
  .property /*17000010*/ instance callconv(8) class System.Diagnostics.Activity/*02000008*/
          Parent()
  {
    .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Diagnostics.Activity/*02000008*/ System.Diagnostics.Activity/*02000008*/::get_Parent() /* 06000031 */
  } // end of property Activity::Parent
  .property /*17000011*/ instance callconv(8) string
          ParentId()
  {
    .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000008*/::get_ParentId() /* 06000032 */
  } // end of property Activity::ParentId
  .property /*17000012*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*0200000C*/
          ParentSpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ System.Diagnostics.Activity/*02000008*/::get_ParentSpanId() /* 06000033 */
  } // end of property Activity::ParentSpanId
  .property /*17000013*/ instance callconv(8) bool
          Recorded()
  {
    .get instance bool System.Diagnostics.Activity/*02000008*/::get_Recorded() /* 06000034 */
  } // end of property Activity::Recorded
  .property /*17000014*/ instance callconv(8) string
          RootId()
  {
    .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000008*/::get_RootId() /* 06000035 */
  } // end of property Activity::RootId
  .property /*17000015*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*0200000C*/
          SpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ System.Diagnostics.Activity/*02000008*/::get_SpanId() /* 06000036 */
  } // end of property Activity::SpanId
  .property /*17000016*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*0100001F*/
          StartTimeUtc()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100001F*/ System.Diagnostics.Activity/*02000008*/::get_StartTimeUtc() /* 06000037 */
  } // end of property Activity::StartTimeUtc
  .property /*17000017*/ instance callconv(8) valuetype System.Diagnostics.ActivityStatusCode/*0200000A*/
          Status()
  {
    .get instance valuetype System.Diagnostics.ActivityStatusCode/*0200000A*/ System.Diagnostics.Activity/*02000008*/::get_Status() /* 06000038 */
  } // end of property Activity::Status
  .property /*17000018*/ instance callconv(8) string
          StatusDescription()
  {
    .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000008*/::get_StatusDescription() /* 06000039 */
  } // end of property Activity::StatusDescription
  .property /*17000019*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>>
          Tags()
  {
    .custom /*0C000072:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> System.Diagnostics.Activity/*02000008*/::get_Tags() /* 0600003A */
  } // end of property Activity::Tags
  .property /*1700001A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          TagObjects()
  {
    .custom /*0C000074:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.Activity/*02000008*/::get_TagObjects() /* 0600003B */
  } // end of property Activity::TagObjects
  .property /*1700001B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityEvent/*02000012*/>
          Events()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityEvent/*02000012*/> System.Diagnostics.Activity/*02000008*/::get_Events() /* 0600003C */
  } // end of property Activity::Events
  .property /*1700001C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/>
          Links()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> System.Diagnostics.Activity/*02000008*/::get_Links() /* 0600003D */
  } // end of property Activity::Links
  .property /*1700001D*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*0200000F*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ System.Diagnostics.Activity/*02000008*/::get_TraceId() /* 0600003E */
  } // end of property Activity::TraceId
  .property /*1700001E*/ instance callconv(8) string
          TraceStateString()
  {
    .custom /*0C00007D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000008*/::get_TraceStateString() /* 0600003F */
    .set instance void System.Diagnostics.Activity/*02000008*/::set_TraceStateString(string) /* 06000040 */
  } // end of property Activity::TraceStateString
  .property /*1700001F*/ callconv(8) class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<valuetype System.Diagnostics.ActivityTraceId/*0200000F*/>
          TraceIdGenerator()
  {
    .custom /*0C00007E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<valuetype System.Diagnostics.ActivityTraceId/*0200000F*/> System.Diagnostics.Activity/*02000008*/::get_TraceIdGenerator() /* 0600004A */
    .set void System.Diagnostics.Activity/*02000008*/::set_TraceIdGenerator(class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<valuetype System.Diagnostics.ActivityTraceId/*0200000F*/>) /* 0600004B */
  } // end of property Activity::TraceIdGenerator
  .property /*17000020*/ instance callconv(8) valuetype System.Diagnostics.ActivityContext/*02000013*/
          Context()
  {
    .get instance valuetype System.Diagnostics.ActivityContext/*02000013*/ System.Diagnostics.Activity/*02000008*/::get_Context() /* 06000057 */
  } // end of property Activity::Context
} // end of class System.Diagnostics.Activity

.class /*02000009*/ public auto ansi beforefieldinit System.Diagnostics.ActivityTagsCollection
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000025*/<string,object>/*1B000003*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000004*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/
{
  .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000037:0A000012*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000021*/::.ctor(string) /* 0A000012 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000025*/<string,object>/*1B000003*/
  .custom /*0C000022:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000004*/
  .custom /*0C000027:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000002*/
  .custom /*0C00002C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000025*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000005*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000024*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  {
    .pack 0
    .size 1
    .custom /*0C000087:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000005*/
    .custom /*0C000068:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .method /*06000126*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> 
            get_Current() cil managed
    // SIG: 20 00 15 11 59 02 0E 1C
    {
      .param [0]/*0800017B*/ 
      .custom /*0C000167:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      // Method begins at RVA 0x2457
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*06000127*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000024*/::get_Current /*01000024::0A000015*/ 
      // Method begins at RVA 0x245a
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*06000128*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x245d
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Dispose

    .method /*06000129*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x2460
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*0600012A*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000024*/::Reset /*01000024::0A000016*/ 
      // Method begins at RVA 0x2463
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.Reset

    .property /*17000051*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>
            Current()
    {
      .custom /*0C0000B4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> System.Diagnostics.ActivityTagsCollection/*02000009*//Enumerator/*02000025*/::get_Current() /* 06000126 */
    } // end of property Enumerator::Current
    .property /*17000052*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .custom /*0C0000B5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .get instance object System.Diagnostics.ActivityTagsCollection/*02000009*//Enumerator/*02000025*/::System.Collections.IEnumerator.get_Current() /* 06000127 */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .method /*06000058*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2190
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::.ctor

  .method /*06000059*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> list) cil managed
  // SIG: 20 01 01 15 12 75 01 15 11 59 02 0E 1C
  {
    .param [1]/*0800004C*/ 
    .custom /*0C0000AD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2199
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::.ctor

  .method /*0600005A*/ public hidebysig newslot specialname virtual final 
          instance object  get_Item(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*0800004D*/ 
    .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Item

  .method /*0600005B*/ public hidebysig newslot specialname virtual final 
          instance void  set_Item(string key,
                                  object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000050*/ 
    .custom /*0C0000B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivityTagsCollection::set_Item

  .method /*0600005C*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<string> 
          get_Keys() cil managed
  // SIG: 20 00 15 12 80 99 01 0E
  {
    // Method begins at RVA 0x21a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Keys

  .method /*0600005D*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<object> 
          get_Values() cil managed
  // SIG: 20 00 15 12 80 99 01 1C
  {
    .param [0]/*08000051*/ 
    .custom /*0C0000B3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Values

  .method /*0600005E*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Count

  .method /*0600005F*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_IsReadOnly

  .method /*06000060*/ public hidebysig newslot virtual final 
          instance void  Add(string key,
                             object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000053*/ 
    .custom /*0C0000B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Add

  .method /*06000061*/ public hidebysig newslot virtual final 
          instance void  Add(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 01 15 11 59 02 0E 1C
  {
    .param [1]/*08000054*/ 
    .custom /*0C0000B8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Add

  .method /*06000062*/ public hidebysig newslot virtual final 
          instance void  Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Clear

  .method /*06000063*/ public hidebysig newslot virtual final 
          instance bool  Contains(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 59 02 0E 1C
  {
    .param [1]/*08000055*/ 
    .custom /*0C0000BA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Contains

  .method /*06000064*/ public hidebysig newslot virtual final 
          instance bool  ContainsKey(string key) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x21bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::ContainsKey

  .method /*06000065*/ public hidebysig newslot virtual final 
          instance void  CopyTo(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] 'array',
                                int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 59 02 0E 1C 08
  {
    .param [1]/*08000057*/ 
    .custom /*0C0000BB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::CopyTo

  .method /*06000066*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 59 02 0E 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000002*/::GetEnumerator() /* 0A000013 */
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'

  .method /*06000067*/ public hidebysig newslot virtual final 
          instance bool  Remove(string key) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Remove

  .method /*06000068*/ public hidebysig newslot virtual final 
          instance bool  Remove(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 59 02 0E 1C
  {
    .param [1]/*0800005A*/ 
    .custom /*0C0000BC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Remove

  .method /*06000069*/ public hidebysig newslot virtual final 
          instance bool  TryGetValue(string key,
                                     [out] object& 'value') cil managed
  // SIG: 20 02 02 0E 10 1C
  {
    .param [2]/*0800005C*/ 
    .custom /*0C0000BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::TryGetValue

  .method /*0600006A*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000024*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 91
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/::GetEnumerator /*01000023::0A000014*/ 
    // Method begins at RVA 0x21d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::System.Collections.IEnumerable.GetEnumerator

  .method /*0600006B*/ public hidebysig instance valuetype System.Diagnostics.ActivityTagsCollection/*02000009*//Enumerator/*02000025*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 80 94
  {
    // Method begins at RVA 0x21d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::GetEnumerator

  .property /*17000021*/ instance callconv(8) object
          Item(string)
  {
    .custom /*0C000083:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Diagnostics.ActivityTagsCollection/*02000009*/::get_Item(string) /* 0600005A */
    .set instance void System.Diagnostics.ActivityTagsCollection/*02000009*/::set_Item(string,
                                                                                       object) /* 0600005B */
  } // end of property ActivityTagsCollection::Item
  .property /*17000022*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<string>
          Keys()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<string> System.Diagnostics.ActivityTagsCollection/*02000009*/::get_Keys() /* 0600005C */
  } // end of property ActivityTagsCollection::Keys
  .property /*17000023*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<object>
          Values()
  {
    .custom /*0C000086:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<object> System.Diagnostics.ActivityTagsCollection/*02000009*/::get_Values() /* 0600005D */
  } // end of property ActivityTagsCollection::Values
  .property /*17000024*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Diagnostics.ActivityTagsCollection/*02000009*/::get_Count() /* 0600005E */
  } // end of property ActivityTagsCollection::Count
  .property /*17000025*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Diagnostics.ActivityTagsCollection/*02000009*/::get_IsReadOnly() /* 0600005F */
  } // end of property ActivityTagsCollection::IsReadOnly
} // end of class System.Diagnostics.ActivityTagsCollection

.class /*0200000A*/ public auto ansi sealed System.Diagnostics.ActivityStatusCode
       extends [System.Runtime/*23000001*/]System.Enum/*01000028*/
{
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*0200000A*/ Unset = int32(0x00000000)
  .field /*04000006*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*0200000A*/ Ok = int32(0x00000001)
  .field /*04000007*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*0200000A*/ Error = int32(0x00000002)
} // end of class System.Diagnostics.ActivityStatusCode

.class /*0200000B*/ public auto ansi sealed System.Diagnostics.ActivityIdFormat
       extends [System.Runtime/*23000001*/]System.Enum/*01000028*/
{
  .field /*04000008*/ public specialname rtspecialname int32 value__
  .field /*04000009*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ Unknown = int32(0x00000000)
  .field /*0400000A*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ Hierarchical = int32(0x00000001)
  .field /*0400000B*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ W3C = int32(0x00000002)
} // end of class System.Diagnostics.ActivityIdFormat

.class /*0200000C*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivitySpanId
       extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002A*/<valuetype System.Diagnostics.ActivitySpanId/*0200000C*/>/*1B000006*/
{
  .custom /*0C00003F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .field /*0400000C*/ private initonly object _dummy
  .field /*0400000D*/ private initonly int32 _dummyPrimitive
  .method /*0600006C*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002B*/<uint8> destination) cil managed
  // SIG: 20 01 01 15 11 80 AD 01 05
  {
    // Method begins at RVA 0x21d7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivitySpanId::CopyTo

  .method /*0600006D*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ 
          CreateFromBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<uint8> idData) cil managed
  // SIG: 00 01 11 30 15 11 80 B1 01 05
  {
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromBytes

  .method /*0600006E*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ 
          CreateFromString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<char> idData) cil managed
  // SIG: 00 01 11 30 15 11 80 B1 01 03
  {
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromString

  .method /*0600006F*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ 
          CreateFromUtf8String(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<uint8> idData) cil managed
  // SIG: 00 01 11 30 15 11 80 B1 01 05
  {
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromUtf8String

  .method /*06000070*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ 
          CreateRandom() cil managed
  // SIG: 00 00 11 30
  {
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateRandom

  .method /*06000071*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ spanId) cil managed
  // SIG: 20 01 02 11 30
  {
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::Equals

  .method /*06000072*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000062*/ 
    .custom /*0C0000BE:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100002D*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::Equals

  .method /*06000073*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::GetHashCode

  .method /*06000074*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ spanId1,
                            valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ spandId2) cil managed
  // SIG: 00 02 02 11 30 11 30
  {
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::op_Equality

  .method /*06000075*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ spanId1,
                              valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ spandId2) cil managed
  // SIG: 00 02 02 11 30 11 30
  {
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::op_Inequality

  .method /*06000076*/ public hidebysig instance string 
          ToHexString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::ToHexString

  .method /*06000077*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::ToString

} // end of class System.Diagnostics.ActivitySpanId

.class /*0200000D*/ public auto ansi sealed beforefieldinit System.Diagnostics.ActivitySource
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000078*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               [opt] string version) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000068*/  = ""
    .custom /*0C0000BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21fa
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivitySource::.ctor

  .method /*06000079*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::get_Name

  .method /*0600007A*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000CA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2206
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::get_Version

  .method /*0600007B*/ public hidebysig instance bool 
          HasListeners() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::HasListeners

  .method /*0600007C*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*02000011*/ kind) cil managed
  // SIG: 20 02 12 20 0E 11 44
  {
    .param [0]/*08000069*/ 
    .custom /*0C0000C0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*0600007D*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*02000011*/ kind,
                         valuetype System.Diagnostics.ActivityContext/*02000013*/ parentContext,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> links,
                         [opt] valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ idFormat) cil managed
  // SIG: 20 06 12 20 0E 11 44 11 4C 15 12 75 01 15 11 59 02 0E 1C 15 12 75 01 11 50 11 2C
  {
    .custom /*0C0000CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800006C*/ 
    .custom /*0C0000C1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800006F*/  = nullref
    .custom /*0C0000C2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000070*/  = nullref
    .param [6]/*08000071*/  = int32(0x00000000)
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*0600007E*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*02000011*/ kind,
                         string parentId,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> links,
                         [opt] valuetype System.Diagnostics.ActivityIdFormat/*0200000B*/ idFormat) cil managed
  // SIG: 20 06 12 20 0E 11 44 0E 15 12 75 01 15 11 59 02 0E 1C 15 12 75 01 11 50 11 2C
  {
    .param [0]/*08000072*/ 
    .custom /*0C0000C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000076*/  = nullref
    .custom /*0C0000C6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000077*/  = nullref
    .custom /*0C0000C8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000078*/  = int32(0x00000000)
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*0600007F*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          StartActivity([opt] string name,
                        [opt] valuetype System.Diagnostics.ActivityKind/*02000011*/ kind) cil managed
  // SIG: 20 02 12 20 0E 11 44
  {
    .param [0]/*08000079*/ 
    .custom /*0C0000C9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800007A*/  = ""
    .custom /*0C0000CB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CallerMemberNameAttribute/*0100002E*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    .param [2]/*0800007B*/  = int32(0x00000000)
    // Method begins at RVA 0x2215
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*06000080*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          StartActivity(string name,
                        valuetype System.Diagnostics.ActivityKind/*02000011*/ kind,
                        valuetype System.Diagnostics.ActivityContext/*02000013*/ parentContext,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100002F*/ startTime) cil managed
  // SIG: 20 06 12 20 0E 11 44 11 4C 15 12 75 01 15 11 59 02 0E 1C 15 12 75 01 11 50 11 80 BD
  {
    .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800007C*/ 
    .custom /*0C0000CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800007F*/  = nullref
    .custom /*0C0000CE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000080*/  = nullref
    .param [6]/*08000081*/  = nullref
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*06000081*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          StartActivity(string name,
                        valuetype System.Diagnostics.ActivityKind/*02000011*/ kind,
                        string parentId,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100002F*/ startTime) cil managed
  // SIG: 20 06 12 20 0E 11 44 0E 15 12 75 01 15 11 59 02 0E 1C 15 12 75 01 11 50 11 80 BD
  {
    .param [0]/*08000082*/ 
    .custom /*0C0000D1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000086*/  = nullref
    .custom /*0C0000D2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000087*/  = nullref
    .custom /*0C0000D3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000088*/  = nullref
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*06000082*/ public hidebysig instance class System.Diagnostics.Activity/*02000008*/ 
          StartActivity(valuetype System.Diagnostics.ActivityKind/*02000011*/ kind,
                        [opt] valuetype System.Diagnostics.ActivityContext/*02000013*/ parentContext,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100002F*/ startTime,
                        [opt] string name) cil managed
  // SIG: 20 06 12 20 11 44 11 4C 15 12 75 01 15 11 59 02 0E 1C 15 12 75 01 11 50 11 80 BD 0E
  {
    .custom /*0C0000D0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800008A*/  = nullref
    .param [3]/*0800008B*/  = nullref
    .custom /*0C0000D5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [4]/*0800008C*/  = nullref
    .param [5]/*0800008D*/  = nullref
    .param [6]/*0800008E*/  = ""
    .custom /*0C0000D6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CallerMemberNameAttribute/*0100002E*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*06000083*/ public hidebysig static 
          void  AddActivityListener(class System.Diagnostics.ActivityListener/*02000017*/ listener) cil managed
  // SIG: 00 01 01 12 5C
  {
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::AddActivityListener

  .method /*06000084*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::Dispose

  .property /*17000026*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.ActivitySource/*0200000D*/::get_Name() /* 06000079 */
  } // end of property ActivitySource::Name
  .property /*17000027*/ instance callconv(8) string
          Version()
  {
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.ActivitySource/*0200000D*/::get_Version() /* 0600007A */
  } // end of property ActivitySource::Version
} // end of class System.Diagnostics.ActivitySource

.class /*0200000E*/ public auto ansi sealed System.Diagnostics.ActivityTraceFlags
       extends [System.Runtime/*23000001*/]System.Enum/*01000028*/
{
  .custom /*0C000046:0A00001A*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*0400000E*/ public specialname rtspecialname int32 value__
  .field /*0400000F*/ public static literal valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ None = int32(0x00000000)
  .field /*04000010*/ public static literal valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ Recorded = int32(0x00000001)
} // end of class System.Diagnostics.ActivityTraceFlags

.class /*0200000F*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityTraceId
       extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002A*/<valuetype System.Diagnostics.ActivityTraceId/*0200000F*/>/*1B000007*/
{
  .custom /*0C00004A:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .field /*04000011*/ private initonly object _dummy
  .field /*04000012*/ private initonly int32 _dummyPrimitive
  .method /*06000085*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002B*/<uint8> destination) cil managed
  // SIG: 20 01 01 15 11 80 AD 01 05
  {
    // Method begins at RVA 0x2227
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivityTraceId::CopyTo

  .method /*06000086*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ 
          CreateFromBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<uint8> idData) cil managed
  // SIG: 00 01 11 3C 15 11 80 B1 01 05
  {
    // Method begins at RVA 0x2229
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromBytes

  .method /*06000087*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ 
          CreateFromString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<char> idData) cil managed
  // SIG: 00 01 11 3C 15 11 80 B1 01 03
  {
    // Method begins at RVA 0x222c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromString

  .method /*06000088*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ 
          CreateFromUtf8String(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<uint8> idData) cil managed
  // SIG: 00 01 11 3C 15 11 80 B1 01 05
  {
    // Method begins at RVA 0x222f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromUtf8String

  .method /*06000089*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ 
          CreateRandom() cil managed
  // SIG: 00 00 11 3C
  {
    // Method begins at RVA 0x2232
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateRandom

  .method /*0600008A*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ traceId) cil managed
  // SIG: 20 01 02 11 3C
  {
    // Method begins at RVA 0x2235
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::Equals

  .method /*0600008B*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000D4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000095*/ 
    .custom /*0C0000DA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100002D*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2238
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::Equals

  .method /*0600008C*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x223b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::GetHashCode

  .method /*0600008D*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ traceId1,
                            valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ traceId2) cil managed
  // SIG: 00 02 02 11 3C 11 3C
  {
    // Method begins at RVA 0x223e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::op_Equality

  .method /*0600008E*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ traceId1,
                              valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ traceId2) cil managed
  // SIG: 00 02 02 11 3C 11 3C
  {
    // Method begins at RVA 0x2241
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::op_Inequality

  .method /*0600008F*/ public hidebysig instance string 
          ToHexString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000D8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2244
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::ToHexString

  .method /*06000090*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000D9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2247
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::ToString

} // end of class System.Diagnostics.ActivityTraceId

.class /*02000010*/ public auto ansi sealed System.Diagnostics.ActivitySamplingResult
       extends [System.Runtime/*23000001*/]System.Enum/*01000028*/
{
  .field /*04000013*/ public specialname rtspecialname int32 value__
  .field /*04000014*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*02000010*/ None = int32(0x00000000)
  .field /*04000015*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*02000010*/ PropagationData = int32(0x00000001)
  .field /*04000016*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*02000010*/ AllData = int32(0x00000002)
  .field /*04000017*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*02000010*/ AllDataAndRecorded = int32(0x00000003)
} // end of class System.Diagnostics.ActivitySamplingResult

.class /*02000011*/ public auto ansi sealed System.Diagnostics.ActivityKind
       extends [System.Runtime/*23000001*/]System.Enum/*01000028*/
{
  .field /*04000018*/ public specialname rtspecialname int32 value__
  .field /*04000019*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000011*/ Internal = int32(0x00000000)
  .field /*0400001A*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000011*/ Server = int32(0x00000001)
  .field /*0400001B*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000011*/ Client = int32(0x00000002)
  .field /*0400001C*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000011*/ Producer = int32(0x00000003)
  .field /*0400001D*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000011*/ Consumer = int32(0x00000004)
} // end of class System.Diagnostics.ActivityKind

.class /*02000012*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityEvent
       extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
{
  .pack 0
  .size 1
  .custom /*0C000055:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000057:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .method /*06000091*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x224a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::.ctor

  .method /*06000092*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100002F*/ timestamp,
                               [opt] class System.Diagnostics.ActivityTagsCollection/*02000009*/ tags) cil managed
  // SIG: 20 03 01 0E 11 80 BD 12 24
  {
    .param [2]/*0800009C*/  = nullref
    .param [3]/*0800009D*/  = nullref
    .custom /*0C0000DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x224d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::.ctor

  .method /*06000093*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2250
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Name

  .method /*06000094*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100002F*/ 
          get_Timestamp() cil managed
  // SIG: 20 00 11 80 BD
  {
    // Method begins at RVA 0x2253
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Timestamp

  .method /*06000095*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 75 01 15 11 59 02 0E 1C
  {
    .param [0]/*0800009E*/ 
    .custom /*0C0000DD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2256
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Tags

  .property /*17000028*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.ActivityEvent/*02000012*/::get_Name() /* 06000093 */
  } // end of property ActivityEvent::Name
  .property /*17000029*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100002F*/
          Timestamp()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100002F*/ System.Diagnostics.ActivityEvent/*02000012*/::get_Timestamp() /* 06000094 */
  } // end of property ActivityEvent::Timestamp
  .property /*1700002A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          Tags()
  {
    .custom /*0C00008D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.ActivityEvent/*02000012*/::get_Tags() /* 06000095 */
  } // end of property ActivityEvent::Tags
} // end of class System.Diagnostics.ActivityEvent

.class /*02000013*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityContext
       extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002A*/<valuetype System.Diagnostics.ActivityContext/*02000013*/>/*1B000008*/
{
  .pack 0
  .size 1
  .custom /*0C00005B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00005D:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002A*/<valuetype System.Diagnostics.ActivityContext/*02000013*/>/*1B000008*/
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000096*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ traceId,
                               valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ spanId,
                               valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ traceFlags,
                               [opt] string traceState,
                               [opt] bool isRemote) cil managed
  // SIG: 20 05 01 11 3C 11 30 11 38 0E 02
  {
    .param [4]/*080000A2*/  = nullref
    .param [5]/*080000A3*/  = bool(false)
    // Method begins at RVA 0x2259
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::.ctor

  .method /*06000097*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 3C
  {
    // Method begins at RVA 0x225c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceId

  .method /*06000098*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ 
          get_SpanId() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x225f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_SpanId

  .method /*06000099*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ 
          get_TraceFlags() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x2262
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceFlags

  .method /*0600009A*/ public hidebysig specialname 
          instance string  get_TraceState() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2265
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceState

  .method /*0600009B*/ public hidebysig specialname 
          instance bool  get_IsRemote() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2268
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_IsRemote

  .method /*0600009C*/ public hidebysig static 
          bool  TryParse(string traceParent,
                         string traceState,
                         [out] valuetype System.Diagnostics.ActivityContext/*02000013*/& context) cil managed
  // SIG: 00 03 02 0E 0E 10 11 4C
  {
    // Method begins at RVA 0x226b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::TryParse

  .method /*0600009D*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityContext/*02000013*/ 
          Parse(string traceParent,
                string traceState) cil managed
  // SIG: 00 02 11 4C 0E 0E
  {
    .custom /*0C0000DB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000A8*/ 
    .custom /*0C0000E0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x226e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Parse

  .method /*0600009E*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityContext/*02000013*/ left,
                            valuetype System.Diagnostics.ActivityContext/*02000013*/ right) cil managed
  // SIG: 00 02 02 11 4C 11 4C
  {
    // Method begins at RVA 0x2271
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::op_Equality

  .method /*0600009F*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityContext/*02000013*/ left,
                              valuetype System.Diagnostics.ActivityContext/*02000013*/ right) cil managed
  // SIG: 00 02 02 11 4C 11 4C
  {
    // Method begins at RVA 0x2274
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::op_Inequality

  .method /*060000A0*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityContext/*02000013*/ 'value') cil managed
  // SIG: 20 01 02 11 4C
  {
    // Method begins at RVA 0x2277
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Equals

  .method /*060000A1*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .param [1]/*080000AE*/ 
    .custom /*0C0000E1:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100002D*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x227a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Equals

  .method /*060000A2*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x227d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::GetHashCode

  .property /*1700002B*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*0200000F*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ System.Diagnostics.ActivityContext/*02000013*/::get_TraceId() /* 06000097 */
  } // end of property ActivityContext::TraceId
  .property /*1700002C*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*0200000C*/
          SpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*0200000C*/ System.Diagnostics.ActivityContext/*02000013*/::get_SpanId() /* 06000098 */
  } // end of property ActivityContext::SpanId
  .property /*1700002D*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/
          TraceFlags()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceFlags/*0200000E*/ System.Diagnostics.ActivityContext/*02000013*/::get_TraceFlags() /* 06000099 */
  } // end of property ActivityContext::TraceFlags
  .property /*1700002E*/ instance callconv(8) string
          TraceState()
  {
    .get instance string System.Diagnostics.ActivityContext/*02000013*/::get_TraceState() /* 0600009A */
  } // end of property ActivityContext::TraceState
  .property /*1700002F*/ instance callconv(8) bool
          IsRemote()
  {
    .get instance bool System.Diagnostics.ActivityContext/*02000013*/::get_IsRemote() /* 0600009B */
  } // end of property ActivityContext::IsRemote
} // end of class System.Diagnostics.ActivityContext

.class /*02000014*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityLink
       extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002A*/<valuetype System.Diagnostics.ActivityLink/*02000014*/>/*1B000009*/
{
  .pack 0
  .size 1
  .custom /*0C000060:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .method /*060000A3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Diagnostics.ActivityContext/*02000013*/ context,
                               [opt] class System.Diagnostics.ActivityTagsCollection/*02000009*/ tags) cil managed
  // SIG: 20 02 01 11 4C 12 24
  {
    .custom /*0C0000DE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000B0*/  = nullref
    // Method begins at RVA 0x2280
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::.ctor

  .method /*060000A4*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityContext/*02000013*/ 
          get_Context() cil managed
  // SIG: 20 00 11 4C
  {
    // Method begins at RVA 0x2283
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::get_Context

  .method /*060000A5*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 75 01 15 11 59 02 0E 1C
  {
    .param [0]/*080000B1*/ 
    .custom /*0C0000E3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2286
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::get_Tags

  .method /*060000A6*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000DF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000B2*/ 
    .custom /*0C0000E4:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100002D*/::.ctor(bool) /* 0A000018 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2289
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::Equals

  .method /*060000A7*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityLink/*02000014*/ 'value') cil managed
  // SIG: 20 01 02 11 50
  {
    // Method begins at RVA 0x228c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::Equals

  .method /*060000A8*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityLink/*02000014*/ left,
                            valuetype System.Diagnostics.ActivityLink/*02000014*/ right) cil managed
  // SIG: 00 02 02 11 50 11 50
  {
    // Method begins at RVA 0x228f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::op_Equality

  .method /*060000A9*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityLink/*02000014*/ left,
                              valuetype System.Diagnostics.ActivityLink/*02000014*/ right) cil managed
  // SIG: 00 02 02 11 50 11 50
  {
    // Method begins at RVA 0x2292
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::op_Inequality

  .method /*060000AA*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2295
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::GetHashCode

  .property /*17000030*/ instance callconv(8) valuetype System.Diagnostics.ActivityContext/*02000013*/
          Context()
  {
    .get instance valuetype System.Diagnostics.ActivityContext/*02000013*/ System.Diagnostics.ActivityLink/*02000014*/::get_Context() /* 060000A4 */
  } // end of property ActivityLink::Context
  .property /*17000031*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          Tags()
  {
    .custom /*0C000092:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.ActivityLink/*02000014*/::get_Tags() /* 060000A5 */
  } // end of property ActivityLink::Tags
} // end of class System.Diagnostics.ActivityLink

.class /*02000015*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityCreationOptions`1<T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
{
  .pack 0
  .size 1
  .custom /*0C000064:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000065:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000066:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .param type T /*2A000001*/ 
    .custom /*0C000015:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000AB*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivitySource/*0200000D*/ 
          get_Source() cil managed
  // SIG: 20 00 12 34
  {
    // Method begins at RVA 0x2298
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Source

  .method /*060000AC*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x229b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Name

  .method /*060000AD*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityKind/*02000011*/ 
          get_Kind() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x229e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Kind

  .method /*060000AE*/ public hidebysig specialname 
          instance !T  get_Parent() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x22a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Parent

  .method /*060000AF*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 75 01 15 11 59 02 0E 1C
  {
    .param [0]/*080000B8*/ 
    .custom /*0C0000E5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x22a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Tags

  .method /*060000B0*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> 
          get_Links() cil managed
  // SIG: 20 00 15 12 75 01 11 50
  {
    .custom /*0C0000E2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Links

  .method /*060000B1*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivityTagsCollection/*02000009*/ 
          get_SamplingTags() cil managed
  // SIG: 20 00 12 24
  {
    // Method begins at RVA 0x22aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_SamplingTags

  .method /*060000B2*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 3C
  {
    // Method begins at RVA 0x22ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_TraceId

  .property /*17000032*/ instance callconv(8) class System.Diagnostics.ActivitySource/*0200000D*/
          Source()
  {
    .get instance class System.Diagnostics.ActivitySource/*0200000D*/ System.Diagnostics.ActivityCreationOptions`1/*02000015*/::get_Source() /* 060000AB */
  } // end of property ActivityCreationOptions`1::Source
  .property /*17000033*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.ActivityCreationOptions`1/*02000015*/::get_Name() /* 060000AC */
  } // end of property ActivityCreationOptions`1::Name
  .property /*17000034*/ instance callconv(8) valuetype System.Diagnostics.ActivityKind/*02000011*/
          Kind()
  {
    .get instance valuetype System.Diagnostics.ActivityKind/*02000011*/ System.Diagnostics.ActivityCreationOptions`1/*02000015*/::get_Kind() /* 060000AD */
  } // end of property ActivityCreationOptions`1::Kind
  .property /*17000035*/ instance callconv(8) !T
          Parent()
  {
    .get instance !T System.Diagnostics.ActivityCreationOptions`1/*02000015*/::get_Parent() /* 060000AE */
  } // end of property ActivityCreationOptions`1::Parent
  .property /*17000036*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          Tags()
  {
    .custom /*0C000098:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.ActivityCreationOptions`1/*02000015*/::get_Tags() /* 060000AF */
  } // end of property ActivityCreationOptions`1::Tags
  .property /*17000037*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/>
          Links()
  {
    .custom /*0C000099:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.ActivityLink/*02000014*/> System.Diagnostics.ActivityCreationOptions`1/*02000015*/::get_Links() /* 060000B0 */
  } // end of property ActivityCreationOptions`1::Links
  .property /*17000038*/ instance callconv(8) class System.Diagnostics.ActivityTagsCollection/*02000009*/
          SamplingTags()
  {
    .get instance class System.Diagnostics.ActivityTagsCollection/*02000009*/ System.Diagnostics.ActivityCreationOptions`1/*02000015*/::get_SamplingTags() /* 060000B1 */
  } // end of property ActivityCreationOptions`1::SamplingTags
  .property /*17000039*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*0200000F*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*0200000F*/ System.Diagnostics.ActivityCreationOptions`1/*02000015*/::get_TraceId() /* 060000B2 */
  } // end of property ActivityCreationOptions`1::TraceId
} // end of class System.Diagnostics.ActivityCreationOptions`1

.class /*02000016*/ public auto ansi sealed System.Diagnostics.SampleActivity`1<T>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000031*/
{
  .param type T /*2A000002*/ 
    .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000B3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method SampleActivity`1::.ctor

  .method /*060000B4*/ public hidebysig newslot virtual 
          instance valuetype System.Diagnostics.ActivitySamplingResult/*02000010*/ 
          Invoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000015*/<!T>& options) runtime managed
  // SIG: 20 01 11 40 10 15 11 54 01 13 00
  {
    .param [1]/*080000BB*/ 
    .custom /*0C0000E6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::Invoke

  .method /*060000B5*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000032*/ 
          BeginInvoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000015*/<!T>& options,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000033*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 80 C9 10 15 11 54 01 13 00 12 80 CD 1C
  {
    .param [1]/*080000BC*/ 
    .custom /*0C0000E7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::BeginInvoke

  .method /*060000B6*/ public hidebysig newslot virtual 
          instance valuetype System.Diagnostics.ActivitySamplingResult/*02000010*/ 
          EndInvoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000015*/<!T>& options,
                    class [System.Runtime/*23000001*/]System.IAsyncResult/*01000032*/ result) runtime managed
  // SIG: 20 02 11 40 10 15 11 54 01 13 00 12 80 C9
  {
    .param [1]/*080000BF*/ 
    .custom /*0C0000E8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::EndInvoke

} // end of class System.Diagnostics.SampleActivity`1

.class /*02000017*/ public auto ansi sealed beforefieldinit System.Diagnostics.ActivityListener
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000B7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22b0
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityListener::.ctor

  .method /*060000B8*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/> 
          get_ActivityStarted() cil managed
  // SIG: 20 00 15 12 80 D1 01 12 20
  {
    .param [0]/*080000C1*/ 
    .custom /*0C0000E9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ActivityStarted

  .method /*060000B9*/ public hidebysig specialname 
          instance void  set_ActivityStarted(class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 D1 01 12 20
  {
    .param [1]/*080000C2*/ 
    .custom /*0C0000EA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ActivityStarted

  .method /*060000BA*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/> 
          get_ActivityStopped() cil managed
  // SIG: 20 00 15 12 80 D1 01 12 20
  {
    .param [0]/*080000C3*/ 
    .custom /*0C0000EB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ActivityStopped

  .method /*060000BB*/ public hidebysig specialname 
          instance void  set_ActivityStopped(class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 D1 01 12 20
  {
    .param [1]/*080000C4*/ 
    .custom /*0C0000ED:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ActivityStopped

  .method /*060000BC*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*01000035*/<class System.Diagnostics.ActivitySource/*0200000D*/,bool> 
          get_ShouldListenTo() cil managed
  // SIG: 20 00 15 12 80 D5 02 12 34 02
  {
    .param [0]/*080000C5*/ 
    .custom /*0C0000EF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ShouldListenTo

  .method /*060000BD*/ public hidebysig specialname 
          instance void  set_ShouldListenTo(class [System.Runtime/*23000001*/]System.Func`2/*01000035*/<class System.Diagnostics.ActivitySource/*0200000D*/,bool> 'value') cil managed
  // SIG: 20 01 01 15 12 80 D5 02 12 34 02
  {
    .param [1]/*080000C6*/ 
    .custom /*0C0000F1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ShouldListenTo

  .method /*060000BE*/ public hidebysig specialname 
          instance class System.Diagnostics.SampleActivity`1/*02000016*/<string> 
          get_SampleUsingParentId() cil managed
  // SIG: 20 00 15 12 58 01 0E
  {
    .param [0]/*080000C7*/ 
    .custom /*0C0000F3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_SampleUsingParentId

  .method /*060000BF*/ public hidebysig specialname 
          instance void  set_SampleUsingParentId(class System.Diagnostics.SampleActivity`1/*02000016*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 58 01 0E
  {
    .param [1]/*080000C8*/ 
    .custom /*0C0000F5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_SampleUsingParentId

  .method /*060000C0*/ public hidebysig specialname 
          instance class System.Diagnostics.SampleActivity`1/*02000016*/<valuetype System.Diagnostics.ActivityContext/*02000013*/> 
          get_Sample() cil managed
  // SIG: 20 00 15 12 58 01 11 4C
  {
    // Method begins at RVA 0x22d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_Sample

  .method /*060000C1*/ public hidebysig specialname 
          instance void  set_Sample(class System.Diagnostics.SampleActivity`1/*02000016*/<valuetype System.Diagnostics.ActivityContext/*02000013*/> 'value') cil managed
  // SIG: 20 01 01 15 12 58 01 11 4C
  {
    // Method begins at RVA 0x22d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_Sample

  .method /*060000C2*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::Dispose

  .property /*1700003A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/>
          ActivityStarted()
  {
    .custom /*0C00009D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/> System.Diagnostics.ActivityListener/*02000017*/::get_ActivityStarted() /* 060000B8 */
    .set instance void System.Diagnostics.ActivityListener/*02000017*/::set_ActivityStarted(class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/>) /* 060000B9 */
  } // end of property ActivityListener::ActivityStarted
  .property /*1700003B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/>
          ActivityStopped()
  {
    .custom /*0C00009F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/> System.Diagnostics.ActivityListener/*02000017*/::get_ActivityStopped() /* 060000BA */
    .set instance void System.Diagnostics.ActivityListener/*02000017*/::set_ActivityStopped(class [System.Runtime/*23000001*/]System.Action`1/*01000034*/<class System.Diagnostics.Activity/*02000008*/>) /* 060000BB */
  } // end of property ActivityListener::ActivityStopped
  .property /*1700003C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*01000035*/<class System.Diagnostics.ActivitySource/*0200000D*/,bool>
          ShouldListenTo()
  {
    .custom /*0C0000A0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*01000035*/<class System.Diagnostics.ActivitySource/*0200000D*/,bool> System.Diagnostics.ActivityListener/*02000017*/::get_ShouldListenTo() /* 060000BC */
    .set instance void System.Diagnostics.ActivityListener/*02000017*/::set_ShouldListenTo(class [System.Runtime/*23000001*/]System.Func`2/*01000035*/<class System.Diagnostics.ActivitySource/*0200000D*/,bool>) /* 060000BD */
  } // end of property ActivityListener::ShouldListenTo
  .property /*1700003D*/ instance callconv(8) class System.Diagnostics.SampleActivity`1/*02000016*/<string>
          SampleUsingParentId()
  {
    .custom /*0C0000A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class System.Diagnostics.SampleActivity`1/*02000016*/<string> System.Diagnostics.ActivityListener/*02000017*/::get_SampleUsingParentId() /* 060000BE */
    .set instance void System.Diagnostics.ActivityListener/*02000017*/::set_SampleUsingParentId(class System.Diagnostics.SampleActivity`1/*02000016*/<string>) /* 060000BF */
  } // end of property ActivityListener::SampleUsingParentId
  .property /*1700003E*/ instance callconv(8) class System.Diagnostics.SampleActivity`1/*02000016*/<valuetype System.Diagnostics.ActivityContext/*02000013*/>
          Sample()
  {
    .get instance class System.Diagnostics.SampleActivity`1/*02000016*/<valuetype System.Diagnostics.ActivityContext/*02000013*/> System.Diagnostics.ActivityListener/*02000017*/::get_Sample() /* 060000C0 */
    .set instance void System.Diagnostics.ActivityListener/*02000017*/::set_Sample(class System.Diagnostics.SampleActivity`1/*02000016*/<valuetype System.Diagnostics.ActivityContext/*02000013*/>) /* 060000C1 */
  } // end of property ActivityListener::Sample
} // end of class System.Diagnostics.ActivityListener

.class /*02000018*/ public abstract auto ansi beforefieldinit System.Diagnostics.DistributedContextPropagator
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000026*/ auto ansi sealed nested public PropagatorGetterCallback
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000031*/
  {
    .custom /*0C000089:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .method /*0600012B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method PropagatorGetterCallback::.ctor

    .method /*0600012C*/ public hidebysig newslot virtual 
            instance void  Invoke(object carrier,
                                  string fieldName,
                                  [out] string& fieldValue,
                                  [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>& fieldValues) runtime managed
    // SIG: 20 04 01 1C 0E 10 0E 10 15 12 75 01 0E
    {
      .custom /*0C00013B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
      .param [2]/*0800017F*/ 
      .custom /*0C000168:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .param [4]/*08000181*/ 
      .custom /*0C000169:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::Invoke

    .method /*0600012D*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000032*/ 
            BeginInvoke(object carrier,
                        string fieldName,
                        [out] string& fieldValue,
                        [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>& fieldValues,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*01000033*/ callback,
                        object 'object') runtime managed
    // SIG: 20 06 12 80 C9 1C 0E 10 0E 10 15 12 75 01 0E 12 80 CD 1C
    {
      .param [1]/*08000182*/ 
      .custom /*0C00016A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .param [2]/*08000183*/ 
      .custom /*0C00016B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .param [3]/*08000184*/ 
      .custom /*0C00016C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .param [4]/*08000185*/ 
      .custom /*0C00016D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::BeginInvoke

    .method /*0600012E*/ public hidebysig newslot virtual 
            instance void  EndInvoke([out] string& fieldValue,
                                     [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<string>& fieldValues,
                                     class [System.Runtime/*23000001*/]System.IAsyncResult/*01000032*/ result) runtime managed
    // SIG: 20 03 01 10 0E 10 15 12 75 01 0E 12 80 C9
    {
      .param [1]/*08000188*/ 
      .custom /*0C00016E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .param [2]/*08000189*/ 
      .custom /*0C00016F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::EndInvoke

  } // end of class PropagatorGetterCallback

  .class /*02000027*/ auto ansi sealed nested public PropagatorSetterCallback
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000031*/
  {
    .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .method /*0600012F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method PropagatorSetterCallback::.ctor

    .method /*06000130*/ public hidebysig newslot virtual 
            instance void  Invoke(object carrier,
                                  string fieldName,
                                  string fieldValue) runtime managed
    // SIG: 20 03 01 1C 0E 0E
    {
      .custom /*0C00013D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
      .param [1]/*0800018D*/ 
      .custom /*0C000170:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    } // end of method PropagatorSetterCallback::Invoke

    .method /*06000131*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000032*/ 
            BeginInvoke(object carrier,
                        string fieldName,
                        string fieldValue,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*01000033*/ callback,
                        object 'object') runtime managed
    // SIG: 20 05 12 80 C9 1C 0E 0E 12 80 CD 1C
    {
      .param [1]/*08000190*/ 
      .custom /*0C000171:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .param [2]/*08000191*/ 
      .custom /*0C000172:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .param [3]/*08000192*/ 
      .custom /*0C000173:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    } // end of method PropagatorSetterCallback::BeginInvoke

    .method /*06000132*/ public hidebysig newslot virtual 
            instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000032*/ result) runtime managed
    // SIG: 20 01 01 12 80 C9
    {
    } // end of method PropagatorSetterCallback::EndInvoke

  } // end of class PropagatorSetterCallback

  .field /*0400001E*/ private static class System.Diagnostics.DistributedContextPropagator/*02000018*/ '<Current>k__BackingField'
  .custom /*0C00007A:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*060000C3*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000036*/<string> 
          get_Fields() cil managed
  // SIG: 20 00 15 12 80 D9 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::get_Fields

  .method /*060000C4*/ public hidebysig newslot abstract virtual 
          instance void  Inject(class System.Diagnostics.Activity/*02000008*/ activity,
                                object carrier,
                                class System.Diagnostics.DistributedContextPropagator/*02000018*//PropagatorSetterCallback/*02000027*/ setter) cil managed
  // SIG: 20 03 01 12 20 1C 12 80 9C
  {
    .custom /*0C0000EC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::Inject

  .method /*060000C5*/ public hidebysig newslot abstract virtual 
          instance void  ExtractTraceIdAndState(object carrier,
                                                class System.Diagnostics.DistributedContextPropagator/*02000018*//PropagatorGetterCallback/*02000026*/ getter,
                                                [out] string& traceId,
                                                [out] string& traceState) cil managed
  // SIG: 20 04 01 1C 12 80 98 10 0E 10 0E
  {
    .custom /*0C0000EE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::ExtractTraceIdAndState

  .method /*060000C6*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> 
          ExtractBaggage(object carrier,
                         class System.Diagnostics.DistributedContextPropagator/*02000018*//PropagatorGetterCallback/*02000026*/ getter) cil managed
  // SIG: 20 02 15 12 75 01 15 11 59 02 0E 0E 1C 12 80 98
  {
    .custom /*0C0000F0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000D1*/ 
    .custom /*0C0000F9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::ExtractBaggage

  .method /*060000C7*/ public hidebysig specialname static 
          class System.Diagnostics.DistributedContextPropagator/*02000018*/ 
          get_Current() cil managed
  // SIG: 00 00 12 60
  {
    .custom /*0C0000F2:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22da
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 7E   | (04)00001E       */ ldsfld     class System.Diagnostics.DistributedContextPropagator/*02000018*/ System.Diagnostics.DistributedContextPropagator/*02000018*/::'<Current>k__BackingField' /* 0400001E */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::get_Current

  .method /*060000C8*/ public hidebysig specialname static 
          void  set_Current(class System.Diagnostics.DistributedContextPropagator/*02000018*/ 'value') cil managed
  // SIG: 00 01 01 12 60
  {
    .custom /*0C0000F4:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22e1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 80   | (04)00001E       */ stsfld     class System.Diagnostics.DistributedContextPropagator/*02000018*/ System.Diagnostics.DistributedContextPropagator/*02000018*/::'<Current>k__BackingField' /* 0400001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::set_Current

  .method /*060000C9*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*02000018*/ 
          CreateDefaultPropagator() cil managed
  // SIG: 00 00 12 60
  {
    // Method begins at RVA 0x22e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreateDefaultPropagator

  .method /*060000CA*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*02000018*/ 
          CreatePassThroughPropagator() cil managed
  // SIG: 00 00 12 60
  {
    // Method begins at RVA 0x22ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreatePassThroughPropagator

  .method /*060000CB*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*02000018*/ 
          CreateNoOutputPropagator() cil managed
  // SIG: 00 00 12 60
  {
    // Method begins at RVA 0x22ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreateNoOutputPropagator

  .method /*060000CC*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22f2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::.ctor

  .property /*1700003F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000036*/<string>
          Fields()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000036*/<string> System.Diagnostics.DistributedContextPropagator/*02000018*/::get_Fields() /* 060000C3 */
  } // end of property DistributedContextPropagator::Fields
  .property /*17000040*/ callconv(8) class System.Diagnostics.DistributedContextPropagator/*02000018*/
          Current()
  {
    .get class System.Diagnostics.DistributedContextPropagator/*02000018*/ System.Diagnostics.DistributedContextPropagator/*02000018*/::get_Current() /* 060000C7 */
    .set void System.Diagnostics.DistributedContextPropagator/*02000018*/::set_Current(class System.Diagnostics.DistributedContextPropagator/*02000018*/) /* 060000C8 */
  } // end of property DistributedContextPropagator::Current
} // end of class System.Diagnostics.DistributedContextPropagator

.class /*02000019*/ public sequential ansi sealed beforefieldinit System.Diagnostics.TagList
       extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000A*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000004*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000002*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000038*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000B*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000C*/
{
  .custom /*0C000070:0A000012*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000021*/::.ctor(string) /* 0A000012 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000A*/
  .custom /*0C000048:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000026*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000004*/
  .custom /*0C00004B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000002*/
  .custom /*0C00004E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000038*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000B*/
  .custom /*0C000059:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000C*/
  .custom /*0C00005F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .class /*02000028*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000005*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000024*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  {
    .pack 0
    .size 1
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000005*/
    .custom /*0C000071:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .method /*06000133*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> 
            get_Current() cil managed
    // SIG: 20 00 15 11 59 02 0E 1C
    {
      .param [0]/*08000196*/ 
      .custom /*0C000174:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      // Method begins at RVA 0x2468
      // Code size       2 (0x2)
      .maxstack  1
      .locals /*11000001*/ init (valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> V_0)
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*06000134*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000024*/::get_Current /*01000024::0A000015*/ 
      // Method begins at RVA 0x2476
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*06000135*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2479
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Dispose

    .method /*06000136*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x247c
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*06000137*/ public hidebysig newslot virtual final 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x247f
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Reset

    .property /*17000053*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>
            Current()
    {
      .custom /*0C0000B7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> System.Diagnostics.TagList/*02000019*//Enumerator/*02000028*/::get_Current() /* 06000133 */
    } // end of property Enumerator::Current
    .property /*17000054*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .custom /*0C0000B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .get instance object System.Diagnostics.TagList/*02000019*//Enumerator/*02000028*/::System.Collections.IEnumerator.get_Current() /* 06000134 */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .method /*060000CD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tagList) cil managed
  // SIG: 20 01 01 15 11 80 B1 01 15 11 59 02 0E 1C
  {
    .param [1]/*080000D5*/ 
    .custom /*0C0000FC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x22fa
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* FE15 | (02)000019       */ initobj    System.Diagnostics.TagList/*02000019*/
    IL_0007:  /* 14   |                  */ ldnull
    IL_0008:  /* 7A   |                  */ throw
  } // end of method TagList::.ctor

  .method /*060000CE*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000F6:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2304
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_Count

  .method /*060000CF*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000F7:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2307
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_IsReadOnly

  .method /*060000D0*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> 
          get_Item(int32 index) cil managed
  // SIG: 20 01 15 11 59 02 0E 1C 08
  {
    .custom /*0C0000F8:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .param [0]/*080000D6*/ 
    .custom /*0C0000FD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x230a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_Item

  .method /*060000D1*/ public hidebysig newslot specialname virtual final 
          instance void  set_Item(int32 index,
                                  valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> 'value') cil managed
  // SIG: 20 02 01 08 15 11 59 02 0E 1C
  {
    .param [2]/*080000D9*/ 
    .custom /*0C000100:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x230d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::set_Item

  .method /*060000D2*/ public hidebysig instance void 
          Add(string key,
              object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C0000FA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000DB*/ 
    .custom /*0C000102:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2310
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Add

  .method /*060000D3*/ public hidebysig newslot virtual final 
          instance void  Add(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 01 01 15 11 59 02 0E 1C
  {
    .param [1]/*080000DC*/ 
    .custom /*0C000104:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2313
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Add

  .method /*060000D4*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002B*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 01 01 15 11 80 AD 01 15 11 59 02 0E 1C
  {
    .custom /*0C0000FB:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .param [1]/*080000DD*/ 
    .custom /*0C000106:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2316
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::CopyTo

  .method /*060000D5*/ public hidebysig newslot virtual final 
          instance void  Insert(int32 index,
                                valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 02 01 08 15 11 59 02 0E 1C
  {
    .param [2]/*080000DF*/ 
    .custom /*0C000107:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2319
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Insert

  .method /*060000D6*/ public hidebysig newslot virtual final 
          instance void  RemoveAt(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x231c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::RemoveAt

  .method /*060000D7*/ public hidebysig newslot virtual final 
          instance void  Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x231f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Clear

  .method /*060000D8*/ public hidebysig newslot virtual final 
          instance bool  Contains(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 59 02 0E 1C
  {
    .custom /*0C0000FE:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .param [1]/*080000E1*/ 
    .custom /*0C000108:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2322
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Contains

  .method /*060000D9*/ public hidebysig newslot virtual final 
          instance void  CopyTo(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] 'array',
                                int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 59 02 0E 1C 08
  {
    .custom /*0C0000FF:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .param [1]/*080000E2*/ 
    .custom /*0C000109:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2325
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::CopyTo

  .method /*060000DA*/ public hidebysig newslot virtual final 
          instance bool  Remove(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 59 02 0E 1C
  {
    .param [1]/*080000E4*/ 
    .custom /*0C00010A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2328
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Remove

  .method /*060000DB*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 59 02 0E 1C
  {
    .custom /*0C000101:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .param [0]/*080000E5*/ 
    .custom /*0C00010B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x232b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::GetEnumerator

  .method /*060000DC*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000024*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 91
  {
    .custom /*0C000103:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/::GetEnumerator /*01000023::0A000014*/ 
    // Method begins at RVA 0x232e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::System.Collections.IEnumerable.GetEnumerator

  .method /*060000DD*/ public hidebysig newslot virtual final 
          instance int32  IndexOf(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 08 15 11 59 02 0E 1C
  {
    .custom /*0C000105:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .param [1]/*080000E6*/ 
    .custom /*0C00010C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2331
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::IndexOf

  .property /*17000041*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Diagnostics.TagList/*02000019*/::get_Count() /* 060000CE */
  } // end of property TagList::Count
  .property /*17000042*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Diagnostics.TagList/*02000019*/::get_IsReadOnly() /* 060000CF */
  } // end of property TagList::IsReadOnly
  .property /*17000043*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>
          Item(int32)
  {
    .custom /*0C0000A4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> System.Diagnostics.TagList/*02000019*/::get_Item(int32) /* 060000D0 */
    .set instance void System.Diagnostics.TagList/*02000019*/::set_Item(int32,
                                                                        valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>) /* 060000D1 */
  } // end of property TagList::Item
} // end of class System.Diagnostics.TagList

.class /*0200001A*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.Counter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*0200001D*/<!T>/*1B00000D*/
{
  .method /*060000DE*/ public hidebysig instance void 
          Add(!T delta) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2334
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000DF*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 59 02 0E 1C
  {
    .param [2]/*080000E9*/ 
    .custom /*0C00010D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2337
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000E0*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*080000EB*/ 
    .custom /*0C00010E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*080000EC*/ 
    .custom /*0C00010F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x233a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000E1*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*080000EE*/ 
    .custom /*0C000111:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*080000EF*/ 
    .custom /*0C000112:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*080000F0*/ 
    .custom /*0C000113:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x233d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000E2*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 B1 01 15 11 59 02 0E 1C
  {
    .param [2]/*080000F2*/ 
    .custom /*0C000114:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2340
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000E3*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 59 02 0E 1C
  {
    .param [2]/*080000F4*/ 
    .custom /*0C000115:0A00001B*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000039*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .custom /*0C000116:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2343
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000E4*/ public hidebysig instance void 
          Add(!T delta,
              [in] valuetype System.Diagnostics.TagList/*02000019*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 64
  {
    .param [2]/*080000F6*/ 
    .custom /*0C000119:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2346
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000E5*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000020*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 80 0E 0E 0E
  {
    // Method begins at RVA 0x2349
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)00001E       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001D*/<!T>/*1B00000D*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000020*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string) /* 0A00001E */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Counter`1::.ctor

} // end of class System.Diagnostics.Metrics.Counter`1

.class /*0200001B*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.Histogram`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*0200001D*/<!T>/*1B00000D*/
{
  .method /*060000E6*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000020*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 80 0E 0E 0E
  {
    // Method begins at RVA 0x2357
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)00001E       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001D*/<!T>/*1B00000D*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000020*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string) /* 0A00001E */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Histogram`1::.ctor

  .method /*060000E7*/ public hidebysig instance void 
          Record(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2365
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*060000E8*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 59 02 0E 1C
  {
    .param [2]/*08000101*/ 
    .custom /*0C00011A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2368
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*060000E9*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*08000103*/ 
    .custom /*0C00011B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000104*/ 
    .custom /*0C00011D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x236b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*060000EA*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*08000106*/ 
    .custom /*0C00011E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000107*/ 
    .custom /*0C00011F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000108*/ 
    .custom /*0C000120:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x236e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*060000EB*/ public hidebysig instance void 
          Record(!T 'value',
                 [in] valuetype System.Diagnostics.TagList/*02000019*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 64
  {
    .param [2]/*0800010A*/ 
    .custom /*0C000121:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*060000EC*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 B1 01 15 11 59 02 0E 1C
  {
    .param [2]/*0800010C*/ 
    .custom /*0C000122:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2374
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*060000ED*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 59 02 0E 1C
  {
    .param [2]/*0800010E*/ 
    .custom /*0C000123:0A00001B*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000039*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .custom /*0C000124:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

} // end of class System.Diagnostics.Metrics.Histogram`1

.class /*0200001C*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.Instrument
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000077:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000EE*/ public hidebysig specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000110:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Description

  .method /*060000EF*/ public hidebysig specialname 
          instance bool  get_Enabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x237d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Enabled

  .method /*060000F0*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000020*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 80 0E 0E 0E
  {
    .param [3]/*08000111*/ 
    .custom /*0C000127:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000112*/ 
    .custom /*0C000129:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2380
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Instrument::.ctor

  .method /*060000F1*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsObservable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2389
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_IsObservable

  .method /*060000F2*/ public hidebysig specialname 
          instance class System.Diagnostics.Metrics.Meter/*02000020*/ 
          get_Meter() cil managed
  // SIG: 20 00 12 80 80
  {
    // Method begins at RVA 0x238c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Meter

  .method /*060000F3*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x238f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Name

  .method /*060000F4*/ family hidebysig instance void 
          Publish() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2392
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::Publish

  .method /*060000F5*/ public hidebysig specialname 
          instance string  get_Unit() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000117:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2395
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Unit

  .property /*17000044*/ instance callconv(8) string
          Description()
  {
    .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001C*/::get_Description() /* 060000EE */
  } // end of property Instrument::Description
  .property /*17000045*/ instance callconv(8) bool
          Enabled()
  {
    .get instance bool System.Diagnostics.Metrics.Instrument/*0200001C*/::get_Enabled() /* 060000EF */
  } // end of property Instrument::Enabled
  .property /*17000046*/ instance callconv(8) bool
          IsObservable()
  {
    .get instance bool System.Diagnostics.Metrics.Instrument/*0200001C*/::get_IsObservable() /* 060000F1 */
  } // end of property Instrument::IsObservable
  .property /*17000047*/ instance callconv(8) class System.Diagnostics.Metrics.Meter/*02000020*/
          Meter()
  {
    .get instance class System.Diagnostics.Metrics.Meter/*02000020*/ System.Diagnostics.Metrics.Instrument/*0200001C*/::get_Meter() /* 060000F2 */
  } // end of property Instrument::Meter
  .property /*17000048*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001C*/::get_Name() /* 060000F3 */
  } // end of property Instrument::Name
  .property /*17000049*/ instance callconv(8) string
          Unit()
  {
    .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001C*/::get_Unit() /* 060000F5 */
  } // end of property Instrument::Unit
} // end of class System.Diagnostics.Metrics.Instrument

.class /*0200001D*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.Instrument`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>
       extends System.Diagnostics.Metrics.Instrument/*0200001C*/
{
  .method /*060000F6*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000020*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 80 0E 0E 0E
  {
    .custom /*0C000118:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000115*/ 
    .custom /*0C00012B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000116*/ 
    .custom /*0C00012D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2398
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (06)0000F0       */ call       instance void System.Diagnostics.Metrics.Instrument/*0200001C*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000020*/,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              string) /* 060000F0 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Instrument`1::.ctor

  .method /*060000F7*/ family hidebysig instance void 
          RecordMeasurement(!T measurement) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x23a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*060000F8*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 59 02 0E 1C
  {
    .param [2]/*08000119*/ 
    .custom /*0C00012E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*060000F9*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*0800011B*/ 
    .custom /*0C00012F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*0800011C*/ 
    .custom /*0C000130:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*060000FA*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*0800011E*/ 
    .custom /*0C000131:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*0800011F*/ 
    .custom /*0C000132:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000120*/ 
    .custom /*0C000133:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*060000FB*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            [in] valuetype System.Diagnostics.TagList/*02000019*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 64
  {
    .param [2]/*08000122*/ 
    .custom /*0C000134:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*060000FC*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 B1 01 15 11 59 02 0E 1C
  {
    .param [2]/*08000124*/ 
    .custom /*0C000136:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

} // end of class System.Diagnostics.Metrics.Instrument`1

.class /*0200001E*/ public sequential ansi sealed beforefieldinit System.Diagnostics.Metrics.Measurement`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000027*/
{
  .pack 0
  .size 1
  .custom /*0C00007B:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000029*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
  .method /*060000FD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x23b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*060000FE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 12 75 01 15 11 59 02 0E 1C
  {
    .param [2]/*08000127*/ 
    .custom /*0C000137:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x23bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*060000FF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 59 02 0E 1C
  {
    .param [2]/*08000129*/ 
    .custom /*0C000138:0A00001B*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000039*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .custom /*0C000139:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x23be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000100*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 B1 01 15 11 59 02 0E 1C
  {
    .param [2]/*0800012B*/ 
    .custom /*0C00013A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000101*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 11 80 B1 01 15 11 59 02 0E 1C
  {
    .param [0]/*0800012C*/ 
    .custom /*0C00013C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::get_Tags

  .method /*06000102*/ public hidebysig specialname 
          instance !T  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::get_Value

  .property /*1700004A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          Tags()
  {
    .custom /*0C0000AB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.Metrics.Measurement`1/*0200001E*/::get_Tags() /* 06000101 */
  } // end of property Measurement`1::Tags
  .property /*1700004B*/ instance callconv(8) !T
          Value()
  {
    .get instance !T System.Diagnostics.Metrics.Measurement`1/*0200001E*/::get_Value() /* 06000102 */
  } // end of property Measurement`1::Value
} // end of class System.Diagnostics.Metrics.Measurement`1

.class /*0200001F*/ public auto ansi sealed System.Diagnostics.Metrics.MeasurementCallback`1<T>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000031*/
{
  .param type T /*2A000007*/ 
    .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000103*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method MeasurementCallback`1::.ctor

  .method /*06000104*/ public hidebysig newslot virtual 
          instance void  Invoke(class System.Diagnostics.Metrics.Instrument/*0200001C*/ instrument,
                                !T measurement,
                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                                object state) runtime managed
  // SIG: 20 04 01 12 70 13 00 15 11 80 B1 01 15 11 59 02 0E 1C 1C
  {
    .custom /*0C00011C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000131*/ 
    .custom /*0C00013E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000132*/ 
    .custom /*0C00013F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method MeasurementCallback`1::Invoke

  .method /*06000105*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000032*/ 
          BeginInvoke(class System.Diagnostics.Metrics.Instrument/*0200001C*/ instrument,
                      !T measurement,
                      valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                      object state,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000033*/ callback,
                      object 'object') runtime managed
  // SIG: 20 06 12 80 C9 12 70 13 00 15 11 80 B1 01 15 11 59 02 0E 1C 1C 12 80 CD 1C
  {
    .param [1]/*08000133*/ 
    .custom /*0C000140:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000134*/ 
    .custom /*0C000141:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000135*/ 
    .custom /*0C000142:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000136*/ 
    .custom /*0C000143:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method MeasurementCallback`1::BeginInvoke

  .method /*06000106*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000032*/ result) runtime managed
  // SIG: 20 01 01 12 80 C9
  {
  } // end of method MeasurementCallback`1::EndInvoke

} // end of class System.Diagnostics.Metrics.MeasurementCallback`1

.class /*02000020*/ public auto ansi beforefieldinit System.Diagnostics.Metrics.Meter
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C00007F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000080:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000107*/ public hidebysig instance class System.Diagnostics.Metrics.Counter`1/*0200001A*/<!!T> 
          CreateCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(string name,
                                                                                                      [opt] string unit,
                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 03 15 12 68 01 1E 00 0E 0E 0E
  {
    .param type T /*2A00000B*/ 
      .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800013A*/ 
    .custom /*0C000144:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800013B*/ 
    .custom /*0C000145:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800013C*/  = nullref
    .param [3]/*0800013D*/  = nullref
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateCounter

  .method /*06000108*/ public hidebysig instance class System.Diagnostics.Metrics.Histogram`1/*0200001B*/<!!T> 
          CreateHistogram<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(string name,
                                                                                                        [opt] string unit,
                                                                                                        [opt] string description) cil managed
  // SIG: 30 01 03 15 12 6C 01 1E 00 0E 0E 0E
  {
    .param type T /*2A00000C*/ 
      .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800013E*/ 
    .custom /*0C000146:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800013F*/ 
    .custom /*0C000147:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000140*/  = nullref
    .param [3]/*08000141*/  = nullref
    // Method begins at RVA 0x23cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateHistogram

  .method /*06000109*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000022*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<!!T> observeValue,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 88 01 1E 00 0E 15 12 80 81 01 1E 00 0E 0E
  {
    .param type T /*2A00000D*/ 
      .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000142*/ 
    .custom /*0C000148:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000143*/ 
    .custom /*0C000149:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000144*/ 
    .custom /*0C00014A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [3]/*08000145*/  = nullref
    .param [4]/*08000146*/  = nullref
    // Method begins at RVA 0x23d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*0600010A*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000022*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<valuetype System.Diagnostics.Metrics.Measurement`1/*0200001E*/<!!T>> observeValue,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 88 01 1E 00 0E 15 12 80 81 01 15 11 78 01 1E 00 0E 0E
  {
    .param type T /*2A00000E*/ 
      .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000147*/ 
    .custom /*0C00014B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000148*/ 
    .custom /*0C00014C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000149*/ 
    .custom /*0C00014D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [3]/*0800014A*/  = nullref
    .param [4]/*0800014B*/  = nullref
    // Method begins at RVA 0x23d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*0600010B*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000022*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.Metrics.Measurement`1/*0200001E*/<!!T>>> observeValues,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 88 01 1E 00 0E 15 12 80 81 01 15 12 75 01 15 11 78 01 1E 00 0E 0E
  {
    .param type T /*2A00000F*/ 
      .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800014C*/ 
    .custom /*0C00014E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800014D*/ 
    .custom /*0C00014F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800014E*/ 
    .custom /*0C000150:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [3]/*0800014F*/  = nullref
    .param [4]/*08000150*/  = nullref
    // Method begins at RVA 0x23d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*0600010C*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000023*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<!!T> observeValue,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 8C 01 1E 00 0E 15 12 80 81 01 1E 00 0E 0E
  {
    .param type T /*2A000010*/ 
      .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000151*/ 
    .custom /*0C000151:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000152*/ 
    .custom /*0C000152:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000153*/ 
    .custom /*0C000153:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [3]/*08000154*/  = nullref
    .param [4]/*08000155*/  = nullref
    // Method begins at RVA 0x23d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*0600010D*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000023*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<valuetype System.Diagnostics.Metrics.Measurement`1/*0200001E*/<!!T>> observeValue,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 8C 01 1E 00 0E 15 12 80 81 01 15 11 78 01 1E 00 0E 0E
  {
    .param type T /*2A000011*/ 
      .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000156*/ 
    .custom /*0C000154:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000157*/ 
    .custom /*0C000155:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000158*/ 
    .custom /*0C000156:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [3]/*08000159*/  = nullref
    .param [4]/*0800015A*/  = nullref
    // Method begins at RVA 0x23dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*0600010E*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000023*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000020*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.Metrics.Measurement`1/*0200001E*/<!!T>>> observeValues,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 8C 01 1E 00 0E 15 12 80 81 01 15 12 75 01 15 11 78 01 1E 00 0E 0E
  {
    .param type T /*2A000012*/ 
      .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800015B*/ 
    .custom /*0C000157:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800015C*/ 
    .custom /*0C000158:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800015D*/ 
    .custom /*0C000159:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [3]/*0800015E*/  = nullref
    .param [4]/*0800015F*/  = nullref
    // Method begins at RVA 0x23df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*0600010F*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::Dispose

  .method /*06000110*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000125:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23e5
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Meter::.ctor

  .method /*06000111*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string version) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C000126:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000162*/ 
    .custom /*0C00015A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23ee
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Meter::.ctor

  .method /*06000112*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000128:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::get_Name

  .method /*06000113*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::get_Version

  .property /*1700004C*/ instance callconv(8) string
          Name()
  {
    .custom /*0C0000AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Meter/*02000020*/::get_Name() /* 06000112 */
  } // end of property Meter::Name
  .property /*1700004D*/ instance callconv(8) string
          Version()
  {
    .get instance string System.Diagnostics.Metrics.Meter/*02000020*/::get_Version() /* 06000113 */
  } // end of property Meter::Version
} // end of class System.Diagnostics.Metrics.Meter

.class /*02000021*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.MeterListener
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .method /*06000114*/ public hidebysig instance object 
          DisableMeasurementEvents(class System.Diagnostics.Metrics.Instrument/*0200001C*/ instrument) cil managed
  // SIG: 20 01 1C 12 70
  {
    .custom /*0C00012A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000163*/ 
    .custom /*0C00015B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::DisableMeasurementEvents

  .method /*06000115*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2400
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::Dispose

  .method /*06000116*/ public hidebysig instance void 
          EnableMeasurementEvents(class System.Diagnostics.Metrics.Instrument/*0200001C*/ instrument,
                                  [opt] object state) cil managed
  // SIG: 20 02 01 12 70 1C
  {
    .custom /*0C00012C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000166*/  = nullref
    .custom /*0C00015C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2403
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::EnableMeasurementEvents

  .method /*06000117*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,class System.Diagnostics.Metrics.MeterListener/*02000021*/> 
          get_InstrumentPublished() cil managed
  // SIG: 20 00 15 12 71 02 12 70 12 80 84
  {
    .param [0]/*08000167*/ 
    .custom /*0C00015D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2406
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::get_InstrumentPublished

  .method /*06000118*/ public hidebysig specialname 
          instance void  set_InstrumentPublished(class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,class System.Diagnostics.Metrics.MeterListener/*02000021*/> 'value') cil managed
  // SIG: 20 01 01 15 12 71 02 12 70 12 80 84
  {
    .param [1]/*08000168*/ 
    .custom /*0C00015E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2409
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::set_InstrumentPublished

  .method /*06000119*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,object> 
          get_MeasurementsCompleted() cil managed
  // SIG: 20 00 15 12 71 02 12 70 1C
  {
    .param [0]/*08000169*/ 
    .custom /*0C00015F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x240c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::get_MeasurementsCompleted

  .method /*0600011A*/ public hidebysig specialname 
          instance void  set_MeasurementsCompleted(class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,object> 'value') cil managed
  // SIG: 20 01 01 15 12 71 02 12 70 1C
  {
    .param [1]/*0800016A*/ 
    .custom /*0C000160:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x240f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::set_MeasurementsCompleted

  .method /*0600011B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2412
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001D */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method MeterListener::.ctor

  .method /*0600011C*/ public hidebysig instance void 
          RecordObservableInstruments() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x241b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::RecordObservableInstruments

  .method /*0600011D*/ public hidebysig instance void 
          SetMeasurementEventCallback<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>(class System.Diagnostics.Metrics.MeasurementCallback`1/*0200001F*/<!!T> measurementCallback) cil managed
  // SIG: 30 01 01 01 15 12 7C 01 1E 00
  {
    .param [1]/*0800016B*/ 
    .custom /*0C000161:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    // Method begins at RVA 0x241e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::SetMeasurementEventCallback

  .method /*0600011E*/ public hidebysig instance void 
          Start() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2421
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::Start

  .property /*1700004E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,class System.Diagnostics.Metrics.MeterListener/*02000021*/>
          InstrumentPublished()
  {
    .custom /*0C0000B0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,class System.Diagnostics.Metrics.MeterListener/*02000021*/> System.Diagnostics.Metrics.MeterListener/*02000021*/::get_InstrumentPublished() /* 06000117 */
    .set instance void System.Diagnostics.Metrics.MeterListener/*02000021*/::set_InstrumentPublished(class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,class System.Diagnostics.Metrics.MeterListener/*02000021*/>) /* 06000118 */
  } // end of property MeterListener::InstrumentPublished
  .property /*1700004F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,object>
          MeasurementsCompleted()
  {
    .custom /*0C0000B1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,object> System.Diagnostics.Metrics.MeterListener/*02000021*/::get_MeasurementsCompleted() /* 06000119 */
    .set instance void System.Diagnostics.Metrics.MeterListener/*02000021*/::set_MeasurementsCompleted(class [System.Runtime/*23000001*/]System.Action`2/*0100001C*/<class System.Diagnostics.Metrics.Instrument/*0200001C*/,object>) /* 0600011A */
  } // end of property MeterListener::MeasurementsCompleted
} // end of class System.Diagnostics.Metrics.MeterListener

.class /*02000022*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.ObservableCounter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>
       extends class System.Diagnostics.Metrics.ObservableInstrument`1/*02000024*/<!T>/*1B00000E*/
{
  .method /*0600011F*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000020*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 80 0E 0E 0E
  {
    // Method begins at RVA 0x2424
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)00001F       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000024*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000020*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string) /* 0A00001F */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableCounter`1::.ctor

  .method /*06000120*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.Metrics.Measurement`1/*0200001E*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 75 01 15 11 78 01 13 00
  {
    .param [0]/*08000170*/ 
    .custom /*0C000162:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x2432
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableCounter`1::Observe

} // end of class System.Diagnostics.Metrics.ObservableCounter`1

.class /*02000023*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.ObservableGauge`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>
       extends class System.Diagnostics.Metrics.ObservableInstrument`1/*02000024*/<!T>/*1B00000E*/
{
  .method /*06000121*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000020*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 80 0E 0E 0E
  {
    // Method begins at RVA 0x2435
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)00001F       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000024*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000020*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string) /* 0A00001F */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableGauge`1::.ctor

  .method /*06000122*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.Metrics.Measurement`1/*0200001E*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 75 01 15 11 78 01 13 00
  {
    .param [0]/*08000175*/ 
    .custom /*0C000163:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x2443
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableGauge`1::Observe

} // end of class System.Diagnostics.Metrics.ObservableGauge`1

.class /*02000024*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.ObservableInstrument`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000027*/) T>
       extends System.Diagnostics.Metrics.Instrument/*0200001C*/
{
  .method /*06000123*/ public hidebysig specialname virtual 
          instance bool  get_IsObservable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2446
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableInstrument`1::get_IsObservable

  .method /*06000124*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000020*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 80 0E 0E 0E
  {
    .custom /*0C000135:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000178*/ 
    .custom /*0C000164:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000179*/ 
    .custom /*0C000165:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2449
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (06)0000F0       */ call       instance void System.Diagnostics.Metrics.Instrument/*0200001C*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000020*/,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              string) /* 060000F0 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableInstrument`1::.ctor

  .method /*06000125*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001D*/<valuetype System.Diagnostics.Metrics.Measurement`1/*0200001E*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 75 01 15 11 78 01 13 00
  {
    .param [0]/*0800017A*/ 
    .custom /*0C000166:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ObservableInstrument`1::Observe

  .property /*17000050*/ instance callconv(8) bool
          IsObservable()
  {
    .get instance bool System.Diagnostics.Metrics.ObservableInstrument`1/*02000024*/::get_IsObservable() /* 06000123 */
  } // end of property ObservableInstrument`1::IsObservable
} // end of class System.Diagnostics.Metrics.ObservableInstrument`1


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
