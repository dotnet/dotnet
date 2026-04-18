


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
// Time-date stamp:            0xb2147e69
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00008600
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000c56a
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
// Size of image:                  0x00012000
// Size of headers:                0x00000400
// Checksum:                       0x00016cf9
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000c518 [0x0000004f] address [size] of Import Directory:          
// 0x0000e000 [0x00000568] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00009400 [0x00002470] address [size] of Security Directory:        
// 0x00010000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000c4fc [0x0000001c] address [size] of Debug Directory:           
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
//              0x00008578 Virtual Size
//              0x00004000 Virtual Address
//              0x00008600 Size of Raw Data
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
//              0x00000568 Virtual Size
//              0x0000e000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00008c00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00010000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00009200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000c000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x0000056c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x0000c55a Import Name Table
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
// 0x00004054 [0x00008428] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000c47c [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00003ebc Size
//              '#~' Name
//    Stream 2:
//              0x00003f28 Offset
//              0x00002ef0 Size
//              '#Strings' Name
//    Stream 3:
//              0x00006e18 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00006e1c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00006e2c Offset
//              0x000015fc Size
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

// File size            : 47216
// PE header size       : 1024 (536 used)    ( 2.17%)
// PE additional info   : 10911             (23.11%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.15%)
// CLR meta-data size  : 33832              (71.65%)
// CLR additional info : 128                ( 0.27%)
// CLR method headers  : 372                ( 0.79%)
// Managed code         : 744               ( 1.58%)
// Data                 : 2560              ( 5.42%)
// Unaccounted          : -2427             (-5.14%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 34304
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 33832
//   Module        -    1 (10 bytes)
//   TypeDef       -   91 (1274 bytes)    3 interfaces, 0 explicit layout
//   TypeRef       -  134 (804 bytes)
//   MethodDef     -  376 (5264 bytes)    4 abstract, 0 native, 372 bodies
//   FieldDef      -  247 (1482 bytes)    8 constant
//   MemberRef     -   38 (228 bytes)
//   ParamDef      -  377 (2262 bytes)
//   MethodImpl    -   64 (384 bytes)
//   Constant      -   13 (78 bytes)
//   CustomAttribute-  342 (2052 bytes)
//   ClassLayout   -    2 (16 bytes)
//   InterfaceImpl -   60 (240 bytes)
//   PropertyMap   -   18 (72 bytes)
//   Property      -   73 (438 bytes)
//   MethodSemantic-  109 (654 bytes)
//   Security      -    1 (6 bytes)
//   TypeSpec      -    6 (12 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   20 (400 bytes)
//   NestedClass   -   47 (188 bytes)
//   FieldRVA      -    1 (6 bytes)
//   GenericParam  -    4 (32 bytes)
//   GenericParamConstraint-    4 (16 bytes)
//   Strings       - 12015 bytes
//   Blobs         -  5628 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   229 bytes

// CLR additional info : 128

// CLR method headers : 372
//   Num.of method bodies  - 372
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 372

// Managed code : 744
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class WebSocketExtensions            (abstract) (auto) (ansi) (sealed) 
// Class StreamExtensions               (abstract) (auto) (ansi) (sealed) 
// Class PipeWriterStream               (auto) (ansi) 
// Class DuplexPipe                     (auto) (ansi) 
// Class TaskExtensions                 (abstract) (auto) (ansi) (sealed) 
// Class NoThrowAwaiter                 (sequential) (ansi) (sealed) 
// Class EncoderResources               (abstract) (auto) (ansi) (sealed) 
// Class SecurityHelper                 (abstract) (auto) (ansi) (sealed) 
// Class WebEncoders                    (abstract) (auto) (ansi) (sealed) 
// Class NonCapturingTimer              (abstract) (auto) (ansi) (sealed) 
// Class ValueStopwatch                 (sequential) (ansi) (sealed) 
// Class ConnectionsDependencyInjectionExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class AwaitableThreadPool            (abstract) (auto) (ansi) (sealed) 
// Class MemoryBufferWriter             (auto) (ansi) (sealed) 
// Class TaskCache                      (abstract) (auto) (ansi) (sealed) 
// Class ValueTaskExtensions            (abstract) (auto) (ansi) (sealed) 
// Class ConnectionOptions              (public) (auto) (ansi) 
// Class ConnectionOptionsSetup         (public) (auto) (ansi) 
// Class HttpConnectionContextExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class HttpConnectionDispatcherOptions (public) (auto) (ansi) 
// Class LongPollingOptions             (public) (auto) (ansi) 
// Class NegotiateMetadata              (public) (auto) (ansi) 
// Class ServerSentEventsMessageFormatter (abstract) (auto) (ansi) (sealed) 
// Class WebSocketOptions               (public) (auto) (ansi) 
// Class ConnectionLogScope             (auto) (ansi) 
// Class HttpConnectionContext          (auto) (ansi) 
// Class HttpConnectionDispatcher       (auto) (ansi) 
// Class HttpConnectionManager          (auto) (ansi) 
// Class HttpConnectionsEventSource     (auto) (ansi) 
// Class HttpConnectionStatus           (auto) (ansi) (sealed) 
// Interface IHttpTransport                 (abstract) (auto) (ansi) 
// Class LongPollingServerTransport     (auto) (ansi) 
// Class ServerSentEventsServerTransport (auto) (ansi) 
// Class WebSocketsServerTransport      (auto) (ansi) 
// Interface IHttpContextFeature            (public) (abstract) (auto) (ansi) 
// Interface IHttpTransportFeature          (public) (abstract) (auto) (ansi) 
// Class ConnectionEndpointRouteBuilder (public) (auto) (ansi) (sealed) 
// Class ConnectionEndpointRouteBuilderExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class <PrivateImplementationDetails> (auto) (ansi) (sealed) 
// Class <SendMultiSegmentAsync>d__1    (auto) (ansi) (sealed) (nested private) 
// Class <WriteMultiSegmentAsync>d__1   (auto) (ansi) (sealed) (nested private) 
// Class <<WriteCoreAsync>g__WriteSlowAsync|21_0>d (auto) (ansi) (sealed) (nested private) 
// Class DuplexPipePair                 (sequential) (ansi) (sealed) (nested public) 
// Class <NoThrow>d__0                  (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class Awaitable                      (sequential) (ansi) (sealed) (nested public) 
// Class CompletedBuffer                (sequential) (ansi) (sealed) (nested private) 
// Class <CopyToSlowAsync>d__28         (auto) (ansi) (sealed) (nested private) 
// Class <WriteMessageAsync>d__3        (auto) (ansi) (sealed) (nested private) 
// Class <WriteMessageToMemory>d__5     (auto) (ansi) (sealed) (nested private) 
// Class <GetEnumerator>d__10           (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <CancelPreviousPoll>d__127     (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__122           (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteApplication>d__129     (auto) (ansi) (sealed) (nested private) 
// Class <WaitOnTasks>d__123            (auto) (ansi) (sealed) (nested private) 
// Class EmptyServiceProvider           (auto) (ansi) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested assembly) 
// Class <DoPersistentConnection>d__11  (auto) (ansi) (sealed) (nested private) 
// Class <EnsureConnectionStateAsync>d__18 (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__10            (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__8             (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteNegotiateAsync>d__9    (auto) (ansi) (sealed) (nested private) 
// Class <GetConnectionAsync>d__22      (auto) (ansi) (sealed) (nested private) 
// Class <GetOrCreateConnectionAsync>d__23 (auto) (ansi) (sealed) (nested private) 
// Class <ProcessDeleteAsync>d__17      (auto) (ansi) (sealed) (nested private) 
// Class <ProcessNegotiate>d__12        (auto) (ansi) (sealed) (nested private) 
// Class <ProcessSend>d__16             (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAndRemoveAsync>d__16   (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteTimerLoop>d__13        (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <ProcessRequestAsync>d__6      (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <ProcessRequestAsync>d__6      (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <ProcessRequestAsync>d__6      (auto) (ansi) (sealed) (nested private) 
// Class <ProcessSocketAsync>d__7       (auto) (ansi) (sealed) (nested private) 
// Class <StartReceiving>d__8           (auto) (ansi) (sealed) (nested private) 
// Class <StartSending>d__9             (auto) (ansi) (sealed) (nested private) 
// Class CompositeEndpointConventionBuilder (auto) (ansi) (nested private) 
// Class <>c__2`1                       (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass2_0`1         (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass3_0           (auto) (ansi) (sealed) (nested private) 
// Class __StaticArrayInitTypeSize=6    (explicit) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Net.WebSockets
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Memory
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.IO.Pipelines
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ System.Security.Claims
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ Microsoft.AspNetCore.Http.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ Microsoft.AspNetCore.Connections.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000A*/ Microsoft.AspNetCore.Metadata
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000B*/ Microsoft.AspNetCore.Http.Connections.Common
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000C*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000D*/ System.Threading
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000E*/ Microsoft.Extensions.Features
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000F*/ System.Threading.ThreadPool
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000010*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000011*/ Microsoft.Extensions.Hosting.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000012*/ System.Diagnostics.Tracing
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000013*/ Microsoft.AspNetCore.Routing
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000014*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Http.Connections
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

  .custom /*0C000009:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 81 77 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...wMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F   // pNetCore.Http.Co
                                                                                                                                                                                    6E 6E 65 63 74 69 6F 6E 73 2E 54 65 73 74 73 2C   // nnections.Tests,
                                                                                                                                                                                    20 50 75 62 6C 69 63 4B 65 79 3D 30 30 32 34 30   //  PublicKey=00240
                                                                                                                                                                                    30 30 30 30 34 38 30 30 30 30 30 39 34 30 30 30   // 0000480000094000
                                                                                                                                                                                    30 30 30 30 36 30 32 30 30 30 30 30 30 32 34 30   // 0000602000000240
                                                                                                                                                                                    30 30 30 35 32 35 33 34 31 33 31 30 30 30 34 30   // 0005253413100040
                                                                                                                                                                                    30 30 30 30 31 30 30 30 31 30 30 66 33 33 61 32   // 00001000100f33a2
                                                                                                                                                                                    39 30 34 34 66 61 39 64 37 34 30 63 39 62 33 32   // 9044fa9d740c9b32
                                                                                                                                                                                    31 33 61 39 33 65 35 37 63 38 34 62 34 37 32 63   // 13a93e57c84b472c
                                                                                                                                                                                    38 34 65 30 62 38 61 30 65 31 61 65 34 38 65 36   // 84e0b8a0e1ae48e6
                                                                                                                                                                                    37 61 39 66 38 66 36 64 65 39 64 35 66 37 66 33   // 7a9f8f6de9d5f7f3
                                                                                                                                                                                    64 35 32 61 63 32 33 65 34 38 61 63 35 31 38 30   // d52ac23e48ac5180
                                                                                                                                                                                    31 66 31 64 63 39 35 30 61 62 65 39 30 31 64 61   // 1f1dc950abe901da
                                                                                                                                                                                    33 34 64 32 61 39 65 33 62 61 61 64 62 31 34 31   // 34d2a9e3baadb141
                                                                                                                                                                                    61 31 37 63 37 37 65 66 33 63 35 36 35 64 64 35   // a17c77ef3c565dd5
                                                                                                                                                                                    65 65 35 30 35 34 62 39 31 63 66 36 33 62 62 33   // ee5054b91cf63bb3
                                                                                                                                                                                    63 36 61 62 38 33 66 37 32 61 62 33 61 61 66 65   // c6ab83f72ab3aafe
                                                                                                                                                                                    39 33 64 30 66 63 33 63 32 33 34 38 62 37 36 34   // 93d0fc3c2348b764
                                                                                                                                                                                    66 61 66 62 30 62 31 63 30 37 33 33 64 65 35 31   // fafb0b1c0733de51
                                                                                                                                                                                    34 35 39 61 65 61 62 34 36 35 38 30 33 38 34 62   // 459aeab46580384b
                                                                                                                                                                                    66 39 64 37 34 63 34 65 32 38 31 36 34 62 37 63   // f9d74c4e28164b7c
                                                                                                                                                                                    64 65 32 34 37 66 38 39 31 62 61 30 37 38 39 31   // de247f891ba07891
                                                                                                                                                                                    63 39 64 38 37 32 61 64 32 62 62 00 00 )          // c9d872ad2bb..
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
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 4F 43 6F 6D 70 6F 6E 65 6E 74 73 20 66 6F   // ..OComponents fo
                                                                                                                                                                       72 20 70 72 6F 76 69 64 69 6E 67 20 72 65 61 6C   // r providing real
                                                                                                                                                                       2D 74 69 6D 65 20 62 69 2D 64 69 72 65 63 74 69   // -time bi-directi
                                                                                                                                                                       6F 6E 61 6C 20 63 6F 6D 6D 75 6E 69 63 61 74 69   // onal communicati
                                                                                                                                                                       6F 6E 20 61 63 72 6F 73 73 20 74 68 65 20 57 65   // on across the We
                                                                                                                                                                       62 2E 00 00 )                                     // b...
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 25 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..%Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                 6E 65 63 74 69 6F 6E 73 00 00 )                   // nections..
  .custom /*0C000017:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
  .permissionset reqmin
             = {[System.Runtime/*23000001*/]System.Security.Permissions.SecurityPermissionAttribute/*01000014*/ = {property bool 'SkipVerification' = bool(true)}}
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
.module Microsoft.AspNetCore.Http.Connections.dll
// MVID: {1d851fdc-845d-4caa-a05c-f26c080cb57b}
.custom /*0C000003:0A000013*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
.custom /*0C000004:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000019:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00001B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit System.Net.WebSockets.WebSocketExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00002A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200002D*/ auto ansi sealed nested private beforefieldinit '<SendMultiSegmentAsync>d__1'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C000080:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000012*/ public int32 '<>1__state'
    .field /*04000013*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000022*/ '<>t__builder'
    .field /*04000014*/ public valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> buffer
    .field /*04000015*/ public class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*0100001C*/ webSocket
    .field /*04000016*/ public valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketMessageType/*0100001E*/ webSocketMessageType
    .field /*04000017*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken
    .field /*04000018*/ private valuetype [System.Memory/*23000003*/]System.SequencePosition/*01000023*/ '<position>5__2'
    .field /*04000019*/ private valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000024*/<uint8> '<segment>5__3'
    .field /*0400001A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000025*/ '<>u__1'
    .method /*060000E2*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SendMultiSegmentAsync>d__1'::MoveNext

    .method /*060000E3*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00012E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SendMultiSegmentAsync>d__1'::SetStateMachine

  } // end of class '<SendMultiSegmentAsync>d__1'

  .method /*06000006*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001B*/ 
          SendAsync(class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*0100001C*/ webSocket,
                    valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> buffer,
                    valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketMessageType/*0100001E*/ webSocketMessageType,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken) cil managed
  // SIG: 00 04 11 6D 12 71 15 11 75 01 05 11 79 11 7D
  {
    .custom /*0C000029:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000001*/ 
    .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000004*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketExtensions::SendAsync

} // end of class System.Net.WebSockets.WebSocketExtensions

.class /*02000007*/ private abstract auto ansi sealed beforefieldinit System.IO.StreamExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00002D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200002E*/ auto ansi sealed nested private beforefieldinit '<WriteMultiSegmentAsync>d__1'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C000085:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400001B*/ public int32 '<>1__state'
    .field /*0400001C*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000022*/ '<>t__builder'
    .field /*0400001D*/ public valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> buffer
    .field /*0400001E*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ 'stream'
    .field /*0400001F*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken
    .field /*04000020*/ private valuetype [System.Memory/*23000003*/]System.SequencePosition/*01000023*/ '<position>5__2'
    .field /*04000021*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000025*/ '<>u__1'
    .method /*060000E4*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteMultiSegmentAsync>d__1'::MoveNext

    .method /*060000E5*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00012F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteMultiSegmentAsync>d__1'::SetStateMachine

  } // end of class '<WriteMultiSegmentAsync>d__1'

  .method /*06000007*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001B*/ 
          WriteAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ 'stream',
                     valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken) cil managed
  // SIG: 00 03 11 6D 12 80 9D 15 11 75 01 05 11 7D
  {
    .custom /*0C00002C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000005*/ 
    .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000007*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamExtensions::WriteAsync

} // end of class System.IO.StreamExtensions

.class /*02000008*/ private auto ansi beforefieldinit System.IO.Pipelines.PipeWriterStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002F*/ auto ansi sealed nested private beforefieldinit '<<WriteCoreAsync>g__WriteSlowAsync|21_0>d'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C000088:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000022*/ public int32 '<>1__state'
    .field /*04000023*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000022*/ '<>t__builder'
    .field /*04000024*/ public valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100002D*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.FlushResult/*0100002E*/> flushTask
    .field /*04000025*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002F*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.FlushResult/*0100002E*/> '<>u__1'
    .method /*060000E6*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<WriteCoreAsync>g__WriteSlowAsync|21_0>d'::MoveNext

    .method /*060000E7*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000130:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<WriteCoreAsync>g__WriteSlowAsync|21_0>d'::SetStateMachine

  } // end of class '<<WriteCoreAsync>g__WriteSlowAsync|21_0>d'

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeWriter/*01000028*/ pipeWriter) cil managed
  // SIG: 20 01 01 12 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::.ctor

  .method /*06000009*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::get_CanRead

  .method /*0600000A*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::get_CanSeek

  .method /*0600000B*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::get_CanWrite

  .method /*0600000C*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::get_Length

  .method /*0600000D*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::get_Position

  .method /*0600000E*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::set_Position

  .method /*0600000F*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::Flush

  .method /*06000010*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::Read

  .method /*06000011*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000029*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 A5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::Seek

  .method /*06000012*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::SetLength

  .method /*06000013*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::Write

  .method /*06000014*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 1D 05 08 08 11 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::WriteAsync

  .method /*06000015*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001B*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000024*/<uint8> source,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken) cil managed
  // SIG: 20 02 11 6D 15 11 80 91 01 05 11 7D
  {
    .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000018*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::WriteAsync

  .method /*06000016*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::Reset

  .method /*06000017*/ assembly hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001B*/ 
          '<WriteCoreAsync>g__WriteSlowAsync|21_0'(valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100002D*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.FlushResult/*0100002E*/> flushTask) cil managed
  // SIG: 00 01 11 6D 15 11 80 B5 01 11 80 B9
  {
    .custom /*0C000051:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000052:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 4E 53 79 73 74 65 6D 2E 49 4F 2E 50 69 70   // ..NSystem.IO.Pip
                                                                                                                                                                                                                                        65 6C 69 6E 65 73 2E 50 69 70 65 57 72 69 74 65   // elines.PipeWrite
                                                                                                                                                                                                                                        72 53 74 72 65 61 6D 2B 3C 3C 57 72 69 74 65 43   // rStream+<<WriteC
                                                                                                                                                                                                                                        6F 72 65 41 73 79 6E 63 3E 67 5F 5F 57 72 69 74   // oreAsync>g__Writ
                                                                                                                                                                                                                                        65 53 6C 6F 77 41 73 79 6E 63 7C 32 31 5F 30 3E   // eSlowAsync|21_0>
                                                                                                                                                                                                                                        64 00 00 )                                        // d..
    .custom /*0C000053:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PipeWriterStream::'<WriteCoreAsync>g__WriteSlowAsync|21_0'

  .property /*17000001*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.IO.Pipelines.PipeWriterStream/*02000008*/::get_CanRead() /* 06000009 */
  } // end of property PipeWriterStream::CanRead
  .property /*17000002*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.IO.Pipelines.PipeWriterStream/*02000008*/::get_CanSeek() /* 0600000A */
  } // end of property PipeWriterStream::CanSeek
  .property /*17000003*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.IO.Pipelines.PipeWriterStream/*02000008*/::get_CanWrite() /* 0600000B */
  } // end of property PipeWriterStream::CanWrite
  .property /*17000004*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.IO.Pipelines.PipeWriterStream/*02000008*/::get_Length() /* 0600000C */
  } // end of property PipeWriterStream::Length
  .property /*17000005*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.IO.Pipelines.PipeWriterStream/*02000008*/::get_Position() /* 0600000D */
    .set instance void System.IO.Pipelines.PipeWriterStream/*02000008*/::set_Position(int64) /* 0600000E */
  } // end of property PipeWriterStream::Position
} // end of class System.IO.Pipelines.PipeWriterStream

.class /*02000009*/ private auto ansi beforefieldinit System.IO.Pipelines.DuplexPipe
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/
{
  .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/
  .custom /*0C000002:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000030*/ sequential ansi sealed nested public beforefieldinit DuplexPipePair
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
  {
    .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00008A:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000033*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    .field /*04000026*/ private initonly class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ '<Transport>k__BackingField'
    .custom /*0C000072:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000027*/ private initonly class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ '<Application>k__BackingField'
    .custom /*0C000076:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .method /*060000E8*/ public hidebysig specialname 
            instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ 
            get_Transport() cil managed
    // SIG: 20 00 12 80 C1
    {
      .custom /*0C000131:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DuplexPipePair::get_Transport

    .method /*060000E9*/ public hidebysig specialname 
            instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ 
            get_Application() cil managed
    // SIG: 20 00 12 80 C1
    {
      .custom /*0C000132:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DuplexPipePair::get_Application

    .method /*060000EA*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ transport,
                                 class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ application) cil managed
    // SIG: 20 02 01 12 80 C1 12 80 C1
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method DuplexPipePair::.ctor

    .property /*17000041*/ instance callconv(8) class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/
            Transport()
    {
      .get instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ System.IO.Pipelines.DuplexPipe/*02000009*//DuplexPipePair/*02000030*/::get_Transport() /* 060000E8 */
    } // end of property DuplexPipePair::Transport
    .property /*17000042*/ instance callconv(8) class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/
            Application()
    {
      .get instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ System.IO.Pipelines.DuplexPipe/*02000009*//DuplexPipePair/*02000030*/::get_Application() /* 060000E9 */
    } // end of property DuplexPipePair::Application
  } // end of class DuplexPipePair

  .method /*06000018*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeReader/*01000031*/ reader,
                               class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeWriter/*01000028*/ writer) cil managed
  // SIG: 20 02 01 12 80 C5 12 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DuplexPipe::.ctor

  .method /*06000019*/ public hidebysig newslot specialname virtual final 
          instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeReader/*01000031*/ 
          get_Input() cil managed
  // SIG: 20 00 12 80 C5
  {
    .custom /*0C000056:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DuplexPipe::get_Input

  .method /*0600001A*/ public hidebysig newslot specialname virtual final 
          instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeWriter/*01000028*/ 
          get_Output() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C000059:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DuplexPipe::get_Output

  .method /*0600001B*/ public hidebysig static 
          valuetype System.IO.Pipelines.DuplexPipe/*02000009*//DuplexPipePair/*02000030*/ 
          CreateConnectionPair(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeOptions/*01000032*/ inputOptions,
                               class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeOptions/*01000032*/ outputOptions) cil managed
  // SIG: 00 02 11 80 C0 12 80 C9 12 80 C9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DuplexPipe::CreateConnectionPair

  .property /*17000006*/ instance callconv(8) class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeReader/*01000031*/
          Input()
  {
    .get instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeReader/*01000031*/ System.IO.Pipelines.DuplexPipe/*02000009*/::get_Input() /* 06000019 */
  } // end of property DuplexPipe::Input
  .property /*17000007*/ instance callconv(8) class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeWriter/*01000028*/
          Output()
  {
    .get instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeWriter/*01000028*/ System.IO.Pipelines.DuplexPipe/*02000009*/::get_Output() /* 0600001A */
  } // end of property DuplexPipe::Output
} // end of class System.IO.Pipelines.DuplexPipe

.class /*0200000A*/ private abstract auto ansi sealed beforefieldinit System.Threading.Tasks.TaskExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000035:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000031*/ auto ansi sealed nested private beforefieldinit '<NoThrow>d__0'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C00008B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000028*/ public int32 '<>1__state'
    .field /*04000029*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*0400002A*/ public class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ task
    .field /*0400002B*/ private valuetype System.Threading.Tasks.NoThrowAwaiter/*0200000B*/ '<>u__1'
    .method /*060000EB*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<NoThrow>d__0'::MoveNext

    .method /*060000EC*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000133:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<NoThrow>d__0'::SetStateMachine

  } // end of class '<NoThrow>d__0'

  .method /*0600001C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          NoThrow(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ task) cil managed
  // SIG: 00 01 12 80 A9 12 80 A9
  {
    .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00005B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 33 53 79 73 74 65 6D 2E 54 68 72 65 61 64   // ..3System.Thread
                                                                                                                                                                                                                                        69 6E 67 2E 54 61 73 6B 73 2E 54 61 73 6B 45 78   // ing.Tasks.TaskEx
                                                                                                                                                                                                                                        74 65 6E 73 69 6F 6E 73 2B 3C 4E 6F 54 68 72 6F   // tensions+<NoThro
                                                                                                                                                                                                                                        77 3E 64 5F 5F 30 00 00 )                         // w>d__0..
    .custom /*0C00005C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TaskExtensions::NoThrow

} // end of class System.Threading.Tasks.TaskExtensions

.class /*0200000B*/ private sequential ansi sealed beforefieldinit System.Threading.Tasks.NoThrowAwaiter
       extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
       implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ICriticalNotifyCompletion/*01000035*/,
                  [System.Runtime/*23000001*/]System.Runtime.CompilerServices.INotifyCompletion/*01000036*/
{
  .custom /*0C000037:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000033*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .field /*04000004*/ private initonly class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ _task
  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ task) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .custom /*0C00005D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NoThrowAwaiter::.ctor

  .method /*0600001E*/ public hidebysig instance valuetype System.Threading.Tasks.NoThrowAwaiter/*0200000B*/ 
          GetAwaiter() cil managed
  // SIG: 20 00 11 2C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NoThrowAwaiter::GetAwaiter

  .method /*0600001F*/ public hidebysig specialname 
          instance bool  get_IsCompleted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NoThrowAwaiter::get_IsCompleted

  .method /*06000020*/ public hidebysig instance void 
          GetResult() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NoThrowAwaiter::GetResult

  .method /*06000021*/ public hidebysig newslot virtual final 
          instance void  OnCompleted(class [System.Runtime/*23000001*/]System.Action/*01000037*/ continuation) cil managed
  // SIG: 20 01 01 12 80 DD
  {
    .custom /*0C000066:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NoThrowAwaiter::OnCompleted

  .method /*06000022*/ public hidebysig newslot virtual final 
          instance void  UnsafeOnCompleted(class [System.Runtime/*23000001*/]System.Action/*01000037*/ continuation) cil managed
  // SIG: 20 01 01 12 80 DD
  {
    .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NoThrowAwaiter::UnsafeOnCompleted

  .property /*17000008*/ instance callconv(8) bool
          IsCompleted()
  {
    .get instance bool System.Threading.Tasks.NoThrowAwaiter/*0200000B*/::get_IsCompleted() /* 0600001F */
  } // end of property NoThrowAwaiter::IsCompleted
} // end of class System.Threading.Tasks.NoThrowAwaiter

.class /*0200000C*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.WebEncoders.Sources.EncoderResources
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000005*/ static assembly literal string WebEncoders_InvalidCountOffsetOrLength = "Invalid {0}, {1} or {2} length."
  .field /*04000006*/ static assembly literal string WebEncoders_MalformedInput = "Malformed input: {0} is an invalid input length."
  .method /*06000023*/ assembly hidebysig static 
          string  FormatWebEncoders_InvalidCountOffsetOrLength(object p0,
                                                               object p1,
                                                               object p2) cil managed
  // SIG: 00 03 0E 1C 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncoderResources::FormatWebEncoders_InvalidCountOffsetOrLength

  .method /*06000024*/ assembly hidebysig static 
          string  FormatWebEncoders_MalformedInput(object p0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method EncoderResources::FormatWebEncoders_MalformedInput

} // end of class Microsoft.Extensions.WebEncoders.Sources.EncoderResources

.class /*0200000D*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.SecurityHelper
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .class /*02000032*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C00008C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400002C*/ public static initonly class Microsoft.Extensions.Internal.SecurityHelper/*0200000D*//'<>c'/*02000032*/ '<>9'
    .field /*0400002D*/ public static class [System.Runtime/*23000001*/]System.Func`2/*01000039*/<class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsIdentity/*0100003A*/,bool> '<>9__0_0'
    .method /*060000ED*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*060000EE*/ assembly hidebysig 
            instance bool  '<MergeUserPrincipal>b__0_0'(class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsIdentity/*0100003A*/ i) cil managed
    // SIG: 20 01 02 12 80 E9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<MergeUserPrincipal>b__0_0'

  } // end of class '<>c'

  .method /*06000025*/ public hidebysig static 
          class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ 
          MergeUserPrincipal(class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ existingPrincipal,
                             class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ additionalPrincipal) cil managed
  // SIG: 00 02 12 80 E1 12 80 E1 12 80 E1
  {
    .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000026*/ 
    .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SecurityHelper::MergeUserPrincipal

} // end of class Microsoft.Extensions.Internal.SecurityHelper

.class /*0200000E*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.WebEncoders
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00003D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000026*/ public hidebysig static 
          uint8[]  Base64UrlDecode(string input) cil managed
  // SIG: 00 01 1D 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlDecode

  .method /*06000027*/ public hidebysig static 
          uint8[]  Base64UrlDecode(string input,
                                   int32 offset,
                                   int32 count) cil managed
  // SIG: 00 03 1D 05 0E 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlDecode

  .method /*06000028*/ public hidebysig static 
          uint8[]  Base64UrlDecode(string input,
                                   int32 offset,
                                   char[] buffer,
                                   int32 bufferOffset,
                                   int32 count) cil managed
  // SIG: 00 05 1D 05 0E 08 1D 03 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlDecode

  .method /*06000029*/ public hidebysig static 
          int32  GetArraySizeRequiredToDecode(int32 count) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::GetArraySizeRequiredToDecode

  .method /*0600002A*/ public hidebysig static 
          string  Base64UrlEncode(uint8[] input) cil managed
  // SIG: 00 01 0E 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlEncode

  .method /*0600002B*/ public hidebysig static 
          string  Base64UrlEncode(uint8[] input,
                                  int32 offset,
                                  int32 count) cil managed
  // SIG: 00 03 0E 1D 05 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlEncode

  .method /*0600002C*/ public hidebysig static 
          int32  Base64UrlEncode(uint8[] input,
                                 int32 offset,
                                 char[] output,
                                 int32 outputOffset,
                                 int32 count) cil managed
  // SIG: 00 05 08 1D 05 08 1D 03 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlEncode

  .method /*0600002D*/ public hidebysig static 
          int32  GetArraySizeRequiredToEncode(int32 count) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::GetArraySizeRequiredToEncode

  .method /*0600002E*/ public hidebysig static 
          string  Base64UrlEncode(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100003C*/<uint8> input) cil managed
  // SIG: 00 01 0E 15 11 80 F1 01 05
  {
    .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000083:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.SkipLocalsInitAttribute/*0100003B*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .param [0]/*0800003D*/ 
    .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlEncode

} // end of class Microsoft.Extensions.Internal.WebEncoders

.class /*0200000F*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.NonCapturingTimer
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*0600002F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Timer/*0100003D*/ 
          Create(class [System.Runtime/*23000001*/]System.Threading.TimerCallback/*0100003E*/ callback,
                 object state,
                 valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ dueTime,
                 valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ period) cil managed
  // SIG: 00 04 12 80 F5 12 80 F9 1C 11 80 FD 11 80 FD
  {
    .custom /*0C000086:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000040*/ 
    .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NonCapturingTimer::Create

} // end of class Microsoft.Extensions.Internal.NonCapturingTimer

.class /*02000010*/ private sequential ansi sealed beforefieldinit Microsoft.Extensions.Internal.ValueStopwatch
       extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
{
  .field /*04000007*/ private static initonly float64 TimestampToTicks
  .field /*04000008*/ private initonly int64 _startTimestamp
  .method /*06000030*/ public hidebysig specialname 
          instance bool  get_IsActive() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStopwatch::get_IsActive

  .method /*06000031*/ public hidebysig static 
          valuetype Microsoft.Extensions.Internal.ValueStopwatch/*02000010*/ 
          StartNew() cil managed
  // SIG: 00 00 11 40
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStopwatch::StartNew

  .method /*06000032*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ 
          GetElapsedTime() cil managed
  // SIG: 20 00 11 80 FD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStopwatch::GetElapsedTime

  .property /*17000009*/ instance callconv(8) bool
          IsActive()
  {
    .get instance bool Microsoft.Extensions.Internal.ValueStopwatch/*02000010*/::get_IsActive() /* 06000030 */
  } // end of property ValueStopwatch::IsActive
} // end of class Microsoft.Extensions.Internal.ValueStopwatch

.class /*02000011*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.DependencyInjection.ConnectionsDependencyInjectionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000044:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000033*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000040*/ 
          AddConnections(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000040*/ services) cil managed
  // SIG: 00 01 12 81 01 12 81 01
  {
    .custom /*0C00008D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionsDependencyInjectionExtensions::AddConnections

  .method /*06000034*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000040*/ 
          AddConnections(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000040*/ services,
                         class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class Microsoft.AspNetCore.Http.Connections.ConnectionOptions/*02000016*/> options) cil managed
  // SIG: 00 02 12 81 01 12 81 01 15 12 81 05 01 12 58
  {
    .custom /*0C00008F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionsDependencyInjectionExtensions::AddConnections

} // end of class Microsoft.Extensions.DependencyInjection.ConnectionsDependencyInjectionExtensions

.class /*02000012*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.AwaitableThreadPool
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .class /*02000033*/ sequential ansi sealed nested public beforefieldinit Awaitable
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ICriticalNotifyCompletion/*01000035*/,
                    [System.Runtime/*23000001*/]System.Runtime.CompilerServices.INotifyCompletion/*01000036*/
  {
    .pack 0
    .size 1
    .custom /*0C00008E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000033*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    .method /*060000EF*/ public hidebysig 
            instance void  GetResult() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaitable::GetResult

    .method /*060000F0*/ public hidebysig 
            instance valuetype Microsoft.AspNetCore.Internal.AwaitableThreadPool/*02000012*//Awaitable/*02000033*/ 
            GetAwaiter() cil managed
    // SIG: 20 00 11 80 CC
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaitable::GetAwaiter

    .method /*060000F1*/ public hidebysig specialname 
            instance bool  get_IsCompleted() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaitable::get_IsCompleted

    .method /*060000F2*/ public hidebysig newslot virtual final 
            instance void  OnCompleted(class [System.Runtime/*23000001*/]System.Action/*01000037*/ continuation) cil managed
    // SIG: 20 01 01 12 80 DD
    {
      .custom /*0C000134:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaitable::OnCompleted

    .method /*060000F3*/ public hidebysig newslot virtual final 
            instance void  UnsafeOnCompleted(class [System.Runtime/*23000001*/]System.Action/*01000037*/ continuation) cil managed
    // SIG: 20 01 01 12 80 DD
    {
      .custom /*0C000135:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaitable::UnsafeOnCompleted

    .property /*17000043*/ instance callconv(8) bool
            IsCompleted()
    {
      .get instance bool Microsoft.AspNetCore.Internal.AwaitableThreadPool/*02000012*//Awaitable/*02000033*/::get_IsCompleted() /* 060000F1 */
    } // end of property Awaitable::IsCompleted
  } // end of class Awaitable

  .method /*06000035*/ public hidebysig static 
          valuetype Microsoft.AspNetCore.Internal.AwaitableThreadPool/*02000012*//Awaitable/*02000033*/ 
          Yield() cil managed
  // SIG: 00 00 11 80 CC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableThreadPool::Yield

} // end of class Microsoft.AspNetCore.Internal.AwaitableThreadPool

.class /*02000013*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.MemoryBufferWriter
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/
       implements class [System.Memory/*23000003*/]System.Buffers.IBufferWriter`1/*01000042*/<uint8>/*1B000001*/
{
  .custom /*0C000047:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Memory/*23000003*/]System.Buffers.IBufferWriter`1/*01000042*/<uint8>/*1B000001*/
  .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000034*/ sequential ansi sealed nested private beforefieldinit CompletedBuffer
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
  {
    .custom /*0C000090:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000033*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    .field /*0400002E*/ private initonly uint8[] '<Buffer>k__BackingField'
    .custom /*0C000084:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400002F*/ private initonly int32 '<Length>k__BackingField'
    .custom /*0C000087:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .method /*060000F4*/ public hidebysig specialname 
            instance uint8[]  get_Buffer() cil managed
    // SIG: 20 00 1D 05
    {
      .custom /*0C000136:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CompletedBuffer::get_Buffer

    .method /*060000F5*/ public hidebysig specialname 
            instance int32  get_Length() cil managed
    // SIG: 20 00 08
    {
      .custom /*0C000137:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CompletedBuffer::get_Length

    .method /*060000F6*/ public hidebysig specialname 
            instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100003C*/<uint8> 
            get_Span() cil managed
    // SIG: 20 00 15 11 80 F1 01 05
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CompletedBuffer::get_Span

    .method /*060000F7*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(uint8[] buffer,
                                 int32 length) cil managed
    // SIG: 20 02 01 1D 05 08
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CompletedBuffer::.ctor

    .method /*060000F8*/ public hidebysig 
            instance void  Return() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CompletedBuffer::Return

    .property /*17000044*/ instance callconv(8) uint8[]
            Buffer()
    {
      .get instance uint8[] Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*//CompletedBuffer/*02000034*/::get_Buffer() /* 060000F4 */
    } // end of property CompletedBuffer::Buffer
    .property /*17000045*/ instance callconv(8) int32
            Length()
    {
      .get instance int32 Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*//CompletedBuffer/*02000034*/::get_Length() /* 060000F5 */
    } // end of property CompletedBuffer::Length
    .property /*17000046*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100003C*/<uint8>
            Span()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100003C*/<uint8> Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*//CompletedBuffer/*02000034*/::get_Span() /* 060000F6 */
    } // end of property CompletedBuffer::Span
  } // end of class CompletedBuffer

  .class /*02000035*/ auto ansi sealed nested private beforefieldinit '<CopyToSlowAsync>d__28'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C000092:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000030*/ public int32 '<>1__state'
    .field /*04000031*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000032*/ public class Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/ '<>4__this'
    .field /*04000033*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ destination
    .field /*04000034*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken
    .field /*04000035*/ private int32 '<count>5__2'
    .field /*04000036*/ private int32 '<i>5__3'
    .field /*04000037*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000025*/ '<>u__1'
    .method /*060000F9*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CopyToSlowAsync>d__28'::MoveNext

    .method /*060000FA*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000138:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CopyToSlowAsync>d__28'::SetStateMachine

  } // end of class '<CopyToSlowAsync>d__28'

  .method /*06000036*/ public hidebysig specialname rtspecialname 
          instance void  .ctor([opt] int32 minimumSegmentSize) cil managed
  // SIG: 20 01 01 08
  {
    .param [1]/*08000046*/  = int32(0x00001000)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::.ctor

  .method /*06000037*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::get_Length

  .method /*06000038*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::get_CanRead

  .method /*06000039*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::get_CanSeek

  .method /*0600003A*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::get_CanWrite

  .method /*0600003B*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::get_Position

  .method /*0600003C*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::set_Position

  .method /*0600003D*/ public hidebysig static 
          class Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/ 
          Get() cil managed
  // SIG: 00 00 12 4C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Get

  .method /*0600003E*/ public hidebysig static 
          void  Return(class Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/ writer) cil managed
  // SIG: 00 01 01 12 4C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Return

  .method /*0600003F*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Reset

  .method /*06000040*/ public hidebysig newslot virtual final 
          instance void  Advance(int32 count) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Advance

  .method /*06000041*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000043*/<uint8> 
          GetMemory([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 81 0D 01 05 08
  {
    .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800004A*/  = int32(0x00000000)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::GetMemory

  .method /*06000042*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*01000044*/<uint8> 
          GetSpan([opt] int32 sizeHint) cil managed
  // SIG: 20 01 15 11 81 11 01 05 08
  {
    .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800004B*/  = int32(0x00000000)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::GetSpan

  .method /*06000043*/ public hidebysig instance void 
          CopyTo(class [System.Memory/*23000003*/]System.Buffers.IBufferWriter`1/*01000042*/<uint8> destination) cil managed
  // SIG: 20 01 01 15 12 81 09 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::CopyTo

  .method /*06000044*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ destination,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 12 80 9D 08 11 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::CopyToAsync

  .method /*06000045*/ public hidebysig instance uint8[] 
          ToArray() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::ToArray

  .method /*06000046*/ public hidebysig instance void 
          CopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000044*/<uint8> span) cil managed
  // SIG: 20 01 01 15 11 81 11 01 05
  {
    .custom /*0C0000A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::CopyTo

  .method /*06000047*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Flush

  .method /*06000048*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 A9 11 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::FlushAsync

  .method /*06000049*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Read

  .method /*0600004A*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000029*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 A5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Seek

  .method /*0600004B*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::SetLength

  .method /*0600004C*/ public hidebysig virtual 
          instance void  WriteByte(uint8 'value') cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::WriteByte

  .method /*0600004D*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Write

  .method /*0600004E*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100003C*/<uint8> span) cil managed
  // SIG: 20 01 01 15 11 80 F1 01 05
  {
    .custom /*0C0000AF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Write

  .method /*0600004F*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MemoryBufferWriter::Dispose

  .property /*1700000A*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/::get_Length() /* 06000037 */
  } // end of property MemoryBufferWriter::Length
  .property /*1700000B*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/::get_CanRead() /* 06000038 */
  } // end of property MemoryBufferWriter::CanRead
  .property /*1700000C*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/::get_CanSeek() /* 06000039 */
  } // end of property MemoryBufferWriter::CanSeek
  .property /*1700000D*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/::get_CanWrite() /* 0600003A */
  } // end of property MemoryBufferWriter::CanWrite
  .property /*1700000E*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/::get_Position() /* 0600003B */
    .set instance void Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/::set_Position(int64) /* 0600003C */
  } // end of property MemoryBufferWriter::Position
} // end of class Microsoft.AspNetCore.Internal.MemoryBufferWriter

.class /*02000014*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.TaskCache
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00004A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000009*/ public static initonly class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000045*/<bool> True
  .field /*0400000A*/ public static initonly class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000045*/<bool> False
} // end of class Microsoft.AspNetCore.Internal.TaskCache

.class /*02000015*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.ValueTaskExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00004E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000050*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          GetAsTask([in] valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100002D*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.FlushResult/*0100002E*/>& valueTask) cil managed aggressiveinlining
  // SIG: 00 01 12 80 A9 10 15 11 80 B5 01 11 80 B9
  {
    .custom /*0C0000B1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800005E*/ 
    .custom /*0C0000C5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800005F*/ 
    .custom /*0C0000C6:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000033*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueTaskExtensions::GetAsTask

  .method /*06000051*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100001B*/ 
          GetAsValueTask([in] valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*0100002D*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.FlushResult/*0100002E*/>& valueTask) cil managed aggressiveinlining
  // SIG: 00 01 11 6D 10 15 11 80 B5 01 11 80 B9
  {
    .custom /*0C0000B2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [1]/*08000060*/ 
    .custom /*0C0000C7:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000033*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueTaskExtensions::GetAsValueTask

} // end of class Microsoft.AspNetCore.Internal.ValueTaskExtensions

.class /*02000016*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.ConnectionOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*06000052*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/> 
          get_DisconnectTimeout() cil managed
  // SIG: 20 00 15 11 81 19 01 11 80 FD
  {
    .custom /*0C0000B4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionOptions::get_DisconnectTimeout

  .method /*06000053*/ public hidebysig specialname 
          instance void  set_DisconnectTimeout(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 19 01 11 80 FD
  {
    .custom /*0C0000B5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionOptions::set_DisconnectTimeout

  .method /*06000054*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionOptions::.ctor

  .property /*1700000F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/>
          DisconnectTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/> Microsoft.AspNetCore.Http.Connections.ConnectionOptions/*02000016*/::get_DisconnectTimeout() /* 06000052 */
    .set instance void Microsoft.AspNetCore.Http.Connections.ConnectionOptions/*02000016*/::set_DisconnectTimeout(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/>) /* 06000053 */
  } // end of property ConnectionOptions::DisconnectTimeout
} // end of class Microsoft.AspNetCore.Http.Connections.ConnectionOptions

.class /*02000017*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.ConnectionOptionsSetup
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements class [Microsoft.Extensions.Options/*23000007*/]Microsoft.Extensions.Options.IConfigureOptions`1/*01000047*/<class Microsoft.AspNetCore.Http.Connections.ConnectionOptions/*02000016*/>/*1B000002*/
{
  .interfaceimpl type class [Microsoft.Extensions.Options/*23000007*/]Microsoft.Extensions.Options.IConfigureOptions`1/*01000047*/<class Microsoft.AspNetCore.Http.Connections.ConnectionOptions/*02000016*/>/*1B000002*/
  .custom /*0C000028:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .field /*0400000B*/ public static valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ DefaultDisconectTimeout
  .method /*06000055*/ public hidebysig newslot virtual final 
          instance void  Configure(class Microsoft.AspNetCore.Http.Connections.ConnectionOptions/*02000016*/ options) cil managed
  // SIG: 20 01 01 12 58
  {
    .custom /*0C0000B8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionOptionsSetup::Configure

  .method /*06000056*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionOptionsSetup::.ctor

} // end of class Microsoft.AspNetCore.Http.Connections.ConnectionOptionsSetup

.class /*02000018*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Connections.HttpConnectionContextExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000055:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000057*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ 
          GetHttpContext(class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000049*/ connection) cil managed
  // SIG: 00 01 12 81 21 12 81 25
  {
    .custom /*0C0000BB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000BC:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000063*/ 
    .custom /*0C0000CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContextExtensions::GetHttpContext

} // end of class Microsoft.AspNetCore.Http.Connections.HttpConnectionContextExtensions

.class /*02000019*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000057:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000058*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::.ctor

  .method /*06000059*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100004A*/<class [Microsoft.AspNetCore.Metadata/*2300000A*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100004B*/> 
          get_AuthorizationData() cil managed
  // SIG: 20 00 15 12 81 29 01 12 81 2D
  {
    .custom /*0C0000BE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_AuthorizationData

  .method /*0600005A*/ public hidebysig specialname 
          instance valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ 
          get_Transports() cil managed
  // SIG: 20 00 11 81 31
  {
    .custom /*0C0000C0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_Transports

  .method /*0600005B*/ public hidebysig specialname 
          instance void  set_Transports(valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ 'value') cil managed
  // SIG: 20 01 01 11 81 31
  {
    .custom /*0C0000C2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::set_Transports

  .method /*0600005C*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Http.Connections.WebSocketOptions/*0200001D*/ 
          get_WebSockets() cil managed
  // SIG: 20 00 12 74
  {
    .custom /*0C0000C3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_WebSockets

  .method /*0600005D*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Http.Connections.LongPollingOptions/*0200001A*/ 
          get_LongPolling() cil managed
  // SIG: 20 00 12 68
  {
    .custom /*0C0000C4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_LongPolling

  .method /*0600005E*/ public hidebysig specialname 
          instance int64  get_TransportMaxBufferSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_TransportMaxBufferSize

  .method /*0600005F*/ public hidebysig specialname 
          instance void  set_TransportMaxBufferSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::set_TransportMaxBufferSize

  .method /*06000060*/ public hidebysig specialname 
          instance int64  get_ApplicationMaxBufferSize() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_ApplicationMaxBufferSize

  .method /*06000061*/ public hidebysig specialname 
          instance void  set_ApplicationMaxBufferSize(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::set_ApplicationMaxBufferSize

  .method /*06000062*/ public hidebysig specialname 
          instance int32  get_MinimumProtocolVersion() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000C8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_MinimumProtocolVersion

  .method /*06000063*/ public hidebysig specialname 
          instance void  set_MinimumProtocolVersion(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000C9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::set_MinimumProtocolVersion

  .method /*06000064*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ 
          get_TransportSendTimeout() cil managed
  // SIG: 20 00 11 80 FD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_TransportSendTimeout

  .method /*06000065*/ public hidebysig specialname 
          instance void  set_TransportSendTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ 'value') cil managed
  // SIG: 20 01 01 11 80 FD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::set_TransportSendTimeout

  .method /*06000066*/ public hidebysig specialname 
          instance bool  get_CloseOnAuthenticationExpiration() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000CB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_CloseOnAuthenticationExpiration

  .method /*06000067*/ public hidebysig specialname 
          instance void  set_CloseOnAuthenticationExpiration(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C0000CC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::set_CloseOnAuthenticationExpiration

  .method /*06000068*/ assembly hidebysig specialname 
          instance int64  get_TransportSendTimeoutTicks() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C0000CD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_TransportSendTimeoutTicks

  .method /*06000069*/ assembly hidebysig specialname 
          instance bool  get_TransportSendTimeoutEnabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_TransportSendTimeoutEnabled

  .method /*0600006A*/ assembly hidebysig specialname 
          instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeOptions/*01000032*/ 
          get_TransportPipeOptions() cil managed
  // SIG: 20 00 12 80 C9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_TransportPipeOptions

  .method /*0600006B*/ assembly hidebysig specialname 
          instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeOptions/*01000032*/ 
          get_AppPipeOptions() cil managed
  // SIG: 20 00 12 80 C9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcherOptions::get_AppPipeOptions

  .property /*17000010*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100004A*/<class [Microsoft.AspNetCore.Metadata/*2300000A*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100004B*/>
          AuthorizationData()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100004A*/<class [Microsoft.AspNetCore.Metadata/*2300000A*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100004B*/> Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_AuthorizationData() /* 06000059 */
  } // end of property HttpConnectionDispatcherOptions::AuthorizationData
  .property /*17000011*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/
          Transports()
  {
    .get instance valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_Transports() /* 0600005A */
    .set instance void Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::set_Transports(valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/) /* 0600005B */
  } // end of property HttpConnectionDispatcherOptions::Transports
  .property /*17000012*/ instance callconv(8) class Microsoft.AspNetCore.Http.Connections.WebSocketOptions/*0200001D*/
          WebSockets()
  {
    .get instance class Microsoft.AspNetCore.Http.Connections.WebSocketOptions/*0200001D*/ Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_WebSockets() /* 0600005C */
  } // end of property HttpConnectionDispatcherOptions::WebSockets
  .property /*17000013*/ instance callconv(8) class Microsoft.AspNetCore.Http.Connections.LongPollingOptions/*0200001A*/
          LongPolling()
  {
    .get instance class Microsoft.AspNetCore.Http.Connections.LongPollingOptions/*0200001A*/ Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_LongPolling() /* 0600005D */
  } // end of property HttpConnectionDispatcherOptions::LongPolling
  .property /*17000014*/ instance callconv(8) int64
          TransportMaxBufferSize()
  {
    .get instance int64 Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_TransportMaxBufferSize() /* 0600005E */
    .set instance void Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::set_TransportMaxBufferSize(int64) /* 0600005F */
  } // end of property HttpConnectionDispatcherOptions::TransportMaxBufferSize
  .property /*17000015*/ instance callconv(8) int64
          ApplicationMaxBufferSize()
  {
    .get instance int64 Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_ApplicationMaxBufferSize() /* 06000060 */
    .set instance void Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::set_ApplicationMaxBufferSize(int64) /* 06000061 */
  } // end of property HttpConnectionDispatcherOptions::ApplicationMaxBufferSize
  .property /*17000016*/ instance callconv(8) int32
          MinimumProtocolVersion()
  {
    .get instance int32 Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_MinimumProtocolVersion() /* 06000062 */
    .set instance void Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::set_MinimumProtocolVersion(int32) /* 06000063 */
  } // end of property HttpConnectionDispatcherOptions::MinimumProtocolVersion
  .property /*17000017*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/
          TransportSendTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_TransportSendTimeout() /* 06000064 */
    .set instance void Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::set_TransportSendTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/) /* 06000065 */
  } // end of property HttpConnectionDispatcherOptions::TransportSendTimeout
  .property /*17000018*/ instance callconv(8) bool
          CloseOnAuthenticationExpiration()
  {
    .get instance bool Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_CloseOnAuthenticationExpiration() /* 06000066 */
    .set instance void Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::set_CloseOnAuthenticationExpiration(bool) /* 06000067 */
  } // end of property HttpConnectionDispatcherOptions::CloseOnAuthenticationExpiration
  .property /*17000019*/ instance callconv(8) int64
          TransportSendTimeoutTicks()
  {
    .get instance int64 Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_TransportSendTimeoutTicks() /* 06000068 */
  } // end of property HttpConnectionDispatcherOptions::TransportSendTimeoutTicks
  .property /*1700001A*/ instance callconv(8) bool
          TransportSendTimeoutEnabled()
  {
    .get instance bool Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_TransportSendTimeoutEnabled() /* 06000069 */
  } // end of property HttpConnectionDispatcherOptions::TransportSendTimeoutEnabled
  .property /*1700001B*/ instance callconv(8) class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeOptions/*01000032*/
          TransportPipeOptions()
  {
    .get instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeOptions/*01000032*/ Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_TransportPipeOptions() /* 0600006A */
  } // end of property HttpConnectionDispatcherOptions::TransportPipeOptions
  .property /*1700001C*/ instance callconv(8) class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeOptions/*01000032*/
          AppPipeOptions()
  {
    .get instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeOptions/*01000032*/ Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/::get_AppPipeOptions() /* 0600006B */
  } // end of property HttpConnectionDispatcherOptions::AppPipeOptions
} // end of class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions

.class /*0200001A*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.LongPollingOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*0600006C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ 
          get_PollTimeout() cil managed
  // SIG: 20 00 11 80 FD
  {
    .custom /*0C0000CE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LongPollingOptions::get_PollTimeout

  .method /*0600006D*/ public hidebysig specialname 
          instance void  set_PollTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ 'value') cil managed
  // SIG: 20 01 01 11 80 FD
  {
    .custom /*0C0000D0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LongPollingOptions::set_PollTimeout

  .method /*0600006E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LongPollingOptions::.ctor

  .property /*1700001D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/
          PollTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ Microsoft.AspNetCore.Http.Connections.LongPollingOptions/*0200001A*/::get_PollTimeout() /* 0600006C */
    .set instance void Microsoft.AspNetCore.Http.Connections.LongPollingOptions/*0200001A*/::set_PollTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/) /* 0600006D */
  } // end of property LongPollingOptions::PollTimeout
} // end of class Microsoft.AspNetCore.Http.Connections.LongPollingOptions

.class /*0200001B*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.NegotiateMetadata
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*0600006F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateMetadata::.ctor

} // end of class Microsoft.AspNetCore.Http.Connections.NegotiateMetadata

.class /*0200001C*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Connections.ServerSentEventsMessageFormatter
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .class /*02000036*/ auto ansi sealed nested private beforefieldinit '<WriteMessageAsync>d__3'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C000093:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000038*/ public int32 '<>1__state'
    .field /*04000039*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*0400003A*/ public valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> payload
    .field /*0400003B*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ output
    .field /*0400003C*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ token
    .field /*0400003D*/ private class [System.Runtime/*23000001*/]System.IO.MemoryStream/*0100004D*/ '<ms>5__2'
    .field /*0400003E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000025*/ '<>u__1'
    .field /*0400003F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__2'
    .method /*060000FB*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteMessageAsync>d__3'::MoveNext

    .method /*060000FC*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000139:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteMessageAsync>d__3'::SetStateMachine

  } // end of class '<WriteMessageAsync>d__3'

  .class /*02000037*/ auto ansi sealed nested private beforefieldinit '<WriteMessageToMemory>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C000095:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000040*/ public int32 '<>1__state'
    .field /*04000041*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000042*/ public valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> payload
    .field /*04000043*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ output
    .field /*04000044*/ private bool '<keepWriting>5__2'
    .field /*04000045*/ private valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> '<lineSegment>5__3'
    .field /*04000046*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000025*/ '<>u__1'
    .method /*060000FD*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteMessageToMemory>d__5'::MoveNext

    .method /*060000FE*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00013A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteMessageToMemory>d__5'::SetStateMachine

  } // end of class '<WriteMessageToMemory>d__5'

  .method /*06000070*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          WriteMessageAsync(valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> payload,
                            class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ output,
                            valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ token) cil managed
  // SIG: 00 03 12 80 A9 15 11 75 01 05 12 80 9D 11 7D
  {
    .custom /*0C0000D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 5E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..^Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 53 65 72 76 65 72 53   // nections.ServerS
                                                                                                                                                                                                                                        65 6E 74 45 76 65 6E 74 73 4D 65 73 73 61 67 65   // entEventsMessage
                                                                                                                                                                                                                                        46 6F 72 6D 61 74 74 65 72 2B 3C 57 72 69 74 65   // Formatter+<Write
                                                                                                                                                                                                                                        4D 65 73 73 61 67 65 41 73 79 6E 63 3E 64 5F 5F   // MessageAsync>d__
                                                                                                                                                                                                                                        33 00 00 )                                        // 3..
    .param [1]/*0800006C*/ 
    .custom /*0C0000CF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServerSentEventsMessageFormatter::WriteMessageAsync

} // end of class Microsoft.AspNetCore.Http.Connections.ServerSentEventsMessageFormatter

.class /*0200001D*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.WebSocketOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*06000071*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ 
          get_CloseTimeout() cil managed
  // SIG: 20 00 11 80 FD
  {
    .custom /*0C0000D4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::get_CloseTimeout

  .method /*06000072*/ public hidebysig specialname 
          instance void  set_CloseTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ 'value') cil managed
  // SIG: 20 01 01 11 80 FD
  {
    .custom /*0C0000D6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::set_CloseTimeout

  .method /*06000073*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*01000039*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100004A*/<string>,string> 
          get_SubProtocolSelector() cil managed
  // SIG: 20 00 15 12 80 E5 02 15 12 81 29 01 0E 0E
  {
    .custom /*0C0000D7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .param [0]/*08000070*/ 
    .custom /*0C0000D3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::get_SubProtocolSelector

  .method /*06000074*/ public hidebysig specialname 
          instance void  set_SubProtocolSelector(class [System.Runtime/*23000001*/]System.Func`2/*01000039*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100004A*/<string>,string> 'value') cil managed
  // SIG: 20 01 01 15 12 80 E5 02 15 12 81 29 01 0E 0E
  {
    .custom /*0C0000D8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .param [1]/*08000071*/ 
    .custom /*0C0000D5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::set_SubProtocolSelector

  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::.ctor

  .property /*1700001E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/
          CloseTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ Microsoft.AspNetCore.Http.Connections.WebSocketOptions/*0200001D*/::get_CloseTimeout() /* 06000071 */
    .set instance void Microsoft.AspNetCore.Http.Connections.WebSocketOptions/*0200001D*/::set_CloseTimeout(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/) /* 06000072 */
  } // end of property WebSocketOptions::CloseTimeout
  .property /*1700001F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*01000039*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100004A*/<string>,string>
          SubProtocolSelector()
  {
    .custom /*0C000063:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*01000039*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100004A*/<string>,string> Microsoft.AspNetCore.Http.Connections.WebSocketOptions/*0200001D*/::get_SubProtocolSelector() /* 06000073 */
    .set instance void Microsoft.AspNetCore.Http.Connections.WebSocketOptions/*0200001D*/::set_SubProtocolSelector(class [System.Runtime/*23000001*/]System.Func`2/*01000039*/<class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100004A*/<string>,string>) /* 06000074 */
  } // end of property WebSocketOptions::SubProtocolSelector
} // end of class Microsoft.AspNetCore.Http.Connections.WebSocketOptions

.class /*0200001E*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000052*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>>/*1B000003*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000054*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>>/*1B000004*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000050*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000055*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>>/*1B000005*/
{
  .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000060:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100004F*/::.ctor(string) /* 0A00001C */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*01000052*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>>/*1B000003*/
  .custom /*0C00002B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000054*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>>/*1B000004*/
  .custom /*0C00002E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000050*/
  .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000055*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>>/*1B000005*/
  .custom /*0C000034:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 00 01 02 00 00 ) 
  .class /*02000038*/ auto ansi sealed nested private beforefieldinit '<GetEnumerator>d__10'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000057*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>>/*1B000006*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000051*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000058*/
  {
    .custom /*0C000096:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000047*/ public class Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*/ '<>4__this'
    .method /*060000FF*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C00013B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__10'::.ctor

    .method /*06000100*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00013C:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000058*/::Dispose /*01000058::0A00001F*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__10'::System.IDisposable.Dispose

    .method /*06000101*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000051*/::MoveNext /*01000051::0A000020*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__10'::MoveNext

    .method /*06000102*/ private hidebysig newslot specialname virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object> 
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'() cil managed
    // SIG: 20 00 15 11 81 4D 02 0E 1C
    {
      .custom /*0C00013D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000057*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>>/*1B000006*/::get_Current() /* 0A000021 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__10'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'

    .method /*06000103*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C00013E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000051*/::Reset /*01000051::0A000022*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__10'::System.Collections.IEnumerator.Reset

    .method /*06000104*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C00013F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000051*/::get_Current /*01000051::0A000023*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetEnumerator>d__10'::System.Collections.IEnumerator.get_Current

    .property /*17000047*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>
            'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Current'()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object> Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*//'<GetEnumerator>d__10'/*02000038*/::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.get_Current'() /* 06000102 */
    } // end of property '<GetEnumerator>d__10'::'System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.String,System.Object>>.Current'
    .property /*17000048*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*//'<GetEnumerator>d__10'/*02000038*/::System.Collections.IEnumerator.get_Current() /* 06000104 */
    } // end of property '<GetEnumerator>d__10'::System.Collections.IEnumerator.Current
  } // end of class '<GetEnumerator>d__10'

  .method /*06000076*/ public hidebysig specialname 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000DA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionLogScope::get_ConnectionId

  .method /*06000077*/ public hidebysig specialname 
          instance void  set_ConnectionId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000DC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionLogScope::set_ConnectionId

  .method /*06000078*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string connectionId) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionLogScope::.ctor

  .method /*06000079*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object> 
          get_Item(int32 index) cil managed
  // SIG: 20 01 15 11 81 4D 02 0E 1C 08
  {
    .param [0]/*08000074*/ 
    .custom /*0C0000D9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionLogScope::get_Item

  .method /*0600007A*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionLogScope::get_Count

  .method /*0600007B*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000057*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 81 5D 01 15 11 81 4D 02 0E 1C
  {
    .custom /*0C0000DD:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*01000056*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A00001E */ = ( 01 00 56 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..VMicrosoft.Asp
                                                                                                                                                                                                                                           4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                           6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                           6C 2E 43 6F 6E 6E 65 63 74 69 6F 6E 4C 6F 67 53   // l.ConnectionLogS
                                                                                                                                                                                                                                           63 6F 70 65 2B 3C 47 65 74 45 6E 75 6D 65 72 61   // cope+<GetEnumera
                                                                                                                                                                                                                                           74 6F 72 3E 64 5F 5F 31 30 00 00 )                // tor>d__10..
    .param [0]/*08000076*/ 
    .custom /*0C0000DB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionLogScope::GetEnumerator

  .method /*0600007C*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000051*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 45
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000050*/::GetEnumerator /*01000050::0A00001D*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionLogScope::System.Collections.IEnumerable.GetEnumerator

  .method /*0600007D*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionLogScope::ToString

  .property /*17000020*/ instance callconv(8) string
          ConnectionId()
  {
    .get instance string Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*/::get_ConnectionId() /* 06000076 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*/::set_ConnectionId(string) /* 06000077 */
  } // end of property ConnectionLogScope::ConnectionId
  .property /*17000021*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object>
          Item(int32)
  {
    .custom /*0C000069:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 02 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000053*/<string,object> Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*/::get_Item(int32) /* 06000079 */
  } // end of property ConnectionLogScope::Item
  .property /*17000022*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*/::get_Count() /* 0600007A */
  } // end of property ConnectionLogScope::Count
} // end of class Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope

.class /*0200001F*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext
       extends [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000049*/
       implements [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*01000059*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100005A*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0100005B*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionUserFeature/*0100005C*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionHeartbeatFeature/*0100005D*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.ITransferFormatFeature/*0100005E*/,
                  Microsoft.AspNetCore.Http.Connections.Features.IHttpContextFeature/*02000028*/,
                  Microsoft.AspNetCore.Http.Connections.Features.IHttpTransportFeature/*02000029*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionInherentKeepAliveFeature/*0100005F*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*01000060*/,
                  [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeNotificationFeature/*01000061*/
{
  .custom /*0C000061:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionIdFeature/*01000059*/
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionItemsFeature/*0100005A*/
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionTransportFeature/*0100005B*/
  .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionUserFeature/*0100005C*/
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionHeartbeatFeature/*0100005D*/
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.ITransferFormatFeature/*0100005E*/
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Http.Connections.Features.IHttpContextFeature/*02000028*/
  .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Http.Connections.Features.IHttpTransportFeature/*02000029*/
  .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionInherentKeepAliveFeature/*0100005F*/
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeFeature/*01000060*/
  .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.Features.IConnectionLifetimeNotificationFeature/*01000061*/
  .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000039*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .method /*06000105*/ public hidebysig static 
            void  DisposingConnection(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                      string connectionId) cil managed
    // SIG: 00 02 01 12 81 89 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::DisposingConnection

    .method /*06000106*/ public hidebysig static 
            void  WaitingForApplication(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::WaitingForApplication

    .method /*06000107*/ public hidebysig static 
            void  ApplicationComplete(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ApplicationComplete

    .method /*06000108*/ public hidebysig static 
            void  WaitingForTransport(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                      valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transportType) cil managed
    // SIG: 00 02 01 12 81 89 11 81 31
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::WaitingForTransport

    .method /*06000109*/ public hidebysig static 
            void  TransportComplete(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                    valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transportType) cil managed
    // SIG: 00 02 01 12 81 89 11 81 31
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::TransportComplete

    .method /*0600010A*/ public hidebysig static 
            void  ShuttingDownTransportAndApplication(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                                      valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transportType) cil managed
    // SIG: 00 02 01 12 81 89 11 81 31
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ShuttingDownTransportAndApplication

    .method /*0600010B*/ public hidebysig static 
            void  WaitingForTransportAndApplication(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                                    valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transportType) cil managed
    // SIG: 00 02 01 12 81 89 11 81 31
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::WaitingForTransportAndApplication

    .method /*0600010C*/ public hidebysig static 
            void  TransportAndApplicationComplete(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                                  valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transportType) cil managed
    // SIG: 00 02 01 12 81 89 11 81 31
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::TransportAndApplicationComplete

    .method /*0600010D*/ public hidebysig static 
            void  TransportSendTimeout(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                       valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ transportTimeout,
                                       string connectionId) cil managed
    // SIG: 00 03 01 12 81 89 11 80 FD 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::TransportSendTimeout

  } // end of class Log

  .class /*0200003A*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C000097:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000048*/ public static initonly class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*//'<>c'/*0200003A*/ '<>9'
    .field /*04000049*/ public static class [System.Threading.ThreadPool/*2300000F*/]System.Threading.WaitCallback/*0100006C*/ '<>9__119_0'
    .field /*0400004A*/ public static class [System.Threading.ThreadPool/*2300000F*/]System.Threading.WaitCallback/*0100006C*/ '<>9__123_0'
    .field /*0400004B*/ public static class [System.Threading.ThreadPool/*2300000F*/]System.Threading.WaitCallback/*0100006C*/ '<>9__123_1'
    .field /*0400004C*/ public static class [System.Threading.ThreadPool/*2300000F*/]System.Threading.WaitCallback/*0100006C*/ '<>9__133_0'
    .method /*0600010E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600010F*/ assembly hidebysig 
            instance void  '<Abort>b__119_0'(object cts) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<Abort>b__119_0'

    .method /*06000110*/ assembly hidebysig 
            instance void  '<WaitOnTasks>b__123_0'(object cts) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<WaitOnTasks>b__123_0'

    .method /*06000111*/ assembly hidebysig 
            instance void  '<WaitOnTasks>b__123_1'(object cts) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<WaitOnTasks>b__123_1'

    .method /*06000112*/ assembly hidebysig 
            instance void  '<RequestClose>b__133_0'(object cts) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<RequestClose>b__133_0'

  } // end of class '<>c'

  .class /*0200003B*/ auto ansi sealed nested private beforefieldinit '<CancelPreviousPoll>d__127'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C000098:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400004D*/ public int32 '<>1__state'
    .field /*0400004E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100006D*/<bool> '<>t__builder'
    .field /*0400004F*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ '<>4__this'
    .field /*04000050*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*04000051*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000063*/ '<>7__wrap1'
    .field /*04000052*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*06000113*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CancelPreviousPoll>d__127'::MoveNext

    .method /*06000114*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000140:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CancelPreviousPoll>d__127'::SetStateMachine

  } // end of class '<CancelPreviousPoll>d__127'

  .class /*0200003C*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__122'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C000099:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000053*/ public int32 '<>1__state'
    .field /*04000054*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000055*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ '<>4__this'
    .field /*04000056*/ public bool closeGracefully
    .field /*04000057*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*06000115*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__122'::MoveNext

    .method /*06000116*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000141:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__122'::SetStateMachine

  } // end of class '<DisposeAsync>d__122'

  .class /*0200003D*/ auto ansi sealed nested private beforefieldinit '<ExecuteApplication>d__129'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C00009B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000058*/ public int32 '<>1__state'
    .field /*04000059*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*0400005A*/ public class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionDelegate/*0100006A*/ connectionDelegate
    .field /*0400005B*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ '<>4__this'
    .field /*0400005C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.YieldAwaitable/*0100006E*//YieldAwaiter/*0100006F*/ '<>u__1'
    .field /*0400005D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__2'
    .method /*06000117*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteApplication>d__129'::MoveNext

    .method /*06000118*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000142:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteApplication>d__129'::SetStateMachine

  } // end of class '<ExecuteApplication>d__129'

  .class /*0200003E*/ auto ansi sealed nested private beforefieldinit '<WaitOnTasks>d__123'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C00009D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400005E*/ public int32 '<>1__state'
    .field /*0400005F*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000060*/ public bool closeGracefully
    .field /*04000061*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ '<>4__this'
    .field /*04000062*/ public class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ applicationTask
    .field /*04000063*/ public class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ transportTask
    .field /*04000064*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .field /*04000065*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> '<>u__2'
    .method /*06000119*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WaitOnTasks>d__123'::MoveNext

    .method /*0600011A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000143:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WaitOnTasks>d__123'::SetStateMachine

  } // end of class '<WaitOnTasks>d__123'

  .method /*0600007E*/ assembly hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ 
          get_SendingToken() cil managed
  // SIG: 20 00 11 7D
  {
    .custom /*0C0000DE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_SendingToken

  .method /*0600007F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string connectionId,
                               string connectionToken,
                               class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                               class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ transport,
                               class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ application,
                               class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options) cil managed
  // SIG: 20 06 01 0E 0E 12 81 89 12 80 C1 12 80 C1 12 64
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::.ctor

  .method /*06000080*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000063*/ 
          get_Cancellation() cil managed
  // SIG: 20 00 12 81 8D
  {
    .custom /*0C0000DF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_Cancellation

  .method /*06000081*/ public hidebysig specialname 
          instance void  set_Cancellation(class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000063*/ 'value') cil managed
  // SIG: 20 01 01 12 81 8D
  {
    .custom /*0C0000E1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_Cancellation

  .method /*06000082*/ public hidebysig newslot specialname virtual final 
          instance valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ 
          get_TransportType() cil managed
  // SIG: 20 00 11 81 31
  {
    .custom /*0C0000E3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_TransportType

  .method /*06000083*/ public hidebysig specialname 
          instance void  set_TransportType(valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ 'value') cil managed
  // SIG: 20 01 01 11 81 31
  {
    .custom /*0C0000E4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_TransportType

  .method /*06000084*/ public hidebysig specialname 
          instance class [System.Threading/*2300000D*/]System.Threading.SemaphoreSlim/*01000064*/ 
          get_WriteLock() cil managed
  // SIG: 20 00 12 81 91
  {
    .custom /*0C0000E5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_WriteLock

  .method /*06000085*/ assembly hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          get_DisposeAndRemoveTask() cil managed
  // SIG: 20 00 12 80 A9
  {
    .custom /*0C0000E6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_DisposeAndRemoveTask

  .method /*06000086*/ assembly hidebysig specialname 
          instance void  set_DisposeAndRemoveTask(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .custom /*0C0000E8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_DisposeAndRemoveTask

  .method /*06000087*/ assembly hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000065*/> 
          get_ServiceScope() cil managed
  // SIG: 20 00 15 11 81 19 01 11 81 95
  {
    .custom /*0C0000EA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_ServiceScope

  .method /*06000088*/ assembly hidebysig specialname 
          instance void  set_ServiceScope(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000065*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 19 01 11 81 95
  {
    .custom /*0C0000EB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_ServiceScope

  .method /*06000089*/ assembly hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/ 
          get_AuthenticationExpiration() cil managed
  // SIG: 20 00 11 81 99
  {
    .custom /*0C0000EC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_AuthenticationExpiration

  .method /*0600008A*/ assembly hidebysig specialname 
          instance void  set_AuthenticationExpiration(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/ 'value') cil managed
  // SIG: 20 01 01 11 81 99
  {
    .custom /*0C0000ED:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_AuthenticationExpiration

  .method /*0600008B*/ assembly hidebysig specialname 
          instance bool  get_IsAuthenticationExpirationEnabled() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_IsAuthenticationExpirationEnabled

  .method /*0600008C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          get_TransportTask() cil managed
  // SIG: 20 00 12 80 A9
  {
    .custom /*0C0000EF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_TransportTask

  .method /*0600008D*/ public hidebysig specialname 
          instance void  set_TransportTask(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .custom /*0C0000F2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_TransportTask

  .method /*0600008E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          get_PreviousPollTask() cil managed
  // SIG: 20 00 12 80 A9
  {
    .custom /*0C0000F4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_PreviousPollTask

  .method /*0600008F*/ public hidebysig specialname 
          instance void  set_PreviousPollTask(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .custom /*0C0000F5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_PreviousPollTask

  .method /*06000090*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          get_ApplicationTask() cil managed
  // SIG: 20 00 12 80 A9
  {
    .custom /*0C0000F6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_ApplicationTask

  .method /*06000091*/ public hidebysig specialname 
          instance void  set_ApplicationTask(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A9
  {
    .custom /*0C0000F8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_ApplicationTask

  .method /*06000092*/ public hidebysig specialname 
          instance int64  get_LastSeenTicks() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C0000FA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_LastSeenTicks

  .method /*06000093*/ public hidebysig specialname 
          instance void  set_LastSeenTicks(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    .custom /*0C0000FB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_LastSeenTicks

  .method /*06000094*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<int64> 
          get_LastSeenTicksIfInactive() cil managed
  // SIG: 20 00 15 11 81 19 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_LastSeenTicksIfInactive

  .method /*06000095*/ public hidebysig specialname 
          instance valuetype Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus/*02000023*/ 
          get_Status() cil managed
  // SIG: 20 00 11 80 8C
  {
    .custom /*0C0000FC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_Status

  .method /*06000096*/ public hidebysig specialname 
          instance void  set_Status(valuetype Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus/*02000023*/ 'value') cil managed
  // SIG: 20 01 01 11 80 8C
  {
    .custom /*0C0000FD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_Status

  .method /*06000097*/ public hidebysig specialname virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000FE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_ConnectionId

  .method /*06000098*/ public hidebysig specialname virtual 
          instance void  set_ConnectionId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000FF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_ConnectionId

  .method /*06000099*/ assembly hidebysig specialname 
          instance string  get_ConnectionToken() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000100:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_ConnectionToken

  .method /*0600009A*/ assembly hidebysig specialname 
          instance void  set_ConnectionToken(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000101:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_ConnectionToken

  .method /*0600009B*/ public hidebysig specialname virtual 
          instance class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000067*/ 
          get_Features() cil managed
  // SIG: 20 00 12 81 9D
  {
    .custom /*0C000102:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_Features

  .method /*0600009C*/ public hidebysig newslot specialname virtual final 
          instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ 
          get_User() cil managed
  // SIG: 20 00 12 80 E1
  {
    .custom /*0C000103:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000104:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_User

  .method /*0600009D*/ public hidebysig newslot specialname virtual final 
          instance void  set_User(class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ 'value') cil managed
  // SIG: 20 01 01 12 80 E1
  {
    .custom /*0C000105:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000106:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_User

  .method /*0600009E*/ public hidebysig newslot specialname virtual final 
          instance bool  get_HasInherentKeepAlive() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000107:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_HasInherentKeepAlive

  .method /*0600009F*/ public hidebysig specialname 
          instance void  set_HasInherentKeepAlive(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000108:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_HasInherentKeepAlive

  .method /*060000A0*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000068*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 81 A1 02 1C 1C
  {
    .param [0]/*0800008B*/ 
    .custom /*0C0000EE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_Items

  .method /*060000A1*/ public hidebysig specialname virtual 
          instance void  set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000068*/<object,object> 'value') cil managed
  // SIG: 20 01 01 15 12 81 A1 02 1C 1C
  {
    .param [1]/*0800008C*/ 
    .custom /*0C0000F1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_Items

  .method /*060000A2*/ public hidebysig specialname 
          instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ 
          get_Application() cil managed
  // SIG: 20 00 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_Application

  .method /*060000A3*/ public hidebysig specialname 
          instance void  set_Application(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_Application

  .method /*060000A4*/ assembly hidebysig specialname 
          instance class System.IO.Pipelines.PipeWriterStream/*02000008*/ 
          get_ApplicationStream() cil managed
  // SIG: 20 00 12 20
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_ApplicationStream

  .method /*060000A5*/ public hidebysig specialname virtual 
          instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ 
          get_Transport() cil managed
  // SIG: 20 00 12 80 C1
  {
    .custom /*0C000109:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_Transport

  .method /*060000A6*/ public hidebysig specialname virtual 
          instance void  set_Transport(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C1
  {
    .custom /*0C00010A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_Transport

  .method /*060000A7*/ public hidebysig newslot specialname virtual final 
          instance valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/ 
          get_SupportedFormats() cil managed
  // SIG: 20 00 11 81 A5
  {
    .custom /*0C00010B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_SupportedFormats

  .method /*060000A8*/ public hidebysig specialname 
          instance void  set_SupportedFormats(valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/ 'value') cil managed
  // SIG: 20 01 01 11 81 A5
  {
    .custom /*0C00010C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_SupportedFormats

  .method /*060000A9*/ public hidebysig newslot specialname virtual final 
          instance valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/ 
          get_ActiveFormat() cil managed
  // SIG: 20 00 11 81 A5
  {
    .custom /*0C00010D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_ActiveFormat

  .method /*060000AA*/ public hidebysig newslot specialname virtual final 
          instance void  set_ActiveFormat(valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/ 'value') cil managed
  // SIG: 20 01 01 11 81 A5
  {
    .custom /*0C00010E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_ActiveFormat

  .method /*060000AB*/ public hidebysig newslot specialname virtual final 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ 
          get_HttpContext() cil managed
  // SIG: 20 00 12 81 21
  {
    .custom /*0C00010F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000110:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_HttpContext

  .method /*060000AC*/ public hidebysig newslot specialname virtual final 
          instance void  set_HttpContext(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ 'value') cil managed
  // SIG: 20 01 01 12 81 21
  {
    .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000112:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_HttpContext

  .method /*060000AD*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ 
          get_ConnectionClosed() cil managed
  // SIG: 20 00 11 7D
  {
    .custom /*0C000113:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_ConnectionClosed

  .method /*060000AE*/ public hidebysig specialname virtual 
          instance void  set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ 'value') cil managed
  // SIG: 20 01 01 11 7D
  {
    .custom /*0C000114:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_ConnectionClosed

  .method /*060000AF*/ public hidebysig newslot specialname virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ 
          get_ConnectionClosedRequested() cil managed
  // SIG: 20 00 11 7D
  {
    .custom /*0C000117:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::get_ConnectionClosedRequested

  .method /*060000B0*/ public hidebysig newslot specialname virtual final 
          instance void  set_ConnectionClosedRequested(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ 'value') cil managed
  // SIG: 20 01 01 11 7D
  {
    .custom /*0C000118:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::set_ConnectionClosedRequested

  .method /*060000B1*/ public hidebysig virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::Abort

  .method /*060000B2*/ public hidebysig newslot virtual final 
          instance void  OnHeartbeat(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<object> action,
                                     object state) cil managed
  // SIG: 20 02 01 15 12 81 05 01 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::OnHeartbeat

  .method /*060000B3*/ public hidebysig instance void 
          TickHeartbeat() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::TickHeartbeat

  .method /*060000B4*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          DisposeAsync([opt] bool closeGracefully) cil managed
  // SIG: 20 01 12 80 A9 02
  {
    .custom /*0C000119:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 59 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..YMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 48 74 74 70 43 6F 6E 6E 65 63 74 69 6F 6E   // l.HttpConnection
                                                                                                                                                                                                                                        43 6F 6E 74 65 78 74 2B 3C 44 69 73 70 6F 73 65   // Context+<Dispose
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 31 32 32 00 00 )       // Async>d__122..
    .param [1]/*08000096*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::DisposeAsync

  .method /*060000B5*/ assembly hidebysig 
          instance bool  TryActivatePersistentConnection(class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionDelegate/*0100006A*/ connectionDelegate,
                                                         class Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport/*02000024*/ transport,
                                                         class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context,
                                                         class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ dispatcherLogger) cil managed
  // SIG: 20 04 02 12 81 A9 12 80 90 12 81 21 12 81 89
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::TryActivatePersistentConnection

  .method /*060000B6*/ public hidebysig instance bool 
          TryActivateLongPollingConnection(class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionDelegate/*0100006A*/ connectionDelegate,
                                           class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ nonClonedContext,
                                           valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003F*/ pollTimeout,
                                           class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ currentRequestTask,
                                           class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006B*/ loggerFactory,
                                           class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ dispatcherLogger) cil managed
  // SIG: 20 06 02 12 81 A9 12 81 21 11 80 FD 12 80 A9 12 81 AD 12 81 89
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::TryActivateLongPollingConnection

  .method /*060000B7*/ assembly hidebysig 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000045*/<bool> 
          CancelPreviousPoll(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context) cil managed
  // SIG: 20 01 15 12 81 15 01 02 12 81 21
  {
    .custom /*0C00011A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 5F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // .._Microsoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 48 74 74 70 43 6F 6E 6E 65 63 74 69 6F 6E   // l.HttpConnection
                                                                                                                                                                                                                                        43 6F 6E 74 65 78 74 2B 3C 43 61 6E 63 65 6C 50   // Context+<CancelP
                                                                                                                                                                                                                                        72 65 76 69 6F 75 73 50 6F 6C 6C 3E 64 5F 5F 31   // reviousPoll>d__1
                                                                                                                                                                                                                                        32 37 00 00 )                                     // 27..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::CancelPreviousPoll

  .method /*060000B8*/ public hidebysig instance void 
          MarkInactive() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::MarkInactive

  .method /*060000B9*/ assembly hidebysig 
          instance void  StartSendCancellation() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::StartSendCancellation

  .method /*060000BA*/ assembly hidebysig 
          instance void  TryCancelSend(int64 currentTicks) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::TryCancelSend

  .method /*060000BB*/ assembly hidebysig 
          instance void  StopSendCancellation() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::StopSendCancellation

  .method /*060000BC*/ public hidebysig newslot virtual final 
          instance void  RequestClose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionContext::RequestClose

  .property /*17000023*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/
          SendingToken()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_SendingToken() /* 0600007E */
  } // end of property HttpConnectionContext::SendingToken
  .property /*17000024*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000063*/
          Cancellation()
  {
    .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000063*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_Cancellation() /* 06000080 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_Cancellation(class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000063*/) /* 06000081 */
  } // end of property HttpConnectionContext::Cancellation
  .property /*17000025*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/
          TransportType()
  {
    .get instance valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_TransportType() /* 06000082 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_TransportType(valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/) /* 06000083 */
  } // end of property HttpConnectionContext::TransportType
  .property /*17000026*/ instance callconv(8) class [System.Threading/*2300000D*/]System.Threading.SemaphoreSlim/*01000064*/
          WriteLock()
  {
    .get instance class [System.Threading/*2300000D*/]System.Threading.SemaphoreSlim/*01000064*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_WriteLock() /* 06000084 */
  } // end of property HttpConnectionContext::WriteLock
  .property /*17000027*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/
          DisposeAndRemoveTask()
  {
    .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_DisposeAndRemoveTask() /* 06000085 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_DisposeAndRemoveTask(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/) /* 06000086 */
  } // end of property HttpConnectionContext::DisposeAndRemoveTask
  .property /*17000028*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000065*/>
          ServiceScope()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000065*/> Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_ServiceScope() /* 06000087 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_ServiceScope(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000006*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000065*/>) /* 06000088 */
  } // end of property HttpConnectionContext::ServiceScope
  .property /*17000029*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/
          AuthenticationExpiration()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_AuthenticationExpiration() /* 06000089 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_AuthenticationExpiration(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000066*/) /* 0600008A */
  } // end of property HttpConnectionContext::AuthenticationExpiration
  .property /*1700002A*/ instance callconv(8) bool
          IsAuthenticationExpirationEnabled()
  {
    .get instance bool Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_IsAuthenticationExpirationEnabled() /* 0600008B */
  } // end of property HttpConnectionContext::IsAuthenticationExpirationEnabled
  .property /*1700002B*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/
          TransportTask()
  {
    .custom /*0C00007E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_TransportTask() /* 0600008C */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_TransportTask(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/) /* 0600008D */
  } // end of property HttpConnectionContext::TransportTask
  .property /*1700002C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/
          PreviousPollTask()
  {
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_PreviousPollTask() /* 0600008E */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_PreviousPollTask(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/) /* 0600008F */
  } // end of property HttpConnectionContext::PreviousPollTask
  .property /*1700002D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/
          ApplicationTask()
  {
    .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_ApplicationTask() /* 06000090 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_ApplicationTask(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/) /* 06000091 */
  } // end of property HttpConnectionContext::ApplicationTask
  .property /*1700002E*/ instance callconv(8) int64
          LastSeenTicks()
  {
    .get instance int64 Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_LastSeenTicks() /* 06000092 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_LastSeenTicks(int64) /* 06000093 */
  } // end of property HttpConnectionContext::LastSeenTicks
  .property /*1700002F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<int64>
          LastSeenTicksIfInactive()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<int64> Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_LastSeenTicksIfInactive() /* 06000094 */
  } // end of property HttpConnectionContext::LastSeenTicksIfInactive
  .property /*17000030*/ instance callconv(8) valuetype Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus/*02000023*/
          Status()
  {
    .get instance valuetype Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus/*02000023*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_Status() /* 06000095 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_Status(valuetype Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus/*02000023*/) /* 06000096 */
  } // end of property HttpConnectionContext::Status
  .property /*17000031*/ instance callconv(8) string
          ConnectionId()
  {
    .get instance string Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_ConnectionId() /* 06000097 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_ConnectionId(string) /* 06000098 */
  } // end of property HttpConnectionContext::ConnectionId
  .property /*17000032*/ instance callconv(8) string
          ConnectionToken()
  {
    .get instance string Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_ConnectionToken() /* 06000099 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_ConnectionToken(string) /* 0600009A */
  } // end of property HttpConnectionContext::ConnectionToken
  .property /*17000033*/ instance callconv(8) class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000067*/
          Features()
  {
    .get instance class [Microsoft.Extensions.Features/*2300000E*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000067*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_Features() /* 0600009B */
  } // end of property HttpConnectionContext::Features
  .property /*17000034*/ instance callconv(8) class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000038*/
          User()
  {
    .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_User() /* 0600009C */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_User(class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000038*/) /* 0600009D */
  } // end of property HttpConnectionContext::User
  .property /*17000035*/ instance callconv(8) bool
          HasInherentKeepAlive()
  {
    .get instance bool Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_HasInherentKeepAlive() /* 0600009E */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_HasInherentKeepAlive(bool) /* 0600009F */
  } // end of property HttpConnectionContext::HasInherentKeepAlive
  .property /*17000036*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000068*/<object,object>
          Items()
  {
    .custom /*0C000094:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000068*/<object,object> Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_Items() /* 060000A0 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_Items(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000068*/<object,object>) /* 060000A1 */
  } // end of property HttpConnectionContext::Items
  .property /*17000037*/ instance callconv(8) class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/
          Application()
  {
    .get instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_Application() /* 060000A2 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_Application(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/) /* 060000A3 */
  } // end of property HttpConnectionContext::Application
  .property /*17000038*/ instance callconv(8) class System.IO.Pipelines.PipeWriterStream/*02000008*/
          ApplicationStream()
  {
    .get instance class System.IO.Pipelines.PipeWriterStream/*02000008*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_ApplicationStream() /* 060000A4 */
  } // end of property HttpConnectionContext::ApplicationStream
  .property /*17000039*/ instance callconv(8) class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/
          Transport()
  {
    .get instance class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_Transport() /* 060000A5 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_Transport(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/) /* 060000A6 */
  } // end of property HttpConnectionContext::Transport
  .property /*1700003A*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/
          SupportedFormats()
  {
    .get instance valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_SupportedFormats() /* 060000A7 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_SupportedFormats(valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/) /* 060000A8 */
  } // end of property HttpConnectionContext::SupportedFormats
  .property /*1700003B*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/
          ActiveFormat()
  {
    .get instance valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_ActiveFormat() /* 060000A9 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_ActiveFormat(valuetype [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.TransferFormat/*01000069*/) /* 060000AA */
  } // end of property HttpConnectionContext::ActiveFormat
  .property /*1700003C*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/
          HttpContext()
  {
    .custom /*0C00009A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_HttpContext() /* 060000AB */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_HttpContext(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/) /* 060000AC */
  } // end of property HttpConnectionContext::HttpContext
  .property /*1700003D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/
          ConnectionClosed()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_ConnectionClosed() /* 060000AD */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_ConnectionClosed(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/) /* 060000AE */
  } // end of property HttpConnectionContext::ConnectionClosed
  .property /*1700003E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/
          ConnectionClosedRequested()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::get_ConnectionClosedRequested() /* 060000AF */
    .set instance void Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/::set_ConnectionClosedRequested(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/) /* 060000B0 */
  } // end of property HttpConnectionContext::ConnectionClosedRequested
} // end of class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext

.class /*02000020*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000064:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000065:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003F*/ auto ansi nested private beforefieldinit EmptyServiceProvider
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
         implements [System.ComponentModel/*23000010*/]System.IServiceProvider/*01000071*/
  {
    .method /*0600011B*/ public hidebysig specialname static 
            class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*//EmptyServiceProvider/*0200003F*/ 
            get_Instance() cil managed
    // SIG: 00 00 12 80 FC
    {
      .custom /*0C000144:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method EmptyServiceProvider::get_Instance

    .method /*0600011C*/ public hidebysig newslot virtual final 
            instance object  GetService(class [System.Runtime/*23000001*/]System.Type/*0100002B*/ serviceType) cil managed
    // SIG: 20 01 1C 12 80 AD
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method EmptyServiceProvider::GetService

    .method /*0600011D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method EmptyServiceProvider::.ctor

    .property /*17000049*/ callconv(8) class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*//EmptyServiceProvider/*0200003F*/
            Instance()
    {
      .get class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*//EmptyServiceProvider/*0200003F*/ Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*//EmptyServiceProvider/*0200003F*/::get_Instance() /* 0600011B */
    } // end of property EmptyServiceProvider::Instance
  } // end of class EmptyServiceProvider

  .class /*02000040*/ abstract auto ansi sealed nested assembly beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*0600011E*/ public hidebysig static 
            void  ConnectionDisposed(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                     string connectionId) cil managed
    // SIG: 00 02 01 12 81 89 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ConnectionDisposed

    .method /*0600011F*/ public hidebysig static 
            void  ConnectionAlreadyActive(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                          string connectionId,
                                          string requestId) cil managed
    // SIG: 00 03 01 12 81 89 0E 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ConnectionAlreadyActive

    .method /*06000120*/ public hidebysig static 
            void  PollCanceled(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                               string connectionId,
                               string requestId) cil managed
    // SIG: 00 03 01 12 81 89 0E 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::PollCanceled

    .method /*06000121*/ public hidebysig static 
            void  EstablishedConnection(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::EstablishedConnection

    .method /*06000122*/ public hidebysig static 
            void  ResumingConnection(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ResumingConnection

    .method /*06000123*/ public hidebysig static 
            void  ReceivedBytes(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                int64 count) cil managed
    // SIG: 00 02 01 12 81 89 0A
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ReceivedBytes

    .method /*06000124*/ public hidebysig static 
            void  TransportNotSupported(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                        valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transport) cil managed
    // SIG: 00 02 01 12 81 89 11 81 31
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::TransportNotSupported

    .method /*06000125*/ public hidebysig static 
            void  CannotChangeTransport(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                        valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transport,
                                        valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ requestTransport) cil managed
    // SIG: 00 03 01 12 81 89 11 81 31 11 81 31
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::CannotChangeTransport

    .method /*06000126*/ public hidebysig static 
            void  PostNotAllowedForWebSockets(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::PostNotAllowedForWebSockets

    .method /*06000127*/ public hidebysig static 
            void  NegotiationRequest(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::NegotiationRequest

    .method /*06000128*/ public hidebysig static 
            void  ReceivedDeleteRequestForUnsupportedTransport(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                                               valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transportType) cil managed
    // SIG: 00 02 01 12 81 89 11 81 31
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ReceivedDeleteRequestForUnsupportedTransport

    .method /*06000129*/ public hidebysig static 
            void  TerminatingConnection(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::TerminatingConnection

    .method /*0600012A*/ public hidebysig static 
            void  ConnectionDisposedWhileWriteInProgress(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                                         string connectionId,
                                                         class [System.Runtime/*23000001*/]System.Exception/*01000072*/ ex) cil managed
    // SIG: 00 03 01 12 81 89 0E 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ConnectionDisposedWhileWriteInProgress

    .method /*0600012B*/ public hidebysig static 
            void  FailedToReadHttpRequestBody(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                              string connectionId,
                                              class [System.Runtime/*23000001*/]System.Exception/*01000072*/ ex) cil managed
    // SIG: 00 03 01 12 81 89 0E 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::FailedToReadHttpRequestBody

    .method /*0600012C*/ public hidebysig static 
            void  NegotiateProtocolVersionMismatch(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                                   int32 clientProtocolVersion) cil managed
    // SIG: 00 02 01 12 81 89 08
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::NegotiateProtocolVersionMismatch

    .method /*0600012D*/ public hidebysig static 
            void  InvalidNegotiateProtocolVersion(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                                  string requestedProtocolVersion) cil managed
    // SIG: 00 02 01 12 81 89 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::InvalidNegotiateProtocolVersion

  } // end of class Log

  .class /*02000041*/ auto ansi sealed nested private beforefieldinit '<DoPersistentConnection>d__11'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000A1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000066*/ public int32 '<>1__state'
    .field /*04000067*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000068*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ connection
    .field /*04000069*/ public class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionDelegate/*0100006A*/ connectionDelegate
    .field /*0400006A*/ public class Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport/*02000024*/ transport
    .field /*0400006B*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*0400006C*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*0400006D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> '<>u__1'
    .field /*0400006E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__2'
    .method /*0600012E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DoPersistentConnection>d__11'::MoveNext

    .method /*0600012F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000145:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DoPersistentConnection>d__11'::SetStateMachine

  } // end of class '<DoPersistentConnection>d__11'

  .class /*02000042*/ auto ansi sealed nested private beforefieldinit '<EnsureConnectionStateAsync>d__18'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000A3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400006F*/ public int32 '<>1__state'
    .field /*04000070*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100006D*/<bool> '<>t__builder'
    .field /*04000071*/ public valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ supportedTransports
    .field /*04000072*/ public valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ transportType
    .field /*04000073*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*04000074*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*04000075*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ connection
    .field /*04000076*/ public class Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*/ logScope
    .field /*04000077*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*06000130*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<EnsureConnectionStateAsync>d__18'::MoveNext

    .method /*06000131*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000146:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<EnsureConnectionStateAsync>d__18'::SetStateMachine

  } // end of class '<EnsureConnectionStateAsync>d__18'

  .class /*02000043*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__10'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000A4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000078*/ public int32 '<>1__state'
    .field /*04000079*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*0400007A*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*0400007B*/ public class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options
    .field /*0400007C*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*0400007D*/ public class Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*/ logScope
    .field /*0400007E*/ public class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionDelegate/*0100006A*/ connectionDelegate
    .field /*0400007F*/ private valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ '<supportedTransports>5__2'
    .field /*04000080*/ private class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ '<connection>5__3'
    .field /*04000081*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/> '<>u__1'
    .field /*04000082*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<bool> '<>u__2'
    .field /*04000083*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__3'
    .field /*04000084*/ private class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskCompletionSource/*01000073*/ '<currentRequestTcs>5__4'
    .field /*04000085*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> '<>u__4'
    .method /*06000132*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__10'::MoveNext

    .method /*06000133*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000147:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__10'::SetStateMachine

  } // end of class '<ExecuteAsync>d__10'

  .class /*02000044*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__8'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000A5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000086*/ public int32 '<>1__state'
    .field /*04000087*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000088*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*04000089*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*0400008A*/ public class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options
    .field /*0400008B*/ public class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionDelegate/*0100006A*/ connectionDelegate
    .field /*0400008C*/ private class [System.Runtime/*23000001*/]System.IDisposable/*01000058*/ '<>7__wrap1'
    .field /*0400008D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*06000134*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__8'::MoveNext

    .method /*06000135*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000148:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__8'::SetStateMachine

  } // end of class '<ExecuteAsync>d__8'

  .class /*02000045*/ auto ansi sealed nested private beforefieldinit '<ExecuteNegotiateAsync>d__9'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000A6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400008E*/ public int32 '<>1__state'
    .field /*0400008F*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000090*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*04000091*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*04000092*/ public class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options
    .field /*04000093*/ private class [System.Runtime/*23000001*/]System.IDisposable/*01000058*/ '<>7__wrap1'
    .field /*04000094*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*06000136*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteNegotiateAsync>d__9'::MoveNext

    .method /*06000137*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000149:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteNegotiateAsync>d__9'::SetStateMachine

  } // end of class '<ExecuteNegotiateAsync>d__9'

  .class /*02000046*/ auto ansi sealed nested private beforefieldinit '<GetConnectionAsync>d__22'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000A8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000095*/ public int32 '<>1__state'
    .field /*04000096*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100006D*/<class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/> '<>t__builder'
    .field /*04000097*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*04000098*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*04000099*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*06000138*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetConnectionAsync>d__22'::MoveNext

    .method /*06000139*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00014A:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetConnectionAsync>d__22'::SetStateMachine

  } // end of class '<GetConnectionAsync>d__22'

  .class /*02000047*/ auto ansi sealed nested private beforefieldinit '<GetOrCreateConnectionAsync>d__23'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000A9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400009A*/ public int32 '<>1__state'
    .field /*0400009B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100006D*/<class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/> '<>t__builder'
    .field /*0400009C*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*0400009D*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*0400009E*/ public class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options
    .field /*0400009F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*0600013A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetOrCreateConnectionAsync>d__23'::MoveNext

    .method /*0600013B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00014B:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetOrCreateConnectionAsync>d__23'::SetStateMachine

  } // end of class '<GetOrCreateConnectionAsync>d__23'

  .class /*02000048*/ auto ansi sealed nested private beforefieldinit '<ProcessDeleteAsync>d__17'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000AA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000A0*/ public int32 '<>1__state'
    .field /*040000A1*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000A2*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*040000A3*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*040000A4*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/> '<>u__1'
    .field /*040000A5*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__2'
    .method /*0600013C*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessDeleteAsync>d__17'::MoveNext

    .method /*0600013D*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00014C:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessDeleteAsync>d__17'::SetStateMachine

  } // end of class '<ProcessDeleteAsync>d__17'

  .class /*02000049*/ auto ansi sealed nested private beforefieldinit '<ProcessNegotiate>d__12'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000AB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000A6*/ public int32 '<>1__state'
    .field /*040000A7*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000A8*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*040000A9*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*040000AA*/ public class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options
    .field /*040000AB*/ public class Microsoft.AspNetCore.Http.Connections.Internal.ConnectionLogScope/*0200001E*/ logScope
    .field /*040000AC*/ private class Microsoft.AspNetCore.Internal.MemoryBufferWriter/*02000013*/ '<writer>5__2'
    .field /*040000AD*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*0600013E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessNegotiate>d__12'::MoveNext

    .method /*0600013F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00014D:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessNegotiate>d__12'::SetStateMachine

  } // end of class '<ProcessNegotiate>d__12'

  .class /*0200004A*/ auto ansi sealed nested private beforefieldinit '<ProcessSend>d__16'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000AC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000AE*/ public int32 '<>1__state'
    .field /*040000AF*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000B0*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ '<>4__this'
    .field /*040000B1*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*040000B2*/ private class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ '<connection>5__2'
    .field /*040000B3*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/> '<>u__1'
    .field /*040000B4*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__2'
    .method /*06000140*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessSend>d__16'::MoveNext

    .method /*06000141*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00014E:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessSend>d__16'::SetStateMachine

  } // end of class '<ProcessSend>d__16'

  .method /*060000BD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionManager/*02000021*/ manager,
                               class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006B*/ loggerFactory) cil managed
  // SIG: 20 02 01 12 80 84 12 81 AD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcher::.ctor

  .method /*060000BE*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context,
                       class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options,
                       class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionDelegate/*0100006A*/ connectionDelegate) cil managed
  // SIG: 20 03 12 80 A9 12 81 21 12 64 12 81 A9
  {
    .custom /*0C00011B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 5A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..ZMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 48 74 74 70 43 6F 6E 6E 65 63 74 69 6F 6E   // l.HttpConnection
                                                                                                                                                                                                                                        44 69 73 70 61 74 63 68 65 72 2B 3C 45 78 65 63   // Dispatcher+<Exec
                                                                                                                                                                                                                                        75 74 65 41 73 79 6E 63 3E 64 5F 5F 38 00 00 )    // uteAsync>d__8..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcher::ExecuteAsync

  .method /*060000BF*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          ExecuteNegotiateAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context,
                                class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options) cil managed
  // SIG: 20 02 12 80 A9 12 81 21 12 64
  {
    .custom /*0C00011C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 63 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..cMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 48 74 74 70 43 6F 6E 6E 65 63 74 69 6F 6E   // l.HttpConnection
                                                                                                                                                                                                                                        44 69 73 70 61 74 63 68 65 72 2B 3C 45 78 65 63   // Dispatcher+<Exec
                                                                                                                                                                                                                                        75 74 65 4E 65 67 6F 74 69 61 74 65 41 73 79 6E   // uteNegotiateAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 39 00 00 )                         // c>d__9..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionDispatcher::ExecuteNegotiateAsync

} // end of class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher

.class /*02000021*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionManager
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000067:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200004B*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .method /*06000142*/ public hidebysig static 
            void  CreatedNewConnection(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                       string connectionId) cil managed
    // SIG: 00 02 01 12 81 89 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::CreatedNewConnection

    .method /*06000143*/ public hidebysig static 
            void  RemovedConnection(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                    string connectionId) cil managed
    // SIG: 00 02 01 12 81 89 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::RemovedConnection

    .method /*06000144*/ public hidebysig static 
            void  FailedDispose(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                string connectionId,
                                class [System.Runtime/*23000001*/]System.Exception/*01000072*/ exception) cil managed
    // SIG: 00 03 01 12 81 89 0E 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::FailedDispose

    .method /*06000145*/ public hidebysig static 
            void  ConnectionTimedOut(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                     string connectionId) cil managed
    // SIG: 00 02 01 12 81 89 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ConnectionTimedOut

    .method /*06000146*/ public hidebysig static 
            void  ConnectionReset(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                  string connectionId,
                                  class [System.Runtime/*23000001*/]System.Exception/*01000072*/ exception) cil managed
    // SIG: 00 03 01 12 81 89 0E 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ConnectionReset

    .method /*06000147*/ public hidebysig static 
            void  ScanningConnectionsFailed(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                            class [System.Runtime/*23000001*/]System.Exception/*01000072*/ exception) cil managed
    // SIG: 00 02 01 12 81 89 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ScanningConnectionsFailed

    .method /*06000148*/ public hidebysig static 
            void  HeartBeatStarted(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::HeartBeatStarted

    .method /*06000149*/ public hidebysig static 
            void  HeartBeatEnded(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::HeartBeatEnded

    .method /*0600014A*/ public hidebysig static 
            void  AuthenticationExpired(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                        string connectionId) cil managed
    // SIG: 00 02 01 12 81 89 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::AuthenticationExpired

  } // end of class Log

  .class /*0200004C*/ auto ansi sealed nested private beforefieldinit '<DisposeAndRemoveAsync>d__16'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000AD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000B5*/ public int32 '<>1__state'
    .field /*040000B6*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000B7*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ connection
    .field /*040000B8*/ public bool closeGracefully
    .field /*040000B9*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionManager/*02000021*/ '<>4__this'
    .field /*040000BA*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .method /*0600014B*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAndRemoveAsync>d__16'::MoveNext

    .method /*0600014C*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C00014F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAndRemoveAsync>d__16'::SetStateMachine

  } // end of class '<DisposeAndRemoveAsync>d__16'

  .class /*0200004D*/ auto ansi sealed nested private beforefieldinit '<ExecuteTimerLoop>d__13'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000AE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000BB*/ public int32 '<>1__state'
    .field /*040000BC*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000BD*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionManager/*02000021*/ '<>4__this'
    .field /*040000BE*/ private class [System.Runtime/*23000001*/]System.Threading.PeriodicTimer/*01000077*/ '<>7__wrap1'
    .field /*040000BF*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002F*/<bool> '<>u__1'
    .method /*0600014D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteTimerLoop>d__13'::MoveNext

    .method /*0600014E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000150:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteTimerLoop>d__13'::SetStateMachine

  } // end of class '<ExecuteTimerLoop>d__13'

  .method /*060000C0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006B*/ loggerFactory,
                               class [Microsoft.Extensions.Hosting.Abstractions/*23000011*/]Microsoft.Extensions.Hosting.IHostApplicationLifetime/*01000074*/ appLifetime,
                               class [Microsoft.Extensions.Options/*23000007*/]Microsoft.Extensions.Options.IOptions`1/*01000075*/<class Microsoft.AspNetCore.Http.Connections.ConnectionOptions/*02000016*/> connectionOptions) cil managed
  // SIG: 20 03 01 12 81 AD 12 81 D1 15 12 81 D5 01 12 58
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::.ctor

  .method /*060000C1*/ public hidebysig instance void 
          Start() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::Start

  .method /*060000C2*/ assembly hidebysig 
          instance bool  TryGetConnection(string id,
                                          [out] class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/& connection) cil managed
  // SIG: 20 02 02 0E 10 12 7C
  {
    .param [2]/*080000AE*/ 
    .custom /*0C000115:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000116:0A000024*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000076*/::.ctor(bool) /* 0A000024 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::TryGetConnection

  .method /*060000C3*/ assembly hidebysig 
          instance class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ 
          CreateConnection() cil managed
  // SIG: 20 00 12 7C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::CreateConnection

  .method /*060000C4*/ assembly hidebysig 
          instance class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ 
          CreateConnection(class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options,
                           [opt] int32 negotiateVersion) cil managed
  // SIG: 20 02 12 7C 12 64 08
  {
    .param [2]/*080000B0*/  = int32(0x00000000)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::CreateConnection

  .method /*060000C5*/ public hidebysig instance void 
          RemoveConnection(string id) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::RemoveConnection

  .method /*060000C6*/ public hidebysig instance void 
          Scan() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::Scan

  .method /*060000C7*/ public hidebysig instance void 
          CloseConnections() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::CloseConnections

  .method /*060000C8*/ assembly hidebysig 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          DisposeAndRemoveAsync(class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ connection,
                                bool closeGracefully) cil managed
  // SIG: 20 02 12 80 A9 12 7C 02
  {
    .custom /*0C00011E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 61 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..aMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 48 74 74 70 43 6F 6E 6E 65 63 74 69 6F 6E   // l.HttpConnection
                                                                                                                                                                                                                                        4D 61 6E 61 67 65 72 2B 3C 44 69 73 70 6F 73 65   // Manager+<Dispose
                                                                                                                                                                                                                                        41 6E 64 52 65 6D 6F 76 65 41 73 79 6E 63 3E 64   // AndRemoveAsync>d
                                                                                                                                                                                                                                        5F 5F 31 36 00 00 )                               // __16..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionManager::DisposeAndRemoveAsync

} // end of class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionManager

.class /*02000022*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionsEventSource
       extends [System.Diagnostics.Tracing/*23000012*/]System.Diagnostics.Tracing.EventSource/*01000078*/
{
  .custom /*0C00006B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*0400000C*/ public static initonly class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionsEventSource/*02000022*/ Log
  .method /*060000C9*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionsEventSource::.ctor

  .method /*060000CA*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(string eventSourceName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionsEventSource::.ctor

  .method /*060000CB*/ public hidebysig instance void 
          ConnectionStop(string connectionId,
                         valuetype Microsoft.Extensions.Internal.ValueStopwatch/*02000010*/ timer) cil managed
  // SIG: 20 02 01 0E 11 40
  {
    .custom /*0C000120:0A000025*/ instance void [System.Diagnostics.Tracing/*23000012*/]System.Diagnostics.Tracing.NonEventAttribute/*01000079*/::.ctor() /* 0A000025 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionsEventSource::ConnectionStop

  .method /*060000CC*/ public hidebysig instance valuetype Microsoft.Extensions.Internal.ValueStopwatch/*02000010*/ 
          ConnectionStart(string connectionId) cil managed
  // SIG: 20 01 11 40 0E
  {
    .custom /*0C000121:0A000026*/ instance void [System.Diagnostics.Tracing/*23000012*/]System.Diagnostics.Tracing.EventAttribute/*0100007A*/::.ctor(int32) /* 0A000026 */ = ( 01 00 01 00 00 00 02 00 54 55 80 84 53 79 73 74   // ........TU..Syst
                                                                                                                                                                               65 6D 2E 44 69 61 67 6E 6F 73 74 69 63 73 2E 54   // em.Diagnostics.T
                                                                                                                                                                               72 61 63 69 6E 67 2E 45 76 65 6E 74 4C 65 76 65   // racing.EventLeve
                                                                                                                                                                               6C 2C 20 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // l, System.Diagno
                                                                                                                                                                               73 74 69 63 73 2E 54 72 61 63 69 6E 67 2C 20 56   // stics.Tracing, V
                                                                                                                                                                               65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                               43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                               20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                               62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                               05 4C 65 76 65 6C 04 00 00 00 54 0E 07 4D 65 73   // .Level....T..Mes
                                                                                                                                                                               73 61 67 65 19 53 74 61 72 74 65 64 20 63 6F 6E   // sage.Started con
                                                                                                                                                                               6E 65 63 74 69 6F 6E 20 27 7B 30 7D 27 2E )       // nection '{0}'.
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionsEventSource::ConnectionStart

  .method /*060000CD*/ public hidebysig instance void 
          ConnectionTimedOut(string connectionId) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000122:0A000026*/ instance void [System.Diagnostics.Tracing/*23000012*/]System.Diagnostics.Tracing.EventAttribute/*0100007A*/::.ctor(int32) /* 0A000026 */ = ( 01 00 03 00 00 00 02 00 54 55 80 84 53 79 73 74   // ........TU..Syst
                                                                                                                                                                               65 6D 2E 44 69 61 67 6E 6F 73 74 69 63 73 2E 54   // em.Diagnostics.T
                                                                                                                                                                               72 61 63 69 6E 67 2E 45 76 65 6E 74 4C 65 76 65   // racing.EventLeve
                                                                                                                                                                               6C 2C 20 53 79 73 74 65 6D 2E 44 69 61 67 6E 6F   // l, System.Diagno
                                                                                                                                                                               73 74 69 63 73 2E 54 72 61 63 69 6E 67 2C 20 56   // stics.Tracing, V
                                                                                                                                                                               65 72 73 69 6F 6E 3D 36 2E 30 2E 30 2E 30 2C 20   // ersion=6.0.0.0, 
                                                                                                                                                                               43 75 6C 74 75 72 65 3D 6E 65 75 74 72 61 6C 2C   // Culture=neutral,
                                                                                                                                                                               20 50 75 62 6C 69 63 4B 65 79 54 6F 6B 65 6E 3D   //  PublicKeyToken=
                                                                                                                                                                               62 30 33 66 35 66 37 66 31 31 64 35 30 61 33 61   // b03f5f7f11d50a3a
                                                                                                                                                                               05 4C 65 76 65 6C 04 00 00 00 54 0E 07 4D 65 73   // .Level....T..Mes
                                                                                                                                                                               73 61 67 65 1B 43 6F 6E 6E 65 63 74 69 6F 6E 20   // sage.Connection 
                                                                                                                                                                               27 7B 30 7D 27 20 74 69 6D 65 64 20 6F 75 74 2E ) // '{0}' timed out.
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionsEventSource::ConnectionTimedOut

  .method /*060000CE*/ family hidebysig virtual 
          instance void  OnEventCommand(class [System.Diagnostics.Tracing/*23000012*/]System.Diagnostics.Tracing.EventCommandEventArgs/*0100007C*/ command) cil managed
  // SIG: 20 01 01 12 81 F1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpConnectionsEventSource::OnEventCommand

} // end of class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionsEventSource

.class /*02000023*/ private auto ansi sealed Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus
       extends [System.Runtime/*23000001*/]System.Enum/*0100007D*/
{
  .field /*0400000D*/ public specialname rtspecialname int32 value__
  .field /*0400000E*/ public static literal valuetype Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus/*02000023*/ Inactive = int32(0x00000000)
  .field /*0400000F*/ public static literal valuetype Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus/*02000023*/ Active = int32(0x00000001)
  .field /*04000010*/ public static literal valuetype Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus/*02000023*/ Disposed = int32(0x00000002)
} // end of class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionStatus

.class /*02000024*/ interface private abstract auto ansi Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport
{
  .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*060000CF*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          ProcessRequestAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ token) cil managed
  // SIG: 20 02 12 80 A9 12 81 21 11 7D
  {
    // Method begins at RVA 0x0
  } // end of method IHttpTransport::ProcessRequestAsync

} // end of class Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport

.class /*02000025*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.Internal.Transports.LongPollingServerTransport
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport/*02000024*/
{
  .custom /*0C000070:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport/*02000024*/
  .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200004E*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .method /*0600014F*/ public hidebysig static 
            void  LongPolling204(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::LongPolling204

    .method /*06000150*/ public hidebysig static 
            void  PollTimedOut(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::PollTimedOut

    .method /*06000151*/ public hidebysig static 
            void  LongPollingWritingMessage(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                            int64 count) cil managed
    // SIG: 00 02 01 12 81 89 0A
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::LongPollingWritingMessage

    .method /*06000152*/ public hidebysig static 
            void  LongPollingDisconnected(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::LongPollingDisconnected

    .method /*06000153*/ public hidebysig static 
            void  LongPollingTerminated(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                        class [System.Runtime/*23000001*/]System.Exception/*01000072*/ ex) cil managed
    // SIG: 00 02 01 12 81 89 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::LongPollingTerminated

  } // end of class Log

  .class /*0200004F*/ auto ansi sealed nested private beforefieldinit '<ProcessRequestAsync>d__6'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000B0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000C0*/ public int32 '<>1__state'
    .field /*040000C1*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000C2*/ public class Microsoft.AspNetCore.Http.Connections.Internal.Transports.LongPollingServerTransport/*02000025*/ '<>4__this'
    .field /*040000C3*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ token
    .field /*040000C4*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*040000C5*/ private valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> '<buffer>5__2'
    .field /*040000C6*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002F*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.ReadResult/*0100007E*/> '<>u__1'
    .field /*040000C7*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000025*/ '<>u__2'
    .method /*06000154*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessRequestAsync>d__6'::MoveNext

    .method /*06000155*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000151:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessRequestAsync>d__6'::SetStateMachine

  } // end of class '<ProcessRequestAsync>d__6'

  .method /*060000D0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ timeoutToken,
                               class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeReader/*01000031*/ application,
                               class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006B*/ loggerFactory) cil managed
  // SIG: 20 03 01 11 7D 12 80 C5 12 81 AD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LongPollingServerTransport::.ctor

  .method /*060000D1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ timeoutToken,
                               class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeReader/*01000031*/ application,
                               class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006B*/ loggerFactory,
                               class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ connection) cil managed
  // SIG: 20 04 01 11 7D 12 80 C5 12 81 AD 12 7C
  {
    .param [4]/*080000C2*/ 
    .custom /*0C00011D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LongPollingServerTransport::.ctor

  .method /*060000D2*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          ProcessRequestAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ token) cil managed
  // SIG: 20 02 12 80 A9 12 81 21 11 7D
  {
    .custom /*0C000123:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 6E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..nMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 54 72 61 6E 73 70 6F 72 74 73 2E 4C 6F 6E   // l.Transports.Lon
                                                                                                                                                                                                                                        67 50 6F 6C 6C 69 6E 67 53 65 72 76 65 72 54 72   // gPollingServerTr
                                                                                                                                                                                                                                        61 6E 73 70 6F 72 74 2B 3C 50 72 6F 63 65 73 73   // ansport+<Process
                                                                                                                                                                                                                                        52 65 71 75 65 73 74 41 73 79 6E 63 3E 64 5F 5F   // RequestAsync>d__
                                                                                                                                                                                                                                        36 00 00 )                                        // 6..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LongPollingServerTransport::ProcessRequestAsync

} // end of class Microsoft.AspNetCore.Http.Connections.Internal.Transports.LongPollingServerTransport

.class /*02000026*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.Internal.Transports.ServerSentEventsServerTransport
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport/*02000024*/
{
  .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport/*02000024*/
  .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000050*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .method /*06000156*/ public hidebysig static 
            void  SSEWritingMessage(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                    int64 count) cil managed
    // SIG: 00 02 01 12 81 89 0A
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::SSEWritingMessage

  } // end of class Log

  .class /*02000051*/ auto ansi sealed nested private beforefieldinit '<ProcessRequestAsync>d__6'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000B3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000C8*/ public int32 '<>1__state'
    .field /*040000C9*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000CA*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*040000CB*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken
    .field /*040000CC*/ public class Microsoft.AspNetCore.Http.Connections.Internal.Transports.ServerSentEventsServerTransport/*02000026*/ '<>4__this'
    .field /*040000CD*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__1'
    .field /*040000CE*/ private valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> '<buffer>5__2'
    .field /*040000CF*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002F*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.ReadResult/*0100007E*/> '<>u__2'
    .method /*06000157*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessRequestAsync>d__6'::MoveNext

    .method /*06000158*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000152:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessRequestAsync>d__6'::SetStateMachine

  } // end of class '<ProcessRequestAsync>d__6'

  .method /*060000D3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeReader/*01000031*/ application,
                               string connectionId,
                               class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006B*/ loggerFactory) cil managed
  // SIG: 20 03 01 12 80 C5 0E 12 81 AD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServerSentEventsServerTransport::.ctor

  .method /*060000D4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.PipeReader/*01000031*/ application,
                               string connectionId,
                               class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ connection,
                               class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006B*/ loggerFactory) cil managed
  // SIG: 20 04 01 12 80 C5 0E 12 7C 12 81 AD
  {
    .param [3]/*080000CA*/ 
    .custom /*0C00011F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServerSentEventsServerTransport::.ctor

  .method /*060000D5*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          ProcessRequestAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 A9 12 81 21 11 7D
  {
    .custom /*0C000124:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 73 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..sMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 54 72 61 6E 73 70 6F 72 74 73 2E 53 65 72   // l.Transports.Ser
                                                                                                                                                                                                                                        76 65 72 53 65 6E 74 45 76 65 6E 74 73 53 65 72   // verSentEventsSer
                                                                                                                                                                                                                                        76 65 72 54 72 61 6E 73 70 6F 72 74 2B 3C 50 72   // verTransport+<Pr
                                                                                                                                                                                                                                        6F 63 65 73 73 52 65 71 75 65 73 74 41 73 79 6E   // ocessRequestAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 36 00 00 )                         // c>d__6..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServerSentEventsServerTransport::ProcessRequestAsync

} // end of class Microsoft.AspNetCore.Http.Connections.Internal.Transports.ServerSentEventsServerTransport

.class /*02000027*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Connections.Internal.Transports.WebSocketsServerTransport
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport/*02000024*/
{
  .custom /*0C000077:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Http.Connections.Internal.Transports.IHttpTransport/*02000024*/
  .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000052*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .method /*06000159*/ public hidebysig static 
            void  SocketOpened(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                               string subProtocol) cil managed
    // SIG: 00 02 01 12 81 89 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::SocketOpened

    .method /*0600015A*/ public hidebysig static 
            void  SocketClosed(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::SocketClosed

    .method /*0600015B*/ public hidebysig static 
            void  ClientClosed(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000046*/<valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketCloseStatus/*0100007F*/> closeStatus,
                               string closeDescription) cil managed
    // SIG: 00 03 01 12 81 89 15 11 81 19 01 11 81 FD 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ClientClosed

    .method /*0600015C*/ public hidebysig static 
            void  WaitingForSend(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::WaitingForSend

    .method /*0600015D*/ public hidebysig static 
            void  FailedSending(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::FailedSending

    .method /*0600015E*/ public hidebysig static 
            void  FinishedSending(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::FinishedSending

    .method /*0600015F*/ public hidebysig static 
            void  WaitingForClose(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::WaitingForClose

    .method /*06000160*/ public hidebysig static 
            void  CloseTimedOut(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger) cil managed
    // SIG: 00 01 01 12 81 89
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::CloseTimedOut

    .method /*06000161*/ public hidebysig static 
            void  MessageReceived(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                  valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocketMessageType/*0100001E*/ 'type',
                                  int32 size,
                                  bool endOfMessage) cil managed
    // SIG: 00 04 01 12 81 89 11 79 08 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::MessageReceived

    .method /*06000162*/ public hidebysig static 
            void  MessageToApplication(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                       int32 size) cil managed
    // SIG: 00 02 01 12 81 89 08
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::MessageToApplication

    .method /*06000163*/ public hidebysig static 
            void  SendPayload(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                              int64 size) cil managed
    // SIG: 00 02 01 12 81 89 0A
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::SendPayload

    .method /*06000164*/ public hidebysig static 
            void  ErrorWritingFrame(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                    class [System.Runtime/*23000001*/]System.Exception/*01000072*/ ex) cil managed
    // SIG: 00 02 01 12 81 89 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ErrorWritingFrame

    .method /*06000165*/ public hidebysig static 
            void  ClosedPrematurely(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                    class [System.Runtime/*23000001*/]System.Exception/*01000072*/ ex) cil managed
    // SIG: 00 02 01 12 81 89 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ClosedPrematurely

    .method /*06000166*/ public hidebysig static 
            void  ClosingWebSocketFailed(class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILogger/*01000062*/ logger,
                                         class [System.Runtime/*23000001*/]System.Exception/*01000072*/ ex) cil managed
    // SIG: 00 02 01 12 81 89 12 81 C9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ClosingWebSocketFailed

  } // end of class Log

  .class /*02000053*/ auto ansi sealed nested private beforefieldinit '<ProcessRequestAsync>d__6'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000B6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000D0*/ public int32 '<>1__state'
    .field /*040000D1*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000D2*/ public class Microsoft.AspNetCore.Http.Connections.Internal.Transports.WebSocketsServerTransport/*02000027*/ '<>4__this'
    .field /*040000D3*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context
    .field /*040000D4*/ private string '<subProtocol>5__2'
    .field /*040000D5*/ private class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*0100001C*/ '<ws>5__3'
    .field /*040000D6*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*0100001C*/> '<>u__1'
    .field /*040000D7*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__2'
    .method /*06000167*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessRequestAsync>d__6'::MoveNext

    .method /*06000168*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000153:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessRequestAsync>d__6'::SetStateMachine

  } // end of class '<ProcessRequestAsync>d__6'

  .class /*02000054*/ auto ansi sealed nested private beforefieldinit '<ProcessSocketAsync>d__7'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000B7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000D8*/ public int32 '<>1__state'
    .field /*040000D9*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000DA*/ public class Microsoft.AspNetCore.Http.Connections.Internal.Transports.WebSocketsServerTransport/*02000027*/ '<>4__this'
    .field /*040000DB*/ public class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*0100001C*/ socket
    .field /*040000DC*/ private class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ '<receiving>5__2'
    .field /*040000DD*/ private class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ '<sending>5__3'
    .field /*040000DE*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000070*/<class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> '<>u__1'
    .field /*040000DF*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*01000063*/ '<delayCts>5__4'
    .method /*06000169*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessSocketAsync>d__7'::MoveNext

    .method /*0600016A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000154:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ProcessSocketAsync>d__7'::SetStateMachine

  } // end of class '<ProcessSocketAsync>d__7'

  .class /*02000055*/ auto ansi sealed nested private beforefieldinit '<StartReceiving>d__8'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000B9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000E0*/ public int32 '<>1__state'
    .field /*040000E1*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000E2*/ public class Microsoft.AspNetCore.Http.Connections.Internal.Transports.WebSocketsServerTransport/*02000027*/ '<>4__this'
    .field /*040000E3*/ public class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*0100001C*/ socket
    .field /*040000E4*/ private valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ '<token>5__2'
    .field /*040000E5*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002F*/<valuetype [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.ValueWebSocketReceiveResult/*01000080*/> '<>u__1'
    .field /*040000E6*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002F*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.FlushResult/*0100002E*/> '<>u__2'
    .method /*0600016B*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartReceiving>d__8'::MoveNext

    .method /*0600016C*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000155:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartReceiving>d__8'::SetStateMachine

  } // end of class '<StartReceiving>d__8'

  .class /*02000056*/ auto ansi sealed nested private beforefieldinit '<StartSending>d__9'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/
  {
    .custom /*0C0000BA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000E7*/ public int32 '<>1__state'
    .field /*040000E8*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000E9*/ public class Microsoft.AspNetCore.Http.Connections.Internal.Transports.WebSocketsServerTransport/*02000027*/ '<>4__this'
    .field /*040000EA*/ public class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*0100001C*/ socket
    .field /*040000EB*/ private class [System.Runtime/*23000001*/]System.Exception/*01000072*/ '<error>5__2'
    .field /*040000EC*/ private object '<>7__wrap2'
    .field /*040000ED*/ private int32 '<>7__wrap3'
    .field /*040000EE*/ private valuetype [System.Memory/*23000003*/]System.Buffers.ReadOnlySequence`1/*0100001D*/<uint8> '<buffer>5__5'
    .field /*040000EF*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002F*/<valuetype [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.ReadResult/*0100007E*/> '<>u__1'
    .field /*040000F0*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000025*/ '<>u__2'
    .field /*040000F1*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100004E*/ '<>u__3'
    .method /*0600016D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::MoveNext /*01000021::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartSending>d__9'::MoveNext

    .method /*0600016E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 85
    {
      .custom /*0C000156:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000026*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000021*/::SetStateMachine /*01000021::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StartSending>d__9'::SetStateMachine

  } // end of class '<StartSending>d__9'

  .method /*060000D6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Http.Connections.WebSocketOptions/*0200001D*/ options,
                               class [System.IO.Pipelines/*23000004*/]System.IO.Pipelines.IDuplexPipe/*01000030*/ application,
                               class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionContext/*0200001F*/ connection,
                               class [Microsoft.Extensions.Logging.Abstractions/*2300000C*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100006B*/ loggerFactory) cil managed
  // SIG: 20 04 01 12 74 12 80 C1 12 7C 12 81 AD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketsServerTransport::.ctor

  .method /*060000D7*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          ProcessRequestAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ context,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100001F*/ token) cil managed
  // SIG: 20 02 12 80 A9 12 81 21 11 7D
  {
    .custom /*0C000125:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 6D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..mMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 54 72 61 6E 73 70 6F 72 74 73 2E 57 65 62   // l.Transports.Web
                                                                                                                                                                                                                                        53 6F 63 6B 65 74 73 53 65 72 76 65 72 54 72 61   // SocketsServerTra
                                                                                                                                                                                                                                        6E 73 70 6F 72 74 2B 3C 50 72 6F 63 65 73 73 52   // nsport+<ProcessR
                                                                                                                                                                                                                                        65 71 75 65 73 74 41 73 79 6E 63 3E 64 5F 5F 36   // equestAsync>d__6
                                                                                                                                                                                                                                        00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketsServerTransport::ProcessRequestAsync

  .method /*060000D8*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          ProcessSocketAsync(class [System.Net.WebSockets/*23000002*/]System.Net.WebSockets.WebSocket/*0100001C*/ socket) cil managed
  // SIG: 20 01 12 80 A9 12 71
  {
    .custom /*0C000126:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002B*/) /* 0A000019 */ = ( 01 00 6C 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..lMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 43 6F 6E   // NetCore.Http.Con
                                                                                                                                                                                                                                        6E 65 63 74 69 6F 6E 73 2E 49 6E 74 65 72 6E 61   // nections.Interna
                                                                                                                                                                                                                                        6C 2E 54 72 61 6E 73 70 6F 72 74 73 2E 57 65 62   // l.Transports.Web
                                                                                                                                                                                                                                        53 6F 63 6B 65 74 73 53 65 72 76 65 72 54 72 61   // SocketsServerTra
                                                                                                                                                                                                                                        6E 73 70 6F 72 74 2B 3C 50 72 6F 63 65 73 73 53   // nsport+<ProcessS
                                                                                                                                                                                                                                        6F 63 6B 65 74 41 73 79 6E 63 3E 64 5F 5F 37 00   // ocketAsync>d__7.
                                                                                                                                                                                                                                        00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketsServerTransport::ProcessSocketAsync

} // end of class Microsoft.AspNetCore.Http.Connections.Internal.Transports.WebSocketsServerTransport

.class /*02000028*/ interface public abstract auto ansi Microsoft.AspNetCore.Http.Connections.Features.IHttpContextFeature
{
  .custom /*0C00007A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .method /*060000D9*/ public hidebysig newslot specialname abstract virtual 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ 
          get_HttpContext() cil managed
  // SIG: 20 00 12 81 21
  {
    // Method begins at RVA 0x0
  } // end of method IHttpContextFeature::get_HttpContext

  .method /*060000DA*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_HttpContext(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ 'value') cil managed
  // SIG: 20 01 01 12 81 21
  {
    // Method begins at RVA 0x0
  } // end of method IHttpContextFeature::set_HttpContext

  .property /*1700003F*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/
          HttpContext()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ Microsoft.AspNetCore.Http.Connections.Features.IHttpContextFeature/*02000028*/::get_HttpContext() /* 060000D9 */
    .set instance void Microsoft.AspNetCore.Http.Connections.Features.IHttpContextFeature/*02000028*/::set_HttpContext(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/) /* 060000DA */
  } // end of property IHttpContextFeature::HttpContext
} // end of class Microsoft.AspNetCore.Http.Connections.Features.IHttpContextFeature

.class /*02000029*/ interface public abstract auto ansi Microsoft.AspNetCore.Http.Connections.Features.IHttpTransportFeature
{
  .method /*060000DB*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ 
          get_TransportType() cil managed
  // SIG: 20 00 11 81 31
  {
    // Method begins at RVA 0x0
  } // end of method IHttpTransportFeature::get_TransportType

  .property /*17000040*/ instance callconv(8) valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/
          TransportType()
  {
    .get instance valuetype [Microsoft.AspNetCore.Http.Connections.Common/*2300000B*/]Microsoft.AspNetCore.Http.Connections.HttpTransportType/*0100004C*/ Microsoft.AspNetCore.Http.Connections.Features.IHttpTransportFeature/*02000029*/::get_TransportType() /* 060000DB */
  } // end of property IHttpTransportFeature::TransportType
} // end of class Microsoft.AspNetCore.Http.Connections.Features.IHttpTransportFeature

.class /*0200002A*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilder
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Builder.IEndpointConventionBuilder/*01000081*/
{
  .method /*060000DC*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Builder.IEndpointConventionBuilder/*01000081*/ endpointConventionBuilder) cil managed
  // SIG: 20 01 01 12 82 05
  {
    .custom /*0C000127:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionEndpointRouteBuilder::.ctor

  .method /*060000DD*/ public hidebysig newslot virtual final 
          instance void  Add(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Builder.EndpointBuilder/*01000082*/> convention) cil managed
  // SIG: 20 01 01 15 12 81 05 01 12 82 09
  {
    .custom /*0C000128:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionEndpointRouteBuilder::Add

} // end of class Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilder

.class /*0200002B*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00007B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00007D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000057*/ auto ansi nested private beforefieldinit CompositeEndpointConventionBuilder
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
         implements [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Builder.IEndpointConventionBuilder/*01000081*/
  {
    .method /*0600016F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Collections/*23000014*/]System.Collections.Generic.List`1/*01000086*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Builder.IEndpointConventionBuilder/*01000081*/> endpointConventionBuilders) cil managed
    // SIG: 20 01 01 15 12 82 19 01 12 82 05
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CompositeEndpointConventionBuilder::.ctor

    .method /*06000170*/ public hidebysig newslot virtual final 
            instance void  Add(class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Builder.EndpointBuilder/*01000082*/> convention) cil managed
    // SIG: 20 01 01 15 12 81 05 01 12 82 09
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CompositeEndpointConventionBuilder::Add

  } // end of class CompositeEndpointConventionBuilder

  .class /*02000058*/ auto ansi serializable sealed nested private beforefieldinit '<>c__2`1'<([Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionHandler/*01000085*/) TConnectionHandler>
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C0000BD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000F2*/ public static initonly class Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilderExtensions/*0200002B*//'<>c__2`1'/*02000058*/<!TConnectionHandler> '<>9'
    .field /*040000F3*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.IConnectionBuilder/*01000084*/> '<>9__2_0'
    .method /*06000171*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__2`1'::.ctor

    .method /*06000172*/ assembly hidebysig 
            instance void  '<MapConnectionHandler>b__2_0'(class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.IConnectionBuilder/*01000084*/ b) cil managed
    // SIG: 20 01 01 12 82 11
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__2`1'::'<MapConnectionHandler>b__2_0'

  } // end of class '<>c__2`1'

  .class /*02000059*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass2_0`1'<([Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionHandler/*01000085*/) TConnectionHandler>
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C0000BF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000F4*/ public object[] attributes
    .method /*06000173*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass2_0`1'::.ctor

    .method /*06000174*/ assembly hidebysig 
            instance void  '<MapConnectionHandler>b__1'(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Builder.EndpointBuilder/*01000082*/ e) cil managed
    // SIG: 20 01 01 12 82 09
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass2_0`1'::'<MapConnectionHandler>b__1'

  } // end of class '<>c__DisplayClass2_0`1'

  .class /*0200005A*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass3_0'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C0000C1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000F5*/ public class Microsoft.AspNetCore.Http.Connections.Internal.HttpConnectionDispatcher/*02000020*/ dispatcher
    .field /*040000F6*/ public class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options
    .field /*040000F7*/ public class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionDelegate/*0100006A*/ connectionDelegate
    .method /*06000175*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::.ctor

    .method /*06000176*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            '<MapConnections>b__0'(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ c) cil managed
    // SIG: 20 01 12 80 A9 12 81 21
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<MapConnections>b__0'

    .method /*06000177*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            '<MapConnections>b__1'(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Http.HttpContext/*01000048*/ c) cil managed
    // SIG: 20 01 12 80 A9 12 81 21
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<MapConnections>b__1'

    .method /*06000178*/ assembly hidebysig 
            instance void  '<MapConnections>b__2'(class [Microsoft.AspNetCore.Http.Abstractions/*23000008*/]Microsoft.AspNetCore.Builder.EndpointBuilder/*01000082*/ e) cil managed
    // SIG: 20 01 01 12 82 09
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass3_0'::'<MapConnections>b__2'

  } // end of class '<>c__DisplayClass3_0'

  .method /*060000DE*/ public hidebysig static 
          class Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilder/*0200002A*/ 
          MapConnections(class [Microsoft.AspNetCore.Routing/*23000013*/]Microsoft.AspNetCore.Routing.IEndpointRouteBuilder/*01000083*/ endpoints,
                         string pattern,
                         class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.IConnectionBuilder/*01000084*/> configure) cil managed
  // SIG: 00 03 12 80 A8 12 82 0D 0E 15 12 81 05 01 12 82 11
  {
    .custom /*0C000129:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionEndpointRouteBuilderExtensions::MapConnections

  .method /*060000DF*/ public hidebysig static 
          class Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilder/*0200002A*/ 
          MapConnectionHandler<([Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionHandler/*01000085*/) TConnectionHandler>(class [Microsoft.AspNetCore.Routing/*23000013*/]Microsoft.AspNetCore.Routing.IEndpointRouteBuilder/*01000083*/ endpoints,
                                                                                                                                                                               string pattern) cil managed
  // SIG: 10 01 02 12 80 A8 12 82 0D 0E
  {
    .custom /*0C00012A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TConnectionHandler /*2A000003*/ 
      .custom /*0C00001E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionEndpointRouteBuilderExtensions::MapConnectionHandler

  .method /*060000E0*/ public hidebysig static 
          class Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilder/*0200002A*/ 
          MapConnectionHandler<([Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.ConnectionHandler/*01000085*/) TConnectionHandler>(class [Microsoft.AspNetCore.Routing/*23000013*/]Microsoft.AspNetCore.Routing.IEndpointRouteBuilder/*01000083*/ endpoints,
                                                                                                                                                                               string pattern,
                                                                                                                                                                               class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/> configureOptions) cil managed
  // SIG: 10 01 03 12 80 A8 12 82 0D 0E 15 12 81 05 01 12 64
  {
    .custom /*0C00012C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TConnectionHandler /*2A000004*/ 
      .custom /*0C000023:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .param [3]/*080000DF*/ 
    .custom /*0C00012B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionEndpointRouteBuilderExtensions::MapConnectionHandler

  .method /*060000E1*/ public hidebysig static 
          class Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilder/*0200002A*/ 
          MapConnections(class [Microsoft.AspNetCore.Routing/*23000013*/]Microsoft.AspNetCore.Routing.IEndpointRouteBuilder/*01000083*/ endpoints,
                         string pattern,
                         class Microsoft.AspNetCore.Http.Connections.HttpConnectionDispatcherOptions/*02000019*/ options,
                         class [System.Runtime/*23000001*/]System.Action`1/*01000041*/<class [Microsoft.AspNetCore.Connections.Abstractions/*23000009*/]Microsoft.AspNetCore.Connections.IConnectionBuilder/*01000084*/> configure) cil managed
  // SIG: 00 04 12 80 A8 12 82 0D 0E 12 64 15 12 81 05 01 12 82 11
  {
    .custom /*0C00012D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConnectionEndpointRouteBuilderExtensions::MapConnections

} // end of class Microsoft.AspNetCore.Builder.ConnectionEndpointRouteBuilderExtensions

.class /*0200002C*/ private auto ansi sealed '<PrivateImplementationDetails>'
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00007F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .class /*0200005B*/ explicit ansi sealed nested private '__StaticArrayInitTypeSize=6'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000020*/
  {
    .pack 1
    .size 6
  } // end of class '__StaticArrayInitTypeSize=6'

  .field /*04000011*/ static assembly initonly valuetype '<PrivateImplementationDetails>'/*0200002C*//'__StaticArrayInitTypeSize=6'/*0200005B*/ '68FDBE4D9BA8C3E1CF7F7F4815DEDBE04E1FEF82A586FDCF18AA8EB68DE0C378' at I_0000C570
} // end of class '<PrivateImplementationDetails>'


// =============================================================

.data cil I_0000C570 = bytearray (
                 64 61 74 61 3A 20)                               // data: 
// *********** DISASSEMBLY COMPLETE ***********************
