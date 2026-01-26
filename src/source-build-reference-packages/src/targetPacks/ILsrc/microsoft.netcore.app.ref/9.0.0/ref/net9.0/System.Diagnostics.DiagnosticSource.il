


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
// Time-date stamp:            0xce624686
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00007400
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000931e
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
// Checksum:                       0x000188db
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000092cc [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x000004f4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00007e00 [0x00002930] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000092b0 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00007324 Virtual Size
//              0x00002000 Virtual Address
//              0x00007400 Size of Raw Data
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
//              0x0000a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00007600 Pointer to Raw Data
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
//              0x00007c00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000320
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000930e Import Name Table
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
// 0x000025e4 [0x00006c4c] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00009230 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00004444 Size
//              '#~' Name
//    Stream 2:
//              0x000044b0 Offset
//              0x00001700 Size
//              '#Strings' Name
//    Stream 3:
//              0x00005bb0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00005bb4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00005bc4 Offset
//              0x00001088 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016890bb69f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 42800
// PE header size       : 512 (496 used)    ( 1.20%)
// PE additional info   : 12011             (28.06%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.17%)
// CLR meta-data size  : 27724              (64.78%)
// CLR additional info : 128                ( 0.30%)
// CLR method headers  : 388                ( 0.91%)
// Managed code         : 1038              ( 2.43%)
// Data                 : 2048              ( 4.79%)
// Unaccounted          : -1121             (-2.62%)

// Num.of PE sections   : 3
//   .text    - 29696
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 27724
//   Module        -    1 (10 bytes)
//   TypeDef       -   46 (644 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -   69 (414 bytes)
//   MethodDef     -  405 (5670 bytes)    8 abstract, 0 native, 377 bodies
//   FieldDef      -   28 (168 bytes)     0 constant
//   MemberRef     -   43 (258 bytes)
//   ParamDef      -  643 (3858 bytes)
//   MethodImpl    -    6 (36 bytes)
//   Constant      -   92 (552 bytes)
//   CustomAttribute-  570 (3420 bytes)
//   ClassLayout   -    9 (72 bytes)
//   StandAloneSig -    1 (2 bytes)
//   InterfaceImpl -   28 (112 bytes)
//   PropertyMap   -   23 (92 bytes)
//   Property      -  101 (606 bytes)
//   MethodSemantic-  130 (780 bytes)
//   TypeSpec      -   15 (30 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   ExportedType  -    1 (14 bytes)
//   NestedClass   -    5 (20 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    1 (6 bytes)
//   GenericParam  -   47 (376 bytes)
//   GenericParamConstraint-   41 (164 bytes)
//   Strings       -  5887 bytes
//   Blobs         -  4232 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   235 bytes

// CLR additional info : 128

// CLR method headers : 388
//   Num.of method bodies  - 377
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 376

// Managed code : 1038
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class DiagnosticListener             (public) (auto) (ansi) 
// Class DiagnosticSource               (public) (abstract) (auto) (ansi) 
// Class Activity                       (public) (auto) (ansi) 
// Class ActivityChangedEventArgs       (public) (sequential) (ansi) (sealed) 
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
// Class ExceptionRecorder              (public) (auto) (ansi) (sealed) 
// Class ActivityListener               (public) (auto) (ansi) (sealed) 
// Class DistributedContextPropagator   (public) (abstract) (auto) (ansi) 
// Class TagList                        (public) (sequential) (ansi) (sealed) 
// Class Counter`1                      (public) (auto) (ansi) (sealed) 
// Class Gauge`1                        (public) (auto) (ansi) (sealed) 
// Class UpDownCounter`1                (public) (auto) (ansi) (sealed) 
// Class Histogram`1                    (public) (auto) (ansi) (sealed) 
// Interface IMeterFactory                  (public) (abstract) (auto) (ansi) 
// Class Instrument                     (public) (abstract) (auto) (ansi) 
// Class Instrument`1                   (public) (abstract) (auto) (ansi) 
// Class InstrumentAdvice`1             (public) (auto) (ansi) (sealed) 
// Class Measurement`1                  (public) (sequential) (ansi) (sealed) 
// Class MeasurementCallback`1          (public) (auto) (ansi) (sealed) 
// Class Meter                          (public) (auto) (ansi) 
// Class MeterFactoryExtensions         (public) (abstract) (auto) (ansi) (sealed) 
// Class MeterListener                  (public) (auto) (ansi) (sealed) 
// Class MeterOptions                   (public) (auto) (ansi) 
// Class ObservableCounter`1            (public) (auto) (ansi) (sealed) 
// Class ObservableUpDownCounter`1      (public) (auto) (ansi) (sealed) 
// Class ObservableGauge`1              (public) (auto) (ansi) (sealed) 
// Class ObservableInstrument`1         (public) (abstract) (auto) (ansi) 
// Class Enumerator`1                   (sequential) (ansi) (sealed) (nested public) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
// Class PropagatorGetterCallback       (auto) (ansi) (sealed) (nested public) 
// Class PropagatorSetterCallback       (auto) (ansi) (sealed) (nested public) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 9:0:0:0
}
.assembly /*20000001*/ noplatform System.Diagnostics.DiagnosticSource
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000005:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000006:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000007:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000008:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 39 2E 30 01 00 54 0E 14   // ersion=v9.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 39 2E 30 )          // Name..NET 9.0
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000007*/::.ctor() /* 0A000006 */ = ( 01 00 00 00 ) 
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                        73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                        53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*0100000A*/::.ctor(bool) /* 0A000009 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                       73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                       53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 39 2E 30 2E 32 34 2E 35 32 38 30 39 00   // ...9.0.24.52809.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 39 2E 30 2E 30 2B 39 64 35 61 36 61 39   // ...9.0.0+9d5a6a9
                                                                                                                                                                                61 61 34 36 33 64 36 64 31 30 62 30 62 30 62 61   // aa463d6d10b0b0ba
                                                                                                                                                                                36 64 35 39 38 32 63 63 38 32 66 33 36 33 64 63   // 6d5982cc82f363dc
                                                                                                                                                                                33 00 00 )                                        // 3..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                 73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                 53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C000016:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
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
  .ver 9:0:0:0
}
.class extern /*27000001*/ forwarder System.Runtime.CompilerServices.IsExternalInit
{
  .assembly extern System.Runtime /*23000001*/ 
}
.module System.Diagnostics.DiagnosticSource.dll
// MVID: {91a92ebb-62e2-4d1a-9ae1-6c65c447907c}
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.Diagnostics.DiagnosticListener
       extends System.Diagnostics.DiagnosticSource/*02000003*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000016*/,
                  class [System.Runtime/*23000001*/]System.IObservable`1/*01000017*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000001*/
{
  .custom /*0C000018:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000019:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
  .custom /*0C000001:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IObservable`1/*01000017*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000001*/
  .custom /*0C00001A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000010       */ call       instance void System.Diagnostics.DiagnosticSource/*02000003*/::.ctor() /* 06000010 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::.ctor

  .method /*06000002*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.IObservable`1/*01000017*/<class System.Diagnostics.DiagnosticListener/*02000002*/> 
          get_AllListeners() cil managed
  // SIG: 00 00 15 12 5D 01 12 08
  {
    // Method begins at RVA 0x2058
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::get_AllListeners

  .method /*06000003*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x205b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::get_Name

  .method /*06000004*/ public hidebysig newslot virtual 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x205e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::Dispose

  .method /*06000005*/ public hidebysig instance bool 
          IsEnabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2060
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*06000006*/ public hidebysig virtual 
          instance bool  IsEnabled(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x2063
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*06000007*/ public hidebysig virtual 
          instance bool  IsEnabled(string name,
                                   object arg1,
                                   [opt] object arg2) cil managed
  // SIG: 20 03 02 0E 1C 1C
  {
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000003*/ 
    .custom /*0C00001E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000005*/  = nullref
    // Method begins at RVA 0x2066
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*06000008*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000016*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*0100001A*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> observer) cil managed
  // SIG: 20 01 12 59 15 12 69 01 15 11 61 02 0E 1C
  {
    .param [1]/*08000006*/ 
    .custom /*0C000030:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2069
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*06000009*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000016*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*0100001A*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Func`4/*0100001B*/<string,object,object,bool> isEnabled) cil managed
  // SIG: 20 02 12 59 15 12 69 01 15 11 61 02 0E 1C 15 12 6D 04 0E 1C 1C 02
  {
    .param [1]/*08000007*/ 
    .custom /*0C000033:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*08000008*/ 
    .custom /*0C000035:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    // Method begins at RVA 0x206c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*0600000A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000016*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*0100001A*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Predicate`1/*0100001C*/<string> isEnabled) cil managed
  // SIG: 20 02 12 59 15 12 69 01 15 11 61 02 0E 1C 15 12 71 01 0E
  {
    .param [1]/*08000009*/ 
    .custom /*0C000039:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*0800000A*/ 
    .custom /*0C00003E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x206f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*0600000B*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2072
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::ToString

  .method /*0600000C*/ public hidebysig virtual 
          instance void  Write(string name,
                               object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C000041:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001D*/::.ctor(string) /* 0A000016 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .param [2]/*0800000C*/ 
    .custom /*0C000043:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2075
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::Write

  .method /*0600000D*/ public hidebysig virtual 
          instance void  OnActivityExport(class System.Diagnostics.Activity/*02000004*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 10 1C
  {
    .param [2]/*0800000E*/ 
    .custom /*0C000045:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2077
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::OnActivityExport

  .method /*0600000E*/ public hidebysig virtual 
          instance void  OnActivityImport(class System.Diagnostics.Activity/*02000004*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 10 1C
  {
    .param [2]/*08000010*/ 
    .custom /*0C00004E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2079
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::OnActivityImport

  .method /*0600000F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000016*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*0100001A*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Func`4/*0100001B*/<string,object,object,bool> isEnabled,
                    [opt] class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Activity/*02000004*/,object> onActivityImport,
                    [opt] class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Activity/*02000004*/,object> onActivityExport) cil managed
  // SIG: 20 04 12 59 15 12 69 01 15 11 61 02 0E 1C 15 12 6D 04 0E 1C 1C 02 15 12 79 02 12 10 1C 15 12 79
  //      02 12 10 1C
  {
    .param [1]/*08000011*/ 
    .custom /*0C000051:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*08000012*/ 
    .custom /*0C000056:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    .param [3]/*08000013*/  = nullref
    .custom /*0C00005B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    .param [4]/*08000014*/  = nullref
    .custom /*0C000060:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x207b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .property /*17000001*/ callconv(8) class [System.Runtime/*23000001*/]System.IObservable`1/*01000017*/<class System.Diagnostics.DiagnosticListener/*02000002*/>
          AllListeners()
  {
    .get class [System.Runtime/*23000001*/]System.IObservable`1/*01000017*/<class System.Diagnostics.DiagnosticListener/*02000002*/> System.Diagnostics.DiagnosticListener/*02000002*/::get_AllListeners() /* 06000002 */
  } // end of property DiagnosticListener::AllListeners
  .property /*17000002*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.DiagnosticListener/*02000002*/::get_Name() /* 06000003 */
  } // end of property DiagnosticListener::Name
} // end of class System.Diagnostics.DiagnosticListener

.class /*02000003*/ public abstract auto ansi beforefieldinit System.Diagnostics.DiagnosticSource
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000010*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x207e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::.ctor

  .method /*06000011*/ public hidebysig newslot abstract virtual 
          instance bool  IsEnabled(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x0
  } // end of method DiagnosticSource::IsEnabled

  .method /*06000012*/ public hidebysig newslot virtual 
          instance bool  IsEnabled(string name,
                                   object arg1,
                                   [opt] object arg2) cil managed
  // SIG: 20 03 02 0E 1C 1C
  {
    .custom /*0C000052:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000016*/ 
    .custom /*0C000067:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000018*/  = nullref
    // Method begins at RVA 0x2086
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticSource::IsEnabled

  .method /*06000013*/ public hidebysig newslot abstract virtual 
          instance void  Write(string name,
                               object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C00005A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001D*/::.ctor(string) /* 0A000016 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .param [2]/*0800001A*/ 
    .custom /*0C000072:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DiagnosticSource::Write

  .method /*06000014*/ public hidebysig instance void 
          Write<T>(string name,
                   !!T 'value') cil managed
  // SIG: 30 01 02 01 0E 1E 00
  {
    .custom /*0C00005F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001D*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 83 4F 6E 6C 79 20 74 68 65 20 70 72 6F   // ....Only the pro
                                                                                                                                                                                            70 65 72 74 69 65 73 20 6F 66 20 74 68 65 20 54   // perties of the T
                                                                                                                                                                                            20 74 79 70 65 20 77 69 6C 6C 20 62 65 20 70 72   //  type will be pr
                                                                                                                                                                                            65 73 65 72 76 65 64 2E 20 50 72 6F 70 65 72 74   // eserved. Propert
                                                                                                                                                                                            69 65 73 20 6F 66 20 72 65 66 65 72 65 6E 63 65   // ies of reference
                                                                                                                                                                                            64 20 74 79 70 65 73 20 61 6E 64 20 70 72 6F 70   // d types and prop
                                                                                                                                                                                            65 72 74 69 65 73 20 6F 66 20 64 65 72 69 76 65   // erties of derive
                                                                                                                                                                                            64 20 74 79 70 65 73 20 6D 61 79 20 62 65 20 74   // d types may be t
                                                                                                                                                                                            72 69 6D 6D 65 64 2E 00 00 )                      // rimmed...
    .param type T /*2A000003*/ 
      .custom /*0C000020:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
      .custom /*0C000021:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001F*/) /* 0A000017 */ = ( 01 00 00 02 00 00 00 00 ) 
    // Method begins at RVA 0x2089
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::Write

  .method /*06000015*/ public hidebysig newslot virtual 
          instance void  OnActivityExport(class System.Diagnostics.Activity/*02000004*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 10 1C
  {
    .param [2]/*0800001E*/ 
    .custom /*0C00007F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::OnActivityExport

  .method /*06000016*/ public hidebysig newslot virtual 
          instance void  OnActivityImport(class System.Diagnostics.Activity/*02000004*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 10 1C
  {
    .param [2]/*08000020*/ 
    .custom /*0C000085:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::OnActivityImport

  .method /*06000017*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          StartActivity(class System.Diagnostics.Activity/*02000004*/ activity,
                        object args) cil managed
  // SIG: 20 02 12 10 12 10 1C
  {
    .custom /*0C00006A:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001D*/::.ctor(string) /* 0A000016 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .param [2]/*08000022*/ 
    .custom /*0C00008E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x208f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticSource::StartActivity

  .method /*06000018*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          StartActivity<T>(class System.Diagnostics.Activity/*02000004*/ activity,
                           !!T args) cil managed
  // SIG: 30 01 02 12 10 12 10 1E 00
  {
    .custom /*0C00006E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001D*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 83 4F 6E 6C 79 20 74 68 65 20 70 72 6F   // ....Only the pro
                                                                                                                                                                                            70 65 72 74 69 65 73 20 6F 66 20 74 68 65 20 54   // perties of the T
                                                                                                                                                                                            20 74 79 70 65 20 77 69 6C 6C 20 62 65 20 70 72   //  type will be pr
                                                                                                                                                                                            65 73 65 72 76 65 64 2E 20 50 72 6F 70 65 72 74   // eserved. Propert
                                                                                                                                                                                            69 65 73 20 6F 66 20 72 65 66 65 72 65 6E 63 65   // ies of reference
                                                                                                                                                                                            64 20 74 79 70 65 73 20 61 6E 64 20 70 72 6F 70   // d types and prop
                                                                                                                                                                                            65 72 74 69 65 73 20 6F 66 20 64 65 72 69 76 65   // erties of derive
                                                                                                                                                                                            64 20 74 79 70 65 73 20 6D 61 79 20 62 65 20 74   // d types may be t
                                                                                                                                                                                            72 69 6D 6D 65 64 2E 00 00 )                      // rimmed...
    .param type T /*2A000005*/ 
      .custom /*0C00002B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
      .custom /*0C00002C:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001F*/) /* 0A000017 */ = ( 01 00 00 02 00 00 00 00 ) 
    // Method begins at RVA 0x2092
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticSource::StartActivity

  .method /*06000019*/ public hidebysig instance void 
          StopActivity(class System.Diagnostics.Activity/*02000004*/ activity,
                       object args) cil managed
  // SIG: 20 02 01 12 10 1C
  {
    .custom /*0C000070:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001D*/::.ctor(string) /* 0A000016 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .param [2]/*08000026*/ 
    .custom /*0C000097:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2095
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::StopActivity

  .method /*0600001A*/ public hidebysig instance void 
          StopActivity<T>(class System.Diagnostics.Activity/*02000004*/ activity,
                          !!T args) cil managed
  // SIG: 30 01 02 01 12 10 1E 00
  {
    .custom /*0C000071:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001D*/::.ctor(string) /* 0A000016 */ = ( 01 00 80 83 4F 6E 6C 79 20 74 68 65 20 70 72 6F   // ....Only the pro
                                                                                                                                                                                            70 65 72 74 69 65 73 20 6F 66 20 74 68 65 20 54   // perties of the T
                                                                                                                                                                                            20 74 79 70 65 20 77 69 6C 6C 20 62 65 20 70 72   //  type will be pr
                                                                                                                                                                                            65 73 65 72 76 65 64 2E 20 50 72 6F 70 65 72 74   // eserved. Propert
                                                                                                                                                                                            69 65 73 20 6F 66 20 72 65 66 65 72 65 6E 63 65   // ies of reference
                                                                                                                                                                                            64 20 74 79 70 65 73 20 61 6E 64 20 70 72 6F 70   // d types and prop
                                                                                                                                                                                            65 72 74 69 65 73 20 6F 66 20 64 65 72 69 76 65   // erties of derive
                                                                                                                                                                                            64 20 74 79 70 65 73 20 6D 61 79 20 62 65 20 74   // d types may be t
                                                                                                                                                                                            72 69 6D 6D 65 64 2E 00 00 )                      // rimmed...
    .param type T /*2A000008*/ 
      .custom /*0C000036:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
      .custom /*0C000037:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000020*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001F*/) /* 0A000017 */ = ( 01 00 00 02 00 00 00 00 ) 
    // Method begins at RVA 0x2097
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticSource::StopActivity

} // end of class System.Diagnostics.DiagnosticSource

.class /*02000004*/ public auto ansi beforefieldinit System.Diagnostics.Activity
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
{
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000023:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
  .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .class /*0200002A*/ sequential ansi sealed nested public beforefieldinit Enumerator`1<T>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
  {
    .pack 0
    .size 1
    .custom /*0C00009E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A000012*/ 
      .custom /*0C000059:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .method /*06000181*/ public hidebysig 
            instance valuetype System.Diagnostics.Activity/*02000004*//Enumerator`1/*0200002A*/<!T> 
            GetEnumerator() cil managed
    // SIG: 20 00 15 11 80 A8 01 13 00
    {
      .custom /*0C00019F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .custom /*0C0001A0:0A000019*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100002A*/) /* 0A000019 */ = ( 01 00 01 00 00 00 00 00 ) 
      .param [0]/*08000267*/ 
      .custom /*0C00022C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      // Method begins at RVA 0x25b0
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator`1::GetEnumerator

    .method /*06000182*/ public hidebysig specialname 
            instance !T&  get_Current() cil managed
    // SIG: 20 00 10 13 00
    {
      .custom /*0C0001A2:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x25b3
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator`1::get_Current

    .method /*06000183*/ public hidebysig 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x25b6
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator`1::MoveNext

    .property /*17000061*/ instance callconv(8) !T&
            Current()
    {
      .get instance !T& System.Diagnostics.Activity/*02000004*//Enumerator`1/*0200002A*/::get_Current() /* 06000182 */
    } // end of property Enumerator`1::Current
  } // end of class Enumerator`1

  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string operationName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x209a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Activity::.ctor

  .method /*0600001C*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ 
          get_ActivityTraceFlags() cil managed
  // SIG: 20 00 11 2C
  {
    // Method begins at RVA 0x20a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ActivityTraceFlags

  .method /*0600001D*/ public hidebysig specialname 
          instance void  set_ActivityTraceFlags(valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ 'value') cil managed
  // SIG: 20 01 01 11 2C
  {
    // Method begins at RVA 0x20a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_ActivityTraceFlags

  .method /*0600001E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,string>> 
          get_Baggage() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 0E
  {
    .param [0]/*0800002B*/ 
    .custom /*0C0000A1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x20a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Baggage

  .method /*0600001F*/ public hidebysig specialname static 
          class System.Diagnostics.Activity/*02000004*/ 
          get_Current() cil managed
  // SIG: 00 00 12 10
  {
    .custom /*0C000081:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Current

  .method /*06000020*/ public hidebysig specialname static 
          void  set_Current(class System.Diagnostics.Activity/*02000004*/ 'value') cil managed
  // SIG: 00 01 01 12 10
  {
    .custom /*0C000083:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ad
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_Current

  .method /*06000021*/ public hidebysig specialname static 
          void  add_CurrentChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*01000021*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*02000005*/> 'value') cil managed
  // SIG: 00 01 01 15 12 80 85 01 11 14
  {
    .custom /*0C000088:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::add_CurrentChanged

  .method /*06000022*/ public hidebysig specialname static 
          void  remove_CurrentChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*01000021*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*02000005*/> 'value') cil managed
  // SIG: 00 01 01 15 12 80 85 01 11 14
  {
    .custom /*0C00008B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::remove_CurrentChanged

  .method /*06000023*/ public hidebysig specialname static 
          valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ 
          get_DefaultIdFormat() cil managed
  // SIG: 00 00 11 20
  {
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_DefaultIdFormat

  .method /*06000024*/ public hidebysig specialname static 
          void  set_DefaultIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ 'value') cil managed
  // SIG: 00 01 01 11 20
  {
    // Method begins at RVA 0x20b6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_DefaultIdFormat

  .method /*06000025*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/ 
          get_Duration() cil managed
  // SIG: 20 00 11 80 8D
  {
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Duration

  .method /*06000026*/ public hidebysig specialname static 
          bool  get_ForceDefaultIdFormat() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ForceDefaultIdFormat

  .method /*06000027*/ public hidebysig specialname static 
          void  set_ForceDefaultIdFormat(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x20be
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_ForceDefaultIdFormat

  .method /*06000028*/ public hidebysig specialname 
          instance string  get_Id() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00009B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Id

  .method /*06000029*/ public hidebysig specialname 
          instance bool  get_HasRemoteParent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_HasRemoteParent

  .method /*0600002A*/ public hidebysig specialname 
          instance bool  get_IsAllDataRequested() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_IsAllDataRequested

  .method /*0600002B*/ public hidebysig specialname 
          instance void  set_IsAllDataRequested(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_IsAllDataRequested

  .method /*0600002C*/ public hidebysig specialname 
          instance bool  get_IsStopped() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_IsStopped

  .method /*0600002D*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ 
          get_IdFormat() cil managed
  // SIG: 20 00 11 20
  {
    // Method begins at RVA 0x20cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_IdFormat

  .method /*0600002E*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityKind/*0200000E*/ 
          get_Kind() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x20d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Kind

  .method /*0600002F*/ public hidebysig specialname 
          instance string  get_OperationName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_OperationName

  .method /*06000030*/ public hidebysig specialname 
          instance string  get_DisplayName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_DisplayName

  .method /*06000031*/ public hidebysig specialname 
          instance void  set_DisplayName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_DisplayName

  .method /*06000032*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivitySource/*0200000A*/ 
          get_Source() cil managed
  // SIG: 20 00 12 28
  {
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Source

  .method /*06000033*/ public hidebysig specialname 
          instance class System.Diagnostics.Activity/*02000004*/ 
          get_Parent() cil managed
  // SIG: 20 00 12 10
  {
    .custom /*0C0000AB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Parent

  .method /*06000034*/ public hidebysig specialname 
          instance string  get_ParentId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000AD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ParentId

  .method /*06000035*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*02000009*/ 
          get_ParentSpanId() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x20e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ParentSpanId

  .method /*06000036*/ public hidebysig specialname 
          instance bool  get_Recorded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Recorded

  .method /*06000037*/ public hidebysig specialname 
          instance string  get_RootId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_RootId

  .method /*06000038*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*02000009*/ 
          get_SpanId() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x20f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_SpanId

  .method /*06000039*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000024*/ 
          get_StartTimeUtc() cil managed
  // SIG: 20 00 11 80 91
  {
    // Method begins at RVA 0x20f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_StartTimeUtc

  .method /*0600003A*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityStatusCode/*02000007*/ 
          get_Status() cil managed
  // SIG: 20 00 11 1C
  {
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Status

  .method /*0600003B*/ public hidebysig specialname 
          instance string  get_StatusDescription() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_StatusDescription

  .method /*0600003C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,string>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 0E
  {
    .param [0]/*08000033*/ 
    .custom /*0C0000AC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Tags

  .method /*0600003D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_TagObjects() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [0]/*08000034*/ 
    .custom /*0C0000AE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x20ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TagObjects

  .method /*0600003E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityEvent/*0200000F*/> 
          get_Events() cil managed
  // SIG: 20 00 15 12 80 89 01 11 3C
  {
    // Method begins at RVA 0x2102
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Events

  .method /*0600003F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> 
          get_Links() cil managed
  // SIG: 20 00 15 12 80 89 01 11 44
  {
    // Method begins at RVA 0x2105
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Links

  .method /*06000040*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceId

  .method /*06000041*/ public hidebysig specialname 
          instance string  get_TraceStateString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000BA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceStateString

  .method /*06000042*/ public hidebysig specialname 
          instance void  set_TraceStateString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000BC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x210e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_TraceStateString

  .method /*06000043*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          AddBaggage(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 10 0E 0E
  {
    .param [2]/*08000037*/ 
    .custom /*0C0000B1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddBaggage

  .method /*06000044*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          AddEvent(valuetype System.Diagnostics.ActivityEvent/*0200000F*/ e) cil managed
  // SIG: 20 01 12 10 11 3C
  {
    // Method begins at RVA 0x2113
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddEvent

  .method /*06000045*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          AddException(class [System.Runtime/*23000001*/]System.Exception/*01000025*/ exception,
                       [in][opt] valuetype System.Diagnostics.TagList/*02000017*/& tags,
                       [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000027*/ timestamp) cil managed
  // SIG: 20 03 12 10 12 80 95 10 11 5C 11 80 9D
  {
    .param [2]/*0800003A*/  = nullref
    .custom /*0C0000B2:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .param [3]/*0800003B*/  = nullref
    // Method begins at RVA 0x2116
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddException

  .method /*06000046*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          AddLink(valuetype System.Diagnostics.ActivityLink/*02000011*/ link) cil managed
  // SIG: 20 01 12 10 11 44
  {
    // Method begins at RVA 0x2119
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddLink

  .method /*06000047*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          AddTag(string key,
                 string 'value') cil managed
  // SIG: 20 02 12 10 0E 0E
  {
    .param [2]/*0800003E*/ 
    .custom /*0C0000B6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddTag

  .method /*06000048*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          AddTag(string key,
                 object 'value') cil managed
  // SIG: 20 02 12 10 0E 1C
  {
    .param [2]/*08000040*/ 
    .custom /*0C0000B8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddTag

  .method /*06000049*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          SetTag(string key,
                 object 'value') cil managed
  // SIG: 20 02 12 10 0E 1C
  {
    .param [2]/*08000042*/ 
    .custom /*0C0000BD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetTag

  .method /*0600004A*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          SetBaggage(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 10 0E 0E
  {
    .param [2]/*08000044*/ 
    .custom /*0C0000BF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetBaggage

  .method /*0600004B*/ public hidebysig instance string 
          GetBaggageItem(string key) cil managed
  // SIG: 20 01 0E 0E
  {
    .param [0]/*08000045*/ 
    .custom /*0C0000C1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetBaggageItem

  .method /*0600004C*/ public hidebysig instance object 
          GetTagItem(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*08000047*/ 
    .custom /*0C0000C2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetTagItem

  .method /*0600004D*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          SetEndTime(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000024*/ endTimeUtc) cil managed
  // SIG: 20 01 12 10 11 80 91
  {
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetEndTime

  .method /*0600004E*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.ActivityTraceId/*0200000C*/> 
          get_TraceIdGenerator() cil managed
  // SIG: 00 00 15 12 80 A1 01 11 30
  {
    .custom /*0C0000C5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceIdGenerator

  .method /*0600004F*/ public hidebysig specialname static 
          void  set_TraceIdGenerator(class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.ActivityTraceId/*0200000C*/> 'value') cil managed
  // SIG: 00 01 01 15 12 80 A1 01 11 30
  {
    .custom /*0C0000C6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_TraceIdGenerator

  .method /*06000050*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          SetIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ format) cil managed
  // SIG: 20 01 12 10 11 20
  {
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetIdFormat

  .method /*06000051*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          SetParentId(valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ traceId,
                      valuetype System.Diagnostics.ActivitySpanId/*02000009*/ spanId,
                      [opt] valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ activityTraceFlags) cil managed
  // SIG: 20 03 12 10 11 30 11 24 11 2C
  {
    .param [3]/*0800004E*/  = int32(0x00000000)
    // Method begins at RVA 0x213a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetParentId

  .method /*06000052*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          SetParentId(string parentId) cil managed
  // SIG: 20 01 12 10 0E
  {
    // Method begins at RVA 0x213d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetParentId

  .method /*06000053*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          SetStartTime(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000024*/ startTimeUtc) cil managed
  // SIG: 20 01 12 10 11 80 91
  {
    // Method begins at RVA 0x2140
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetStartTime

  .method /*06000054*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          SetStatus(valuetype System.Diagnostics.ActivityStatusCode/*02000007*/ code,
                    [opt] string description) cil managed
  // SIG: 20 02 12 10 11 1C 0E
  {
    .param [2]/*08000052*/  = nullref
    .custom /*0C0000C9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetStatus

  .method /*06000055*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          Start() cil managed
  // SIG: 20 00 12 10
  {
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Start

  .method /*06000056*/ public hidebysig instance void 
          Stop() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Stop

  .method /*06000057*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Dispose

  .method /*06000058*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x214f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Dispose

  .method /*06000059*/ public hidebysig instance void 
          SetCustomProperty(string propertyName,
                            object propertyValue) cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000055*/ 
    .custom /*0C0000CD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2152
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetCustomProperty

  .method /*0600005A*/ public hidebysig instance object 
          GetCustomProperty(string propertyName) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*08000056*/ 
    .custom /*0C0000CE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2155
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetCustomProperty

  .method /*0600005B*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityContext/*02000010*/ 
          get_Context() cil managed
  // SIG: 20 00 11 40
  {
    // Method begins at RVA 0x2158
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Context

  .method /*0600005C*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000004*//Enumerator`1/*0200002A*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          EnumerateTagObjects() cil managed
  // SIG: 20 00 15 11 80 A8 01 15 11 61 02 0E 1C
  {
    .param [0]/*08000058*/ 
    .custom /*0C0000D0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::EnumerateTagObjects

  .method /*0600005D*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000004*//Enumerator`1/*0200002A*/<valuetype System.Diagnostics.ActivityEvent/*0200000F*/> 
          EnumerateEvents() cil managed
  // SIG: 20 00 15 11 80 A8 01 11 3C
  {
    .custom /*0C0000D7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x215e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::EnumerateEvents

  .method /*0600005E*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000004*//Enumerator`1/*0200002A*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> 
          EnumerateLinks() cil managed
  // SIG: 20 00 15 11 80 A8 01 11 44
  {
    .custom /*0C0000D8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2161
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::EnumerateLinks

  .event /*14000001*/ class [System.Runtime/*23000001*/]System.EventHandler`1/*01000021*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*02000005*/>/*1B000002*/ CurrentChanged
  {
    .custom /*0C000004:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .addon void System.Diagnostics.Activity/*02000004*/::add_CurrentChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*01000021*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*02000005*/>) /* 06000021 */
    .removeon void System.Diagnostics.Activity/*02000004*/::remove_CurrentChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*01000021*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*02000005*/>) /* 06000022 */
  } // end of event Activity::CurrentChanged
  .property /*17000003*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/
          ActivityTraceFlags()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ System.Diagnostics.Activity/*02000004*/::get_ActivityTraceFlags() /* 0600001C */
    .set instance void System.Diagnostics.Activity/*02000004*/::set_ActivityTraceFlags(valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/) /* 0600001D */
  } // end of property Activity::ActivityTraceFlags
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,string>>
          Baggage()
  {
    .custom /*0C000025:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,string>> System.Diagnostics.Activity/*02000004*/::get_Baggage() /* 0600001E */
  } // end of property Activity::Baggage
  .property /*17000005*/ callconv(8) class System.Diagnostics.Activity/*02000004*/
          Current()
  {
    .custom /*0C00002A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get class System.Diagnostics.Activity/*02000004*/ System.Diagnostics.Activity/*02000004*/::get_Current() /* 0600001F */
    .set void System.Diagnostics.Activity/*02000004*/::set_Current(class System.Diagnostics.Activity/*02000004*/) /* 06000020 */
  } // end of property Activity::Current
  .property /*17000006*/ callconv(8) valuetype System.Diagnostics.ActivityIdFormat/*02000008*/
          DefaultIdFormat()
  {
    .get valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ System.Diagnostics.Activity/*02000004*/::get_DefaultIdFormat() /* 06000023 */
    .set void System.Diagnostics.Activity/*02000004*/::set_DefaultIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*02000008*/) /* 06000024 */
  } // end of property Activity::DefaultIdFormat
  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/
          Duration()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000023*/ System.Diagnostics.Activity/*02000004*/::get_Duration() /* 06000025 */
  } // end of property Activity::Duration
  .property /*17000008*/ callconv(8) bool
          ForceDefaultIdFormat()
  {
    .get bool System.Diagnostics.Activity/*02000004*/::get_ForceDefaultIdFormat() /* 06000026 */
    .set void System.Diagnostics.Activity/*02000004*/::set_ForceDefaultIdFormat(bool) /* 06000027 */
  } // end of property Activity::ForceDefaultIdFormat
  .property /*17000009*/ instance callconv(8) string
          Id()
  {
    .custom /*0C00003B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000004*/::get_Id() /* 06000028 */
  } // end of property Activity::Id
  .property /*1700000A*/ instance callconv(8) bool
          HasRemoteParent()
  {
    .get instance bool System.Diagnostics.Activity/*02000004*/::get_HasRemoteParent() /* 06000029 */
  } // end of property Activity::HasRemoteParent
  .property /*1700000B*/ instance callconv(8) bool
          IsAllDataRequested()
  {
    .get instance bool System.Diagnostics.Activity/*02000004*/::get_IsAllDataRequested() /* 0600002A */
    .set instance void System.Diagnostics.Activity/*02000004*/::set_IsAllDataRequested(bool) /* 0600002B */
  } // end of property Activity::IsAllDataRequested
  .property /*1700000C*/ instance callconv(8) bool
          IsStopped()
  {
    .get instance bool System.Diagnostics.Activity/*02000004*/::get_IsStopped() /* 0600002C */
  } // end of property Activity::IsStopped
  .property /*1700000D*/ instance callconv(8) valuetype System.Diagnostics.ActivityIdFormat/*02000008*/
          IdFormat()
  {
    .get instance valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ System.Diagnostics.Activity/*02000004*/::get_IdFormat() /* 0600002D */
  } // end of property Activity::IdFormat
  .property /*1700000E*/ instance callconv(8) valuetype System.Diagnostics.ActivityKind/*0200000E*/
          Kind()
  {
    .get instance valuetype System.Diagnostics.ActivityKind/*0200000E*/ System.Diagnostics.Activity/*02000004*/::get_Kind() /* 0600002E */
  } // end of property Activity::Kind
  .property /*1700000F*/ instance callconv(8) string
          OperationName()
  {
    .get instance string System.Diagnostics.Activity/*02000004*/::get_OperationName() /* 0600002F */
  } // end of property Activity::OperationName
  .property /*17000010*/ instance callconv(8) string
          DisplayName()
  {
    .get instance string System.Diagnostics.Activity/*02000004*/::get_DisplayName() /* 06000030 */
    .set instance void System.Diagnostics.Activity/*02000004*/::set_DisplayName(string) /* 06000031 */
  } // end of property Activity::DisplayName
  .property /*17000011*/ instance callconv(8) class System.Diagnostics.ActivitySource/*0200000A*/
          Source()
  {
    .get instance class System.Diagnostics.ActivitySource/*0200000A*/ System.Diagnostics.Activity/*02000004*/::get_Source() /* 06000032 */
  } // end of property Activity::Source
  .property /*17000012*/ instance callconv(8) class System.Diagnostics.Activity/*02000004*/
          Parent()
  {
    .custom /*0C000058:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Diagnostics.Activity/*02000004*/ System.Diagnostics.Activity/*02000004*/::get_Parent() /* 06000033 */
  } // end of property Activity::Parent
  .property /*17000013*/ instance callconv(8) string
          ParentId()
  {
    .custom /*0C00005D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000004*/::get_ParentId() /* 06000034 */
  } // end of property Activity::ParentId
  .property /*17000014*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*02000009*/
          ParentSpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*02000009*/ System.Diagnostics.Activity/*02000004*/::get_ParentSpanId() /* 06000035 */
  } // end of property Activity::ParentSpanId
  .property /*17000015*/ instance callconv(8) bool
          Recorded()
  {
    .get instance bool System.Diagnostics.Activity/*02000004*/::get_Recorded() /* 06000036 */
  } // end of property Activity::Recorded
  .property /*17000016*/ instance callconv(8) string
          RootId()
  {
    .custom /*0C000068:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000004*/::get_RootId() /* 06000037 */
  } // end of property Activity::RootId
  .property /*17000017*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*02000009*/
          SpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*02000009*/ System.Diagnostics.Activity/*02000004*/::get_SpanId() /* 06000038 */
  } // end of property Activity::SpanId
  .property /*17000018*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*01000024*/
          StartTimeUtc()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000024*/ System.Diagnostics.Activity/*02000004*/::get_StartTimeUtc() /* 06000039 */
  } // end of property Activity::StartTimeUtc
  .property /*17000019*/ instance callconv(8) valuetype System.Diagnostics.ActivityStatusCode/*02000007*/
          Status()
  {
    .get instance valuetype System.Diagnostics.ActivityStatusCode/*02000007*/ System.Diagnostics.Activity/*02000004*/::get_Status() /* 0600003A */
  } // end of property Activity::Status
  .property /*1700001A*/ instance callconv(8) string
          StatusDescription()
  {
    .custom /*0C000074:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000004*/::get_StatusDescription() /* 0600003B */
  } // end of property Activity::StatusDescription
  .property /*1700001B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,string>>
          Tags()
  {
    .custom /*0C000077:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,string>> System.Diagnostics.Activity/*02000004*/::get_Tags() /* 0600003C */
  } // end of property Activity::Tags
  .property /*1700001C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          TagObjects()
  {
    .custom /*0C00007A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.Activity/*02000004*/::get_TagObjects() /* 0600003D */
  } // end of property Activity::TagObjects
  .property /*1700001D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityEvent/*0200000F*/>
          Events()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityEvent/*0200000F*/> System.Diagnostics.Activity/*02000004*/::get_Events() /* 0600003E */
  } // end of property Activity::Events
  .property /*1700001E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/>
          Links()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> System.Diagnostics.Activity/*02000004*/::get_Links() /* 0600003F */
  } // end of property Activity::Links
  .property /*1700001F*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*0200000C*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ System.Diagnostics.Activity/*02000004*/::get_TraceId() /* 06000040 */
  } // end of property Activity::TraceId
  .property /*17000020*/ instance callconv(8) string
          TraceStateString()
  {
    .custom /*0C000086:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000004*/::get_TraceStateString() /* 06000041 */
    .set instance void System.Diagnostics.Activity/*02000004*/::set_TraceStateString(string) /* 06000042 */
  } // end of property Activity::TraceStateString
  .property /*17000021*/ callconv(8) class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.ActivityTraceId/*0200000C*/>
          TraceIdGenerator()
  {
    .custom /*0C000089:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.ActivityTraceId/*0200000C*/> System.Diagnostics.Activity/*02000004*/::get_TraceIdGenerator() /* 0600004E */
    .set void System.Diagnostics.Activity/*02000004*/::set_TraceIdGenerator(class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.ActivityTraceId/*0200000C*/>) /* 0600004F */
  } // end of property Activity::TraceIdGenerator
  .property /*17000022*/ instance callconv(8) valuetype System.Diagnostics.ActivityContext/*02000010*/
          Context()
  {
    .get instance valuetype System.Diagnostics.ActivityContext/*02000010*/ System.Diagnostics.Activity/*02000004*/::get_Context() /* 0600005B */
  } // end of property Activity::Context
} // end of class System.Diagnostics.Activity

.class /*02000005*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityChangedEventArgs
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
{
  .pack 0
  .size 1
  .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000027:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000028:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .method /*0600005F*/ public hidebysig specialname 
          instance class System.Diagnostics.Activity/*02000004*/ 
          get_Previous() cil managed
  // SIG: 20 00 12 10
  {
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityChangedEventArgs::get_Previous

  .method /*06000060*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*0100002C*/) 
          set_Previous(class System.Diagnostics.Activity/*02000004*/ 'value') cil managed
  // SIG: 20 01 1F 80 B1 01 12 10
  {
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityChangedEventArgs::set_Previous

  .method /*06000061*/ public hidebysig specialname 
          instance class System.Diagnostics.Activity/*02000004*/ 
          get_Current() cil managed
  // SIG: 20 00 12 10
  {
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityChangedEventArgs::get_Current

  .method /*06000062*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*0100002C*/) 
          set_Current(class System.Diagnostics.Activity/*02000004*/ 'value') cil managed
  // SIG: 20 01 1F 80 B1 01 12 10
  {
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityChangedEventArgs::set_Current

  .property /*17000023*/ instance callconv(8) class System.Diagnostics.Activity/*02000004*/
          Previous()
  {
    .get instance class System.Diagnostics.Activity/*02000004*/ System.Diagnostics.ActivityChangedEventArgs/*02000005*/::get_Previous() /* 0600005F */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*0100002C*/) System.Diagnostics.ActivityChangedEventArgs/*02000005*/::set_Previous(class System.Diagnostics.Activity/*02000004*/) /* 06000060 */
  } // end of property ActivityChangedEventArgs::Previous
  .property /*17000024*/ instance callconv(8) class System.Diagnostics.Activity/*02000004*/
          Current()
  {
    .get instance class System.Diagnostics.Activity/*02000004*/ System.Diagnostics.ActivityChangedEventArgs/*02000005*/::get_Current() /* 06000061 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*0100002C*/) System.Diagnostics.ActivityChangedEventArgs/*02000005*/::set_Current(class System.Diagnostics.Activity/*02000004*/) /* 06000062 */
  } // end of property ActivityChangedEventArgs::Current
} // end of class System.Diagnostics.ActivityChangedEventArgs

.class /*02000006*/ public auto ansi beforefieldinit System.Diagnostics.ActivityTagsCollection
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000031*/<string,object>/*1B000004*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000005*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000003*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002F*/
{
  .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100002D*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000031*/<string,object>/*1B000004*/
  .custom /*0C000024:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000005*/
  .custom /*0C000029:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000003*/
  .custom /*0C000031:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002F*/
  .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .class /*0200002B*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000006*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000030*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
  {
    .pack 0
    .size 1
    .custom /*0C0000A0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000006*/
    .custom /*0C00006C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .method /*06000184*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> 
            get_Current() cil managed
    // SIG: 20 00 15 11 61 02 0E 1C
    {
      .param [0]/*08000268*/ 
      .custom /*0C00022D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      // Method begins at RVA 0x25b9
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*06000185*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000030*/::get_Current /*01000030::0A00001D*/ 
      // Method begins at RVA 0x25bc
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*06000186*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x25bf
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Dispose

    .method /*06000187*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x25c2
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*06000188*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000030*/::Reset /*01000030::0A00001E*/ 
      // Method begins at RVA 0x25c5
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.Reset

    .property /*17000062*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>
            Current()
    {
      .custom /*0C0000DD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> System.Diagnostics.ActivityTagsCollection/*02000006*//Enumerator/*0200002B*/::get_Current() /* 06000184 */
    } // end of property Enumerator::Current
    .property /*17000063*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .custom /*0C0000DF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
      .get instance object System.Diagnostics.ActivityTagsCollection/*02000006*//Enumerator/*0200002B*/::System.Collections.IEnumerator.get_Current() /* 06000185 */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2170
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::.ctor

  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> list) cil managed
  // SIG: 20 01 01 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [1]/*0800005B*/ 
    .custom /*0C0000D3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2179
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::.ctor

  .method /*06000065*/ public hidebysig newslot specialname virtual final 
          instance object  get_Item(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*0800005C*/ 
    .custom /*0C0000D5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Item

  .method /*06000066*/ public hidebysig newslot specialname virtual final 
          instance void  set_Item(string key,
                                  object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*0800005F*/ 
    .custom /*0C0000DA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2185
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivityTagsCollection::set_Item

  .method /*06000067*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<string> 
          get_Keys() cil managed
  // SIG: 20 00 15 12 80 C9 01 0E
  {
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Keys

  .method /*06000068*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<object> 
          get_Values() cil managed
  // SIG: 20 00 15 12 80 C9 01 1C
  {
    .param [0]/*08000060*/ 
    .custom /*0C0000DB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Values

  .method /*06000069*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x218d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Count

  .method /*0600006A*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2190
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_IsReadOnly

  .method /*0600006B*/ public hidebysig newslot virtual final 
          instance void  Add(string key,
                             object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000062*/ 
    .custom /*0C0000DC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2193
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Add

  .method /*0600006C*/ public hidebysig newslot virtual final 
          instance void  Add(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> item) cil managed
  // SIG: 20 01 01 15 11 61 02 0E 1C
  {
    .param [1]/*08000063*/ 
    .custom /*0C0000DE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2196
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Add

  .method /*0600006D*/ public hidebysig newslot virtual final 
          instance void  Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2199
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Clear

  .method /*0600006E*/ public hidebysig newslot virtual final 
          instance bool  Contains(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 61 02 0E 1C
  {
    .param [1]/*08000064*/ 
    .custom /*0C0000E0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x219c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Contains

  .method /*0600006F*/ public hidebysig newslot virtual final 
          instance bool  ContainsKey(string key) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::ContainsKey

  .method /*06000070*/ public hidebysig newslot virtual final 
          instance void  CopyTo(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>[] 'array',
                                int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 61 02 0E 1C 08
  {
    .param [1]/*08000066*/ 
    .custom /*0C0000E3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x21a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::CopyTo

  .method /*06000071*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 80 B9 01 15 11 61 02 0E 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002E*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000003*/::GetEnumerator() /* 0A00001B */
    // Method begins at RVA 0x21a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'

  .method /*06000072*/ public hidebysig newslot virtual final 
          instance bool  Remove(string key) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x21a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Remove

  .method /*06000073*/ public hidebysig newslot virtual final 
          instance bool  Remove(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 61 02 0E 1C
  {
    .param [1]/*08000069*/ 
    .custom /*0C0000E4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Remove

  .method /*06000074*/ public hidebysig newslot virtual final 
          instance bool  TryGetValue(string key,
                                     [out] object& 'value') cil managed
  // SIG: 20 02 02 0E 10 1C
  {
    .param [2]/*0800006B*/ 
    .custom /*0C0000E5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::TryGetValue

  .method /*06000075*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000030*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 C1
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002F*/::GetEnumerator /*0100002F::0A00001C*/ 
    // Method begins at RVA 0x21b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::System.Collections.IEnumerable.GetEnumerator

  .method /*06000076*/ public hidebysig instance valuetype System.Diagnostics.ActivityTagsCollection/*02000006*//Enumerator/*0200002B*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 80 AC
  {
    // Method begins at RVA 0x21b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::GetEnumerator

  .property /*17000025*/ instance callconv(8) object
          Item(string)
  {
    .custom /*0C000095:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Diagnostics.ActivityTagsCollection/*02000006*/::get_Item(string) /* 06000065 */
    .set instance void System.Diagnostics.ActivityTagsCollection/*02000006*/::set_Item(string,
                                                                                       object) /* 06000066 */
  } // end of property ActivityTagsCollection::Item
  .property /*17000026*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<string>
          Keys()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<string> System.Diagnostics.ActivityTagsCollection/*02000006*/::get_Keys() /* 06000067 */
  } // end of property ActivityTagsCollection::Keys
  .property /*17000027*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<object>
          Values()
  {
    .custom /*0C000099:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<object> System.Diagnostics.ActivityTagsCollection/*02000006*/::get_Values() /* 06000068 */
  } // end of property ActivityTagsCollection::Values
  .property /*17000028*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Diagnostics.ActivityTagsCollection/*02000006*/::get_Count() /* 06000069 */
  } // end of property ActivityTagsCollection::Count
  .property /*17000029*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Diagnostics.ActivityTagsCollection/*02000006*/::get_IsReadOnly() /* 0600006A */
  } // end of property ActivityTagsCollection::IsReadOnly
} // end of class System.Diagnostics.ActivityTagsCollection

.class /*02000007*/ public auto ansi sealed System.Diagnostics.ActivityStatusCode
       extends [System.Runtime/*23000001*/]System.Enum/*01000033*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*02000007*/ Unset = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*02000007*/ Ok = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*02000007*/ Error = int32(0x00000002)
} // end of class System.Diagnostics.ActivityStatusCode

.class /*02000008*/ public auto ansi sealed System.Diagnostics.ActivityIdFormat
       extends [System.Runtime/*23000001*/]System.Enum/*01000033*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ Unknown = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ Hierarchical = int32(0x00000001)
  .field /*04000008*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ W3C = int32(0x00000002)
} // end of class System.Diagnostics.ActivityIdFormat

.class /*02000009*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivitySpanId
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000034*/<valuetype System.Diagnostics.ActivitySpanId/*02000009*/>/*1B000007*/
{
  .custom /*0C000038:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .field /*04000009*/ private initonly object _dummy
  .field /*0400000A*/ private initonly int32 _dummyPrimitive
  .method /*06000077*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000035*/<uint8> destination) cil managed
  // SIG: 20 01 01 15 11 80 D5 01 05
  {
    // Method begins at RVA 0x21b7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivitySpanId::CopyTo

  .method /*06000078*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*02000009*/ 
          CreateFromBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<uint8> idData) cil managed
  // SIG: 00 01 11 24 15 11 80 D9 01 05
  {
    // Method begins at RVA 0x21b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromBytes

  .method /*06000079*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*02000009*/ 
          CreateFromString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<char> idData) cil managed
  // SIG: 00 01 11 24 15 11 80 D9 01 03
  {
    // Method begins at RVA 0x21bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromString

  .method /*0600007A*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*02000009*/ 
          CreateFromUtf8String(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<uint8> idData) cil managed
  // SIG: 00 01 11 24 15 11 80 D9 01 05
  {
    // Method begins at RVA 0x21bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromUtf8String

  .method /*0600007B*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*02000009*/ 
          CreateRandom() cil managed
  // SIG: 00 00 11 24
  {
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateRandom

  .method /*0600007C*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivitySpanId/*02000009*/ spanId) cil managed
  // SIG: 20 01 02 11 24
  {
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::Equals

  .method /*0600007D*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000EB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000071*/ 
    .custom /*0C0000E6:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000037*/::.ctor(bool) /* 0A00001F */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::Equals

  .method /*0600007E*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::GetHashCode

  .method /*0600007F*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivitySpanId/*02000009*/ spanId1,
                            valuetype System.Diagnostics.ActivitySpanId/*02000009*/ spandId2) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x21ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::op_Equality

  .method /*06000080*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivitySpanId/*02000009*/ spanId1,
                              valuetype System.Diagnostics.ActivitySpanId/*02000009*/ spandId2) cil managed
  // SIG: 00 02 02 11 24 11 24
  {
    // Method begins at RVA 0x21d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::op_Inequality

  .method /*06000081*/ public hidebysig instance string 
          ToHexString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000EE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::ToHexString

  .method /*06000082*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000EF:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::ToString

} // end of class System.Diagnostics.ActivitySpanId

.class /*0200000A*/ public auto ansi sealed beforefieldinit System.Diagnostics.ActivitySource
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
{
  .custom /*0C00003C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
  .custom /*0C00003A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21da
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivitySource::.ctor

  .method /*06000084*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               [opt] string version) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C0000F1:0A000019*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100002A*/) /* 0A000019 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [2]/*08000078*/  = ""
    .custom /*0C0000E7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e3
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivitySource::.ctor

  .method /*06000085*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               [opt] string version,
                               [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 03 01 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [2]/*0800007A*/  = ""
    .custom /*0C0000E8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800007B*/  = nullref
    .custom /*0C0000E9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x21ec
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivitySource::.ctor

  .method /*06000086*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::get_Name

  .method /*06000087*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000F3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::get_Version

  .method /*06000088*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [0]/*0800007C*/ 
    .custom /*0C0000EA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x21fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::get_Tags

  .method /*06000089*/ public hidebysig instance bool 
          HasListeners() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::HasListeners

  .method /*0600008A*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*0200000E*/ kind) cil managed
  // SIG: 20 02 12 10 0E 11 38
  {
    .param [0]/*0800007D*/ 
    .custom /*0C0000EC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2201
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*0600008B*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*0200000E*/ kind,
                         valuetype System.Diagnostics.ActivityContext/*02000010*/ parentContext,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> links,
                         [opt] valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ idFormat) cil managed
  // SIG: 20 06 12 10 0E 11 38 11 40 15 12 80 89 01 15 11 61 02 0E 1C 15 12 80 89 01 11 44 11 20
  {
    .custom /*0C0000F5:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000080*/ 
    .custom /*0C0000ED:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000083*/  = nullref
    .custom /*0C0000F0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000084*/  = nullref
    .param [6]/*08000085*/  = int32(0x00000000)
    // Method begins at RVA 0x2204
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*0600008C*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*0200000E*/ kind,
                         string parentId,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> links,
                         [opt] valuetype System.Diagnostics.ActivityIdFormat/*02000008*/ idFormat) cil managed
  // SIG: 20 06 12 10 0E 11 38 0E 15 12 80 89 01 15 11 61 02 0E 1C 15 12 80 89 01 11 44 11 20
  {
    .custom /*0C0000F6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000086*/ 
    .custom /*0C0000F2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000089*/  = nullref
    .custom /*0C0000F4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*0800008A*/  = nullref
    .param [6]/*0800008B*/  = int32(0x00000000)
    // Method begins at RVA 0x2207
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*0600008D*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          StartActivity([opt] string name,
                        [opt] valuetype System.Diagnostics.ActivityKind/*0200000E*/ kind) cil managed
  // SIG: 20 02 12 10 0E 11 38
  {
    .param [0]/*0800008C*/ 
    .custom /*0C0000F7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800008D*/  = ""
    .custom /*0C0000F8:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CallerMemberNameAttribute/*01000038*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    .param [2]/*0800008E*/  = int32(0x00000000)
    // Method begins at RVA 0x220a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*0600008E*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          StartActivity(string name,
                        valuetype System.Diagnostics.ActivityKind/*0200000E*/ kind,
                        valuetype System.Diagnostics.ActivityContext/*02000010*/ parentContext,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000027*/ startTime) cil managed
  // SIG: 20 06 12 10 0E 11 38 11 40 15 12 80 89 01 15 11 61 02 0E 1C 15 12 80 89 01 11 44 11 80 9D
  {
    .custom /*0C0000F9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800008F*/ 
    .custom /*0C0000FB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000092*/  = nullref
    .custom /*0C0000FD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000093*/  = nullref
    .param [6]/*08000094*/  = nullref
    // Method begins at RVA 0x220d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*0600008F*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          StartActivity(string name,
                        valuetype System.Diagnostics.ActivityKind/*0200000E*/ kind,
                        string parentId,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000027*/ startTime) cil managed
  // SIG: 20 06 12 10 0E 11 38 0E 15 12 80 89 01 15 11 61 02 0E 1C 15 12 80 89 01 11 44 11 80 9D
  {
    .custom /*0C0000FA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000095*/ 
    .custom /*0C0000FE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000098*/  = nullref
    .custom /*0C0000FF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000099*/  = nullref
    .param [6]/*0800009A*/  = nullref
    // Method begins at RVA 0x2210
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*06000090*/ public hidebysig instance class System.Diagnostics.Activity/*02000004*/ 
          StartActivity(valuetype System.Diagnostics.ActivityKind/*0200000E*/ kind,
                        [opt] valuetype System.Diagnostics.ActivityContext/*02000010*/ parentContext,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000027*/ startTime,
                        [opt] string name) cil managed
  // SIG: 20 06 12 10 11 38 11 40 15 12 80 89 01 15 11 61 02 0E 1C 15 12 80 89 01 11 44 11 80 9D 0E
  {
    .custom /*0C0000FC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800009C*/  = nullref
    .param [3]/*0800009D*/  = nullref
    .custom /*0C000102:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [4]/*0800009E*/  = nullref
    .param [5]/*0800009F*/  = nullref
    .param [6]/*080000A0*/  = ""
    .custom /*0C000104:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000105:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CallerMemberNameAttribute/*01000038*/::.ctor() /* 0A000020 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2213
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*06000091*/ public hidebysig static 
          void  AddActivityListener(class System.Diagnostics.ActivityListener/*02000015*/ listener) cil managed
  // SIG: 00 01 01 12 54
  {
    // Method begins at RVA 0x2216
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::AddActivityListener

  .method /*06000092*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2219
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::Dispose

  .property /*1700002A*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.ActivitySource/*0200000A*/::get_Name() /* 06000086 */
  } // end of property ActivitySource::Name
  .property /*1700002B*/ instance callconv(8) string
          Version()
  {
    .custom /*0C0000A2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.ActivitySource/*0200000A*/::get_Version() /* 06000087 */
  } // end of property ActivitySource::Version
  .property /*1700002C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          Tags()
  {
    .custom /*0C0000A5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.ActivitySource/*0200000A*/::get_Tags() /* 06000088 */
  } // end of property ActivitySource::Tags
} // end of class System.Diagnostics.ActivitySource

.class /*0200000B*/ public auto ansi sealed System.Diagnostics.ActivityTraceFlags
       extends [System.Runtime/*23000001*/]System.Enum/*01000033*/
{
  .custom /*0C00003F:0A000021*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000039*/::.ctor() /* 0A000021 */ = ( 01 00 00 00 ) 
  .field /*0400000B*/ public specialname rtspecialname int32 value__
  .field /*0400000C*/ public static literal valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ None = int32(0x00000000)
  .field /*0400000D*/ public static literal valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ Recorded = int32(0x00000001)
} // end of class System.Diagnostics.ActivityTraceFlags

.class /*0200000C*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityTraceId
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000034*/<valuetype System.Diagnostics.ActivityTraceId/*0200000C*/>/*1B000008*/
{
  .custom /*0C000042:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .field /*0400000E*/ private initonly object _dummy
  .field /*0400000F*/ private initonly int32 _dummyPrimitive
  .method /*06000093*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000035*/<uint8> destination) cil managed
  // SIG: 20 01 01 15 11 80 D5 01 05
  {
    // Method begins at RVA 0x221c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivityTraceId::CopyTo

  .method /*06000094*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ 
          CreateFromBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<uint8> idData) cil managed
  // SIG: 00 01 11 30 15 11 80 D9 01 05
  {
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromBytes

  .method /*06000095*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ 
          CreateFromString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<char> idData) cil managed
  // SIG: 00 01 11 30 15 11 80 D9 01 03
  {
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromString

  .method /*06000096*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ 
          CreateFromUtf8String(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<uint8> idData) cil managed
  // SIG: 00 01 11 30 15 11 80 D9 01 05
  {
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromUtf8String

  .method /*06000097*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ 
          CreateRandom() cil managed
  // SIG: 00 00 11 30
  {
    // Method begins at RVA 0x2227
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateRandom

  .method /*06000098*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ traceId) cil managed
  // SIG: 20 01 02 11 30
  {
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::Equals

  .method /*06000099*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000100:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000A7*/ 
    .custom /*0C000106:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000037*/::.ctor(bool) /* 0A00001F */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x222d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::Equals

  .method /*0600009A*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::GetHashCode

  .method /*0600009B*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ traceId1,
                            valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ traceId2) cil managed
  // SIG: 00 02 02 11 30 11 30
  {
    // Method begins at RVA 0x2233
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::op_Equality

  .method /*0600009C*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ traceId1,
                              valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ traceId2) cil managed
  // SIG: 00 02 02 11 30 11 30
  {
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::op_Inequality

  .method /*0600009D*/ public hidebysig instance string 
          ToHexString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000101:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::ToHexString

  .method /*0600009E*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000103:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::ToString

} // end of class System.Diagnostics.ActivityTraceId

.class /*0200000D*/ public auto ansi sealed System.Diagnostics.ActivitySamplingResult
       extends [System.Runtime/*23000001*/]System.Enum/*01000033*/
{
  .field /*04000010*/ public specialname rtspecialname int32 value__
  .field /*04000011*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*0200000D*/ None = int32(0x00000000)
  .field /*04000012*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*0200000D*/ PropagationData = int32(0x00000001)
  .field /*04000013*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*0200000D*/ AllData = int32(0x00000002)
  .field /*04000014*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*0200000D*/ AllDataAndRecorded = int32(0x00000003)
} // end of class System.Diagnostics.ActivitySamplingResult

.class /*0200000E*/ public auto ansi sealed System.Diagnostics.ActivityKind
       extends [System.Runtime/*23000001*/]System.Enum/*01000033*/
{
  .field /*04000015*/ public specialname rtspecialname int32 value__
  .field /*04000016*/ public static literal valuetype System.Diagnostics.ActivityKind/*0200000E*/ Internal = int32(0x00000000)
  .field /*04000017*/ public static literal valuetype System.Diagnostics.ActivityKind/*0200000E*/ Server = int32(0x00000001)
  .field /*04000018*/ public static literal valuetype System.Diagnostics.ActivityKind/*0200000E*/ Client = int32(0x00000002)
  .field /*04000019*/ public static literal valuetype System.Diagnostics.ActivityKind/*0200000E*/ Producer = int32(0x00000003)
  .field /*0400001A*/ public static literal valuetype System.Diagnostics.ActivityKind/*0200000E*/ Consumer = int32(0x00000004)
} // end of class System.Diagnostics.ActivityKind

.class /*0200000F*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityEvent
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
{
  .pack 0
  .size 1
  .custom /*0C000047:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000048:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000049:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .method /*0600009F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::.ctor

  .method /*060000A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000027*/ timestamp,
                               [opt] class System.Diagnostics.ActivityTagsCollection/*02000006*/ tags) cil managed
  // SIG: 20 03 01 0E 11 80 9D 12 18
  {
    .param [2]/*080000AE*/  = nullref
    .param [3]/*080000AF*/  = nullref
    .custom /*0C000108:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::.ctor

  .method /*060000A1*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Name

  .method /*060000A2*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000027*/ 
          get_Timestamp() cil managed
  // SIG: 20 00 11 80 9D
  {
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Timestamp

  .method /*060000A3*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [0]/*080000B0*/ 
    .custom /*0C000109:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Tags

  .method /*060000A4*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000004*//Enumerator`1/*0200002A*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          EnumerateTagObjects() cil managed
  // SIG: 20 00 15 11 80 A8 01 15 11 61 02 0E 1C
  {
    .param [0]/*080000B1*/ 
    .custom /*0C00010A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::EnumerateTagObjects

  .property /*1700002D*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.ActivityEvent/*0200000F*/::get_Name() /* 060000A1 */
  } // end of property ActivityEvent::Name
  .property /*1700002E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000027*/
          Timestamp()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000027*/ System.Diagnostics.ActivityEvent/*0200000F*/::get_Timestamp() /* 060000A2 */
  } // end of property ActivityEvent::Timestamp
  .property /*1700002F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          Tags()
  {
    .custom /*0C0000AA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.ActivityEvent/*0200000F*/::get_Tags() /* 060000A3 */
  } // end of property ActivityEvent::Tags
} // end of class System.Diagnostics.ActivityEvent

.class /*02000010*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityContext
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000034*/<valuetype System.Diagnostics.ActivityContext/*02000010*/>/*1B000009*/
{
  .pack 0
  .size 1
  .custom /*0C00004B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00004C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00004D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*01000034*/<valuetype System.Diagnostics.ActivityContext/*02000010*/>/*1B000009*/
  .custom /*0C000040:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000A5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ traceId,
                               valuetype System.Diagnostics.ActivitySpanId/*02000009*/ spanId,
                               valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ traceFlags,
                               [opt] string traceState,
                               [opt] bool isRemote) cil managed
  // SIG: 20 05 01 11 30 11 24 11 2C 0E 02
  {
    .param [4]/*080000B5*/  = nullref
    .param [5]/*080000B6*/  = bool(false)
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::.ctor

  .method /*060000A6*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x2254
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceId

  .method /*060000A7*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*02000009*/ 
          get_SpanId() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_SpanId

  .method /*060000A8*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ 
          get_TraceFlags() cil managed
  // SIG: 20 00 11 2C
  {
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceFlags

  .method /*060000A9*/ public hidebysig specialname 
          instance string  get_TraceState() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceState

  .method /*060000AA*/ public hidebysig specialname 
          instance bool  get_IsRemote() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2260
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_IsRemote

  .method /*060000AB*/ public hidebysig static 
          bool  TryParse(string traceParent,
                         string traceState,
                         [out] valuetype System.Diagnostics.ActivityContext/*02000010*/& context) cil managed
  // SIG: 00 03 02 0E 0E 10 11 40
  {
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::TryParse

  .method /*060000AC*/ public hidebysig static 
          bool  TryParse(string traceParent,
                         string traceState,
                         bool isRemote,
                         [out] valuetype System.Diagnostics.ActivityContext/*02000010*/& context) cil managed
  // SIG: 00 04 02 0E 0E 02 10 11 40
  {
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::TryParse

  .method /*060000AD*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityContext/*02000010*/ 
          Parse(string traceParent,
                string traceState) cil managed
  // SIG: 00 02 11 40 0E 0E
  {
    .custom /*0C000107:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000BF*/ 
    .custom /*0C00010D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Parse

  .method /*060000AE*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityContext/*02000010*/ left,
                            valuetype System.Diagnostics.ActivityContext/*02000010*/ right) cil managed
  // SIG: 00 02 02 11 40 11 40
  {
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::op_Equality

  .method /*060000AF*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityContext/*02000010*/ left,
                              valuetype System.Diagnostics.ActivityContext/*02000010*/ right) cil managed
  // SIG: 00 02 02 11 40 11 40
  {
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::op_Inequality

  .method /*060000B0*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityContext/*02000010*/ 'value') cil managed
  // SIG: 20 01 02 11 40
  {
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Equals

  .method /*060000B1*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .param [1]/*080000C5*/ 
    .custom /*0C000111:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000037*/::.ctor(bool) /* 0A00001F */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Equals

  .method /*060000B2*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::GetHashCode

  .property /*17000030*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*0200000C*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ System.Diagnostics.ActivityContext/*02000010*/::get_TraceId() /* 060000A6 */
  } // end of property ActivityContext::TraceId
  .property /*17000031*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*02000009*/
          SpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*02000009*/ System.Diagnostics.ActivityContext/*02000010*/::get_SpanId() /* 060000A7 */
  } // end of property ActivityContext::SpanId
  .property /*17000032*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/
          TraceFlags()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceFlags/*0200000B*/ System.Diagnostics.ActivityContext/*02000010*/::get_TraceFlags() /* 060000A8 */
  } // end of property ActivityContext::TraceFlags
  .property /*17000033*/ instance callconv(8) string
          TraceState()
  {
    .get instance string System.Diagnostics.ActivityContext/*02000010*/::get_TraceState() /* 060000A9 */
  } // end of property ActivityContext::TraceState
  .property /*17000034*/ instance callconv(8) bool
          IsRemote()
  {
    .get instance bool System.Diagnostics.ActivityContext/*02000010*/::get_IsRemote() /* 060000AA */
  } // end of property ActivityContext::IsRemote
} // end of class System.Diagnostics.ActivityContext

.class /*02000011*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityLink
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*01000034*/<valuetype System.Diagnostics.ActivityLink/*02000011*/>/*1B00000A*/
{
  .pack 0
  .size 1
  .custom /*0C000050:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .method /*060000B3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Diagnostics.ActivityContext/*02000010*/ context,
                               [opt] class System.Diagnostics.ActivityTagsCollection/*02000006*/ tags) cil managed
  // SIG: 20 02 01 11 40 12 18
  {
    .custom /*0C00010B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000C7*/  = nullref
    // Method begins at RVA 0x227b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::.ctor

  .method /*060000B4*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityContext/*02000010*/ 
          get_Context() cil managed
  // SIG: 20 00 11 40
  {
    // Method begins at RVA 0x227e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::get_Context

  .method /*060000B5*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [0]/*080000C8*/ 
    .custom /*0C000112:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::get_Tags

  .method /*060000B6*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00010C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000C9*/ 
    .custom /*0C000113:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000037*/::.ctor(bool) /* 0A00001F */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::Equals

  .method /*060000B7*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityLink/*02000011*/ 'value') cil managed
  // SIG: 20 01 02 11 44
  {
    // Method begins at RVA 0x2287
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::Equals

  .method /*060000B8*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityLink/*02000011*/ left,
                            valuetype System.Diagnostics.ActivityLink/*02000011*/ right) cil managed
  // SIG: 00 02 02 11 44 11 44
  {
    // Method begins at RVA 0x228a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::op_Equality

  .method /*060000B9*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityLink/*02000011*/ left,
                              valuetype System.Diagnostics.ActivityLink/*02000011*/ right) cil managed
  // SIG: 00 02 02 11 44 11 44
  {
    // Method begins at RVA 0x228d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::op_Inequality

  .method /*060000BA*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2290
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::GetHashCode

  .method /*060000BB*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000004*//Enumerator`1/*0200002A*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          EnumerateTagObjects() cil managed
  // SIG: 20 00 15 11 80 A8 01 15 11 61 02 0E 1C
  {
    .param [0]/*080000CF*/ 
    .custom /*0C000115:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::EnumerateTagObjects

  .property /*17000035*/ instance callconv(8) valuetype System.Diagnostics.ActivityContext/*02000010*/
          Context()
  {
    .get instance valuetype System.Diagnostics.ActivityContext/*02000010*/ System.Diagnostics.ActivityLink/*02000011*/::get_Context() /* 060000B4 */
  } // end of property ActivityLink::Context
  .property /*17000036*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          Tags()
  {
    .custom /*0C0000AF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.ActivityLink/*02000011*/::get_Tags() /* 060000B5 */
  } // end of property ActivityLink::Tags
} // end of class System.Diagnostics.ActivityLink

.class /*02000012*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityCreationOptions`1<T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
{
  .pack 0
  .size 1
  .custom /*0C000053:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000054:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000055:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .param type T /*2A000001*/ 
    .custom /*0C000017:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .method /*060000BC*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivitySource/*0200000A*/ 
          get_Source() cil managed
  // SIG: 20 00 12 28
  {
    // Method begins at RVA 0x2296
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Source

  .method /*060000BD*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2299
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Name

  .method /*060000BE*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityKind/*0200000E*/ 
          get_Kind() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x229c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Kind

  .method /*060000BF*/ public hidebysig specialname 
          instance !T  get_Parent() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x229f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Parent

  .method /*060000C0*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [0]/*080000D0*/ 
    .custom /*0C000116:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Tags

  .method /*060000C1*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> 
          get_Links() cil managed
  // SIG: 20 00 15 12 80 89 01 11 44
  {
    .custom /*0C00010E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Links

  .method /*060000C2*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivityTagsCollection/*02000006*/ 
          get_SamplingTags() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x22a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_SamplingTags

  .method /*060000C3*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x22ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_TraceId

  .method /*060000C4*/ public hidebysig specialname 
          instance string  get_TraceState() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00010F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_TraceState

  .method /*060000C5*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*0100002C*/) 
          set_TraceState(string 'value') cil managed
  // SIG: 20 01 1F 80 B1 01 0E
  {
    .custom /*0C000110:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::set_TraceState

  .property /*17000037*/ instance callconv(8) class System.Diagnostics.ActivitySource/*0200000A*/
          Source()
  {
    .get instance class System.Diagnostics.ActivitySource/*0200000A*/ System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_Source() /* 060000BC */
  } // end of property ActivityCreationOptions`1::Source
  .property /*17000038*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_Name() /* 060000BD */
  } // end of property ActivityCreationOptions`1::Name
  .property /*17000039*/ instance callconv(8) valuetype System.Diagnostics.ActivityKind/*0200000E*/
          Kind()
  {
    .get instance valuetype System.Diagnostics.ActivityKind/*0200000E*/ System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_Kind() /* 060000BE */
  } // end of property ActivityCreationOptions`1::Kind
  .property /*1700003A*/ instance callconv(8) !T
          Parent()
  {
    .get instance !T System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_Parent() /* 060000BF */
  } // end of property ActivityCreationOptions`1::Parent
  .property /*1700003B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          Tags()
  {
    .custom /*0C0000B4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_Tags() /* 060000C0 */
  } // end of property ActivityCreationOptions`1::Tags
  .property /*1700003C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/>
          Links()
  {
    .custom /*0C0000B5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.ActivityLink/*02000011*/> System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_Links() /* 060000C1 */
  } // end of property ActivityCreationOptions`1::Links
  .property /*1700003D*/ instance callconv(8) class System.Diagnostics.ActivityTagsCollection/*02000006*/
          SamplingTags()
  {
    .get instance class System.Diagnostics.ActivityTagsCollection/*02000006*/ System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_SamplingTags() /* 060000C2 */
  } // end of property ActivityCreationOptions`1::SamplingTags
  .property /*1700003E*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*0200000C*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*0200000C*/ System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_TraceId() /* 060000C3 */
  } // end of property ActivityCreationOptions`1::TraceId
  .property /*1700003F*/ instance callconv(8) string
          TraceState()
  {
    .custom /*0C0000B7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.ActivityCreationOptions`1/*02000012*/::get_TraceState() /* 060000C4 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*0100002C*/) System.Diagnostics.ActivityCreationOptions`1/*02000012*/::set_TraceState(string) /* 060000C5 */
  } // end of property ActivityCreationOptions`1::TraceState
} // end of class System.Diagnostics.ActivityCreationOptions`1

.class /*02000013*/ public auto ansi sealed System.Diagnostics.SampleActivity`1<T>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100003A*/
{
  .param type T /*2A000002*/ 
    .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  .method /*060000C6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method SampleActivity`1::.ctor

  .method /*060000C7*/ public hidebysig newslot virtual 
          instance valuetype System.Diagnostics.ActivitySamplingResult/*0200000D*/ 
          Invoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000012*/<!T>& options) runtime managed
  // SIG: 20 01 11 34 10 15 11 48 01 13 00
  {
    .param [1]/*080000D4*/ 
    .custom /*0C000117:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::Invoke

  .method /*060000C8*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ 
          BeginInvoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000012*/<!T>& options,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100003C*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 80 ED 10 15 11 48 01 13 00 12 80 F1 1C
  {
    .param [1]/*080000D5*/ 
    .custom /*0C000118:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::BeginInvoke

  .method /*060000C9*/ public hidebysig newslot virtual 
          instance valuetype System.Diagnostics.ActivitySamplingResult/*0200000D*/ 
          EndInvoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000012*/<!T>& options,
                    class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ result) runtime managed
  // SIG: 20 02 11 34 10 15 11 48 01 13 00 12 80 ED
  {
    .param [1]/*080000D8*/ 
    .custom /*0C000119:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::EndInvoke

} // end of class System.Diagnostics.SampleActivity`1

.class /*02000014*/ public auto ansi sealed System.Diagnostics.ExceptionRecorder
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100003A*/
{
  .method /*060000CA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ExceptionRecorder::.ctor

  .method /*060000CB*/ public hidebysig newslot virtual 
          instance void  Invoke(class System.Diagnostics.Activity/*02000004*/ activity,
                                class [System.Runtime/*23000001*/]System.Exception/*01000025*/ exception,
                                valuetype System.Diagnostics.TagList/*02000017*/& tags) runtime managed
  // SIG: 20 03 01 12 10 12 80 95 10 11 5C
  {
    .custom /*0C000114:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  } // end of method ExceptionRecorder::Invoke

  .method /*060000CC*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ 
          BeginInvoke(class System.Diagnostics.Activity/*02000004*/ activity,
                      class [System.Runtime/*23000001*/]System.Exception/*01000025*/ exception,
                      valuetype System.Diagnostics.TagList/*02000017*/& tags,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100003C*/ callback,
                      object 'object') runtime managed
  // SIG: 20 05 12 80 ED 12 10 12 80 95 10 11 5C 12 80 F1 1C
  {
    .param [1]/*080000DF*/ 
    .custom /*0C00011D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000E0*/ 
    .custom /*0C00011F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method ExceptionRecorder::BeginInvoke

  .method /*060000CD*/ public hidebysig newslot virtual 
          instance void  EndInvoke(valuetype System.Diagnostics.TagList/*02000017*/& tags,
                                   class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ result) runtime managed
  // SIG: 20 02 01 10 11 5C 12 80 ED
  {
  } // end of method ExceptionRecorder::EndInvoke

} // end of class System.Diagnostics.ExceptionRecorder

.class /*02000015*/ public auto ansi sealed beforefieldinit System.Diagnostics.ActivityListener
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
{
  .custom /*0C000062:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000063:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
  .custom /*0C000044:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000CE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22b4
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityListener::.ctor

  .method /*060000CF*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/> 
          get_ActivityStarted() cil managed
  // SIG: 20 00 15 12 80 F5 01 12 10
  {
    .param [0]/*080000E6*/ 
    .custom /*0C000121:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ActivityStarted

  .method /*060000D0*/ public hidebysig specialname 
          instance void  set_ActivityStarted(class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 F5 01 12 10
  {
    .param [1]/*080000E7*/ 
    .custom /*0C000123:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ActivityStarted

  .method /*060000D1*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/> 
          get_ActivityStopped() cil managed
  // SIG: 20 00 15 12 80 F5 01 12 10
  {
    .param [0]/*080000E8*/ 
    .custom /*0C000125:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ActivityStopped

  .method /*060000D2*/ public hidebysig specialname 
          instance void  set_ActivityStopped(class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 F5 01 12 10
  {
    .param [1]/*080000E9*/ 
    .custom /*0C000127:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ActivityStopped

  .method /*060000D3*/ public hidebysig specialname 
          instance class System.Diagnostics.ExceptionRecorder/*02000014*/ 
          get_ExceptionRecorder() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ExceptionRecorder

  .method /*060000D4*/ public hidebysig specialname 
          instance void  set_ExceptionRecorder(class System.Diagnostics.ExceptionRecorder/*02000014*/ 'value') cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ExceptionRecorder

  .method /*060000D5*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100003E*/<class System.Diagnostics.ActivitySource/*0200000A*/,bool> 
          get_ShouldListenTo() cil managed
  // SIG: 20 00 15 12 80 F9 02 12 28 02
  {
    .param [0]/*080000EB*/ 
    .custom /*0C000129:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ShouldListenTo

  .method /*060000D6*/ public hidebysig specialname 
          instance void  set_ShouldListenTo(class [System.Runtime/*23000001*/]System.Func`2/*0100003E*/<class System.Diagnostics.ActivitySource/*0200000A*/,bool> 'value') cil managed
  // SIG: 20 01 01 15 12 80 F9 02 12 28 02
  {
    .param [1]/*080000EC*/ 
    .custom /*0C00012A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ShouldListenTo

  .method /*060000D7*/ public hidebysig specialname 
          instance class System.Diagnostics.SampleActivity`1/*02000013*/<string> 
          get_SampleUsingParentId() cil managed
  // SIG: 20 00 15 12 4C 01 0E
  {
    .param [0]/*080000ED*/ 
    .custom /*0C00012C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_SampleUsingParentId

  .method /*060000D8*/ public hidebysig specialname 
          instance void  set_SampleUsingParentId(class System.Diagnostics.SampleActivity`1/*02000013*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 4C 01 0E
  {
    .param [1]/*080000EE*/ 
    .custom /*0C00012D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_SampleUsingParentId

  .method /*060000D9*/ public hidebysig specialname 
          instance class System.Diagnostics.SampleActivity`1/*02000013*/<valuetype System.Diagnostics.ActivityContext/*02000010*/> 
          get_Sample() cil managed
  // SIG: 20 00 15 12 4C 01 11 40
  {
    // Method begins at RVA 0x22db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_Sample

  .method /*060000DA*/ public hidebysig specialname 
          instance void  set_Sample(class System.Diagnostics.SampleActivity`1/*02000013*/<valuetype System.Diagnostics.ActivityContext/*02000010*/> 'value') cil managed
  // SIG: 20 01 01 15 12 4C 01 11 40
  {
    // Method begins at RVA 0x22de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_Sample

  .method /*060000DB*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::Dispose

  .property /*17000040*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/>
          ActivityStarted()
  {
    .custom /*0C0000B9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/> System.Diagnostics.ActivityListener/*02000015*/::get_ActivityStarted() /* 060000CF */
    .set instance void System.Diagnostics.ActivityListener/*02000015*/::set_ActivityStarted(class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/>) /* 060000D0 */
  } // end of property ActivityListener::ActivityStarted
  .property /*17000041*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/>
          ActivityStopped()
  {
    .custom /*0C0000BB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/> System.Diagnostics.ActivityListener/*02000015*/::get_ActivityStopped() /* 060000D1 */
    .set instance void System.Diagnostics.ActivityListener/*02000015*/::set_ActivityStopped(class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<class System.Diagnostics.Activity/*02000004*/>) /* 060000D2 */
  } // end of property ActivityListener::ActivityStopped
  .property /*17000042*/ instance callconv(8) class System.Diagnostics.ExceptionRecorder/*02000014*/
          ExceptionRecorder()
  {
    .get instance class System.Diagnostics.ExceptionRecorder/*02000014*/ System.Diagnostics.ActivityListener/*02000015*/::get_ExceptionRecorder() /* 060000D3 */
    .set instance void System.Diagnostics.ActivityListener/*02000015*/::set_ExceptionRecorder(class System.Diagnostics.ExceptionRecorder/*02000014*/) /* 060000D4 */
  } // end of property ActivityListener::ExceptionRecorder
  .property /*17000043*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100003E*/<class System.Diagnostics.ActivitySource/*0200000A*/,bool>
          ShouldListenTo()
  {
    .custom /*0C0000BE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100003E*/<class System.Diagnostics.ActivitySource/*0200000A*/,bool> System.Diagnostics.ActivityListener/*02000015*/::get_ShouldListenTo() /* 060000D5 */
    .set instance void System.Diagnostics.ActivityListener/*02000015*/::set_ShouldListenTo(class [System.Runtime/*23000001*/]System.Func`2/*0100003E*/<class System.Diagnostics.ActivitySource/*0200000A*/,bool>) /* 060000D6 */
  } // end of property ActivityListener::ShouldListenTo
  .property /*17000044*/ instance callconv(8) class System.Diagnostics.SampleActivity`1/*02000013*/<string>
          SampleUsingParentId()
  {
    .custom /*0C0000C0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class System.Diagnostics.SampleActivity`1/*02000013*/<string> System.Diagnostics.ActivityListener/*02000015*/::get_SampleUsingParentId() /* 060000D7 */
    .set instance void System.Diagnostics.ActivityListener/*02000015*/::set_SampleUsingParentId(class System.Diagnostics.SampleActivity`1/*02000013*/<string>) /* 060000D8 */
  } // end of property ActivityListener::SampleUsingParentId
  .property /*17000045*/ instance callconv(8) class System.Diagnostics.SampleActivity`1/*02000013*/<valuetype System.Diagnostics.ActivityContext/*02000010*/>
          Sample()
  {
    .get instance class System.Diagnostics.SampleActivity`1/*02000013*/<valuetype System.Diagnostics.ActivityContext/*02000010*/> System.Diagnostics.ActivityListener/*02000015*/::get_Sample() /* 060000D9 */
    .set instance void System.Diagnostics.ActivityListener/*02000015*/::set_Sample(class System.Diagnostics.SampleActivity`1/*02000013*/<valuetype System.Diagnostics.ActivityContext/*02000010*/>) /* 060000DA */
  } // end of property ActivityListener::Sample
} // end of class System.Diagnostics.ActivityListener

.class /*02000016*/ public abstract auto ansi beforefieldinit System.Diagnostics.DistributedContextPropagator
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000065:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000066:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .class /*0200002C*/ auto ansi sealed nested public PropagatorGetterCallback
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100003A*/
  {
    .custom /*0C0000A4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .method /*06000189*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method PropagatorGetterCallback::.ctor

    .method /*0600018A*/ public hidebysig newslot virtual 
            instance void  Invoke(object carrier,
                                  string fieldName,
                                  [out] string& fieldValue,
                                  [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<string>& fieldValues) runtime managed
    // SIG: 20 04 01 1C 0E 10 0E 10 15 12 80 89 01 0E
    {
      .custom /*0C0001A8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
      .param [2]/*0800026C*/ 
      .custom /*0C00022E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
      .param [4]/*0800026E*/ 
      .custom /*0C00022F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::Invoke

    .method /*0600018B*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ 
            BeginInvoke(object carrier,
                        string fieldName,
                        [out] string& fieldValue,
                        [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<string>& fieldValues,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*0100003C*/ callback,
                        object 'object') runtime managed
    // SIG: 20 06 12 80 ED 1C 0E 10 0E 10 15 12 80 89 01 0E 12 80 F1 1C
    {
      .param [1]/*0800026F*/ 
      .custom /*0C000230:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
      .param [2]/*08000270*/ 
      .custom /*0C000231:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
      .param [3]/*08000271*/ 
      .custom /*0C000232:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
      .param [4]/*08000272*/ 
      .custom /*0C000233:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::BeginInvoke

    .method /*0600018C*/ public hidebysig newslot virtual 
            instance void  EndInvoke([out] string& fieldValue,
                                     [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<string>& fieldValues,
                                     class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ result) runtime managed
    // SIG: 20 03 01 10 0E 10 15 12 80 89 01 0E 12 80 ED
    {
      .param [1]/*08000275*/ 
      .custom /*0C000234:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
      .param [2]/*08000276*/ 
      .custom /*0C000235:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::EndInvoke

  } // end of class PropagatorGetterCallback

  .class /*0200002D*/ auto ansi sealed nested public PropagatorSetterCallback
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100003A*/
  {
    .custom /*0C0000A7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .method /*0600018D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method PropagatorSetterCallback::.ctor

    .method /*0600018E*/ public hidebysig newslot virtual 
            instance void  Invoke(object carrier,
                                  string fieldName,
                                  string fieldValue) runtime managed
    // SIG: 20 03 01 1C 0E 0E
    {
      .custom /*0C0001AC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
      .param [1]/*0800027A*/ 
      .custom /*0C000236:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    } // end of method PropagatorSetterCallback::Invoke

    .method /*0600018F*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ 
            BeginInvoke(object carrier,
                        string fieldName,
                        string fieldValue,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*0100003C*/ callback,
                        object 'object') runtime managed
    // SIG: 20 05 12 80 ED 1C 0E 0E 12 80 F1 1C
    {
      .param [1]/*0800027D*/ 
      .custom /*0C000237:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
      .param [2]/*0800027E*/ 
      .custom /*0C000238:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
      .param [3]/*0800027F*/ 
      .custom /*0C000239:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    } // end of method PropagatorSetterCallback::BeginInvoke

    .method /*06000190*/ public hidebysig newslot virtual 
            instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ result) runtime managed
    // SIG: 20 01 01 12 80 ED
    {
    } // end of method PropagatorSetterCallback::EndInvoke

  } // end of class PropagatorSetterCallback

  .field /*0400001B*/ private static class System.Diagnostics.DistributedContextPropagator/*02000016*/ '<Current>k__BackingField'
  .custom /*0C000076:0A000022*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100003F*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
  .method /*060000DC*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000040*/<string> 
          get_Fields() cil managed
  // SIG: 20 00 15 12 81 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::get_Fields

  .method /*060000DD*/ public hidebysig newslot abstract virtual 
          instance void  Inject(class System.Diagnostics.Activity/*02000004*/ activity,
                                object carrier,
                                class System.Diagnostics.DistributedContextPropagator/*02000016*//PropagatorSetterCallback/*0200002D*/ setter) cil managed
  // SIG: 20 03 01 12 10 1C 12 80 B4
  {
    .custom /*0C00011A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::Inject

  .method /*060000DE*/ public hidebysig newslot abstract virtual 
          instance void  ExtractTraceIdAndState(object carrier,
                                                class System.Diagnostics.DistributedContextPropagator/*02000016*//PropagatorGetterCallback/*0200002C*/ getter,
                                                [out] string& traceId,
                                                [out] string& traceState) cil managed
  // SIG: 20 04 01 1C 12 80 B0 10 0E 10 0E
  {
    .custom /*0C00011B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::ExtractTraceIdAndState

  .method /*060000DF*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,string>> 
          ExtractBaggage(object carrier,
                         class System.Diagnostics.DistributedContextPropagator/*02000016*//PropagatorGetterCallback/*0200002C*/ getter) cil managed
  // SIG: 20 02 15 12 80 89 01 15 11 61 02 0E 0E 1C 12 80 B0
  {
    .custom /*0C00011C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000F7*/ 
    .custom /*0C000133:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::ExtractBaggage

  .method /*060000E0*/ public hidebysig specialname static 
          class System.Diagnostics.DistributedContextPropagator/*02000016*/ 
          get_Current() cil managed
  // SIG: 00 00 12 58
  {
    .custom /*0C00011E:0A000022*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100003F*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22e4
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 7E   | (04)00001B       */ ldsfld     class System.Diagnostics.DistributedContextPropagator/*02000016*/ System.Diagnostics.DistributedContextPropagator/*02000016*/::'<Current>k__BackingField' /* 0400001B */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::get_Current

  .method /*060000E1*/ public hidebysig specialname static 
          void  set_Current(class System.Diagnostics.DistributedContextPropagator/*02000016*/ 'value') cil managed
  // SIG: 00 01 01 12 58
  {
    .custom /*0C000120:0A000022*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100003F*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22eb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 80   | (04)00001B       */ stsfld     class System.Diagnostics.DistributedContextPropagator/*02000016*/ System.Diagnostics.DistributedContextPropagator/*02000016*/::'<Current>k__BackingField' /* 0400001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::set_Current

  .method /*060000E2*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*02000016*/ 
          CreateDefaultPropagator() cil managed
  // SIG: 00 00 12 58
  {
    // Method begins at RVA 0x22f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreateDefaultPropagator

  .method /*060000E3*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*02000016*/ 
          CreatePassThroughPropagator() cil managed
  // SIG: 00 00 12 58
  {
    // Method begins at RVA 0x22f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreatePassThroughPropagator

  .method /*060000E4*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*02000016*/ 
          CreateNoOutputPropagator() cil managed
  // SIG: 00 00 12 58
  {
    // Method begins at RVA 0x22f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreateNoOutputPropagator

  .method /*060000E5*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22fc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::.ctor

  .property /*17000046*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000040*/<string>
          Fields()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000040*/<string> System.Diagnostics.DistributedContextPropagator/*02000016*/::get_Fields() /* 060000DC */
  } // end of property DistributedContextPropagator::Fields
  .property /*17000047*/ callconv(8) class System.Diagnostics.DistributedContextPropagator/*02000016*/
          Current()
  {
    .get class System.Diagnostics.DistributedContextPropagator/*02000016*/ System.Diagnostics.DistributedContextPropagator/*02000016*/::get_Current() /* 060000E0 */
    .set void System.Diagnostics.DistributedContextPropagator/*02000016*/::set_Current(class System.Diagnostics.DistributedContextPropagator/*02000016*/) /* 060000E1 */
  } // end of property DistributedContextPropagator::Current
} // end of class System.Diagnostics.DistributedContextPropagator

.class /*02000017*/ public sequential ansi sealed beforefieldinit System.Diagnostics.TagList
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000041*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B00000B*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000005*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000003*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002F*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000042*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B00000C*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000040*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B00000D*/
{
  .custom /*0C00006B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100002D*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000041*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B00000B*/
  .custom /*0C000046:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*01000032*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000005*/
  .custom /*0C00004A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000003*/
  .custom /*0C00004F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000042*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B00000C*/
  .custom /*0C000057:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000040*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B00000D*/
  .custom /*0C00005C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .class /*0200002E*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000006*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000030*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
  {
    .pack 0
    .size 1
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>/*1B000006*/
    .custom /*0C000073:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .method /*06000191*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> 
            get_Current() cil managed
    // SIG: 20 00 15 11 61 02 0E 1C
    {
      .param [0]/*08000283*/ 
      .custom /*0C00023A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      // Method begins at RVA 0x25c8
      // Code size       2 (0x2)
      .maxstack  1
      .locals /*11000001*/ init (valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> V_0)
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*06000192*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000030*/::get_Current /*01000030::0A00001D*/ 
      // Method begins at RVA 0x25d6
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*06000193*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x25d9
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Dispose

    .method /*06000194*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x25dc
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*06000195*/ public hidebysig newslot virtual final 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x25df
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Reset

    .property /*17000064*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>
            Current()
    {
      .custom /*0C0000E1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> System.Diagnostics.TagList/*02000017*//Enumerator/*0200002E*/::get_Current() /* 06000191 */
    } // end of property Enumerator::Current
    .property /*17000065*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .custom /*0C0000E2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
      .get instance object System.Diagnostics.TagList/*02000017*//Enumerator/*0200002E*/::System.Collections.IEnumerator.get_Current() /* 06000192 */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .method /*060000E6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tagList) cil managed
  // SIG: 20 01 01 15 11 80 D9 01 15 11 61 02 0E 1C
  {
    .param [1]/*080000FB*/ 
    .custom /*0C000134:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ParamCollectionAttribute/*01000043*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000135:0A000024*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000044*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000136:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2304
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* FE15 | (02)000017       */ initobj    System.Diagnostics.TagList/*02000017*/
    IL_0007:  /* 14   |                  */ ldnull
    IL_0008:  /* 7A   |                  */ throw
  } // end of method TagList::.ctor

  .method /*060000E7*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000122:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x230e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_Count

  .method /*060000E8*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000124:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_IsReadOnly

  .method /*060000E9*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> 
          get_Item(int32 index) cil managed
  // SIG: 20 01 15 11 61 02 0E 1C 08
  {
    .custom /*0C000126:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .param [0]/*080000FC*/ 
    .custom /*0C000137:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2314
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_Item

  .method /*060000EA*/ public hidebysig newslot specialname virtual final 
          instance void  set_Item(int32 index,
                                  valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> 'value') cil managed
  // SIG: 20 02 01 08 15 11 61 02 0E 1C
  {
    .param [2]/*080000FF*/ 
    .custom /*0C000138:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2317
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::set_Item

  .method /*060000EB*/ public hidebysig instance void 
          Add(string key,
              object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C000128:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000101*/ 
    .custom /*0C000139:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x231a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Add

  .method /*060000EC*/ public hidebysig newslot virtual final 
          instance void  Add(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag) cil managed
  // SIG: 20 01 01 15 11 61 02 0E 1C
  {
    .param [1]/*08000102*/ 
    .custom /*0C00013A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x231d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Add

  .method /*060000ED*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000035*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 01 01 15 11 80 D5 01 15 11 61 02 0E 1C
  {
    .custom /*0C00012B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .param [1]/*08000103*/ 
    .custom /*0C00013B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2320
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::CopyTo

  .method /*060000EE*/ public hidebysig newslot virtual final 
          instance void  Insert(int32 index,
                                valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> item) cil managed
  // SIG: 20 02 01 08 15 11 61 02 0E 1C
  {
    .param [2]/*08000105*/ 
    .custom /*0C00013C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2323
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Insert

  .method /*060000EF*/ public hidebysig newslot virtual final 
          instance void  RemoveAt(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2326
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::RemoveAt

  .method /*060000F0*/ public hidebysig newslot virtual final 
          instance void  Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2329
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Clear

  .method /*060000F1*/ public hidebysig newslot virtual final 
          instance bool  Contains(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 61 02 0E 1C
  {
    .custom /*0C00012E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .param [1]/*08000107*/ 
    .custom /*0C00013D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x232c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Contains

  .method /*060000F2*/ public hidebysig newslot virtual final 
          instance void  CopyTo(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>[] 'array',
                                int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 61 02 0E 1C 08
  {
    .custom /*0C00012F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .param [1]/*08000108*/ 
    .custom /*0C00013E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x232f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::CopyTo

  .method /*060000F3*/ public hidebysig newslot virtual final 
          instance bool  Remove(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 61 02 0E 1C
  {
    .param [1]/*0800010A*/ 
    .custom /*0C00013F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2332
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Remove

  .method /*060000F4*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 80 B9 01 15 11 61 02 0E 1C
  {
    .custom /*0C000130:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .param [0]/*0800010B*/ 
    .custom /*0C000140:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2335
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::GetEnumerator

  .method /*060000F5*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000030*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 C1
  {
    .custom /*0C000131:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002F*/::GetEnumerator /*0100002F::0A00001C*/ 
    // Method begins at RVA 0x2338
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::System.Collections.IEnumerable.GetEnumerator

  .method /*060000F6*/ public hidebysig newslot virtual final 
          instance int32  IndexOf(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> item) cil managed
  // SIG: 20 01 08 15 11 61 02 0E 1C
  {
    .custom /*0C000132:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    .param [1]/*0800010C*/ 
    .custom /*0C000141:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x233b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::IndexOf

  .property /*17000048*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Diagnostics.TagList/*02000017*/::get_Count() /* 060000E7 */
  } // end of property TagList::Count
  .property /*17000049*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Diagnostics.TagList/*02000017*/::get_IsReadOnly() /* 060000E8 */
  } // end of property TagList::IsReadOnly
  .property /*1700004A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>
          Item(int32)
  {
    .custom /*0C0000C3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> System.Diagnostics.TagList/*02000017*/::get_Item(int32) /* 060000E9 */
    .set instance void System.Diagnostics.TagList/*02000017*/::set_Item(int32,
                                                                        valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>) /* 060000EA */
  } // end of property TagList::Item
} // end of class System.Diagnostics.TagList

.class /*02000018*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.Counter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/
{
  .method /*060000F7*/ public hidebysig instance void 
          Add(!T delta) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x233e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F8*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 61 02 0E 1C
  {
    .param [2]/*0800010F*/ 
    .custom /*0C000142:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2341
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F9*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*08000111*/ 
    .custom /*0C000143:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000112*/ 
    .custom /*0C000144:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2344
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000FA*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*08000114*/ 
    .custom /*0C000145:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000115*/ 
    .custom /*0C000146:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000116*/ 
    .custom /*0C000147:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2347
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000FB*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 D9 01 15 11 61 02 0E 1C
  {
    .param [2]/*08000118*/ 
    .custom /*0C00014A:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ParamCollectionAttribute/*01000043*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00014B:0A000024*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000044*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00014C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x234a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000FC*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 61 02 0E 1C
  {
    .param [2]/*0800011A*/ 
    .custom /*0C00014D:0A000025*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000045*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    .custom /*0C00014E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x234d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000FD*/ public hidebysig instance void 
          Add(!T delta,
              [in] valuetype System.Diagnostics.TagList/*02000017*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 5C
  {
    .param [2]/*0800011C*/ 
    .custom /*0C000150:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2350
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000FE*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    // Method begins at RVA 0x2353
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000027       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string) /* 0A000027 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Counter`1::.ctor

} // end of class System.Diagnostics.Metrics.Counter`1

.class /*02000019*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.Gauge`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/
{
  .method /*060000FF*/ public hidebysig instance void 
          Record(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2361
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Gauge`1::Record

  .method /*06000100*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 61 02 0E 1C
  {
    .param [2]/*08000123*/ 
    .custom /*0C000153:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2364
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Gauge`1::Record

  .method /*06000101*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*08000125*/ 
    .custom /*0C000157:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000126*/ 
    .custom /*0C00015A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2367
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Gauge`1::Record

  .method /*06000102*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*08000128*/ 
    .custom /*0C00015C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000129*/ 
    .custom /*0C00015D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*0800012A*/ 
    .custom /*0C00015E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x236a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Gauge`1::Record

  .method /*06000103*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 D9 01 15 11 61 02 0E 1C
  {
    .param [2]/*0800012C*/ 
    .custom /*0C00015F:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ParamCollectionAttribute/*01000043*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C000160:0A000024*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000044*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000161:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x236d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Gauge`1::Record

  .method /*06000104*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 61 02 0E 1C
  {
    .param [2]/*0800012E*/ 
    .custom /*0C000162:0A000025*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000045*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    .custom /*0C000163:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2370
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Gauge`1::Record

  .method /*06000105*/ public hidebysig instance void 
          Record(!T 'value',
                 [in] valuetype System.Diagnostics.TagList/*02000017*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 5C
  {
    .param [2]/*08000130*/ 
    .custom /*0C000164:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2373
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Gauge`1::Record

  .method /*06000106*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 05 01 12 80 88 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    // Method begins at RVA 0x2376
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 0E   | 05               */ ldarg.s    tags
    IL_0008:  /* 28   | (0A)000028       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>) /* 0A000028 */
    IL_000d:  /* 14   |                  */ ldnull
    IL_000e:  /* 7A   |                  */ throw
  } // end of method Gauge`1::.ctor

} // end of class System.Diagnostics.Metrics.Gauge`1

.class /*0200001A*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.UpDownCounter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/
{
  .method /*06000107*/ public hidebysig instance void 
          Add(!T delta) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2386
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*06000108*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 61 02 0E 1C
  {
    .param [2]/*08000138*/ 
    .custom /*0C000165:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2389
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*06000109*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*0800013A*/ 
    .custom /*0C000166:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*0800013B*/ 
    .custom /*0C000167:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x238c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*0600010A*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*0800013D*/ 
    .custom /*0C000168:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*0800013E*/ 
    .custom /*0C00016A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*0800013F*/ 
    .custom /*0C00016B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x238f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*0600010B*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 D9 01 15 11 61 02 0E 1C
  {
    .param [2]/*08000141*/ 
    .custom /*0C00016C:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ParamCollectionAttribute/*01000043*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00016D:0A000024*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000044*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C00016E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2392
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*0600010C*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 61 02 0E 1C
  {
    .param [2]/*08000143*/ 
    .custom /*0C000171:0A000025*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000045*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    .custom /*0C000172:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2395
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*0600010D*/ public hidebysig instance void 
          Add(!T delta,
              [in] valuetype System.Diagnostics.TagList/*02000017*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 5C
  {
    .param [2]/*08000145*/ 
    .custom /*0C000174:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2398
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*0600010E*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    // Method begins at RVA 0x239b
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000027       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string) /* 0A000027 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::.ctor

} // end of class System.Diagnostics.Metrics.UpDownCounter`1

.class /*0200001B*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.Histogram`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/
{
  .method /*0600010F*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    // Method begins at RVA 0x23a9
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000027       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string) /* 0A000027 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Histogram`1::.ctor

  .method /*06000110*/ public hidebysig instance void 
          Record(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x23b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000111*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 61 02 0E 1C
  {
    .param [2]/*08000150*/ 
    .custom /*0C000175:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000112*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*08000152*/ 
    .custom /*0C000176:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000153*/ 
    .custom /*0C000177:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000113*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*08000155*/ 
    .custom /*0C000178:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000156*/ 
    .custom /*0C000179:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000157*/ 
    .custom /*0C00017A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000114*/ public hidebysig instance void 
          Record(!T 'value',
                 [in] valuetype System.Diagnostics.TagList/*02000017*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 5C
  {
    .param [2]/*08000159*/ 
    .custom /*0C00017B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000115*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 D9 01 15 11 61 02 0E 1C
  {
    .param [2]/*0800015B*/ 
    .custom /*0C00017E:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ParamCollectionAttribute/*01000043*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C00017F:0A000024*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000044*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C000180:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000116*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 61 02 0E 1C
  {
    .param [2]/*0800015D*/ 
    .custom /*0C000182:0A000025*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000045*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    .custom /*0C000183:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x23c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

} // end of class System.Diagnostics.Metrics.Histogram`1

.class /*0200001C*/ interface public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.IMeterFactory
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
{
  .method /*06000117*/ public hidebysig newslot abstract virtual 
          instance class System.Diagnostics.Metrics.Meter/*02000022*/ 
          Create(class System.Diagnostics.Metrics.MeterOptions/*02000025*/ options) cil managed
  // SIG: 20 01 12 80 88 12 80 94
  {
    .custom /*0C000148:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IMeterFactory::Create

} // end of class System.Diagnostics.Metrics.IMeterFactory

.class /*0200001D*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.Instrument
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00007C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .field /*0400001C*/ private initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> '<Tags>k__BackingField'
  .custom /*0C000079:0A000022*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100003F*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
  .method /*06000118*/ public hidebysig specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000149:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Description

  .method /*06000119*/ public hidebysig specialname 
          instance bool  get_Enabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Enabled

  .method /*0600011A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name) cil managed
  // SIG: 20 02 01 12 80 88 0E
  {
    // Method begins at RVA 0x23d2
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 14   |                  */ ldnull
    IL_0005:  /* 14   |                  */ ldnull
    IL_0006:  /* 28   | (06)00011C       */ call       instance void System.Diagnostics.Metrics.Instrument/*0200001D*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>) /* 0600011C */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Instrument::.ctor

  .method /*0600011B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    .custom /*0C00014F:0A000019*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100002A*/) /* 0A000019 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [3]/*08000163*/ 
    .custom /*0C000188:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000164*/ 
    .custom /*0C000189:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e0
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 28   | (06)00011C       */ call       instance void System.Diagnostics.Metrics.Instrument/*0200001D*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>) /* 0600011C */
    IL_000c:  /* 14   |                  */ ldnull
    IL_000d:  /* 7A   |                  */ throw
  } // end of method Instrument::.ctor

  .method /*0600011C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               [opt] string unit,
                               [opt] string description,
                               [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 05 01 12 80 88 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [3]/*08000167*/  = nullref
    .custom /*0C00018B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000168*/  = nullref
    .custom /*0C00018C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000169*/  = nullref
    .custom /*0C00018D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x23ef
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Instrument::.ctor

  .method /*0600011D*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsObservable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_IsObservable

  .method /*0600011E*/ public hidebysig specialname 
          instance class System.Diagnostics.Metrics.Meter/*02000022*/ 
          get_Meter() cil managed
  // SIG: 20 00 12 80 88
  {
    // Method begins at RVA 0x23fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Meter

  .method /*0600011F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Name

  .method /*06000120*/ family hidebysig instance void 
          Publish() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2401
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::Publish

  .method /*06000121*/ public hidebysig specialname 
          instance string  get_Unit() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000151:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2404
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Unit

  .method /*06000122*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .custom /*0C000152:0A000022*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*0100003F*/::.ctor() /* 0A000022 */ = ( 01 00 00 00 ) 
    .param [0]/*0800016A*/ 
    .custom /*0C00018E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2407
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 7B   | (04)00001C       */ ldfld      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.Metrics.Instrument/*0200001D*/::'<Tags>k__BackingField' /* 0400001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Instrument::get_Tags

  .property /*1700004B*/ instance callconv(8) string
          Description()
  {
    .custom /*0C0000C4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001D*/::get_Description() /* 06000118 */
  } // end of property Instrument::Description
  .property /*1700004C*/ instance callconv(8) bool
          Enabled()
  {
    .get instance bool System.Diagnostics.Metrics.Instrument/*0200001D*/::get_Enabled() /* 06000119 */
  } // end of property Instrument::Enabled
  .property /*1700004D*/ instance callconv(8) bool
          IsObservable()
  {
    .get instance bool System.Diagnostics.Metrics.Instrument/*0200001D*/::get_IsObservable() /* 0600011D */
  } // end of property Instrument::IsObservable
  .property /*1700004E*/ instance callconv(8) class System.Diagnostics.Metrics.Meter/*02000022*/
          Meter()
  {
    .get instance class System.Diagnostics.Metrics.Meter/*02000022*/ System.Diagnostics.Metrics.Instrument/*0200001D*/::get_Meter() /* 0600011E */
  } // end of property Instrument::Meter
  .property /*1700004F*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001D*/::get_Name() /* 0600011F */
  } // end of property Instrument::Name
  .property /*17000050*/ instance callconv(8) string
          Unit()
  {
    .custom /*0C0000C7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001D*/::get_Unit() /* 06000121 */
  } // end of property Instrument::Unit
  .property /*17000051*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          Tags()
  {
    .custom /*0C0000C8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.Metrics.Instrument/*0200001D*/::get_Tags() /* 06000122 */
  } // end of property Instrument::Tags
} // end of class System.Diagnostics.Metrics.Instrument

.class /*0200001E*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.Instrument`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends System.Diagnostics.Metrics.Instrument/*0200001D*/
{
  .method /*06000123*/ public hidebysig specialname 
          instance class System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/<!T> 
          get_Advice() cil managed
  // SIG: 20 00 15 12 7C 01 13 00
  {
    .param [0]/*0800016B*/ 
    .custom /*0C00018F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    // Method begins at RVA 0x240f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::get_Advice

  .method /*06000124*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name) cil managed
  // SIG: 20 02 01 12 80 88 0E
  {
    .custom /*0C000154:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2412
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 14   |                  */ ldnull
    IL_0004:  /* 14   |                  */ ldnull
    IL_0005:  /* 14   |                  */ ldnull
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 28   | (0A)000029       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>,
                                                                                                                                                      class System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/<!0>) /* 0A000029 */
    IL_000c:  /* 14   |                  */ ldnull
    IL_000d:  /* 7A   |                  */ throw
  } // end of method Instrument`1::.ctor

  .method /*06000125*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    .custom /*0C000155:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000156:0A000019*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100002A*/) /* 0A000019 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [3]/*08000170*/ 
    .custom /*0C000192:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000171*/ 
    .custom /*0C000193:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2421
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 14   |                  */ ldnull
    IL_0008:  /* 28   | (0A)000029       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>,
                                                                                                                                                      class System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/<!0>) /* 0A000029 */
    IL_000d:  /* 14   |                  */ ldnull
    IL_000e:  /* 7A   |                  */ throw
  } // end of method Instrument`1::.ctor

  .method /*06000126*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 05 01 12 80 88 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .custom /*0C000158:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000159:0A000019*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100002A*/) /* 0A000019 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [3]/*08000174*/ 
    .custom /*0C000194:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000175*/ 
    .custom /*0C000195:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000176*/ 
    .custom /*0C000197:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2431
    // Code size       16 (0x10)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 0E   | 05               */ ldarg.s    tags
    IL_0008:  /* 14   |                  */ ldnull
    IL_0009:  /* 28   | (0A)000029       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*0200001E*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>,
                                                                                                                                                      class System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/<!0>) /* 0A000029 */
    IL_000e:  /* 14   |                  */ ldnull
    IL_000f:  /* 7A   |                  */ throw
  } // end of method Instrument`1::.ctor

  .method /*06000127*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               [opt] string unit,
                               [opt] string description,
                               [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                               [opt] class System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/<!T> advice) cil managed
  // SIG: 20 06 01 12 80 88 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C 15 12 7C 01 13 00
  {
    .custom /*0C00015B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000179*/  = nullref
    .custom /*0C000198:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800017A*/  = nullref
    .custom /*0C000199:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800017B*/  = nullref
    .custom /*0C00019A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [6]/*0800017C*/  = nullref
    .custom /*0C00019B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    // Method begins at RVA 0x2442
    // Code size       15 (0xf)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 0E   | 05               */ ldarg.s    tags
    IL_0008:  /* 28   | (06)00011C       */ call       instance void System.Diagnostics.Metrics.Instrument/*0200001D*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>) /* 0600011C */
    IL_000d:  /* 14   |                  */ ldnull
    IL_000e:  /* 7A   |                  */ throw
  } // end of method Instrument`1::.ctor

  .method /*06000128*/ family hidebysig instance void 
          RecordMeasurement(!T measurement) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2452
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*06000129*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 61 02 0E 1C
  {
    .param [2]/*0800017F*/ 
    .custom /*0C00019E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2455
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*0600012A*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*08000181*/ 
    .custom /*0C0001A1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000182*/ 
    .custom /*0C0001A3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2458
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*0600012B*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag1,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag2,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 61 02 0E 1C 15 11 61 02 0E 1C 15 11 61 02 0E 1C
  {
    .param [2]/*08000184*/ 
    .custom /*0C0001A4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000185*/ 
    .custom /*0C0001A5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000186*/ 
    .custom /*0C0001A6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x245b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*0600012C*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            [in] valuetype System.Diagnostics.TagList/*02000017*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 5C
  {
    .param [2]/*08000188*/ 
    .custom /*0C0001A7:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x245e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*0600012D*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 D9 01 15 11 61 02 0E 1C
  {
    .param [2]/*0800018A*/ 
    .custom /*0C0001A9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2461
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .property /*17000052*/ instance callconv(8) class System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/<!T>
          Advice()
  {
    .custom /*0C0000CA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    .get instance class System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/<!T> System.Diagnostics.Metrics.Instrument`1/*0200001E*/::get_Advice() /* 06000123 */
  } // end of property Instrument`1::Advice
} // end of class System.Diagnostics.Metrics.Instrument`1

.class /*0200001F*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.InstrumentAdvice`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .method /*0600012E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2464
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method InstrumentAdvice`1::.ctor

  .method /*0600012F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000042*/<!T> 
          get_HistogramBucketBoundaries() cil managed
  // SIG: 20 00 15 12 81 09 01 13 00
  {
    .param [0]/*0800018B*/ 
    .custom /*0C0001AA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    // Method begins at RVA 0x246d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InstrumentAdvice`1::get_HistogramBucketBoundaries

  .method /*06000130*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*0100002C*/) 
          set_HistogramBucketBoundaries(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000042*/<!T> 'value') cil managed
  // SIG: 20 01 1F 80 B1 01 15 12 81 09 01 13 00
  {
    .param [1]/*0800018C*/ 
    .custom /*0C0001AB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    // Method begins at RVA 0x2470
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method InstrumentAdvice`1::set_HistogramBucketBoundaries

  .property /*17000053*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000042*/<!T>
          HistogramBucketBoundaries()
  {
    .custom /*0C0000CB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000042*/<!T> System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/::get_HistogramBucketBoundaries() /* 0600012F */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*0100002C*/) System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/::set_HistogramBucketBoundaries(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000042*/<!T>) /* 06000130 */
  } // end of property InstrumentAdvice`1::HistogramBucketBoundaries
} // end of class System.Diagnostics.Metrics.InstrumentAdvice`1

.class /*02000020*/ public sequential ansi sealed beforefieldinit System.Diagnostics.Metrics.Measurement`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
{
  .pack 0
  .size 1
  .custom /*0C000084:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
  .method /*06000131*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2473
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000132*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [2]/*0800018F*/ 
    .custom /*0C0001AD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2476
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000133*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 61 02 0E 1C
  {
    .param [2]/*08000191*/ 
    .custom /*0C0001AE:0A000025*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000045*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    .custom /*0C0001AF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2479
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000134*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 D9 01 15 11 61 02 0E 1C
  {
    .param [2]/*08000193*/ 
    .custom /*0C0001B0:0A000023*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ParamCollectionAttribute/*01000043*/::.ctor() /* 0A000023 */ = ( 01 00 00 00 ) 
    .custom /*0C0001B1:0A000024*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ScopedRefAttribute/*01000044*/::.ctor() /* 0A000024 */ = ( 01 00 00 00 ) 
    .custom /*0C0001B2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x247c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000135*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               [in] valuetype System.Diagnostics.TagList/*02000017*/& tags) cil managed
  // SIG: 20 02 01 13 00 10 11 5C
  {
    .param [2]/*08000195*/ 
    .custom /*0C0001B3:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x247f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000136*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 11 80 D9 01 15 11 61 02 0E 1C
  {
    .param [0]/*08000196*/ 
    .custom /*0C0001B4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2482
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::get_Tags

  .method /*06000137*/ public hidebysig specialname 
          instance !T  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x2485
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::get_Value

  .property /*17000054*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          Tags()
  {
    .custom /*0C0000CC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.Metrics.Measurement`1/*02000020*/::get_Tags() /* 06000136 */
  } // end of property Measurement`1::Tags
  .property /*17000055*/ instance callconv(8) !T
          Value()
  {
    .get instance !T System.Diagnostics.Metrics.Measurement`1/*02000020*/::get_Value() /* 06000137 */
  } // end of property Measurement`1::Value
} // end of class System.Diagnostics.Metrics.Measurement`1

.class /*02000021*/ public auto ansi sealed System.Diagnostics.Metrics.MeasurementCallback`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100003A*/
{
  .method /*06000138*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method MeasurementCallback`1::.ctor

  .method /*06000139*/ public hidebysig newslot virtual 
          instance void  Invoke(class System.Diagnostics.Metrics.Instrument/*0200001D*/ instrument,
                                !T measurement,
                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                                object state) runtime managed
  // SIG: 20 04 01 12 74 13 00 15 11 80 D9 01 15 11 61 02 0E 1C 1C
  {
    .param [1]/*08000199*/ 
    .custom /*0C0001B5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800019B*/ 
    .custom /*0C0001B6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .param [4]/*0800019C*/ 
    .custom /*0C0001B7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method MeasurementCallback`1::Invoke

  .method /*0600013A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ 
          BeginInvoke(class System.Diagnostics.Metrics.Instrument/*0200001D*/ instrument,
                      !T measurement,
                      valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000036*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                      object state,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100003C*/ callback,
                      object 'object') runtime managed
  // SIG: 20 06 12 80 ED 12 74 13 00 15 11 80 D9 01 15 11 61 02 0E 1C 1C 12 80 F1 1C
  {
    .param [1]/*0800019D*/ 
    .custom /*0C0001B8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800019F*/ 
    .custom /*0C0001B9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .param [4]/*080001A0*/ 
    .custom /*0C0001BA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method MeasurementCallback`1::BeginInvoke

  .method /*0600013B*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100003B*/ result) runtime managed
  // SIG: 20 01 01 12 80 ED
  {
  } // end of method MeasurementCallback`1::EndInvoke

} // end of class System.Diagnostics.Metrics.MeasurementCallback`1

.class /*02000022*/ public auto ansi beforefieldinit System.Diagnostics.Metrics.Meter
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
{
  .custom /*0C00008C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00008D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
  .custom /*0C000064:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600013C*/ public hidebysig instance class System.Diagnostics.Metrics.Counter`1/*02000018*/<!!T> 
          CreateCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                      [opt] string unit,
                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 03 15 12 60 01 1E 00 0E 0E 0E
  {
    .param type T /*2A000013*/ 
      .custom /*0C00005E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001A4*/ 
    .custom /*0C0001BB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001A5*/ 
    .custom /*0C0001BC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001A6*/  = nullref
    .param [3]/*080001A7*/  = nullref
    // Method begins at RVA 0x2488
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateCounter

  .method /*0600013D*/ public hidebysig instance class System.Diagnostics.Metrics.Counter`1/*02000018*/<!!T> 
          CreateCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                      string unit,
                                                                                                      string description,
                                                                                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 04 15 12 60 01 1E 00 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A000014*/ 
      .custom /*0C000061:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001A8*/ 
    .custom /*0C0001BD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001A9*/ 
    .custom /*0C0001BE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*080001AC*/ 
    .custom /*0C0001BF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x248b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateCounter

  .method /*0600013E*/ public hidebysig instance class System.Diagnostics.Metrics.Gauge`1/*02000019*/<!!T> 
          CreateGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name) cil managed
  // SIG: 30 01 01 15 12 64 01 1E 00 0E
  {
    .custom /*0C000169:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001AD*/ 
    .custom /*0C0001C0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001AE*/ 
    .custom /*0C0001C1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x248e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateGauge

  .method /*0600013F*/ public hidebysig instance class System.Diagnostics.Metrics.Gauge`1/*02000019*/<!!T> 
          CreateGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                    [opt] string unit,
                                                                                                    [opt] string description,
                                                                                                    [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 04 15 12 64 01 1E 00 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A000016*/ 
      .custom /*0C000069:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001AF*/ 
    .custom /*0C0001C2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001B0*/ 
    .custom /*0C0001C3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001B1*/  = nullref
    .param [3]/*080001B2*/  = nullref
    .param [4]/*080001B3*/  = nullref
    .custom /*0C0001C4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2491
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateGauge

  .method /*06000140*/ public hidebysig instance class System.Diagnostics.Metrics.UpDownCounter`1/*0200001A*/<!!T> 
          CreateUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                            [opt] string unit,
                                                                                                            [opt] string description) cil managed
  // SIG: 30 01 03 15 12 68 01 1E 00 0E 0E 0E
  {
    .param type T /*2A000017*/ 
      .custom /*0C00006D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001B4*/ 
    .custom /*0C0001C5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001B5*/ 
    .custom /*0C0001C6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001B6*/  = nullref
    .param [3]/*080001B7*/  = nullref
    // Method begins at RVA 0x2494
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateUpDownCounter

  .method /*06000141*/ public hidebysig instance class System.Diagnostics.Metrics.UpDownCounter`1/*0200001A*/<!!T> 
          CreateUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                            string unit,
                                                                                                            string description,
                                                                                                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 04 15 12 68 01 1E 00 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A000018*/ 
      .custom /*0C00006F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001B8*/ 
    .custom /*0C0001C7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001B9*/ 
    .custom /*0C0001C8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*080001BC*/ 
    .custom /*0C0001C9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2497
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateUpDownCounter

  .method /*06000142*/ public hidebysig instance class System.Diagnostics.Metrics.Histogram`1/*0200001B*/<!!T> 
          CreateHistogram<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name) cil managed
  // SIG: 30 01 01 15 12 6C 01 1E 00 0E
  {
    .custom /*0C00016F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001BD*/ 
    .custom /*0C0001CA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001BE*/ 
    .custom /*0C0001CB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x249a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateHistogram

  .method /*06000143*/ public hidebysig instance class System.Diagnostics.Metrics.Histogram`1/*0200001B*/<!!T> 
          CreateHistogram<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                        string unit,
                                                                                                        string description) cil managed
  // SIG: 30 01 03 15 12 6C 01 1E 00 0E 0E 0E
  {
    .custom /*0C000170:0A000019*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100002A*/) /* 0A000019 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param type T /*2A00001A*/ 
      .custom /*0C000075:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001BF*/ 
    .custom /*0C0001CC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001C0*/ 
    .custom /*0C0001CD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x249d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateHistogram

  .method /*06000144*/ public hidebysig instance class System.Diagnostics.Metrics.Histogram`1/*0200001B*/<!!T> 
          CreateHistogram<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                        string unit,
                                                                                                        string description,
                                                                                                        class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 04 15 12 6C 01 1E 00 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .custom /*0C000173:0A000019*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100002B*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100002A*/) /* 0A000019 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param type T /*2A00001B*/ 
      .custom /*0C000078:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001C3*/ 
    .custom /*0C0001CE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001C4*/ 
    .custom /*0C0001CF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [4]/*080001C7*/ 
    .custom /*0C0001D0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x24a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateHistogram

  .method /*06000145*/ public hidebysig instance class System.Diagnostics.Metrics.Histogram`1/*0200001B*/<!!T> 
          CreateHistogram<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                        [opt] string unit,
                                                                                                        [opt] string description,
                                                                                                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                                                                                                        [opt] class System.Diagnostics.Metrics.InstrumentAdvice`1/*0200001F*/<!!T> advice) cil managed
  // SIG: 30 01 05 15 12 6C 01 1E 00 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C 15 12 7C 01 1E 00
  {
    .param type T /*2A00001C*/ 
      .custom /*0C00007B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001C8*/ 
    .custom /*0C0001D1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001C9*/ 
    .custom /*0C0001D2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001CA*/  = nullref
    .param [3]/*080001CB*/  = nullref
    .param [4]/*080001CC*/  = nullref
    .custom /*0C0001D3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*080001CD*/  = nullref
    .custom /*0C0001D4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    // Method begins at RVA 0x24a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateHistogram

  .method /*06000146*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000026*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<!!T> observeValue,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 98 01 1E 00 0E 15 12 80 A1 01 1E 00 0E 0E
  {
    .param type T /*2A00001D*/ 
      .custom /*0C00007E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001CE*/ 
    .custom /*0C0001D5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001CF*/ 
    .custom /*0C0001D6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001D0*/ 
    .custom /*0C0001D7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [3]/*080001D1*/  = nullref
    .param [4]/*080001D2*/  = nullref
    // Method begins at RVA 0x24a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*06000147*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000026*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<!!T> observeValue,
                                                                                                                string unit,
                                                                                                                string description,
                                                                                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 98 01 1E 00 0E 15 12 80 A1 01 1E 00 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A00001E*/ 
      .custom /*0C000080:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001D3*/ 
    .custom /*0C0001D8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001D4*/ 
    .custom /*0C0001D9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001D5*/ 
    .custom /*0C0001DA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [5]/*080001D8*/ 
    .custom /*0C0001DB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*06000148*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000026*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>> observeValue,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 98 01 1E 00 0E 15 12 80 A1 01 15 11 80 80 01 1E 00 0E 0E
  {
    .param type T /*2A00001F*/ 
      .custom /*0C000082:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001D9*/ 
    .custom /*0C0001DC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001DA*/ 
    .custom /*0C0001DD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001DB*/ 
    .custom /*0C0001DE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [3]/*080001DC*/  = nullref
    .param [4]/*080001DD*/  = nullref
    // Method begins at RVA 0x24ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*06000149*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000026*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>> observeValue,
                                                                                                                string unit,
                                                                                                                string description,
                                                                                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 98 01 1E 00 0E 15 12 80 A1 01 15 11 80 80 01 1E 00 0E 0E 15 12 80 89 01 15 11
  //      61 02 0E 1C
  {
    .param type T /*2A000020*/ 
      .custom /*0C000087:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001DE*/ 
    .custom /*0C0001DF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001DF*/ 
    .custom /*0C0001E0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001E0*/ 
    .custom /*0C0001E1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [5]/*080001E3*/ 
    .custom /*0C0001E2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*0600014A*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000026*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>>> observeValues,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 98 01 1E 00 0E 15 12 80 A1 01 15 12 80 89 01 15 11 80 80 01 1E 00 0E 0E
  {
    .param type T /*2A000021*/ 
      .custom /*0C00008A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001E4*/ 
    .custom /*0C0001E3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001E5*/ 
    .custom /*0C0001E4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001E6*/ 
    .custom /*0C0001E5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [3]/*080001E7*/  = nullref
    .param [4]/*080001E8*/  = nullref
    // Method begins at RVA 0x24b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*0600014B*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000026*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>>> observeValues,
                                                                                                                string unit,
                                                                                                                string description,
                                                                                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 98 01 1E 00 0E 15 12 80 A1 01 15 12 80 89 01 15 11 80 80 01 1E 00 0E 0E 15 12
  //      80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A000022*/ 
      .custom /*0C00008F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001E9*/ 
    .custom /*0C0001E6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001EA*/ 
    .custom /*0C0001E7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001EB*/ 
    .custom /*0C0001E8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [5]/*080001EE*/ 
    .custom /*0C0001E9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*0600014C*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000027*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<!!T> observeValue,
                                                                                                                      [opt] string unit,
                                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 9C 01 1E 00 0E 15 12 80 A1 01 1E 00 0E 0E
  {
    .param type T /*2A000023*/ 
      .custom /*0C000091:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001EF*/ 
    .custom /*0C0001EA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001F0*/ 
    .custom /*0C0001EB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001F1*/ 
    .custom /*0C0001EC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [3]/*080001F2*/  = nullref
    .param [4]/*080001F3*/  = nullref
    // Method begins at RVA 0x24b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*0600014D*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000027*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<!!T> observeValue,
                                                                                                                      string unit,
                                                                                                                      string description,
                                                                                                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 9C 01 1E 00 0E 15 12 80 A1 01 1E 00 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A000024*/ 
      .custom /*0C000092:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001F4*/ 
    .custom /*0C0001ED:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001F5*/ 
    .custom /*0C0001EE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001F6*/ 
    .custom /*0C0001EF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [5]/*080001F9*/ 
    .custom /*0C0001F0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*0600014E*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000027*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>> observeValue,
                                                                                                                      [opt] string unit,
                                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 9C 01 1E 00 0E 15 12 80 A1 01 15 11 80 80 01 1E 00 0E 0E
  {
    .param type T /*2A000025*/ 
      .custom /*0C000096:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001FA*/ 
    .custom /*0C0001F1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*080001FB*/ 
    .custom /*0C0001F2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080001FC*/ 
    .custom /*0C0001F3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [3]/*080001FD*/  = nullref
    .param [4]/*080001FE*/  = nullref
    // Method begins at RVA 0x24be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*0600014F*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000027*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>> observeValue,
                                                                                                                      string unit,
                                                                                                                      string description,
                                                                                                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 9C 01 1E 00 0E 15 12 80 A1 01 15 11 80 80 01 1E 00 0E 0E 15 12 80 89 01 15 11
  //      61 02 0E 1C
  {
    .param type T /*2A000026*/ 
      .custom /*0C000098:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080001FF*/ 
    .custom /*0C0001F4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000200*/ 
    .custom /*0C0001F5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000201*/ 
    .custom /*0C0001F6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [5]/*08000204*/ 
    .custom /*0C0001F7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*06000150*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000027*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>>> observeValues,
                                                                                                                      [opt] string unit,
                                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 9C 01 1E 00 0E 15 12 80 A1 01 15 12 80 89 01 15 11 80 80 01 1E 00 0E 0E
  {
    .param type T /*2A000027*/ 
      .custom /*0C00009A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000205*/ 
    .custom /*0C0001F8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000206*/ 
    .custom /*0C0001F9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000207*/ 
    .custom /*0C0001FA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [3]/*08000208*/  = nullref
    .param [4]/*08000209*/  = nullref
    // Method begins at RVA 0x24c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*06000151*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000027*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>>> observeValues,
                                                                                                                      string unit,
                                                                                                                      string description,
                                                                                                                      class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 9C 01 1E 00 0E 15 12 80 A1 01 15 12 80 89 01 15 11 80 80 01 1E 00 0E 0E 15 12
  //      80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A000028*/ 
      .custom /*0C00009C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800020A*/ 
    .custom /*0C0001FB:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800020B*/ 
    .custom /*0C0001FC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800020C*/ 
    .custom /*0C0001FD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [5]/*0800020F*/ 
    .custom /*0C0001FE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*06000152*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000028*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<!!T> observeValue,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 A0 01 1E 00 0E 15 12 80 A1 01 1E 00 0E 0E
  {
    .param type T /*2A000029*/ 
      .custom /*0C00009D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000210*/ 
    .custom /*0C0001FF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000211*/ 
    .custom /*0C000200:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000212*/ 
    .custom /*0C000201:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [3]/*08000213*/  = nullref
    .param [4]/*08000214*/  = nullref
    // Method begins at RVA 0x24ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*06000153*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000028*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<!!T> observeValue,
                                                                                                              string unit,
                                                                                                              string description,
                                                                                                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 A0 01 1E 00 0E 15 12 80 A1 01 1E 00 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A00002A*/ 
      .custom /*0C00009F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000215*/ 
    .custom /*0C000202:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000216*/ 
    .custom /*0C000203:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000217*/ 
    .custom /*0C000204:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [5]/*0800021A*/ 
    .custom /*0C000205:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*06000154*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000028*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>> observeValue,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 A0 01 1E 00 0E 15 12 80 A1 01 15 11 80 80 01 1E 00 0E 0E
  {
    .param type T /*2A00002B*/ 
      .custom /*0C0000A3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800021B*/ 
    .custom /*0C000206:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800021C*/ 
    .custom /*0C000207:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800021D*/ 
    .custom /*0C000208:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [3]/*0800021E*/  = nullref
    .param [4]/*0800021F*/  = nullref
    // Method begins at RVA 0x24d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*06000155*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000028*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>> observeValue,
                                                                                                              string unit,
                                                                                                              string description,
                                                                                                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 A0 01 1E 00 0E 15 12 80 A1 01 15 11 80 80 01 1E 00 0E 0E 15 12 80 89 01 15 11
  //      61 02 0E 1C
  {
    .param type T /*2A00002C*/ 
      .custom /*0C0000A6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000220*/ 
    .custom /*0C000209:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000221*/ 
    .custom /*0C00020A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000222*/ 
    .custom /*0C00020B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [5]/*08000225*/ 
    .custom /*0C00020C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*06000156*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000028*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>>> observeValues,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 A0 01 1E 00 0E 15 12 80 A1 01 15 12 80 89 01 15 11 80 80 01 1E 00 0E 0E
  {
    .param type T /*2A00002D*/ 
      .custom /*0C0000A8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000226*/ 
    .custom /*0C00020D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000227*/ 
    .custom /*0C00020E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000228*/ 
    .custom /*0C00020F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [3]/*08000229*/  = nullref
    .param [4]/*0800022A*/  = nullref
    // Method begins at RVA 0x24d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*06000157*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000028*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000028*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!!T>>> observeValues,
                                                                                                              string unit,
                                                                                                              string description,
                                                                                                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 30 01 05 15 12 80 A0 01 1E 00 0E 15 12 80 A1 01 15 12 80 89 01 15 11 80 80 01 1E 00 0E 0E 15 12
  //      80 89 01 15 11 61 02 0E 1C
  {
    .param type T /*2A00002E*/ 
      .custom /*0C0000A9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800022B*/ 
    .custom /*0C000210:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800022C*/ 
    .custom /*0C000211:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800022D*/ 
    .custom /*0C000212:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [5]/*08000230*/ 
    .custom /*0C000213:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x24d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*06000158*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::Dispose

  .method /*06000159*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::Dispose

  .method /*0600015A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.MeterOptions/*02000025*/ options) cil managed
  // SIG: 20 01 01 12 80 94
  {
    .custom /*0C00017C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24e2
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Meter::.ctor

  .method /*0600015B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00017D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24eb
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Meter::.ctor

  .method /*0600015C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string version) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C000181:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000235*/ 
    .custom /*0C000214:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24f4
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Meter::.ctor

  .method /*0600015D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string version,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags,
                               [opt] object scope) cil managed
  // SIG: 20 04 01 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C 1C
  {
    .param [1]/*08000236*/ 
    .custom /*0C000215:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000238*/ 
    .custom /*0C000216:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [4]/*08000239*/  = nullref
    // Method begins at RVA 0x24fd
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Meter::.ctor

  .method /*0600015E*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000184:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2506
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::get_Name

  .method /*0600015F*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2509
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::get_Version

  .method /*06000160*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [0]/*0800023A*/ 
    .custom /*0C000217:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x250c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::get_Tags

  .method /*06000161*/ public hidebysig specialname 
          instance object  get_Scope() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x250f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::get_Scope

  .property /*17000056*/ instance callconv(8) string
          Name()
  {
    .custom /*0C0000CF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Meter/*02000022*/::get_Name() /* 0600015E */
  } // end of property Meter::Name
  .property /*17000057*/ instance callconv(8) string
          Version()
  {
    .get instance string System.Diagnostics.Metrics.Meter/*02000022*/::get_Version() /* 0600015F */
  } // end of property Meter::Version
  .property /*17000058*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          Tags()
  {
    .custom /*0C0000D1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.Metrics.Meter/*02000022*/::get_Tags() /* 06000160 */
  } // end of property Meter::Tags
  .property /*17000059*/ instance callconv(8) object
          Scope()
  {
    .get instance object System.Diagnostics.Metrics.Meter/*02000022*/::get_Scope() /* 06000161 */
  } // end of property Meter::Scope
} // end of class System.Diagnostics.Metrics.Meter

.class /*02000023*/ public abstract auto ansi sealed beforefieldinit System.Diagnostics.Metrics.MeterFactoryExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000090:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000162*/ public hidebysig static 
          class System.Diagnostics.Metrics.Meter/*02000022*/ 
          Create(class System.Diagnostics.Metrics.IMeterFactory/*0200001C*/ meterFactory,
                 string name,
                 [opt] string version,
                 [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 00 04 12 80 88 12 70 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .custom /*0C000185:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000186:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*0800023D*/  = nullref
    .custom /*0C000218:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800023E*/  = nullref
    .custom /*0C000219:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2512
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 2A   |                  */ ret
  } // end of method MeterFactoryExtensions::Create

} // end of class System.Diagnostics.Metrics.MeterFactoryExtensions

.class /*02000024*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.MeterListener
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000016*/
{
  .method /*06000163*/ public hidebysig instance object 
          DisableMeasurementEvents(class System.Diagnostics.Metrics.Instrument/*0200001D*/ instrument) cil managed
  // SIG: 20 01 1C 12 74
  {
    .custom /*0C000187:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800023F*/ 
    .custom /*0C00021A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2515
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::DisableMeasurementEvents

  .method /*06000164*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2518
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::Dispose

  .method /*06000165*/ public hidebysig instance void 
          EnableMeasurementEvents(class System.Diagnostics.Metrics.Instrument/*0200001D*/ instrument,
                                  [opt] object state) cil managed
  // SIG: 20 02 01 12 74 1C
  {
    .custom /*0C00018A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000242*/  = nullref
    .custom /*0C00021B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x251b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::EnableMeasurementEvents

  .method /*06000166*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/> 
          get_InstrumentPublished() cil managed
  // SIG: 20 00 15 12 79 02 12 74 12 80 90
  {
    .param [0]/*08000243*/ 
    .custom /*0C00021C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x251e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::get_InstrumentPublished

  .method /*06000167*/ public hidebysig specialname 
          instance void  set_InstrumentPublished(class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/> 'value') cil managed
  // SIG: 20 01 01 15 12 79 02 12 74 12 80 90
  {
    .param [1]/*08000244*/ 
    .custom /*0C00021D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2521
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::set_InstrumentPublished

  .method /*06000168*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,object> 
          get_MeasurementsCompleted() cil managed
  // SIG: 20 00 15 12 79 02 12 74 1C
  {
    .param [0]/*08000245*/ 
    .custom /*0C00021E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x2524
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::get_MeasurementsCompleted

  .method /*06000169*/ public hidebysig specialname 
          instance void  set_MeasurementsCompleted(class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,object> 'value') cil managed
  // SIG: 20 01 01 15 12 79 02 12 74 1C
  {
    .param [1]/*08000246*/ 
    .custom /*0C00021F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x2527
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::set_MeasurementsCompleted

  .method /*0600016A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x252a
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method MeterListener::.ctor

  .method /*0600016B*/ public hidebysig instance void 
          RecordObservableInstruments() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2533
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::RecordObservableInstruments

  .method /*0600016C*/ public hidebysig instance void 
          SetMeasurementEventCallback<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>(class System.Diagnostics.Metrics.MeasurementCallback`1/*02000021*/<!!T> measurementCallback) cil managed
  // SIG: 30 01 01 01 15 12 80 84 01 1E 00
  {
    .param [1]/*08000247*/ 
    .custom /*0C000220:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    // Method begins at RVA 0x2536
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::SetMeasurementEventCallback

  .method /*0600016D*/ public hidebysig instance void 
          Start() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2539
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::Start

  .property /*1700005A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/>
          InstrumentPublished()
  {
    .custom /*0C0000D2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/> System.Diagnostics.Metrics.MeterListener/*02000024*/::get_InstrumentPublished() /* 06000166 */
    .set instance void System.Diagnostics.Metrics.MeterListener/*02000024*/::set_InstrumentPublished(class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/>) /* 06000167 */
  } // end of property MeterListener::InstrumentPublished
  .property /*1700005B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,object>
          MeasurementsCompleted()
  {
    .custom /*0C0000D4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,object> System.Diagnostics.Metrics.MeterListener/*02000024*/::get_MeasurementsCompleted() /* 06000168 */
    .set instance void System.Diagnostics.Metrics.MeterListener/*02000024*/::set_MeasurementsCompleted(class [System.Runtime/*23000001*/]System.Action`2/*0100001E*/<class System.Diagnostics.Metrics.Instrument/*0200001D*/,object>) /* 06000169 */
  } // end of property MeterListener::MeasurementsCompleted
} // end of class System.Diagnostics.Metrics.MeterListener

.class /*02000025*/ public auto ansi beforefieldinit System.Diagnostics.Metrics.MeterOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000093:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000094:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600016E*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000190:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x253c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterOptions::get_Name

  .method /*0600016F*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000191:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x253f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterOptions::set_Name

  .method /*06000170*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2542
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterOptions::get_Version

  .method /*06000171*/ public hidebysig specialname 
          instance void  set_Version(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2545
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterOptions::set_Version

  .method /*06000172*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [0]/*0800024A*/ 
    .custom /*0C000221:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x2548
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterOptions::get_Tags

  .method /*06000173*/ public hidebysig specialname 
          instance void  set_Tags(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> 'value') cil managed
  // SIG: 20 01 01 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .param [1]/*0800024B*/ 
    .custom /*0C000222:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x254b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterOptions::set_Tags

  .method /*06000174*/ public hidebysig specialname 
          instance object  get_Scope() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x254e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterOptions::get_Scope

  .method /*06000175*/ public hidebysig specialname 
          instance void  set_Scope(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x2551
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterOptions::set_Scope

  .method /*06000176*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000196:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2554
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000019*/::.ctor() /* 0A000026 */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method MeterOptions::.ctor

  .property /*1700005C*/ instance callconv(8) string
          Name()
  {
    .custom /*0C0000D6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Diagnostics.Metrics.MeterOptions/*02000025*/::get_Name() /* 0600016E */
    .set instance void System.Diagnostics.Metrics.MeterOptions/*02000025*/::set_Name(string) /* 0600016F */
  } // end of property MeterOptions::Name
  .property /*1700005D*/ instance callconv(8) string
          Version()
  {
    .get instance string System.Diagnostics.Metrics.MeterOptions/*02000025*/::get_Version() /* 06000170 */
    .set instance void System.Diagnostics.Metrics.MeterOptions/*02000025*/::set_Version(string) /* 06000171 */
  } // end of property MeterOptions::Version
  .property /*1700005E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>
          Tags()
  {
    .custom /*0C0000D9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> System.Diagnostics.Metrics.MeterOptions/*02000025*/::get_Tags() /* 06000172 */
    .set instance void System.Diagnostics.Metrics.MeterOptions/*02000025*/::set_Tags(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>) /* 06000173 */
  } // end of property MeterOptions::Tags
  .property /*1700005F*/ instance callconv(8) object
          Scope()
  {
    .get instance object System.Diagnostics.Metrics.MeterOptions/*02000025*/::get_Scope() /* 06000174 */
    .set instance void System.Diagnostics.Metrics.MeterOptions/*02000025*/::set_Scope(object) /* 06000175 */
  } // end of property MeterOptions::Scope
} // end of class System.Diagnostics.Metrics.MeterOptions

.class /*02000026*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.ObservableCounter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends class System.Diagnostics.Metrics.ObservableInstrument`1/*02000029*/<!T>/*1B00000F*/
{
  .method /*06000177*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    // Method begins at RVA 0x255d
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)00002A       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000029*/<!T>/*1B00000F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string) /* 0A00002A */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableCounter`1::.ctor

  .method /*06000178*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 80 80 01 13 00
  {
    .param [0]/*08000252*/ 
    .custom /*0C000223:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x256b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableCounter`1::Observe

} // end of class System.Diagnostics.Metrics.ObservableCounter`1

.class /*02000027*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.ObservableUpDownCounter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends class System.Diagnostics.Metrics.ObservableInstrument`1/*02000029*/<!T>/*1B00000F*/
{
  .method /*06000179*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    // Method begins at RVA 0x256e
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)00002A       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000029*/<!T>/*1B00000F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string) /* 0A00002A */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableUpDownCounter`1::.ctor

  .method /*0600017A*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 80 80 01 13 00
  {
    .param [0]/*08000257*/ 
    .custom /*0C000224:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x257c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableUpDownCounter`1::Observe

} // end of class System.Diagnostics.Metrics.ObservableUpDownCounter`1

.class /*02000028*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.ObservableGauge`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends class System.Diagnostics.Metrics.ObservableInstrument`1/*02000029*/<!T>/*1B00000F*/
{
  .method /*0600017B*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    // Method begins at RVA 0x257f
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)00002A       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000029*/<!T>/*1B00000F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string) /* 0A00002A */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableGauge`1::.ctor

  .method /*0600017C*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 80 80 01 13 00
  {
    .param [0]/*0800025C*/ 
    .custom /*0C000225:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x258d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableGauge`1::Observe

} // end of class System.Diagnostics.Metrics.ObservableGauge`1

.class /*02000029*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.ObservableInstrument`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000029*/) T>
       extends System.Diagnostics.Metrics.Instrument/*0200001D*/
{
  .method /*0600017D*/ public hidebysig specialname virtual 
          instance bool  get_IsObservable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2590
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableInstrument`1::get_IsObservable

  .method /*0600017E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 88 0E 0E 0E
  {
    .custom /*0C00019C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800025F*/ 
    .custom /*0C000226:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000260*/ 
    .custom /*0C000227:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2593
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 28   | (0A)00002B       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000029*/<!T>/*1B00000F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>>) /* 0A00002B */
    IL_000c:  /* 14   |                  */ ldnull
    IL_000d:  /* 7A   |                  */ throw
  } // end of method ObservableInstrument`1::.ctor

  .method /*0600017F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000022*/ meter,
                               string name,
                               string unit,
                               string description,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000018*/<string,object>> tags) cil managed
  // SIG: 20 05 01 12 80 88 0E 0E 0E 15 12 80 89 01 15 11 61 02 0E 1C
  {
    .custom /*0C00019D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000263*/ 
    .custom /*0C000228:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000264*/ 
    .custom /*0C000229:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000265*/ 
    .custom /*0C00022A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x25a2
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (06)00011B       */ call       instance void System.Diagnostics.Metrics.Instrument/*0200001D*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000022*/,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              string) /* 0600011B */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableInstrument`1::.ctor

  .method /*06000180*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000020*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 80 89 01 15 11 80 80 01 13 00
  {
    .param [0]/*08000266*/ 
    .custom /*0C00022B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000015 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ObservableInstrument`1::Observe

  .property /*17000060*/ instance callconv(8) bool
          IsObservable()
  {
    .get instance bool System.Diagnostics.Metrics.ObservableInstrument`1/*02000029*/::get_IsObservable() /* 0600017D */
  } // end of property ObservableInstrument`1::IsObservable
} // end of class System.Diagnostics.Metrics.ObservableInstrument`1


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
