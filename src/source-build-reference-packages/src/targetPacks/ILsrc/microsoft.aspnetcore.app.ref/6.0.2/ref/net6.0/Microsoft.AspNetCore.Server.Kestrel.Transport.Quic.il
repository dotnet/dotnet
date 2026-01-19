


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
// Time-date stamp:            0xdf0030a7
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00007400
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000b3b6
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
// Size of image:                  0x00010000
// Size of headers:                0x00000400
// Checksum:                       0x0000f80f
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000b364 [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x000005ac] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00008200 [0x00002470] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000b348 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000073cc Virtual Size
//              0x00004000 Virtual Address
//              0x00007400 Size of Raw Data
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
//              0x000005ac Virtual Size
//              0x0000c000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00007a00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x0000e000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00008000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000b000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000003b8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x0000b3a6 Import Name Table
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
// 0x00004054 [0x00007274] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000b2c8 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00003588 Size
//              '#~' Name
//    Stream 2:
//              0x000035f4 Offset
//              0x000029bc Size
//              '#Strings' Name
//    Stream 3:
//              0x00005fb0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00005fb4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00005fc4 Offset
//              0x000012b0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000016092ba0df57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 42608
// PE header size       : 1024 (536 used)    ( 2.40%)
// PE additional info   : 10979             (25.77%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.17%)
// CLR meta-data size  : 29300              (68.77%)
// CLR additional info : 128                ( 0.30%)
// CLR method headers  : 309                ( 0.73%)
// Managed code         : 618               ( 1.45%)
// Data                 : 2560              ( 6.01%)
// Unaccounted          : -2382             (-5.59%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 29696
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 29300
//   Module        -    1 (10 bytes)
//   TypeDef       -   63 (882 bytes)     3 interfaces, 0 explicit layout
//   TypeRef       -  117 (702 bytes)
//   MethodDef     -  327 (4578 bytes)    18 abstract, 0 native, 309 bodies
//   FieldDef      -  133 (798 bytes)     5 constant
//   MemberRef     -   50 (300 bytes)
//   ParamDef      -  270 (1620 bytes)
//   MethodImpl    -   71 (426 bytes)
//   Constant      -   35 (210 bytes)
//   CustomAttribute-  333 (1998 bytes)
//   ClassLayout   -    1 (8 bytes)
//   InterfaceImpl -   59 (236 bytes)
//   PropertyMap   -   22 (88 bytes)
//   Property      -   81 (486 bytes)
//   MethodSemantic-  115 (690 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    5 (10 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   16 (320 bytes)
//   NestedClass   -   27 (108 bytes)
//   FieldRVA      -    1 (6 bytes)
//   GenericParam  -    9 (72 bytes)
//   GenericParamConstraint-    2 (8 bytes)
//   Strings       - 10681 bytes
//   Blobs         -  4784 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   231 bytes

// CLR additional info : 128

// CLR method headers : 309
//   Num.of method bodies  - 309
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 309

// Managed code : 618
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class Interop                        (abstract) (auto) (ansi) (sealed) 
// Class TaskToApm                      (abstract) (auto) (ansi) (sealed) 
// Class DuplexPipe                     (auto) (ansi) 
// Class DiagnosticMemoryPool           (auto) (ansi) 
// Class DiagnosticPoolBlock            (auto) (ansi) (sealed) 
// Class MemoryPoolBlock                (auto) (ansi) (sealed) 
// Class PinnedBlockMemoryPoolFactory   (abstract) (auto) (ansi) (sealed) 
// Class MemoryPoolThrowHelper          (auto) (ansi) 
// Class PinnedBlockMemoryPool          (auto) (ansi) (sealed) 
// Class CompletionPipeReader           (auto) (ansi) (sealed) 
// Class CompletionPipeWriter           (auto) (ansi) (sealed) 
// Class ConnectionCompletion           (abstract) (auto) (ansi) (sealed) 
// Class CorrelationIdGenerator         (abstract) (auto) (ansi) (sealed) 
// Class TransportConnection            (abstract) (auto) (ansi) 
// Class TransportMultiplexedConnection (abstract) (auto) (ansi) 
// Class WebHostBuilderQuicExtensions   (public) (abstract) (auto) (ansi) (sealed) 
// Interface IPooledStream                  (abstract) (auto) (ansi) 
// Class PooledStreamStack`1            (sequential) (ansi) (sealed) 
// Class StringUtilities                (abstract) (auto) (ansi) (sealed) 
// Class QuicConnectionFactory          (auto) (ansi) 
// Class QuicTransportFactory           (auto) (ansi) 
// Class QuicTransportOptions           (public) (auto) (ansi) 
// Interface IQuicTrace                     (abstract) (auto) (ansi) 
// Interface ISystemClock                   (abstract) (auto) (ansi) 
// Class SystemClock                    (auto) (ansi) 
// Class QuicConnectionContext          (auto) (ansi) (sealed) 
// Class QuicConnectionListener         (auto) (ansi) 
// Class QuicStreamContext              (auto) (ansi) (sealed) 
// Class QuicTrace                      (auto) (ansi) 
// Class QuicTransportContext           (auto) (ansi) 
// Class <PrivateImplementationDetails> (auto) (ansi) (sealed) 
// Class Libraries                      (abstract) (auto) (ansi) (sealed) (nested assembly) 
// Class TaskAsyncResult                (auto) (ansi) (sealed) (nested assembly) 
// Class DuplexPipePair                 (sequential) (ansi) (sealed) (nested public) 
// Class <WhenAllBlocksReturnedAsync>d__22 (auto) (ansi) (sealed) (nested private) 
// Class ExceptionArgument              (auto) (ansi) (sealed) (nested assembly) 
// Class <CompleteAsyncAwaited>d__2     (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <FastEnumerable>d__66          (auto) (ansi) (sealed) (nested private) 
// Class <FastEnumerable>d__53          (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_0           (auto) (ansi) (sealed) (nested private) 
// Class StreamAsValueType              (sequential) (ansi) (sealed) (nested assembly) 
// Class <ConnectAsync>d__2             (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <AcceptAsync>d__18             (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__15            (auto) (ansi) (sealed) (nested private) 
// Class <AcceptAsync>d__9              (auto) (ansi) (sealed) (nested private) 
// Class <UnbindAsync>d__10             (auto) (ansi) (sealed) (nested private) 
// Class <<DoReceive>g__AwaitCompleteTaskAsync|46_0>d (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__53            (auto) (ansi) (sealed) (nested private) 
// Class <DoReceive>d__46               (auto) (ansi) (sealed) (nested private) 
// Class <DoSend>d__50                  (auto) (ansi) (sealed) (nested private) 
// Class <StartAsync>d__44              (auto) (ansi) (sealed) (nested private) 
// Class <WaitForWritesCompleted>d__45  (auto) (ansi) (sealed) (nested private) 
// Class StreamType                     (auto) (ansi) (sealed) (nested private) 
// Class __StaticArrayInitTypeSize=16   (explicit) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.IO.Pipelines
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Memory
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Diagnostics.StackTrace
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ Microsoft.AspNetCore.Connections.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ Microsoft.Extensions.Features
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000A*/ Microsoft.AspNetCore.Http.Features
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000B*/ Microsoft.AspNetCore.Hosting.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000C*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000D*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000E*/ System.Net.Quic
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000F*/ System.Security.Cryptography.X509Certificates
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000010*/ System.Net.Security
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Server.Kestrel.Transport.Quic
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000004:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000006:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 60 51 75 69 63 2E 46 75 6E 63 74 69 6F   // ...`Quic.Functio
                                                                                                                                                                                    6E 61 6C 54 65 73 74 73 2C 20 50 75 62 6C 69 63   // nalTests, Public
                                                                                                                                                                                    4B 65 79 3D 30 30 32 34 30 30 30 30 30 34 38 30   // Key=002400000480
                                                                                                                                                                                    30 30 30 30 39 34 30 30 30 30 30 30 30 36 30 32   // 0000940000000602
                                                                                                                                                                                    30 30 30 30 30 30 32 34 30 30 30 30 35 32 35 33   // 0000002400005253
                                                                                                                                                                                    34 31 33 31 30 30 30 34 30 30 30 30 30 31 30 30   // 4131000400000100
                                                                                                                                                                                    30 31 30 30 66 33 33 61 32 39 30 34 34 66 61 39   // 0100f33a29044fa9
                                                                                                                                                                                    64 37 34 30 63 39 62 33 32 31 33 61 39 33 65 35   // d740c9b3213a93e5
                                                                                                                                                                                    37 63 38 34 62 34 37 32 63 38 34 65 30 62 38 61   // 7c84b472c84e0b8a
                                                                                                                                                                                    30 65 31 61 65 34 38 65 36 37 61 39 66 38 66 36   // 0e1ae48e67a9f8f6
                                                                                                                                                                                    64 65 39 64 35 66 37 66 33 64 35 32 61 63 32 33   // de9d5f7f3d52ac23
                                                                                                                                                                                    65 34 38 61 63 35 31 38 30 31 66 31 64 63 39 35   // e48ac51801f1dc95
                                                                                                                                                                                    30 61 62 65 39 30 31 64 61 33 34 64 32 61 39 65   // 0abe901da34d2a9e
                                                                                                                                                                                    33 62 61 61 64 62 31 34 31 61 31 37 63 37 37 65   // 3baadb141a17c77e
                                                                                                                                                                                    66 33 63 35 36 35 64 64 35 65 65 35 30 35 34 62   // f3c565dd5ee5054b
                                                                                                                                                                                    39 31 63 66 36 33 62 62 33 63 36 61 62 38 33 66   // 91cf63bb3c6ab83f
                                                                                                                                                                                    37 32 61 62 33 61 61 66 65 39 33 64 30 66 63 33   // 72ab3aafe93d0fc3
                                                                                                                                                                                    63 32 33 34 38 62 37 36 34 66 61 66 62 30 62 31   // c2348b764fafb0b1
                                                                                                                                                                                    63 30 37 33 33 64 65 35 31 34 35 39 61 65 61 62   // c0733de51459aeab
                                                                                                                                                                                    34 36 35 38 30 33 38 34 62 66 39 64 37 34 63 34   // 46580384bf9d74c4
                                                                                                                                                                                    65 32 38 31 36 34 62 37 63 64 65 32 34 37 66 38   // e28164b7cde247f8
                                                                                                                                                                                    39 31 62 61 30 37 38 39 31 63 39 64 38 37 32 61   // 91ba07891c9d872a
                                                                                                                                                                                    64 32 62 62 00 00 )                               // d2bb..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 84 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ....Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E   // pNetCore.Server.
                                                                                                                                                                                    4B 65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72   // Kestrel.Transpor
                                                                                                                                                                                    74 2E 51 75 69 63 2E 54 65 73 74 73 2C 20 50 75   // t.Quic.Tests, Pu
                                                                                                                                                                                    62 6C 69 63 4B 65 79 3D 30 30 32 34 30 30 30 30   // blicKey=00240000
                                                                                                                                                                                    30 34 38 30 30 30 30 30 39 34 30 30 30 30 30 30   // 0480000094000000
                                                                                                                                                                                    30 36 30 32 30 30 30 30 30 30 32 34 30 30 30 30   // 0602000000240000
                                                                                                                                                                                    35 32 35 33 34 31 33 31 30 30 30 34 30 30 30 30   // 5253413100040000
                                                                                                                                                                                    30 31 30 30 30 31 30 30 66 33 33 61 32 39 30 34   // 01000100f33a2904
                                                                                                                                                                                    34 66 61 39 64 37 34 30 63 39 62 33 32 31 33 61   // 4fa9d740c9b3213a
                                                                                                                                                                                    39 33 65 35 37 63 38 34 62 34 37 32 63 38 34 65   // 93e57c84b472c84e
                                                                                                                                                                                    30 62 38 61 30 65 31 61 65 34 38 65 36 37 61 39   // 0b8a0e1ae48e67a9
                                                                                                                                                                                    66 38 66 36 64 65 39 64 35 66 37 66 33 64 35 32   // f8f6de9d5f7f3d52
                                                                                                                                                                                    61 63 32 33 65 34 38 61 63 35 31 38 30 31 66 31   // ac23e48ac51801f1
                                                                                                                                                                                    64 63 39 35 30 61 62 65 39 30 31 64 61 33 34 64   // dc950abe901da34d
                                                                                                                                                                                    32 61 39 65 33 62 61 61 64 62 31 34 31 61 31 37   // 2a9e3baadb141a17
                                                                                                                                                                                    63 37 37 65 66 33 63 35 36 35 64 64 35 65 65 35   // c77ef3c565dd5ee5
                                                                                                                                                                                    30 35 34 62 39 31 63 66 36 33 62 62 33 63 36 61   // 054b91cf63bb3c6a
                                                                                                                                                                                    62 38 33 66 37 32 61 62 33 61 61 66 65 39 33 64   // b83f72ab3aafe93d
                                                                                                                                                                                    30 66 63 33 63 32 33 34 38 62 37 36 34 66 61 66   // 0fc3c2348b764faf
                                                                                                                                                                                    62 30 62 31 63 30 37 33 33 64 65 35 31 34 35 39   // b0b1c0733de51459
                                                                                                                                                                                    61 65 61 62 34 36 35 38 30 33 38 34 62 66 39 64   // aeab46580384bf9d
                                                                                                                                                                                    37 34 63 34 65 32 38 31 36 34 62 37 63 64 65 32   // 74c4e28164b7cde2
                                                                                                                                                                                    34 37 66 38 39 31 62 61 30 37 38 39 31 63 39 64   // 47f891ba07891c9d
                                                                                                                                                                                    38 37 32 61 64 32 62 62 00 00 )                   // 872ad2bb..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 46 51 75 69 63 20 74 72 61 6E 73 70 6F 72   // ..FQuic transpor
                                                                                                                                                                       74 20 66 6F 72 20 74 68 65 20 41 53 50 2E 4E 45   // t for the ASP.NE
                                                                                                                                                                       54 20 43 6F 72 65 20 4B 65 73 74 72 65 6C 20 63   // T Core Kestrel c
                                                                                                                                                                       72 6F 73 73 2D 70 6C 61 74 66 6F 72 6D 20 77 65   // ross-platform we
                                                                                                                                                                       62 20 73 65 72 76 65 72 2E 00 00 )                // b server...
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 32 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..2Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E 4B   // NetCore.Server.K
                                                                                                                                                                 65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72 74   // estrel.Transport
                                                                                                                                                                 2E 51 75 69 63 00 00 )                            // .Quic..
  .custom /*0C000017:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.RequiresPreviewFeaturesAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .permissionset reqmin
             = {[System.Runtime/*23000001*/]System.Security.Permissions.SecurityPermissionAttribute/*01000015*/ = {property bool 'SkipVerification' = bool(true)}}
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
.module Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.dll
// MVID: {1b9e5580-161b-435e-be3b-c70145503af5}
.custom /*0C000002:0A000014*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
.custom /*0C000003:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000018*/
{
  .custom /*0C00001B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000018*/
{
  .custom /*0C00001E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000016*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000019*/) /* 0A000016 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000018*/
{
  .custom /*0C000022:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000016*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000019*/) /* 0A000016 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000018*/
{
  .custom /*0C000026:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .custom /*0C000027:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:0A000016*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*0100001A*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000019*/) /* 0A000016 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Interop
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .class /*02000025*/ abstract auto ansi sealed nested assembly beforefieldinit Libraries
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
  {
    .field /*0400001F*/ static assembly literal string MsQuic = "msquic"
    .custom /*0C000072:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of class Libraries

} // end of class Interop

.class /*02000007*/ private abstract auto ansi sealed beforefieldinit System.Threading.Tasks.TaskToApm
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000026*/ auto ansi sealed nested assembly beforefieldinit TaskAsyncResult
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
         implements [System.Runtime/*23000001*/]System.IAsyncResult/*0100001C*/
  {
    .custom /*0C00008D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncResult/*0100001C*/
    .custom /*0C00008A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000020*/ assembly initonly class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ _task
    .method /*06000109*/ assembly hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ task,
                                 object state,
                                 class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001E*/ callback) cil managed
    // SIG: 20 03 01 12 75 1C 12 79
    {
      .custom /*0C00012D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
      .param [1]/*080000F1*/ 
      .custom /*0C000124:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TaskAsyncResult::.ctor

    .method /*0600010A*/ public hidebysig newslot specialname virtual final 
            instance object  get_AsyncState() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C00012E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
      .custom /*0C00012F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TaskAsyncResult::get_AsyncState

    .method /*0600010B*/ public hidebysig newslot specialname virtual final 
            instance bool  get_CompletedSynchronously() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C000130:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TaskAsyncResult::get_CompletedSynchronously

    .method /*0600010C*/ public hidebysig newslot specialname virtual final 
            instance bool  get_IsCompleted() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TaskAsyncResult::get_IsCompleted

    .method /*0600010D*/ public hidebysig newslot specialname virtual final 
            instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*0100001F*/ 
            get_AsyncWaitHandle() cil managed
    // SIG: 20 00 12 7D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TaskAsyncResult::get_AsyncWaitHandle

    .property /*17000048*/ instance callconv(8) object
            AsyncState()
    {
      .custom /*0C0000B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .get instance object System.Threading.Tasks.TaskToApm/*02000007*//TaskAsyncResult/*02000026*/::get_AsyncState() /* 0600010A */
    } // end of property TaskAsyncResult::AsyncState
    .property /*17000049*/ instance callconv(8) bool
            CompletedSynchronously()
    {
      .get instance bool System.Threading.Tasks.TaskToApm/*02000007*//TaskAsyncResult/*02000026*/::get_CompletedSynchronously() /* 0600010B */
    } // end of property TaskAsyncResult::CompletedSynchronously
    .property /*1700004A*/ instance callconv(8) bool
            IsCompleted()
    {
      .get instance bool System.Threading.Tasks.TaskToApm/*02000007*//TaskAsyncResult/*02000026*/::get_IsCompleted() /* 0600010C */
    } // end of property TaskAsyncResult::IsCompleted
    .property /*1700004B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*0100001F*/
            AsyncWaitHandle()
    {
      .get instance class [System.Runtime/*23000001*/]System.Threading.WaitHandle/*0100001F*/ System.Threading.Tasks.TaskToApm/*02000007*//TaskAsyncResult/*02000026*/::get_AsyncWaitHandle() /* 0600010D */
    } // end of property TaskAsyncResult::AsyncWaitHandle
  } // end of class TaskAsyncResult

  .method /*06000006*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001C*/ 
          Begin(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ task,
                class [System.Runtime/*23000001*/]System.AsyncCallback/*0100001E*/ callback,
                object state) cil managed
  // SIG: 00 03 12 71 12 75 12 79 1C
  {
    .param [2]/*08000002*/ 
    .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000003*/ 
    .custom /*0C000021:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TaskToApm::Begin

  .method /*06000007*/ public hidebysig static 
          void  End(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001C*/ asyncResult) cil managed
  // SIG: 00 01 01 12 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TaskToApm::End

  .method /*06000008*/ public hidebysig static 
          !!TResult  End<TResult>(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100001C*/ asyncResult) cil managed
  // SIG: 10 01 01 1E 00 12 71
  {
    .param type TResult /*2A000001*/ 
      .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TaskToApm::End

} // end of class System.Threading.Tasks.TaskToApm

.class /*02000008*/ private auto ansi beforefieldinit System.IO.Pipelines.DuplexPipe
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
       implements [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/
{
  .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000027*/ sequential ansi sealed nested public beforefieldinit DuplexPipePair
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
  {
    .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000090:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .field /*04000021*/ private initonly class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ '<Transport>k__BackingField'
    .custom /*0C00007E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000022*/ private initonly class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ '<Application>k__BackingField'
    .custom /*0C000082:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .method /*0600010E*/ public hidebysig specialname 
            instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 
            get_Transport() cil managed
    // SIG: 20 00 12 80 81
    {
      .custom /*0C000131:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DuplexPipePair::get_Transport

    .method /*0600010F*/ public hidebysig specialname 
            instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 
            get_Application() cil managed
    // SIG: 20 00 12 80 81
    {
      .custom /*0C000132:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DuplexPipePair::get_Application

    .method /*06000110*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ transport,
                                 class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ application) cil managed
    // SIG: 20 02 01 12 80 81 12 80 81
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DuplexPipePair::.ctor

    .property /*1700004C*/ instance callconv(8) class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/
            Transport()
    {
      .get instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ System.IO.Pipelines.DuplexPipe/*02000008*//DuplexPipePair/*02000027*/::get_Transport() /* 0600010E */
    } // end of property DuplexPipePair::Transport
    .property /*1700004D*/ instance callconv(8) class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/
            Application()
    {
      .get instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ System.IO.Pipelines.DuplexPipe/*02000008*//DuplexPipePair/*02000027*/::get_Application() /* 0600010F */
    } // end of property DuplexPipePair::Application
  } // end of class DuplexPipePair

  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*01000021*/ reader,
                               class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000022*/ writer) cil managed
  // SIG: 20 02 01 12 80 85 12 80 89
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DuplexPipe::.ctor

  .method /*0600000A*/ public hidebysig newslot specialname virtual final 
          instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*01000021*/ 
          get_Input() cil managed
  // SIG: 20 00 12 80 85
  {
    .custom /*0C000037:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DuplexPipe::get_Input

  .method /*0600000B*/ public hidebysig newslot specialname virtual final 
          instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000022*/ 
          get_Output() cil managed
  // SIG: 20 00 12 80 89
  {
    .custom /*0C000039:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DuplexPipe::get_Output

  .method /*0600000C*/ public hidebysig static 
          valuetype System.IO.Pipelines.DuplexPipe/*02000008*//DuplexPipePair/*02000027*/ 
          CreateConnectionPair(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeOptions/*01000023*/ inputOptions,
                               class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeOptions/*01000023*/ outputOptions) cil managed
  // SIG: 00 02 11 80 9C 12 80 8D 12 80 8D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DuplexPipe::CreateConnectionPair

  .property /*17000001*/ instance callconv(8) class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*01000021*/
          Input()
  {
    .get instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*01000021*/ System.IO.Pipelines.DuplexPipe/*02000008*/::get_Input() /* 0600000A */
  } // end of property DuplexPipe::Input
  .property /*17000002*/ instance callconv(8) class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000022*/
          Output()
  {
    .get instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000022*/ System.IO.Pipelines.DuplexPipe/*02000008*/::get_Output() /* 0600000B */
  } // end of property DuplexPipe::Output
} // end of class System.IO.Pipelines.DuplexPipe

.class /*02000009*/ private auto ansi beforefieldinit System.Buffers.DiagnosticMemoryPool
       extends class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8>/*1B000001*/
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000028*/ auto ansi sealed nested private beforefieldinit '<WhenAllBlocksReturnedAsync>d__22'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C000092:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000023*/ public int32 '<>1__state'
    .field /*04000024*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002D*/ '<>t__builder'
    .field /*04000025*/ public class System.Buffers.DiagnosticMemoryPool/*02000009*/ '<>4__this'
    .field /*04000026*/ public valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002B*/ timeout
    .field /*04000027*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002E*/<class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/> '<>u__1'
    .field /*04000028*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__2'
    .method /*06000111*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WhenAllBlocksReturnedAsync>d__22'::MoveNext

    .method /*06000112*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000133:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WhenAllBlocksReturnedAsync>d__22'::SetStateMachine

  } // end of class '<WhenAllBlocksReturnedAsync>d__22'

  .method /*0600000D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> pool,
                               [opt] bool allowLateReturn,
                               [opt] bool rentTracking) cil managed
  // SIG: 20 03 01 15 12 80 99 01 05 02 02
  {
    .param [2]/*0800000B*/  = bool(false)
    .param [3]/*0800000C*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticMemoryPool::.ctor

  .method /*0600000E*/ public hidebysig specialname 
          instance bool  get_IsDisposed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000041:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticMemoryPool::get_IsDisposed

  .method /*0600000F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Buffers.IMemoryOwner`1/*01000027*/<uint8> 
          Rent([opt] int32 size) cil managed
  // SIG: 20 01 15 12 80 9D 01 05 08
  {
    .param [1]/*0800000D*/  = int32(0xFFFFFFFF)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticMemoryPool::Rent

  .method /*06000010*/ public hidebysig specialname virtual 
          instance int32  get_MaxBufferSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticMemoryPool::get_MaxBufferSize

  .method /*06000011*/ assembly hidebysig 
          instance void  Return(class System.Buffers.DiagnosticPoolBlock/*0200000A*/ block) cil managed
  // SIG: 20 01 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticMemoryPool::Return

  .method /*06000012*/ assembly hidebysig 
          instance void  ReportException(class [System.Runtime/*23000001*/]System.Exception/*01000028*/ exception) cil managed
  // SIG: 20 01 01 12 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticMemoryPool::ReportException

  .method /*06000013*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticMemoryPool::Dispose

  .method /*06000014*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          WhenAllBlocksReturnedAsync(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002B*/ timeout) cil managed
  // SIG: 20 01 12 75 11 80 AD
  {
    .custom /*0C00004E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002A*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0A000018 */ = ( 01 00 45 53 79 73 74 65 6D 2E 42 75 66 66 65 72   // ..ESystem.Buffer
                                                                                                                                                                                                                                        73 2E 44 69 61 67 6E 6F 73 74 69 63 4D 65 6D 6F   // s.DiagnosticMemo
                                                                                                                                                                                                                                        72 79 50 6F 6F 6C 2B 3C 57 68 65 6E 41 6C 6C 42   // ryPool+<WhenAllB
                                                                                                                                                                                                                                        6C 6F 63 6B 73 52 65 74 75 72 6E 65 64 41 73 79   // locksReturnedAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 32 32 00 00 )                   // nc>d__22..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticMemoryPool::WhenAllBlocksReturnedAsync

  .property /*17000003*/ instance callconv(8) bool
          IsDisposed()
  {
    .get instance bool System.Buffers.DiagnosticMemoryPool/*02000009*/::get_IsDisposed() /* 0600000E */
  } // end of property DiagnosticMemoryPool::IsDisposed
  .property /*17000004*/ instance callconv(8) int32
          MaxBufferSize()
  {
    .get instance int32 System.Buffers.DiagnosticMemoryPool/*02000009*/::get_MaxBufferSize() /* 06000010 */
  } // end of property DiagnosticMemoryPool::MaxBufferSize
} // end of class System.Buffers.DiagnosticMemoryPool

.class /*0200000A*/ private auto ansi sealed beforefieldinit System.Buffers.DiagnosticPoolBlock
       extends class [System.Runtime/*23000001*/]System.Buffers.MemoryManager`1/*01000031*/<uint8>/*1B000002*/
{
  .method /*06000015*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Buffers.DiagnosticMemoryPool/*02000009*/ pool,
                               class [System.Runtime/*23000001*/]System.Buffers.IMemoryOwner`1/*01000027*/<uint8> memoryOwner) cil managed
  // SIG: 20 02 01 12 24 15 12 80 9D 01 05
  {
    .custom /*0C000052:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::.ctor

  .method /*06000016*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000032*/<uint8> 
          get_Memory() cil managed
  // SIG: 20 00 15 11 80 C9 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::get_Memory

  .method /*06000017*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::Dispose

  .method /*06000018*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*01000033*/<uint8> 
          GetSpan() cil managed
  // SIG: 20 00 15 11 80 CD 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::GetSpan

  .method /*06000019*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Buffers.MemoryHandle/*01000034*/ 
          Pin([opt] int32 byteOffset) cil managed
  // SIG: 20 01 11 80 D1 08
  {
    .param [1]/*08000015*/  = int32(0x00000000)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::Pin

  .method /*0600001A*/ family hidebysig virtual 
          instance bool  TryGetArray([out] valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*01000035*/<uint8>& segment) cil managed
  // SIG: 20 01 02 10 15 11 80 D5 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::TryGetArray

  .method /*0600001B*/ public hidebysig virtual 
          instance void  Unpin() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::Unpin

  .method /*0600001C*/ public hidebysig specialname 
          instance class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackTrace/*01000036*/ 
          get_Leaser() cil managed
  // SIG: 20 00 12 80 D9
  {
    .custom /*0C000068:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000069:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::get_Leaser

  .method /*0600001D*/ public hidebysig specialname 
          instance void  set_Leaser(class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackTrace/*01000036*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D9
  {
    .custom /*0C00006C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00006D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::set_Leaser

  .method /*0600001E*/ public hidebysig instance void 
          Track() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DiagnosticPoolBlock::Track

  .property /*17000005*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000032*/<uint8>
          Memory()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000032*/<uint8> System.Buffers.DiagnosticPoolBlock/*0200000A*/::get_Memory() /* 06000016 */
  } // end of property DiagnosticPoolBlock::Memory
  .property /*17000006*/ instance callconv(8) class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackTrace/*01000036*/
          Leaser()
  {
    .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackTrace/*01000036*/ System.Buffers.DiagnosticPoolBlock/*0200000A*/::get_Leaser() /* 0600001C */
    .set instance void System.Buffers.DiagnosticPoolBlock/*0200000A*/::set_Leaser(class [System.Diagnostics.StackTrace/*23000004*/]System.Diagnostics.StackTrace/*01000036*/) /* 0600001D */
  } // end of property DiagnosticPoolBlock::Leaser
} // end of class System.Buffers.DiagnosticPoolBlock

.class /*0200000B*/ private auto ansi sealed beforefieldinit System.Buffers.MemoryPoolBlock
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
       implements class [System.Runtime/*23000001*/]System.Buffers.IMemoryOwner`1/*01000027*/<uint8>/*1B000003*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000037*/
{
  .method /*0600001F*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Buffers.PinnedBlockMemoryPool/*0200000E*/ pool,
                               int32 length) cil managed
  // SIG: 20 02 01 12 38 08
  {
    .custom /*0C000071:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolBlock::.ctor

  .method /*06000020*/ public hidebysig specialname 
          instance class System.Buffers.PinnedBlockMemoryPool/*0200000E*/ 
          get_Pool() cil managed
  // SIG: 20 00 12 38
  {
    .custom /*0C000077:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000078:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolBlock::get_Pool

  .method /*06000021*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000032*/<uint8> 
          get_Memory() cil managed
  // SIG: 20 00 15 11 80 C9 01 05
  {
    .custom /*0C00007D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolBlock::get_Memory

  .method /*06000022*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolBlock::Dispose

  .property /*17000007*/ instance callconv(8) class System.Buffers.PinnedBlockMemoryPool/*0200000E*/
          Pool()
  {
    .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class System.Buffers.PinnedBlockMemoryPool/*0200000E*/ System.Buffers.MemoryPoolBlock/*0200000B*/::get_Pool() /* 06000020 */
  } // end of property MemoryPoolBlock::Pool
  .property /*17000008*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000032*/<uint8>
          Memory()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000032*/<uint8> System.Buffers.MemoryPoolBlock/*0200000B*/::get_Memory() /* 06000021 */
  } // end of property MemoryPoolBlock::Memory
} // end of class System.Buffers.MemoryPoolBlock

.class /*0200000C*/ private abstract auto ansi sealed beforefieldinit System.Buffers.PinnedBlockMemoryPoolFactory
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000023*/ public hidebysig static 
          class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> 
          Create() cil managed
  // SIG: 00 00 15 12 80 99 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PinnedBlockMemoryPoolFactory::Create

  .method /*06000024*/ public hidebysig static 
          class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> 
          CreatePinnedBlockMemoryPool() cil managed
  // SIG: 00 00 15 12 80 99 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PinnedBlockMemoryPoolFactory::CreatePinnedBlockMemoryPool

} // end of class System.Buffers.PinnedBlockMemoryPoolFactory

.class /*0200000D*/ private auto ansi beforefieldinit System.Buffers.MemoryPoolThrowHelper
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000029*/ auto ansi sealed nested assembly ExceptionArgument
         extends [System.Runtime/*23000001*/]System.Enum/*01000038*/
  {
    .custom /*0C000094:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .field /*04000029*/ public specialname rtspecialname int32 value__
    .field /*0400002A*/ public static literal valuetype System.Buffers.MemoryPoolThrowHelper/*0200000D*//ExceptionArgument/*02000029*/ size = int32(0x00000000)
    .field /*0400002B*/ public static literal valuetype System.Buffers.MemoryPoolThrowHelper/*0200000D*//ExceptionArgument/*02000029*/ offset = int32(0x00000001)
    .field /*0400002C*/ public static literal valuetype System.Buffers.MemoryPoolThrowHelper/*0200000D*//ExceptionArgument/*02000029*/ length = int32(0x00000002)
    .field /*0400002D*/ public static literal valuetype System.Buffers.MemoryPoolThrowHelper/*0200000D*//ExceptionArgument/*02000029*/ MemoryPoolBlock = int32(0x00000003)
    .field /*0400002E*/ public static literal valuetype System.Buffers.MemoryPoolThrowHelper/*0200000D*//ExceptionArgument/*02000029*/ MemoryPool = int32(0x00000004)
  } // end of class ExceptionArgument

  .method /*06000025*/ public hidebysig static 
          void  ThrowArgumentOutOfRangeException(int32 sourceLength,
                                                 int32 offset) cil managed
  // SIG: 00 02 01 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowArgumentOutOfRangeException

  .method /*06000026*/ public hidebysig static 
          void  ThrowInvalidOperationException_PinCountZero(class System.Buffers.DiagnosticPoolBlock/*0200000A*/ block) cil managed
  // SIG: 00 01 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowInvalidOperationException_PinCountZero

  .method /*06000027*/ public hidebysig static 
          void  ThrowInvalidOperationException_ReturningPinnedBlock(class System.Buffers.DiagnosticPoolBlock/*0200000A*/ block) cil managed
  // SIG: 00 01 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowInvalidOperationException_ReturningPinnedBlock

  .method /*06000028*/ public hidebysig static 
          void  ThrowInvalidOperationException_DoubleDispose() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowInvalidOperationException_DoubleDispose

  .method /*06000029*/ public hidebysig static 
          void  ThrowInvalidOperationException_BlockDoubleDispose(class System.Buffers.DiagnosticPoolBlock/*0200000A*/ block) cil managed
  // SIG: 00 01 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowInvalidOperationException_BlockDoubleDispose

  .method /*0600002A*/ public hidebysig static 
          void  ThrowInvalidOperationException_BlockReturnedToDisposedPool(class System.Buffers.DiagnosticPoolBlock/*0200000A*/ block) cil managed
  // SIG: 00 01 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowInvalidOperationException_BlockReturnedToDisposedPool

  .method /*0600002B*/ public hidebysig static 
          void  ThrowInvalidOperationException_BlockIsBackedByDisposedSlab(class System.Buffers.DiagnosticPoolBlock/*0200000A*/ block) cil managed
  // SIG: 00 01 01 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowInvalidOperationException_BlockIsBackedByDisposedSlab

  .method /*0600002C*/ public hidebysig static 
          void  ThrowInvalidOperationException_DisposingPoolWithActiveBlocks(int32 returned,
                                                                             int32 total,
                                                                             class System.Buffers.DiagnosticPoolBlock/*0200000A*/[] blocks) cil managed
  // SIG: 00 03 01 08 08 1D 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowInvalidOperationException_DisposingPoolWithActiveBlocks

  .method /*0600002D*/ public hidebysig static 
          void  ThrowInvalidOperationException_BlocksWereNotReturnedInTime(int32 returned,
                                                                           int32 total,
                                                                           class System.Buffers.DiagnosticPoolBlock/*0200000A*/[] blocks) cil managed
  // SIG: 00 03 01 08 08 1D 12 28
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowInvalidOperationException_BlocksWereNotReturnedInTime

  .method /*0600002E*/ public hidebysig static 
          void  ThrowArgumentOutOfRangeException_BufferRequestTooLarge(int32 maxSize) cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowArgumentOutOfRangeException_BufferRequestTooLarge

  .method /*0600002F*/ public hidebysig static 
          void  ThrowObjectDisposedException(valuetype System.Buffers.MemoryPoolThrowHelper/*0200000D*//ExceptionArgument/*02000029*/ argument) cil managed
  // SIG: 00 01 01 11 80 A4
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::ThrowObjectDisposedException

  .method /*06000030*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryPoolThrowHelper::.ctor

} // end of class System.Buffers.MemoryPoolThrowHelper

.class /*0200000E*/ private auto ansi sealed beforefieldinit System.Buffers.PinnedBlockMemoryPool
       extends class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8>/*1B000001*/
{
  .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000031*/ public hidebysig specialname virtual 
          instance int32  get_MaxBufferSize() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00009F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PinnedBlockMemoryPool::get_MaxBufferSize

  .method /*06000032*/ public hidebysig specialname static 
          int32  get_BlockSize() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PinnedBlockMemoryPool::get_BlockSize

  .method /*06000033*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Buffers.IMemoryOwner`1/*01000027*/<uint8> 
          Rent([opt] int32 size) cil managed
  // SIG: 20 01 15 12 80 9D 01 05 08
  {
    .param [1]/*08000029*/  = int32(0xFFFFFFFF)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PinnedBlockMemoryPool::Rent

  .method /*06000034*/ assembly hidebysig 
          instance void  Return(class System.Buffers.MemoryPoolBlock/*0200000B*/ block) cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PinnedBlockMemoryPool::Return

  .method /*06000035*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PinnedBlockMemoryPool::Dispose

  .method /*06000036*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PinnedBlockMemoryPool::.ctor

  .property /*17000009*/ instance callconv(8) int32
          MaxBufferSize()
  {
    .get instance int32 System.Buffers.PinnedBlockMemoryPool/*0200000E*/::get_MaxBufferSize() /* 06000031 */
  } // end of property PinnedBlockMemoryPool::MaxBufferSize
  .property /*1700000A*/ callconv(8) int32
          BlockSize()
  {
    .get int32 System.Buffers.PinnedBlockMemoryPool/*0200000E*/::get_BlockSize() /* 06000032 */
  } // end of property PinnedBlockMemoryPool::BlockSize
} // end of class System.Buffers.PinnedBlockMemoryPool

.class /*0200000F*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Connections.CompletionPipeReader
       extends [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*01000021*/
{
  .custom /*0C000045:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000037*/ public hidebysig specialname 
          instance bool  get_IsCompleted() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000A6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::get_IsCompleted

  .method /*06000038*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*01000028*/ 
          get_CompleteException() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C0000A8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::get_CompleteException

  .method /*06000039*/ public hidebysig specialname 
          instance bool  get_IsCompletedSuccessfully() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::get_IsCompletedSuccessfully

  .method /*0600003A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*01000021*/ inner) cil managed
  // SIG: 20 01 01 12 80 85
  {
    .custom /*0C0000AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::.ctor

  .method /*0600003B*/ public hidebysig virtual 
          instance void  AdvanceTo(valuetype [System.Memory/*23000003*/]System.SequencePosition/*01000039*/ consumed) cil managed
  // SIG: 20 01 01 11 80 E5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::AdvanceTo

  .method /*0600003C*/ public hidebysig virtual 
          instance void  AdvanceTo(valuetype [System.Memory/*23000003*/]System.SequencePosition/*01000039*/ consumed,
                                   valuetype [System.Memory/*23000003*/]System.SequencePosition/*01000039*/ examined) cil managed
  // SIG: 20 02 01 11 80 E5 11 80 E5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::AdvanceTo

  .method /*0600003D*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003A*/ 
          CompleteAsync([opt] class [System.Runtime/*23000001*/]System.Exception/*01000028*/ exception) cil managed
  // SIG: 20 01 11 80 E9 12 80 A1
  {
    .param [1]/*08000030*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::CompleteAsync

  .method /*0600003E*/ public hidebysig virtual 
          instance void  Complete([opt] class [System.Runtime/*23000001*/]System.Exception/*01000028*/ exception) cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .param [1]/*08000031*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::Complete

  .method /*0600003F*/ public hidebysig virtual 
          instance void  CancelPendingRead() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::CancelPendingRead

  .method /*06000040*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.ReadResult/*0100003C*/> 
          ReadAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 01 15 11 80 ED 01 11 80 F1 11 80 F5
  {
    .custom /*0C0000B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [1]/*08000032*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::ReadAsync

  .method /*06000041*/ public hidebysig virtual 
          instance bool  TryRead([out] valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.ReadResult/*0100003C*/& result) cil managed
  // SIG: 20 01 02 10 11 80 F1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::TryRead

  .method /*06000042*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeReader::Reset

  .property /*1700000B*/ instance callconv(8) bool
          IsCompleted()
  {
    .get instance bool Microsoft.AspNetCore.Connections.CompletionPipeReader/*0200000F*/::get_IsCompleted() /* 06000037 */
  } // end of property CompletionPipeReader::IsCompleted
  .property /*1700000C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*01000028*/
          CompleteException()
  {
    .get instance class [System.Runtime/*23000001*/]System.Exception/*01000028*/ Microsoft.AspNetCore.Connections.CompletionPipeReader/*0200000F*/::get_CompleteException() /* 06000038 */
  } // end of property CompletionPipeReader::CompleteException
  .property /*1700000D*/ instance callconv(8) bool
          IsCompletedSuccessfully()
  {
    .get instance bool Microsoft.AspNetCore.Connections.CompletionPipeReader/*0200000F*/::get_IsCompletedSuccessfully() /* 06000039 */
  } // end of property CompletionPipeReader::IsCompletedSuccessfully
} // end of class Microsoft.AspNetCore.Connections.CompletionPipeReader

.class /*02000010*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Connections.CompletionPipeWriter
       extends [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000022*/
{
  .method /*06000043*/ public hidebysig specialname 
          instance bool  get_IsCompleted() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000B2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::get_IsCompleted

  .method /*06000044*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*01000028*/ 
          get_CompleteException() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000B4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::get_CompleteException

  .method /*06000045*/ public hidebysig specialname 
          instance bool  get_IsCompletedSuccessfully() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::get_IsCompletedSuccessfully

  .method /*06000046*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000022*/ inner) cil managed
  // SIG: 20 01 01 12 80 89
  {
    .custom /*0C0000B7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::.ctor

  .method /*06000047*/ public hidebysig virtual 
          instance void  Advance(int32 bytes) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::Advance

  .method /*06000048*/ public hidebysig virtual 
          instance void  CancelPendingFlush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::CancelPendingFlush

  .method /*06000049*/ public hidebysig virtual 
          instance void  Complete([opt] class [System.Runtime/*23000001*/]System.Exception/*01000028*/ exception) cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .custom /*0C0000BA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000036*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::Complete

  .method /*0600004A*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003A*/ 
          CompleteAsync([opt] class [System.Runtime/*23000001*/]System.Exception/*01000028*/ exception) cil managed
  // SIG: 20 01 11 80 E9 12 80 A1
  {
    .custom /*0C0000BB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000037*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::CompleteAsync

  .method /*0600004B*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.FlushResult/*0100003E*/> 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*0100003F*/<uint8> source,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 ED 01 11 80 F9 15 11 80 FD 01 05 11 80 F5
  {
    .param [2]/*08000039*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::WriteAsync

  .method /*0600004C*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.FlushResult/*0100003E*/> 
          FlushAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 01 15 11 80 ED 01 11 80 F9 11 80 F5
  {
    .param [1]/*0800003A*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::FlushAsync

  .method /*0600004D*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000032*/<uint8> 
          GetMemory([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 C9 01 05 08
  {
    .param [1]/*0800003B*/  = int32(0x00000000)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::GetMemory

  .method /*0600004E*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*01000033*/<uint8> 
          GetSpan([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 80 CD 01 05 08
  {
    .param [1]/*0800003C*/  = int32(0x00000000)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::GetSpan

  .method /*0600004F*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CompletionPipeWriter::Reset

  .property /*1700000E*/ instance callconv(8) bool
          IsCompleted()
  {
    .get instance bool Microsoft.AspNetCore.Connections.CompletionPipeWriter/*02000010*/::get_IsCompleted() /* 06000043 */
  } // end of property CompletionPipeWriter::IsCompleted
  .property /*1700000F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*01000028*/
          CompleteException()
  {
    .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Exception/*01000028*/ Microsoft.AspNetCore.Connections.CompletionPipeWriter/*02000010*/::get_CompleteException() /* 06000044 */
  } // end of property CompletionPipeWriter::CompleteException
  .property /*17000010*/ instance callconv(8) bool
          IsCompletedSuccessfully()
  {
    .get instance bool Microsoft.AspNetCore.Connections.CompletionPipeWriter/*02000010*/::get_IsCompletedSuccessfully() /* 06000045 */
  } // end of property CompletionPipeWriter::IsCompletedSuccessfully
} // end of class Microsoft.AspNetCore.Connections.CompletionPipeWriter

.class /*02000011*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Connections.ConnectionCompletion
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .class /*0200002A*/ auto ansi sealed nested private beforefieldinit '<CompleteAsyncAwaited>d__2'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C000095:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400002F*/ public int32 '<>1__state'
    .field /*04000030*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002D*/ '<>t__builder'
    .field /*04000031*/ public class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ currentTask
    .field /*04000032*/ public class [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILogger/*01000040*/ logger
    .field /*04000033*/ public class [System.Collections/*23000006*/]System.Collections.Generic.Stack`1/*01000041*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Func`2/*01000043*/<object,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/>,object>> onCompleted
    .field /*04000034*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .method /*06000113*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CompleteAsyncAwaited>d__2'::MoveNext

    .method /*06000114*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000134:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CompleteAsyncAwaited>d__2'::SetStateMachine

  } // end of class '<CompleteAsyncAwaited>d__2'

  .method /*06000050*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          FireOnCompletedAsync(class [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILogger/*01000040*/ logger,
                               class [System.Collections/*23000006*/]System.Collections.Generic.Stack`1/*01000041*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Func`2/*01000043*/<object,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/>,object>> onCompleted) cil managed
  // SIG: 00 02 12 75 12 81 01 15 12 81 05 01 15 11 81 09 02 15 12 81 0D 02 1C 12 75 1C
  {
    .custom /*0C0000BC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800003E*/ 
    .custom /*0C0000B0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 06 00 00 00 02 00 01 01 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionCompletion::FireOnCompletedAsync

} // end of class Microsoft.AspNetCore.Connections.ConnectionCompletion

.class /*02000012*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Connections.CorrelationIdGenerator
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .class /*0200002B*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
  {
    .custom /*0C000096:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000035*/ public static initonly class Microsoft.AspNetCore.Connections.CorrelationIdGenerator/*02000012*//'<>c'/*0200002B*/ '<>9'
    .field /*04000036*/ public static class [System.Runtime/*23000001*/]System.Buffers.SpanAction`2/*01000044*/<char,int64> '<>9__3_0'
    .method /*06000115*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000116*/ assembly hidebysig 
            instance void  '<GenerateId>b__3_0'(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000033*/<char> buffer,
                                                int64 'value') cil managed
    // SIG: 20 02 01 15 11 80 CD 01 03 0A
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GenerateId>b__3_0'

  } // end of class '<>c'

  .method /*06000051*/ public hidebysig static 
          string  GetNextId() cil managed
  // SIG: 00 00 0E
  {
    .custom /*0C0000BD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CorrelationIdGenerator::GetNextId

} // end of class Microsoft.AspNetCore.Connections.CorrelationIdGenerator

.class /*02000013*/ private abstract auto ansi beforefieldinit Microsoft.AspNetCore.Connections.TransportConnection
       extends [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000045*/
       implements [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*01000050*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0100004C*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0100004B*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/
{
  .custom /*0C00004C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/
  .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/
  .custom /*0C000029:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*01000050*/
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0100004C*/
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0100004B*/
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002C*/ auto ansi sealed nested private beforefieldinit '<FastEnumerable>d__66'
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000005*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000037*/
  {
    .custom /*0C000097:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000037*/ public class Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/ '<>4__this'
    .method /*06000117*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C000135:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__66'::.ctor

    .method /*06000118*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000136:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000037*/::Dispose /*01000037::0A00002C*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__66'::System.IDisposable.Dispose

    .method /*06000119*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/::MoveNext /*0100004A::0A00002D*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__66'::MoveNext

    .method /*0600011A*/ private hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object> 
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.get_Current'() cil managed
    // SIG: 20 00 15 11 81 09 02 12 80 A5 1C
    {
      .custom /*0C000137:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000005*/::get_Current() /* 0A00002E */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__66'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.get_Current'

    .method /*0600011B*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C000138:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/::Reset /*0100004A::0A00002F*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__66'::System.Collections.IEnumerator.Reset

    .method /*0600011C*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C000139:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/::get_Current /*0100004A::0A000030*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__66'::System.Collections.IEnumerator.get_Current

    .method /*0600011D*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>> 
            'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 81 21 01 15 11 81 09 02 12 80 A5 1C
    {
      .custom /*0C00013A:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/::GetEnumerator() /* 0A000022 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__66'::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.GetEnumerator'

    .method /*0600011E*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 81 29
    {
      .custom /*0C00013B:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/::GetEnumerator /*01000049::0A000023*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__66'::System.Collections.IEnumerable.GetEnumerator

    .property /*1700004E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.Current'()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object> Microsoft.AspNetCore.Connections.TransportConnection/*02000013*//'<FastEnumerable>d__66'/*0200002C*/::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.get_Current'() /* 0600011A */
    } // end of property '<FastEnumerable>d__66'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.Current'
    .property /*1700004F*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.Connections.TransportConnection/*02000013*//'<FastEnumerable>d__66'/*0200002C*/::System.Collections.IEnumerator.get_Current() /* 0600011C */
    } // end of property '<FastEnumerable>d__66'::System.Collections.IEnumerator.Current
  } // end of class '<FastEnumerable>d__66'

  .field /*04000004*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*01000050*/ _currentIConnectionIdFeature
  .field /*04000005*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0100004C*/ _currentIConnectionTransportFeature
  .field /*04000006*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/ _currentIConnectionItemsFeature
  .field /*04000007*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0100004B*/ _currentIMemoryPoolFeature
  .field /*04000008*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/ _currentIConnectionLifetimeFeature
  .field /*04000009*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature/*01000051*/ _currentIPersistentStateFeature
  .field /*0400000A*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionSocketFeature/*01000052*/ _currentIConnectionSocketFeature
  .field /*0400000B*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature/*01000053*/ _currentIProtocolErrorCodeFeature
  .field /*0400000C*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamDirectionFeature/*01000054*/ _currentIStreamDirectionFeature
  .field /*0400000D*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamIdFeature/*01000055*/ _currentIStreamIdFeature
  .field /*0400000E*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamAbortFeature/*01000056*/ _currentIStreamAbortFeature
  .method /*06000052*/ public hidebysig specialname 
          instance class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ 
          get_MultiplexedConnectionFeatures() cil managed
  // SIG: 20 00 12 81 19
  {
    .custom /*0C0000BE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_MultiplexedConnectionFeatures

  .method /*06000053*/ family hidebysig specialname 
          instance void  set_MultiplexedConnectionFeatures(class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ 'value') cil managed
  // SIG: 20 01 01 12 81 19
  {
    .custom /*0C0000BF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::set_MultiplexedConnectionFeatures

  .method /*06000054*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::.ctor

  .method /*06000055*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 
          get_LocalEndPoint() cil managed
  // SIG: 20 00 12 81 5D
  {
    .custom /*0C0000C1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_LocalEndPoint

  .method /*06000056*/ public hidebysig specialname virtual 
          instance void  set_LocalEndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 'value') cil managed
  // SIG: 20 01 01 12 81 5D
  {
    .custom /*0C0000C3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::set_LocalEndPoint

  .method /*06000057*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 
          get_RemoteEndPoint() cil managed
  // SIG: 20 00 12 81 5D
  {
    .custom /*0C0000C4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_RemoteEndPoint

  .method /*06000058*/ public hidebysig specialname virtual 
          instance void  set_RemoteEndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 'value') cil managed
  // SIG: 20 01 01 12 81 5D
  {
    .custom /*0C0000C5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::set_RemoteEndPoint

  .method /*06000059*/ public hidebysig specialname virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_ConnectionId

  .method /*0600005A*/ public hidebysig specialname virtual 
          instance void  set_ConnectionId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::set_ConnectionId

  .method /*0600005B*/ public hidebysig specialname virtual 
          instance class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ 
          get_Features() cil managed
  // SIG: 20 00 12 81 19
  {
    .custom /*0C0000C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_Features

  .method /*0600005C*/ public hidebysig newslot specialname virtual 
          instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> 
          get_MemoryPool() cil managed
  // SIG: 20 00 15 12 80 99 01 05
  {
    .custom /*0C0000C9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000CA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_MemoryPool

  .method /*0600005D*/ public hidebysig specialname virtual 
          instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 
          get_Transport() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C0000CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000CC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_Transport

  .method /*0600005E*/ public hidebysig specialname virtual 
          instance void  set_Transport(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 'value') cil managed
  // SIG: 20 01 01 12 80 81
  {
    .custom /*0C0000CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000CE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::set_Transport

  .method /*0600005F*/ public hidebysig specialname 
          instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 
          get_Application() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_Application

  .method /*06000060*/ public hidebysig specialname 
          instance void  set_Application(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 'value') cil managed
  // SIG: 20 01 01 12 80 81
  {
    .custom /*0C0000D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::set_Application

  .method /*06000061*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 81 39 02 1C 1C
  {
    .param [0]/*08000045*/ 
    .custom /*0C0000B5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_Items

  .method /*06000062*/ public hidebysig specialname virtual 
          instance void  set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 'value') cil managed
  // SIG: 20 01 01 15 12 81 39 02 1C 1C
  {
    .param [1]/*08000046*/ 
    .custom /*0C0000B8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::set_Items

  .method /*06000063*/ assembly hidebysig 
          instance void  ResetItems() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::ResetItems

  .method /*06000064*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ 
          get_ConnectionClosed() cil managed
  // SIG: 20 00 11 80 F5
  {
    .custom /*0C0000D5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::get_ConnectionClosed

  .method /*06000065*/ public hidebysig specialname virtual 
          instance void  set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 F5
  {
    .custom /*0C0000D6:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::set_ConnectionClosed

  .method /*06000066*/ public hidebysig virtual 
          instance void  Abort(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionAbortedException/*01000058*/ abortReason) cil managed
  // SIG: 20 01 01 12 81 61
  {
    .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Abort

  .method /*06000067*/ assembly hidebysig 
          instance void  ResetFeatureCollection() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::ResetFeatureCollection

  .method /*06000068*/ private hidebysig newslot specialname virtual final 
          instance bool  Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::get_IsReadOnly /*01000046::0A00001C*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_IsReadOnly

  .method /*06000069*/ private hidebysig newslot specialname virtual final 
          instance int32  Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Revision() cil managed
  // SIG: 20 00 08
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::get_Revision /*01000046::0A00001D*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Revision

  .method /*0600006A*/ private hidebysig newslot specialname virtual final 
          instance object  Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/ key) cil managed
  // SIG: 20 01 1C 12 80 A5
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::get_Item /*01000046::0A00001E*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Item

  .method /*0600006B*/ private hidebysig newslot specialname virtual final 
          instance void  Microsoft.AspNetCore.Http.Features.IFeatureCollection.set_Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/ key,
                                                                                        object 'value') cil managed
  // SIG: 20 02 01 12 80 A5 1C
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::set_Item /*01000046::0A00001F*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.set_Item

  .method /*0600006C*/ private hidebysig newslot virtual final 
          instance !!TFeature  Microsoft.AspNetCore.Http.Features.IFeatureCollection.Get<TFeature>() cil managed
  // SIG: 30 01 00 1E 00
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::Get /*01000046::0A000020*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.Get

  .method /*0600006D*/ private hidebysig newslot virtual final 
          instance void  Microsoft.AspNetCore.Http.Features.IFeatureCollection.Set<TFeature>(!!TFeature feature) cil managed
  // SIG: 30 01 01 01 1E 00
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::Set /*01000046::0A000021*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.Set

  .method /*0600006E*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 81 21 01 15 11 81 09 02 12 80 A5 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/::GetEnumerator() /* 0A000022 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.GetEnumerator'

  .method /*0600006F*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 29
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/::GetEnumerator /*01000049::0A000023*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::System.Collections.IEnumerable.GetEnumerator

  .method /*06000070*/ private hidebysig newslot specialname virtual final 
          instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> 
          Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.get_MemoryPool() cil managed
  // SIG: 20 00 15 12 80 99 01 05
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0100004B*/::get_MemoryPool /*0100004B::0A000024*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.get_MemoryPool

  .method /*06000071*/ private hidebysig newslot specialname virtual final 
          instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 
          Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature.get_Transport() cil managed
  // SIG: 20 00 12 80 81
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0100004C*/::get_Transport /*0100004C::0A000025*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature.get_Transport

  .method /*06000072*/ private hidebysig newslot specialname virtual final 
          instance void  Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature.set_Transport(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 'value') cil managed
  // SIG: 20 01 01 12 80 81
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0100004C*/::set_Transport /*0100004C::0A000026*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature.set_Transport

  .method /*06000073*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 
          Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.get_Items() cil managed
  // SIG: 20 00 15 12 81 39 02 1C 1C
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/::get_Items /*0100004D::0A000027*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.get_Items

  .method /*06000074*/ private hidebysig newslot specialname virtual final 
          instance void  Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 'value') cil managed
  // SIG: 20 01 01 15 12 81 39 02 1C 1C
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/::set_Items /*0100004D::0A000028*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.set_Items

  .method /*06000075*/ private hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ 
          Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.get_ConnectionClosed() cil managed
  // SIG: 20 00 11 80 F5
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/::get_ConnectionClosed /*0100004F::0A000029*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.get_ConnectionClosed

  .method /*06000076*/ private hidebysig newslot specialname virtual final 
          instance void  Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 F5
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/::set_ConnectionClosed /*0100004F::0A00002A*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.set_ConnectionClosed

  .method /*06000077*/ private hidebysig newslot virtual final 
          instance void  Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.Abort() cil managed
  // SIG: 20 00 01
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/::Abort /*0100004F::0A00002B*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.Abort

  .property /*17000011*/ instance callconv(8) class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/
          MultiplexedConnectionFeatures()
  {
    .get instance class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_MultiplexedConnectionFeatures() /* 06000052 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::set_MultiplexedConnectionFeatures(class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/) /* 06000053 */
  } // end of property TransportConnection::MultiplexedConnectionFeatures
  .property /*17000012*/ instance callconv(8) class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/
          LocalEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_LocalEndPoint() /* 06000055 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::set_LocalEndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/) /* 06000056 */
  } // end of property TransportConnection::LocalEndPoint
  .property /*17000013*/ instance callconv(8) class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/
          RemoteEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_RemoteEndPoint() /* 06000057 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::set_RemoteEndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/) /* 06000058 */
  } // end of property TransportConnection::RemoteEndPoint
  .property /*17000014*/ instance callconv(8) string
          ConnectionId()
  {
    .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_ConnectionId() /* 06000059 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::set_ConnectionId(string) /* 0600005A */
  } // end of property TransportConnection::ConnectionId
  .property /*17000015*/ instance callconv(8) class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/
          Features()
  {
    .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_Features() /* 0600005B */
  } // end of property TransportConnection::Features
  .property /*17000016*/ instance callconv(8) class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8>
          MemoryPool()
  {
    .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_MemoryPool() /* 0600005C */
  } // end of property TransportConnection::MemoryPool
  .property /*17000017*/ instance callconv(8) class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/
          Transport()
  {
    .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_Transport() /* 0600005D */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::set_Transport(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/) /* 0600005E */
  } // end of property TransportConnection::Transport
  .property /*17000018*/ instance callconv(8) class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/
          Application()
  {
    .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_Application() /* 0600005F */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::set_Application(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/) /* 06000060 */
  } // end of property TransportConnection::Application
  .property /*17000019*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>
          Items()
  {
    .custom /*0C000064:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_Items() /* 06000061 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>) /* 06000062 */
  } // end of property TransportConnection::Items
  .property /*1700001A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/
          ConnectionClosed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::get_ConnectionClosed() /* 06000064 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/) /* 06000065 */
  } // end of property TransportConnection::ConnectionClosed
  .property /*1700001B*/ instance callconv(8) bool
          Microsoft.AspNetCore.Http.Features.IFeatureCollection.IsReadOnly()
  {
    .get instance bool Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_IsReadOnly() /* 06000068 */
  } // end of property TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.IsReadOnly
  .property /*1700001C*/ instance callconv(8) int32
          Microsoft.AspNetCore.Http.Features.IFeatureCollection.Revision()
  {
    .get instance int32 Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Revision() /* 06000069 */
  } // end of property TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.Revision
  .property /*1700001D*/ instance callconv(8) object
          Microsoft.AspNetCore.Http.Features.IFeatureCollection.Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/)
  {
    .get instance object Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0600006A */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Http.Features.IFeatureCollection.set_Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/,
                                                                                                                                                        object) /* 0600006B */
  } // end of property TransportConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.Item
  .property /*1700001E*/ instance callconv(8) class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8>
          Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.MemoryPool()
  {
    .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.get_MemoryPool() /* 06000070 */
  } // end of property TransportConnection::Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.MemoryPool
  .property /*1700001F*/ instance callconv(8) class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/
          Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature.Transport()
  {
    .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature.get_Transport() /* 06000071 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature.set_Transport(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/) /* 06000072 */
  } // end of property TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature.Transport
  .property /*17000020*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>
          Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.Items()
  {
    .custom /*0C00007C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.get_Items() /* 06000073 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>) /* 06000074 */
  } // end of property TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.Items
  .property /*17000021*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/
          Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.ConnectionClosed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.get_ConnectionClosed() /* 06000075 */
    .set instance void Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/) /* 06000076 */
  } // end of property TransportConnection::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.ConnectionClosed
} // end of class Microsoft.AspNetCore.Connections.TransportConnection

.class /*02000014*/ private abstract auto ansi beforefieldinit Microsoft.AspNetCore.Connections.TransportMultiplexedConnection
       extends [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*01000059*/
       implements [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*01000050*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0100004B*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/
{
  .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/
  .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/
  .custom /*0C000040:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/
  .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*01000050*/
  .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/
  .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0100004B*/
  .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/
  .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002D*/ auto ansi sealed nested private beforefieldinit '<FastEnumerable>d__53'
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000005*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000037*/
  {
    .custom /*0C000098:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000038*/ public class Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/ '<>4__this'
    .method /*0600011F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C00013C:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__53'::.ctor

    .method /*06000120*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00013D:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000037*/::Dispose /*01000037::0A00002C*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__53'::System.IDisposable.Dispose

    .method /*06000121*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/::MoveNext /*0100004A::0A00002D*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__53'::MoveNext

    .method /*06000122*/ private hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object> 
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.get_Current'() cil managed
    // SIG: 20 00 15 11 81 09 02 12 80 A5 1C
    {
      .custom /*0C00013E:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000005*/::get_Current() /* 0A00002E */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__53'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.get_Current'

    .method /*06000123*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00013F:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/::Reset /*0100004A::0A00002F*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__53'::System.Collections.IEnumerator.Reset

    .method /*06000124*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C000140:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/::get_Current /*0100004A::0A000030*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__53'::System.Collections.IEnumerator.get_Current

    .method /*06000125*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>> 
            'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 81 21 01 15 11 81 09 02 12 80 A5 1C
    {
      .custom /*0C000141:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/::GetEnumerator() /* 0A000022 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__53'::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.GetEnumerator'

    .method /*06000126*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 81 29
    {
      .custom /*0C000142:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/::GetEnumerator /*01000049::0A000023*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FastEnumerable>d__53'::System.Collections.IEnumerable.GetEnumerator

    .property /*17000050*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.Current'()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object> Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*//'<FastEnumerable>d__53'/*0200002D*/::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.get_Current'() /* 06000122 */
    } // end of property '<FastEnumerable>d__53'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.Current'
    .property /*17000051*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*//'<FastEnumerable>d__53'/*0200002D*/::System.Collections.IEnumerator.get_Current() /* 06000124 */
    } // end of property '<FastEnumerable>d__53'::System.Collections.IEnumerator.Current
  } // end of class '<FastEnumerable>d__53'

  .field /*0400000F*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*01000050*/ _currentIConnectionIdFeature
  .field /*04000010*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/ _currentIConnectionItemsFeature
  .field /*04000011*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0100004B*/ _currentIMemoryPoolFeature
  .field /*04000012*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/ _currentIConnectionLifetimeFeature
  .field /*04000013*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0100004C*/ _currentIConnectionTransportFeature
  .field /*04000014*/ famorassem class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature/*01000053*/ _currentIProtocolErrorCodeFeature
  .field /*04000015*/ famorassem class [Microsoft.AspNetCore.Http.Features/*2300000A*/]Microsoft.AspNetCore.Http.Features.ITlsConnectionFeature/*0100005A*/ _currentITlsConnectionFeature
  .method /*06000078*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::.ctor

  .method /*06000079*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 
          get_LocalEndPoint() cil managed
  // SIG: 20 00 12 81 5D
  {
    .custom /*0C0000DC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::get_LocalEndPoint

  .method /*0600007A*/ public hidebysig specialname virtual 
          instance void  set_LocalEndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 'value') cil managed
  // SIG: 20 01 01 12 81 5D
  {
    .custom /*0C0000DE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::set_LocalEndPoint

  .method /*0600007B*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 
          get_RemoteEndPoint() cil managed
  // SIG: 20 00 12 81 5D
  {
    .custom /*0C0000DF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::get_RemoteEndPoint

  .method /*0600007C*/ public hidebysig specialname virtual 
          instance void  set_RemoteEndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 'value') cil managed
  // SIG: 20 01 01 12 81 5D
  {
    .custom /*0C0000E1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::set_RemoteEndPoint

  .method /*0600007D*/ public hidebysig specialname virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::get_ConnectionId

  .method /*0600007E*/ public hidebysig specialname virtual 
          instance void  set_ConnectionId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000E3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::set_ConnectionId

  .method /*0600007F*/ public hidebysig specialname virtual 
          instance class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ 
          get_Features() cil managed
  // SIG: 20 00 12 81 19
  {
    .custom /*0C0000E4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::get_Features

  .method /*06000080*/ public hidebysig newslot specialname virtual 
          instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> 
          get_MemoryPool() cil managed
  // SIG: 20 00 15 12 80 99 01 05
  {
    .custom /*0C0000E6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000E7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::get_MemoryPool

  .method /*06000081*/ public hidebysig specialname 
          instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 
          get_Application() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C0000E8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000E9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::get_Application

  .method /*06000082*/ public hidebysig specialname 
          instance void  set_Application(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ 'value') cil managed
  // SIG: 20 01 01 12 80 81
  {
    .custom /*0C0000EB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000EC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::set_Application

  .method /*06000083*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 81 39 02 1C 1C
  {
    .param [0]/*08000054*/ 
    .custom /*0C0000C0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::get_Items

  .method /*06000084*/ public hidebysig specialname virtual 
          instance void  set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 'value') cil managed
  // SIG: 20 01 01 15 12 81 39 02 1C 1C
  {
    .param [1]/*08000055*/ 
    .custom /*0C0000C2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::set_Items

  .method /*06000085*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ 
          get_ConnectionClosed() cil managed
  // SIG: 20 00 11 80 F5
  {
    .custom /*0C0000ED:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::get_ConnectionClosed

  .method /*06000086*/ public hidebysig specialname virtual 
          instance void  set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 F5
  {
    .custom /*0C0000EE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::set_ConnectionClosed

  .method /*06000087*/ public hidebysig virtual 
          instance void  Abort(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionAbortedException/*01000058*/ abortReason) cil managed
  // SIG: 20 01 01 12 81 61
  {
    .custom /*0C0000EF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Abort

  .method /*06000088*/ assembly hidebysig 
          instance void  ResetFeatureCollection() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::ResetFeatureCollection

  .method /*06000089*/ private hidebysig newslot specialname virtual final 
          instance bool  Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_IsReadOnly() cil managed
  // SIG: 20 00 02
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::get_IsReadOnly /*01000046::0A00001C*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_IsReadOnly

  .method /*0600008A*/ private hidebysig newslot specialname virtual final 
          instance int32  Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Revision() cil managed
  // SIG: 20 00 08
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::get_Revision /*01000046::0A00001D*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Revision

  .method /*0600008B*/ private hidebysig newslot specialname virtual final 
          instance object  Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/ key) cil managed
  // SIG: 20 01 1C 12 80 A5
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::get_Item /*01000046::0A00001E*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Item

  .method /*0600008C*/ private hidebysig newslot specialname virtual final 
          instance void  Microsoft.AspNetCore.Http.Features.IFeatureCollection.set_Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/ key,
                                                                                        object 'value') cil managed
  // SIG: 20 02 01 12 80 A5 1C
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::set_Item /*01000046::0A00001F*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.set_Item

  .method /*0600008D*/ private hidebysig newslot virtual final 
          instance !!TFeature  Microsoft.AspNetCore.Http.Features.IFeatureCollection.Get<TFeature>() cil managed
  // SIG: 30 01 00 1E 00
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::Get /*01000046::0A000020*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.Get

  .method /*0600008E*/ private hidebysig newslot virtual final 
          instance void  Microsoft.AspNetCore.Http.Features.IFeatureCollection.Set<TFeature>(!!TFeature feature) cil managed
  // SIG: 30 01 01 01 1E 00
  {
    .override [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/::Set /*01000046::0A000021*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.Set

  .method /*0600008F*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>> 
          'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.GetEnumerator'() cil managed
  // SIG: 20 00 15 12 81 21 01 15 11 81 09 02 12 80 A5 1C
  {
    .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000048*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000047*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000042*/<class [System.Runtime/*23000001*/]System.Type/*01000029*/,object>>/*1B000004*/::GetEnumerator() /* 0A000022 */
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::'System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Type,System.Object>>.GetEnumerator'

  .method /*06000090*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*0100004A*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 29
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000049*/::GetEnumerator /*01000049::0A000023*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::System.Collections.IEnumerable.GetEnumerator

  .method /*06000091*/ private hidebysig newslot specialname virtual final 
          instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> 
          Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.get_MemoryPool() cil managed
  // SIG: 20 00 15 12 80 99 01 05
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature/*0100004B*/::get_MemoryPool /*0100004B::0A000024*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.get_MemoryPool

  .method /*06000092*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 
          Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.get_Items() cil managed
  // SIG: 20 00 15 12 81 39 02 1C 1C
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/::get_Items /*0100004D::0A000027*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.get_Items

  .method /*06000093*/ private hidebysig newslot specialname virtual final 
          instance void  Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 'value') cil managed
  // SIG: 20 01 01 15 12 81 39 02 1C 1C
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100004D*/::set_Items /*0100004D::0A000028*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.set_Items

  .method /*06000094*/ private hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ 
          Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.get_ConnectionClosed() cil managed
  // SIG: 20 00 11 80 F5
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/::get_ConnectionClosed /*0100004F::0A000029*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.get_ConnectionClosed

  .method /*06000095*/ private hidebysig newslot specialname virtual final 
          instance void  Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 F5
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/::set_ConnectionClosed /*0100004F::0A00002A*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.set_ConnectionClosed

  .method /*06000096*/ private hidebysig newslot virtual final 
          instance void  Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.Abort() cil managed
  // SIG: 20 00 01
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*0100004F*/::Abort /*0100004F::0A00002B*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.Abort

  .property /*17000022*/ instance callconv(8) class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/
          LocalEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::get_LocalEndPoint() /* 06000079 */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::set_LocalEndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/) /* 0600007A */
  } // end of property TransportMultiplexedConnection::LocalEndPoint
  .property /*17000023*/ instance callconv(8) class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/
          RemoteEndPoint()
  {
    .get instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::get_RemoteEndPoint() /* 0600007B */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::set_RemoteEndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/) /* 0600007C */
  } // end of property TransportMultiplexedConnection::RemoteEndPoint
  .property /*17000024*/ instance callconv(8) string
          ConnectionId()
  {
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::get_ConnectionId() /* 0600007D */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::set_ConnectionId(string) /* 0600007E */
  } // end of property TransportMultiplexedConnection::ConnectionId
  .property /*17000025*/ instance callconv(8) class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/
          Features()
  {
    .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::get_Features() /* 0600007F */
  } // end of property TransportMultiplexedConnection::Features
  .property /*17000026*/ instance callconv(8) class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8>
          MemoryPool()
  {
    .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::get_MemoryPool() /* 06000080 */
  } // end of property TransportMultiplexedConnection::MemoryPool
  .property /*17000027*/ instance callconv(8) class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/
          Application()
  {
    .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/ Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::get_Application() /* 06000081 */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::set_Application(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.IDuplexPipe/*01000020*/) /* 06000082 */
  } // end of property TransportMultiplexedConnection::Application
  .property /*17000028*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>
          Items()
  {
    .custom /*0C000093:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::get_Items() /* 06000083 */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>) /* 06000084 */
  } // end of property TransportMultiplexedConnection::Items
  .property /*17000029*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/
          ConnectionClosed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::get_ConnectionClosed() /* 06000085 */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/) /* 06000086 */
  } // end of property TransportMultiplexedConnection::ConnectionClosed
  .property /*1700002A*/ instance callconv(8) bool
          Microsoft.AspNetCore.Http.Features.IFeatureCollection.IsReadOnly()
  {
    .get instance bool Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_IsReadOnly() /* 06000089 */
  } // end of property TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.IsReadOnly
  .property /*1700002B*/ instance callconv(8) int32
          Microsoft.AspNetCore.Http.Features.IFeatureCollection.Revision()
  {
    .get instance int32 Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Revision() /* 0600008A */
  } // end of property TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.Revision
  .property /*1700002C*/ instance callconv(8) object
          Microsoft.AspNetCore.Http.Features.IFeatureCollection.Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/)
  {
    .get instance object Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Http.Features.IFeatureCollection.get_Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0600008B */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Http.Features.IFeatureCollection.set_Item(class [System.Runtime/*23000001*/]System.Type/*01000029*/,
                                                                                                                                                                   object) /* 0600008C */
  } // end of property TransportMultiplexedConnection::Microsoft.AspNetCore.Http.Features.IFeatureCollection.Item
  .property /*1700002D*/ instance callconv(8) class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8>
          Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.MemoryPool()
  {
    .custom /*0C000099:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.get_MemoryPool() /* 06000091 */
  } // end of property TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IMemoryPoolFeature.MemoryPool
  .property /*1700002E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>
          Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.Items()
  {
    .custom /*0C00009B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.get_Items() /* 06000092 */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>) /* 06000093 */
  } // end of property TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature.Items
  .property /*1700002F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/
          Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.ConnectionClosed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.get_ConnectionClosed() /* 06000094 */
    .set instance void Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/) /* 06000095 */
  } // end of property TransportMultiplexedConnection::Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature.ConnectionClosed
} // end of class Microsoft.AspNetCore.Connections.TransportMultiplexedConnection

.class /*02000015*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Hosting.WebHostBuilderQuicExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C000053:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000055:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200002E*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
  {
    .custom /*0C00009A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000039*/ public static initonly class Microsoft.AspNetCore.Hosting.WebHostBuilderQuicExtensions/*02000015*//'<>c'/*0200002E*/ '<>9'
    .field /*0400003A*/ public static class [System.Runtime/*23000001*/]System.Action`1/*0100005C*/<class [Microsoft.Extensions.DependencyInjection.Abstractions/*2300000C*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100005D*/> '<>9__0_0'
    .method /*06000127*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000128*/ assembly hidebysig 
            instance void  '<UseQuic>b__0_0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*2300000C*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100005D*/ services) cil managed
    // SIG: 20 01 01 12 81 75
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<UseQuic>b__0_0'

  } // end of class '<>c'

  .class /*0200002F*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_0'
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
  {
    .custom /*0C00009C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400003B*/ public class [System.Runtime/*23000001*/]System.Action`1/*0100005C*/<class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/> configureOptions
    .method /*06000129*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::.ctor

    .method /*0600012A*/ assembly hidebysig 
            instance void  '<UseQuic>b__0'(class [Microsoft.Extensions.DependencyInjection.Abstractions/*2300000C*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100005D*/ services) cil managed
    // SIG: 20 01 01 12 81 75
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::'<UseQuic>b__0'

  } // end of class '<>c__DisplayClass1_0'

  .method /*06000097*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*2300000B*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*0100005B*/ 
          UseQuic(class [Microsoft.AspNetCore.Hosting.Abstractions/*2300000B*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*0100005B*/ hostBuilder) cil managed
  // SIG: 00 01 12 81 6D 12 81 6D
  {
    .custom /*0C0000F0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.RequiresPreviewFeaturesAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderQuicExtensions::UseQuic

  .method /*06000098*/ public hidebysig static 
          class [Microsoft.AspNetCore.Hosting.Abstractions/*2300000B*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*0100005B*/ 
          UseQuic(class [Microsoft.AspNetCore.Hosting.Abstractions/*2300000B*/]Microsoft.AspNetCore.Hosting.IWebHostBuilder/*0100005B*/ hostBuilder,
                  class [System.Runtime/*23000001*/]System.Action`1/*0100005C*/<class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/> configureOptions) cil managed
  // SIG: 00 02 12 81 6D 12 81 6D 15 12 81 71 01 12 6C
  {
    .custom /*0C0000F2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.RequiresPreviewFeaturesAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebHostBuilderQuicExtensions::UseQuic

} // end of class Microsoft.AspNetCore.Hosting.WebHostBuilderQuicExtensions

.class /*02000016*/ interface private abstract auto ansi Microsoft.AspNetCore.Server.Kestrel.IPooledStream
{
  .method /*06000099*/ public hidebysig newslot specialname abstract virtual 
          instance int64  get_PoolExpirationTicks() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x0
  } // end of method IPooledStream::get_PoolExpirationTicks

  .method /*0600009A*/ public hidebysig newslot abstract virtual 
          instance void  DisposeCore() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IPooledStream::DisposeCore

  .property /*17000030*/ instance callconv(8) int64
          PoolExpirationTicks()
  {
    .get instance int64 Microsoft.AspNetCore.Server.Kestrel.IPooledStream/*02000016*/::get_PoolExpirationTicks() /* 06000099 */
  } // end of property IPooledStream::PoolExpirationTicks
} // end of class Microsoft.AspNetCore.Server.Kestrel.IPooledStream

.class /*02000017*/ private sequential ansi sealed beforefieldinit Microsoft.AspNetCore.Server.Kestrel.PooledStreamStack`1<class (Microsoft.AspNetCore.Server.Kestrel.IPooledStream/*02000016*/) TValue>
       extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
{
  .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000030*/ sequential ansi sealed nested assembly beforefieldinit StreamAsValueType<class (Microsoft.AspNetCore.Server.Kestrel.IPooledStream/*02000016*/) TValue>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
  {
    .custom /*0C00009D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00009E:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000024*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .field /*0400003C*/ private initonly !TValue _value
    .method /*0600012B*/ public hidebysig specialname static 
            valuetype Microsoft.AspNetCore.Server.Kestrel.PooledStreamStack`1/*02000017*//StreamAsValueType/*02000030*/<!TValue> 
            op_Implicit(!TValue s) cil managed
    // SIG: 00 01 15 11 80 C0 01 13 00 13 00
    {
      .param [1]/*080000FE*/ 
      .custom /*0C000129:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StreamAsValueType::op_Implicit

    .method /*0600012C*/ public hidebysig specialname static 
            !TValue  op_Implicit(valuetype Microsoft.AspNetCore.Server.Kestrel.PooledStreamStack`1/*02000017*//StreamAsValueType/*02000030*/<!TValue> s) cil managed
    // SIG: 00 01 13 00 15 11 80 C0 01 13 00
    {
      .param [0]/*080000FF*/ 
      .custom /*0C00012A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method StreamAsValueType::op_Implicit

  } // end of class StreamAsValueType

  .field /*04000016*/ assembly valuetype Microsoft.AspNetCore.Server.Kestrel.PooledStreamStack`1/*02000017*//StreamAsValueType/*02000030*/<!TValue>[] _array
  .custom /*0C000058:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 00 00 00 ) 
  .field /*04000017*/ private int32 _size
  .method /*0600009B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 size) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PooledStreamStack`1::.ctor

  .method /*0600009C*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PooledStreamStack`1::get_Count

  .method /*0600009D*/ public hidebysig instance bool 
          TryPop([out] !TValue& result) cil managed
  // SIG: 20 01 02 10 13 00
  {
    .custom /*0C0000F4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000062*/ 
    .custom /*0C0000D3:0A000031*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005E*/::.ctor(bool) /* 0A000031 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PooledStreamStack`1::TryPop

  .method /*0600009E*/ public hidebysig instance bool 
          TryPeek([out] !TValue& result) cil managed
  // SIG: 20 01 02 10 13 00
  {
    .custom /*0C0000F5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000063*/ 
    .custom /*0C0000D4:0A000031*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100005E*/::.ctor(bool) /* 0A000031 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PooledStreamStack`1::TryPeek

  .method /*0600009F*/ public hidebysig instance void 
          Push(!TValue item) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PooledStreamStack`1::Push

  .method /*060000A0*/ public hidebysig instance void 
          RemoveExpired(int64 now) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PooledStreamStack`1::RemoveExpired

  .property /*17000031*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.AspNetCore.Server.Kestrel.PooledStreamStack`1/*02000017*/::get_Count() /* 0600009C */
  } // end of property PooledStreamStack`1::Count
} // end of class Microsoft.AspNetCore.Server.Kestrel.PooledStreamStack`1

.class /*02000018*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure.StringUtilities
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C00005F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000A1*/ public hidebysig static 
          string  GetAsciiOrUTF8StringNonNullCharacters(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100005F*/<uint8> span,
                                                        class [System.Runtime/*23000001*/]System.Text.Encoding/*01000060*/ defaultEncoding) cil managed
  // SIG: 00 02 0E 15 11 81 7D 01 05 12 81 81
  {
    .custom /*0C0000F6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000F7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000066*/ 
    .custom /*0C0000D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringUtilities::GetAsciiOrUTF8StringNonNullCharacters

  .method /*060000A2*/ public hidebysig static 
          string  GetAsciiStringNonNullCharacters(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100005F*/<uint8> span) cil managed
  // SIG: 00 01 0E 15 11 81 7D 01 05
  {
    .custom /*0C0000F8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000068*/ 
    .custom /*0C0000D9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringUtilities::GetAsciiStringNonNullCharacters

  .method /*060000A3*/ public hidebysig static 
          string  GetLatin1StringNonNullCharacters(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100005F*/<uint8> span) cil managed
  // SIG: 00 01 0E 15 11 81 7D 01 05
  {
    .custom /*0C0000F9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800006A*/ 
    .custom /*0C0000DA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringUtilities::GetLatin1StringNonNullCharacters

  .method /*060000A4*/ public hidebysig static 
          bool  TryGetAsciiString(uint8* input,
                                  char* output,
                                  int32 count) cil managed aggressiveoptimization
  // SIG: 00 03 02 0F 05 0F 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringUtilities::TryGetAsciiString

  .method /*060000A5*/ public hidebysig static 
          bool  TryGetLatin1String(uint8* input,
                                   char* output,
                                   int32 count) cil managed aggressiveoptimization
  // SIG: 00 03 02 0F 05 0F 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringUtilities::TryGetLatin1String

  .method /*060000A6*/ public hidebysig static 
          bool  BytesOrdinalEqualsStringAndAscii(string previousValue,
                                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100005F*/<uint8> newValue) cil managed aggressiveoptimization
  // SIG: 00 02 02 0E 15 11 81 7D 01 05
  {
    .param [1]/*08000072*/ 
    .custom /*0C0000DB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringUtilities::BytesOrdinalEqualsStringAndAscii

  .method /*060000A7*/ public hidebysig static 
          string  ConcatAsHexSuffix(string str,
                                    char separator,
                                    uint32 number) cil managed
  // SIG: 00 03 0E 0E 03 09
  {
    .custom /*0C0000FA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StringUtilities::ConcatAsHexSuffix

} // end of class Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure.StringUtilities

.class /*02000019*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicConnectionFactory
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
       implements [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.IMultiplexedConnectionFactory/*01000061*/
{
  .class /*02000031*/ auto ansi sealed nested private beforefieldinit '<ConnectAsync>d__2'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000A0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400003D*/ public int32 '<>1__state'
    .field /*0400003E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*01000064*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*01000059*/> '<>t__builder'
    .field /*0400003F*/ public class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ endPoint
    .field /*04000040*/ public class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ features
    .field /*04000041*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken
    .field /*04000042*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicConnectionFactory/*02000019*/ '<>4__this'
    .field /*04000043*/ private class [System.Net.Quic/*2300000E*/]System.Net.Quic.QuicConnection/*01000065*/ '<connection>5__2'
    .field /*04000044*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000066*/ '<>u__1'
    .method /*0600012D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ConnectAsync>d__2'::MoveNext

    .method /*0600012E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000143:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ConnectAsync>d__2'::SetStateMachine

  } // end of class '<ConnectAsync>d__2'

  .method /*060000A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IOptions`1/*01000062*/<class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/> options,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000063*/ loggerFactory) cil managed
  // SIG: 20 02 01 15 12 81 89 01 12 6C 12 81 8D
  {
    .custom /*0C0000FB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionFactory::.ctor

  .method /*060000A9*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*01000059*/> 
          ConnectAsync(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ endPoint,
                       [opt] class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ features,
                       [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 03 15 11 80 ED 01 12 81 65 12 81 5D 12 81 19 11 80 F5
  {
    .custom /*0C0000FC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000FD:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002A*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0A000018 */ = ( 01 00 5B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..[Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E 4B   // NetCore.Server.K
                                                                                                                                                                                                                                        65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72 74   // estrel.Transport
                                                                                                                                                                                                                                        2E 51 75 69 63 2E 51 75 69 63 43 6F 6E 6E 65 63   // .Quic.QuicConnec
                                                                                                                                                                                                                                        74 69 6F 6E 46 61 63 74 6F 72 79 2B 3C 43 6F 6E   // tionFactory+<Con
                                                                                                                                                                                                                                        6E 65 63 74 41 73 79 6E 63 3E 64 5F 5F 32 00 00 ) // nectAsync>d__2..
    .param [0]/*08000079*/ 
    .custom /*0C0000DD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800007B*/  = nullref
    .custom /*0C0000E0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800007C*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionFactory::ConnectAsync

} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicConnectionFactory

.class /*0200001A*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportFactory
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
       implements [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.IMultiplexedConnectionListenerFactory/*01000067*/
{
  .method /*060000AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000063*/ loggerFactory,
                               class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IOptions`1/*01000062*/<class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/> options) cil managed
  // SIG: 20 02 01 12 81 8D 15 12 81 89 01 12 6C
  {
    .custom /*0C0000FE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportFactory::.ctor

  .method /*060000AB*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.IMultiplexedConnectionListener/*01000068*/> 
          BindAsync(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ endpoint,
                    [opt] class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ features,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 03 15 11 80 ED 01 12 81 A1 12 81 5D 12 81 19 11 80 F5
  {
    .custom /*0C000100:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*0800007F*/ 
    .custom /*0C0000E5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000081*/  = nullref
    .custom /*0C0000EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000082*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportFactory::BindAsync

} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportFactory

.class /*0200001B*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C000066:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.RequiresPreviewFeaturesAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000018*/ assembly class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.ISystemClock/*0200001D*/ SystemClock
  .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060000AC*/ public hidebysig specialname 
          instance uint16  get_MaxBidirectionalStreamCount() cil managed
  // SIG: 20 00 07
  {
    .custom /*0C000101:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::get_MaxBidirectionalStreamCount

  .method /*060000AD*/ public hidebysig specialname 
          instance void  set_MaxBidirectionalStreamCount(uint16 'value') cil managed
  // SIG: 20 01 01 07
  {
    .custom /*0C000103:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::set_MaxBidirectionalStreamCount

  .method /*060000AE*/ public hidebysig specialname 
          instance uint16  get_MaxUnidirectionalStreamCount() cil managed
  // SIG: 20 00 07
  {
    .custom /*0C000104:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::get_MaxUnidirectionalStreamCount

  .method /*060000AF*/ public hidebysig specialname 
          instance void  set_MaxUnidirectionalStreamCount(uint16 'value') cil managed
  // SIG: 20 01 01 07
  {
    .custom /*0C000105:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::set_MaxUnidirectionalStreamCount

  .method /*060000B0*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002B*/ 
          get_IdleTimeout() cil managed
  // SIG: 20 00 11 80 AD
  {
    .custom /*0C000106:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::get_IdleTimeout

  .method /*060000B1*/ public hidebysig specialname 
          instance void  set_IdleTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002B*/ 'value') cil managed
  // SIG: 20 01 01 11 80 AD
  {
    .custom /*0C000107:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::set_IdleTimeout

  .method /*060000B2*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64> 
          get_MaxReadBufferSize() cil managed
  // SIG: 20 00 15 11 81 A5 01 0A
  {
    .custom /*0C000108:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::get_MaxReadBufferSize

  .method /*060000B3*/ public hidebysig specialname 
          instance void  set_MaxReadBufferSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 81 A5 01 0A
  {
    .custom /*0C00010A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::set_MaxReadBufferSize

  .method /*060000B4*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64> 
          get_MaxWriteBufferSize() cil managed
  // SIG: 20 00 15 11 81 A5 01 0A
  {
    .custom /*0C00010B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::get_MaxWriteBufferSize

  .method /*060000B5*/ public hidebysig specialname 
          instance void  set_MaxWriteBufferSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 81 A5 01 0A
  {
    .custom /*0C00010C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::set_MaxWriteBufferSize

  .method /*060000B6*/ public hidebysig specialname 
          instance int32  get_Backlog() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00010D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::get_Backlog

  .method /*060000B7*/ public hidebysig specialname 
          instance void  set_Backlog(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C00010E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::set_Backlog

  .method /*060000B8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportOptions::.ctor

  .property /*17000032*/ instance callconv(8) uint16
          MaxBidirectionalStreamCount()
  {
    .get instance uint16 Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::get_MaxBidirectionalStreamCount() /* 060000AC */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::set_MaxBidirectionalStreamCount(uint16) /* 060000AD */
  } // end of property QuicTransportOptions::MaxBidirectionalStreamCount
  .property /*17000033*/ instance callconv(8) uint16
          MaxUnidirectionalStreamCount()
  {
    .get instance uint16 Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::get_MaxUnidirectionalStreamCount() /* 060000AE */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::set_MaxUnidirectionalStreamCount(uint16) /* 060000AF */
  } // end of property QuicTransportOptions::MaxUnidirectionalStreamCount
  .property /*17000034*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002B*/
          IdleTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002B*/ Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::get_IdleTimeout() /* 060000B0 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::set_IdleTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100002B*/) /* 060000B1 */
  } // end of property QuicTransportOptions::IdleTimeout
  .property /*17000035*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64>
          MaxReadBufferSize()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64> Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::get_MaxReadBufferSize() /* 060000B2 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::set_MaxReadBufferSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64>) /* 060000B3 */
  } // end of property QuicTransportOptions::MaxReadBufferSize
  .property /*17000036*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64>
          MaxWriteBufferSize()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64> Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::get_MaxWriteBufferSize() /* 060000B4 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::set_MaxWriteBufferSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000069*/<int64>) /* 060000B5 */
  } // end of property QuicTransportOptions::MaxWriteBufferSize
  .property /*17000037*/ instance callconv(8) int32
          Backlog()
  {
    .get instance int32 Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::get_Backlog() /* 060000B6 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/::set_Backlog(int32) /* 060000B7 */
  } // end of property QuicTransportOptions::Backlog
} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions

.class /*0200001C*/ interface private abstract auto ansi Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace
       implements [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILogger/*01000040*/
{
  .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILogger/*01000040*/
  .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000B9*/ public hidebysig newslot abstract virtual 
          instance void  AcceptedConnection(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.BaseConnectionContext/*0100006A*/ connection) cil managed
  // SIG: 20 01 01 12 81 A9
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::AcceptedConnection

  .method /*060000BA*/ public hidebysig newslot abstract virtual 
          instance void  AcceptedStream(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext) cil managed
  // SIG: 20 01 01 12 80 84
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::AcceptedStream

  .method /*060000BB*/ public hidebysig newslot abstract virtual 
          instance void  ConnectedStream(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext) cil managed
  // SIG: 20 01 01 12 80 84
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::ConnectedStream

  .method /*060000BC*/ public hidebysig newslot abstract virtual 
          instance void  ConnectionError(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.BaseConnectionContext/*0100006A*/ connection,
                                         class [System.Runtime/*23000001*/]System.Exception/*01000028*/ ex) cil managed
  // SIG: 20 02 01 12 81 A9 12 80 A1
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::ConnectionError

  .method /*060000BD*/ public hidebysig newslot abstract virtual 
          instance void  ConnectionAborted(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.BaseConnectionContext/*0100006A*/ connection,
                                           int64 errorCode,
                                           class [System.Runtime/*23000001*/]System.Exception/*01000028*/ ex) cil managed
  // SIG: 20 03 01 12 81 A9 0A 12 80 A1
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::ConnectionAborted

  .method /*060000BE*/ public hidebysig newslot abstract virtual 
          instance void  ConnectionAbort(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.BaseConnectionContext/*0100006A*/ connection,
                                         int64 errorCode,
                                         string reason) cil managed
  // SIG: 20 03 01 12 81 A9 0A 0E
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::ConnectionAbort

  .method /*060000BF*/ public hidebysig newslot abstract virtual 
          instance void  StreamError(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                     class [System.Runtime/*23000001*/]System.Exception/*01000028*/ ex) cil managed
  // SIG: 20 02 01 12 80 84 12 80 A1
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamError

  .method /*060000C0*/ public hidebysig newslot abstract virtual 
          instance void  StreamPause(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext) cil managed
  // SIG: 20 01 01 12 80 84
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamPause

  .method /*060000C1*/ public hidebysig newslot abstract virtual 
          instance void  StreamResume(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext) cil managed
  // SIG: 20 01 01 12 80 84
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamResume

  .method /*060000C2*/ public hidebysig newslot abstract virtual 
          instance void  StreamShutdownWrite(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                             string reason) cil managed
  // SIG: 20 02 01 12 80 84 0E
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamShutdownWrite

  .method /*060000C3*/ public hidebysig newslot abstract virtual 
          instance void  StreamAbortedRead(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                           int64 errorCode) cil managed
  // SIG: 20 02 01 12 80 84 0A
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamAbortedRead

  .method /*060000C4*/ public hidebysig newslot abstract virtual 
          instance void  StreamAbortedWrite(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                            int64 errorCode) cil managed
  // SIG: 20 02 01 12 80 84 0A
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamAbortedWrite

  .method /*060000C5*/ public hidebysig newslot abstract virtual 
          instance void  StreamAbort(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                     int64 errorCode,
                                     string reason) cil managed
  // SIG: 20 03 01 12 80 84 0A 0E
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamAbort

  .method /*060000C6*/ public hidebysig newslot abstract virtual 
          instance void  StreamAbortRead(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                         int64 errorCode,
                                         string reason) cil managed
  // SIG: 20 03 01 12 80 84 0A 0E
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamAbortRead

  .method /*060000C7*/ public hidebysig newslot abstract virtual 
          instance void  StreamAbortWrite(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                          int64 errorCode,
                                          string reason) cil managed
  // SIG: 20 03 01 12 80 84 0A 0E
  {
    // Method begins at RVA 0x0
  } // end of method IQuicTrace::StreamAbortWrite

} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace

.class /*0200001D*/ interface private abstract auto ansi Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.ISystemClock
{
  .method /*060000C8*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100006B*/ 
          get_UtcNow() cil managed
  // SIG: 20 00 11 81 AD
  {
    // Method begins at RVA 0x0
  } // end of method ISystemClock::get_UtcNow

  .property /*17000038*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100006B*/
          UtcNow()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100006B*/ Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.ISystemClock/*0200001D*/::get_UtcNow() /* 060000C8 */
  } // end of property ISystemClock::UtcNow
} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.ISystemClock

.class /*0200001E*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.SystemClock
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
       implements Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.ISystemClock/*0200001D*/
{
  .method /*060000C9*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100006B*/ 
          get_UtcNow() cil managed
  // SIG: 20 00 11 81 AD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemClock::get_UtcNow

  .method /*060000CA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemClock::.ctor

  .property /*17000039*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100006B*/
          UtcNow()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*0100006B*/ Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.SystemClock/*0200001E*/::get_UtcNow() /* 060000C9 */
  } // end of property SystemClock::UtcNow
} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.SystemClock

.class /*0200001F*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext
       extends Microsoft.AspNetCore.Connections.TransportMultiplexedConnection/*02000014*/
       implements [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature/*01000053*/,
                  [Microsoft.AspNetCore.Http.Features/*2300000A*/]Microsoft.AspNetCore.Http.Features.ITlsConnectionFeature/*0100005A*/
{
  .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature/*01000053*/
  .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Features/*2300000A*/]Microsoft.AspNetCore.Http.Features.ITlsConnectionFeature/*0100005A*/
  .custom /*0C000060:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000032*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
  {
    .custom /*0C0000A1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000045*/ public static initonly class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*//'<>c'/*02000032*/ '<>9'
    .field /*04000046*/ public static class [System.Runtime/*23000001*/]System.Action`1/*0100005C*/<class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/> '<>9__18_0'
    .field /*04000047*/ public static class [System.Runtime/*23000001*/]System.Action`1/*0100005C*/<object> '<>9__21_0'
    .method /*0600012F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000130*/ assembly hidebysig 
            instance void  '<AcceptAsync>b__18_0'(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/ state) cil managed
    // SIG: 20 01 01 12 7C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AcceptAsync>b__18_0'

    .method /*06000131*/ assembly hidebysig 
            instance void  '<TryReturnStream>b__21_0'(object state) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<TryReturnStream>b__21_0'

  } // end of class '<>c'

  .class /*02000033*/ auto ansi sealed nested private beforefieldinit '<AcceptAsync>d__18'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000A2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000048*/ public int32 '<>1__state'
    .field /*04000049*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*01000064*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000045*/> '<>t__builder'
    .field /*0400004A*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/ '<>4__this'
    .field /*0400004B*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken
    .field /*0400004C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100006E*/<class [System.Net.Quic/*2300000E*/]System.Net.Quic.QuicStream/*0100006F*/> '<>u__1'
    .method /*06000132*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AcceptAsync>d__18'::MoveNext

    .method /*06000133*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000144:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AcceptAsync>d__18'::SetStateMachine

  } // end of class '<AcceptAsync>d__18'

  .class /*02000034*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__15'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000A3:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400004D*/ public int32 '<>1__state'
    .field /*0400004E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000070*/ '<>t__builder'
    .field /*0400004F*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/ '<>4__this'
    .field /*04000050*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .method /*06000134*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__15'::MoveNext

    .method /*06000135*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000145:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__15'::SetStateMachine

  } // end of class '<DisposeAsync>d__15'

  .field /*04000019*/ assembly valuetype Microsoft.AspNetCore.Server.Kestrel.PooledStreamStack`1/*02000017*/<class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/> StreamPool
  .custom /*0C000062:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .field /*0400001A*/ static assembly literal int32 InitialStreamPoolSize = int32(0x00000005)
  .field /*0400001B*/ static assembly literal int32 MaxStreamPoolSize = int32(0x00000064)
  .field /*0400001C*/ static assembly literal int64 StreamPoolExpiryTicks = int64(0x2FAF080)
  .method /*060000CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Net.Quic/*2300000E*/]System.Net.Quic.QuicConnection/*01000065*/ connection,
                               class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTransportContext/*02000023*/ context) cil managed
  // SIG: 20 02 01 12 81 95 12 80 8C
  {
    .custom /*0C000110:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::.ctor

  .method /*060000CC*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003A*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 E9
  {
    .custom /*0C000111:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002A*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0A000018 */ = ( 01 00 65 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..eMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E 4B   // NetCore.Server.K
                                                                                                                                                                                                                                        65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72 74   // estrel.Transport
                                                                                                                                                                                                                                        2E 51 75 69 63 2E 49 6E 74 65 72 6E 61 6C 2E 51   // .Quic.Internal.Q
                                                                                                                                                                                                                                        75 69 63 43 6F 6E 6E 65 63 74 69 6F 6E 43 6F 6E   // uicConnectionCon
                                                                                                                                                                                                                                        74 65 78 74 2B 3C 44 69 73 70 6F 73 65 41 73 79   // text+<DisposeAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 31 35 00 00 )                   // nc>d__15..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::DisposeAsync

  .method /*060000CD*/ public hidebysig virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::Abort

  .method /*060000CE*/ public hidebysig virtual 
          instance void  Abort(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionAbortedException/*01000058*/ abortReason) cil managed
  // SIG: 20 01 01 12 81 61
  {
    .custom /*0C000112:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::Abort

  .method /*060000CF*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000045*/> 
          AcceptAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 01 15 11 80 ED 01 12 81 15 11 80 F5
  {
    .custom /*0C000114:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002A*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0A000018 */ = ( 01 00 64 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..dMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E 4B   // NetCore.Server.K
                                                                                                                                                                                                                                        65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72 74   // estrel.Transport
                                                                                                                                                                                                                                        2E 51 75 69 63 2E 49 6E 74 65 72 6E 61 6C 2E 51   // .Quic.Internal.Q
                                                                                                                                                                                                                                        75 69 63 43 6F 6E 6E 65 63 74 69 6F 6E 43 6F 6E   // uicConnectionCon
                                                                                                                                                                                                                                        74 65 78 74 2B 3C 41 63 63 65 70 74 41 73 79 6E   // text+<AcceptAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 31 38 00 00 )                      // c>d__18..
    .param [0]/*080000AA*/ 
    .custom /*0C0000FF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [1]/*080000AB*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::AcceptAsync

  .method /*060000D0*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000045*/> 
          ConnectAsync([opt] class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ features,
                       [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 ED 01 12 81 15 12 81 19 11 80 F5
  {
    .param [0]/*080000AC*/ 
    .custom /*0C000102:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*080000AD*/  = nullref
    .param [2]/*080000AE*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::ConnectAsync

  .method /*060000D1*/ assembly hidebysig 
          instance bool  TryReturnStream(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ 'stream') cil managed
  // SIG: 20 01 02 12 80 84
  {
    .custom /*0C000116:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::TryReturnStream

  .method /*060000D2*/ public hidebysig newslot specialname virtual final 
          instance int64  get_Error() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::get_Error

  .method /*060000D3*/ public hidebysig newslot specialname virtual final 
          instance void  set_Error(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::set_Error

  .method /*060000D4*/ public hidebysig newslot specialname virtual final 
          instance class [System.Security.Cryptography.X509Certificates/*2300000F*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100006C*/ 
          get_ClientCertificate() cil managed
  // SIG: 20 00 12 81 B1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::get_ClientCertificate

  .method /*060000D5*/ public hidebysig newslot specialname virtual final 
          instance void  set_ClientCertificate(class [System.Security.Cryptography.X509Certificates/*2300000F*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100006C*/ 'value') cil managed
  // SIG: 20 01 01 12 81 B1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::set_ClientCertificate

  .method /*060000D6*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*0100006D*/<class [System.Security.Cryptography.X509Certificates/*2300000F*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100006C*/> 
          GetClientCertificateAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 81 B5 01 12 81 B1 11 80 F5
  {
    .param [0]/*080000B2*/ 
    .custom /*0C000109:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionContext::GetClientCertificateAsync

  .property /*1700003A*/ instance callconv(8) int64
          Error()
  {
    .get instance int64 Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/::get_Error() /* 060000D2 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/::set_Error(int64) /* 060000D3 */
  } // end of property QuicConnectionContext::Error
  .property /*1700003B*/ instance callconv(8) class [System.Security.Cryptography.X509Certificates/*2300000F*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100006C*/
          ClientCertificate()
  {
    .get instance class [System.Security.Cryptography.X509Certificates/*2300000F*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100006C*/ Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/::get_ClientCertificate() /* 060000D4 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/::set_ClientCertificate(class [System.Security.Cryptography.X509Certificates/*2300000F*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100006C*/) /* 060000D5 */
  } // end of property QuicConnectionContext::ClientCertificate
} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext

.class /*02000020*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionListener
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
       implements [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.IMultiplexedConnectionListener/*01000068*/,
                  [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000071*/
{
  .custom /*0C000079:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.IMultiplexedConnectionListener/*01000068*/
  .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*01000071*/
  .custom /*0C000065:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000035*/ auto ansi sealed nested private beforefieldinit '<AcceptAsync>d__9'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000A4:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000051*/ public int32 '<>1__state'
    .field /*04000052*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*01000064*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*01000059*/> '<>t__builder'
    .field /*04000053*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionListener/*02000020*/ '<>4__this'
    .field /*04000054*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken
    .field /*04000055*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100006E*/<class [System.Net.Quic/*2300000E*/]System.Net.Quic.QuicConnection/*01000065*/> '<>u__1'
    .method /*06000136*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AcceptAsync>d__9'::MoveNext

    .method /*06000137*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000146:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AcceptAsync>d__9'::SetStateMachine

  } // end of class '<AcceptAsync>d__9'

  .class /*02000036*/ auto ansi sealed nested private beforefieldinit '<UnbindAsync>d__10'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000A5:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000056*/ public int32 '<>1__state'
    .field /*04000057*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000070*/ '<>t__builder'
    .field /*04000058*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionListener/*02000020*/ '<>4__this'
    .field /*04000059*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000066*/ '<>u__1'
    .method /*06000138*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<UnbindAsync>d__10'::MoveNext

    .method /*06000139*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000147:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<UnbindAsync>d__10'::SetStateMachine

  } // end of class '<UnbindAsync>d__10'

  .method /*060000D7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/ options,
                               class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace/*0200001C*/ log,
                               class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ endpoint,
                               class [System.Net.Security/*23000010*/]System.Net.Security.SslServerAuthenticationOptions/*01000072*/ sslServerAuthenticationOptions) cil managed
  // SIG: 20 04 01 12 6C 12 70 12 81 5D 12 81 C9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionListener::.ctor

  .method /*060000D8*/ public hidebysig newslot specialname virtual final 
          instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 
          get_EndPoint() cil managed
  // SIG: 20 00 12 81 5D
  {
    .custom /*0C000117:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionListener::get_EndPoint

  .method /*060000D9*/ public hidebysig specialname 
          instance void  set_EndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ 'value') cil managed
  // SIG: 20 01 01 12 81 5D
  {
    .custom /*0C000118:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionListener::set_EndPoint

  .method /*060000DA*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.MultiplexedConnectionContext/*01000059*/> 
          AcceptAsync([opt] class [Microsoft.Extensions.Features/*23000008*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000046*/ features,
                      [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 ED 01 12 81 65 12 81 19 11 80 F5
  {
    .custom /*0C000119:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00011A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002A*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0A000018 */ = ( 01 00 64 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..dMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E 4B   // NetCore.Server.K
                                                                                                                                                                                                                                        65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72 74   // estrel.Transport
                                                                                                                                                                                                                                        2E 51 75 69 63 2E 49 6E 74 65 72 6E 61 6C 2E 51   // .Quic.Internal.Q
                                                                                                                                                                                                                                        75 69 63 43 6F 6E 6E 65 63 74 69 6F 6E 4C 69 73   // uicConnectionLis
                                                                                                                                                                                                                                        74 65 6E 65 72 2B 3C 41 63 63 65 70 74 41 73 79   // tener+<AcceptAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 39 00 00 )                      // nc>d__9..
    .param [0]/*080000B9*/ 
    .custom /*0C00010F:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [1]/*080000BA*/  = nullref
    .param [2]/*080000BB*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionListener::AcceptAsync

  .method /*060000DB*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003A*/ 
          UnbindAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003D*/ cancellationToken) cil managed
  // SIG: 20 01 11 80 E9 11 80 F5
  {
    .custom /*0C00011B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002A*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0A000018 */ = ( 01 00 65 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..eMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E 4B   // NetCore.Server.K
                                                                                                                                                                                                                                        65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72 74   // estrel.Transport
                                                                                                                                                                                                                                        2E 51 75 69 63 2E 49 6E 74 65 72 6E 61 6C 2E 51   // .Quic.Internal.Q
                                                                                                                                                                                                                                        75 69 63 43 6F 6E 6E 65 63 74 69 6F 6E 4C 69 73   // uicConnectionLis
                                                                                                                                                                                                                                        74 65 6E 65 72 2B 3C 55 6E 62 69 6E 64 41 73 79   // tener+<UnbindAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 31 30 00 00 )                   // nc>d__10..
    .param [1]/*080000BC*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionListener::UnbindAsync

  .method /*060000DC*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003A*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 E9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicConnectionListener::DisposeAsync

  .property /*1700003C*/ instance callconv(8) class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/
          EndPoint()
  {
    .get instance class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/ Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionListener/*02000020*/::get_EndPoint() /* 060000D8 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionListener/*02000020*/::set_EndPoint(class [System.Net.Primitives/*23000009*/]System.Net.EndPoint/*01000057*/) /* 060000D9 */
  } // end of property QuicConnectionListener::EndPoint
} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionListener

.class /*02000021*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext
       extends Microsoft.AspNetCore.Connections.TransportConnection/*02000013*/
       implements Microsoft.AspNetCore.Server.Kestrel.IPooledStream/*02000016*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*01000037*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature/*01000051*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamDirectionFeature/*01000054*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature/*01000053*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamIdFeature/*01000055*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamAbortFeature/*01000056*/
{
  .custom /*0C00007F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000080:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Server.Kestrel.IPooledStream/*02000016*/
  .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000037*/
  .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature/*01000051*/
  .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamDirectionFeature/*01000054*/
  .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IProtocolErrorCodeFeature/*01000053*/
  .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamIdFeature/*01000055*/
  .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IStreamAbortFeature/*01000056*/
  .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000037*/ auto ansi sealed nested private beforefieldinit '<<DoReceive>g__AwaitCompleteTaskAsync|46_0>d'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000A7:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400005A*/ public int32 '<>1__state'
    .field /*0400005B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*01000064*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.FlushResult/*0100003E*/> '<>t__builder'
    .field /*0400005C*/ public valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003A*/ completeTask
    .field /*0400005D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000066*/ '<>u__1'
    .method /*0600013A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<DoReceive>g__AwaitCompleteTaskAsync|46_0>d'::MoveNext

    .method /*0600013B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000148:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<DoReceive>g__AwaitCompleteTaskAsync|46_0>d'::SetStateMachine

  } // end of class '<<DoReceive>g__AwaitCompleteTaskAsync|46_0>d'

  .class /*02000038*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
  {
    .custom /*0C0000A9:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400005E*/ public static initonly class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*//'<>c'/*02000038*/ '<>9'
    .field /*0400005F*/ public static class [System.Runtime/*23000001*/]System.Action`1/*0100005C*/<class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/> '<>9__48_0'
    .method /*0600013C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600013D*/ assembly hidebysig 
            instance void  '<FireStreamClosedAsync>b__48_0'(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ state) cil managed
    // SIG: 20 01 01 12 80 84
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<FireStreamClosedAsync>b__48_0'

  } // end of class '<>c'

  .class /*02000039*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__53'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000AA:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000060*/ public int32 '<>1__state'
    .field /*04000061*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000070*/ '<>t__builder'
    .field /*04000062*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ '<>4__this'
    .field /*04000063*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .method /*0600013E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__53'::MoveNext

    .method /*0600013F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C000149:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__53'::SetStateMachine

  } // end of class '<DisposeAsync>d__53'

  .class /*0200003A*/ auto ansi sealed nested private beforefieldinit '<DoReceive>d__46'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000AC:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*04000064*/ public int32 '<>1__state'
    .field /*04000065*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002D*/ '<>t__builder'
    .field /*04000066*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ '<>4__this'
    .field /*04000067*/ private class [System.Runtime/*23000001*/]System.Exception/*01000028*/ '<error>5__2'
    .field /*04000068*/ private class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000022*/ '<input>5__3'
    .field /*04000069*/ private bool '<paused>5__4'
    .field /*0400006A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100006E*/<int32> '<>u__1'
    .field /*0400006B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100006E*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.FlushResult/*0100003E*/> '<>u__2'
    .method /*06000140*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DoReceive>d__46'::MoveNext

    .method /*06000141*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C00014A:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DoReceive>d__46'::SetStateMachine

  } // end of class '<DoReceive>d__46'

  .class /*0200003B*/ auto ansi sealed nested private beforefieldinit '<DoSend>d__50'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000AD:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400006C*/ public int32 '<>1__state'
    .field /*0400006D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002D*/ '<>t__builder'
    .field /*0400006E*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ '<>4__this'
    .field /*0400006F*/ private class [System.Runtime/*23000001*/]System.Exception/*01000028*/ '<shutdownReason>5__2'
    .field /*04000070*/ private class [System.Runtime/*23000001*/]System.Exception/*01000028*/ '<unexpectedError>5__3'
    .field /*04000071*/ private class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ '<sendCompletedTask>5__4'
    .field /*04000072*/ private object '<>7__wrap4'
    .field /*04000073*/ private int32 '<>7__wrap5'
    .field /*04000074*/ private class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*01000021*/ '<output>5__7'
    .field /*04000075*/ private valuetype [System.Memory/*23000003*/]System.SequencePosition/*01000039*/ '<end>5__8'
    .field /*04000076*/ private bool '<isCompleted>5__9'
    .field /*04000077*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100006E*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.ReadResult/*0100003C*/> '<>u__1'
    .field /*04000078*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000066*/ '<>u__2'
    .field /*04000079*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__3'
    .method /*06000142*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DoSend>d__50'::MoveNext

    .method /*06000143*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C00014B:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DoSend>d__50'::SetStateMachine

  } // end of class '<DoSend>d__50'

  .class /*0200003C*/ auto ansi sealed nested private beforefieldinit '<StartAsync>d__44'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000AE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400007A*/ public int32 '<>1__state'
    .field /*0400007B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002D*/ '<>t__builder'
    .field /*0400007C*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ '<>4__this'
    .field /*0400007D*/ private class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ '<sendTask>5__2'
    .field /*0400007E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .method /*06000144*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartAsync>d__44'::MoveNext

    .method /*06000145*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C00014C:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartAsync>d__44'::SetStateMachine

  } // end of class '<StartAsync>d__44'

  .class /*0200003D*/ auto ansi sealed nested private beforefieldinit '<WaitForWritesCompleted>d__45'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/
  {
    .custom /*0C0000AF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400007F*/ public int32 '<>1__state'
    .field /*04000080*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002D*/ '<>t__builder'
    .field /*04000081*/ public class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ '<>4__this'
    .field /*04000082*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000066*/ '<>u__1'
    .method /*06000146*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::MoveNext /*0100002C::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WaitForWritesCompleted>d__45'::MoveNext

    .method /*06000147*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B1
    {
      .custom /*0C00014D:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002C*/::SetStateMachine /*0100002C::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WaitForWritesCompleted>d__45'::SetStateMachine

  } // end of class '<WaitForWritesCompleted>d__45'

  .field /*0400001D*/ assembly class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ _processingTask
  .method /*060000DD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicConnectionContext/*0200001F*/ connection,
                               class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTransportContext/*02000023*/ context) cil managed
  // SIG: 20 02 01 12 7C 12 80 8C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::.ctor

  .method /*060000DE*/ public hidebysig specialname virtual 
          instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> 
          get_MemoryPool() cil managed
  // SIG: 20 00 15 12 80 99 01 05
  {
    .custom /*0C00011C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::get_MemoryPool

  .method /*060000DF*/ public hidebysig specialname 
          instance bool  get_CanReuse() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00011D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::get_CanReuse

  .method /*060000E0*/ public hidebysig instance void 
          Initialize(class [System.Net.Quic/*2300000E*/]System.Net.Quic.QuicStream/*0100006F*/ 'stream') cil managed
  // SIG: 20 01 01 12 81 BD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::Initialize

  .method /*060000E1*/ public hidebysig specialname virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::get_ConnectionId

  .method /*060000E2*/ public hidebysig specialname virtual 
          instance void  set_ConnectionId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::set_ConnectionId

  .method /*060000E3*/ public hidebysig newslot specialname virtual final 
          instance int64  get_PoolExpirationTicks() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C00011E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::get_PoolExpirationTicks

  .method /*060000E4*/ public hidebysig specialname 
          instance void  set_PoolExpirationTicks(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C00011F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::set_PoolExpirationTicks

  .method /*060000E5*/ public hidebysig instance void 
          Start() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::Start

  .method /*060000E6*/ public hidebysig virtual 
          instance void  Abort(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionAbortedException/*01000058*/ abortReason) cil managed
  // SIG: 20 01 01 12 81 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::Abort

  .method /*060000E7*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003A*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 E9
  {
    .custom /*0C000120:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002A*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0A000018 */ = ( 01 00 61 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..aMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E 4B   // NetCore.Server.K
                                                                                                                                                                                                                                        65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72 74   // estrel.Transport
                                                                                                                                                                                                                                        2E 51 75 69 63 2E 49 6E 74 65 72 6E 61 6C 2E 51   // .Quic.Internal.Q
                                                                                                                                                                                                                                        75 69 63 53 74 72 65 61 6D 43 6F 6E 74 65 78 74   // uicStreamContext
                                                                                                                                                                                                                                        2B 3C 44 69 73 70 6F 73 65 41 73 79 6E 63 3E 64   // +<DisposeAsync>d
                                                                                                                                                                                                                                        5F 5F 35 33 00 00 )                               // __53..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::DisposeAsync

  .method /*060000E8*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::Dispose

  .method /*060000E9*/ public hidebysig newslot virtual final 
          instance void  DisposeCore() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::DisposeCore

  .method /*060000EA*/ public hidebysig newslot specialname virtual final 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000121:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::get_CanRead

  .method /*060000EB*/ public hidebysig newslot specialname virtual final 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000122:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::get_CanWrite

  .method /*060000EC*/ public hidebysig newslot specialname virtual final 
          instance int64  get_Error() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::get_Error

  .method /*060000ED*/ public hidebysig newslot specialname virtual final 
          instance void  set_Error(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::set_Error

  .method /*060000EE*/ public hidebysig newslot specialname virtual final 
          instance int64  get_StreamId() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000123:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::get_StreamId

  .method /*060000EF*/ private hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> 
          Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature.get_State() cil managed
  // SIG: 20 00 15 12 81 39 02 1C 1C
  {
    .override [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature/*01000051*/::get_State /*01000051::0A000032*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature.get_State

  .method /*060000F0*/ public hidebysig newslot virtual final 
          instance void  AbortRead(int64 errorCode,
                                   class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionAbortedException/*01000058*/ abortReason) cil managed
  // SIG: 20 02 01 0A 12 81 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::AbortRead

  .method /*060000F1*/ public hidebysig newslot virtual final 
          instance void  AbortWrite(int64 errorCode,
                                    class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionAbortedException/*01000058*/ abortReason) cil managed
  // SIG: 20 02 01 0A 12 81 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::AbortWrite

  .method /*060000F2*/ assembly hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100003B*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.FlushResult/*0100003E*/> 
          '<DoReceive>g__AwaitCompleteTaskAsync|46_0'(valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003A*/ completeTask) cil managed
  // SIG: 00 01 15 11 80 ED 01 11 80 F9 11 80 E9
  {
    .custom /*0C000125:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000126:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002A*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000029*/) /* 0A000018 */ = ( 01 00 7A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..zMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 65 72 76 65 72 2E 4B   // NetCore.Server.K
                                                                                                                                                                                                                                        65 73 74 72 65 6C 2E 54 72 61 6E 73 70 6F 72 74   // estrel.Transport
                                                                                                                                                                                                                                        2E 51 75 69 63 2E 49 6E 74 65 72 6E 61 6C 2E 51   // .Quic.Internal.Q
                                                                                                                                                                                                                                        75 69 63 53 74 72 65 61 6D 43 6F 6E 74 65 78 74   // uicStreamContext
                                                                                                                                                                                                                                        2B 3C 3C 44 6F 52 65 63 65 69 76 65 3E 67 5F 5F   // +<<DoReceive>g__
                                                                                                                                                                                                                                        41 77 61 69 74 43 6F 6D 70 6C 65 74 65 54 61 73   // AwaitCompleteTas
                                                                                                                                                                                                                                        6B 41 73 79 6E 63 7C 34 36 5F 30 3E 64 00 00 )    // kAsync|46_0>d..
    .custom /*0C000127:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicStreamContext::'<DoReceive>g__AwaitCompleteTaskAsync|46_0'

  .property /*1700003D*/ instance callconv(8) class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8>
          MemoryPool()
  {
    .get instance class [System.Memory/*23000003*/]System.Buffers.MemoryPool`1/*01000026*/<uint8> Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::get_MemoryPool() /* 060000DE */
  } // end of property QuicStreamContext::MemoryPool
  .property /*1700003E*/ instance callconv(8) bool
          CanReuse()
  {
    .get instance bool Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::get_CanReuse() /* 060000DF */
  } // end of property QuicStreamContext::CanReuse
  .property /*1700003F*/ instance callconv(8) string
          ConnectionId()
  {
    .get instance string Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::get_ConnectionId() /* 060000E1 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::set_ConnectionId(string) /* 060000E2 */
  } // end of property QuicStreamContext::ConnectionId
  .property /*17000040*/ instance callconv(8) int64
          PoolExpirationTicks()
  {
    .get instance int64 Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::get_PoolExpirationTicks() /* 060000E3 */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::set_PoolExpirationTicks(int64) /* 060000E4 */
  } // end of property QuicStreamContext::PoolExpirationTicks
  .property /*17000041*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::get_CanRead() /* 060000EA */
  } // end of property QuicStreamContext::CanRead
  .property /*17000042*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::get_CanWrite() /* 060000EB */
  } // end of property QuicStreamContext::CanWrite
  .property /*17000043*/ instance callconv(8) int64
          Error()
  {
    .get instance int64 Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::get_Error() /* 060000EC */
    .set instance void Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::set_Error(int64) /* 060000ED */
  } // end of property QuicStreamContext::Error
  .property /*17000044*/ instance callconv(8) int64
          StreamId()
  {
    .get instance int64 Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::get_StreamId() /* 060000EE */
  } // end of property QuicStreamContext::StreamId
  .property /*17000045*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object>
          Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature.State()
  {
    .custom /*0C0000B6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<object,object> Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/::Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature.get_State() /* 060000EF */
  } // end of property QuicStreamContext::Microsoft.AspNetCore.Connections.Features.IPersistentStateFeature.State
} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext

.class /*02000022*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTrace
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
       implements Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace/*0200001C*/,
                  [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILogger/*01000040*/
{
  .custom /*0C000083:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace/*0200001C*/
  .custom /*0C000085:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILogger/*01000040*/
  .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003E*/ auto ansi sealed nested private StreamType
         extends [System.Runtime/*23000001*/]System.Enum/*01000038*/
  {
    .field /*04000083*/ public specialname rtspecialname int32 value__
    .field /*04000084*/ public static literal valuetype Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTrace/*02000022*//StreamType/*0200003E*/ Unidirectional = int32(0x00000000)
    .field /*04000085*/ public static literal valuetype Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTrace/*02000022*//StreamType/*0200003E*/ Bidirectional = int32(0x00000001)
  } // end of class StreamType

  .method /*060000F3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.ILogger/*01000040*/ logger) cil managed
  // SIG: 20 01 01 12 81 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::.ctor

  .method /*060000F4*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.IDisposable/*01000037*/ 
          BeginScope<TState>(!!TState state) cil managed
  // SIG: 30 01 01 12 80 DD 1E 00
  {
    .param type TState /*2A000008*/ 
      .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::BeginScope

  .method /*060000F5*/ public hidebysig newslot virtual final 
          instance bool  IsEnabled(valuetype [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.LogLevel/*01000073*/ logLevel) cil managed
  // SIG: 20 01 02 11 81 CD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::IsEnabled

  .method /*060000F6*/ public hidebysig newslot virtual final 
          instance void  Log<TState>(valuetype [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.LogLevel/*01000073*/ logLevel,
                                     valuetype [Microsoft.Extensions.Logging.Abstractions/*23000005*/]Microsoft.Extensions.Logging.EventId/*01000074*/ eventId,
                                     !!TState state,
                                     class [System.Runtime/*23000001*/]System.Exception/*01000028*/ exception,
                                     class [System.Runtime/*23000001*/]System.Func`3/*01000075*/<!!TState,class [System.Runtime/*23000001*/]System.Exception/*01000028*/,string> formatter) cil managed
  // SIG: 30 01 05 01 11 81 CD 11 81 D1 1E 00 12 80 A1 15 12 81 D5 03 1E 00 12 80 A1 0E
  {
    .custom /*0C000128:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000CE*/ 
    .custom /*0C000113:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*080000D0*/ 
    .custom /*0C000115:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::Log

  .method /*060000F7*/ public hidebysig newslot virtual final 
          instance void  AcceptedConnection(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.BaseConnectionContext/*0100006A*/ connection) cil managed
  // SIG: 20 01 01 12 81 A9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::AcceptedConnection

  .method /*060000F8*/ public hidebysig newslot virtual final 
          instance void  AcceptedStream(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext) cil managed
  // SIG: 20 01 01 12 80 84
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::AcceptedStream

  .method /*060000F9*/ public hidebysig newslot virtual final 
          instance void  ConnectedStream(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext) cil managed
  // SIG: 20 01 01 12 80 84
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::ConnectedStream

  .method /*060000FA*/ public hidebysig newslot virtual final 
          instance void  ConnectionError(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.BaseConnectionContext/*0100006A*/ connection,
                                         class [System.Runtime/*23000001*/]System.Exception/*01000028*/ ex) cil managed
  // SIG: 20 02 01 12 81 A9 12 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::ConnectionError

  .method /*060000FB*/ public hidebysig newslot virtual final 
          instance void  ConnectionAborted(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.BaseConnectionContext/*0100006A*/ connection,
                                           int64 errorCode,
                                           class [System.Runtime/*23000001*/]System.Exception/*01000028*/ ex) cil managed
  // SIG: 20 03 01 12 81 A9 0A 12 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::ConnectionAborted

  .method /*060000FC*/ public hidebysig newslot virtual final 
          instance void  ConnectionAbort(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.BaseConnectionContext/*0100006A*/ connection,
                                         int64 errorCode,
                                         string reason) cil managed
  // SIG: 20 03 01 12 81 A9 0A 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::ConnectionAbort

  .method /*060000FD*/ public hidebysig newslot virtual final 
          instance void  StreamError(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                     class [System.Runtime/*23000001*/]System.Exception/*01000028*/ ex) cil managed
  // SIG: 20 02 01 12 80 84 12 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamError

  .method /*060000FE*/ public hidebysig newslot virtual final 
          instance void  StreamPause(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext) cil managed
  // SIG: 20 01 01 12 80 84
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamPause

  .method /*060000FF*/ public hidebysig newslot virtual final 
          instance void  StreamResume(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext) cil managed
  // SIG: 20 01 01 12 80 84
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamResume

  .method /*06000100*/ public hidebysig newslot virtual final 
          instance void  StreamShutdownWrite(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                             string reason) cil managed
  // SIG: 20 02 01 12 80 84 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamShutdownWrite

  .method /*06000101*/ public hidebysig newslot virtual final 
          instance void  StreamAbortedRead(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                           int64 errorCode) cil managed
  // SIG: 20 02 01 12 80 84 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamAbortedRead

  .method /*06000102*/ public hidebysig newslot virtual final 
          instance void  StreamAbortedWrite(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                            int64 errorCode) cil managed
  // SIG: 20 02 01 12 80 84 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamAbortedWrite

  .method /*06000103*/ public hidebysig newslot virtual final 
          instance void  StreamAbort(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                     int64 errorCode,
                                     string reason) cil managed
  // SIG: 20 03 01 12 80 84 0A 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamAbort

  .method /*06000104*/ public hidebysig newslot virtual final 
          instance void  StreamAbortRead(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                         int64 errorCode,
                                         string reason) cil managed
  // SIG: 20 03 01 12 80 84 0A 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamAbortRead

  .method /*06000105*/ public hidebysig newslot virtual final 
          instance void  StreamAbortWrite(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicStreamContext/*02000021*/ streamContext,
                                          int64 errorCode,
                                          string reason) cil managed
  // SIG: 20 03 01 12 80 84 0A 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTrace::StreamAbortWrite

} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTrace

.class /*02000023*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTransportContext
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C000086:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000087:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000106*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace/*0200001C*/ log,
                               class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/ options) cil managed
  // SIG: 20 02 01 12 70 12 6C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportContext::.ctor

  .method /*06000107*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace/*0200001C*/ 
          get_Log() cil managed
  // SIG: 20 00 12 70
  {
    .custom /*0C00012B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportContext::get_Log

  .method /*06000108*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/ 
          get_Options() cil managed
  // SIG: 20 00 12 6C
  {
    .custom /*0C00012C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QuicTransportContext::get_Options

  .property /*17000046*/ instance callconv(8) class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace/*0200001C*/
          Log()
  {
    .get instance class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.IQuicTrace/*0200001C*/ Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTransportContext/*02000023*/::get_Log() /* 06000107 */
  } // end of property QuicTransportContext::Log
  .property /*17000047*/ instance callconv(8) class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/
          Options()
  {
    .get instance class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.QuicTransportOptions/*0200001B*/ Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTransportContext/*02000023*/::get_Options() /* 06000108 */
  } // end of property QuicTransportContext::Options
} // end of class Microsoft.AspNetCore.Server.Kestrel.Transport.Quic.Internal.QuicTransportContext

.class /*02000024*/ private auto ansi sealed '<PrivateImplementationDetails>'
       extends [System.Runtime/*23000001*/]System.Object/*0100001B*/
{
  .custom /*0C000089:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000017*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .class /*0200003F*/ explicit ansi sealed nested private '__StaticArrayInitTypeSize=16'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000025*/
  {
    .pack 1
    .size 16
  } // end of class '__StaticArrayInitTypeSize=16'

  .field /*0400001E*/ static assembly initonly valuetype '<PrivateImplementationDetails>'/*02000024*//'__StaticArrayInitTypeSize=16'/*0200003F*/ '2125B2C332B1113AAE9BFC5E9F7E3B4C91D828CB942C2DF1EEB02502ECCAE9E9' at I_0000B3BC
} // end of class '<PrivateImplementationDetails>'


// =============================================================

.data cil I_0000B3BC = bytearray (
                 30 31 32 33 34 35 36 37 38 39 41 42 43 44 45 46) // 0123456789ABCDEF
// *********** DISASSEMBLY COMPLETE ***********************
