


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
// Number of sections:         0x0004
// Time-date stamp:            0xed86036a
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00004400
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000083e6
// Base of code:                   0x00004000
// Base of data:                   0x00002000
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
// Size of headers:                0x00000400
// Checksum:                       0x00009ef6
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00008394 [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x00000570] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00005200 [0x00002468] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00008378 [0x0000001c] address [size] of Debug Directory:           
// 0x00000000 [0x00000000] address [size] of Architecture Specific:     
// 0x00000000 [0x00000000] address [size] of Global Pointer:            
// 0x00000000 [0x00000000] address [size] of TLS Directory:             
// 0x00000000 [0x00000000] address [size] of Load Config Directory:     
// 0x00000000 [0x00000000] address [size] of Bound Import Directory:    
// 0x00004000 [0x00000008] address [size] of Import Address Table:      
// 0x00000000 [0x00000000] address [size] of Delay Load IAT:            
// 0x00004008 [0x00000048] address [size] of CLR Header:                


// Image sections:
//              .mvid
//              0x00000010 Virtual Size
//              0x00002000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00000400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

//              .text
//              0x000043ec Virtual Size
//              0x00004000 Virtual Address
//              0x00004400 Size of Raw Data
//              0x00000600 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x00000570 Virtual Size
//              0x0000a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00004a00 Pointer to Raw Data
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
//              0x00005000 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000003e8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x000083d6 Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 40 00 10 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x00004054 [0x000042a4] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000082f8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001ee0 Size
//              '#~' Name
//    Stream 2:
//              0x00001f4c Offset
//              0x00001a9c Size
//              '#Strings' Name
//    Stream 3:
//              0x000039e8 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000039ec Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000039fc Offset
//              0x000008a8 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 30312
// PE header size       : 1024 (536 used)    ( 3.38%)
// PE additional info   : 10911             (36.00%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.24%)
// CLR meta-data size  : 17060              (56.28%)
// CLR additional info : 128                ( 0.42%)
// CLR method headers  : 104                ( 0.34%)
// Managed code         : 208               ( 0.69%)
// Data                 : 2560              ( 8.45%)
// Unaccounted          : -1755             (-5.79%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 17408
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 17060
//   Module        -    1 (10 bytes)
//   TypeDef       -   64 (896 bytes)     27 interfaces, 0 explicit layout
//   TypeRef       -   67 (402 bytes)
//   MethodDef     -  185 (2590 bytes)    69 abstract, 0 native, 104 bodies
//   FieldDef      -   28 (168 bytes)     4 constant
//   MemberRef     -   38 (228 bytes)
//   ParamDef      -  149 (894 bytes)
//   MethodImpl    -   17 (102 bytes)
//   Constant      -   19 (114 bytes)
//   CustomAttribute-  184 (1104 bytes)
//   InterfaceImpl -   17 (68 bytes)
//   PropertyMap   -   28 (112 bytes)
//   Property      -   56 (336 bytes)
//   MethodSemantic-   81 (486 bytes)
//   TypeSpec      -    3 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    9 (180 bytes)
//   NestedClass   -   10 (40 bytes)
//   GenericParam  -    4 (32 bytes)
//   GenericParamConstraint-    2 (8 bytes)
//   Strings       -  6811 bytes
//   Blobs         -  2216 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   215 bytes

// CLR additional info : 128

// CLR method headers : 104
//   Num.of method bodies  - 104
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 104

// Managed code : 208
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class ActivatorUtilities             (abstract) (auto) (ansi) (sealed) 
// Class ActivatorUtilitiesConstructorAttribute (auto) (ansi) (sealed) 
// Class ObjectFactory                  (auto) (ansi) (sealed) 
// Class ParameterDefaultValue          (auto) (ansi) 
// Class BaseConnectionContext          (public) (abstract) (auto) (ansi) 
// Class ConnectionBuilder              (public) (auto) (ansi) 
// Class ConnectionBuilderExtensions    (public) (abstract) (auto) (ansi) (sealed) 
// Class ConnectionContext              (public) (abstract) (auto) (ansi) 
// Class ConnectionDelegate             (public) (auto) (ansi) (sealed) 
// Class ConnectionHandler              (public) (abstract) (auto) (ansi) 
// Class ConnectionItems                (public) (auto) (ansi) 
// Class DefaultConnectionContext       (public) (auto) (ansi) 
// Class AddressInUseException          (public) (auto) (ansi) 
// Class ConnectionAbortedException     (public) (auto) (ansi) 
// Class ConnectionResetException       (public) (auto) (ansi) 
// Class FileHandleEndPoint             (public) (auto) (ansi) 
// Class FileHandleType                 (public) (auto) (ansi) (sealed) 
// Interface IConnectionBuilder             (public) (abstract) (auto) (ansi) 
// Interface IConnectionFactory             (public) (abstract) (auto) (ansi) 
// Interface IConnectionListener            (public) (abstract) (auto) (ansi) 
// Interface IConnectionListenerFactory     (public) (abstract) (auto) (ansi) 
// Interface IMultiplexedConnectionListener (public) (abstract) (auto) (ansi) 
// Interface IMultiplexedConnectionBuilder  (public) (abstract) (auto) (ansi) 
// Interface IMultiplexedConnectionFactory  (public) (abstract) (auto) (ansi) 
// Interface IMultiplexedConnectionListenerFactory (public) (abstract) (auto) (ansi) 
// Class MultiplexedConnectionBuilder   (public) (auto) (ansi) 
// Class MultiplexedConnectionContext   (public) (abstract) (auto) (ansi) 
// Class MultiplexedConnectionDelegate  (public) (auto) (ansi) (sealed) 
// Class TransferFormat                 (public) (auto) (ansi) (sealed) 
// Class UriEndPoint                    (public) (auto) (ansi) 
// Interface IConnectionCompleteFeature     (public) (abstract) (auto) (ansi) 
// Interface IConnectionEndPointFeature     (public) (abstract) (auto) (ansi) 
// Interface IConnectionHeartbeatFeature    (public) (abstract) (auto) (ansi) 
// Interface IConnectionIdFeature           (public) (abstract) (auto) (ansi) 
// Interface IConnectionInherentKeepAliveFeature (public) (abstract) (auto) (ansi) 
// Interface IConnectionItemsFeature        (public) (abstract) (auto) (ansi) 
// Interface IConnectionLifetimeFeature     (public) (abstract) (auto) (ansi) 
// Interface IConnectionLifetimeNotificationFeature (public) (abstract) (auto) (ansi) 
// Interface IConnectionSocketFeature       (public) (abstract) (auto) (ansi) 
// Interface IConnectionTransportFeature    (public) (abstract) (auto) (ansi) 
// Interface IConnectionUserFeature         (public) (abstract) (auto) (ansi) 
// Interface IMemoryPoolFeature             (public) (abstract) (auto) (ansi) 
// Interface IPersistentStateFeature        (public) (abstract) (auto) (ansi) 
// Interface IProtocolErrorCodeFeature      (public) (abstract) (auto) (ansi) 
// Interface IStreamAbortFeature            (public) (abstract) (auto) (ansi) 
// Interface IStreamDirectionFeature        (public) (abstract) (auto) (ansi) 
// Interface IStreamIdFeature               (public) (abstract) (auto) (ansi) 
// Interface ITlsHandshakeFeature           (public) (abstract) (auto) (ansi) 
// Interface ITransferFormatFeature         (public) (abstract) (auto) (ansi) 
// Class ConstructorMatcher             (sequential) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass0_0`1         (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_1           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_2           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass2_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.Extensions.Features
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ System.IO.Pipelines
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ System.Security.Claims
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ System.Threading.ThreadPool
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ System.Net.Sockets
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ System.Memory
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Connections.Abstractions
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

  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 3A 43 6F 72 65 20 63 6F 6D 70 6F 6E 65 6E   // ..:Core componen
                                                                                                                                                                       74 73 20 6F 66 20 41 53 50 2E 4E 45 54 20 43 6F   // ts of ASP.NET Co
                                                                                                                                                                       72 65 20 6E 65 74 77 6F 72 6B 69 6E 67 20 70 72   // re networking pr
                                                                                                                                                                       6F 74 6F 63 6F 6C 20 73 74 61 63 6B 2E 00 00 )    // otocol stack...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 2D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..-Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 43 6F 6E 6E 65 63 74 69   // NetCore.Connecti
                                                                                                                                                                 6F 6E 73 2E 41 62 73 74 72 61 63 74 69 6F 6E 73   // ons.Abstractions
                                                                                                                                                                 00 00 ) 
  .custom /*0C000014:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000008*/::.ctor(string,
                                                                                                                                         string) /* 0A000007 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                F3 3A 29 04 4F A9 D7 40 C9 B3 21 3A 93 E5 7C 84   // .:).O..@..!:..|.
                B4 72 C8 4E 0B 8A 0E 1A E4 8E 67 A9 F8 F6 DE 9D   // .r.N......g.....
                5F 7F 3D 52 AC 23 E4 8A C5 18 01 F1 DC 95 0A BE   // _.=R.#..........
                90 1D A3 4D 2A 9E 3B AA DB 14 1A 17 C7 7E F3 C5   // ...M*.;......~..
                65 DD 5E E5 05 4B 91 CF 63 BB 3C 6A B8 3F 72 AB   // e.^..K..c.<j.?r.
                3A AF E9 3D 0F C3 C2 34 8B 76 4F AF B0 B1 C0 73   // :..=...4.vO....s
                3D E5 14 59 AE AB 46 58 03 84 BF 9D 74 C4 E2 81   // =..Y..FX....t...
                64 B7 CD E2 47 F8 91 BA 07 89 1C 9D 87 2A D2 BB ) // d...G........*..
  .hash algorithm 0x00008004
  .ver 6:0:0:0
}
.module Microsoft.AspNetCore.Connections.Abstractions.dll
// MVID: {41bb4282-f5e7-4099-b465-732a5e8a406f}
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
  .custom /*0C000016:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000019:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000001*/ public initonly uint8[] NullableFlags
  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullableAttribute::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] A_1) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
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
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000025:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000027:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000003*/ public initonly bool IncludesInternals
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool A_1) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.ActivatorUtilities
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .class /*02000037*/ sequential ansi sealed nested private beforefieldinit ConstructorMatcher
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
  {
    .field /*0400000B*/ private initonly class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100001B*/ _constructor
    .field /*0400000C*/ private initonly class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001C*/[] _parameters
    .field /*0400000D*/ private initonly object[] _parameterValues
    .method /*060000AE*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.ConstructorInfo/*0100001B*/ constructor) cil managed
    // SIG: 20 01 01 12 6D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConstructorMatcher::.ctor

    .method /*060000AF*/ public hidebysig 
            instance int32  Match(object[] givenParameters) cil managed
    // SIG: 20 01 08 1D 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConstructorMatcher::Match

    .method /*060000B0*/ public hidebysig 
            instance object  CreateInstance(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ provider) cil managed
    // SIG: 20 01 1C 12 5D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ConstructorMatcher::CreateInstance

  } // end of class ConstructorMatcher

  .class /*02000038*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C00007F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400000E*/ public static initonly class Microsoft.Extensions.Internal.ActivatorUtilities/*02000006*//'<>c'/*02000038*/ '<>9'
    .method /*060000B1*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

  } // end of class '<>c'

  .method /*06000006*/ public hidebysig static 
          object  CreateInstance(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ provider,
                                 class [System.Runtime/*23000001*/]System.Type/*01000018*/ instanceType,
                                 object[] parameters) cil managed
  // SIG: 00 03 1C 12 5D 12 61 1D 1C
  {
    .param [3]/*08000003*/ 
    .custom /*0C00001C:0A000013*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000019*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivatorUtilities::CreateInstance

  .method /*06000007*/ public hidebysig static 
          class Microsoft.Extensions.Internal.ObjectFactory/*02000008*/ 
          CreateFactory(class [System.Runtime/*23000001*/]System.Type/*01000018*/ instanceType,
                        class [System.Runtime/*23000001*/]System.Type/*01000018*/[] argumentTypes) cil managed
  // SIG: 00 02 12 20 12 61 1D 12 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivatorUtilities::CreateFactory

  .method /*06000008*/ public hidebysig static 
          !!T  CreateInstance<T>(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ provider,
                                 object[] parameters) cil managed
  // SIG: 10 01 02 1E 00 12 5D 1D 1C
  {
    .param [2]/*08000007*/ 
    .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000019*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivatorUtilities::CreateInstance

  .method /*06000009*/ public hidebysig static 
          !!T  GetServiceOrCreateInstance<T>(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ provider) cil managed
  // SIG: 10 01 01 1E 00 12 5D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivatorUtilities::GetServiceOrCreateInstance

  .method /*0600000A*/ public hidebysig static 
          object  GetServiceOrCreateInstance(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ provider,
                                             class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'type') cil managed
  // SIG: 00 02 1C 12 5D 12 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivatorUtilities::GetServiceOrCreateInstance

} // end of class Microsoft.Extensions.Internal.ActivatorUtilities

.class /*02000007*/ private auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.ActivatorUtilitiesConstructorAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00002C:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 FF 7F 00 00 00 00 ) 
  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ActivatorUtilitiesConstructorAttribute::.ctor

} // end of class Microsoft.Extensions.Internal.ActivatorUtilitiesConstructorAttribute

.class /*02000008*/ private auto ansi sealed Microsoft.Extensions.Internal.ObjectFactory
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001D*/
{
  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ObjectFactory::.ctor

  .method /*0600000D*/ public hidebysig newslot virtual 
          instance object  Invoke(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ serviceProvider,
                                  object[] arguments) runtime managed
  // SIG: 20 02 1C 12 5D 1D 1C
  {
  } // end of method ObjectFactory::Invoke

  .method /*0600000E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginInvoke(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ serviceProvider,
                      object[] arguments,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 79 12 5D 1D 1C 12 7D 1C
  {
  } // end of method ObjectFactory::BeginInvoke

  .method /*0600000F*/ public hidebysig newslot virtual 
          instance object  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001E*/ result) runtime managed
  // SIG: 20 01 1C 12 79
  {
  } // end of method ObjectFactory::EndInvoke

} // end of class Microsoft.Extensions.Internal.ObjectFactory

.class /*02000009*/ private auto ansi beforefieldinit Microsoft.Extensions.Internal.ParameterDefaultValue
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .method /*06000010*/ public hidebysig static 
          bool  TryGetDefaultValue(class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001C*/ parameter,
                                   [out] object& defaultValue) cil managed
  // SIG: 00 02 02 12 71 10 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDefaultValue::TryGetDefaultValue

  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ParameterDefaultValue::.ctor

} // end of class Microsoft.Extensions.Internal.ParameterDefaultValue

.class /*0200000A*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Connections.BaseConnectionContext
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
{
  .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000012*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method BaseConnectionContext::get_ConnectionId

  .method /*06000013*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_ConnectionId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method BaseConnectionContext::set_ConnectionId

  .method /*06000014*/ public hidebysig newslot specialname abstract virtual 
          instance class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/ 
          get_Features() cil managed
  // SIG: 20 00 12 80 85
  {
    // Method begins at RVA 0x0
  } // end of method BaseConnectionContext::get_Features

  .method /*06000015*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 80 89 02 1C 1C
  {
    .param [0]/*08000017*/ 
    .custom /*0C000052:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method BaseConnectionContext::get_Items

  .method /*06000016*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 89 02 1C 1C
  {
    .param [1]/*08000018*/ 
    .custom /*0C000055:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method BaseConnectionContext::set_Items

  .method /*06000017*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ 
          get_ConnectionClosed() cil managed
  // SIG: 20 00 11 80 8D
  {
    .custom /*0C000050:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseConnectionContext::get_ConnectionClosed

  .method /*06000018*/ public hidebysig newslot specialname virtual 
          instance void  set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 11 80 8D
  {
    .custom /*0C000053:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseConnectionContext::set_ConnectionClosed

  .method /*06000019*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 
          get_LocalEndPoint() cil managed
  // SIG: 20 00 12 80 91
  {
    .custom /*0C000056:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000057:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseConnectionContext::get_LocalEndPoint

  .method /*0600001A*/ public hidebysig newslot specialname virtual 
          instance void  set_LocalEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    .custom /*0C000059:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00005A:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseConnectionContext::set_LocalEndPoint

  .method /*0600001B*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 
          get_RemoteEndPoint() cil managed
  // SIG: 20 00 12 80 91
  {
    .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00005D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseConnectionContext::get_RemoteEndPoint

  .method /*0600001C*/ public hidebysig newslot specialname virtual 
          instance void  set_RemoteEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    .custom /*0C00005F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000060:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseConnectionContext::set_RemoteEndPoint

  .method /*0600001D*/ public hidebysig newslot abstract virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method BaseConnectionContext::Abort

  .method /*0600001E*/ public hidebysig newslot abstract virtual 
          instance void  Abort(class Microsoft.AspNetCore.Connections.ConnectionAbortedException/*02000013*/ abortReason) cil managed
  // SIG: 20 01 01 12 4C
  {
    // Method begins at RVA 0x0
  } // end of method BaseConnectionContext::Abort

  .method /*0600001F*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseConnectionContext::DisposeAsync

  .method /*06000020*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BaseConnectionContext::.ctor

  .property /*17000001*/ instance callconv(8) string
          ConnectionId()
  {
    .get instance string Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::get_ConnectionId() /* 06000012 */
    .set instance void Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::set_ConnectionId(string) /* 06000013 */
  } // end of property BaseConnectionContext::ConnectionId
  .property /*17000002*/ instance callconv(8) class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/
          Features()
  {
    .get instance class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/ Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::get_Features() /* 06000014 */
  } // end of property BaseConnectionContext::Features
  .property /*17000003*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>
          Items()
  {
    .custom /*0C00001E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::get_Items() /* 06000015 */
    .set instance void Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>) /* 06000016 */
  } // end of property BaseConnectionContext::Items
  .property /*17000004*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/
          ConnectionClosed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::get_ConnectionClosed() /* 06000017 */
    .set instance void Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/) /* 06000018 */
  } // end of property BaseConnectionContext::ConnectionClosed
  .property /*17000005*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
          LocalEndPoint()
  {
    .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::get_LocalEndPoint() /* 06000019 */
    .set instance void Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::set_LocalEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/) /* 0600001A */
  } // end of property BaseConnectionContext::LocalEndPoint
  .property /*17000006*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
          RemoteEndPoint()
  {
    .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::get_RemoteEndPoint() /* 0600001B */
    .set instance void Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/::set_RemoteEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/) /* 0600001C */
  } // end of property BaseConnectionContext::RemoteEndPoint
} // end of class Microsoft.AspNetCore.Connections.BaseConnectionContext

.class /*0200000B*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.ConnectionBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/
{
  .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/
  .custom /*0C000018:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000039*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000080:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400000F*/ public static initonly class Microsoft.AspNetCore.Connections.ConnectionBuilder/*0200000B*//'<>c'/*02000039*/ '<>9'
    .field /*04000010*/ public static class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/ '<>9__6_0'
    .method /*060000B2*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

  } // end of class '<>c'

  .method /*06000021*/ public hidebysig newslot specialname virtual final 
          instance class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ 
          get_ApplicationServices() cil managed
  // SIG: 20 00 12 5D
  {
    .custom /*0C000066:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionBuilder::get_ApplicationServices

  .method /*06000022*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ applicationServices) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionBuilder::.ctor

  .method /*06000023*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/ 
          Use(class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/,class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/> middleware) cil managed
  // SIG: 20 01 12 5C 15 12 80 99 02 12 38 12 38
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionBuilder::Use

  .method /*06000024*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/ 
          Build() cil managed
  // SIG: 20 00 12 38
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionBuilder::Build

  .property /*17000007*/ instance callconv(8) class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/
          ApplicationServices()
  {
    .get instance class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ Microsoft.AspNetCore.Connections.ConnectionBuilder/*0200000B*/::get_ApplicationServices() /* 06000021 */
  } // end of property ConnectionBuilder::ApplicationServices
} // end of class Microsoft.AspNetCore.Connections.ConnectionBuilder

.class /*0200000C*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Connections.ConnectionBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200003A*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass0_0`1'<(Microsoft.AspNetCore.Connections.ConnectionHandler/*0200000F*/) TConnectionHandler>
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000081:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000011*/ public !TConnectionHandler 'handler'
    .method /*060000B3*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass0_0`1'::.ctor

  } // end of class '<>c__DisplayClass0_0`1'

  .class /*0200003B*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000082:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000012*/ public class [System.Runtime/*23000001*/]System.Func`3/*01000029*/<class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Func`1/*0100002A*/<class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/>,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/> middleware
    .method /*060000B4*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::.ctor

  } // end of class '<>c__DisplayClass1_0'

  .class /*0200003C*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_1'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000083:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000013*/ public class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/ next
    .field /*04000014*/ public class Microsoft.AspNetCore.Connections.ConnectionBuilderExtensions/*0200000C*//'<>c__DisplayClass1_0'/*0200003B*/ 'CS$<>8__locals1'
    .method /*060000B5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_1'::.ctor

  } // end of class '<>c__DisplayClass1_1'

  .class /*0200003D*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_2'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000084:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000015*/ public class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/ context
    .field /*04000016*/ public class Microsoft.AspNetCore.Connections.ConnectionBuilderExtensions/*0200000C*//'<>c__DisplayClass1_1'/*0200003C*/ 'CS$<>8__locals2'
    .method /*060000B6*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_2'::.ctor

  } // end of class '<>c__DisplayClass1_2'

  .class /*0200003E*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass2_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000085:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000017*/ public class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/> middleware
    .field /*04000018*/ public class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/ '<>9__1'
    .method /*060000B7*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass2_0'::.ctor

  } // end of class '<>c__DisplayClass2_0'

  .method /*06000025*/ public hidebysig static 
          class Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/ 
          UseConnectionHandler<(Microsoft.AspNetCore.Connections.ConnectionHandler/*0200000F*/) TConnectionHandler>(class Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/ connectionBuilder) cil managed
  // SIG: 10 01 01 12 5C 12 5C
  {
    .custom /*0C00006C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TConnectionHandler /*2A000003*/ 
      .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
      .custom /*0C000020:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*01000028*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*01000027*/) /* 0A000014 */ = ( 01 00 03 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionBuilderExtensions::UseConnectionHandler

  .method /*06000026*/ public hidebysig static 
          class Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/ 
          Use(class Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/ connectionBuilder,
              class [System.Runtime/*23000001*/]System.Func`3/*01000029*/<class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Func`1/*0100002A*/<class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/>,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/> middleware) cil managed
  // SIG: 00 02 12 5C 12 5C 15 12 80 A5 03 12 34 15 12 80 A9 01 12 80 AD 12 80 AD
  {
    .custom /*0C00006E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionBuilderExtensions::Use

  .method /*06000027*/ public hidebysig static 
          class Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/ 
          Run(class Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/ connectionBuilder,
              class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/> middleware) cil managed
  // SIG: 00 02 12 5C 12 5C 15 12 80 99 02 12 34 12 80 AD
  {
    .custom /*0C000070:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionBuilderExtensions::Run

} // end of class Microsoft.AspNetCore.Connections.ConnectionBuilderExtensions

.class /*0200000D*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Connections.ConnectionContext
       extends Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
{
  .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
  .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000028*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ 
          get_Transport() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x0
  } // end of method ConnectionContext::get_Transport

  .method /*06000029*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Transport(class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x0
  } // end of method ConnectionContext::set_Transport

  .method /*0600002A*/ public hidebysig virtual 
          instance void  Abort(class Microsoft.AspNetCore.Connections.ConnectionAbortedException/*02000013*/ abortReason) cil managed
  // SIG: 20 01 01 12 4C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionContext::Abort

  .method /*0600002B*/ public hidebysig virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionContext::Abort

  .method /*0600002C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionContext::.ctor

  .property /*17000008*/ instance callconv(8) class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/
          Transport()
  {
    .get instance class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/::get_Transport() /* 06000028 */
    .set instance void Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/::set_Transport(class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/) /* 06000029 */
  } // end of property ConnectionContext::Transport
} // end of class Microsoft.AspNetCore.Connections.ConnectionContext

.class /*0200000E*/ public auto ansi sealed Microsoft.AspNetCore.Connections.ConnectionDelegate
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001D*/
{
  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ConnectionDelegate::.ctor

  .method /*0600002E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/ 
          Invoke(class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/ connection) runtime managed
  // SIG: 20 01 12 80 AD 12 34
  {
    .custom /*0C000077:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  } // end of method ConnectionDelegate::Invoke

  .method /*0600002F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginInvoke(class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/ connection,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 79 12 34 12 7D 1C
  {
    .param [1]/*08000029*/ 
    .custom /*0C000072:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method ConnectionDelegate::BeginInvoke

  .method /*06000030*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/ 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001E*/ result) runtime managed
  // SIG: 20 01 12 80 AD 12 79
  {
    .param [0]/*0800002C*/ 
    .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method ConnectionDelegate::EndInvoke

} // end of class Microsoft.AspNetCore.Connections.ConnectionDelegate

.class /*0200000F*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Connections.ConnectionHandler
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .method /*06000031*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/ 
          OnConnectedAsync(class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/ connection) cil managed
  // SIG: 20 01 12 80 AD 12 34
  {
    .custom /*0C00007C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ConnectionHandler::OnConnectedAsync

  .method /*06000032*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionHandler::.ctor

} // end of class Microsoft.AspNetCore.Connections.ConnectionHandler

.class /*02000010*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.ConnectionItems
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000003*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000031*/
{
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/
  .custom /*0C000024:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/
  .custom /*0C000028:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000003*/
  .custom /*0C00002A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .method /*06000033*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::.ctor

  .method /*06000034*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> items) cil managed
  // SIG: 20 01 01 15 12 80 89 02 1C 1C
  {
    .param [1]/*0800002F*/ 
    .custom /*0C00007A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::.ctor

  .method /*06000035*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 80 89 02 1C 1C
  {
    .custom /*0C00007E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .param [0]/*08000030*/ 
    .custom /*0C00007B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::get_Items

  .method /*06000036*/ private hidebysig newslot specialname virtual final 
          instance object  'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Item'(object key) cil managed
  // SIG: 20 01 1C 1C
  {
    .override  method instance !1 class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/::get_Item(!0) /* 0A000015 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Item'

  .method /*06000037*/ private hidebysig newslot specialname virtual final 
          instance void  'System.Collections.Generic.IDictionary<System.Object,System.Object>.set_Item'(object key,
                                                                                                        object 'value') cil managed
  // SIG: 20 02 01 1C 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/::set_Item(!0,
                                                                                                                                                                !1) /* 0A000016 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.set_Item'

  .method /*06000038*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.IDictionary<System.Object,System.Object>.Add'(object key,
                                                                                                   object 'value') cil managed
  // SIG: 20 02 01 1C 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/::Add(!0,
                                                                                                                                                           !1) /* 0A000017 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.Add'

  .method /*06000039*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.Object,System.Object>.ContainsKey'(object key) cil managed
  // SIG: 20 01 02 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/::ContainsKey(!0) /* 0A000018 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.ContainsKey'

  .method /*0600003A*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object> 
          'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Keys'() cil managed
  // SIG: 20 00 15 12 80 B5 01 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/::get_Keys() /* 0A000019 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Keys'

  .method /*0600003B*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.Object,System.Object>.Remove'(object key) cil managed
  // SIG: 20 01 02 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/::Remove(!0) /* 0A00001A */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.Remove'

  .method /*0600003C*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.IDictionary<System.Object,System.Object>.TryGetValue'(object key,
                                                                                                           [out] object& 'value') cil managed
  // SIG: 20 02 02 1C 10 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/::TryGetValue(!0,
                                                                                                                                                                   !1&) /* 0A00001B */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.TryGetValue'

  .method /*0600003D*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object> 
          'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Values'() cil managed
  // SIG: 20 00 15 12 80 B5 01 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<!1> class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>/*1B000001*/::get_Values() /* 0A00001C */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Values'

  .method /*0600003E*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Add'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object> item) cil managed
  // SIG: 20 01 01 15 11 80 B9 02 1C 1C
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/::Add(!0) /* 0A00001D */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Add'

  .method /*0600003F*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Clear'() cil managed
  // SIG: 20 00 01
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/::Clear() /* 0A00001E */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Clear'

  .method /*06000040*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Contains'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object> item) cil managed
  // SIG: 20 01 02 15 11 80 B9 02 1C 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/::Contains(!0) /* 0A00001F */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Contains'

  .method /*06000041*/ private hidebysig newslot virtual final 
          instance void  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.CopyTo'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>[] 'array',
                                                                                                                                               int32 arrayIndex) cil managed
  // SIG: 20 02 01 1D 15 11 80 B9 02 1C 1C 08
  {
    .override  method instance void class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/::CopyTo(!0[],
                                                                                                                                                                                                                                                           int32) /* 0A000020 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.CopyTo'

  .method /*06000042*/ private hidebysig newslot specialname virtual final 
          instance int32  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.get_Count'() cil managed
  // SIG: 20 00 08
  {
    .override  method instance int32 class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/::get_Count() /* 0A000021 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.get_Count'

  .method /*06000043*/ private hidebysig newslot specialname virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.get_IsReadOnly'() cil managed
  // SIG: 20 00 02
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/::get_IsReadOnly() /* 0A000022 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.get_IsReadOnly'

  .method /*06000044*/ private hidebysig newslot virtual final 
          instance bool  'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Remove'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object> item) cil managed
  // SIG: 20 01 02 15 11 80 B9 02 1C 1C
  {
    .override  method instance bool class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000002*/::Remove(!0) /* 0A000023 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Remove'

  .method /*06000045*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000030*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 80 C1 01 15 11 80 B9 02 1C 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000030*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100002F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*0100002E*/<object,object>>/*1B000003*/::GetEnumerator() /* 0A000024 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.GetEnumerator'

  .method /*06000046*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000032*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 80 C9
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000031*/::GetEnumerator /*01000031::0A000025*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionItems::System.Collections.IEnumerable.GetEnumerator

  .property /*17000009*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>
          Items()
  {
    .custom /*0C000030:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> Microsoft.AspNetCore.Connections.ConnectionItems/*02000010*/::get_Items() /* 06000035 */
  } // end of property ConnectionItems::Items
  .property /*1700000A*/ instance callconv(8) object
          'System.Collections.Generic.IDictionary<System.Object,System.Object>.Item'(object)
  {
    .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance object Microsoft.AspNetCore.Connections.ConnectionItems/*02000010*/::'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Item'(object) /* 06000036 */
    .set instance void Microsoft.AspNetCore.Connections.ConnectionItems/*02000010*/::'System.Collections.Generic.IDictionary<System.Object,System.Object>.set_Item'(object,
                                                                                                                                                                    object) /* 06000037 */
  } // end of property ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.Item'
  .property /*1700000B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object>
          'System.Collections.Generic.IDictionary<System.Object,System.Object>.Keys'()
  {
    .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object> Microsoft.AspNetCore.Connections.ConnectionItems/*02000010*/::'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Keys'() /* 0600003A */
  } // end of property ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.Keys'
  .property /*1700000C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object>
          'System.Collections.Generic.IDictionary<System.Object,System.Object>.Values'()
  {
    .custom /*0C00003D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.ICollection`1/*0100002D*/<object> Microsoft.AspNetCore.Connections.ConnectionItems/*02000010*/::'System.Collections.Generic.IDictionary<System.Object,System.Object>.get_Values'() /* 0600003D */
  } // end of property ConnectionItems::'System.Collections.Generic.IDictionary<System.Object,System.Object>.Values'
  .property /*1700000D*/ instance callconv(8) int32
          'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Count'()
  {
    .get instance int32 Microsoft.AspNetCore.Connections.ConnectionItems/*02000010*/::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.get_Count'() /* 06000042 */
  } // end of property ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.Count'
  .property /*1700000E*/ instance callconv(8) bool
          'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.IsReadOnly'()
  {
    .get instance bool Microsoft.AspNetCore.Connections.ConnectionItems/*02000010*/::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.get_IsReadOnly'() /* 06000043 */
  } // end of property ConnectionItems::'System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Object,System.Object>>.IsReadOnly'
} // end of class Microsoft.AspNetCore.Connections.ConnectionItems

.class /*02000011*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.DefaultConnectionContext
       extends Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/
       implements Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*02000027*/,
                  Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*02000029*/,
                  Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0200002D*/,
                  Microsoft.AspNetCore.Connections.Features.IConnectionUserFeature/*0200002E*/,
                  Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0200002A*/,
                  Microsoft.AspNetCore.Connections.Features.IConnectionEndPointFeature/*02000025*/
{
  .custom /*0C000046:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*02000027*/
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*02000029*/
  .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0200002D*/
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Connections.Features.IConnectionUserFeature/*0200002E*/
  .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0200002A*/
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Connections.Features.IConnectionEndPointFeature/*02000025*/
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003F*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000086:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000019*/ public static initonly class Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*//'<>c'/*0200003F*/ '<>9'
    .field /*0400001A*/ public static class [System.Threading.ThreadPool/*23000007*/]System.Threading.WaitCallback/*01000034*/ '<>9__39_0'
    .method /*060000B8*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

  } // end of class '<>c'

  .method /*06000047*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::.ctor

  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string id) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::.ctor

  .method /*06000049*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string id,
                               class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ transport,
                               class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ application) cil managed
  // SIG: 20 03 01 0E 12 80 B1 12 80 B1
  {
    .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::.ctor

  .method /*0600004A*/ public hidebysig specialname virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00008C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00008D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_ConnectionId

  .method /*0600004B*/ public hidebysig specialname virtual 
          instance void  set_ConnectionId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00008F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::set_ConnectionId

  .method /*0600004C*/ public hidebysig specialname virtual 
          instance class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/ 
          get_Features() cil managed
  // SIG: 20 00 12 80 85
  {
    .custom /*0C000090:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000091:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_Features

  .method /*0600004D*/ public hidebysig newslot specialname virtual final 
          instance class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/ 
          get_User() cil managed
  // SIG: 20 00 12 80 CD
  {
    .custom /*0C000092:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_User

  .method /*0600004E*/ public hidebysig newslot specialname virtual final 
          instance void  set_User(class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/ 'value') cil managed
  // SIG: 20 01 01 12 80 CD
  {
    .custom /*0C000093:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::set_User

  .method /*0600004F*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 80 89 02 1C 1C
  {
    .custom /*0C000094:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .param [0]/*08000045*/ 
    .custom /*0C000088:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_Items

  .method /*06000050*/ public hidebysig specialname virtual 
          instance void  set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 89 02 1C 1C
  {
    .custom /*0C000095:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .param [1]/*08000046*/ 
    .custom /*0C000089:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::set_Items

  .method /*06000051*/ public hidebysig specialname 
          instance class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ 
          get_Application() cil managed
  // SIG: 20 00 12 80 B1
  {
    .custom /*0C000096:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_Application

  .method /*06000052*/ public hidebysig specialname 
          instance void  set_Application(class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    .custom /*0C000097:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::set_Application

  .method /*06000053*/ public hidebysig specialname virtual 
          instance class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ 
          get_Transport() cil managed
  // SIG: 20 00 12 80 B1
  {
    .custom /*0C000098:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000099:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_Transport

  .method /*06000054*/ public hidebysig specialname virtual 
          instance void  set_Transport(class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00009B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::set_Transport

  .method /*06000055*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ 
          get_ConnectionClosed() cil managed
  // SIG: 20 00 11 80 8D
  {
    .custom /*0C00009C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_ConnectionClosed

  .method /*06000056*/ public hidebysig specialname virtual 
          instance void  set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 11 80 8D
  {
    .custom /*0C00009D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::set_ConnectionClosed

  .method /*06000057*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 
          get_LocalEndPoint() cil managed
  // SIG: 20 00 12 80 91
  {
    .custom /*0C00009E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_LocalEndPoint

  .method /*06000058*/ public hidebysig specialname virtual 
          instance void  set_LocalEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    .custom /*0C00009F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::set_LocalEndPoint

  .method /*06000059*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 
          get_RemoteEndPoint() cil managed
  // SIG: 20 00 12 80 91
  {
    .custom /*0C0000A0:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::get_RemoteEndPoint

  .method /*0600005A*/ public hidebysig specialname virtual 
          instance void  set_RemoteEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    .custom /*0C0000A2:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::set_RemoteEndPoint

  .method /*0600005B*/ public hidebysig virtual 
          instance void  Abort(class Microsoft.AspNetCore.Connections.ConnectionAbortedException/*02000013*/ abortReason) cil managed
  // SIG: 20 01 01 12 4C
  {
    .custom /*0C0000A3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::Abort

  .method /*0600005C*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultConnectionContext::DisposeAsync

  .property /*1700000F*/ instance callconv(8) string
          ConnectionId()
  {
    .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_ConnectionId() /* 0600004A */
    .set instance void Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::set_ConnectionId(string) /* 0600004B */
  } // end of property DefaultConnectionContext::ConnectionId
  .property /*17000010*/ instance callconv(8) class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/
          Features()
  {
    .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/ Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_Features() /* 0600004C */
  } // end of property DefaultConnectionContext::Features
  .property /*17000011*/ instance callconv(8) class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/
          User()
  {
    .get instance class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/ Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_User() /* 0600004D */
    .set instance void Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::set_User(class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/) /* 0600004E */
  } // end of property DefaultConnectionContext::User
  .property /*17000012*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>
          Items()
  {
    .custom /*0C00004A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_Items() /* 0600004F */
    .set instance void Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>) /* 06000050 */
  } // end of property DefaultConnectionContext::Items
  .property /*17000013*/ instance callconv(8) class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/
          Application()
  {
    .get instance class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_Application() /* 06000051 */
    .set instance void Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::set_Application(class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/) /* 06000052 */
  } // end of property DefaultConnectionContext::Application
  .property /*17000014*/ instance callconv(8) class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/
          Transport()
  {
    .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_Transport() /* 06000053 */
    .set instance void Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::set_Transport(class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/) /* 06000054 */
  } // end of property DefaultConnectionContext::Transport
  .property /*17000015*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/
          ConnectionClosed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_ConnectionClosed() /* 06000055 */
    .set instance void Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/) /* 06000056 */
  } // end of property DefaultConnectionContext::ConnectionClosed
  .property /*17000016*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
          LocalEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_LocalEndPoint() /* 06000057 */
    .set instance void Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::set_LocalEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/) /* 06000058 */
  } // end of property DefaultConnectionContext::LocalEndPoint
  .property /*17000017*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
          RemoteEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::get_RemoteEndPoint() /* 06000059 */
    .set instance void Microsoft.AspNetCore.Connections.DefaultConnectionContext/*02000011*/::set_RemoteEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/) /* 0600005A */
  } // end of property DefaultConnectionContext::RemoteEndPoint
} // end of class Microsoft.AspNetCore.Connections.DefaultConnectionContext

.class /*02000012*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.AddressInUseException
       extends [System.Runtime/*23000001*/]System.InvalidOperationException/*01000035*/
{
  .custom /*0C000048:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AddressInUseException::.ctor

  .method /*0600005E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000036*/ inner) cil managed
  // SIG: 20 02 01 0E 12 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AddressInUseException::.ctor

} // end of class Microsoft.AspNetCore.Connections.AddressInUseException

.class /*02000013*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.ConnectionAbortedException
       extends [System.Runtime/*23000001*/]System.OperationCanceledException/*01000037*/
{
  .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionAbortedException::.ctor

  .method /*06000060*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionAbortedException::.ctor

  .method /*06000061*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000036*/ inner) cil managed
  // SIG: 20 02 01 0E 12 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionAbortedException::.ctor

} // end of class Microsoft.AspNetCore.Connections.ConnectionAbortedException

.class /*02000014*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.ConnectionResetException
       extends [System.Runtime/*23000001*/]System.IO.IOException/*01000038*/
{
  .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000062*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionResetException::.ctor

  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000036*/ inner) cil managed
  // SIG: 20 02 01 0E 12 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionResetException::.ctor

} // end of class Microsoft.AspNetCore.Connections.ConnectionResetException

.class /*02000015*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.FileHandleEndPoint
       extends [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
{
  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint64 fileHandle,
                               valuetype Microsoft.AspNetCore.Connections.FileHandleType/*02000016*/ fileHandleType) cil managed
  // SIG: 20 02 01 0B 11 58
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileHandleEndPoint::.ctor

  .method /*06000065*/ public hidebysig specialname 
          instance uint64  get_FileHandle() cil managed
  // SIG: 20 00 0B
  {
    .custom /*0C0000A7:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileHandleEndPoint::get_FileHandle

  .method /*06000066*/ public hidebysig specialname 
          instance valuetype Microsoft.AspNetCore.Connections.FileHandleType/*02000016*/ 
          get_FileHandleType() cil managed
  // SIG: 20 00 11 58
  {
    .custom /*0C0000A8:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileHandleEndPoint::get_FileHandleType

  .property /*17000018*/ instance callconv(8) uint64
          FileHandle()
  {
    .get instance uint64 Microsoft.AspNetCore.Connections.FileHandleEndPoint/*02000015*/::get_FileHandle() /* 06000065 */
  } // end of property FileHandleEndPoint::FileHandle
  .property /*17000019*/ instance callconv(8) valuetype Microsoft.AspNetCore.Connections.FileHandleType/*02000016*/
          FileHandleType()
  {
    .get instance valuetype Microsoft.AspNetCore.Connections.FileHandleType/*02000016*/ Microsoft.AspNetCore.Connections.FileHandleEndPoint/*02000015*/::get_FileHandleType() /* 06000066 */
  } // end of property FileHandleEndPoint::FileHandleType
} // end of class Microsoft.AspNetCore.Connections.FileHandleEndPoint

.class /*02000016*/ public auto ansi sealed Microsoft.AspNetCore.Connections.FileHandleType
       extends [System.Runtime/*23000001*/]System.Enum/*01000039*/
{
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype Microsoft.AspNetCore.Connections.FileHandleType/*02000016*/ Auto = int32(0x00000000)
  .field /*04000006*/ public static literal valuetype Microsoft.AspNetCore.Connections.FileHandleType/*02000016*/ Tcp = int32(0x00000001)
  .field /*04000007*/ public static literal valuetype Microsoft.AspNetCore.Connections.FileHandleType/*02000016*/ Pipe = int32(0x00000002)
} // end of class Microsoft.AspNetCore.Connections.FileHandleType

.class /*02000017*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.IConnectionBuilder
{
  .custom /*0C000051:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000067*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ 
          get_ApplicationServices() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionBuilder::get_ApplicationServices

  .method /*06000068*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/ 
          Use(class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/,class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/> middleware) cil managed
  // SIG: 20 01 12 5C 15 12 80 99 02 12 38 12 38
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionBuilder::Use

  .method /*06000069*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Connections.ConnectionDelegate/*0200000E*/ 
          Build() cil managed
  // SIG: 20 00 12 38
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionBuilder::Build

  .property /*1700001A*/ instance callconv(8) class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/
          ApplicationServices()
  {
    .get instance class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ Microsoft.AspNetCore.Connections.IConnectionBuilder/*02000017*/::get_ApplicationServices() /* 06000067 */
  } // end of property IConnectionBuilder::ApplicationServices
} // end of class Microsoft.AspNetCore.Connections.IConnectionBuilder

.class /*02000018*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.IConnectionFactory
{
  .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600006A*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003A*/<class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/> 
          ConnectAsync(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ endpoint,
                       [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 E9 01 12 34 12 80 91 11 80 8D
  {
    .param [0]/*08000059*/ 
    .custom /*0C0000A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800005B*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IConnectionFactory::ConnectAsync

} // end of class Microsoft.AspNetCore.Connections.IConnectionFactory

.class /*02000019*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.IConnectionListener
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
{
  .custom /*0C000058:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
  .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006B*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 
          get_EndPoint() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionListener::get_EndPoint

  .method /*0600006C*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003A*/<class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/> 
          AcceptAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 01 15 11 80 E9 01 12 34 11 80 8D
  {
    .param [0]/*0800005C*/ 
    .custom /*0C0000A4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [1]/*0800005D*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IConnectionListener::AcceptAsync

  .method /*0600006D*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          UnbindAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 01 11 80 95 11 80 8D
  {
    .param [1]/*0800005E*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IConnectionListener::UnbindAsync

  .property /*1700001B*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
          EndPoint()
  {
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ Microsoft.AspNetCore.Connections.IConnectionListener/*02000019*/::get_EndPoint() /* 0600006B */
  } // end of property IConnectionListener::EndPoint
} // end of class Microsoft.AspNetCore.Connections.IConnectionListener

.class /*0200001A*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.IConnectionListenerFactory
{
  .custom /*0C00005B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600006E*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003A*/<class Microsoft.AspNetCore.Connections.IConnectionListener/*02000019*/> 
          BindAsync(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ endpoint,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 E9 01 12 64 12 80 91 11 80 8D
  {
    .param [0]/*0800005F*/ 
    .custom /*0C0000A5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000061*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IConnectionListenerFactory::BindAsync

} // end of class Microsoft.AspNetCore.Connections.IConnectionListenerFactory

.class /*0200001B*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.IMultiplexedConnectionListener
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
{
  .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
  .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006F*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 
          get_EndPoint() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x0
  } // end of method IMultiplexedConnectionListener::get_EndPoint

  .method /*06000070*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          UnbindAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 01 11 80 95 11 80 8D
  {
    .param [1]/*08000062*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IMultiplexedConnectionListener::UnbindAsync

  .method /*06000071*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003A*/<class Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*02000020*/> 
          AcceptAsync([opt] class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/ features,
                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 E9 01 12 80 80 12 80 85 11 80 8D
  {
    .custom /*0C0000AD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000063*/ 
    .custom /*0C0000A6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [1]/*08000064*/  = nullref
    .param [2]/*08000065*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IMultiplexedConnectionListener::AcceptAsync

  .property /*1700001C*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
          EndPoint()
  {
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ Microsoft.AspNetCore.Connections.IMultiplexedConnectionListener/*0200001B*/::get_EndPoint() /* 0600006F */
  } // end of property IMultiplexedConnectionListener::EndPoint
} // end of class Microsoft.AspNetCore.Connections.IMultiplexedConnectionListener

.class /*0200001C*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.IMultiplexedConnectionBuilder
{
  .custom /*0C000061:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000072*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ 
          get_ApplicationServices() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x0
  } // end of method IMultiplexedConnectionBuilder::get_ApplicationServices

  .method /*06000073*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Connections.IMultiplexedConnectionBuilder/*0200001C*/ 
          Use(class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<class Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate/*02000021*/,class Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate/*02000021*/> middleware) cil managed
  // SIG: 20 01 12 70 15 12 80 99 02 12 80 84 12 80 84
  {
    // Method begins at RVA 0x0
  } // end of method IMultiplexedConnectionBuilder::Use

  .method /*06000074*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate/*02000021*/ 
          Build() cil managed
  // SIG: 20 00 12 80 84
  {
    // Method begins at RVA 0x0
  } // end of method IMultiplexedConnectionBuilder::Build

  .property /*1700001D*/ instance callconv(8) class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/
          ApplicationServices()
  {
    .get instance class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ Microsoft.AspNetCore.Connections.IMultiplexedConnectionBuilder/*0200001C*/::get_ApplicationServices() /* 06000072 */
  } // end of property IMultiplexedConnectionBuilder::ApplicationServices
} // end of class Microsoft.AspNetCore.Connections.IMultiplexedConnectionBuilder

.class /*0200001D*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.IMultiplexedConnectionFactory
{
  .custom /*0C000062:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000075*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003A*/<class Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*02000020*/> 
          ConnectAsync(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ endpoint,
                       [opt] class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/ features,
                       [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 03 15 11 80 E9 01 12 80 80 12 80 91 12 80 85 11 80 8D
  {
    .param [0]/*08000067*/ 
    .custom /*0C0000A9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000069*/  = nullref
    .custom /*0C0000AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800006A*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IMultiplexedConnectionFactory::ConnectAsync

} // end of class Microsoft.AspNetCore.Connections.IMultiplexedConnectionFactory

.class /*0200001E*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.IMultiplexedConnectionListenerFactory
{
  .custom /*0C000063:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000076*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003A*/<class Microsoft.AspNetCore.Connections.IMultiplexedConnectionListener/*0200001B*/> 
          BindAsync(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ endpoint,
                    [opt] class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/ features,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 03 15 11 80 E9 01 12 6C 12 80 91 12 80 85 11 80 8D
  {
    .param [0]/*0800006B*/ 
    .custom /*0C0000AB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800006D*/  = nullref
    .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800006E*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IMultiplexedConnectionListenerFactory::BindAsync

} // end of class Microsoft.AspNetCore.Connections.IMultiplexedConnectionListenerFactory

.class /*0200001F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.MultiplexedConnectionBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Connections.IMultiplexedConnectionBuilder/*0200001C*/
{
  .custom /*0C000064:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000065:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Connections.IMultiplexedConnectionBuilder/*0200001C*/
  .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000040*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000087:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400001B*/ public static initonly class Microsoft.AspNetCore.Connections.MultiplexedConnectionBuilder/*0200001F*//'<>c'/*02000040*/ '<>9'
    .field /*0400001C*/ public static class Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate/*02000021*/ '<>9__6_0'
    .method /*060000B9*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

  } // end of class '<>c'

  .method /*06000077*/ public hidebysig newslot specialname virtual final 
          instance class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ 
          get_ApplicationServices() cil managed
  // SIG: 20 00 12 5D
  {
    .custom /*0C0000B0:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultiplexedConnectionBuilder::get_ApplicationServices

  .method /*06000078*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ applicationServices) cil managed
  // SIG: 20 01 01 12 5D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultiplexedConnectionBuilder::.ctor

  .method /*06000079*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Connections.IMultiplexedConnectionBuilder/*0200001C*/ 
          Use(class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<class Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate/*02000021*/,class Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate/*02000021*/> middleware) cil managed
  // SIG: 20 01 12 70 15 12 80 99 02 12 80 84 12 80 84
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultiplexedConnectionBuilder::Use

  .method /*0600007A*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate/*02000021*/ 
          Build() cil managed
  // SIG: 20 00 12 80 84
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultiplexedConnectionBuilder::Build

  .property /*1700001E*/ instance callconv(8) class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/
          ApplicationServices()
  {
    .get instance class [System.ComponentModel/*23000002*/]System.IServiceProvider/*01000017*/ Microsoft.AspNetCore.Connections.MultiplexedConnectionBuilder/*0200001F*/::get_ApplicationServices() /* 06000077 */
  } // end of property MultiplexedConnectionBuilder::ApplicationServices
} // end of class Microsoft.AspNetCore.Connections.MultiplexedConnectionBuilder

.class /*02000020*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Connections.MultiplexedConnectionContext
       extends Microsoft.AspNetCore.Connections.BaseConnectionContext/*0200000A*/
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000020*/
{
  .method /*0600007B*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003A*/<class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/> 
          AcceptAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 01 15 11 80 E9 01 12 34 11 80 8D
  {
    .param [0]/*08000071*/ 
    .custom /*0C0000AE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [1]/*08000072*/  = nullref
    // Method begins at RVA 0x0
  } // end of method MultiplexedConnectionContext::AcceptAsync

  .method /*0600007C*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003A*/<class Microsoft.AspNetCore.Connections.ConnectionContext/*0200000D*/> 
          ConnectAsync([opt] class [Microsoft.Extensions.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000021*/ features,
                       [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 E9 01 12 34 12 80 85 11 80 8D
  {
    .custom /*0C0000B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000073*/ 
    .custom /*0C0000AF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000074*/  = nullref
    .param [2]/*08000075*/  = nullref
    // Method begins at RVA 0x0
  } // end of method MultiplexedConnectionContext::ConnectAsync

  .method /*0600007D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultiplexedConnectionContext::.ctor

} // end of class Microsoft.AspNetCore.Connections.MultiplexedConnectionContext

.class /*02000021*/ public auto ansi sealed Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100001D*/
{
  .method /*0600007E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method MultiplexedConnectionDelegate::.ctor

  .method /*0600007F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/ 
          Invoke(class Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*02000020*/ connection) runtime managed
  // SIG: 20 01 12 80 AD 12 80 80
  {
    .custom /*0C0000B4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  } // end of method MultiplexedConnectionDelegate::Invoke

  .method /*06000080*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001E*/ 
          BeginInvoke(class Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*02000020*/ connection,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001F*/ callback,
                      object 'object') runtime managed
  // SIG: 20 03 12 79 12 80 80 12 7D 1C
  {
    .param [1]/*08000079*/ 
    .custom /*0C0000B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method MultiplexedConnectionDelegate::BeginInvoke

  .method /*06000081*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/ 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001E*/ result) runtime managed
  // SIG: 20 01 12 80 AD 12 79
  {
    .param [0]/*0800007C*/ 
    .custom /*0C0000B3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method MultiplexedConnectionDelegate::EndInvoke

} // end of class Microsoft.AspNetCore.Connections.MultiplexedConnectionDelegate

.class /*02000022*/ public auto ansi sealed Microsoft.AspNetCore.Connections.TransferFormat
       extends [System.Runtime/*23000001*/]System.Enum/*01000039*/
{
  .custom /*0C000067:0A000026*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*0100003B*/::.ctor() /* 0A000026 */ = ( 01 00 00 00 ) 
  .field /*04000008*/ public specialname rtspecialname int32 value__
  .field /*04000009*/ public static literal valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/ Binary = int32(0x00000001)
  .field /*0400000A*/ public static literal valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/ Text = int32(0x00000002)
} // end of class Microsoft.AspNetCore.Connections.TransferFormat

.class /*02000023*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Connections.UriEndPoint
       extends [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
{
  .custom /*0C000068:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000082*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Uri/*0100003C*/ uri) cil managed
  // SIG: 20 01 01 12 80 F1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UriEndPoint::.ctor

  .method /*06000083*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*0100003C*/ 
          get_Uri() cil managed
  // SIG: 20 00 12 80 F1
  {
    .custom /*0C0000B5:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UriEndPoint::get_Uri

  .method /*06000084*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UriEndPoint::ToString

  .property /*1700001F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*0100003C*/
          Uri()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*0100003C*/ Microsoft.AspNetCore.Connections.UriEndPoint/*02000023*/::get_Uri() /* 06000083 */
  } // end of property UriEndPoint::Uri
} // end of class Microsoft.AspNetCore.Connections.UriEndPoint

.class /*02000024*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionCompleteFeature
{
  .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000085*/ public hidebysig newslot abstract virtual 
          instance void  OnCompleted(class [System.Runtime/*23000001*/]System.Func`2/*01000026*/<object,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002B*/> callback,
                                     object state) cil managed
  // SIG: 20 02 01 15 12 80 99 02 1C 12 80 AD 1C
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionCompleteFeature::OnCompleted

} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionCompleteFeature

.class /*02000025*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionEndPointFeature
{
  .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .method /*06000086*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 
          get_LocalEndPoint() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionEndPointFeature::get_LocalEndPoint

  .method /*06000087*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_LocalEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionEndPointFeature::set_LocalEndPoint

  .method /*06000088*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 
          get_RemoteEndPoint() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionEndPointFeature::get_RemoteEndPoint

  .method /*06000089*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_RemoteEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ 'value') cil managed
  // SIG: 20 01 01 12 80 91
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionEndPointFeature::set_RemoteEndPoint

  .property /*17000020*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
          LocalEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ Microsoft.AspNetCore.Connections.Features.IConnectionEndPointFeature/*02000025*/::get_LocalEndPoint() /* 06000086 */
    .set instance void Microsoft.AspNetCore.Connections.Features.IConnectionEndPointFeature/*02000025*/::set_LocalEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/) /* 06000087 */
  } // end of property IConnectionEndPointFeature::LocalEndPoint
  .property /*17000021*/ instance callconv(8) class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/
          RemoteEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/ Microsoft.AspNetCore.Connections.Features.IConnectionEndPointFeature/*02000025*/::get_RemoteEndPoint() /* 06000088 */
    .set instance void Microsoft.AspNetCore.Connections.Features.IConnectionEndPointFeature/*02000025*/::set_RemoteEndPoint(class [System.Net.Primitives/*23000004*/]System.Net.EndPoint/*01000024*/) /* 06000089 */
  } // end of property IConnectionEndPointFeature::RemoteEndPoint
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionEndPointFeature

.class /*02000026*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionHeartbeatFeature
{
  .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600008A*/ public hidebysig newslot abstract virtual 
          instance void  OnHeartbeat(class [System.Runtime/*23000001*/]System.Action`1/*0100003D*/<object> action,
                                     object state) cil managed
  // SIG: 20 02 01 15 12 80 F5 01 1C 1C
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionHeartbeatFeature::OnHeartbeat

} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionHeartbeatFeature

.class /*02000027*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature
{
  .custom /*0C000071:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600008B*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionIdFeature::get_ConnectionId

  .method /*0600008C*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_ConnectionId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionIdFeature::set_ConnectionId

  .property /*17000022*/ instance callconv(8) string
          ConnectionId()
  {
    .get instance string Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*02000027*/::get_ConnectionId() /* 0600008B */
    .set instance void Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*02000027*/::set_ConnectionId(string) /* 0600008C */
  } // end of property IConnectionIdFeature::ConnectionId
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature

.class /*02000028*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionInherentKeepAliveFeature
{
  .method /*0600008D*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_HasInherentKeepAlive() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionInherentKeepAliveFeature::get_HasInherentKeepAlive

  .property /*17000023*/ instance callconv(8) bool
          HasInherentKeepAlive()
  {
    .get instance bool Microsoft.AspNetCore.Connections.Features.IConnectionInherentKeepAliveFeature/*02000028*/::get_HasInherentKeepAlive() /* 0600008D */
  } // end of property IConnectionInherentKeepAliveFeature::HasInherentKeepAlive
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionInherentKeepAliveFeature

.class /*02000029*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature
{
  .method /*0600008E*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 80 89 02 1C 1C
  {
    .param [0]/*08000086*/ 
    .custom /*0C0000B6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IConnectionItemsFeature::get_Items

  .method /*0600008F*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> 'value') cil managed
  // SIG: 20 01 01 15 12 80 89 02 1C 1C
  {
    .param [1]/*08000087*/ 
    .custom /*0C0000B7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IConnectionItemsFeature::set_Items

  .property /*17000024*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>
          Items()
  {
    .custom /*0C00006B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*02000029*/::get_Items() /* 0600008E */
    .set instance void Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*02000029*/::set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>) /* 0600008F */
  } // end of property IConnectionItemsFeature::Items
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature

.class /*0200002A*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature
{
  .method /*06000090*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ 
          get_ConnectionClosed() cil managed
  // SIG: 20 00 11 80 8D
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionLifetimeFeature::get_ConnectionClosed

  .method /*06000091*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 11 80 8D
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionLifetimeFeature::set_ConnectionClosed

  .method /*06000092*/ public hidebysig newslot abstract virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionLifetimeFeature::Abort

  .property /*17000025*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/
          ConnectionClosed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0200002A*/::get_ConnectionClosed() /* 06000090 */
    .set instance void Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0200002A*/::set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/) /* 06000091 */
  } // end of property IConnectionLifetimeFeature::ConnectionClosed
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature

.class /*0200002B*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeNotificationFeature
{
  .method /*06000093*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ 
          get_ConnectionClosedRequested() cil managed
  // SIG: 20 00 11 80 8D
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionLifetimeNotificationFeature::get_ConnectionClosedRequested

  .method /*06000094*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_ConnectionClosedRequested(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ 'value') cil managed
  // SIG: 20 01 01 11 80 8D
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionLifetimeNotificationFeature::set_ConnectionClosedRequested

  .method /*06000095*/ public hidebysig newslot abstract virtual 
          instance void  RequestClose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionLifetimeNotificationFeature::RequestClose

  .property /*17000026*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/
          ConnectionClosedRequested()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/ Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeNotificationFeature/*0200002B*/::get_ConnectionClosedRequested() /* 06000093 */
    .set instance void Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeNotificationFeature/*0200002B*/::set_ConnectionClosedRequested(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000023*/) /* 06000094 */
  } // end of property IConnectionLifetimeNotificationFeature::ConnectionClosedRequested
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeNotificationFeature

.class /*0200002C*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionSocketFeature
{
  .custom /*0C000074:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000096*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Net.Sockets/*23000008*/]System.Net.Sockets.Socket/*0100003E*/ 
          get_Socket() cil managed
  // SIG: 20 00 12 80 F9
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionSocketFeature::get_Socket

  .property /*17000027*/ instance callconv(8) class [System.Net.Sockets/*23000008*/]System.Net.Sockets.Socket/*0100003E*/
          Socket()
  {
    .get instance class [System.Net.Sockets/*23000008*/]System.Net.Sockets.Socket/*0100003E*/ Microsoft.AspNetCore.Connections.Features.IConnectionSocketFeature/*0200002C*/::get_Socket() /* 06000096 */
  } // end of property IConnectionSocketFeature::Socket
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionSocketFeature

.class /*0200002D*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature
{
  .custom /*0C000076:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000097*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ 
          get_Transport() cil managed
  // SIG: 20 00 12 80 B1
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionTransportFeature::get_Transport

  .method /*06000098*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Transport(class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ 'value') cil managed
  // SIG: 20 01 01 12 80 B1
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionTransportFeature::set_Transport

  .property /*17000028*/ instance callconv(8) class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/
          Transport()
  {
    .get instance class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/ Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0200002D*/::get_Transport() /* 06000097 */
    .set instance void Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0200002D*/::set_Transport(class [System.IO.Pipelines/*23000005*/]System.IO.Pipelines.IDuplexPipe/*0100002C*/) /* 06000098 */
  } // end of property IConnectionTransportFeature::Transport
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature

.class /*0200002E*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IConnectionUserFeature
{
  .custom /*0C000078:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .method /*06000099*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/ 
          get_User() cil managed
  // SIG: 20 00 12 80 CD
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionUserFeature::get_User

  .method /*0600009A*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_User(class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/ 'value') cil managed
  // SIG: 20 01 01 12 80 CD
  {
    // Method begins at RVA 0x0
  } // end of method IConnectionUserFeature::set_User

  .property /*17000029*/ instance callconv(8) class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/
          User()
  {
    .get instance class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/ Microsoft.AspNetCore.Connections.Features.IConnectionUserFeature/*0200002E*/::get_User() /* 06000099 */
    .set instance void Microsoft.AspNetCore.Connections.Features.IConnectionUserFeature/*0200002E*/::set_User(class [System.Security.Claims/*23000006*/]System.Security.Claims.ClaimsPrincipal/*01000033*/) /* 0600009A */
  } // end of property IConnectionUserFeature::User
} // end of class Microsoft.AspNetCore.Connections.Features.IConnectionUserFeature

.class /*0200002F*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature
{
  .custom /*0C000079:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600009B*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Memory/*23000009*/]System.Buffers.MemoryPool`1/*0100003F*/<uint8> 
          get_MemoryPool() cil managed
  // SIG: 20 00 15 12 80 FD 01 05
  {
    // Method begins at RVA 0x0
  } // end of method IMemoryPoolFeature::get_MemoryPool

  .property /*1700002A*/ instance callconv(8) class [System.Memory/*23000009*/]System.Buffers.MemoryPool`1/*0100003F*/<uint8>
          MemoryPool()
  {
    .get instance class [System.Memory/*23000009*/]System.Buffers.MemoryPool`1/*0100003F*/<uint8> Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0200002F*/::get_MemoryPool() /* 0600009B */
  } // end of property IMemoryPoolFeature::MemoryPool
} // end of class Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature

.class /*02000030*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature
{
  .method /*0600009C*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> 
          get_State() cil managed
  // SIG: 20 00 15 12 80 89 02 1C 1C
  {
    .param [0]/*0800008C*/ 
    .custom /*0C0000B8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IPersistentStateFeature::get_State

  .property /*1700002B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object>
          State()
  {
    .custom /*0C000073:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000022*/<object,object> Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature/*02000030*/::get_State() /* 0600009C */
  } // end of property IPersistentStateFeature::State
} // end of class Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature

.class /*02000031*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature
{
  .method /*0600009D*/ public hidebysig newslot specialname abstract virtual 
          instance int64  get_Error() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x0
  } // end of method IProtocolErrorCodeFeature::get_Error

  .method /*0600009E*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Error(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x0
  } // end of method IProtocolErrorCodeFeature::set_Error

  .property /*1700002C*/ instance callconv(8) int64
          Error()
  {
    .get instance int64 Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature/*02000031*/::get_Error() /* 0600009D */
    .set instance void Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature/*02000031*/::set_Error(int64) /* 0600009E */
  } // end of property IProtocolErrorCodeFeature::Error
} // end of class Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature

.class /*02000032*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IStreamAbortFeature
{
  .custom /*0C00007D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600009F*/ public hidebysig newslot abstract virtual 
          instance void  AbortRead(int64 errorCode,
                                   class Microsoft.AspNetCore.Connections.ConnectionAbortedException/*02000013*/ abortReason) cil managed
  // SIG: 20 02 01 0A 12 4C
  {
    // Method begins at RVA 0x0
  } // end of method IStreamAbortFeature::AbortRead

  .method /*060000A0*/ public hidebysig newslot abstract virtual 
          instance void  AbortWrite(int64 errorCode,
                                    class Microsoft.AspNetCore.Connections.ConnectionAbortedException/*02000013*/ abortReason) cil managed
  // SIG: 20 02 01 0A 12 4C
  {
    // Method begins at RVA 0x0
  } // end of method IStreamAbortFeature::AbortWrite

} // end of class Microsoft.AspNetCore.Connections.Features.IStreamAbortFeature

.class /*02000033*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IStreamDirectionFeature
{
  .method /*060000A1*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IStreamDirectionFeature::get_CanRead

  .method /*060000A2*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IStreamDirectionFeature::get_CanWrite

  .property /*1700002D*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool Microsoft.AspNetCore.Connections.Features.IStreamDirectionFeature/*02000033*/::get_CanRead() /* 060000A1 */
  } // end of property IStreamDirectionFeature::CanRead
  .property /*1700002E*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool Microsoft.AspNetCore.Connections.Features.IStreamDirectionFeature/*02000033*/::get_CanWrite() /* 060000A2 */
  } // end of property IStreamDirectionFeature::CanWrite
} // end of class Microsoft.AspNetCore.Connections.Features.IStreamDirectionFeature

.class /*02000034*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.IStreamIdFeature
{
  .method /*060000A3*/ public hidebysig newslot specialname abstract virtual 
          instance int64  get_StreamId() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x0
  } // end of method IStreamIdFeature::get_StreamId

  .property /*1700002F*/ instance callconv(8) int64
          StreamId()
  {
    .get instance int64 Microsoft.AspNetCore.Connections.Features.IStreamIdFeature/*02000034*/::get_StreamId() /* 060000A3 */
  } // end of property IStreamIdFeature::StreamId
} // end of class Microsoft.AspNetCore.Connections.Features.IStreamIdFeature

.class /*02000035*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature
{
  .method /*060000A4*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.SslProtocols/*01000040*/ 
          get_Protocol() cil managed
  // SIG: 20 00 11 81 01
  {
    // Method begins at RVA 0x0
  } // end of method ITlsHandshakeFeature::get_Protocol

  .method /*060000A5*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.CipherAlgorithmType/*01000041*/ 
          get_CipherAlgorithm() cil managed
  // SIG: 20 00 11 81 05
  {
    // Method begins at RVA 0x0
  } // end of method ITlsHandshakeFeature::get_CipherAlgorithm

  .method /*060000A6*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_CipherStrength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ITlsHandshakeFeature::get_CipherStrength

  .method /*060000A7*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.HashAlgorithmType/*01000042*/ 
          get_HashAlgorithm() cil managed
  // SIG: 20 00 11 81 09
  {
    // Method begins at RVA 0x0
  } // end of method ITlsHandshakeFeature::get_HashAlgorithm

  .method /*060000A8*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_HashStrength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ITlsHandshakeFeature::get_HashStrength

  .method /*060000A9*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.ExchangeAlgorithmType/*01000043*/ 
          get_KeyExchangeAlgorithm() cil managed
  // SIG: 20 00 11 81 0D
  {
    // Method begins at RVA 0x0
  } // end of method ITlsHandshakeFeature::get_KeyExchangeAlgorithm

  .method /*060000AA*/ public hidebysig newslot specialname abstract virtual 
          instance int32  get_KeyExchangeStrength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x0
  } // end of method ITlsHandshakeFeature::get_KeyExchangeStrength

  .property /*17000030*/ instance callconv(8) valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.SslProtocols/*01000040*/
          Protocol()
  {
    .get instance valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.SslProtocols/*01000040*/ Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature/*02000035*/::get_Protocol() /* 060000A4 */
  } // end of property ITlsHandshakeFeature::Protocol
  .property /*17000031*/ instance callconv(8) valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.CipherAlgorithmType/*01000041*/
          CipherAlgorithm()
  {
    .get instance valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.CipherAlgorithmType/*01000041*/ Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature/*02000035*/::get_CipherAlgorithm() /* 060000A5 */
  } // end of property ITlsHandshakeFeature::CipherAlgorithm
  .property /*17000032*/ instance callconv(8) int32
          CipherStrength()
  {
    .get instance int32 Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature/*02000035*/::get_CipherStrength() /* 060000A6 */
  } // end of property ITlsHandshakeFeature::CipherStrength
  .property /*17000033*/ instance callconv(8) valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.HashAlgorithmType/*01000042*/
          HashAlgorithm()
  {
    .get instance valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.HashAlgorithmType/*01000042*/ Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature/*02000035*/::get_HashAlgorithm() /* 060000A7 */
  } // end of property ITlsHandshakeFeature::HashAlgorithm
  .property /*17000034*/ instance callconv(8) int32
          HashStrength()
  {
    .get instance int32 Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature/*02000035*/::get_HashStrength() /* 060000A8 */
  } // end of property ITlsHandshakeFeature::HashStrength
  .property /*17000035*/ instance callconv(8) valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.ExchangeAlgorithmType/*01000043*/
          KeyExchangeAlgorithm()
  {
    .get instance valuetype [System.Net.Primitives/*23000004*/]System.Security.Authentication.ExchangeAlgorithmType/*01000043*/ Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature/*02000035*/::get_KeyExchangeAlgorithm() /* 060000A9 */
  } // end of property ITlsHandshakeFeature::KeyExchangeAlgorithm
  .property /*17000036*/ instance callconv(8) int32
          KeyExchangeStrength()
  {
    .get instance int32 Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature/*02000035*/::get_KeyExchangeStrength() /* 060000AA */
  } // end of property ITlsHandshakeFeature::KeyExchangeStrength
} // end of class Microsoft.AspNetCore.Connections.Features.ITlsHandshakeFeature

.class /*02000036*/ interface public abstract auto ansi Microsoft.AspNetCore.Connections.Features.ITransferFormatFeature
{
  .method /*060000AB*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/ 
          get_SupportedFormats() cil managed
  // SIG: 20 00 11 80 88
  {
    // Method begins at RVA 0x0
  } // end of method ITransferFormatFeature::get_SupportedFormats

  .method /*060000AC*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/ 
          get_ActiveFormat() cil managed
  // SIG: 20 00 11 80 88
  {
    // Method begins at RVA 0x0
  } // end of method ITransferFormatFeature::get_ActiveFormat

  .method /*060000AD*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_ActiveFormat(valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/ 'value') cil managed
  // SIG: 20 01 01 11 80 88
  {
    // Method begins at RVA 0x0
  } // end of method ITransferFormatFeature::set_ActiveFormat

  .property /*17000037*/ instance callconv(8) valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/
          SupportedFormats()
  {
    .get instance valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/ Microsoft.AspNetCore.Connections.Features.ITransferFormatFeature/*02000036*/::get_SupportedFormats() /* 060000AB */
  } // end of property ITransferFormatFeature::SupportedFormats
  .property /*17000038*/ instance callconv(8) valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/
          ActiveFormat()
  {
    .get instance valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/ Microsoft.AspNetCore.Connections.Features.ITransferFormatFeature/*02000036*/::get_ActiveFormat() /* 060000AC */
    .set instance void Microsoft.AspNetCore.Connections.Features.ITransferFormatFeature/*02000036*/::set_ActiveFormat(valuetype Microsoft.AspNetCore.Connections.TransferFormat/*02000022*/) /* 060000AD */
  } // end of property ITransferFormatFeature::ActiveFormat
} // end of class Microsoft.AspNetCore.Connections.Features.ITransferFormatFeature


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
