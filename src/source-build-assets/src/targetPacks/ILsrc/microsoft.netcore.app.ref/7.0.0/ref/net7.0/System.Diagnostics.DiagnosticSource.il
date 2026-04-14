


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
// Time-date stamp:            0x98f9b43c
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00006200
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000081de
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
// Checksum:                       0x00015ef2
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000818b [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x000004f4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00006c00 [0x000028a0] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00008088 [0x00000054] address [size] of Debug Directory:           
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
//              0x000061e4 Virtual Size
//              0x00002000 Virtual Address
//              0x00006200 Size of Raw Data
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
//              0x00006400 Pointer to Raw Data
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
//              0x00006a00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000001e0
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x000081cd Import Name Table
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
// 0x00002508 [0x00005b00] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00008008 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000036d4 Size
//              '#~' Name
//    Stream 2:
//              0x00003740 Offset
//              0x000015b0 Size
//              '#Strings' Name
//    Stream 3:
//              0x00004cf0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00004cf4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00004d04 Offset
//              0x00000dfc Size
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

// File size            : 38048
// PE header size       : 512 (496 used)    ( 1.35%)
// PE additional info   : 11923             (31.34%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.19%)
// CLR meta-data size  : 23296              (61.23%)
// CLR additional info : 128                ( 0.34%)
// CLR method headers  : 333                ( 0.88%)
// Managed code         : 873               ( 2.29%)
// Data                 : 2048              ( 5.38%)
// Unaccounted          : -1137             (-2.99%)

// Num.of PE sections   : 3
//   .text    - 25088
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 23296
//   Module        -    1 (10 bytes)
//   TypeDef       -   45 (630 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   63 (378 bytes)
//   MethodDef     -  345 (4830 bytes)    7 abstract, 0 native, 322 bodies
//   FieldDef      -   31 (186 bytes)     0 constant
//   MemberRef     -   33 (198 bytes)
//   ParamDef      -  461 (2766 bytes)
//   MethodImpl    -    6 (36 bytes)
//   Constant      -   73 (438 bytes)
//   CustomAttribute-  424 (2544 bytes)
//   ClassLayout   -    9 (72 bytes)
//   StandAloneSig -    1 (2 bytes)
//   InterfaceImpl -   27 (108 bytes)
//   PropertyMap   -   20 (80 bytes)
//   Property      -   90 (540 bytes)
//   MethodSemantic-  113 (678 bytes)
//   TypeSpec      -   15 (30 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    1 (20 bytes)
//   ExportedType  -    1 (14 bytes)
//   NestedClass   -    5 (20 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    1 (6 bytes)
//   GenericParam  -   26 (208 bytes)
//   GenericParamConstraint-   22 (88 bytes)
//   Strings       -  5551 bytes
//   Blobs         -  3580 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   237 bytes

// CLR additional info : 128

// CLR method headers : 333
//   Num.of method bodies  - 322
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 321

// Managed code : 873
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
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
// Class ActivityListener               (public) (auto) (ansi) (sealed) 
// Class DistributedContextPropagator   (public) (abstract) (auto) (ansi) 
// Class TagList                        (public) (sequential) (ansi) (sealed) 
// Class Counter`1                      (public) (auto) (ansi) (sealed) 
// Class UpDownCounter`1                (public) (auto) (ansi) (sealed) 
// Class Histogram`1                    (public) (auto) (ansi) (sealed) 
// Class Instrument                     (public) (abstract) (auto) (ansi) 
// Class Instrument`1                   (public) (abstract) (auto) (ansi) 
// Class Measurement`1                  (public) (sequential) (ansi) (sealed) 
// Class MeasurementCallback`1          (public) (auto) (ansi) (sealed) 
// Class Meter                          (public) (auto) (ansi) 
// Class MeterListener                  (public) (auto) (ansi) (sealed) 
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
  .ver 7:0:0:0
}
.assembly /*20000001*/ noplatform System.Diagnostics.DiagnosticSource
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                        73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                        53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                       73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                       53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 30 35 00   // ...7.0.22.51805.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E 37 2E 30 2E 30 2B 64 30 39 39 66 30 37   // ...7.0.0+d099f07
                                                                                                                                                                                35 65 34 35 64 32 61 61 36 30 30 37 61 32 32 62   // 5e45d2aa6007a22b
                                                                                                                                                                                37 31 62 34 35 61 30 38 37 35 38 35 35 39 66 38   // 71b45a08758559f8
                                                                                                                                                                                30 00 00 )                                        // 0..
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 23 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // ..#System.Diagno
                                                                                                                                                                 73 74 69 63 73 2E 44 69 61 67 6E 6F 73 74 69 63   // stics.Diagnostic
                                                                                                                                                                 53 6F 75 72 63 65 00 00 )                         // Source..
  .custom /*0C000015:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 7:0:0:0
}
.class extern /*27000001*/ forwarder System.Runtime.CompilerServices.IsExternalInit
{
  .assembly extern System.Runtime /*23000001*/ 
}
.module System.Diagnostics.DiagnosticSource.dll
// MVID: {17bb5643-0c58-4b7a-a8c9-d38cb615348c}
.custom /*0C000002:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C000017:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C00001B:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000010*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000013*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000012*/) /* 0A000010 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001E */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00003F       */ newarr     [System.Runtime/*23000001*/]System.Byte/*0100003F*/
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
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001E */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C000020:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000010*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000013*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000012*/) /* 0A000010 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001E */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C000025:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C000026:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000027:0A000010*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000013*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000012*/) /* 0A000010 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001E */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000011*/
{
  .custom /*0C00002A:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .custom /*0C00002B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002C:0A000010*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000013*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000012*/) /* 0A000010 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000011*/::.ctor() /* 0A00001E */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000004       */ stfld      int32 System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::Version /* 04000004 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ public auto ansi beforefieldinit System.Diagnostics.DiagnosticListener
       extends System.Diagnostics.DiagnosticSource/*02000008*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/,
                  class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000001*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000001*/
  .custom /*0C000019:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20ac
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000016       */ call       instance void System.Diagnostics.DiagnosticSource/*02000008*/::.ctor() /* 06000016 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::.ctor

  .method /*06000008*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<class System.Diagnostics.DiagnosticListener/*02000007*/> 
          get_AllListeners() cil managed
  // SIG: 00 00 15 12 55 01 12 1C
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::get_AllListeners

  .method /*06000009*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::get_Name

  .method /*0600000A*/ public hidebysig newslot virtual 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::Dispose

  .method /*0600000B*/ public hidebysig instance bool 
          IsEnabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*0600000C*/ public hidebysig virtual 
          instance bool  IsEnabled(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x20bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*0600000D*/ public hidebysig virtual 
          instance bool  IsEnabled(string name,
                                   object arg1,
                                   [opt] object arg2) cil managed
  // SIG: 20 03 02 0E 1C 1C
  {
    .custom /*0C000045:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000003*/ 
    .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000005*/  = nullref
    // Method begins at RVA 0x20c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::IsEnabled

  .method /*0600000E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000014*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*01000018*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> observer) cil managed
  // SIG: 20 01 12 51 15 12 61 01 15 11 59 02 0E 1C
  {
    .param [1]/*08000006*/ 
    .custom /*0C00002D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x20c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*0600000F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000014*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*01000018*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Func`4/*01000019*/<string,object,object,bool> isEnabled) cil managed
  // SIG: 20 02 12 51 15 12 61 01 15 11 59 02 0E 1C 15 12 65 04 0E 1C 1C 02
  {
    .param [1]/*08000007*/ 
    .custom /*0C000031:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*08000008*/ 
    .custom /*0C000035:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    // Method begins at RVA 0x20c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*06000010*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000014*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*01000018*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Predicate`1/*0100001A*/<string> isEnabled) cil managed
  // SIG: 20 02 12 51 15 12 61 01 15 11 59 02 0E 1C 15 12 69 01 0E
  {
    .param [1]/*08000009*/ 
    .custom /*0C000039:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*0800000A*/ 
    .custom /*0C00003F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .method /*06000011*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::ToString

  .method /*06000012*/ public hidebysig virtual 
          instance void  Write(string name,
                               object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C000057:0A000011*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000011 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .custom /*0C000058:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001C*/::.ctor(string) /* 0A000012 */ = ( 01 00 80 92 44 69 61 67 6E 6F 73 74 69 63 53 6F   // ....DiagnosticSo
                                                                                                                                                                                       75 72 63 65 20 6D 61 79 20 72 65 71 75 69 72 65   // urce may require
                                                                                                                                                                                       20 63 72 65 61 74 69 6E 67 20 6E 65 77 20 67 65   //  creating new ge
                                                                                                                                                                                       6E 65 72 69 63 20 74 79 70 65 73 20 6F 72 20 6D   // neric types or m
                                                                                                                                                                                       65 74 68 6F 64 73 2C 20 77 68 69 63 68 20 72 65   // ethods, which re
                                                                                                                                                                                       71 75 69 72 65 73 20 63 72 65 61 74 69 6E 67 20   // quires creating 
                                                                                                                                                                                       63 6F 64 65 20 61 74 20 72 75 6E 74 69 6D 65 2E   // code at runtime.
                                                                                                                                                                                       20 54 68 69 73 20 6D 61 79 20 6E 6F 74 20 77 6F   //  This may not wo
                                                                                                                                                                                       72 6B 20 77 68 65 6E 20 41 4F 54 20 63 6F 6D 70   // rk when AOT comp
                                                                                                                                                                                       69 6C 69 6E 67 2E 00 00 )                         // iling...
    .param [2]/*0800000C*/ 
    .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::Write

  .method /*06000013*/ public hidebysig virtual 
          instance void  OnActivityExport(class System.Diagnostics.Activity/*02000009*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 24 1C
  {
    .param [2]/*0800000E*/ 
    .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::OnActivityExport

  .method /*06000014*/ public hidebysig virtual 
          instance void  OnActivityImport(class System.Diagnostics.Activity/*02000009*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 24 1C
  {
    .param [2]/*08000010*/ 
    .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticListener::OnActivityImport

  .method /*06000015*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000014*/ 
          Subscribe(class [System.Runtime/*23000001*/]System.IObserver`1/*01000018*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> observer,
                    class [System.Runtime/*23000001*/]System.Func`4/*01000019*/<string,object,object,bool> isEnabled,
                    [opt] class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Activity/*02000009*/,object> onActivityImport,
                    [opt] class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Activity/*02000009*/,object> onActivityExport) cil managed
  // SIG: 20 04 12 51 15 12 61 01 15 11 59 02 0E 1C 15 12 65 04 0E 1C 1C 02 15 12 75 02 12 24 1C 15 12 75
  //      02 12 24 1C
  {
    .param [1]/*08000011*/ 
    .custom /*0C000055:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .param [2]/*08000012*/ 
    .custom /*0C000059:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 02 02 00 00 ) 
    .param [3]/*08000013*/  = nullref
    .custom /*0C00005D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    .param [4]/*08000014*/  = nullref
    .custom /*0C000064:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticListener::Subscribe

  .property /*17000001*/ callconv(8) class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<class System.Diagnostics.DiagnosticListener/*02000007*/>
          AllListeners()
  {
    .get class [System.Runtime/*23000001*/]System.IObservable`1/*01000015*/<class System.Diagnostics.DiagnosticListener/*02000007*/> System.Diagnostics.DiagnosticListener/*02000007*/::get_AllListeners() /* 06000008 */
  } // end of property DiagnosticListener::AllListeners
  .property /*17000002*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.DiagnosticListener/*02000007*/::get_Name() /* 06000009 */
  } // end of property DiagnosticListener::Name
} // end of class System.Diagnostics.DiagnosticListener

.class /*02000008*/ public abstract auto ansi beforefieldinit System.Diagnostics.DiagnosticSource
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000016*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20da
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::.ctor

  .method /*06000017*/ public hidebysig newslot abstract virtual 
          instance bool  IsEnabled(string name) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x0
  } // end of method DiagnosticSource::IsEnabled

  .method /*06000018*/ public hidebysig newslot virtual 
          instance bool  IsEnabled(string name,
                                   object arg1,
                                   [opt] object arg2) cil managed
  // SIG: 20 03 02 0E 1C 1C
  {
    .custom /*0C000074:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000016*/ 
    .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000018*/  = nullref
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticSource::IsEnabled

  .method /*06000019*/ public hidebysig newslot abstract virtual 
          instance void  Write(string name,
                               object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C000076:0A000011*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000011 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .custom /*0C000077:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001C*/::.ctor(string) /* 0A000012 */ = ( 01 00 80 92 44 69 61 67 6E 6F 73 74 69 63 53 6F   // ....DiagnosticSo
                                                                                                                                                                                       75 72 63 65 20 6D 61 79 20 72 65 71 75 69 72 65   // urce may require
                                                                                                                                                                                       20 63 72 65 61 74 69 6E 67 20 6E 65 77 20 67 65   //  creating new ge
                                                                                                                                                                                       6E 65 72 69 63 20 74 79 70 65 73 20 6F 72 20 6D   // neric types or m
                                                                                                                                                                                       65 74 68 6F 64 73 2C 20 77 68 69 63 68 20 72 65   // ethods, which re
                                                                                                                                                                                       71 75 69 72 65 73 20 63 72 65 61 74 69 6E 67 20   // quires creating 
                                                                                                                                                                                       63 6F 64 65 20 61 74 20 72 75 6E 74 69 6D 65 2E   // code at runtime.
                                                                                                                                                                                       20 54 68 69 73 20 6D 61 79 20 6E 6F 74 20 77 6F   //  This may not wo
                                                                                                                                                                                       72 6B 20 77 68 65 6E 20 41 4F 54 20 63 6F 6D 70   // rk when AOT comp
                                                                                                                                                                                       69 6C 69 6E 67 2E 00 00 )                         // iling...
    .param [2]/*0800001A*/ 
    .custom /*0C00007E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DiagnosticSource::Write

  .method /*0600001A*/ public hidebysig newslot virtual 
          instance void  OnActivityExport(class System.Diagnostics.Activity/*02000009*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 24 1C
  {
    .param [2]/*0800001C*/ 
    .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::OnActivityExport

  .method /*0600001B*/ public hidebysig newslot virtual 
          instance void  OnActivityImport(class System.Diagnostics.Activity/*02000009*/ activity,
                                          object payload) cil managed
  // SIG: 20 02 01 12 24 1C
  {
    .param [2]/*0800001E*/ 
    .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::OnActivityImport

  .method /*0600001C*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          StartActivity(class System.Diagnostics.Activity/*02000009*/ activity,
                        object args) cil managed
  // SIG: 20 02 12 24 12 24 1C
  {
    .custom /*0C000082:0A000011*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000011 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .custom /*0C000083:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001C*/::.ctor(string) /* 0A000012 */ = ( 01 00 80 92 44 69 61 67 6E 6F 73 74 69 63 53 6F   // ....DiagnosticSo
                                                                                                                                                                                       75 72 63 65 20 6D 61 79 20 72 65 71 75 69 72 65   // urce may require
                                                                                                                                                                                       20 63 72 65 61 74 69 6E 67 20 6E 65 77 20 67 65   //  creating new ge
                                                                                                                                                                                       6E 65 72 69 63 20 74 79 70 65 73 20 6F 72 20 6D   // neric types or m
                                                                                                                                                                                       65 74 68 6F 64 73 2C 20 77 68 69 63 68 20 72 65   // ethods, which re
                                                                                                                                                                                       71 75 69 72 65 73 20 63 72 65 61 74 69 6E 67 20   // quires creating 
                                                                                                                                                                                       63 6F 64 65 20 61 74 20 72 75 6E 74 69 6D 65 2E   // code at runtime.
                                                                                                                                                                                       20 54 68 69 73 20 6D 61 79 20 6E 6F 74 20 77 6F   //  This may not wo
                                                                                                                                                                                       72 6B 20 77 68 65 6E 20 41 4F 54 20 63 6F 6D 70   // rk when AOT comp
                                                                                                                                                                                       69 6C 69 6E 67 2E 00 00 )                         // iling...
    .param [2]/*08000020*/ 
    .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticSource::StartActivity

  .method /*0600001D*/ public hidebysig instance void 
          StopActivity(class System.Diagnostics.Activity/*02000009*/ activity,
                       object args) cil managed
  // SIG: 20 02 01 12 24 1C
  {
    .custom /*0C000086:0A000011*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000011 */ = ( 01 00 55 54 68 65 20 74 79 70 65 20 6F 66 20 6F   // ..UThe type of o
                                                                                                                                                                                            62 6A 65 63 74 20 62 65 69 6E 67 20 77 72 69 74   // bject being writ
                                                                                                                                                                                            74 65 6E 20 74 6F 20 44 69 61 67 6E 6F 73 74 69   // ten to Diagnosti
                                                                                                                                                                                            63 53 6F 75 72 63 65 20 63 61 6E 6E 6F 74 20 62   // cSource cannot b
                                                                                                                                                                                            65 20 64 69 73 63 6F 76 65 72 65 64 20 73 74 61   // e discovered sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 2E 00 00 )                   // tically...
    .custom /*0C000087:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresDynamicCodeAttribute/*0100001C*/::.ctor(string) /* 0A000012 */ = ( 01 00 80 92 44 69 61 67 6E 6F 73 74 69 63 53 6F   // ....DiagnosticSo
                                                                                                                                                                                       75 72 63 65 20 6D 61 79 20 72 65 71 75 69 72 65   // urce may require
                                                                                                                                                                                       20 63 72 65 61 74 69 6E 67 20 6E 65 77 20 67 65   //  creating new ge
                                                                                                                                                                                       6E 65 72 69 63 20 74 79 70 65 73 20 6F 72 20 6D   // neric types or m
                                                                                                                                                                                       65 74 68 6F 64 73 2C 20 77 68 69 63 68 20 72 65   // ethods, which re
                                                                                                                                                                                       71 75 69 72 65 73 20 63 72 65 61 74 69 6E 67 20   // quires creating 
                                                                                                                                                                                       63 6F 64 65 20 61 74 20 72 75 6E 74 69 6D 65 2E   // code at runtime.
                                                                                                                                                                                       20 54 68 69 73 20 6D 61 79 20 6E 6F 74 20 77 6F   //  This may not wo
                                                                                                                                                                                       72 6B 20 77 68 65 6E 20 41 4F 54 20 63 6F 6D 70   // rk when AOT comp
                                                                                                                                                                                       69 6C 69 6E 67 2E 00 00 )                         // iling...
    .param [2]/*08000022*/ 
    .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DiagnosticSource::StopActivity

} // end of class System.Diagnostics.DiagnosticSource

.class /*02000009*/ public auto ansi beforefieldinit System.Diagnostics.Activity
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000029*/ sequential ansi sealed nested public beforefieldinit Enumerator`1<T>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
  {
    .pack 0
    .size 1
    .custom /*0C00009A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param type T /*2A00000D*/ 
      .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .method /*06000145*/ public hidebysig 
            instance valuetype System.Diagnostics.Activity/*02000009*//Enumerator`1/*02000029*/<!T> 
            GetEnumerator() cil managed
    // SIG: 20 00 15 11 80 A4 01 13 00
    {
      .custom /*0C00015B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
      .custom /*0C00015C:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000026*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000025*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
      .param [0]/*080001B1*/ 
      .custom /*0C00019A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
      // Method begins at RVA 0x24d4
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator`1::GetEnumerator

    .method /*06000146*/ public hidebysig specialname 
            instance !T&  get_Current() cil managed
    // SIG: 20 00 10 13 00
    {
      .custom /*0C00015E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x24d7
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator`1::get_Current

    .method /*06000147*/ public hidebysig 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x24da
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator`1::MoveNext

    .property /*17000056*/ instance callconv(8) !T&
            Current()
    {
      .get instance !T& System.Diagnostics.Activity/*02000009*//Enumerator`1/*02000029*/::get_Current() /* 06000146 */
    } // end of property Enumerator`1::Current
  } // end of class Enumerator`1

  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string operationName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20ee
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Activity::.ctor

  .method /*0600001F*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ 
          get_ActivityTraceFlags() cil managed
  // SIG: 20 00 11 40
  {
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ActivityTraceFlags

  .method /*06000020*/ public hidebysig specialname 
          instance void  set_ActivityTraceFlags(valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ 'value') cil managed
  // SIG: 20 01 01 11 40
  {
    // Method begins at RVA 0x20f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_ActivityTraceFlags

  .method /*06000021*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> 
          get_Baggage() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 59 02 0E 0E
  {
    .param [0]/*08000025*/ 
    .custom /*0C000097:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Baggage

  .method /*06000022*/ public hidebysig specialname static 
          class System.Diagnostics.Activity/*02000009*/ 
          get_Current() cil managed
  // SIG: 00 00 12 24
  {
    .custom /*0C000090:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Current

  .method /*06000023*/ public hidebysig specialname static 
          void  set_Current(class System.Diagnostics.Activity/*02000009*/ 'value') cil managed
  // SIG: 00 01 01 12 24
  {
    .custom /*0C000092:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_Current

  .method /*06000024*/ public hidebysig specialname static 
          void  add_CurrentChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001E*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*0200000A*/> 'value') cil managed
  // SIG: 00 01 01 15 12 79 01 11 28
  {
    .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2103
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::add_CurrentChanged

  .method /*06000025*/ public hidebysig specialname static 
          void  remove_CurrentChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001E*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*0200000A*/> 'value') cil managed
  // SIG: 00 01 01 15 12 79 01 11 28
  {
    .custom /*0C000096:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2105
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::remove_CurrentChanged

  .method /*06000026*/ public hidebysig specialname static 
          valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ 
          get_DefaultIdFormat() cil managed
  // SIG: 00 00 11 34
  {
    // Method begins at RVA 0x2107
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_DefaultIdFormat

  .method /*06000027*/ public hidebysig specialname static 
          void  set_DefaultIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ 'value') cil managed
  // SIG: 00 01 01 11 34
  {
    // Method begins at RVA 0x210a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_DefaultIdFormat

  .method /*06000028*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000020*/ 
          get_Duration() cil managed
  // SIG: 20 00 11 80 81
  {
    // Method begins at RVA 0x210c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Duration

  .method /*06000029*/ public hidebysig specialname static 
          bool  get_ForceDefaultIdFormat() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x210f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ForceDefaultIdFormat

  .method /*0600002A*/ public hidebysig specialname static 
          void  set_ForceDefaultIdFormat(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x2112
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_ForceDefaultIdFormat

  .method /*0600002B*/ public hidebysig specialname 
          instance string  get_Id() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00009D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Id

  .method /*0600002C*/ public hidebysig specialname 
          instance bool  get_HasRemoteParent() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_HasRemoteParent

  .method /*0600002D*/ public hidebysig specialname 
          instance bool  get_IsAllDataRequested() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_IsAllDataRequested

  .method /*0600002E*/ public hidebysig specialname 
          instance void  set_IsAllDataRequested(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_IsAllDataRequested

  .method /*0600002F*/ public hidebysig specialname 
          instance bool  get_IsStopped() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2120
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_IsStopped

  .method /*06000030*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ 
          get_IdFormat() cil managed
  // SIG: 20 00 11 34
  {
    // Method begins at RVA 0x2123
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_IdFormat

  .method /*06000031*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityKind/*02000013*/ 
          get_Kind() cil managed
  // SIG: 20 00 11 4C
  {
    // Method begins at RVA 0x2126
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Kind

  .method /*06000032*/ public hidebysig specialname 
          instance string  get_OperationName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2129
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_OperationName

  .method /*06000033*/ public hidebysig specialname 
          instance string  get_DisplayName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_DisplayName

  .method /*06000034*/ public hidebysig specialname 
          instance void  set_DisplayName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x212f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_DisplayName

  .method /*06000035*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivitySource/*0200000F*/ 
          get_Source() cil managed
  // SIG: 20 00 12 3C
  {
    // Method begins at RVA 0x2132
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Source

  .method /*06000036*/ public hidebysig specialname 
          instance class System.Diagnostics.Activity/*02000009*/ 
          get_Parent() cil managed
  // SIG: 20 00 12 24
  {
    .custom /*0C0000A6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2135
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Parent

  .method /*06000037*/ public hidebysig specialname 
          instance string  get_ParentId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2138
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ParentId

  .method /*06000038*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ 
          get_ParentSpanId() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x213b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_ParentSpanId

  .method /*06000039*/ public hidebysig specialname 
          instance bool  get_Recorded() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x213e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Recorded

  .method /*0600003A*/ public hidebysig specialname 
          instance string  get_RootId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000AA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2141
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_RootId

  .method /*0600003B*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ 
          get_SpanId() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x2144
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_SpanId

  .method /*0600003C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/ 
          get_StartTimeUtc() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x2147
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_StartTimeUtc

  .method /*0600003D*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityStatusCode/*0200000C*/ 
          get_Status() cil managed
  // SIG: 20 00 11 30
  {
    // Method begins at RVA 0x214a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Status

  .method /*0600003E*/ public hidebysig specialname 
          instance string  get_StatusDescription() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_StatusDescription

  .method /*0600003F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 59 02 0E 0E
  {
    .param [0]/*0800002D*/ 
    .custom /*0C0000A0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Tags

  .method /*06000040*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_TagObjects() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 59 02 0E 1C
  {
    .param [0]/*0800002E*/ 
    .custom /*0C0000A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2153
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TagObjects

  .method /*06000041*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityEvent/*02000014*/> 
          get_Events() cil managed
  // SIG: 20 00 15 12 7D 01 11 50
  {
    // Method begins at RVA 0x2156
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Events

  .method /*06000042*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> 
          get_Links() cil managed
  // SIG: 20 00 15 12 7D 01 11 58
  {
    // Method begins at RVA 0x2159
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Links

  .method /*06000043*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*02000011*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x215c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceId

  .method /*06000044*/ public hidebysig specialname 
          instance string  get_TraceStateString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x215f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceStateString

  .method /*06000045*/ public hidebysig specialname 
          instance void  set_TraceStateString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000B7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2162
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Activity::set_TraceStateString

  .method /*06000046*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          AddBaggage(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 24 0E 0E
  {
    .param [2]/*08000031*/ 
    .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2164
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddBaggage

  .method /*06000047*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          AddEvent(valuetype System.Diagnostics.ActivityEvent/*02000014*/ e) cil managed
  // SIG: 20 01 12 24 11 50
  {
    // Method begins at RVA 0x2167
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddEvent

  .method /*06000048*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          AddTag(string key,
                 string 'value') cil managed
  // SIG: 20 02 12 24 0E 0E
  {
    .param [2]/*08000034*/ 
    .custom /*0C0000A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddTag

  .method /*06000049*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          AddTag(string key,
                 object 'value') cil managed
  // SIG: 20 02 12 24 0E 1C
  {
    .param [2]/*08000036*/ 
    .custom /*0C0000A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::AddTag

  .method /*0600004A*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          SetTag(string key,
                 object 'value') cil managed
  // SIG: 20 02 12 24 0E 1C
  {
    .param [2]/*08000038*/ 
    .custom /*0C0000A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2170
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetTag

  .method /*0600004B*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          SetBaggage(string key,
                     string 'value') cil managed
  // SIG: 20 02 12 24 0E 0E
  {
    .param [2]/*0800003A*/ 
    .custom /*0C0000AB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetBaggage

  .method /*0600004C*/ public hidebysig instance string 
          GetBaggageItem(string key) cil managed
  // SIG: 20 01 0E 0E
  {
    .param [0]/*0800003B*/ 
    .custom /*0C0000AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetBaggageItem

  .method /*0600004D*/ public hidebysig instance object 
          GetTagItem(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*0800003D*/ 
    .custom /*0C0000AF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetTagItem

  .method /*0600004E*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          SetEndTime(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/ endTimeUtc) cil managed
  // SIG: 20 01 12 24 11 80 85
  {
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetEndTime

  .method /*0600004F*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<valuetype System.Diagnostics.ActivityTraceId/*02000011*/> 
          get_TraceIdGenerator() cil managed
  // SIG: 00 00 15 12 80 89 01 11 44
  {
    .custom /*0C0000BF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_TraceIdGenerator

  .method /*06000050*/ public hidebysig specialname static 
          void  set_TraceIdGenerator(class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<valuetype System.Diagnostics.ActivityTraceId/*02000011*/> 'value') cil managed
  // SIG: 00 01 01 15 12 80 89 01 11 44
  {
    .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::set_TraceIdGenerator

  .method /*06000051*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          SetIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ format) cil managed
  // SIG: 20 01 12 24 11 34
  {
    // Method begins at RVA 0x2185
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetIdFormat

  .method /*06000052*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          SetParentId(valuetype System.Diagnostics.ActivityTraceId/*02000011*/ traceId,
                      valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ spanId,
                      [opt] valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ activityTraceFlags) cil managed
  // SIG: 20 03 12 24 11 44 11 38 11 40
  {
    .param [3]/*08000044*/  = int32(0x00000000)
    // Method begins at RVA 0x2188
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetParentId

  .method /*06000053*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          SetParentId(string parentId) cil managed
  // SIG: 20 01 12 24 0E
  {
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetParentId

  .method /*06000054*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          SetStartTime(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/ startTimeUtc) cil managed
  // SIG: 20 01 12 24 11 80 85
  {
    // Method begins at RVA 0x218e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetStartTime

  .method /*06000055*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          SetStatus(valuetype System.Diagnostics.ActivityStatusCode/*0200000C*/ code,
                    [opt] string description) cil managed
  // SIG: 20 02 12 24 11 30 0E
  {
    .param [2]/*08000048*/  = nullref
    .custom /*0C0000B8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2191
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetStatus

  .method /*06000056*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          Start() cil managed
  // SIG: 20 00 12 24
  {
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Start

  .method /*06000057*/ public hidebysig instance void 
          Stop() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2197
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Stop

  .method /*06000058*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Dispose

  .method /*06000059*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::Dispose

  .method /*0600005A*/ public hidebysig instance void 
          SetCustomProperty(string propertyName,
                            object propertyValue) cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*0800004B*/ 
    .custom /*0C0000BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::SetCustomProperty

  .method /*0600005B*/ public hidebysig instance object 
          GetCustomProperty(string propertyName) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*0800004C*/ 
    .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::GetCustomProperty

  .method /*0600005C*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityContext/*02000015*/ 
          get_Context() cil managed
  // SIG: 20 00 11 54
  {
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::get_Context

  .method /*0600005D*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000009*//Enumerator`1/*02000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          EnumerateTagObjects() cil managed
  // SIG: 20 00 15 11 80 A4 01 15 11 59 02 0E 1C
  {
    .param [0]/*0800004E*/ 
    .custom /*0C0000BD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::EnumerateTagObjects

  .method /*0600005E*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000009*//Enumerator`1/*02000029*/<valuetype System.Diagnostics.ActivityEvent/*02000014*/> 
          EnumerateEvents() cil managed
  // SIG: 20 00 15 11 80 A4 01 11 50
  {
    .custom /*0C0000D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::EnumerateEvents

  .method /*0600005F*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000009*//Enumerator`1/*02000029*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> 
          EnumerateLinks() cil managed
  // SIG: 20 00 15 11 80 A4 01 11 58
  {
    .custom /*0C0000D2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x21af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Activity::EnumerateLinks

  .event /*14000001*/ class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001E*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*0200000A*/>/*1B000002*/ CurrentChanged
  {
    .custom /*0C000004:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .addon void System.Diagnostics.Activity/*02000009*/::add_CurrentChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001E*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*0200000A*/>) /* 06000024 */
    .removeon void System.Diagnostics.Activity/*02000009*/::remove_CurrentChanged(class [System.Runtime/*23000001*/]System.EventHandler`1/*0100001E*/<valuetype System.Diagnostics.ActivityChangedEventArgs/*0200000A*/>) /* 06000025 */
  } // end of event Activity::CurrentChanged
  .property /*17000003*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/
          ActivityTraceFlags()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ System.Diagnostics.Activity/*02000009*/::get_ActivityTraceFlags() /* 0600001F */
    .set instance void System.Diagnostics.Activity/*02000009*/::set_ActivityTraceFlags(valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/) /* 06000020 */
  } // end of property Activity::ActivityTraceFlags
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>>
          Baggage()
  {
    .custom /*0C000024:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> System.Diagnostics.Activity/*02000009*/::get_Baggage() /* 06000021 */
  } // end of property Activity::Baggage
  .property /*17000005*/ callconv(8) class System.Diagnostics.Activity/*02000009*/
          Current()
  {
    .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get class System.Diagnostics.Activity/*02000009*/ System.Diagnostics.Activity/*02000009*/::get_Current() /* 06000022 */
    .set void System.Diagnostics.Activity/*02000009*/::set_Current(class System.Diagnostics.Activity/*02000009*/) /* 06000023 */
  } // end of property Activity::Current
  .property /*17000006*/ callconv(8) valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/
          DefaultIdFormat()
  {
    .get valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ System.Diagnostics.Activity/*02000009*/::get_DefaultIdFormat() /* 06000026 */
    .set void System.Diagnostics.Activity/*02000009*/::set_DefaultIdFormat(valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/) /* 06000027 */
  } // end of property Activity::DefaultIdFormat
  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000020*/
          Duration()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000020*/ System.Diagnostics.Activity/*02000009*/::get_Duration() /* 06000028 */
  } // end of property Activity::Duration
  .property /*17000008*/ callconv(8) bool
          ForceDefaultIdFormat()
  {
    .get bool System.Diagnostics.Activity/*02000009*/::get_ForceDefaultIdFormat() /* 06000029 */
    .set void System.Diagnostics.Activity/*02000009*/::set_ForceDefaultIdFormat(bool) /* 0600002A */
  } // end of property Activity::ForceDefaultIdFormat
  .property /*17000009*/ instance callconv(8) string
          Id()
  {
    .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000009*/::get_Id() /* 0600002B */
  } // end of property Activity::Id
  .property /*1700000A*/ instance callconv(8) bool
          HasRemoteParent()
  {
    .get instance bool System.Diagnostics.Activity/*02000009*/::get_HasRemoteParent() /* 0600002C */
  } // end of property Activity::HasRemoteParent
  .property /*1700000B*/ instance callconv(8) bool
          IsAllDataRequested()
  {
    .get instance bool System.Diagnostics.Activity/*02000009*/::get_IsAllDataRequested() /* 0600002D */
    .set instance void System.Diagnostics.Activity/*02000009*/::set_IsAllDataRequested(bool) /* 0600002E */
  } // end of property Activity::IsAllDataRequested
  .property /*1700000C*/ instance callconv(8) bool
          IsStopped()
  {
    .get instance bool System.Diagnostics.Activity/*02000009*/::get_IsStopped() /* 0600002F */
  } // end of property Activity::IsStopped
  .property /*1700000D*/ instance callconv(8) valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/
          IdFormat()
  {
    .get instance valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ System.Diagnostics.Activity/*02000009*/::get_IdFormat() /* 06000030 */
  } // end of property Activity::IdFormat
  .property /*1700000E*/ instance callconv(8) valuetype System.Diagnostics.ActivityKind/*02000013*/
          Kind()
  {
    .get instance valuetype System.Diagnostics.ActivityKind/*02000013*/ System.Diagnostics.Activity/*02000009*/::get_Kind() /* 06000031 */
  } // end of property Activity::Kind
  .property /*1700000F*/ instance callconv(8) string
          OperationName()
  {
    .get instance string System.Diagnostics.Activity/*02000009*/::get_OperationName() /* 06000032 */
  } // end of property Activity::OperationName
  .property /*17000010*/ instance callconv(8) string
          DisplayName()
  {
    .get instance string System.Diagnostics.Activity/*02000009*/::get_DisplayName() /* 06000033 */
    .set instance void System.Diagnostics.Activity/*02000009*/::set_DisplayName(string) /* 06000034 */
  } // end of property Activity::DisplayName
  .property /*17000011*/ instance callconv(8) class System.Diagnostics.ActivitySource/*0200000F*/
          Source()
  {
    .get instance class System.Diagnostics.ActivitySource/*0200000F*/ System.Diagnostics.Activity/*02000009*/::get_Source() /* 06000035 */
  } // end of property Activity::Source
  .property /*17000012*/ instance callconv(8) class System.Diagnostics.Activity/*02000009*/
          Parent()
  {
    .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Diagnostics.Activity/*02000009*/ System.Diagnostics.Activity/*02000009*/::get_Parent() /* 06000036 */
  } // end of property Activity::Parent
  .property /*17000013*/ instance callconv(8) string
          ParentId()
  {
    .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000009*/::get_ParentId() /* 06000037 */
  } // end of property Activity::ParentId
  .property /*17000014*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*0200000E*/
          ParentSpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ System.Diagnostics.Activity/*02000009*/::get_ParentSpanId() /* 06000038 */
  } // end of property Activity::ParentSpanId
  .property /*17000015*/ instance callconv(8) bool
          Recorded()
  {
    .get instance bool System.Diagnostics.Activity/*02000009*/::get_Recorded() /* 06000039 */
  } // end of property Activity::Recorded
  .property /*17000016*/ instance callconv(8) string
          RootId()
  {
    .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000009*/::get_RootId() /* 0600003A */
  } // end of property Activity::RootId
  .property /*17000017*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*0200000E*/
          SpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ System.Diagnostics.Activity/*02000009*/::get_SpanId() /* 0600003B */
  } // end of property Activity::SpanId
  .property /*17000018*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/
          StartTimeUtc()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000021*/ System.Diagnostics.Activity/*02000009*/::get_StartTimeUtc() /* 0600003C */
  } // end of property Activity::StartTimeUtc
  .property /*17000019*/ instance callconv(8) valuetype System.Diagnostics.ActivityStatusCode/*0200000C*/
          Status()
  {
    .get instance valuetype System.Diagnostics.ActivityStatusCode/*0200000C*/ System.Diagnostics.Activity/*02000009*/::get_Status() /* 0600003D */
  } // end of property Activity::Status
  .property /*1700001A*/ instance callconv(8) string
          StatusDescription()
  {
    .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000009*/::get_StatusDescription() /* 0600003E */
  } // end of property Activity::StatusDescription
  .property /*1700001B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>>
          Tags()
  {
    .custom /*0C000081:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> System.Diagnostics.Activity/*02000009*/::get_Tags() /* 0600003F */
  } // end of property Activity::Tags
  .property /*1700001C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          TagObjects()
  {
    .custom /*0C000085:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.Activity/*02000009*/::get_TagObjects() /* 06000040 */
  } // end of property Activity::TagObjects
  .property /*1700001D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityEvent/*02000014*/>
          Events()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityEvent/*02000014*/> System.Diagnostics.Activity/*02000009*/::get_Events() /* 06000041 */
  } // end of property Activity::Events
  .property /*1700001E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/>
          Links()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> System.Diagnostics.Activity/*02000009*/::get_Links() /* 06000042 */
  } // end of property Activity::Links
  .property /*1700001F*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*02000011*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*02000011*/ System.Diagnostics.Activity/*02000009*/::get_TraceId() /* 06000043 */
  } // end of property Activity::TraceId
  .property /*17000020*/ instance callconv(8) string
          TraceStateString()
  {
    .custom /*0C00008D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Activity/*02000009*/::get_TraceStateString() /* 06000044 */
    .set instance void System.Diagnostics.Activity/*02000009*/::set_TraceStateString(string) /* 06000045 */
  } // end of property Activity::TraceStateString
  .property /*17000021*/ callconv(8) class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<valuetype System.Diagnostics.ActivityTraceId/*02000011*/>
          TraceIdGenerator()
  {
    .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<valuetype System.Diagnostics.ActivityTraceId/*02000011*/> System.Diagnostics.Activity/*02000009*/::get_TraceIdGenerator() /* 0600004F */
    .set void System.Diagnostics.Activity/*02000009*/::set_TraceIdGenerator(class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<valuetype System.Diagnostics.ActivityTraceId/*02000011*/>) /* 06000050 */
  } // end of property Activity::TraceIdGenerator
  .property /*17000022*/ instance callconv(8) valuetype System.Diagnostics.ActivityContext/*02000015*/
          Context()
  {
    .get instance valuetype System.Diagnostics.ActivityContext/*02000015*/ System.Diagnostics.Activity/*02000009*/::get_Context() /* 0600005C */
  } // end of property Activity::Context
} // end of class System.Diagnostics.Activity

.class /*0200000A*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityChangedEventArgs
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
{
  .pack 0
  .size 1
  .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*06000060*/ public hidebysig specialname 
          instance class System.Diagnostics.Activity/*02000009*/ 
          get_Previous() cil managed
  // SIG: 20 00 12 24
  {
    // Method begins at RVA 0x21b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityChangedEventArgs::get_Previous

  .method /*06000061*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000027*/) 
          set_Previous(class System.Diagnostics.Activity/*02000009*/ 'value') cil managed
  // SIG: 20 01 1F 80 9D 01 12 24
  {
    // Method begins at RVA 0x21b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityChangedEventArgs::set_Previous

  .method /*06000062*/ public hidebysig specialname 
          instance class System.Diagnostics.Activity/*02000009*/ 
          get_Current() cil managed
  // SIG: 20 00 12 24
  {
    // Method begins at RVA 0x21b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityChangedEventArgs::get_Current

  .method /*06000063*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000027*/) 
          set_Current(class System.Diagnostics.Activity/*02000009*/ 'value') cil managed
  // SIG: 20 01 1F 80 9D 01 12 24
  {
    // Method begins at RVA 0x21bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityChangedEventArgs::set_Current

  .property /*17000023*/ instance callconv(8) class System.Diagnostics.Activity/*02000009*/
          Previous()
  {
    .get instance class System.Diagnostics.Activity/*02000009*/ System.Diagnostics.ActivityChangedEventArgs/*0200000A*/::get_Previous() /* 06000060 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000027*/) System.Diagnostics.ActivityChangedEventArgs/*0200000A*/::set_Previous(class System.Diagnostics.Activity/*02000009*/) /* 06000061 */
  } // end of property ActivityChangedEventArgs::Previous
  .property /*17000024*/ instance callconv(8) class System.Diagnostics.Activity/*02000009*/
          Current()
  {
    .get instance class System.Diagnostics.Activity/*02000009*/ System.Diagnostics.ActivityChangedEventArgs/*0200000A*/::get_Current() /* 06000062 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000027*/) System.Diagnostics.ActivityChangedEventArgs/*0200000A*/::set_Current(class System.Diagnostics.Activity/*02000009*/) /* 06000063 */
  } // end of property ActivityChangedEventArgs::Current
} // end of class System.Diagnostics.ActivityChangedEventArgs

.class /*0200000B*/ public auto ansi beforefieldinit System.Diagnostics.ActivityTagsCollection
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100002C*/<string,object>/*1B000004*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000005*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000003*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002A*/
{
  .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000042:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000028*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100002C*/<string,object>/*1B000004*/
  .custom /*0C000023:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000005*/
  .custom /*0C000028:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000003*/
  .custom /*0C00002E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002A*/
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002A*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000006*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100002B*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  {
    .pack 0
    .size 1
    .custom /*0C00009B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000006*/
    .custom /*0C00006D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .method /*06000148*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> 
            get_Current() cil managed
    // SIG: 20 00 15 11 59 02 0E 1C
    {
      .param [0]/*080001B2*/ 
      .custom /*0C00019B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      // Method begins at RVA 0x24dd
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*06000149*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100002B*/::get_Current /*0100002B::0A000018*/ 
      // Method begins at RVA 0x24e0
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*0600014A*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x24e3
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Dispose

    .method /*0600014B*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x24e6
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*0600014C*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100002B*/::Reset /*0100002B::0A000019*/ 
      // Method begins at RVA 0x24e9
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.Reset

    .property /*17000057*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>
            Current()
    {
      .custom /*0C0000C9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> System.Diagnostics.ActivityTagsCollection/*0200000B*//Enumerator/*0200002A*/::get_Current() /* 06000148 */
    } // end of property Enumerator::Current
    .property /*17000058*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .custom /*0C0000CB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .get instance object System.Diagnostics.ActivityTagsCollection/*0200000B*//Enumerator/*0200002A*/::System.Collections.IEnumerator.get_Current() /* 06000149 */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21be
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::.ctor

  .method /*06000065*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> list) cil managed
  // SIG: 20 01 01 15 12 7D 01 15 11 59 02 0E 1C
  {
    .param [1]/*08000051*/ 
    .custom /*0C0000C2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x21c7
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::.ctor

  .method /*06000066*/ public hidebysig newslot specialname virtual final 
          instance object  get_Item(string key) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*08000052*/ 
    .custom /*0C0000C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Item

  .method /*06000067*/ public hidebysig newslot specialname virtual final 
          instance void  set_Item(string key,
                                  object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000055*/ 
    .custom /*0C0000C7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivityTagsCollection::set_Item

  .method /*06000068*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<string> 
          get_Keys() cil managed
  // SIG: 20 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x21d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Keys

  .method /*06000069*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object> 
          get_Values() cil managed
  // SIG: 20 00 15 12 80 B5 01 1C
  {
    .param [0]/*08000056*/ 
    .custom /*0C0000C8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Values

  .method /*0600006A*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_Count

  .method /*0600006B*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::get_IsReadOnly

  .method /*0600006C*/ public hidebysig newslot virtual final 
          instance void  Add(string key,
                             object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*08000058*/ 
    .custom /*0C0000CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Add

  .method /*0600006D*/ public hidebysig newslot virtual final 
          instance void  Add(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 01 15 11 59 02 0E 1C
  {
    .param [1]/*08000059*/ 
    .custom /*0C0000CC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Add

  .method /*0600006E*/ public hidebysig newslot virtual final 
          instance void  Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Clear

  .method /*0600006F*/ public hidebysig newslot virtual final 
          instance bool  Contains(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 59 02 0E 1C
  {
    .param [1]/*0800005A*/ 
    .custom /*0C0000CE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Contains

  .method /*06000070*/ public hidebysig newslot virtual final 
          instance bool  ContainsKey(string key) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x21ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::ContainsKey

  .method /*06000071*/ public hidebysig newslot virtual final 
          instance void  CopyTo(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] 'array',
                                int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 59 02 0E 1C 08
  {
    .param [1]/*0800005C*/ 
    .custom /*0C0000D0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x21f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::CopyTo

  .method /*06000072*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 80 A5 01 15 11 59 02 0E 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000029*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000003*/::GetEnumerator() /* 0A000016 */
    // Method begins at RVA 0x21f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.GetEnumerator'

  .method /*06000073*/ public hidebysig newslot virtual final 
          instance bool  Remove(string key) cil managed
  // SIG: 20 01 02 0E
  {
    // Method begins at RVA 0x21f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Remove

  .method /*06000074*/ public hidebysig newslot virtual final 
          instance bool  Remove(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 59 02 0E 1C
  {
    .param [1]/*0800005F*/ 
    .custom /*0C0000D3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x21f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::Remove

  .method /*06000075*/ public hidebysig newslot virtual final 
          instance bool  TryGetValue(string key,
                                     [out] object& 'value') cil managed
  // SIG: 20 02 02 0E 10 1C
  {
    .param [2]/*08000061*/ 
    .custom /*0C0000D4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::TryGetValue

  .method /*06000076*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100002B*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 AD
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002A*/::GetEnumerator /*0100002A::0A000017*/ 
    // Method begins at RVA 0x21ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::System.Collections.IEnumerable.GetEnumerator

  .method /*06000077*/ public hidebysig instance valuetype System.Diagnostics.ActivityTagsCollection/*0200000B*//Enumerator/*0200002A*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 80 A8
  {
    // Method begins at RVA 0x2202
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTagsCollection::GetEnumerator

  .property /*17000025*/ instance callconv(8) object
          Item(string)
  {
    .custom /*0C000098:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object System.Diagnostics.ActivityTagsCollection/*0200000B*/::get_Item(string) /* 06000066 */
    .set instance void System.Diagnostics.ActivityTagsCollection/*0200000B*/::set_Item(string,
                                                                                       object) /* 06000067 */
  } // end of property ActivityTagsCollection::Item
  .property /*17000026*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<string>
          Keys()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<string> System.Diagnostics.ActivityTagsCollection/*0200000B*/::get_Keys() /* 06000068 */
  } // end of property ActivityTagsCollection::Keys
  .property /*17000027*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object>
          Values()
  {
    .custom /*0C000099:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object> System.Diagnostics.ActivityTagsCollection/*0200000B*/::get_Values() /* 06000069 */
  } // end of property ActivityTagsCollection::Values
  .property /*17000028*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Diagnostics.ActivityTagsCollection/*0200000B*/::get_Count() /* 0600006A */
  } // end of property ActivityTagsCollection::Count
  .property /*17000029*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Diagnostics.ActivityTagsCollection/*0200000B*/::get_IsReadOnly() /* 0600006B */
  } // end of property ActivityTagsCollection::IsReadOnly
} // end of class System.Diagnostics.ActivityTagsCollection

.class /*0200000C*/ public auto ansi sealed System.Diagnostics.ActivityStatusCode
       extends [System.Runtime/*23000001*/]System.Enum/*0100002E*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*0200000C*/ Unset = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*0200000C*/ Ok = int32(0x00000001)
  .field /*04000008*/ public static literal valuetype System.Diagnostics.ActivityStatusCode/*0200000C*/ Error = int32(0x00000002)
} // end of class System.Diagnostics.ActivityStatusCode

.class /*0200000D*/ public auto ansi sealed System.Diagnostics.ActivityIdFormat
       extends [System.Runtime/*23000001*/]System.Enum/*0100002E*/
{
  .field /*04000009*/ public specialname rtspecialname int32 value__
  .field /*0400000A*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ Unknown = int32(0x00000000)
  .field /*0400000B*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ Hierarchical = int32(0x00000001)
  .field /*0400000C*/ public static literal valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ W3C = int32(0x00000002)
} // end of class System.Diagnostics.ActivityIdFormat

.class /*0200000E*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivitySpanId
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<valuetype System.Diagnostics.ActivitySpanId/*0200000E*/>/*1B000007*/
{
  .custom /*0C000048:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*0400000D*/ private initonly object _dummy
  .field /*0400000E*/ private initonly int32 _dummyPrimitive
  .method /*06000078*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<uint8> destination) cil managed
  // SIG: 20 01 01 15 11 80 C1 01 05
  {
    // Method begins at RVA 0x2205
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivitySpanId::CopyTo

  .method /*06000079*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ 
          CreateFromBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<uint8> idData) cil managed
  // SIG: 00 01 11 38 15 11 80 C5 01 05
  {
    // Method begins at RVA 0x2207
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromBytes

  .method /*0600007A*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ 
          CreateFromString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<char> idData) cil managed
  // SIG: 00 01 11 38 15 11 80 C5 01 03
  {
    // Method begins at RVA 0x220a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromString

  .method /*0600007B*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ 
          CreateFromUtf8String(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<uint8> idData) cil managed
  // SIG: 00 01 11 38 15 11 80 C5 01 05
  {
    // Method begins at RVA 0x220d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateFromUtf8String

  .method /*0600007C*/ public hidebysig static 
          valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ 
          CreateRandom() cil managed
  // SIG: 00 00 11 38
  {
    // Method begins at RVA 0x2210
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::CreateRandom

  .method /*0600007D*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ spanId) cil managed
  // SIG: 20 01 02 11 38
  {
    // Method begins at RVA 0x2213
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::Equals

  .method /*0600007E*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000DD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000067*/ 
    .custom /*0C0000D5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000032*/::.ctor(bool) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2216
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::Equals

  .method /*0600007F*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2219
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::GetHashCode

  .method /*06000080*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ spanId1,
                            valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ spandId2) cil managed
  // SIG: 00 02 02 11 38 11 38
  {
    // Method begins at RVA 0x221c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::op_Equality

  .method /*06000081*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ spanId1,
                              valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ spandId2) cil managed
  // SIG: 00 02 02 11 38 11 38
  {
    // Method begins at RVA 0x221f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::op_Inequality

  .method /*06000082*/ public hidebysig instance string 
          ToHexString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2222
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::ToHexString

  .method /*06000083*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2225
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySpanId::ToString

} // end of class System.Diagnostics.ActivitySpanId

.class /*0200000F*/ public auto ansi sealed beforefieldinit System.Diagnostics.ActivitySource
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C00004C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000084*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               [opt] string version) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C0000E3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800006D*/  = ""
    .custom /*0C0000D6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2228
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivitySource::.ctor

  .method /*06000085*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2231
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::get_Name

  .method /*06000086*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2234
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::get_Version

  .method /*06000087*/ public hidebysig instance bool 
          HasListeners() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2237
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::HasListeners

  .method /*06000088*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*02000013*/ kind) cil managed
  // SIG: 20 02 12 24 0E 11 4C
  {
    .custom /*0C0000E6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800006E*/ 
    .custom /*0C0000D7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*06000089*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*02000013*/ kind,
                         valuetype System.Diagnostics.ActivityContext/*02000015*/ parentContext,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> links,
                         [opt] valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ idFormat) cil managed
  // SIG: 20 06 12 24 0E 11 4C 11 54 15 12 7D 01 15 11 59 02 0E 1C 15 12 7D 01 11 58 11 34
  {
    .param [1]/*08000071*/ 
    .custom /*0C0000D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000074*/  = nullref
    .custom /*0C0000D9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000075*/  = nullref
    .param [6]/*08000076*/  = int32(0x00000000)
    // Method begins at RVA 0x223d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*0600008A*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          CreateActivity(string name,
                         valuetype System.Diagnostics.ActivityKind/*02000013*/ kind,
                         string parentId,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                         [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> links,
                         [opt] valuetype System.Diagnostics.ActivityIdFormat/*0200000D*/ idFormat) cil managed
  // SIG: 20 06 12 24 0E 11 4C 0E 15 12 7D 01 15 11 59 02 0E 1C 15 12 7D 01 11 58 11 34
  {
    .param [1]/*08000077*/ 
    .custom /*0C0000DA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800007A*/  = nullref
    .custom /*0C0000DB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*0800007B*/  = nullref
    .param [6]/*0800007C*/  = int32(0x00000000)
    // Method begins at RVA 0x2240
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::CreateActivity

  .method /*0600008B*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          StartActivity([opt] string name,
                        [opt] valuetype System.Diagnostics.ActivityKind/*02000013*/ kind) cil managed
  // SIG: 20 02 12 24 0E 11 4C
  {
    .custom /*0C0000E8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800007D*/ 
    .custom /*0C0000DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800007E*/  = ""
    .custom /*0C0000DE:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CallerMemberNameAttribute/*01000033*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .param [2]/*0800007F*/  = int32(0x00000000)
    // Method begins at RVA 0x2243
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*0600008C*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          StartActivity(string name,
                        valuetype System.Diagnostics.ActivityKind/*02000013*/ kind,
                        valuetype System.Diagnostics.ActivityContext/*02000015*/ parentContext,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000034*/ startTime) cil managed
  // SIG: 20 06 12 24 0E 11 4C 11 54 15 12 7D 01 15 11 59 02 0E 1C 15 12 7D 01 11 58 11 80 D1
  {
    .param [1]/*08000080*/ 
    .custom /*0C0000DF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000083*/  = nullref
    .custom /*0C0000E2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*08000084*/  = nullref
    .param [6]/*08000085*/  = nullref
    // Method begins at RVA 0x2246
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*0600008D*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          StartActivity(string name,
                        valuetype System.Diagnostics.ActivityKind/*02000013*/ kind,
                        string parentId,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000034*/ startTime) cil managed
  // SIG: 20 06 12 24 0E 11 4C 0E 15 12 7D 01 15 11 59 02 0E 1C 15 12 7D 01 11 58 11 80 D1
  {
    .param [1]/*08000086*/ 
    .custom /*0C0000E5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000089*/  = nullref
    .custom /*0C0000E7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [5]/*0800008A*/  = nullref
    .param [6]/*0800008B*/  = nullref
    // Method begins at RVA 0x2249
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*0600008E*/ public hidebysig instance class System.Diagnostics.Activity/*02000009*/ 
          StartActivity(valuetype System.Diagnostics.ActivityKind/*02000013*/ kind,
                        [opt] valuetype System.Diagnostics.ActivityContext/*02000015*/ parentContext,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                        [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> links,
                        [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000034*/ startTime,
                        [opt] string name) cil managed
  // SIG: 20 06 12 24 11 4C 11 54 15 12 7D 01 15 11 59 02 0E 1C 15 12 7D 01 11 58 11 80 D1 0E
  {
    .param [2]/*0800008D*/  = nullref
    .param [3]/*0800008E*/  = nullref
    .custom /*0C0000E9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .param [4]/*0800008F*/  = nullref
    .param [5]/*08000090*/  = nullref
    .param [6]/*08000091*/  = ""
    .custom /*0C0000EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000EC:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CallerMemberNameAttribute/*01000033*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x224c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::StartActivity

  .method /*0600008F*/ public hidebysig static 
          void  AddActivityListener(class System.Diagnostics.ActivityListener/*02000019*/ listener) cil managed
  // SIG: 00 01 01 12 64
  {
    .custom /*0C0000EA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x224f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::AddActivityListener

  .method /*06000090*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2252
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivitySource::Dispose

  .property /*1700002A*/ instance callconv(8) string
          Name()
  {
    .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Diagnostics.ActivitySource/*0200000F*/::get_Name() /* 06000085 */
  } // end of property ActivitySource::Name
  .property /*1700002B*/ instance callconv(8) string
          Version()
  {
    .get instance string System.Diagnostics.ActivitySource/*0200000F*/::get_Version() /* 06000086 */
  } // end of property ActivitySource::Version
} // end of class System.Diagnostics.ActivitySource

.class /*02000010*/ public auto ansi sealed System.Diagnostics.ActivityTraceFlags
       extends [System.Runtime/*23000001*/]System.Enum/*0100002E*/
{
  .custom /*0C000050:0A00001C*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000035*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
  .field /*0400000F*/ public specialname rtspecialname int32 value__
  .field /*04000010*/ public static literal valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ None = int32(0x00000000)
  .field /*04000011*/ public static literal valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ Recorded = int32(0x00000001)
} // end of class System.Diagnostics.ActivityTraceFlags

.class /*02000011*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityTraceId
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<valuetype System.Diagnostics.ActivityTraceId/*02000011*/>/*1B000008*/
{
  .custom /*0C000054:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000012*/ private initonly object _dummy
  .field /*04000013*/ private initonly int32 _dummyPrimitive
  .method /*06000091*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<uint8> destination) cil managed
  // SIG: 20 01 01 15 11 80 C1 01 05
  {
    // Method begins at RVA 0x2255
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ActivityTraceId::CopyTo

  .method /*06000092*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*02000011*/ 
          CreateFromBytes(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<uint8> idData) cil managed
  // SIG: 00 01 11 44 15 11 80 C5 01 05
  {
    // Method begins at RVA 0x2257
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromBytes

  .method /*06000093*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*02000011*/ 
          CreateFromString(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<char> idData) cil managed
  // SIG: 00 01 11 44 15 11 80 C5 01 03
  {
    // Method begins at RVA 0x225a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromString

  .method /*06000094*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*02000011*/ 
          CreateFromUtf8String(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<uint8> idData) cil managed
  // SIG: 00 01 11 44 15 11 80 C5 01 05
  {
    // Method begins at RVA 0x225d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateFromUtf8String

  .method /*06000095*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityTraceId/*02000011*/ 
          CreateRandom() cil managed
  // SIG: 00 00 11 44
  {
    // Method begins at RVA 0x2260
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::CreateRandom

  .method /*06000096*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityTraceId/*02000011*/ traceId) cil managed
  // SIG: 20 01 02 11 44
  {
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::Equals

  .method /*06000097*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000ED:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000098*/ 
    .custom /*0C0000EE:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000032*/::.ctor(bool) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2266
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::Equals

  .method /*06000098*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2269
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::GetHashCode

  .method /*06000099*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityTraceId/*02000011*/ traceId1,
                            valuetype System.Diagnostics.ActivityTraceId/*02000011*/ traceId2) cil managed
  // SIG: 00 02 02 11 44 11 44
  {
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::op_Equality

  .method /*0600009A*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityTraceId/*02000011*/ traceId1,
                              valuetype System.Diagnostics.ActivityTraceId/*02000011*/ traceId2) cil managed
  // SIG: 00 02 02 11 44 11 44
  {
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::op_Inequality

  .method /*0600009B*/ public hidebysig instance string 
          ToHexString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000EF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::ToHexString

  .method /*0600009C*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000F0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2275
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityTraceId::ToString

} // end of class System.Diagnostics.ActivityTraceId

.class /*02000012*/ public auto ansi sealed System.Diagnostics.ActivitySamplingResult
       extends [System.Runtime/*23000001*/]System.Enum/*0100002E*/
{
  .field /*04000014*/ public specialname rtspecialname int32 value__
  .field /*04000015*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*02000012*/ None = int32(0x00000000)
  .field /*04000016*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*02000012*/ PropagationData = int32(0x00000001)
  .field /*04000017*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*02000012*/ AllData = int32(0x00000002)
  .field /*04000018*/ public static literal valuetype System.Diagnostics.ActivitySamplingResult/*02000012*/ AllDataAndRecorded = int32(0x00000003)
} // end of class System.Diagnostics.ActivitySamplingResult

.class /*02000013*/ public auto ansi sealed System.Diagnostics.ActivityKind
       extends [System.Runtime/*23000001*/]System.Enum/*0100002E*/
{
  .field /*04000019*/ public specialname rtspecialname int32 value__
  .field /*0400001A*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000013*/ Internal = int32(0x00000000)
  .field /*0400001B*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000013*/ Server = int32(0x00000001)
  .field /*0400001C*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000013*/ Client = int32(0x00000002)
  .field /*0400001D*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000013*/ Producer = int32(0x00000003)
  .field /*0400001E*/ public static literal valuetype System.Diagnostics.ActivityKind/*02000013*/ Consumer = int32(0x00000004)
} // end of class System.Diagnostics.ActivityKind

.class /*02000014*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityEvent
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
{
  .pack 0
  .size 1
  .custom /*0C000061:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000063:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*0600009D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2278
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::.ctor

  .method /*0600009E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               [opt] valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000034*/ timestamp,
                               [opt] class System.Diagnostics.ActivityTagsCollection/*0200000B*/ tags) cil managed
  // SIG: 20 03 01 0E 11 80 D1 12 2C
  {
    .param [2]/*0800009F*/  = nullref
    .param [3]/*080000A0*/  = nullref
    .custom /*0C0000F1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x227b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::.ctor

  .method /*0600009F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x227e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Name

  .method /*060000A0*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000034*/ 
          get_Timestamp() cil managed
  // SIG: 20 00 11 80 D1
  {
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Timestamp

  .method /*060000A1*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 59 02 0E 1C
  {
    .param [0]/*080000A1*/ 
    .custom /*0C0000F2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::get_Tags

  .method /*060000A2*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000009*//Enumerator`1/*02000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          EnumerateTagObjects() cil managed
  // SIG: 20 00 15 11 80 A4 01 15 11 59 02 0E 1C
  {
    .param [0]/*080000A2*/ 
    .custom /*0C0000F3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2287
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityEvent::EnumerateTagObjects

  .property /*1700002C*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.ActivityEvent/*02000014*/::get_Name() /* 0600009F */
  } // end of property ActivityEvent::Name
  .property /*1700002D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000034*/
          Timestamp()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000034*/ System.Diagnostics.ActivityEvent/*02000014*/::get_Timestamp() /* 060000A0 */
  } // end of property ActivityEvent::Timestamp
  .property /*1700002E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          Tags()
  {
    .custom /*0C0000A2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.ActivityEvent/*02000014*/::get_Tags() /* 060000A1 */
  } // end of property ActivityEvent::Tags
} // end of class System.Diagnostics.ActivityEvent

.class /*02000015*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityContext
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<valuetype System.Diagnostics.ActivityContext/*02000015*/>/*1B000009*/
{
  .pack 0
  .size 1
  .custom /*0C000067:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000069:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<valuetype System.Diagnostics.ActivityContext/*02000015*/>/*1B000009*/
  .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000A3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Diagnostics.ActivityTraceId/*02000011*/ traceId,
                               valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ spanId,
                               valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ traceFlags,
                               [opt] string traceState,
                               [opt] bool isRemote) cil managed
  // SIG: 20 05 01 11 44 11 38 11 40 0E 02
  {
    .param [4]/*080000A6*/  = nullref
    .param [5]/*080000A7*/  = bool(false)
    // Method begins at RVA 0x228a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::.ctor

  .method /*060000A4*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*02000011*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x228d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceId

  .method /*060000A5*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ 
          get_SpanId() cil managed
  // SIG: 20 00 11 38
  {
    // Method begins at RVA 0x2290
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_SpanId

  .method /*060000A6*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ 
          get_TraceFlags() cil managed
  // SIG: 20 00 11 40
  {
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceFlags

  .method /*060000A7*/ public hidebysig specialname 
          instance string  get_TraceState() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2296
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_TraceState

  .method /*060000A8*/ public hidebysig specialname 
          instance bool  get_IsRemote() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2299
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::get_IsRemote

  .method /*060000A9*/ public hidebysig static 
          bool  TryParse(string traceParent,
                         string traceState,
                         [out] valuetype System.Diagnostics.ActivityContext/*02000015*/& context) cil managed
  // SIG: 00 03 02 0E 0E 10 11 54
  {
    // Method begins at RVA 0x229c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::TryParse

  .method /*060000AA*/ public hidebysig static 
          bool  TryParse(string traceParent,
                         string traceState,
                         bool isRemote,
                         [out] valuetype System.Diagnostics.ActivityContext/*02000015*/& context) cil managed
  // SIG: 00 04 02 0E 0E 02 10 11 54
  {
    // Method begins at RVA 0x229f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::TryParse

  .method /*060000AB*/ public hidebysig static 
          valuetype System.Diagnostics.ActivityContext/*02000015*/ 
          Parse(string traceParent,
                string traceState) cil managed
  // SIG: 00 02 11 54 0E 0E
  {
    .custom /*0C0000F4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000B0*/ 
    .custom /*0C0000F5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Parse

  .method /*060000AC*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityContext/*02000015*/ left,
                            valuetype System.Diagnostics.ActivityContext/*02000015*/ right) cil managed
  // SIG: 00 02 02 11 54 11 54
  {
    // Method begins at RVA 0x22a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::op_Equality

  .method /*060000AD*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityContext/*02000015*/ left,
                              valuetype System.Diagnostics.ActivityContext/*02000015*/ right) cil managed
  // SIG: 00 02 02 11 54 11 54
  {
    // Method begins at RVA 0x22a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::op_Inequality

  .method /*060000AE*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityContext/*02000015*/ 'value') cil managed
  // SIG: 20 01 02 11 54
  {
    // Method begins at RVA 0x22ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Equals

  .method /*060000AF*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .param [1]/*080000B6*/ 
    .custom /*0C0000F8:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000032*/::.ctor(bool) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::Equals

  .method /*060000B0*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityContext::GetHashCode

  .property /*1700002F*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*02000011*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*02000011*/ System.Diagnostics.ActivityContext/*02000015*/::get_TraceId() /* 060000A4 */
  } // end of property ActivityContext::TraceId
  .property /*17000030*/ instance callconv(8) valuetype System.Diagnostics.ActivitySpanId/*0200000E*/
          SpanId()
  {
    .get instance valuetype System.Diagnostics.ActivitySpanId/*0200000E*/ System.Diagnostics.ActivityContext/*02000015*/::get_SpanId() /* 060000A5 */
  } // end of property ActivityContext::SpanId
  .property /*17000031*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/
          TraceFlags()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceFlags/*02000010*/ System.Diagnostics.ActivityContext/*02000015*/::get_TraceFlags() /* 060000A6 */
  } // end of property ActivityContext::TraceFlags
  .property /*17000032*/ instance callconv(8) string
          TraceState()
  {
    .get instance string System.Diagnostics.ActivityContext/*02000015*/::get_TraceState() /* 060000A7 */
  } // end of property ActivityContext::TraceState
  .property /*17000033*/ instance callconv(8) bool
          IsRemote()
  {
    .get instance bool System.Diagnostics.ActivityContext/*02000015*/::get_IsRemote() /* 060000A8 */
  } // end of property ActivityContext::IsRemote
} // end of class System.Diagnostics.ActivityContext

.class /*02000016*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityLink
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/>/*1B00000A*/
{
  .pack 0
  .size 1
  .custom /*0C00006B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*060000B1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Diagnostics.ActivityContext/*02000015*/ context,
                               [opt] class System.Diagnostics.ActivityTagsCollection/*0200000B*/ tags) cil managed
  // SIG: 20 02 01 11 54 12 2C
  {
    .custom /*0C0000F6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000B8*/  = nullref
    // Method begins at RVA 0x22b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::.ctor

  .method /*060000B2*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityContext/*02000015*/ 
          get_Context() cil managed
  // SIG: 20 00 11 54
  {
    // Method begins at RVA 0x22b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::get_Context

  .method /*060000B3*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 59 02 0E 1C
  {
    .param [0]/*080000B9*/ 
    .custom /*0C0000F9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x22ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::get_Tags

  .method /*060000B4*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0000F7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000BA*/ 
    .custom /*0C0000FA:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000032*/::.ctor(bool) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::Equals

  .method /*060000B5*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Diagnostics.ActivityLink/*02000016*/ 'value') cil managed
  // SIG: 20 01 02 11 58
  {
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::Equals

  .method /*060000B6*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Diagnostics.ActivityLink/*02000016*/ left,
                            valuetype System.Diagnostics.ActivityLink/*02000016*/ right) cil managed
  // SIG: 00 02 02 11 58 11 58
  {
    // Method begins at RVA 0x22c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::op_Equality

  .method /*060000B7*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Diagnostics.ActivityLink/*02000016*/ left,
                              valuetype System.Diagnostics.ActivityLink/*02000016*/ right) cil managed
  // SIG: 00 02 02 11 58 11 58
  {
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::op_Inequality

  .method /*060000B8*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::GetHashCode

  .method /*060000B9*/ public hidebysig instance valuetype System.Diagnostics.Activity/*02000009*//Enumerator`1/*02000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          EnumerateTagObjects() cil managed
  // SIG: 20 00 15 11 80 A4 01 15 11 59 02 0E 1C
  {
    .param [0]/*080000C0*/ 
    .custom /*0C0000FC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityLink::EnumerateTagObjects

  .property /*17000034*/ instance callconv(8) valuetype System.Diagnostics.ActivityContext/*02000015*/
          Context()
  {
    .get instance valuetype System.Diagnostics.ActivityContext/*02000015*/ System.Diagnostics.ActivityLink/*02000016*/::get_Context() /* 060000B2 */
  } // end of property ActivityLink::Context
  .property /*17000035*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          Tags()
  {
    .custom /*0C0000A5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.ActivityLink/*02000016*/::get_Tags() /* 060000B3 */
  } // end of property ActivityLink::Tags
} // end of class System.Diagnostics.ActivityLink

.class /*02000017*/ public sequential ansi sealed beforefieldinit System.Diagnostics.ActivityCreationOptions`1<T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
{
  .pack 0
  .size 1
  .custom /*0C000070:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000072:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .param type T /*2A000001*/ 
    .custom /*0C000016:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000BA*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivitySource/*0200000F*/ 
          get_Source() cil managed
  // SIG: 20 00 12 3C
  {
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Source

  .method /*060000BB*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Name

  .method /*060000BC*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityKind/*02000013*/ 
          get_Kind() cil managed
  // SIG: 20 00 11 4C
  {
    // Method begins at RVA 0x22d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Kind

  .method /*060000BD*/ public hidebysig specialname 
          instance !T  get_Parent() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x22d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Parent

  .method /*060000BE*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 59 02 0E 1C
  {
    .param [0]/*080000C1*/ 
    .custom /*0C0000FD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x22db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Tags

  .method /*060000BF*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> 
          get_Links() cil managed
  // SIG: 20 00 15 12 7D 01 11 58
  {
    .custom /*0C0000FB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_Links

  .method /*060000C0*/ public hidebysig specialname 
          instance class System.Diagnostics.ActivityTagsCollection/*0200000B*/ 
          get_SamplingTags() cil managed
  // SIG: 20 00 12 2C
  {
    // Method begins at RVA 0x22e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_SamplingTags

  .method /*060000C1*/ public hidebysig specialname 
          instance valuetype System.Diagnostics.ActivityTraceId/*02000011*/ 
          get_TraceId() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x22e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_TraceId

  .method /*060000C2*/ public hidebysig specialname 
          instance string  get_TraceState() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000FE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::get_TraceState

  .method /*060000C3*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000027*/) 
          set_TraceState(string 'value') cil managed
  // SIG: 20 01 1F 80 9D 01 0E
  {
    .custom /*0C0000FF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityCreationOptions`1::set_TraceState

  .property /*17000036*/ instance callconv(8) class System.Diagnostics.ActivitySource/*0200000F*/
          Source()
  {
    .get instance class System.Diagnostics.ActivitySource/*0200000F*/ System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_Source() /* 060000BA */
  } // end of property ActivityCreationOptions`1::Source
  .property /*17000037*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_Name() /* 060000BB */
  } // end of property ActivityCreationOptions`1::Name
  .property /*17000038*/ instance callconv(8) valuetype System.Diagnostics.ActivityKind/*02000013*/
          Kind()
  {
    .get instance valuetype System.Diagnostics.ActivityKind/*02000013*/ System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_Kind() /* 060000BC */
  } // end of property ActivityCreationOptions`1::Kind
  .property /*17000039*/ instance callconv(8) !T
          Parent()
  {
    .get instance !T System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_Parent() /* 060000BD */
  } // end of property ActivityCreationOptions`1::Parent
  .property /*1700003A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          Tags()
  {
    .custom /*0C0000AC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_Tags() /* 060000BE */
  } // end of property ActivityCreationOptions`1::Tags
  .property /*1700003B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/>
          Links()
  {
    .custom /*0C0000AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.ActivityLink/*02000016*/> System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_Links() /* 060000BF */
  } // end of property ActivityCreationOptions`1::Links
  .property /*1700003C*/ instance callconv(8) class System.Diagnostics.ActivityTagsCollection/*0200000B*/
          SamplingTags()
  {
    .get instance class System.Diagnostics.ActivityTagsCollection/*0200000B*/ System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_SamplingTags() /* 060000C0 */
  } // end of property ActivityCreationOptions`1::SamplingTags
  .property /*1700003D*/ instance callconv(8) valuetype System.Diagnostics.ActivityTraceId/*02000011*/
          TraceId()
  {
    .get instance valuetype System.Diagnostics.ActivityTraceId/*02000011*/ System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_TraceId() /* 060000C1 */
  } // end of property ActivityCreationOptions`1::TraceId
  .property /*1700003E*/ instance callconv(8) string
          TraceState()
  {
    .custom /*0C0000B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.ActivityCreationOptions`1/*02000017*/::get_TraceState() /* 060000C2 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000027*/) System.Diagnostics.ActivityCreationOptions`1/*02000017*/::set_TraceState(string) /* 060000C3 */
  } // end of property ActivityCreationOptions`1::TraceState
} // end of class System.Diagnostics.ActivityCreationOptions`1

.class /*02000018*/ public auto ansi sealed System.Diagnostics.SampleActivity`1<T>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000036*/
{
  .param type T /*2A000002*/ 
    .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060000C4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method SampleActivity`1::.ctor

  .method /*060000C5*/ public hidebysig newslot virtual 
          instance valuetype System.Diagnostics.ActivitySamplingResult/*02000012*/ 
          Invoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000017*/<!T>& options) runtime managed
  // SIG: 20 01 11 48 10 15 11 5C 01 13 00
  {
    .param [1]/*080000C5*/ 
    .custom /*0C000100:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::Invoke

  .method /*060000C6*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000037*/ 
          BeginInvoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000017*/<!T>& options,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000038*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 80 DD 10 15 11 5C 01 13 00 12 80 E1 1C
  {
    .param [1]/*080000C6*/ 
    .custom /*0C000101:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::BeginInvoke

  .method /*060000C7*/ public hidebysig newslot virtual 
          instance valuetype System.Diagnostics.ActivitySamplingResult/*02000012*/ 
          EndInvoke(valuetype System.Diagnostics.ActivityCreationOptions`1/*02000017*/<!T>& options,
                    class [System.Runtime/*23000001*/]System.IAsyncResult/*01000037*/ result) runtime managed
  // SIG: 20 02 11 48 10 15 11 5C 01 13 00 12 80 DD
  {
    .param [1]/*080000C9*/ 
    .custom /*0C000102:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method SampleActivity`1::EndInvoke

} // end of class System.Diagnostics.SampleActivity`1

.class /*02000019*/ public auto ansi sealed beforefieldinit System.Diagnostics.ActivityListener
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C000078:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22ed
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method ActivityListener::.ctor

  .method /*060000C9*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/> 
          get_ActivityStarted() cil managed
  // SIG: 20 00 15 12 80 E5 01 12 24
  {
    .param [0]/*080000CB*/ 
    .custom /*0C000103:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ActivityStarted

  .method /*060000CA*/ public hidebysig specialname 
          instance void  set_ActivityStarted(class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 E5 01 12 24
  {
    .param [1]/*080000CC*/ 
    .custom /*0C000104:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ActivityStarted

  .method /*060000CB*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/> 
          get_ActivityStopped() cil managed
  // SIG: 20 00 15 12 80 E5 01 12 24
  {
    .param [0]/*080000CD*/ 
    .custom /*0C000105:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ActivityStopped

  .method /*060000CC*/ public hidebysig specialname 
          instance void  set_ActivityStopped(class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 E5 01 12 24
  {
    .param [1]/*080000CE*/ 
    .custom /*0C000106:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x22ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ActivityStopped

  .method /*060000CD*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100003A*/<class System.Diagnostics.ActivitySource/*0200000F*/,bool> 
          get_ShouldListenTo() cil managed
  // SIG: 20 00 15 12 80 E9 02 12 3C 02
  {
    .param [0]/*080000CF*/ 
    .custom /*0C000107:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2302
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_ShouldListenTo

  .method /*060000CE*/ public hidebysig specialname 
          instance void  set_ShouldListenTo(class [System.Runtime/*23000001*/]System.Func`2/*0100003A*/<class System.Diagnostics.ActivitySource/*0200000F*/,bool> 'value') cil managed
  // SIG: 20 01 01 15 12 80 E9 02 12 3C 02
  {
    .param [1]/*080000D0*/ 
    .custom /*0C000108:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2305
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_ShouldListenTo

  .method /*060000CF*/ public hidebysig specialname 
          instance class System.Diagnostics.SampleActivity`1/*02000018*/<string> 
          get_SampleUsingParentId() cil managed
  // SIG: 20 00 15 12 60 01 0E
  {
    .param [0]/*080000D1*/ 
    .custom /*0C000109:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x2308
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_SampleUsingParentId

  .method /*060000D0*/ public hidebysig specialname 
          instance void  set_SampleUsingParentId(class System.Diagnostics.SampleActivity`1/*02000018*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 60 01 0E
  {
    .param [1]/*080000D2*/ 
    .custom /*0C00010A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x230b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_SampleUsingParentId

  .method /*060000D1*/ public hidebysig specialname 
          instance class System.Diagnostics.SampleActivity`1/*02000018*/<valuetype System.Diagnostics.ActivityContext/*02000015*/> 
          get_Sample() cil managed
  // SIG: 20 00 15 12 60 01 11 54
  {
    // Method begins at RVA 0x230e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::get_Sample

  .method /*060000D2*/ public hidebysig specialname 
          instance void  set_Sample(class System.Diagnostics.SampleActivity`1/*02000018*/<valuetype System.Diagnostics.ActivityContext/*02000015*/> 'value') cil managed
  // SIG: 20 01 01 15 12 60 01 11 54
  {
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::set_Sample

  .method /*060000D3*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2314
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivityListener::Dispose

  .property /*1700003F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/>
          ActivityStarted()
  {
    .custom /*0C0000B2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/> System.Diagnostics.ActivityListener/*02000019*/::get_ActivityStarted() /* 060000C9 */
    .set instance void System.Diagnostics.ActivityListener/*02000019*/::set_ActivityStarted(class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/>) /* 060000CA */
  } // end of property ActivityListener::ActivityStarted
  .property /*17000040*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/>
          ActivityStopped()
  {
    .custom /*0C0000B3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/> System.Diagnostics.ActivityListener/*02000019*/::get_ActivityStopped() /* 060000CB */
    .set instance void System.Diagnostics.ActivityListener/*02000019*/::set_ActivityStopped(class [System.Runtime/*23000001*/]System.Action`1/*01000039*/<class System.Diagnostics.Activity/*02000009*/>) /* 060000CC */
  } // end of property ActivityListener::ActivityStopped
  .property /*17000041*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100003A*/<class System.Diagnostics.ActivitySource/*0200000F*/,bool>
          ShouldListenTo()
  {
    .custom /*0C0000B4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100003A*/<class System.Diagnostics.ActivitySource/*0200000F*/,bool> System.Diagnostics.ActivityListener/*02000019*/::get_ShouldListenTo() /* 060000CD */
    .set instance void System.Diagnostics.ActivityListener/*02000019*/::set_ShouldListenTo(class [System.Runtime/*23000001*/]System.Func`2/*0100003A*/<class System.Diagnostics.ActivitySource/*0200000F*/,bool>) /* 060000CE */
  } // end of property ActivityListener::ShouldListenTo
  .property /*17000042*/ instance callconv(8) class System.Diagnostics.SampleActivity`1/*02000018*/<string>
          SampleUsingParentId()
  {
    .custom /*0C0000B5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class System.Diagnostics.SampleActivity`1/*02000018*/<string> System.Diagnostics.ActivityListener/*02000019*/::get_SampleUsingParentId() /* 060000CF */
    .set instance void System.Diagnostics.ActivityListener/*02000019*/::set_SampleUsingParentId(class System.Diagnostics.SampleActivity`1/*02000018*/<string>) /* 060000D0 */
  } // end of property ActivityListener::SampleUsingParentId
  .property /*17000043*/ instance callconv(8) class System.Diagnostics.SampleActivity`1/*02000018*/<valuetype System.Diagnostics.ActivityContext/*02000015*/>
          Sample()
  {
    .get instance class System.Diagnostics.SampleActivity`1/*02000018*/<valuetype System.Diagnostics.ActivityContext/*02000015*/> System.Diagnostics.ActivityListener/*02000019*/::get_Sample() /* 060000D1 */
    .set instance void System.Diagnostics.ActivityListener/*02000019*/::set_Sample(class System.Diagnostics.SampleActivity`1/*02000018*/<valuetype System.Diagnostics.ActivityContext/*02000015*/>) /* 060000D2 */
  } // end of property ActivityListener::Sample
} // end of class System.Diagnostics.ActivityListener

.class /*0200001A*/ public abstract auto ansi beforefieldinit System.Diagnostics.DistributedContextPropagator
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00007C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002B*/ auto ansi sealed nested public PropagatorGetterCallback
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000036*/
  {
    .custom /*0C00009E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .method /*0600014D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method PropagatorGetterCallback::.ctor

    .method /*0600014E*/ public hidebysig newslot virtual 
            instance void  Invoke(object carrier,
                                  string fieldName,
                                  [out] string& fieldValue,
                                  [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<string>& fieldValues) runtime managed
    // SIG: 20 04 01 1C 0E 10 0E 10 15 12 7D 01 0E
    {
      .custom /*0C000163:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
      .param [2]/*080001B6*/ 
      .custom /*0C00019C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .param [4]/*080001B8*/ 
      .custom /*0C00019D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::Invoke

    .method /*0600014F*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000037*/ 
            BeginInvoke(object carrier,
                        string fieldName,
                        [out] string& fieldValue,
                        [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<string>& fieldValues,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*01000038*/ callback,
                        object 'object') runtime managed
    // SIG: 20 06 12 80 DD 1C 0E 10 0E 10 15 12 7D 01 0E 12 80 E1 1C
    {
      .param [1]/*080001B9*/ 
      .custom /*0C00019E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .param [2]/*080001BA*/ 
      .custom /*0C00019F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .param [3]/*080001BB*/ 
      .custom /*0C0001A0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .param [4]/*080001BC*/ 
      .custom /*0C0001A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::BeginInvoke

    .method /*06000150*/ public hidebysig newslot virtual 
            instance void  EndInvoke([out] string& fieldValue,
                                     [out] class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<string>& fieldValues,
                                     class [System.Runtime/*23000001*/]System.IAsyncResult/*01000037*/ result) runtime managed
    // SIG: 20 03 01 10 0E 10 15 12 7D 01 0E 12 80 DD
    {
      .param [1]/*080001BF*/ 
      .custom /*0C0001A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .param [2]/*080001C0*/ 
      .custom /*0C0001A3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    } // end of method PropagatorGetterCallback::EndInvoke

  } // end of class PropagatorGetterCallback

  .class /*0200002C*/ auto ansi sealed nested public PropagatorSetterCallback
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000036*/
  {
    .custom /*0C00009F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .method /*06000151*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method PropagatorSetterCallback::.ctor

    .method /*06000152*/ public hidebysig newslot virtual 
            instance void  Invoke(object carrier,
                                  string fieldName,
                                  string fieldValue) runtime managed
    // SIG: 20 03 01 1C 0E 0E
    {
      .custom /*0C000167:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
      .param [1]/*080001C4*/ 
      .custom /*0C0001A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    } // end of method PropagatorSetterCallback::Invoke

    .method /*06000153*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000037*/ 
            BeginInvoke(object carrier,
                        string fieldName,
                        string fieldValue,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*01000038*/ callback,
                        object 'object') runtime managed
    // SIG: 20 05 12 80 DD 1C 0E 0E 12 80 E1 1C
    {
      .param [1]/*080001C7*/ 
      .custom /*0C0001A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .param [2]/*080001C8*/ 
      .custom /*0C0001A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .param [3]/*080001C9*/ 
      .custom /*0C0001A7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    } // end of method PropagatorSetterCallback::BeginInvoke

    .method /*06000154*/ public hidebysig newslot virtual 
            instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000037*/ result) runtime managed
    // SIG: 20 01 01 12 80 DD
    {
    } // end of method PropagatorSetterCallback::EndInvoke

  } // end of class PropagatorSetterCallback

  .field /*0400001F*/ private static class System.Diagnostics.DistributedContextPropagator/*0200001A*/ '<Current>k__BackingField'
  .custom /*0C000089:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
  .method /*060000D4*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003B*/<string> 
          get_Fields() cil managed
  // SIG: 20 00 15 12 80 ED 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::get_Fields

  .method /*060000D5*/ public hidebysig newslot abstract virtual 
          instance void  Inject(class System.Diagnostics.Activity/*02000009*/ activity,
                                object carrier,
                                class System.Diagnostics.DistributedContextPropagator/*0200001A*//PropagatorSetterCallback/*0200002C*/ setter) cil managed
  // SIG: 20 03 01 12 24 1C 12 80 B0
  {
    .custom /*0C00010B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::Inject

  .method /*060000D6*/ public hidebysig newslot abstract virtual 
          instance void  ExtractTraceIdAndState(object carrier,
                                                class System.Diagnostics.DistributedContextPropagator/*0200001A*//PropagatorGetterCallback/*0200002B*/ getter,
                                                [out] string& traceId,
                                                [out] string& traceState) cil managed
  // SIG: 20 04 01 1C 12 80 AC 10 0E 10 0E
  {
    .custom /*0C00010C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::ExtractTraceIdAndState

  .method /*060000D7*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,string>> 
          ExtractBaggage(object carrier,
                         class System.Diagnostics.DistributedContextPropagator/*0200001A*//PropagatorGetterCallback/*0200002B*/ getter) cil managed
  // SIG: 20 02 15 12 7D 01 15 11 59 02 0E 0E 1C 12 80 AC
  {
    .custom /*0C00010D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000DB*/ 
    .custom /*0C000110:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DistributedContextPropagator::ExtractBaggage

  .method /*060000D8*/ public hidebysig specialname static 
          class System.Diagnostics.DistributedContextPropagator/*0200001A*/ 
          get_Current() cil managed
  // SIG: 00 00 12 68
  {
    .custom /*0C00010E:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2317
    // Code size       6 (0x6)
    .maxstack  8
    IL_0000:  /* 7E   | (04)00001F       */ ldsfld     class System.Diagnostics.DistributedContextPropagator/*0200001A*/ System.Diagnostics.DistributedContextPropagator/*0200001A*/::'<Current>k__BackingField' /* 0400001F */
    IL_0005:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::get_Current

  .method /*060000D9*/ public hidebysig specialname static 
          void  set_Current(class System.Diagnostics.DistributedContextPropagator/*0200001A*/ 'value') cil managed
  // SIG: 00 01 01 12 68
  {
    .custom /*0C00010F:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000010*/::.ctor() /* 0A00000F */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x231e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 80   | (04)00001F       */ stsfld     class System.Diagnostics.DistributedContextPropagator/*0200001A*/ System.Diagnostics.DistributedContextPropagator/*0200001A*/::'<Current>k__BackingField' /* 0400001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::set_Current

  .method /*060000DA*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*0200001A*/ 
          CreateDefaultPropagator() cil managed
  // SIG: 00 00 12 68
  {
    // Method begins at RVA 0x2326
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreateDefaultPropagator

  .method /*060000DB*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*0200001A*/ 
          CreatePassThroughPropagator() cil managed
  // SIG: 00 00 12 68
  {
    // Method begins at RVA 0x2329
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreatePassThroughPropagator

  .method /*060000DC*/ public hidebysig static 
          class System.Diagnostics.DistributedContextPropagator/*0200001A*/ 
          CreateNoOutputPropagator() cil managed
  // SIG: 00 00 12 68
  {
    // Method begins at RVA 0x232c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DistributedContextPropagator::CreateNoOutputPropagator

  .method /*060000DD*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x232f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DistributedContextPropagator::.ctor

  .property /*17000044*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003B*/<string>
          Fields()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003B*/<string> System.Diagnostics.DistributedContextPropagator/*0200001A*/::get_Fields() /* 060000D4 */
  } // end of property DistributedContextPropagator::Fields
  .property /*17000045*/ callconv(8) class System.Diagnostics.DistributedContextPropagator/*0200001A*/
          Current()
  {
    .get class System.Diagnostics.DistributedContextPropagator/*0200001A*/ System.Diagnostics.DistributedContextPropagator/*0200001A*/::get_Current() /* 060000D8 */
    .set void System.Diagnostics.DistributedContextPropagator/*0200001A*/::set_Current(class System.Diagnostics.DistributedContextPropagator/*0200001A*/) /* 060000D9 */
  } // end of property DistributedContextPropagator::Current
} // end of class System.Diagnostics.DistributedContextPropagator

.class /*0200001B*/ public sequential ansi sealed beforefieldinit System.Diagnostics.TagList
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100003C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000B*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000005*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000003*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002A*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000C*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003B*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000D*/
{
  .custom /*0C000080:0A000015*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000028*/::.ctor(string) /* 0A000015 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100003C*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000B*/
  .custom /*0C00004A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000005*/
  .custom /*0C00004E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000003*/
  .custom /*0C000052:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100003D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000C*/
  .custom /*0C00005A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*0100003B*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B00000D*/
  .custom /*0C00005E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .class /*0200002D*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000006*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100002B*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  {
    .pack 0
    .size 1
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>/*1B000006*/
    .custom /*0C00007A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .method /*06000155*/ public hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> 
            get_Current() cil managed
    // SIG: 20 00 15 11 59 02 0E 1C
    {
      .param [0]/*080001CD*/ 
      .custom /*0C0001A8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      // Method begins at RVA 0x24ec
      // Code size       2 (0x2)
      .maxstack  1
      .locals /*11000001*/ init (valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> V_0)
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*06000156*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100002B*/::get_Current /*0100002B::0A000018*/ 
      // Method begins at RVA 0x24fa
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*06000157*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x24fd
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Dispose

    .method /*06000158*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x2500
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*06000159*/ public hidebysig newslot virtual final 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x2503
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Reset

    .property /*17000059*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>
            Current()
    {
      .custom /*0C0000CD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> System.Diagnostics.TagList/*0200001B*//Enumerator/*0200002D*/::get_Current() /* 06000155 */
    } // end of property Enumerator::Current
    .property /*1700005A*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .custom /*0C0000CF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .get instance object System.Diagnostics.TagList/*0200001B*//Enumerator/*0200002D*/::System.Collections.IEnumerator.get_Current() /* 06000156 */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .method /*060000DE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tagList) cil managed
  // SIG: 20 01 01 15 11 80 C5 01 15 11 59 02 0E 1C
  {
    .param [1]/*080000DF*/ 
    .custom /*0C000112:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2337
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* FE15 | (02)00001B       */ initobj    System.Diagnostics.TagList/*0200001B*/
    IL_0007:  /* 14   |                  */ ldnull
    IL_0008:  /* 7A   |                  */ throw
  } // end of method TagList::.ctor

  .method /*060000DF*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000111:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2341
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_Count

  .method /*060000E0*/ public hidebysig newslot specialname virtual final 
          instance bool  get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000113:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2344
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_IsReadOnly

  .method /*060000E1*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> 
          get_Item(int32 index) cil managed
  // SIG: 20 01 15 11 59 02 0E 1C 08
  {
    .custom /*0C000115:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .param [0]/*080000E0*/ 
    .custom /*0C000114:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2347
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::get_Item

  .method /*060000E2*/ public hidebysig newslot specialname virtual final 
          instance void  set_Item(int32 index,
                                  valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> 'value') cil managed
  // SIG: 20 02 01 08 15 11 59 02 0E 1C
  {
    .param [2]/*080000E3*/ 
    .custom /*0C000117:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x234a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::set_Item

  .method /*060000E3*/ public hidebysig instance void 
          Add(string key,
              object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .custom /*0C000116:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000E5*/ 
    .custom /*0C000119:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x234d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Add

  .method /*060000E4*/ public hidebysig newslot virtual final 
          instance void  Add(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 01 01 15 11 59 02 0E 1C
  {
    .param [1]/*080000E6*/ 
    .custom /*0C00011A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2350
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Add

  .method /*060000E5*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 01 01 15 11 80 C1 01 15 11 59 02 0E 1C
  {
    .custom /*0C000118:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .param [1]/*080000E7*/ 
    .custom /*0C00011B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2353
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::CopyTo

  .method /*060000E6*/ public hidebysig newslot virtual final 
          instance void  Insert(int32 index,
                                valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 02 01 08 15 11 59 02 0E 1C
  {
    .param [2]/*080000E9*/ 
    .custom /*0C00011D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2356
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Insert

  .method /*060000E7*/ public hidebysig newslot virtual final 
          instance void  RemoveAt(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2359
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::RemoveAt

  .method /*060000E8*/ public hidebysig newslot virtual final 
          instance void  Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x235c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Clear

  .method /*060000E9*/ public hidebysig newslot virtual final 
          instance bool  Contains(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 59 02 0E 1C
  {
    .custom /*0C00011C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .param [1]/*080000EB*/ 
    .custom /*0C00011F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x235f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Contains

  .method /*060000EA*/ public hidebysig newslot virtual final 
          instance void  CopyTo(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] 'array',
                                int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 59 02 0E 1C 08
  {
    .custom /*0C00011E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .param [1]/*080000EC*/ 
    .custom /*0C000121:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2362
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::CopyTo

  .method /*060000EB*/ public hidebysig newslot virtual final 
          instance bool  Remove(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 02 15 11 59 02 0E 1C
  {
    .param [1]/*080000EE*/ 
    .custom /*0C000124:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2365
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::Remove

  .method /*060000EC*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000029*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 80 A5 01 15 11 59 02 0E 1C
  {
    .custom /*0C000120:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .param [0]/*080000EF*/ 
    .custom /*0C000125:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2368
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::GetEnumerator

  .method /*060000ED*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100002B*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 AD
  {
    .custom /*0C000122:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*0100002A*/::GetEnumerator /*0100002A::0A000017*/ 
    // Method begins at RVA 0x236b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::System.Collections.IEnumerable.GetEnumerator

  .method /*060000EE*/ public hidebysig newslot virtual final 
          instance int32  IndexOf(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> item) cil managed
  // SIG: 20 01 08 15 11 59 02 0E 1C
  {
    .custom /*0C000123:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .param [1]/*080000F0*/ 
    .custom /*0C000126:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x236e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TagList::IndexOf

  .property /*17000046*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 System.Diagnostics.TagList/*0200001B*/::get_Count() /* 060000DF */
  } // end of property TagList::Count
  .property /*17000047*/ instance callconv(8) bool
          IsReadOnly()
  {
    .get instance bool System.Diagnostics.TagList/*0200001B*/::get_IsReadOnly() /* 060000E0 */
  } // end of property TagList::IsReadOnly
  .property /*17000048*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>
          Item(int32)
  {
    .custom /*0C0000B9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> System.Diagnostics.TagList/*0200001B*/::get_Item(int32) /* 060000E1 */
    .set instance void System.Diagnostics.TagList/*0200001B*/::set_Item(int32,
                                                                        valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>) /* 060000E2 */
  } // end of property TagList::Item
} // end of class System.Diagnostics.TagList

.class /*0200001C*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.Counter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*02000020*/<!T>/*1B00000E*/
{
  .method /*060000EF*/ public hidebysig instance void 
          Add(!T delta) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F0*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 59 02 0E 1C
  {
    .param [2]/*080000F3*/ 
    .custom /*0C000127:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2374
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F1*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*080000F5*/ 
    .custom /*0C000128:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*080000F6*/ 
    .custom /*0C000129:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F2*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*080000F8*/ 
    .custom /*0C00012A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*080000F9*/ 
    .custom /*0C00012B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*080000FA*/ 
    .custom /*0C00012C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F3*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 C5 01 15 11 59 02 0E 1C
  {
    .param [2]/*080000FC*/ 
    .custom /*0C00012D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x237d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F4*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 59 02 0E 1C
  {
    .param [2]/*080000FE*/ 
    .custom /*0C00012E:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100003E*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    .custom /*0C00012F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x2380
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F5*/ public hidebysig instance void 
          Add(!T delta,
              [in] valuetype System.Diagnostics.TagList/*0200001B*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 6C
  {
    .param [2]/*08000100*/ 
    .custom /*0C000130:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2383
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Counter`1::Add

  .method /*060000F6*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    // Method begins at RVA 0x2386
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000020       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*02000020*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000023*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string) /* 0A000020 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Counter`1::.ctor

} // end of class System.Diagnostics.Metrics.Counter`1

.class /*0200001D*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.UpDownCounter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*02000020*/<!T>/*1B00000E*/
{
  .method /*060000F7*/ public hidebysig instance void 
          Add(!T delta) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2394
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*060000F8*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 59 02 0E 1C
  {
    .param [2]/*08000107*/ 
    .custom /*0C000132:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2397
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*060000F9*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*08000109*/ 
    .custom /*0C000133:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*0800010A*/ 
    .custom /*0C000134:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x239a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*060000FA*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*0800010C*/ 
    .custom /*0C000135:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*0800010D*/ 
    .custom /*0C000136:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*0800010E*/ 
    .custom /*0C000138:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x239d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*060000FB*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 C5 01 15 11 59 02 0E 1C
  {
    .param [2]/*08000110*/ 
    .custom /*0C00013A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*060000FC*/ public hidebysig instance void 
          Add(!T delta,
              valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 59 02 0E 1C
  {
    .param [2]/*08000112*/ 
    .custom /*0C00013B:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100003E*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    .custom /*0C00013C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x23a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*060000FD*/ public hidebysig instance void 
          Add(!T delta,
              [in] valuetype System.Diagnostics.TagList/*0200001B*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 6C
  {
    .param [2]/*08000114*/ 
    .custom /*0C00013D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::Add

  .method /*060000FE*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    // Method begins at RVA 0x23a9
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000020       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*02000020*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000023*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string) /* 0A000020 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method UpDownCounter`1::.ctor

} // end of class System.Diagnostics.Metrics.UpDownCounter`1

.class /*0200001E*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.Histogram`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends class System.Diagnostics.Metrics.Instrument`1/*02000020*/<!T>/*1B00000E*/
{
  .method /*060000FF*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    // Method begins at RVA 0x23b7
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000020       */ call       instance void class System.Diagnostics.Metrics.Instrument`1/*02000020*/<!T>/*1B00000E*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000023*/,
                                                                                                                                                      string,
                                                                                                                                                      string,
                                                                                                                                                      string) /* 0A000020 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Histogram`1::.ctor

  .method /*06000100*/ public hidebysig instance void 
          Record(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x23c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000101*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 59 02 0E 1C
  {
    .param [2]/*0800011F*/ 
    .custom /*0C00013F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000102*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*08000121*/ 
    .custom /*0C000140:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000122*/ 
    .custom /*0C000141:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000103*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2,
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*08000124*/ 
    .custom /*0C000142:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*08000125*/ 
    .custom /*0C000143:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000126*/ 
    .custom /*0C000144:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000104*/ public hidebysig instance void 
          Record(!T 'value',
                 [in] valuetype System.Diagnostics.TagList/*0200001B*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 6C
  {
    .param [2]/*08000128*/ 
    .custom /*0C000145:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000105*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 C5 01 15 11 59 02 0E 1C
  {
    .param [2]/*0800012A*/ 
    .custom /*0C000146:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x23d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

  .method /*06000106*/ public hidebysig instance void 
          Record(!T 'value',
                 valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 59 02 0E 1C
  {
    .param [2]/*0800012C*/ 
    .custom /*0C000147:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100003E*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    .custom /*0C000148:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x23d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Histogram`1::Record

} // end of class System.Diagnostics.Metrics.Histogram`1

.class /*0200001F*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.Instrument
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000107*/ public hidebysig specialname 
          instance string  get_Description() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000131:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Description

  .method /*06000108*/ public hidebysig specialname 
          instance bool  get_Enabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Enabled

  .method /*06000109*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    .param [3]/*0800012F*/ 
    .custom /*0C00014C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000130*/ 
    .custom /*0C00014D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e0
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Instrument::.ctor

  .method /*0600010A*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsObservable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x23e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_IsObservable

  .method /*0600010B*/ public hidebysig specialname 
          instance class System.Diagnostics.Metrics.Meter/*02000023*/ 
          get_Meter() cil managed
  // SIG: 20 00 12 80 8C
  {
    // Method begins at RVA 0x23ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Meter

  .method /*0600010C*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Name

  .method /*0600010D*/ family hidebysig instance void 
          Publish() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::Publish

  .method /*0600010E*/ public hidebysig specialname 
          instance string  get_Unit() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000137:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument::get_Unit

  .property /*17000049*/ instance callconv(8) string
          Description()
  {
    .custom /*0C0000BA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001F*/::get_Description() /* 06000107 */
  } // end of property Instrument::Description
  .property /*1700004A*/ instance callconv(8) bool
          Enabled()
  {
    .get instance bool System.Diagnostics.Metrics.Instrument/*0200001F*/::get_Enabled() /* 06000108 */
  } // end of property Instrument::Enabled
  .property /*1700004B*/ instance callconv(8) bool
          IsObservable()
  {
    .get instance bool System.Diagnostics.Metrics.Instrument/*0200001F*/::get_IsObservable() /* 0600010A */
  } // end of property Instrument::IsObservable
  .property /*1700004C*/ instance callconv(8) class System.Diagnostics.Metrics.Meter/*02000023*/
          Meter()
  {
    .get instance class System.Diagnostics.Metrics.Meter/*02000023*/ System.Diagnostics.Metrics.Instrument/*0200001F*/::get_Meter() /* 0600010B */
  } // end of property Instrument::Meter
  .property /*1700004D*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001F*/::get_Name() /* 0600010C */
  } // end of property Instrument::Name
  .property /*1700004E*/ instance callconv(8) string
          Unit()
  {
    .custom /*0C0000BE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Instrument/*0200001F*/::get_Unit() /* 0600010E */
  } // end of property Instrument::Unit
} // end of class System.Diagnostics.Metrics.Instrument

.class /*02000020*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.Instrument`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends System.Diagnostics.Metrics.Instrument/*0200001F*/
{
  .method /*0600010F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    .custom /*0C000139:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000133*/ 
    .custom /*0C000150:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000134*/ 
    .custom /*0C000151:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f8
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (06)000109       */ call       instance void System.Diagnostics.Metrics.Instrument/*0200001F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000023*/,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              string) /* 06000109 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method Instrument`1::.ctor

  .method /*06000110*/ family hidebysig instance void 
          RecordMeasurement(!T measurement) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2406
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*06000111*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag) cil managed
  // SIG: 20 02 01 13 00 15 11 59 02 0E 1C
  {
    .param [2]/*08000137*/ 
    .custom /*0C000152:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2409
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*06000112*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2) cil managed
  // SIG: 20 03 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*08000139*/ 
    .custom /*0C000153:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*0800013A*/ 
    .custom /*0C000154:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x240c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*06000113*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag1,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag2,
                            valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object> tag3) cil managed
  // SIG: 20 04 01 13 00 15 11 59 02 0E 1C 15 11 59 02 0E 1C 15 11 59 02 0E 1C
  {
    .param [2]/*0800013C*/ 
    .custom /*0C000155:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [3]/*0800013D*/ 
    .custom /*0C000156:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .param [4]/*0800013E*/ 
    .custom /*0C000157:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x240f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*06000114*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            [in] valuetype System.Diagnostics.TagList/*0200001B*/& tagList) cil managed
  // SIG: 20 02 01 13 00 10 11 6C
  {
    .param [2]/*08000140*/ 
    .custom /*0C000158:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2412
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

  .method /*06000115*/ family hidebysig instance void 
          RecordMeasurement(!T measurement,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 C5 01 15 11 59 02 0E 1C
  {
    .param [2]/*08000142*/ 
    .custom /*0C000159:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2415
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Instrument`1::RecordMeasurement

} // end of class System.Diagnostics.Metrics.Instrument`1

.class /*02000021*/ public sequential ansi sealed beforefieldinit System.Diagnostics.Metrics.Measurement`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000023*/
{
  .pack 0
  .size 1
  .custom /*0C00008E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*06000116*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x2418
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000117*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 12 7D 01 15 11 59 02 0E 1C
  {
    .param [2]/*08000145*/ 
    .custom /*0C00015D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x241b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000118*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>[] tags) cil managed
  // SIG: 20 02 01 13 00 1D 15 11 59 02 0E 1C
  {
    .param [2]/*08000147*/ 
    .custom /*0C00015F:0A00001D*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*0100003E*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    .custom /*0C000160:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 00 01 02 00 00 ) 
    // Method begins at RVA 0x241e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*06000119*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(!T 'value',
                               valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags) cil managed
  // SIG: 20 02 01 13 00 15 11 80 C5 01 15 11 59 02 0E 1C
  {
    .param [2]/*08000149*/ 
    .custom /*0C000161:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2421
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::.ctor

  .method /*0600011A*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> 
          get_Tags() cil managed
  // SIG: 20 00 15 11 80 C5 01 15 11 59 02 0E 1C
  {
    .param [0]/*0800014A*/ 
    .custom /*0C000162:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x2424
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::get_Tags

  .method /*0600011B*/ public hidebysig specialname 
          instance !T  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x2427
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Measurement`1::get_Value

  .property /*1700004F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>>
          Tags()
  {
    .custom /*0C0000C0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> System.Diagnostics.Metrics.Measurement`1/*02000021*/::get_Tags() /* 0600011A */
  } // end of property Measurement`1::Tags
  .property /*17000050*/ instance callconv(8) !T
          Value()
  {
    .get instance !T System.Diagnostics.Metrics.Measurement`1/*02000021*/::get_Value() /* 0600011B */
  } // end of property Measurement`1::Value
} // end of class System.Diagnostics.Metrics.Measurement`1

.class /*02000022*/ public auto ansi sealed System.Diagnostics.Metrics.MeasurementCallback`1<T>
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000036*/
{
  .param type T /*2A000008*/ 
    .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*0600011C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method MeasurementCallback`1::.ctor

  .method /*0600011D*/ public hidebysig newslot virtual 
          instance void  Invoke(class System.Diagnostics.Metrics.Instrument/*0200001F*/ instrument,
                                !T measurement,
                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                                object state) runtime managed
  // SIG: 20 04 01 12 7C 13 00 15 11 80 C5 01 15 11 59 02 0E 1C 1C
  {
    .custom /*0C00013E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800014F*/ 
    .custom /*0C000164:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000150*/ 
    .custom /*0C000165:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method MeasurementCallback`1::Invoke

  .method /*0600011E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000037*/ 
          BeginInvoke(class System.Diagnostics.Metrics.Instrument/*0200001F*/ instrument,
                      !T measurement,
                      valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000016*/<string,object>> tags,
                      object state,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000038*/ callback,
                      object 'object') runtime managed
  // SIG: 20 06 12 80 DD 12 7C 13 00 15 11 80 C5 01 15 11 59 02 0E 1C 1C 12 80 E1 1C
  {
    .param [1]/*08000151*/ 
    .custom /*0C000166:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000152*/ 
    .custom /*0C000168:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000153*/ 
    .custom /*0C000169:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
    .param [4]/*08000154*/ 
    .custom /*0C00016A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  } // end of method MeasurementCallback`1::BeginInvoke

  .method /*0600011F*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000037*/ result) runtime managed
  // SIG: 20 01 01 12 80 DD
  {
  } // end of method MeasurementCallback`1::EndInvoke

} // end of class System.Diagnostics.Metrics.MeasurementCallback`1

.class /*02000023*/ public auto ansi beforefieldinit System.Diagnostics.Metrics.Meter
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
  .custom /*0C000065:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000120*/ public hidebysig instance class System.Diagnostics.Metrics.Counter`1/*0200001C*/<!!T> 
          CreateCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                      [opt] string unit,
                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 03 15 12 70 01 1E 00 0E 0E 0E
  {
    .param type T /*2A00000E*/ 
      .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000158*/ 
    .custom /*0C00016B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000159*/ 
    .custom /*0C00016C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800015A*/  = nullref
    .param [3]/*0800015B*/  = nullref
    // Method begins at RVA 0x242a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateCounter

  .method /*06000121*/ public hidebysig instance class System.Diagnostics.Metrics.UpDownCounter`1/*0200001D*/<!!T> 
          CreateUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                            [opt] string unit,
                                                                                                            [opt] string description) cil managed
  // SIG: 30 01 03 15 12 74 01 1E 00 0E 0E 0E
  {
    .param type T /*2A00000F*/ 
      .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800015C*/ 
    .custom /*0C00016D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800015D*/ 
    .custom /*0C00016E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800015E*/  = nullref
    .param [3]/*0800015F*/  = nullref
    // Method begins at RVA 0x242d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateUpDownCounter

  .method /*06000122*/ public hidebysig instance class System.Diagnostics.Metrics.Histogram`1/*0200001E*/<!!T> 
          CreateHistogram<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                        [opt] string unit,
                                                                                                        [opt] string description) cil managed
  // SIG: 30 01 03 15 12 78 01 1E 00 0E 0E 0E
  {
    .param type T /*2A000010*/ 
      .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000160*/ 
    .custom /*0C00016F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000161*/ 
    .custom /*0C000170:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000162*/  = nullref
    .param [3]/*08000163*/  = nullref
    // Method begins at RVA 0x2430
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateHistogram

  .method /*06000123*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000025*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<!!T> observeValue,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 94 01 1E 00 0E 15 12 80 89 01 1E 00 0E 0E
  {
    .param type T /*2A000011*/ 
      .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000164*/ 
    .custom /*0C000171:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000165*/ 
    .custom /*0C000172:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000166*/ 
    .custom /*0C000173:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [3]/*08000167*/  = nullref
    .param [4]/*08000168*/  = nullref
    // Method begins at RVA 0x2433
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*06000124*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000025*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!!T>> observeValue,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 94 01 1E 00 0E 15 12 80 89 01 15 11 80 84 01 1E 00 0E 0E
  {
    .param type T /*2A000012*/ 
      .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000169*/ 
    .custom /*0C000174:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800016A*/ 
    .custom /*0C000175:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800016B*/ 
    .custom /*0C000176:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [3]/*0800016C*/  = nullref
    .param [4]/*0800016D*/  = nullref
    // Method begins at RVA 0x2436
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*06000125*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableCounter`1/*02000025*/<!!T> 
          CreateObservableCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                                class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!!T>>> observeValues,
                                                                                                                [opt] string unit,
                                                                                                                [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 94 01 1E 00 0E 15 12 80 89 01 15 12 7D 01 15 11 80 84 01 1E 00 0E 0E
  {
    .param type T /*2A000013*/ 
      .custom /*0C000060:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800016E*/ 
    .custom /*0C000177:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800016F*/ 
    .custom /*0C000178:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000170*/ 
    .custom /*0C000179:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [3]/*08000171*/  = nullref
    .param [4]/*08000172*/  = nullref
    // Method begins at RVA 0x2439
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableCounter

  .method /*06000126*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000026*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<!!T> observeValue,
                                                                                                                      [opt] string unit,
                                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 98 01 1E 00 0E 15 12 80 89 01 1E 00 0E 0E
  {
    .param type T /*2A000014*/ 
      .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000173*/ 
    .custom /*0C00017A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000174*/ 
    .custom /*0C00017B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000175*/ 
    .custom /*0C00017C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [3]/*08000176*/  = nullref
    .param [4]/*08000177*/  = nullref
    // Method begins at RVA 0x243c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*06000127*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000026*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!!T>> observeValue,
                                                                                                                      [opt] string unit,
                                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 98 01 1E 00 0E 15 12 80 89 01 15 11 80 84 01 1E 00 0E 0E
  {
    .param type T /*2A000015*/ 
      .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000178*/ 
    .custom /*0C00017D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000179*/ 
    .custom /*0C00017E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800017A*/ 
    .custom /*0C00017F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [3]/*0800017B*/  = nullref
    .param [4]/*0800017C*/  = nullref
    // Method begins at RVA 0x243f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*06000128*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableUpDownCounter`1/*02000026*/<!!T> 
          CreateObservableUpDownCounter<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                                      class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!!T>>> observeValues,
                                                                                                                      [opt] string unit,
                                                                                                                      [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 98 01 1E 00 0E 15 12 80 89 01 15 12 7D 01 15 11 80 84 01 1E 00 0E 0E
  {
    .param type T /*2A000016*/ 
      .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800017D*/ 
    .custom /*0C000180:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800017E*/ 
    .custom /*0C000181:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800017F*/ 
    .custom /*0C000182:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [3]/*08000180*/  = nullref
    .param [4]/*08000181*/  = nullref
    // Method begins at RVA 0x2442
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableUpDownCounter

  .method /*06000129*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000027*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<!!T> observeValue,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 9C 01 1E 00 0E 15 12 80 89 01 1E 00 0E 0E
  {
    .param type T /*2A000017*/ 
      .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000182*/ 
    .custom /*0C000183:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000183*/ 
    .custom /*0C000184:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000184*/ 
    .custom /*0C000185:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [3]/*08000185*/  = nullref
    .param [4]/*08000186*/  = nullref
    // Method begins at RVA 0x2445
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*0600012A*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000027*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!!T>> observeValue,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 9C 01 1E 00 0E 15 12 80 89 01 15 11 80 84 01 1E 00 0E 0E
  {
    .param type T /*2A000018*/ 
      .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000187*/ 
    .custom /*0C000186:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*08000188*/ 
    .custom /*0C000187:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000189*/ 
    .custom /*0C000188:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    .param [3]/*0800018A*/  = nullref
    .param [4]/*0800018B*/  = nullref
    // Method begins at RVA 0x2448
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*0600012B*/ public hidebysig instance class System.Diagnostics.Metrics.ObservableGauge`1/*02000027*/<!!T> 
          CreateObservableGauge<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(string name,
                                                                                                              class [System.Runtime/*23000001*/]System.Func`1/*01000022*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!!T>>> observeValues,
                                                                                                              [opt] string unit,
                                                                                                              [opt] string description) cil managed
  // SIG: 30 01 04 15 12 80 9C 01 1E 00 0E 15 12 80 89 01 15 12 7D 01 15 11 80 84 01 1E 00 0E 0E
  {
    .param type T /*2A000019*/ 
      .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800018C*/ 
    .custom /*0C000189:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 00 00 00 ) 
    .param [1]/*0800018D*/ 
    .custom /*0C00018A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800018E*/ 
    .custom /*0C00018B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 00 00 00 00 ) 
    .param [3]/*0800018F*/  = nullref
    .param [4]/*08000190*/  = nullref
    // Method begins at RVA 0x244b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::CreateObservableGauge

  .method /*0600012C*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x244e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::Dispose

  .method /*0600012D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000149:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2451
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Meter::.ctor

  .method /*0600012E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name,
                               string version) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C00014A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000193*/ 
    .custom /*0C00018C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x245a
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method Meter::.ctor

  .method /*0600012F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00014B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2463
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::get_Name

  .method /*06000130*/ public hidebysig specialname 
          instance string  get_Version() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2466
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Meter::get_Version

  .property /*17000051*/ instance callconv(8) string
          Name()
  {
    .custom /*0C0000C3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Diagnostics.Metrics.Meter/*02000023*/::get_Name() /* 0600012F */
  } // end of property Meter::Name
  .property /*17000052*/ instance callconv(8) string
          Version()
  {
    .get instance string System.Diagnostics.Metrics.Meter/*02000023*/::get_Version() /* 06000130 */
  } // end of property Meter::Version
} // end of class System.Diagnostics.Metrics.Meter

.class /*02000024*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.MeterListener
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000014*/
{
  .method /*06000131*/ public hidebysig instance object 
          DisableMeasurementEvents(class System.Diagnostics.Metrics.Instrument/*0200001F*/ instrument) cil managed
  // SIG: 20 01 1C 12 7C
  {
    .custom /*0C00014E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000194*/ 
    .custom /*0C00018D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2469
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::DisableMeasurementEvents

  .method /*06000132*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x246c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::Dispose

  .method /*06000133*/ public hidebysig instance void 
          EnableMeasurementEvents(class System.Diagnostics.Metrics.Instrument/*0200001F*/ instrument,
                                  [opt] object state) cil managed
  // SIG: 20 02 01 12 7C 1C
  {
    .custom /*0C00014F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000197*/  = nullref
    .custom /*0C00018E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x246f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::EnableMeasurementEvents

  .method /*06000134*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/> 
          get_InstrumentPublished() cil managed
  // SIG: 20 00 15 12 75 02 12 7C 12 80 90
  {
    .param [0]/*08000198*/ 
    .custom /*0C00018F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::get_InstrumentPublished

  .method /*06000135*/ public hidebysig specialname 
          instance void  set_InstrumentPublished(class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/> 'value') cil managed
  // SIG: 20 01 01 15 12 75 02 12 7C 12 80 90
  {
    .param [1]/*08000199*/ 
    .custom /*0C000190:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x2475
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::set_InstrumentPublished

  .method /*06000136*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,object> 
          get_MeasurementsCompleted() cil managed
  // SIG: 20 00 15 12 75 02 12 7C 1C
  {
    .param [0]/*0800019A*/ 
    .custom /*0C000191:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x2478
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::get_MeasurementsCompleted

  .method /*06000137*/ public hidebysig specialname 
          instance void  set_MeasurementsCompleted(class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,object> 'value') cil managed
  // SIG: 20 01 01 15 12 75 02 12 7C 1C
  {
    .param [1]/*0800019B*/ 
    .custom /*0C000192:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x247b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::set_MeasurementsCompleted

  .method /*06000138*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x247e
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000017*/::.ctor() /* 0A00001F */
    IL_0006:  /* 14   |                  */ ldnull
    IL_0007:  /* 7A   |                  */ throw
  } // end of method MeterListener::.ctor

  .method /*06000139*/ public hidebysig instance void 
          RecordObservableInstruments() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2487
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::RecordObservableInstruments

  .method /*0600013A*/ public hidebysig instance void 
          SetMeasurementEventCallback<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>(class System.Diagnostics.Metrics.MeasurementCallback`1/*02000022*/<!!T> measurementCallback) cil managed
  // SIG: 30 01 01 01 15 12 80 88 01 1E 00
  {
    .param [1]/*0800019C*/ 
    .custom /*0C000193:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 00 00 00 ) 
    // Method begins at RVA 0x248a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::SetMeasurementEventCallback

  .method /*0600013B*/ public hidebysig instance void 
          Start() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x248d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MeterListener::Start

  .property /*17000053*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/>
          InstrumentPublished()
  {
    .custom /*0C0000C5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/> System.Diagnostics.Metrics.MeterListener/*02000024*/::get_InstrumentPublished() /* 06000134 */
    .set instance void System.Diagnostics.Metrics.MeterListener/*02000024*/::set_InstrumentPublished(class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,class System.Diagnostics.Metrics.MeterListener/*02000024*/>) /* 06000135 */
  } // end of property MeterListener::InstrumentPublished
  .property /*17000054*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,object>
          MeasurementsCompleted()
  {
    .custom /*0C0000C6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,object> System.Diagnostics.Metrics.MeterListener/*02000024*/::get_MeasurementsCompleted() /* 06000136 */
    .set instance void System.Diagnostics.Metrics.MeterListener/*02000024*/::set_MeasurementsCompleted(class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<class System.Diagnostics.Metrics.Instrument/*0200001F*/,object>) /* 06000137 */
  } // end of property MeterListener::MeasurementsCompleted
} // end of class System.Diagnostics.Metrics.MeterListener

.class /*02000025*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.ObservableCounter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends class System.Diagnostics.Metrics.ObservableInstrument`1/*02000028*/<!T>/*1B00000F*/
{
  .method /*0600013C*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    // Method begins at RVA 0x2490
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000021       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000028*/<!T>/*1B00000F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000023*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string) /* 0A000021 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableCounter`1::.ctor

  .method /*0600013D*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 80 84 01 13 00
  {
    .param [0]/*080001A1*/ 
    .custom /*0C000194:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x249e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableCounter`1::Observe

} // end of class System.Diagnostics.Metrics.ObservableCounter`1

.class /*02000026*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.ObservableUpDownCounter`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends class System.Diagnostics.Metrics.ObservableInstrument`1/*02000028*/<!T>/*1B00000F*/
{
  .method /*0600013E*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    // Method begins at RVA 0x24a1
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000021       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000028*/<!T>/*1B00000F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000023*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string) /* 0A000021 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableUpDownCounter`1::.ctor

  .method /*0600013F*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 80 84 01 13 00
  {
    .param [0]/*080001A6*/ 
    .custom /*0C000195:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x24af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableUpDownCounter`1::Observe

} // end of class System.Diagnostics.Metrics.ObservableUpDownCounter`1

.class /*02000027*/ public auto ansi sealed beforefieldinit System.Diagnostics.Metrics.ObservableGauge`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends class System.Diagnostics.Metrics.ObservableInstrument`1/*02000028*/<!T>/*1B00000F*/
{
  .method /*06000140*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    // Method begins at RVA 0x24b2
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (0A)000021       */ call       instance void class System.Diagnostics.Metrics.ObservableInstrument`1/*02000028*/<!T>/*1B00000F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000023*/,
                                                                                                                                                                string,
                                                                                                                                                                string,
                                                                                                                                                                string) /* 0A000021 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableGauge`1::.ctor

  .method /*06000141*/ family hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 80 84 01 13 00
  {
    .param [0]/*080001AB*/ 
    .custom /*0C000196:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x24c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableGauge`1::Observe

} // end of class System.Diagnostics.Metrics.ObservableGauge`1

.class /*02000028*/ public abstract auto ansi beforefieldinit System.Diagnostics.Metrics.ObservableInstrument`1<valuetype .ctor ([System.Runtime/*23000001*/]System.ValueType/*01000023*/) T>
       extends System.Diagnostics.Metrics.Instrument/*0200001F*/
{
  .method /*06000142*/ public hidebysig specialname virtual 
          instance bool  get_IsObservable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObservableInstrument`1::get_IsObservable

  .method /*06000143*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Diagnostics.Metrics.Meter/*02000023*/ meter,
                               string name,
                               string unit,
                               string description) cil managed
  // SIG: 20 04 01 12 80 8C 0E 0E 0E
  {
    .custom /*0C00015A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080001AE*/ 
    .custom /*0C000197:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080001AF*/ 
    .custom /*0C000198:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x24c6
    // Code size       13 (0xd)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 03   |                  */ ldarg.1
    IL_0002:  /* 04   |                  */ ldarg.2
    IL_0003:  /* 05   |                  */ ldarg.3
    IL_0004:  /* 0E   | 04               */ ldarg.s    description
    IL_0006:  /* 28   | (06)000109       */ call       instance void System.Diagnostics.Metrics.Instrument/*0200001F*/::.ctor(class System.Diagnostics.Metrics.Meter/*02000023*/,
                                                                                                                              string,
                                                                                                                              string,
                                                                                                                              string) /* 06000109 */
    IL_000b:  /* 14   |                  */ ldnull
    IL_000c:  /* 7A   |                  */ throw
  } // end of method ObservableInstrument`1::.ctor

  .method /*06000144*/ family hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100001F*/<valuetype System.Diagnostics.Metrics.Measurement`1/*02000021*/<!T>> 
          Observe() cil managed
  // SIG: 20 00 15 12 7D 01 15 11 80 84 01 13 00
  {
    .param [0]/*080001B0*/ 
    .custom /*0C000199:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ObservableInstrument`1::Observe

  .property /*17000055*/ instance callconv(8) bool
          IsObservable()
  {
    .get instance bool System.Diagnostics.Metrics.ObservableInstrument`1/*02000028*/::get_IsObservable() /* 06000142 */
  } // end of property ObservableInstrument`1::IsObservable
} // end of class System.Diagnostics.Metrics.ObservableInstrument`1


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
