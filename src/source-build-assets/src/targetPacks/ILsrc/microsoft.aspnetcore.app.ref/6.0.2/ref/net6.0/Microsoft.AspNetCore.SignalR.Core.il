


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
// Time-date stamp:            0xb7c62468
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x0000d400
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00011376
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
// Size of image:                  0x00016000
// Size of headers:                0x00000400
// Checksum:                       0x00011307
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00011324 [0x0000004f] address [size] of Import Directory:          
// 0x00012000 [0x00000518] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000e200 [0x00002470] address [size] of Security Directory:        
// 0x00014000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00011308 [0x0000001c] address [size] of Debug Directory:           
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
//              0x0000d37c Virtual Size
//              0x00004000 Virtual Address
//              0x0000d400 Size of Raw Data
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
//              0x00000518 Virtual Size
//              0x00012000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x0000da00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00014000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x0000e000 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00011000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000378
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00011366 Import Name Table
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
// 0x00004054 [0x0000d234] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00011288 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000076c8 Size
//              '#~' Name
//    Stream 2:
//              0x00007734 Offset
//              0x00003c54 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000b388 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x0000b38c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x0000b39c Offset
//              0x00001e98 Size
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

// File size            : 67184
// PE header size       : 1024 (536 used)    ( 1.52%)
// PE additional info   : 10831             (16.12%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.11%)
// CLR meta-data size  : 53812              (80.10%)
// CLR additional info : 128                ( 0.19%)
// CLR method headers  : 612                ( 0.91%)
// Managed code         : 1224              ( 1.82%)
// Data                 : 2560              ( 3.81%)
// Unaccounted          : -3079             (-4.58%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 54272
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 53812
//   Module        -    1 (10 bytes)
//   TypeDef       -  158 (2212 bytes)    16 interfaces, 0 explicit layout
//   TypeRef       -  126 (756 bytes)
//   MethodDef     -  679 (9506 bytes)    55 abstract, 0 native, 612 bodies
//   FieldDef      -  349 (2094 bytes)    16 constant
//   MemberRef     -   39 (234 bytes)
//   ParamDef      - 1018 (6108 bytes)
//   MethodImpl    -   72 (432 bytes)
//   Constant      -   56 (336 bytes)
//   CustomAttribute-  748 (4488 bytes)
//   InterfaceImpl -   91 (364 bytes)
//   PropertyMap   -   47 (188 bytes)
//   Property      -  136 (816 bytes)
//   MethodSemantic-  162 (972 bytes)
//   TypeSpec      -   24 (48 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   18 (360 bytes)
//   NestedClass   -   63 (252 bytes)
//   GenericParam  -  115 (920 bytes)
//   GenericParamConstraint-   46 (184 bytes)
//   Strings       - 15444 bytes
//   Blobs         -  7832 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   214 bytes

// CLR additional info : 128

// CLR method headers : 612
//   Num.of method bodies  - 612
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 612

// Managed code : 1224
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class ClosedGenericMatcher           (abstract) (auto) (ansi) (sealed) 
// Class AwaitableInfo                  (sequential) (ansi) (sealed) 
// Class CoercedAwaitableInfo           (sequential) (ansi) (sealed) 
// Class ObjectMethodExecutor           (auto) (ansi) 
// Class ObjectMethodExecutorAwaitable  (sequential) (ansi) (sealed) 
// Class ObjectMethodExecutorFSharpSupport (abstract) (auto) (ansi) (sealed) 
// Class SignalRDependencyInjectionExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class TaskCache                      (abstract) (auto) (ansi) (sealed) 
// Interface ISystemClock                   (abstract) (auto) (ansi) 
// Class SystemClock                    (auto) (ansi) 
// Class ClientProxyExtensions          (public) (abstract) (auto) (ansi) (sealed) 
// Class DefaultHubLifetimeManager`1    (public) (auto) (ansi) 
// Class DefaultUserIdProvider          (public) (auto) (ansi) 
// Class DynamicHub                     (public) (abstract) (auto) (ansi) 
// Class DynamicHubClients              (public) (auto) (ansi) 
// Class Hub                            (public) (abstract) (auto) (ansi) 
// Class HubCallerContext               (public) (abstract) (auto) (ansi) 
// Class HubClientsExtensions           (public) (abstract) (auto) (ansi) (sealed) 
// Class HubConnectionContext           (public) (auto) (ansi) 
// Class HubConnectionContextOptions    (public) (auto) (ansi) 
// Class HubConnectionHandler`1         (public) (auto) (ansi) 
// Class HubConnectionHandlerLog        (abstract) (auto) (ansi) (sealed) 
// Class HubConnectionStore             (public) (auto) (ansi) 
// Class HubInvocationContext           (public) (auto) (ansi) 
// Class HubLifetimeContext             (public) (auto) (ansi) (sealed) 
// Class HubLifetimeManager`1           (public) (abstract) (auto) (ansi) 
// Class HubMetadata                    (public) (auto) (ansi) 
// Class HubMethodNameAttribute         (public) (auto) (ansi) 
// Class HubOptions                     (public) (auto) (ansi) 
// Class HubOptionsExtensions           (public) (abstract) (auto) (ansi) (sealed) 
// Class HubOptionsSetup                (public) (auto) (ansi) 
// Class HubOptionsSetup`1              (public) (auto) (ansi) 
// Class HubOptions`1                   (public) (auto) (ansi) 
// Class Hub`1                          (public) (abstract) (auto) (ansi) 
// Interface IClientProxy                   (public) (abstract) (auto) (ansi) 
// Interface IGroupManager                  (public) (abstract) (auto) (ansi) 
// Interface IHubActivator`1                (public) (abstract) (auto) (ansi) 
// Interface IHubCallerClients              (public) (abstract) (auto) (ansi) 
// Interface IHubCallerClients`1            (public) (abstract) (auto) (ansi) 
// Interface IHubClients                    (public) (abstract) (auto) (ansi) 
// Interface IHubClients`1                  (public) (abstract) (auto) (ansi) 
// Interface IHubContext                    (public) (abstract) (auto) (ansi) 
// Interface IHubContext`1                  (public) (abstract) (auto) (ansi) 
// Interface IHubContext`2                  (public) (abstract) (auto) (ansi) 
// Interface IHubFilter                     (public) (abstract) (auto) (ansi) 
// Interface IHubProtocolResolver           (public) (abstract) (auto) (ansi) 
// Interface ISignalRServerBuilder          (public) (abstract) (auto) (ansi) 
// Interface IUserIdProvider                (public) (abstract) (auto) (ansi) 
// Class SerializedHubMessage           (public) (auto) (ansi) 
// Class SerializedMessage              (public) (sequential) (ansi) (sealed) 
// Class SignalRConnectionBuilderExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class StreamTracker                  (auto) (ansi) 
// Class ReflectionHelper               (abstract) (auto) (ansi) (sealed) 
// Class AsyncDisposableExtensions      (abstract) (auto) (ansi) (sealed) 
// Class DefaultHubActivator`1          (auto) (ansi) 
// Class DefaultHubCallerContext        (auto) (ansi) 
// Class DefaultHubDispatcher`1         (auto) (ansi) 
// Class DefaultHubDispatcherLog        (abstract) (auto) (ansi) (sealed) 
// Class DefaultHubProtocolResolver     (auto) (ansi) 
// Class DynamicClientProxy             (auto) (ansi) 
// Class ErrorMessageHelper             (abstract) (auto) (ansi) (sealed) 
// Class GroupManager`1                 (auto) (ansi) 
// Class HubCallerClients               (auto) (ansi) 
// Class HubClients`1                   (auto) (ansi) 
// Class HubClients`2                   (auto) (ansi) 
// Class HubConnectionBinder`1          (auto) (ansi) 
// Class HubContext`1                   (auto) (ansi) 
// Class HubContext`2                   (auto) (ansi) 
// Class HubDispatcher`1                (abstract) (auto) (ansi) 
// Class HubFilterFactory               (auto) (ansi) 
// Class HubGroupList                   (auto) (ansi) 
// Class GroupConnectionList            (auto) (ansi) 
// Class HubMethodDescriptor            (auto) (ansi) 
// Class HubReflectionHelper            (abstract) (auto) (ansi) (sealed) 
// Class UserProxy`1                    (auto) (ansi) 
// Class MultipleUserProxy`1            (auto) (ansi) 
// Class GroupProxy`1                   (auto) (ansi) 
// Class MultipleGroupProxy`1           (auto) (ansi) 
// Class GroupExceptProxy`1             (auto) (ansi) 
// Class AllClientProxy`1               (auto) (ansi) 
// Class AllClientsExceptProxy`1        (auto) (ansi) 
// Class SingleClientProxy`1            (auto) (ansi) 
// Class MultipleClientProxy`1          (auto) (ansi) 
// Class SemaphoreSlimExtensions        (abstract) (auto) (ansi) (sealed) 
// Class SignalRServerBuilder           (auto) (ansi) 
// Class SignalRCoreMarkerService       (auto) (ansi) 
// Class TypeBaseEnumerationExtensions  (abstract) (auto) (ansi) (sealed) 
// Class TypedClientBuilder`1           (abstract) (auto) (ansi) (sealed) 
// Class TypedHubClients`1              (auto) (ansi) 
// Class AsyncEnumerableAdapters        (abstract) (auto) (ansi) (sealed) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class MethodExecutorAsync            (auto) (ansi) (sealed) (nested private) 
// Class MethodExecutor                 (auto) (ansi) (sealed) (nested private) 
// Class VoidMethodExecutor             (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass33_0          (auto) (ansi) (sealed) (nested private) 
// Class Awaiter                        (sequential) (ansi) (sealed) (nested public) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <<AbortConnection>g__InnerAbortConnection|87_0>d (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <AbortAsyncSlow>d__83          (auto) (ansi) (sealed) (nested private) 
// Class <CompleteWriteAsync>d__73      (auto) (ansi) (sealed) (nested private) 
// Class <HandshakeAsync>d__81          (auto) (ansi) (sealed) (nested private) 
// Class <TryWritePingSlowAsync>d__77   (auto) (ansi) (sealed) (nested private) 
// Class <WriteHandshakeResponseAsync>d__78 (auto) (ansi) (sealed) (nested private) 
// Class <WriteSlowAsync>d__74          (auto) (ansi) (sealed) (nested private) 
// Class <WriteSlowAsync>d__75          (auto) (ansi) (sealed) (nested private) 
// Class <DispatchMessagesAsync>d__20   (auto) (ansi) (sealed) (nested private) 
// Class <HubOnDisconnectedAsync>d__18  (auto) (ansi) (sealed) (nested private) 
// Class <OnConnectedAsync>d__16        (auto) (ansi) (sealed) (nested private) 
// Class <RunHubAsync>d__17             (auto) (ansi) (sealed) (nested private) 
// Class <SendCloseAsync>d__19          (auto) (ansi) (sealed) (nested private) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Interface IStreamConverter               (abstract) (auto) (ansi) (nested private) 
// Class ChannelConverter`1             (auto) (ansi) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <<Invoke>g__ExecuteInvocation|16_0>d (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass8_0           (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteMethod>d__21           (auto) (ansi) (sealed) (nested private) 
// Class <Invoke>d__16                  (auto) (ansi) (sealed) (nested private) 
// Class <IsHubMethodAuthorizedSlow>d__25 (auto) (ansi) (sealed) (nested private) 
// Class <OnConnectedAsync>d__9         (auto) (ansi) (sealed) (nested private) 
// Class <OnDisconnectedAsync>d__10     (auto) (ansi) (sealed) (nested private) 
// Class <SendInvocationError>d__22     (auto) (ansi) (sealed) (nested private) 
// Class <StreamAsync>d__18             (auto) (ansi) (sealed) (nested private) 
// Class <ValidateInvocationMode>d__26  (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <InvokeMethodAsync>d__3        (auto) (ansi) (sealed) (nested private) 
// Class <OnConnectedAsync>d__4         (auto) (ansi) (sealed) (nested private) 
// Class <OnDisconnectedAsync>d__5      (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass11_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass12_0          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass6_0           (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass1_0           (auto) (ansi) (sealed) (nested private) 
// Class <RunSlowAsync>d__1`1           (auto) (ansi) (sealed) (nested private) 
// Class <RunTask>d__2`1                (auto) (ansi) (sealed) (nested private) 
// Class <AllBaseTypes>d__0             (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <GetAllInterfaceMethods>d__9   (auto) (ansi) (sealed) (nested private) 
// Class ChannelAsyncEnumerator`1       (auto) (ansi) (nested private) 
// Class CancelableTypedAsyncEnumerable`1 (auto) (ansi) (nested private) 
// Class BoxedAsyncEnumerator`1         (auto) (ansi) (nested private) 
// Class <MoveNextAsyncAwaited>d__8     (auto) (ansi) (sealed) (nested private) 
// Class CancelableEnumerator`1         (auto) (ansi) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Linq.Expressions
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ System.Security.Claims
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ Microsoft.Extensions.Features
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ Microsoft.AspNetCore.Connections.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ System.Threading
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ System.IO.Pipelines
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*2300000A*/ Microsoft.AspNetCore.SignalR.Common
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000B*/ System.Collections.Concurrent
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000C*/ System.Memory
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*2300000D*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000E*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000F*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000010*/ Microsoft.AspNetCore.Metadata
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000011*/ Microsoft.AspNetCore.Authorization
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000012*/ System.Threading.Channels
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.SignalR.Core
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
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 81 74 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...tMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52   // pNetCore.SignalR
                                                                                                                                                                                    2E 54 65 73 74 73 2E 55 74 69 6C 73 2C 20 50 75   // .Tests.Utils, Pu
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
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 81 78 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...xMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52   // pNetCore.SignalR
                                                                                                                                                                                    2E 4D 69 63 72 6F 62 65 6E 63 68 6D 61 72 6B 73   // .Microbenchmarks
                                                                                                                                                                                    2C 20 50 75 62 6C 69 63 4B 65 79 3D 30 30 32 34   // , PublicKey=0024
                                                                                                                                                                                    30 30 30 30 30 34 38 30 30 30 30 30 39 34 30 30   // 0000048000009400
                                                                                                                                                                                    30 30 30 30 30 36 30 32 30 30 30 30 30 30 32 34   // 0000060200000024
                                                                                                                                                                                    30 30 30 30 35 32 35 33 34 31 33 31 30 30 30 34   // 0000525341310004
                                                                                                                                                                                    30 30 30 30 30 31 30 30 30 31 30 30 66 33 33 61   // 000001000100f33a
                                                                                                                                                                                    32 39 30 34 34 66 61 39 64 37 34 30 63 39 62 33   // 29044fa9d740c9b3
                                                                                                                                                                                    32 31 33 61 39 33 65 35 37 63 38 34 62 34 37 32   // 213a93e57c84b472
                                                                                                                                                                                    63 38 34 65 30 62 38 61 30 65 31 61 65 34 38 65   // c84e0b8a0e1ae48e
                                                                                                                                                                                    36 37 61 39 66 38 66 36 64 65 39 64 35 66 37 66   // 67a9f8f6de9d5f7f
                                                                                                                                                                                    33 64 35 32 61 63 32 33 65 34 38 61 63 35 31 38   // 3d52ac23e48ac518
                                                                                                                                                                                    30 31 66 31 64 63 39 35 30 61 62 65 39 30 31 64   // 01f1dc950abe901d
                                                                                                                                                                                    61 33 34 64 32 61 39 65 33 62 61 61 64 62 31 34   // a34d2a9e3baadb14
                                                                                                                                                                                    31 61 31 37 63 37 37 65 66 33 63 35 36 35 64 64   // 1a17c77ef3c565dd
                                                                                                                                                                                    35 65 65 35 30 35 34 62 39 31 63 66 36 33 62 62   // 5ee5054b91cf63bb
                                                                                                                                                                                    33 63 36 61 62 38 33 66 37 32 61 62 33 61 61 66   // 3c6ab83f72ab3aaf
                                                                                                                                                                                    65 39 33 64 30 66 63 33 63 32 33 34 38 62 37 36   // e93d0fc3c2348b76
                                                                                                                                                                                    34 66 61 66 62 30 62 31 63 30 37 33 33 64 65 35   // 4fafb0b1c0733de5
                                                                                                                                                                                    31 34 35 39 61 65 61 62 34 36 35 38 30 33 38 34   // 1459aeab46580384
                                                                                                                                                                                    62 66 39 64 37 34 63 34 65 32 38 31 36 34 62 37   // bf9d74c4e28164b7
                                                                                                                                                                                    63 64 65 32 34 37 66 38 39 31 62 61 30 37 38 39   // cde247f891ba0789
                                                                                                                                                                                    31 63 39 64 38 37 32 61 64 32 62 62 00 00 )       // 1c9d872ad2bb..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 81 6E 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...nMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52   // pNetCore.SignalR
                                                                                                                                                                                    2E 54 65 73 74 73 2C 20 50 75 62 6C 69 63 4B 65   // .Tests, PublicKe
                                                                                                                                                                                    79 3D 30 30 32 34 30 30 30 30 30 34 38 30 30 30   // y=00240000048000
                                                                                                                                                                                    30 30 39 34 30 30 30 30 30 30 30 36 30 32 30 30   // 0094000000060200
                                                                                                                                                                                    30 30 30 30 32 34 30 30 30 30 35 32 35 33 34 31   // 0000240000525341
                                                                                                                                                                                    33 31 30 30 30 34 30 30 30 30 30 31 30 30 30 31   // 3100040000010001
                                                                                                                                                                                    30 30 66 33 33 61 32 39 30 34 34 66 61 39 64 37   // 00f33a29044fa9d7
                                                                                                                                                                                    34 30 63 39 62 33 32 31 33 61 39 33 65 35 37 63   // 40c9b3213a93e57c
                                                                                                                                                                                    38 34 62 34 37 32 63 38 34 65 30 62 38 61 30 65   // 84b472c84e0b8a0e
                                                                                                                                                                                    31 61 65 34 38 65 36 37 61 39 66 38 66 36 64 65   // 1ae48e67a9f8f6de
                                                                                                                                                                                    39 64 35 66 37 66 33 64 35 32 61 63 32 33 65 34   // 9d5f7f3d52ac23e4
                                                                                                                                                                                    38 61 63 35 31 38 30 31 66 31 64 63 39 35 30 61   // 8ac51801f1dc950a
                                                                                                                                                                                    62 65 39 30 31 64 61 33 34 64 32 61 39 65 33 62   // be901da34d2a9e3b
                                                                                                                                                                                    61 61 64 62 31 34 31 61 31 37 63 37 37 65 66 33   // aadb141a17c77ef3
                                                                                                                                                                                    63 35 36 35 64 64 35 65 65 35 30 35 34 62 39 31   // c565dd5ee5054b91
                                                                                                                                                                                    63 66 36 33 62 62 33 63 36 61 62 38 33 66 37 32   // cf63bb3c6ab83f72
                                                                                                                                                                                    61 62 33 61 61 66 65 39 33 64 30 66 63 33 63 32   // ab3aafe93d0fc3c2
                                                                                                                                                                                    33 34 38 62 37 36 34 66 61 66 62 30 62 31 63 30   // 348b764fafb0b1c0
                                                                                                                                                                                    37 33 33 64 65 35 31 34 35 39 61 65 61 62 34 36   // 733de51459aeab46
                                                                                                                                                                                    35 38 30 33 38 34 62 66 39 64 37 34 63 34 65 32   // 580384bf9d74c4e2
                                                                                                                                                                                    38 31 36 34 62 37 63 64 65 32 34 37 66 38 39 31   // 8164b7cde247f891
                                                                                                                                                                                    62 61 30 37 38 39 31 63 39 64 38 37 32 61 64 32   // ba07891c9d872ad2
                                                                                                                                                                                    62 62 00 00 )                                     // bb..
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 81 81 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ....Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52   // pNetCore.SignalR
                                                                                                                                                                                    2E 53 74 61 63 6B 45 78 63 68 61 6E 67 65 52 65   // .StackExchangeRe
                                                                                                                                                                                    64 69 73 2E 54 65 73 74 73 2C 20 50 75 62 6C 69   // dis.Tests, Publi
                                                                                                                                                                                    63 4B 65 79 3D 30 30 32 34 30 30 30 30 30 34 38   // cKey=00240000048
                                                                                                                                                                                    30 30 30 30 30 39 34 30 30 30 30 30 30 30 36 30   // 0000094000000060
                                                                                                                                                                                    32 30 30 30 30 30 30 32 34 30 30 30 30 35 32 35   // 2000000240000525
                                                                                                                                                                                    33 34 31 33 31 30 30 30 34 30 30 30 30 30 31 30   // 3413100040000010
                                                                                                                                                                                    30 30 31 30 30 66 33 33 61 32 39 30 34 34 66 61   // 00100f33a29044fa
                                                                                                                                                                                    39 64 37 34 30 63 39 62 33 32 31 33 61 39 33 65   // 9d740c9b3213a93e
                                                                                                                                                                                    35 37 63 38 34 62 34 37 32 63 38 34 65 30 62 38   // 57c84b472c84e0b8
                                                                                                                                                                                    61 30 65 31 61 65 34 38 65 36 37 61 39 66 38 66   // a0e1ae48e67a9f8f
                                                                                                                                                                                    36 64 65 39 64 35 66 37 66 33 64 35 32 61 63 32   // 6de9d5f7f3d52ac2
                                                                                                                                                                                    33 65 34 38 61 63 35 31 38 30 31 66 31 64 63 39   // 3e48ac51801f1dc9
                                                                                                                                                                                    35 30 61 62 65 39 30 31 64 61 33 34 64 32 61 39   // 50abe901da34d2a9
                                                                                                                                                                                    65 33 62 61 61 64 62 31 34 31 61 31 37 63 37 37   // e3baadb141a17c77
                                                                                                                                                                                    65 66 33 63 35 36 35 64 64 35 65 65 35 30 35 34   // ef3c565dd5ee5054
                                                                                                                                                                                    62 39 31 63 66 36 33 62 62 33 63 36 61 62 38 33   // b91cf63bb3c6ab83
                                                                                                                                                                                    66 37 32 61 62 33 61 61 66 65 39 33 64 30 66 63   // f72ab3aafe93d0fc
                                                                                                                                                                                    33 63 32 33 34 38 62 37 36 34 66 61 66 62 30 62   // 3c2348b764fafb0b
                                                                                                                                                                                    31 63 30 37 33 33 64 65 35 31 34 35 39 61 65 61   // 1c0733de51459aea
                                                                                                                                                                                    62 34 36 35 38 30 33 38 34 62 66 39 64 37 34 63   // b46580384bf9d74c
                                                                                                                                                                                    34 65 32 38 31 36 34 62 37 63 64 65 32 34 37 66   // 4e28164b7cde247f
                                                                                                                                                                                    38 39 31 62 61 30 37 38 39 31 63 39 64 38 37 32   // 891ba07891c9d872
                                                                                                                                                                                    61 64 32 62 62 00 00 )                            // ad2bb..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000F:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000010:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000012:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000013:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 33 52 65 61 6C 2D 74 69 6D 65 20 63 6F 6D   // ..3Real-time com
                                                                                                                                                                       6D 75 6E 69 63 61 74 69 6F 6E 20 66 72 61 6D 65   // munication frame
                                                                                                                                                                       77 6F 72 6B 20 66 6F 72 20 41 53 50 2E 4E 45 54   // work for ASP.NET
                                                                                                                                                                       20 43 6F 72 65 2E 00 00 )                         //  Core...
  .custom /*0C000014:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000015:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000016:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 21 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..!Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                 43 6F 72 65 00 00 )                               // Core..
  .custom /*0C000018:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000019:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.module Microsoft.AspNetCore.SignalR.Core.dll
// MVID: {f481778e-2af3-434b-8a0b-6cd9f37d61e6}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C000024:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00002B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002C:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.ClosedGenericMatcher
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000006*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          ExtractGenericInterface(class [System.Runtime/*23000001*/]System.Type/*01000018*/ queryType,
                                  class [System.Runtime/*23000001*/]System.Type/*01000018*/ interfaceType) cil managed
  // SIG: 00 02 12 61 12 61 12 61
  {
    .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000001*/ 
    .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClosedGenericMatcher::ExtractGenericInterface

} // end of class Microsoft.Extensions.Internal.ClosedGenericMatcher

.class /*02000007*/ private sequential ansi sealed beforefieldinit Microsoft.Extensions.Internal.AwaitableInfo
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .class /*02000060*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00016A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400001A*/ public static initonly class Microsoft.Extensions.Internal.AwaitableInfo/*02000007*//'<>c'/*02000060*/ '<>9'
    .field /*0400001B*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/,bool> '<>9__22_0'
    .field /*0400001C*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001B*/,bool> '<>9__22_1'
    .field /*0400001D*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/,bool> '<>9__22_2'
    .field /*0400001E*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/,bool> '<>9__22_3'
    .field /*0400001F*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/,bool> '<>9__22_4'
    .field /*04000020*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/,bool> '<>9__22_5'
    .field /*04000021*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/,bool> '<>9__22_6'
    .method /*060001E5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*060001E6*/ assembly hidebysig 
            instance bool  '<IsTypeAwaitable>b__22_0'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsTypeAwaitable>b__22_0'

    .method /*060001E7*/ assembly hidebysig 
            instance bool  '<IsTypeAwaitable>b__22_1'(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001B*/ p) cil managed
    // SIG: 20 01 02 12 6D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsTypeAwaitable>b__22_1'

    .method /*060001E8*/ assembly hidebysig 
            instance bool  '<IsTypeAwaitable>b__22_2'(class [System.Runtime/*23000001*/]System.Type/*01000018*/ t) cil managed
    // SIG: 20 01 02 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsTypeAwaitable>b__22_2'

    .method /*060001E9*/ assembly hidebysig 
            instance bool  '<IsTypeAwaitable>b__22_3'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsTypeAwaitable>b__22_3'

    .method /*060001EA*/ assembly hidebysig 
            instance bool  '<IsTypeAwaitable>b__22_4'(class [System.Runtime/*23000001*/]System.Type/*01000018*/ t) cil managed
    // SIG: 20 01 02 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsTypeAwaitable>b__22_4'

    .method /*060001EB*/ assembly hidebysig 
            instance bool  '<IsTypeAwaitable>b__22_5'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsTypeAwaitable>b__22_5'

    .method /*060001EC*/ assembly hidebysig 
            instance bool  '<IsTypeAwaitable>b__22_6'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsTypeAwaitable>b__22_6'

  } // end of class '<>c'

  .field /*04000004*/ private initonly class [System.Runtime/*23000001*/]System.Type/*01000018*/ '<AwaiterType>k__BackingField'
  .custom /*0C000023:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000005*/ private initonly class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001B*/ '<AwaiterIsCompletedProperty>k__BackingField'
  .custom /*0C000029:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000006*/ private initonly class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ '<AwaiterGetResultMethod>k__BackingField'
  .custom /*0C000030:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000007*/ private initonly class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ '<AwaiterOnCompletedMethod>k__BackingField'
  .custom /*0C000033:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000008*/ private initonly class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ '<AwaiterUnsafeOnCompletedMethod>k__BackingField'
  .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000009*/ private initonly class [System.Runtime/*23000001*/]System.Type/*01000018*/ '<ResultType>k__BackingField'
  .custom /*0C00003D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000A*/ private initonly class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ '<GetAwaiterMethod>k__BackingField'
  .custom /*0C000043:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000007*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          get_AwaiterType() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000032:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::get_AwaiterType

  .method /*06000008*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001B*/ 
          get_AwaiterIsCompletedProperty() cil managed
  // SIG: 20 00 12 6D
  {
    .custom /*0C000037:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::get_AwaiterIsCompletedProperty

  .method /*06000009*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ 
          get_AwaiterGetResultMethod() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00003C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::get_AwaiterGetResultMethod

  .method /*0600000A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ 
          get_AwaiterOnCompletedMethod() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000042:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::get_AwaiterOnCompletedMethod

  .method /*0600000B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ 
          get_AwaiterUnsafeOnCompletedMethod() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000046:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::get_AwaiterUnsafeOnCompletedMethod

  .method /*0600000C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          get_ResultType() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C00004A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::get_ResultType

  .method /*0600000D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ 
          get_GetAwaiterMethod() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00004E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::get_GetAwaiterMethod

  .method /*0600000E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/ awaiterType,
                               class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001B*/ awaiterIsCompletedProperty,
                               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ awaiterGetResultMethod,
                               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ awaiterOnCompletedMethod,
                               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ awaiterUnsafeOnCompletedMethod,
                               class [System.Runtime/*23000001*/]System.Type/*01000018*/ resultType,
                               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ getAwaiterMethod) cil managed
  // SIG: 20 07 01 12 61 12 6D 12 71 12 71 12 71 12 61 12 71
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::.ctor

  .method /*0600000F*/ public hidebysig static 
          bool  IsTypeAwaitable(class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'type',
                                [out] valuetype Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/& awaitableInfo) cil managed
  // SIG: 00 02 02 12 61 10 11 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AwaitableInfo::IsTypeAwaitable

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
          AwaiterType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/::get_AwaiterType() /* 06000007 */
  } // end of property AwaitableInfo::AwaiterType
  .property /*17000002*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001B*/
          AwaiterIsCompletedProperty()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001B*/ Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/::get_AwaiterIsCompletedProperty() /* 06000008 */
  } // end of property AwaitableInfo::AwaiterIsCompletedProperty
  .property /*17000003*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/
          AwaiterGetResultMethod()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/::get_AwaiterGetResultMethod() /* 06000009 */
  } // end of property AwaitableInfo::AwaiterGetResultMethod
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/
          AwaiterOnCompletedMethod()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/::get_AwaiterOnCompletedMethod() /* 0600000A */
  } // end of property AwaitableInfo::AwaiterOnCompletedMethod
  .property /*17000005*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/
          AwaiterUnsafeOnCompletedMethod()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/::get_AwaiterUnsafeOnCompletedMethod() /* 0600000B */
  } // end of property AwaitableInfo::AwaiterUnsafeOnCompletedMethod
  .property /*17000006*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
          ResultType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/::get_ResultType() /* 0600000C */
  } // end of property AwaitableInfo::ResultType
  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/
          GetAwaiterMethod()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/::get_GetAwaiterMethod() /* 0600000D */
  } // end of property AwaitableInfo::GetAwaiterMethod
} // end of class Microsoft.Extensions.Internal.AwaitableInfo

.class /*02000008*/ private sequential ansi sealed beforefieldinit Microsoft.Extensions.Internal.CoercedAwaitableInfo
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C000039:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .field /*0400000B*/ private initonly valuetype Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/ '<AwaitableInfo>k__BackingField'
  .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000C*/ private initonly class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*0100001E*/ '<CoercerExpression>k__BackingField'
  .custom /*0C00004B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*0400000D*/ private initonly class [System.Runtime/*23000001*/]System.Type/*01000018*/ '<CoercerResultType>k__BackingField'
  .custom /*0C00004F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000010*/ public hidebysig specialname 
          instance valuetype Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/ 
          get_AwaitableInfo() cil managed
  // SIG: 20 00 11 1C
  {
    .custom /*0C000059:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CoercedAwaitableInfo::get_AwaitableInfo

  .method /*06000011*/ public hidebysig specialname 
          instance class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*0100001E*/ 
          get_CoercerExpression() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C00005F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CoercedAwaitableInfo::get_CoercerExpression

  .method /*06000012*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          get_CoercerResultType() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000064:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CoercedAwaitableInfo::get_CoercerResultType

  .method /*06000013*/ public hidebysig specialname 
          instance bool  get_RequiresCoercion() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CoercedAwaitableInfo::get_RequiresCoercion

  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/ awaitableInfo) cil managed
  // SIG: 20 01 01 11 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CoercedAwaitableInfo::.ctor

  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*0100001E*/ coercerExpression,
                               class [System.Runtime/*23000001*/]System.Type/*01000018*/ coercerResultType,
                               valuetype Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/ coercedAwaitableInfo) cil managed
  // SIG: 20 03 01 12 79 12 61 11 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CoercedAwaitableInfo::.ctor

  .method /*06000016*/ public hidebysig static 
          bool  IsTypeAwaitable(class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'type',
                                [out] valuetype Microsoft.Extensions.Internal.CoercedAwaitableInfo/*02000008*/& info) cil managed
  // SIG: 00 02 02 12 61 10 11 20
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CoercedAwaitableInfo::IsTypeAwaitable

  .property /*17000008*/ instance callconv(8) valuetype Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/
          AwaitableInfo()
  {
    .get instance valuetype Microsoft.Extensions.Internal.AwaitableInfo/*02000007*/ Microsoft.Extensions.Internal.CoercedAwaitableInfo/*02000008*/::get_AwaitableInfo() /* 06000010 */
  } // end of property CoercedAwaitableInfo::AwaitableInfo
  .property /*17000009*/ instance callconv(8) class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*0100001E*/
          CoercerExpression()
  {
    .get instance class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*0100001E*/ Microsoft.Extensions.Internal.CoercedAwaitableInfo/*02000008*/::get_CoercerExpression() /* 06000011 */
  } // end of property CoercedAwaitableInfo::CoercerExpression
  .property /*1700000A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
          CoercerResultType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.Extensions.Internal.CoercedAwaitableInfo/*02000008*/::get_CoercerResultType() /* 06000012 */
  } // end of property CoercedAwaitableInfo::CoercerResultType
  .property /*1700000B*/ instance callconv(8) bool
          RequiresCoercion()
  {
    .get instance bool Microsoft.Extensions.Internal.CoercedAwaitableInfo/*02000008*/::get_RequiresCoercion() /* 06000013 */
  } // end of property CoercedAwaitableInfo::RequiresCoercion
} // end of class Microsoft.Extensions.Internal.CoercedAwaitableInfo

.class /*02000009*/ private auto ansi beforefieldinit Microsoft.Extensions.Internal.ObjectMethodExecutor
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000061*/ auto ansi sealed nested private MethodExecutorAsync
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000021*/
  {
    .method /*060001ED*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method MethodExecutorAsync::.ctor

    .method /*060001EE*/ public hidebysig newslot virtual 
            instance valuetype Microsoft.Extensions.Internal.ObjectMethodExecutorAwaitable/*0200000A*/ 
            Invoke(object target,
                   object[] parameters) runtime managed
    // SIG: 20 02 11 28 1C 1D 1C
    {
    } // end of method MethodExecutorAsync::Invoke

    .method /*060001EF*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000022*/ 
            BeginInvoke(object target,
                        object[] parameters,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*01000023*/ callback,
                        object 'object') runtime managed
    // SIG: 20 04 12 80 89 1C 1D 1C 12 80 8D 1C
    {
    } // end of method MethodExecutorAsync::BeginInvoke

    .method /*060001F0*/ public hidebysig newslot virtual 
            instance valuetype Microsoft.Extensions.Internal.ObjectMethodExecutorAwaitable/*0200000A*/ 
            EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000022*/ result) runtime managed
    // SIG: 20 01 11 28 12 80 89
    {
    } // end of method MethodExecutorAsync::EndInvoke

  } // end of class MethodExecutorAsync

  .class /*02000062*/ auto ansi sealed nested private MethodExecutor
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000021*/
  {
    .method /*060001F1*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method MethodExecutor::.ctor

    .method /*060001F2*/ public hidebysig newslot virtual 
            instance object  Invoke(object target,
                                    object[] parameters) runtime managed
    // SIG: 20 02 1C 1C 1D 1C
    {
    } // end of method MethodExecutor::Invoke

    .method /*060001F3*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000022*/ 
            BeginInvoke(object target,
                        object[] parameters,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*01000023*/ callback,
                        object 'object') runtime managed
    // SIG: 20 04 12 80 89 1C 1D 1C 12 80 8D 1C
    {
    } // end of method MethodExecutor::BeginInvoke

    .method /*060001F4*/ public hidebysig newslot virtual 
            instance object  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000022*/ result) runtime managed
    // SIG: 20 01 1C 12 80 89
    {
    } // end of method MethodExecutor::EndInvoke

  } // end of class MethodExecutor

  .class /*02000063*/ auto ansi sealed nested private VoidMethodExecutor
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000021*/
  {
    .method /*060001F5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method VoidMethodExecutor::.ctor

    .method /*060001F6*/ public hidebysig newslot virtual 
            instance void  Invoke(object target,
                                  object[] parameters) runtime managed
    // SIG: 20 02 01 1C 1D 1C
    {
    } // end of method VoidMethodExecutor::Invoke

    .method /*060001F7*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000022*/ 
            BeginInvoke(object target,
                        object[] parameters,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*01000023*/ callback,
                        object 'object') runtime managed
    // SIG: 20 04 12 80 89 1C 1D 1C 12 80 8D 1C
    {
    } // end of method VoidMethodExecutor::BeginInvoke

    .method /*060001F8*/ public hidebysig newslot virtual 
            instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000022*/ result) runtime managed
    // SIG: 20 01 01 12 80 89
    {
    } // end of method VoidMethodExecutor::EndInvoke

  } // end of class VoidMethodExecutor

  .class /*02000064*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass33_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000176:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000022*/ public class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*//VoidMethodExecutor/*02000063*/ executor
    .method /*060001F9*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass33_0'::.ctor

    .method /*060001FA*/ assembly hidebysig 
            instance object  '<WrapVoidMethod>b__0'(object target,
                                                    object[] parameters) cil managed
    // SIG: 20 02 1C 1C 1D 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass33_0'::'<WrapVoidMethod>b__0'

  } // end of class '<>c__DisplayClass33_0'

  .method /*06000017*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ 
          get_MethodInfo() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00007D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::get_MethodInfo

  .method /*06000018*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/[] 
          get_MethodParameters() cil managed
  // SIG: 20 00 1D 12 7D
  {
    .custom /*0C000085:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::get_MethodParameters

  .method /*06000019*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000020*/ 
          get_TargetTypeInfo() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C00008C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::get_TargetTypeInfo

  .method /*0600001A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          get_AsyncResultType() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000094:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::get_AsyncResultType

  .method /*0600001B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          get_MethodReturnType() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000099:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::get_MethodReturnType

  .method /*0600001C*/ assembly hidebysig specialname 
          instance void  set_MethodReturnType(class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'value') cil managed
  // SIG: 20 01 01 12 61
  {
    .custom /*0C00009D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::set_MethodReturnType

  .method /*0600001D*/ public hidebysig specialname 
          instance bool  get_IsMethodAsync() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000A3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::get_IsMethodAsync

  .method /*0600001E*/ public hidebysig static 
          class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ 
          Create(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ methodInfo,
                 class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000020*/ targetTypeInfo) cil managed
  // SIG: 00 02 12 24 12 71 12 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::Create

  .method /*0600001F*/ public hidebysig static 
          class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ 
          Create(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ methodInfo,
                 class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000020*/ targetTypeInfo,
                 object[] parameterDefaultValues) cil managed
  // SIG: 00 03 12 24 12 71 12 80 81 1D 1C
  {
    .param [3]/*08000018*/ 
    .custom /*0C000089:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::Create

  .method /*06000020*/ public hidebysig instance object 
          Execute(object target,
                  object[] parameters) cil managed
  // SIG: 20 02 1C 1C 1D 1C
  {
    .custom /*0C0000AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000019*/ 
    .custom /*0C000090:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::Execute

  .method /*06000021*/ public hidebysig instance valuetype Microsoft.Extensions.Internal.ObjectMethodExecutorAwaitable/*0200000A*/ 
          ExecuteAsync(object target,
                       object[] parameters) cil managed
  // SIG: 20 02 11 28 1C 1D 1C
  {
    .param [2]/*0800001C*/ 
    .custom /*0C0000A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::ExecuteAsync

  .method /*06000022*/ public hidebysig instance object 
          GetDefaultValueForParameter(int32 index) cil managed
  // SIG: 20 01 1C 08
  {
    .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutor::GetDefaultValueForParameter

  .property /*1700000C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/
          MethodInfo()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/::get_MethodInfo() /* 06000017 */
  } // end of property ObjectMethodExecutor::MethodInfo
  .property /*1700000D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/[]
          MethodParameters()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/[] Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/::get_MethodParameters() /* 06000018 */
  } // end of property ObjectMethodExecutor::MethodParameters
  .property /*1700000E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000020*/
          TargetTypeInfo()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.TypeInfo/*01000020*/ Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/::get_TargetTypeInfo() /* 06000019 */
  } // end of property ObjectMethodExecutor::TargetTypeInfo
  .property /*1700000F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
          AsyncResultType()
  {
    .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/::get_AsyncResultType() /* 0600001A */
  } // end of property ObjectMethodExecutor::AsyncResultType
  .property /*17000010*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
          MethodReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/::get_MethodReturnType() /* 0600001B */
    .set instance void Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/::set_MethodReturnType(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0600001C */
  } // end of property ObjectMethodExecutor::MethodReturnType
  .property /*17000011*/ instance callconv(8) bool
          IsMethodAsync()
  {
    .get instance bool Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/::get_IsMethodAsync() /* 0600001D */
  } // end of property ObjectMethodExecutor::IsMethodAsync
} // end of class Microsoft.Extensions.Internal.ObjectMethodExecutor

.class /*0200000A*/ private sequential ansi sealed beforefieldinit Microsoft.Extensions.Internal.ObjectMethodExecutorAwaitable
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C000044:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .class /*02000065*/ sequential ansi sealed nested public beforefieldinit Awaiter
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ICriticalNotifyCompletion/*01000026*/,
                    [System.Runtime/*23000001*/]System.Runtime.CompilerServices.INotifyCompletion/*01000027*/
  {
    .custom /*0C000177:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000023*/ private initonly object _customAwaiter
    .field /*04000024*/ private initonly class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,bool> _isCompletedMethod
    .field /*04000025*/ private initonly class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,object> _getResultMethod
    .field /*04000026*/ private initonly class [System.Runtime/*23000001*/]System.Action`2/*01000024*/<object,class [System.Runtime/*23000001*/]System.Action/*01000025*/> _onCompletedMethod
    .field /*04000027*/ private initonly class [System.Runtime/*23000001*/]System.Action`2/*01000024*/<object,class [System.Runtime/*23000001*/]System.Action/*01000025*/> _unsafeOnCompletedMethod
    .method /*060001FB*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object customAwaiter,
                                 class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,bool> isCompletedMethod,
                                 class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,object> getResultMethod,
                                 class [System.Runtime/*23000001*/]System.Action`2/*01000024*/<object,class [System.Runtime/*23000001*/]System.Action/*01000025*/> onCompletedMethod,
                                 class [System.Runtime/*23000001*/]System.Action`2/*01000024*/<object,class [System.Runtime/*23000001*/]System.Action/*01000025*/> unsafeOnCompletedMethod) cil managed
    // SIG: 20 05 01 1C 15 12 75 02 1C 02 15 12 75 02 1C 1C 15 12 80 91 02 1C 12 80 95 15 12 80 91 02 1C 12
    //      80 95
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaiter::.ctor

    .method /*060001FC*/ public hidebysig specialname 
            instance bool  get_IsCompleted() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaiter::get_IsCompleted

    .method /*060001FD*/ public hidebysig 
            instance object  GetResult() cil managed
    // SIG: 20 00 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaiter::GetResult

    .method /*060001FE*/ public hidebysig newslot virtual final 
            instance void  OnCompleted(class [System.Runtime/*23000001*/]System.Action/*01000025*/ continuation) cil managed
    // SIG: 20 01 01 12 80 95
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaiter::OnCompleted

    .method /*060001FF*/ public hidebysig newslot virtual final 
            instance void  UnsafeOnCompleted(class [System.Runtime/*23000001*/]System.Action/*01000025*/ continuation) cil managed
    // SIG: 20 01 01 12 80 95
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Awaiter::UnsafeOnCompleted

    .property /*1700007F*/ instance callconv(8) bool
            IsCompleted()
    {
      .get instance bool Microsoft.Extensions.Internal.ObjectMethodExecutorAwaitable/*0200000A*//Awaiter/*02000065*/::get_IsCompleted() /* 060001FC */
    } // end of property Awaiter::IsCompleted
  } // end of class Awaiter

  .field /*0400000E*/ private initonly object _customAwaitable
  .field /*0400000F*/ private initonly class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,object> _getAwaiterMethod
  .field /*04000010*/ private initonly class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,bool> _isCompletedMethod
  .field /*04000011*/ private initonly class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,object> _getResultMethod
  .field /*04000012*/ private initonly class [System.Runtime/*23000001*/]System.Action`2/*01000024*/<object,class [System.Runtime/*23000001*/]System.Action/*01000025*/> _onCompletedMethod
  .field /*04000013*/ private initonly class [System.Runtime/*23000001*/]System.Action`2/*01000024*/<object,class [System.Runtime/*23000001*/]System.Action/*01000025*/> _unsafeOnCompletedMethod
  .method /*06000023*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object customAwaitable,
                               class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,object> getAwaiterMethod,
                               class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,bool> isCompletedMethod,
                               class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<object,object> getResultMethod,
                               class [System.Runtime/*23000001*/]System.Action`2/*01000024*/<object,class [System.Runtime/*23000001*/]System.Action/*01000025*/> onCompletedMethod,
                               class [System.Runtime/*23000001*/]System.Action`2/*01000024*/<object,class [System.Runtime/*23000001*/]System.Action/*01000025*/> unsafeOnCompletedMethod) cil managed
  // SIG: 20 06 01 1C 15 12 75 02 1C 1C 15 12 75 02 1C 02 15 12 75 02 1C 1C 15 12 80 91 02 1C 12 80 95 15
  //      12 80 91 02 1C 12 80 95
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutorAwaitable::.ctor

  .method /*06000024*/ public hidebysig instance valuetype Microsoft.Extensions.Internal.ObjectMethodExecutorAwaitable/*0200000A*//Awaiter/*02000065*/ 
          GetAwaiter() cil managed
  // SIG: 20 00 11 81 94
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutorAwaitable::GetAwaiter

} // end of class Microsoft.Extensions.Internal.ObjectMethodExecutorAwaitable

.class /*0200000B*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.ObjectMethodExecutorFSharpSupport
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .class /*02000066*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000179:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000028*/ public static initonly class Microsoft.Extensions.Internal.ObjectMethodExecutorFSharpSupport/*0200000B*//'<>c'/*02000066*/ '<>9'
    .field /*04000029*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/,bool> '<>9__7_0'
    .method /*06000200*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000201*/ assembly hidebysig 
            instance bool  '<TryPopulateFSharpValueCaches>b__7_0'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<TryPopulateFSharpValueCaches>b__7_0'

  } // end of class '<>c'

  .method /*06000025*/ public hidebysig static 
          bool  TryBuildCoercerFromFSharpAsyncToAwaitable(class [System.Runtime/*23000001*/]System.Type/*01000018*/ possibleFSharpAsyncType,
                                                          [out] class [System.Linq.Expressions/*23000002*/]System.Linq.Expressions.Expression/*0100001E*/& coerceToAwaitableExpression,
                                                          [out] class [System.Runtime/*23000001*/]System.Type/*01000018*/& awaitableType) cil managed
  // SIG: 00 03 02 12 61 10 12 79 10 12 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectMethodExecutorFSharpSupport::TryBuildCoercerFromFSharpAsyncToAwaitable

} // end of class Microsoft.Extensions.Internal.ObjectMethodExecutorFSharpSupport

.class /*0200000C*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.DependencyInjection.SignalRDependencyInjectionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00004C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000026*/ public hidebysig static 
          class Microsoft.AspNetCore.SignalR.ISignalRServerBuilder/*02000034*/ 
          AddSignalRCore(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000028*/ services) cil managed
  // SIG: 00 01 12 80 D0 12 80 A1
  {
    .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000C2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignalRDependencyInjectionExtensions::AddSignalRCore

} // end of class Microsoft.Extensions.DependencyInjection.SignalRDependencyInjectionExtensions

.class /*0200000D*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.TaskCache
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000050:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000014*/ public static initonly class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000029*/<bool> True
  .field /*04000015*/ public static initonly class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000029*/<bool> False
} // end of class Microsoft.AspNetCore.Internal.TaskCache

.class /*0200000E*/ interface private abstract auto ansi Microsoft.AspNetCore.Internal.ISystemClock
{
  .method /*06000027*/ public hidebysig newslot specialname abstract virtual 
          instance int64  get_CurrentTicks() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x0
  } // end of method ISystemClock::get_CurrentTicks

  .property /*17000012*/ instance callconv(8) int64
          CurrentTicks()
  {
    .get instance int64 Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/::get_CurrentTicks() /* 06000027 */
  } // end of property ISystemClock::CurrentTicks
} // end of class Microsoft.AspNetCore.Internal.ISystemClock

.class /*0200000F*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Internal.SystemClock
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/
{
  .method /*06000028*/ public hidebysig newslot specialname virtual final 
          instance int64  get_CurrentTicks() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemClock::get_CurrentTicks

  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SystemClock::.ctor

  .property /*17000013*/ instance callconv(8) int64
          CurrentTicks()
  {
    .get instance int64 Microsoft.AspNetCore.Internal.SystemClock/*0200000F*/::get_CurrentTicks() /* 06000028 */
  } // end of property SystemClock::CurrentTicks
} // end of class Microsoft.AspNetCore.Internal.SystemClock

.class /*02000010*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.ClientProxyExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00005C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600002A*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 03 12 80 A9 12 80 A0 0E 11 80 AD
  {
    .custom /*0C0000CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000CE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*0800002A*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*0600002B*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 04 12 80 A9 12 80 A0 0E 1C 11 80 AD
  {
    .custom /*0C0000D2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*0800002D*/ 
    .custom /*0C0000DB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*0800002E*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*0600002C*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 05 12 80 A9 12 80 A0 0E 1C 1C 11 80 AD
  {
    .custom /*0C0000D5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000031*/ 
    .custom /*0C0000E9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000032*/ 
    .custom /*0C0000ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000033*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*0600002D*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    object arg3,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 06 12 80 A9 12 80 A0 0E 1C 1C 1C 11 80 AD
  {
    .custom /*0C0000D9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000DA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000036*/ 
    .custom /*0C0000F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000037*/ 
    .custom /*0C0000FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000038*/ 
    .custom /*0C0000FF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000039*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*0600002E*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    object arg3,
                    object arg4,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 07 12 80 A9 12 80 A0 0E 1C 1C 1C 1C 11 80 AD
  {
    .custom /*0C0000DE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800003A*/ 
    .custom /*0C000108:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800003B*/ 
    .custom /*0C00010C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800003C*/ 
    .custom /*0C00010F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [7]/*08000041*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*0600002F*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    object arg3,
                    object arg4,
                    object arg5,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 08 12 80 A9 12 80 A0 0E 1C 1C 1C 1C 1C 11 80 AD
  {
    .custom /*0C0000E1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000042*/ 
    .custom /*0C00011F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000043*/ 
    .custom /*0C000124:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000044*/ 
    .custom /*0C000128:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [8]/*0800004A*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*06000030*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    object arg3,
                    object arg4,
                    object arg5,
                    object arg6,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 09 12 80 A9 12 80 A0 0E 1C 1C 1C 1C 1C 1C 11 80 AD
  {
    .custom /*0C0000E4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800004B*/ 
    .custom /*0C00013F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800004C*/ 
    .custom /*0C000144:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800004D*/ 
    .custom /*0C000149:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [9]/*08000054*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*06000031*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    object arg3,
                    object arg4,
                    object arg5,
                    object arg6,
                    object arg7,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 0A 12 80 A9 12 80 A0 0E 1C 1C 1C 1C 1C 1C 1C 11 80 AD
  {
    .custom /*0C0000E7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000055*/ 
    .custom /*0C000154:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000056*/ 
    .custom /*0C000156:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000057*/ 
    .custom /*0C000158:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [10]/*0800005F*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*06000032*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    object arg3,
                    object arg4,
                    object arg5,
                    object arg6,
                    object arg7,
                    object arg8,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 0B 12 80 A9 12 80 A0 0E 1C 1C 1C 1C 1C 1C 1C 1C 11 80 AD
  {
    .custom /*0C0000EB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000060*/ 
    .custom /*0C00016B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000061*/ 
    .custom /*0C00016E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000062*/ 
    .custom /*0C000172:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [11]/*0800006B*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*06000033*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    object arg3,
                    object arg4,
                    object arg5,
                    object arg6,
                    object arg7,
                    object arg8,
                    object arg9,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 0C 12 80 A9 12 80 A0 0E 1C 1C 1C 1C 1C 1C 1C 1C 1C 11 80 AD
  {
    .custom /*0C0000EF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800006C*/ 
    .custom /*0C000185:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800006D*/ 
    .custom /*0C000188:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800006E*/ 
    .custom /*0C00018B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [12]/*08000078*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

  .method /*06000034*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAsync(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy,
                    string 'method',
                    object arg1,
                    object arg2,
                    object arg3,
                    object arg4,
                    object arg5,
                    object arg6,
                    object arg7,
                    object arg8,
                    object arg9,
                    object arg10,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 00 0D 12 80 A9 12 80 A0 0E 1C 1C 1C 1C 1C 1C 1C 1C 1C 1C 11 80 AD
  {
    .custom /*0C0000F2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*08000079*/ 
    .custom /*0C0001AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800007A*/ 
    .custom /*0C0001B0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800007B*/ 
    .custom /*0C0001B3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [13]/*08000086*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ClientProxyExtensions::SendAsync

} // end of class Microsoft.AspNetCore.SignalR.ClientProxyExtensions

.class /*02000011*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.DefaultHubLifetimeManager`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub>/*1B000001*/
{
  .custom /*0C000060:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000061:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type THub /*2A000001*/ 
    .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000067*/ auto ansi serializable sealed nested private beforefieldinit '<>c'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C00017B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000023*/ 
      .custom /*0C0000B8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400002A*/ public static initonly class Microsoft.AspNetCore.SignalR.DefaultHubLifetimeManager`1/*02000011*//'<>c'/*02000067*/<!THub> '<>9'
    .field /*0400002B*/ public static class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/,object,bool> '<>9__12_0'
    .field /*0400002C*/ public static class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/,object,bool> '<>9__15_0'
    .field /*0400002D*/ public static class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/,object,bool> '<>9__18_0'
    .field /*0400002E*/ public static class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/,object,bool> '<>9__19_0'
    .field /*0400002F*/ public static class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/,object,bool> '<>9__20_0'
    .method /*06000202*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000203*/ assembly hidebysig 
            instance bool  '<SendGroupExceptAsync>b__12_0'(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                                                           object state) cil managed
    // SIG: 20 02 02 12 60 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<SendGroupExceptAsync>b__12_0'

    .method /*06000204*/ assembly hidebysig 
            instance bool  '<SendUserAsync>b__15_0'(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                                                    object state) cil managed
    // SIG: 20 02 02 12 60 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<SendUserAsync>b__15_0'

    .method /*06000205*/ assembly hidebysig 
            instance bool  '<SendAllExceptAsync>b__18_0'(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                                                         object state) cil managed
    // SIG: 20 02 02 12 60 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<SendAllExceptAsync>b__18_0'

    .method /*06000206*/ assembly hidebysig 
            instance bool  '<SendConnectionsAsync>b__19_0'(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                                                           object state) cil managed
    // SIG: 20 02 02 12 60 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<SendConnectionsAsync>b__19_0'

    .method /*06000207*/ assembly hidebysig 
            instance bool  '<SendUsersAsync>b__20_0'(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                                                     object state) cil managed
    // SIG: 20 02 02 12 60 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<SendUsersAsync>b__20_0'

  } // end of class '<>c'

  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger`1/*0100002C*/<class Microsoft.AspNetCore.SignalR.DefaultHubLifetimeManager`1/*02000011*/<!THub>> logger) cil managed
  // SIG: 20 01 01 15 12 80 B1 01 15 12 44 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::.ctor

  .method /*06000036*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          AddToGroupAsync(string connectionId,
                          string groupName,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 0E 11 80 AD
  {
    .param [3]/*0800008A*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::AddToGroupAsync

  .method /*06000037*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          RemoveFromGroupAsync(string connectionId,
                               string groupName,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 0E 11 80 AD
  {
    .param [3]/*0800008D*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::RemoveFromGroupAsync

  .method /*06000038*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAllAsync(string methodName,
                       object[] args,
                       [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .param [2]/*0800008F*/ 
    .custom /*0C0001DC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000090*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendAllAsync

  .method /*06000039*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendConnectionAsync(string connectionId,
                              string methodName,
                              object[] args,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*08000093*/ 
    .custom /*0C0001E5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000094*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendConnectionAsync

  .method /*0600003A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendGroupAsync(string groupName,
                         string methodName,
                         object[] args,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*08000097*/ 
    .custom /*0C0001EC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000098*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendGroupAsync

  .method /*0600003B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendGroupsAsync(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames,
                          string methodName,
                          object[] args,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 15 12 80 B5 01 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*0800009B*/ 
    .custom /*0C0001F4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*0800009C*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendGroupsAsync

  .method /*0600003C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendGroupExceptAsync(string groupName,
                               string methodName,
                               object[] args,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 05 12 80 A9 0E 0E 1D 1C 15 12 80 B5 01 0E 11 80 AD
  {
    .param [3]/*0800009F*/ 
    .custom /*0C0001F8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [5]/*080000A1*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendGroupExceptAsync

  .method /*0600003D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendUserAsync(string userId,
                        string methodName,
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*080000A4*/ 
    .custom /*0C0001FD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*080000A5*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendUserAsync

  .method /*0600003E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnConnectedAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 12 80 A9 12 60
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::OnConnectedAsync

  .method /*0600003F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnDisconnectedAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 12 80 A9 12 60
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::OnDisconnectedAsync

  .method /*06000040*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAllExceptAsync(string methodName,
                             object[] args,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 0E 1D 1C 15 12 80 B5 01 0E 11 80 AD
  {
    .param [2]/*080000A9*/ 
    .custom /*0C000201:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*080000AB*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendAllExceptAsync

  .method /*06000041*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendConnectionsAsync(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds,
                               string methodName,
                               object[] args,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 15 12 80 B5 01 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*080000AE*/ 
    .custom /*0C000203:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*080000AF*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendConnectionsAsync

  .method /*06000042*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendUsersAsync(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds,
                         string methodName,
                         object[] args,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 15 12 80 B5 01 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*080000B2*/ 
    .custom /*0C000206:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*080000B3*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubLifetimeManager`1::SendUsersAsync

} // end of class Microsoft.AspNetCore.SignalR.DefaultHubLifetimeManager`1

.class /*02000012*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.DefaultUserIdProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IUserIdProvider/*02000035*/
{
  .method /*06000043*/ public hidebysig newslot virtual 
          instance string  GetUserId(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 0E 12 60
  {
    .custom /*0C000121:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [0]/*080000B4*/ 
    .custom /*0C00020A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultUserIdProvider::GetUserId

  .method /*06000044*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultUserIdProvider::.ctor

} // end of class Microsoft.AspNetCore.SignalR.DefaultUserIdProvider

.class /*02000013*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.DynamicHub
       extends Microsoft.AspNetCore.SignalR.Hub/*02000015*/
{
  .custom /*0C000068:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000045*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.DynamicHubClients/*02000014*/ 
          get_Clients() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHub::get_Clients

  .method /*06000046*/ public hidebysig specialname 
          instance void  set_Clients(class Microsoft.AspNetCore.SignalR.DynamicHubClients/*02000014*/ 'value') cil managed
  // SIG: 20 01 01 12 50
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHub::set_Clients

  .method /*06000047*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHub::.ctor

  .property /*17000014*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.DynamicHubClients/*02000014*/
          Clients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.DynamicHubClients/*02000014*/ Microsoft.AspNetCore.SignalR.DynamicHub/*02000013*/::get_Clients() /* 06000045 */
    .set instance void Microsoft.AspNetCore.SignalR.DynamicHub/*02000013*/::set_Clients(class Microsoft.AspNetCore.SignalR.DynamicHubClients/*02000014*/) /* 06000046 */
  } // end of property DynamicHub::Clients
} // end of class Microsoft.AspNetCore.SignalR.DynamicHub

.class /*02000014*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.DynamicHubClients
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/ clients) cil managed
  // SIG: 20 01 01 12 80 AC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::.ctor

  .method /*06000049*/ public hidebysig specialname 
          instance object  get_All() cil managed
  // SIG: 20 00 1C
  {
    .param [0]/*080000B8*/ 
    .custom /*0C00020F:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::get_All

  .method /*0600004A*/ public hidebysig instance object 
          AllExcept(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 01 1C 15 12 80 B5 01 0E
  {
    .param [0]/*080000B9*/ 
    .custom /*0C000211:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::AllExcept

  .method /*0600004B*/ public hidebysig specialname 
          instance object  get_Caller() cil managed
  // SIG: 20 00 1C
  {
    .param [0]/*080000BB*/ 
    .custom /*0C000214:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::get_Caller

  .method /*0600004C*/ public hidebysig instance object 
          Client(string connectionId) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*080000BC*/ 
    .custom /*0C000216:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::Client

  .method /*0600004D*/ public hidebysig instance object 
          Clients(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds) cil managed
  // SIG: 20 01 1C 15 12 80 B5 01 0E
  {
    .param [0]/*080000BE*/ 
    .custom /*0C000219:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::Clients

  .method /*0600004E*/ public hidebysig instance object 
          Group(string groupName) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*080000C0*/ 
    .custom /*0C00021B:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::Group

  .method /*0600004F*/ public hidebysig instance object 
          Groups(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames) cil managed
  // SIG: 20 01 1C 15 12 80 B5 01 0E
  {
    .param [0]/*080000C2*/ 
    .custom /*0C00021D:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::Groups

  .method /*06000050*/ public hidebysig instance object 
          GroupExcept(string groupName,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 02 1C 0E 15 12 80 B5 01 0E
  {
    .param [0]/*080000C4*/ 
    .custom /*0C00021F:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::GroupExcept

  .method /*06000051*/ public hidebysig instance object 
          OthersInGroup(string groupName) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*080000C7*/ 
    .custom /*0C000220:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::OthersInGroup

  .method /*06000052*/ public hidebysig specialname 
          instance object  get_Others() cil managed
  // SIG: 20 00 1C
  {
    .param [0]/*080000C9*/ 
    .custom /*0C000221:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::get_Others

  .method /*06000053*/ public hidebysig instance object 
          User(string userId) cil managed
  // SIG: 20 01 1C 0E
  {
    .param [0]/*080000CA*/ 
    .custom /*0C000222:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::User

  .method /*06000054*/ public hidebysig instance object 
          Users(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds) cil managed
  // SIG: 20 01 1C 15 12 80 B5 01 0E
  {
    .param [0]/*080000CC*/ 
    .custom /*0C000223:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicHubClients::Users

  .property /*17000015*/ instance callconv(8) object
          All()
  {
    .custom /*0C000075:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .get instance object Microsoft.AspNetCore.SignalR.DynamicHubClients/*02000014*/::get_All() /* 06000049 */
  } // end of property DynamicHubClients::All
  .property /*17000016*/ instance callconv(8) object
          Caller()
  {
    .custom /*0C00007B:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .get instance object Microsoft.AspNetCore.SignalR.DynamicHubClients/*02000014*/::get_Caller() /* 0600004B */
  } // end of property DynamicHubClients::Caller
  .property /*17000017*/ instance callconv(8) object
          Others()
  {
    .custom /*0C000082:0A000015*/ instance void [System.Linq.Expressions/*23000002*/]System.Runtime.CompilerServices.DynamicAttribute/*0100002F*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .get instance object Microsoft.AspNetCore.SignalR.DynamicHubClients/*02000014*/::get_Others() /* 06000052 */
  } // end of property DynamicHubClients::Others
} // end of class Microsoft.AspNetCore.SignalR.DynamicHubClients

.class /*02000015*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Hub
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000030*/
{
  .custom /*0C000072:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000030*/
  .custom /*0C000021:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000055*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/ 
          get_Clients() cil managed
  // SIG: 20 00 12 80 AC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::get_Clients

  .method /*06000056*/ public hidebysig specialname 
          instance void  set_Clients(class Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/ 'value') cil managed
  // SIG: 20 01 01 12 80 AC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::set_Clients

  .method /*06000057*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ 
          get_Context() cil managed
  // SIG: 20 00 12 58
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::get_Context

  .method /*06000058*/ public hidebysig specialname 
          instance void  set_Context(class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ 'value') cil managed
  // SIG: 20 01 01 12 58
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::set_Context

  .method /*06000059*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ 
          get_Groups() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::get_Groups

  .method /*0600005A*/ public hidebysig specialname 
          instance void  set_Groups(class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A4
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::set_Groups

  .method /*0600005B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnConnectedAsync() cil managed
  // SIG: 20 00 12 80 A9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::OnConnectedAsync

  .method /*0600005C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnDisconnectedAsync(class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 20 01 12 80 A9 12 80 C5
  {
    .param [1]/*080000D1*/ 
    .custom /*0C000225:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::OnDisconnectedAsync

  .method /*0600005D*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::Dispose

  .method /*0600005E*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::Dispose

  .method /*0600005F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub::.ctor

  .property /*17000018*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/
          Clients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/ Microsoft.AspNetCore.SignalR.Hub/*02000015*/::get_Clients() /* 06000055 */
    .set instance void Microsoft.AspNetCore.SignalR.Hub/*02000015*/::set_Clients(class Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/) /* 06000056 */
  } // end of property Hub::Clients
  .property /*17000019*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/
          Context()
  {
    .get instance class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ Microsoft.AspNetCore.SignalR.Hub/*02000015*/::get_Context() /* 06000057 */
    .set instance void Microsoft.AspNetCore.SignalR.Hub/*02000015*/::set_Context(class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/) /* 06000058 */
  } // end of property Hub::Context
  .property /*1700001A*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/
          Groups()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ Microsoft.AspNetCore.SignalR.Hub/*02000015*/::get_Groups() /* 06000059 */
    .set instance void Microsoft.AspNetCore.SignalR.Hub/*02000015*/::set_Groups(class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/) /* 0600005A */
  } // end of property Hub::Groups
} // end of class Microsoft.AspNetCore.SignalR.Hub

.class /*02000016*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubCallerContext
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000078:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000060*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method HubCallerContext::get_ConnectionId

  .method /*06000061*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_UserIdentifier() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00016D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method HubCallerContext::get_UserIdentifier

  .method /*06000062*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/ 
          get_User() cil managed
  // SIG: 20 00 12 80 C9
  {
    .custom /*0C000171:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method HubCallerContext::get_User

  .method /*06000063*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 80 CD 02 1C 1C
  {
    .param [0]/*080000D3*/ 
    .custom /*0C000227:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method HubCallerContext::get_Items

  .method /*06000064*/ public hidebysig newslot specialname abstract virtual 
          instance class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/ 
          get_Features() cil managed
  // SIG: 20 00 12 80 D1
  {
    // Method begins at RVA 0x0
  } // end of method HubCallerContext::get_Features

  .method /*06000065*/ public hidebysig newslot specialname abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ 
          get_ConnectionAborted() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x0
  } // end of method HubCallerContext::get_ConnectionAborted

  .method /*06000066*/ public hidebysig newslot abstract virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method HubCallerContext::Abort

  .method /*06000067*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerContext::.ctor

  .property /*1700001B*/ instance callconv(8) string
          ConnectionId()
  {
    .get instance string Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/::get_ConnectionId() /* 06000060 */
  } // end of property HubCallerContext::ConnectionId
  .property /*1700001C*/ instance callconv(8) string
          UserIdentifier()
  {
    .custom /*0C0000A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/::get_UserIdentifier() /* 06000061 */
  } // end of property HubCallerContext::UserIdentifier
  .property /*1700001D*/ instance callconv(8) class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/
          User()
  {
    .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/ Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/::get_User() /* 06000062 */
  } // end of property HubCallerContext::User
  .property /*1700001E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object>
          Items()
  {
    .custom /*0C0000A9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object> Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/::get_Items() /* 06000063 */
  } // end of property HubCallerContext::Items
  .property /*1700001F*/ instance callconv(8) class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/
          Features()
  {
    .get instance class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/ Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/::get_Features() /* 06000064 */
  } // end of property HubCallerContext::Features
  .property /*17000020*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/
          ConnectionAborted()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/::get_ConnectionAborted() /* 06000065 */
  } // end of property HubCallerContext::ConnectionAborted
} // end of class Microsoft.AspNetCore.SignalR.HubCallerContext

.class /*02000017*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.HubClientsExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000080:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000068*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            string excludedConnectionId1) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 B8 01 1E 00 0E
  {
    .custom /*0C00017C:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000024*/ 
      .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*06000069*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            string excludedConnectionId1,
                            string excludedConnectionId2) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 B8 01 1E 00 0E 0E
  {
    .custom /*0C00017D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000025*/ 
      .custom /*0C0000C0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*0600006A*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            string excludedConnectionId1,
                            string excludedConnectionId2,
                            string excludedConnectionId3) cil managed
  // SIG: 10 01 04 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E
  {
    .custom /*0C00017F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000026*/ 
      .custom /*0C0000C3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*0600006B*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            string excludedConnectionId1,
                            string excludedConnectionId2,
                            string excludedConnectionId3,
                            string excludedConnectionId4) cil managed
  // SIG: 10 01 05 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E
  {
    .custom /*0C000181:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000027*/ 
      .custom /*0C0000C7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*0600006C*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            string excludedConnectionId1,
                            string excludedConnectionId2,
                            string excludedConnectionId3,
                            string excludedConnectionId4,
                            string excludedConnectionId5) cil managed
  // SIG: 10 01 06 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E
  {
    .custom /*0C000183:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000028*/ 
      .custom /*0C0000CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*0600006D*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            string excludedConnectionId1,
                            string excludedConnectionId2,
                            string excludedConnectionId3,
                            string excludedConnectionId4,
                            string excludedConnectionId5,
                            string excludedConnectionId6) cil managed
  // SIG: 10 01 07 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C000186:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000029*/ 
      .custom /*0C0000CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*0600006E*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            string excludedConnectionId1,
                            string excludedConnectionId2,
                            string excludedConnectionId3,
                            string excludedConnectionId4,
                            string excludedConnectionId5,
                            string excludedConnectionId6,
                            string excludedConnectionId7) cil managed
  // SIG: 10 01 08 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C000189:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002A*/ 
      .custom /*0C0000D1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*0600006F*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            string excludedConnectionId1,
                            string excludedConnectionId2,
                            string excludedConnectionId3,
                            string excludedConnectionId4,
                            string excludedConnectionId5,
                            string excludedConnectionId6,
                            string excludedConnectionId7,
                            string excludedConnectionId8) cil managed
  // SIG: 10 01 09 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C00018E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002B*/ 
      .custom /*0C0000D4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*06000070*/ public hidebysig static 
          !!T  AllExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                            class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<string> excludedConnectionIds) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 B8 01 1E 00 15 12 80 D5 01 0E
  {
    .custom /*0C000192:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002C*/ 
      .custom /*0C0000D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::AllExcept

  .method /*06000071*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          string connection1) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 B8 01 1E 00 0E
  {
    .custom /*0C000196:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002D*/ 
      .custom /*0C0000DD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*06000072*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          string connection1,
                          string connection2) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 B8 01 1E 00 0E 0E
  {
    .custom /*0C000199:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00002F*/ 
      .custom /*0C0000E3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*06000073*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          string connection1,
                          string connection2,
                          string connection3) cil managed
  // SIG: 10 01 04 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E
  {
    .custom /*0C00019D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000031*/ 
      .custom /*0C0000EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*06000074*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          string connection1,
                          string connection2,
                          string connection3,
                          string connection4) cil managed
  // SIG: 10 01 05 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E
  {
    .custom /*0C0001A0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000033*/ 
      .custom /*0C0000F1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*06000075*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          string connection1,
                          string connection2,
                          string connection3,
                          string connection4,
                          string connection5) cil managed
  // SIG: 10 01 06 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E
  {
    .custom /*0C0001A2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000035*/ 
      .custom /*0C0000F5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*06000076*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          string connection1,
                          string connection2,
                          string connection3,
                          string connection4,
                          string connection5,
                          string connection6) cil managed
  // SIG: 10 01 07 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001A5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000037*/ 
      .custom /*0C0000FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*06000077*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          string connection1,
                          string connection2,
                          string connection3,
                          string connection4,
                          string connection5,
                          string connection6,
                          string connection7) cil managed
  // SIG: 10 01 08 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001A7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000038*/ 
      .custom /*0C000101:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*06000078*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          string connection1,
                          string connection2,
                          string connection3,
                          string connection4,
                          string connection5,
                          string connection6,
                          string connection7,
                          string connection8) cil managed
  // SIG: 10 01 09 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001AA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000039*/ 
      .custom /*0C000105:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*06000079*/ public hidebysig static 
          !!T  Clients<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<string> connectionIds) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 B8 01 1E 00 15 12 80 D5 01 0E
  {
    .custom /*0C0001AD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003A*/ 
      .custom /*0C00010A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Clients

  .method /*0600007A*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         string group1) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 B8 01 1E 00 0E
  {
    .custom /*0C0001AF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003C*/ 
      .custom /*0C000110:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*0600007B*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         string group1,
                         string group2) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 B8 01 1E 00 0E 0E
  {
    .custom /*0C0001B1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003D*/ 
      .custom /*0C000114:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*0600007C*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         string group1,
                         string group2,
                         string group3) cil managed
  // SIG: 10 01 04 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E
  {
    .custom /*0C0001B5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00003E*/ 
      .custom /*0C000117:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*0600007D*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         string group1,
                         string group2,
                         string group3,
                         string group4) cil managed
  // SIG: 10 01 05 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E
  {
    .custom /*0C0001B7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000040*/ 
      .custom /*0C00011D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*0600007E*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         string group1,
                         string group2,
                         string group3,
                         string group4,
                         string group5) cil managed
  // SIG: 10 01 06 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E
  {
    .custom /*0C0001B9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000042*/ 
      .custom /*0C000120:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*0600007F*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         string group1,
                         string group2,
                         string group3,
                         string group4,
                         string group5,
                         string group6) cil managed
  // SIG: 10 01 07 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001BB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000044*/ 
      .custom /*0C000129:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*06000080*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         string group1,
                         string group2,
                         string group3,
                         string group4,
                         string group5,
                         string group6,
                         string group7) cil managed
  // SIG: 10 01 08 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001BD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000046*/ 
      .custom /*0C00012F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*06000081*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         string group1,
                         string group2,
                         string group3,
                         string group4,
                         string group5,
                         string group6,
                         string group7,
                         string group8) cil managed
  // SIG: 10 01 09 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001BF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000048*/ 
      .custom /*0C000136:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*06000082*/ public hidebysig static 
          !!T  Groups<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<string> groupNames) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 B8 01 1E 00 15 12 80 D5 01 0E
  {
    .custom /*0C0001C1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00004A*/ 
      .custom /*0C00013C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Groups

  .method /*06000083*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              string excludedConnectionId1) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 B8 01 1E 00 0E 0E
  {
    .custom /*0C0001C3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00004C*/ 
      .custom /*0C000146:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*06000084*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              string excludedConnectionId1,
                              string excludedConnectionId2) cil managed
  // SIG: 10 01 04 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E
  {
    .custom /*0C0001C5:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00004E*/ 
      .custom /*0C00014D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*06000085*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              string excludedConnectionId1,
                              string excludedConnectionId2,
                              string excludedConnectionId3) cil managed
  // SIG: 10 01 05 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E
  {
    .custom /*0C0001C7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000050*/ 
      .custom /*0C00014F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*06000086*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              string excludedConnectionId1,
                              string excludedConnectionId2,
                              string excludedConnectionId3,
                              string excludedConnectionId4) cil managed
  // SIG: 10 01 06 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E
  {
    .custom /*0C0001C9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000052*/ 
      .custom /*0C000151:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*06000087*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              string excludedConnectionId1,
                              string excludedConnectionId2,
                              string excludedConnectionId3,
                              string excludedConnectionId4,
                              string excludedConnectionId5) cil managed
  // SIG: 10 01 07 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001CB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000054*/ 
      .custom /*0C000153:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*06000088*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              string excludedConnectionId1,
                              string excludedConnectionId2,
                              string excludedConnectionId3,
                              string excludedConnectionId4,
                              string excludedConnectionId5,
                              string excludedConnectionId6) cil managed
  // SIG: 10 01 08 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001CD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000056*/ 
      .custom /*0C000157:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*06000089*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              string excludedConnectionId1,
                              string excludedConnectionId2,
                              string excludedConnectionId3,
                              string excludedConnectionId4,
                              string excludedConnectionId5,
                              string excludedConnectionId6,
                              string excludedConnectionId7) cil managed
  // SIG: 10 01 09 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001CF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000057*/ 
      .custom /*0C000159:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*0600008A*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              string excludedConnectionId1,
                              string excludedConnectionId2,
                              string excludedConnectionId3,
                              string excludedConnectionId4,
                              string excludedConnectionId5,
                              string excludedConnectionId6,
                              string excludedConnectionId7,
                              string excludedConnectionId8) cil managed
  // SIG: 10 01 0A 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001D0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000058*/ 
      .custom /*0C00015A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*0600008B*/ public hidebysig static 
          !!T  GroupExcept<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                              string groupName,
                              class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<string> excludedConnectionIds) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 B8 01 1E 00 0E 15 12 80 D5 01 0E
  {
    .custom /*0C0001D2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000059*/ 
      .custom /*0C00015C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::GroupExcept

  .method /*0600008C*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        string user1) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 B8 01 1E 00 0E
  {
    .custom /*0C0001D4:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005A*/ 
      .custom /*0C00015D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

  .method /*0600008D*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        string user1,
                        string user2) cil managed
  // SIG: 10 01 03 1E 00 15 12 80 B8 01 1E 00 0E 0E
  {
    .custom /*0C0001D6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005B*/ 
      .custom /*0C00015E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

  .method /*0600008E*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        string user1,
                        string user2,
                        string user3) cil managed
  // SIG: 10 01 04 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E
  {
    .custom /*0C0001D8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005C*/ 
      .custom /*0C000160:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

  .method /*0600008F*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        string user1,
                        string user2,
                        string user3,
                        string user4) cil managed
  // SIG: 10 01 05 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E
  {
    .custom /*0C0001DA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005D*/ 
      .custom /*0C000162:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

  .method /*06000090*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        string user1,
                        string user2,
                        string user3,
                        string user4,
                        string user5) cil managed
  // SIG: 10 01 06 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E
  {
    .custom /*0C0001DD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005E*/ 
      .custom /*0C000166:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

  .method /*06000091*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        string user1,
                        string user2,
                        string user3,
                        string user4,
                        string user5,
                        string user6) cil managed
  // SIG: 10 01 07 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001DF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A00005F*/ 
      .custom /*0C000169:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

  .method /*06000092*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        string user1,
                        string user2,
                        string user3,
                        string user4,
                        string user5,
                        string user6,
                        string user7) cil managed
  // SIG: 10 01 08 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001E1:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000060*/ 
      .custom /*0C00016C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

  .method /*06000093*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        string user1,
                        string user2,
                        string user3,
                        string user4,
                        string user5,
                        string user6,
                        string user7,
                        string user8) cil managed
  // SIG: 10 01 09 1E 00 15 12 80 B8 01 1E 00 0E 0E 0E 0E 0E 0E 0E 0E
  {
    .custom /*0C0001E3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000061*/ 
      .custom /*0C000170:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

  .method /*06000094*/ public hidebysig static 
          !!T  Users<T>(class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!!T> hubClients,
                        class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<string> userIds) cil managed
  // SIG: 10 01 02 1E 00 15 12 80 B8 01 1E 00 15 12 80 D5 01 0E
  {
    .custom /*0C0001E6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type T /*2A000063*/ 
      .custom /*0C000175:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClientsExtensions::Users

} // end of class Microsoft.AspNetCore.SignalR.HubClientsExtensions

.class /*02000018*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubConnectionContext
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000087:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000068*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .method /*06000208*/ public hidebysig static 
            void  HandshakeComplete(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                    string hubProtocol) cil managed
    // SIG: 00 02 01 12 81 05 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::HandshakeComplete

    .method /*06000209*/ public hidebysig static 
            void  HandshakeCanceled(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger) cil managed
    // SIG: 00 01 01 12 81 05
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::HandshakeCanceled

    .method /*0600020A*/ public hidebysig static 
            void  SentPing(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger) cil managed
    // SIG: 00 01 01 12 81 05
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::SentPing

    .method /*0600020B*/ public hidebysig static 
            void  TransportBufferFull(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger) cil managed
    // SIG: 00 01 01 12 81 05
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::TransportBufferFull

    .method /*0600020C*/ public hidebysig static 
            void  HandshakeFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                  class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
    // SIG: 00 02 01 12 81 05 12 80 C5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::HandshakeFailed

    .method /*0600020D*/ public hidebysig static 
            void  FailedWritingMessage(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                       class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
    // SIG: 00 02 01 12 81 05 12 80 C5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::FailedWritingMessage

    .method /*0600020E*/ public hidebysig static 
            void  ProtocolVersionFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                        string protocolName,
                                        int32 version) cil managed
    // SIG: 00 03 01 12 81 05 0E 08
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ProtocolVersionFailed

    .method /*0600020F*/ public hidebysig static 
            void  AbortFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                              class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
    // SIG: 00 02 01 12 81 05 12 80 C5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::AbortFailed

    .method /*06000210*/ public hidebysig static 
            void  ClientTimeout(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ timeout) cil managed
    // SIG: 00 02 01 12 81 05 11 81 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ClientTimeout

    .method /*06000211*/ public hidebysig static 
            void  HandshakeSizeLimitExceeded(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                             int64 maxMessageSize) cil managed
    // SIG: 00 02 01 12 81 05 0A
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::HandshakeSizeLimitExceeded

  } // end of class Log

  .class /*02000069*/ auto ansi sealed nested private beforefieldinit '<<AbortConnection>g__InnerAbortConnection|87_0>d'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C00017E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000030*/ public int32 '<>1__state'
    .field /*04000031*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000032*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*04000033*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .method /*06000212*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<AbortConnection>g__InnerAbortConnection|87_0>d'::MoveNext

    .method /*06000213*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C00028F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<AbortConnection>g__InnerAbortConnection|87_0>d'::SetStateMachine

  } // end of class '<<AbortConnection>g__InnerAbortConnection|87_0>d'

  .class /*0200006A*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000180:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000034*/ public static initonly class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*//'<>c'/*0200006A*/ '<>9'
    .field /*04000035*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<object> '<>9__25_0'
    .field /*04000036*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<object> '<>9__25_1'
    .field /*04000037*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<object> '<>9__81_0'
    .field /*04000038*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<object> '<>9__85_0'
    .method /*06000214*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000215*/ assembly hidebysig 
            instance void  '<.ctor>b__25_0'(object state) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__25_0'

    .method /*06000216*/ assembly hidebysig 
            instance void  '<.ctor>b__25_1'(object state) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__25_1'

    .method /*06000217*/ assembly hidebysig 
            instance void  '<HandshakeAsync>b__81_0'(object state) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<HandshakeAsync>b__81_0'

    .method /*06000218*/ assembly hidebysig 
            instance void  '<StartClientTimeout>b__85_0'(object state) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<StartClientTimeout>b__85_0'

    .method /*06000219*/ assembly hidebysig 
            instance void  '<.cctor>b__92_0'(object state) cil managed
    // SIG: 20 01 01 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__92_0'

  } // end of class '<>c'

  .class /*0200006B*/ auto ansi sealed nested private beforefieldinit '<AbortAsyncSlow>d__83'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C000182:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000039*/ public int32 '<>1__state'
    .field /*0400003A*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*0400003B*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ '<>4__this'
    .field /*0400003C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .method /*0600021A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AbortAsyncSlow>d__83'::MoveNext

    .method /*0600021B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C000292:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AbortAsyncSlow>d__83'::SetStateMachine

  } // end of class '<AbortAsyncSlow>d__83'

  .class /*0200006C*/ auto ansi sealed nested private beforefieldinit '<CompleteWriteAsync>d__73'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C000184:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400003D*/ public int32 '<>1__state'
    .field /*0400003E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*0400003F*/ public valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.FlushResult/*01000048*/> task
    .field /*04000040*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ '<>4__this'
    .field /*04000041*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.FlushResult/*01000048*/> '<>u__1'
    .method /*0600021C*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CompleteWriteAsync>d__73'::MoveNext

    .method /*0600021D*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C000293:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CompleteWriteAsync>d__73'::SetStateMachine

  } // end of class '<CompleteWriteAsync>d__73'

  .class /*0200006D*/ auto ansi sealed nested private beforefieldinit '<HandshakeAsync>d__81'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C000187:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000042*/ public int32 '<>1__state'
    .field /*04000043*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004A*/<bool> '<>t__builder'
    .field /*04000044*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ '<>4__this'
    .field /*04000045*/ public valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ timeout
    .field /*04000046*/ public class Microsoft.AspNetCore.SignalR.IHubProtocolResolver/*02000033*/ protocolResolver
    .field /*04000047*/ public class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> supportedProtocols
    .field /*04000048*/ public class Microsoft.AspNetCore.SignalR.IUserIdProvider/*02000035*/ userIdProvider
    .field /*04000049*/ public bool enableDetailedErrors
    .field /*0400004A*/ private class [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.PipeReader/*01000039*/ '<input>5__2'
    .field /*0400004B*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100003C*/ '<cts>5__3'
    .field /*0400004C*/ private valuetype [System.Runtime/*23000001*/]System.Threading.CancellationTokenRegistration/*0100004B*/ '<registration>5__4'
    .field /*0400004D*/ private valuetype [System.Memory/*2300000C*/]System.SequencePosition/*0100004C*/ '<consumed>5__5'
    .field /*0400004E*/ private valuetype [System.Memory/*2300000C*/]System.SequencePosition/*0100004C*/ '<examined>5__6'
    .field /*0400004F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.ReadResult/*0100004D*/> '<>u__1'
    .field /*04000050*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__2'
    .method /*0600021E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandshakeAsync>d__81'::MoveNext

    .method /*0600021F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C000295:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HandshakeAsync>d__81'::SetStateMachine

  } // end of class '<HandshakeAsync>d__81'

  .class /*0200006E*/ auto ansi sealed nested private beforefieldinit '<TryWritePingSlowAsync>d__77'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C00018A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000051*/ public int32 '<>1__state'
    .field /*04000052*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000053*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ '<>4__this'
    .field /*04000054*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.FlushResult/*01000048*/> '<>u__1'
    .method /*06000220*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<TryWritePingSlowAsync>d__77'::MoveNext

    .method /*06000221*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C000296:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<TryWritePingSlowAsync>d__77'::SetStateMachine

  } // end of class '<TryWritePingSlowAsync>d__77'

  .class /*0200006F*/ auto ansi sealed nested private beforefieldinit '<WriteHandshakeResponseAsync>d__78'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C00018F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000055*/ public int32 '<>1__state'
    .field /*04000056*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000057*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ '<>4__this'
    .field /*04000058*/ public class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HandshakeResponseMessage/*0100004E*/ message
    .field /*04000059*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .field /*0400005A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.FlushResult/*01000048*/> '<>u__2'
    .method /*06000222*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteHandshakeResponseAsync>d__78'::MoveNext

    .method /*06000223*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C000297:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteHandshakeResponseAsync>d__78'::SetStateMachine

  } // end of class '<WriteHandshakeResponseAsync>d__78'

  .class /*02000070*/ auto ansi sealed nested private beforefieldinit '<WriteSlowAsync>d__74'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C000193:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400005B*/ public int32 '<>1__state'
    .field /*0400005C*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*0400005D*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ '<>4__this'
    .field /*0400005E*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken
    .field /*0400005F*/ public bool ignoreAbort
    .field /*04000060*/ public class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/ message
    .field /*04000061*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .field /*04000062*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.FlushResult/*01000048*/> '<>u__2'
    .method /*06000224*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteSlowAsync>d__74'::MoveNext

    .method /*06000225*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C000299:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteSlowAsync>d__74'::SetStateMachine

  } // end of class '<WriteSlowAsync>d__74'

  .class /*02000071*/ auto ansi sealed nested private beforefieldinit '<WriteSlowAsync>d__75'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C000197:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000063*/ public int32 '<>1__state'
    .field /*04000064*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000065*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ '<>4__this'
    .field /*04000066*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken
    .field /*04000067*/ public class Microsoft.AspNetCore.SignalR.SerializedHubMessage/*02000036*/ message
    .field /*04000068*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .field /*04000069*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.FlushResult/*01000048*/> '<>u__2'
    .method /*06000226*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteSlowAsync>d__75'::MoveNext

    .method /*06000227*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C00029A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteSlowAsync>d__75'::SetStateMachine

  } // end of class '<WriteSlowAsync>d__75'

  .method /*06000095*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000036*/ connectionContext,
                               class Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/ contextOptions,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000037*/ loggerFactory) cil managed
  // SIG: 20 03 01 12 80 D9 12 64 12 80 DD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::.ctor

  .method /*06000096*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.StreamTracker/*02000039*/ 
          get_StreamTracker() cil managed
  // SIG: 20 00 12 80 E4
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_StreamTracker

  .method /*06000097*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ 
          get_HubCallerContext() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C0001EA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_HubCallerContext

  .method /*06000098*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.Internal.HubCallerClients/*02000044*/ 
          get_HubCallerClients() cil managed
  // SIG: 20 00 12 81 10
  {
    .custom /*0C0001ED:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_HubCallerClients

  .method /*06000099*/ assembly hidebysig specialname 
          instance void  set_HubCallerClients(class Microsoft.AspNetCore.SignalR.Internal.HubCallerClients/*02000044*/ 'value') cil managed
  // SIG: 20 01 01 12 81 10
  {
    .custom /*0C0001EF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::set_HubCallerClients

  .method /*0600009A*/ assembly hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*01000031*/ 
          get_CloseException() cil managed
  // SIG: 20 00 12 80 C5
  {
    .custom /*0C0001F0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001F1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_CloseException

  .method /*0600009B*/ assembly hidebysig specialname 
          instance class [System.Threading/*23000008*/]System.Threading.SemaphoreSlim/*01000038*/ 
          get_ActiveInvocationLimit() cil managed
  // SIG: 20 00 12 80 E1
  {
    .custom /*0C0001F2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001F3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_ActiveInvocationLimit

  .method /*0600009C*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ 
          get_ConnectionAborted() cil managed
  // SIG: 20 00 11 80 AD
  {
    .custom /*0C0001F5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_ConnectionAborted

  .method /*0600009D*/ public hidebysig newslot specialname virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_ConnectionId

  .method /*0600009E*/ public hidebysig newslot specialname virtual 
          instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/ 
          get_User() cil managed
  // SIG: 20 00 12 80 C9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_User

  .method /*0600009F*/ public hidebysig newslot specialname virtual 
          instance class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/ 
          get_Features() cil managed
  // SIG: 20 00 12 80 D1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_Features

  .method /*060000A0*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 80 CD 02 1C 1C
  {
    .param [0]/*080001C7*/ 
    .custom /*0C000276:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_Items

  .method /*060000A1*/ assembly hidebysig specialname 
          instance bool  get_AllowReconnect() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_AllowReconnect

  .method /*060000A2*/ assembly hidebysig specialname 
          instance class [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.PipeReader/*01000039*/ 
          get_Input() cil managed
  // SIG: 20 00 12 80 E5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_Input

  .method /*060000A3*/ public hidebysig specialname 
          instance string  get_UserIdentifier() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001F9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001FA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_UserIdentifier

  .method /*060000A4*/ public hidebysig specialname 
          instance void  set_UserIdentifier(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001FB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0001FC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::set_UserIdentifier

  .method /*060000A5*/ public hidebysig newslot specialname virtual 
          instance class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/ 
          get_Protocol() cil managed
  // SIG: 20 00 12 80 E9
  {
    .custom /*0C0001FE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_Protocol

  .method /*060000A6*/ public hidebysig newslot specialname virtual 
          instance void  set_Protocol(class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 E9
  {
    .custom /*0C0001FF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::set_Protocol

  .method /*060000A7*/ assembly hidebysig specialname 
          instance class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100003C*/> 
          get_ActiveRequestCancellationSources() cil managed
  // SIG: 20 00 15 12 80 ED 02 0E 12 80 F1
  {
    .custom /*0C000200:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::get_ActiveRequestCancellationSources

  .method /*060000A8*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003D*/ 
          WriteAsync(class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/ message,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 02 11 80 F5 12 80 F9 11 80 AD
  {
    .param [2]/*080001CB*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::WriteAsync

  .method /*060000A9*/ assembly hidebysig 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003D*/ 
          WriteAsync(class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/ message,
                     bool ignoreAbort,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 11 80 F5 12 80 F9 02 11 80 AD
  {
    .param [3]/*080001CE*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::WriteAsync

  .method /*060000AA*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003D*/ 
          WriteAsync(class Microsoft.AspNetCore.SignalR.SerializedHubMessage/*02000036*/ message,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 02 11 80 F5 12 80 D8 11 80 AD
  {
    .param [2]/*080001D0*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::WriteAsync

  .method /*060000AB*/ public hidebysig newslot virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::Abort

  .method /*060000AC*/ assembly hidebysig 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000029*/<bool> 
          HandshakeAsync(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ timeout,
                         class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> supportedProtocols,
                         class Microsoft.AspNetCore.SignalR.IHubProtocolResolver/*02000033*/ protocolResolver,
                         class Microsoft.AspNetCore.SignalR.IUserIdProvider/*02000035*/ userIdProvider,
                         bool enableDetailedErrors) cil managed
  // SIG: 20 05 15 12 80 A5 01 02 11 81 01 15 12 80 B5 01 0E 12 80 CC 12 80 D4 02
  {
    .custom /*0C000202:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 47 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..GMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        48 75 62 43 6F 6E 6E 65 63 74 69 6F 6E 43 6F 6E   // HubConnectionCon
                                                                                                                                                                                                                                        74 65 78 74 2B 3C 48 61 6E 64 73 68 61 6B 65 41   // text+<HandshakeA
                                                                                                                                                                                                                                        73 79 6E 63 3E 64 5F 5F 38 31 00 00 )             // sync>d__81..
    .param [2]/*080001D2*/ 
    .custom /*0C000285:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::HandshakeAsync

  .method /*060000AD*/ assembly hidebysig 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          AbortAsync() cil managed
  // SIG: 20 00 12 80 A9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::AbortAsync

  .method /*060000AE*/ assembly hidebysig 
          instance void  StartClientTimeout() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::StartClientTimeout

  .method /*060000AF*/ assembly hidebysig 
          instance void  BeginClientTimeout() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::BeginClientTimeout

  .method /*060000B0*/ assembly hidebysig 
          instance void  StopClientTimeout() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::StopClientTimeout

  .method /*060000B1*/ assembly hidebysig 
          instance void  Cleanup() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::Cleanup

  .method /*060000B2*/ assembly hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          '<AbortConnection>g__InnerAbortConnection|87_0'(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 00 01 12 80 A9 12 60
  {
    .custom /*0C000204:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 62 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..bMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        48 75 62 43 6F 6E 6E 65 63 74 69 6F 6E 43 6F 6E   // HubConnectionCon
                                                                                                                                                                                                                                        74 65 78 74 2B 3C 3C 41 62 6F 72 74 43 6F 6E 6E   // text+<<AbortConn
                                                                                                                                                                                                                                        65 63 74 69 6F 6E 3E 67 5F 5F 49 6E 6E 65 72 41   // ection>g__InnerA
                                                                                                                                                                                                                                        62 6F 72 74 43 6F 6E 6E 65 63 74 69 6F 6E 7C 38   // bortConnection|8
                                                                                                                                                                                                                                        37 5F 30 3E 64 00 00 )                            // 7_0>d..
    .custom /*0C000205:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContext::'<AbortConnection>g__InnerAbortConnection|87_0'

  .property /*17000021*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.StreamTracker/*02000039*/
          StreamTracker()
  {
    .get instance class Microsoft.AspNetCore.SignalR.StreamTracker/*02000039*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_StreamTracker() /* 06000096 */
  } // end of property HubConnectionContext::StreamTracker
  .property /*17000022*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/
          HubCallerContext()
  {
    .get instance class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_HubCallerContext() /* 06000097 */
  } // end of property HubConnectionContext::HubCallerContext
  .property /*17000023*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.Internal.HubCallerClients/*02000044*/
          HubCallerClients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.Internal.HubCallerClients/*02000044*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_HubCallerClients() /* 06000098 */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::set_HubCallerClients(class Microsoft.AspNetCore.SignalR.Internal.HubCallerClients/*02000044*/) /* 06000099 */
  } // end of property HubConnectionContext::HubCallerClients
  .property /*17000024*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*01000031*/
          CloseException()
  {
    .custom /*0C0000BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Exception/*01000031*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_CloseException() /* 0600009A */
  } // end of property HubConnectionContext::CloseException
  .property /*17000025*/ instance callconv(8) class [System.Threading/*23000008*/]System.Threading.SemaphoreSlim/*01000038*/
          ActiveInvocationLimit()
  {
    .custom /*0C0000BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Threading/*23000008*/]System.Threading.SemaphoreSlim/*01000038*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_ActiveInvocationLimit() /* 0600009B */
  } // end of property HubConnectionContext::ActiveInvocationLimit
  .property /*17000026*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/
          ConnectionAborted()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_ConnectionAborted() /* 0600009C */
  } // end of property HubConnectionContext::ConnectionAborted
  .property /*17000027*/ instance callconv(8) string
          ConnectionId()
  {
    .get instance string Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_ConnectionId() /* 0600009D */
  } // end of property HubConnectionContext::ConnectionId
  .property /*17000028*/ instance callconv(8) class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/
          User()
  {
    .get instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_User() /* 0600009E */
  } // end of property HubConnectionContext::User
  .property /*17000029*/ instance callconv(8) class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/
          Features()
  {
    .get instance class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_Features() /* 0600009F */
  } // end of property HubConnectionContext::Features
  .property /*1700002A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object>
          Items()
  {
    .custom /*0C0000D0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object> Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_Items() /* 060000A0 */
  } // end of property HubConnectionContext::Items
  .property /*1700002B*/ instance callconv(8) bool
          AllowReconnect()
  {
    .get instance bool Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_AllowReconnect() /* 060000A1 */
  } // end of property HubConnectionContext::AllowReconnect
  .property /*1700002C*/ instance callconv(8) class [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.PipeReader/*01000039*/
          Input()
  {
    .get instance class [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.PipeReader/*01000039*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_Input() /* 060000A2 */
  } // end of property HubConnectionContext::Input
  .property /*1700002D*/ instance callconv(8) string
          UserIdentifier()
  {
    .custom /*0C0000DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_UserIdentifier() /* 060000A3 */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::set_UserIdentifier(string) /* 060000A4 */
  } // end of property HubConnectionContext::UserIdentifier
  .property /*1700002E*/ instance callconv(8) class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/
          Protocol()
  {
    .get instance class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/ Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_Protocol() /* 060000A5 */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::set_Protocol(class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/) /* 060000A6 */
  } // end of property HubConnectionContext::Protocol
  .property /*1700002F*/ instance callconv(8) class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100003C*/>
          ActiveRequestCancellationSources()
  {
    .get instance class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100003C*/> Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/::get_ActiveRequestCancellationSources() /* 060000A7 */
  } // end of property HubConnectionContext::ActiveRequestCancellationSources
} // end of class Microsoft.AspNetCore.SignalR.HubConnectionContext

.class /*02000019*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubConnectionContextOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000B3*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ 
          get_KeepAliveInterval() cil managed
  // SIG: 20 00 11 81 01
  {
    .custom /*0C000207:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::get_KeepAliveInterval

  .method /*060000B4*/ public hidebysig specialname 
          instance void  set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ 'value') cil managed
  // SIG: 20 01 01 11 81 01
  {
    .custom /*0C000208:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::set_KeepAliveInterval

  .method /*060000B5*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ 
          get_ClientTimeoutInterval() cil managed
  // SIG: 20 00 11 81 01
  {
    .custom /*0C00020B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::get_ClientTimeoutInterval

  .method /*060000B6*/ public hidebysig specialname 
          instance void  set_ClientTimeoutInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ 'value') cil managed
  // SIG: 20 01 01 11 81 01
  {
    .custom /*0C00020C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::set_ClientTimeoutInterval

  .method /*060000B7*/ public hidebysig specialname 
          instance int32  get_StreamBufferCapacity() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00020D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::get_StreamBufferCapacity

  .method /*060000B8*/ public hidebysig specialname 
          instance void  set_StreamBufferCapacity(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C00020E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::set_StreamBufferCapacity

  .method /*060000B9*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64> 
          get_MaximumReceiveMessageSize() cil managed
  // SIG: 20 00 15 11 81 3D 01 0A
  {
    .custom /*0C000210:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::get_MaximumReceiveMessageSize

  .method /*060000BA*/ public hidebysig specialname 
          instance void  set_MaximumReceiveMessageSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 81 3D 01 0A
  {
    .custom /*0C000212:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::set_MaximumReceiveMessageSize

  .method /*060000BB*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/ 
          get_SystemClock() cil managed
  // SIG: 20 00 12 38
  {
    .custom /*0C000213:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::get_SystemClock

  .method /*060000BC*/ assembly hidebysig specialname 
          instance void  set_SystemClock(class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/ 'value') cil managed
  // SIG: 20 01 01 12 38
  {
    .custom /*0C000215:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::set_SystemClock

  .method /*060000BD*/ public hidebysig specialname 
          instance int32  get_MaximumParallelInvocations() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000217:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::get_MaximumParallelInvocations

  .method /*060000BE*/ public hidebysig specialname 
          instance void  set_MaximumParallelInvocations(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C000218:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::set_MaximumParallelInvocations

  .method /*060000BF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionContextOptions::.ctor

  .property /*17000030*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/
          KeepAliveInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::get_KeepAliveInterval() /* 060000B3 */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/) /* 060000B4 */
  } // end of property HubConnectionContextOptions::KeepAliveInterval
  .property /*17000031*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/
          ClientTimeoutInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::get_ClientTimeoutInterval() /* 060000B5 */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::set_ClientTimeoutInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/) /* 060000B6 */
  } // end of property HubConnectionContextOptions::ClientTimeoutInterval
  .property /*17000032*/ instance callconv(8) int32
          StreamBufferCapacity()
  {
    .get instance int32 Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::get_StreamBufferCapacity() /* 060000B7 */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::set_StreamBufferCapacity(int32) /* 060000B8 */
  } // end of property HubConnectionContextOptions::StreamBufferCapacity
  .property /*17000033*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64>
          MaximumReceiveMessageSize()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64> Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::get_MaximumReceiveMessageSize() /* 060000B9 */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::set_MaximumReceiveMessageSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64>) /* 060000BA */
  } // end of property HubConnectionContextOptions::MaximumReceiveMessageSize
  .property /*17000034*/ instance callconv(8) class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/
          SystemClock()
  {
    .get instance class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/ Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::get_SystemClock() /* 060000BB */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::set_SystemClock(class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/) /* 060000BC */
  } // end of property HubConnectionContextOptions::SystemClock
  .property /*17000035*/ instance callconv(8) int32
          MaximumParallelInvocations()
  {
    .get instance int32 Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::get_MaximumParallelInvocations() /* 060000BD */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionContextOptions/*02000019*/::set_MaximumParallelInvocations(int32) /* 060000BE */
  } // end of property HubConnectionContextOptions::MaximumParallelInvocations
} // end of class Microsoft.AspNetCore.SignalR.HubConnectionContextOptions

.class /*0200001A*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubConnectionHandler`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionHandler/*01000050*/
{
  .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000096:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type THub /*2A000002*/ 
    .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000072*/ auto ansi sealed nested private beforefieldinit '<DispatchMessagesAsync>d__20'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C00019A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A00002E*/ 
      .custom /*0C0000E0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400006A*/ public int32 '<>1__state'
    .field /*0400006B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*0400006C*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*0400006D*/ public class Microsoft.AspNetCore.SignalR.HubConnectionHandler`1/*0200001A*/<!THub> '<>4__this'
    .field /*0400006E*/ private class [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.PipeReader/*01000039*/ '<input>5__2'
    .field /*0400006F*/ private class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/ '<protocol>5__3'
    .field /*04000070*/ private class Microsoft.AspNetCore.SignalR.Internal.HubConnectionBinder`1/*02000047*/<!THub> '<binder>5__4'
    .field /*04000071*/ private valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.ReadResult/*0100004D*/ '<result>5__5'
    .field /*04000072*/ private valuetype [System.Memory/*2300000C*/]System.Buffers.ReadOnlySequence`1/*01000053*/<uint8> '<buffer>5__6'
    .field /*04000073*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<valuetype [System.IO.Pipelines/*23000009*/]System.IO.Pipelines.ReadResult/*0100004D*/> '<>u__1'
    .field /*04000074*/ private bool '<messageReceived>5__7'
    .field /*04000075*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__2'
    .field /*04000076*/ private int64 '<maxMessageSize>5__8'
    .field /*04000077*/ private valuetype [System.Memory/*2300000C*/]System.Buffers.ReadOnlySequence`1/*01000053*/<uint8> '<segment>5__9'
    .method /*06000228*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DispatchMessagesAsync>d__20'::MoveNext

    .method /*06000229*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C00029C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DispatchMessagesAsync>d__20'::SetStateMachine

  } // end of class '<DispatchMessagesAsync>d__20'

  .class /*02000073*/ auto ansi sealed nested private beforefieldinit '<HubOnDisconnectedAsync>d__18'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C00019E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000030*/ 
      .custom /*0C0000E6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000078*/ public int32 '<>1__state'
    .field /*04000079*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*0400007A*/ public class Microsoft.AspNetCore.SignalR.HubConnectionHandler`1/*0200001A*/<!THub> '<>4__this'
    .field /*0400007B*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*0400007C*/ public class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception
    .field /*0400007D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .method /*0600022A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HubOnDisconnectedAsync>d__18'::MoveNext

    .method /*0600022B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C00029D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<HubOnDisconnectedAsync>d__18'::SetStateMachine

  } // end of class '<HubOnDisconnectedAsync>d__18'

  .class /*02000074*/ auto ansi sealed nested private beforefieldinit '<OnConnectedAsync>d__16'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001A1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000032*/ 
      .custom /*0C0000EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400007E*/ public int32 '<>1__state'
    .field /*0400007F*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000080*/ public class Microsoft.AspNetCore.SignalR.HubConnectionHandler`1/*0200001A*/<!THub> '<>4__this'
    .field /*04000081*/ public class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000036*/ connection
    .field /*04000082*/ private class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ '<connectionContext>5__2'
    .field /*04000083*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000054*/<bool> '<>u__1'
    .field /*04000084*/ private object '<>7__wrap2'
    .field /*04000085*/ private int32 '<>7__wrap3'
    .field /*04000086*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__2'
    .method /*0600022C*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnConnectedAsync>d__16'::MoveNext

    .method /*0600022D*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C00029E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnConnectedAsync>d__16'::SetStateMachine

  } // end of class '<OnConnectedAsync>d__16'

  .class /*02000075*/ auto ansi sealed nested private beforefieldinit '<RunHubAsync>d__17'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001A3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000034*/ 
      .custom /*0C0000F3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000087*/ public int32 '<>1__state'
    .field /*04000088*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000089*/ public class Microsoft.AspNetCore.SignalR.HubConnectionHandler`1/*0200001A*/<!THub> '<>4__this'
    .field /*0400008A*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*0400008B*/ private object '<>7__wrap1'
    .field /*0400008C*/ private int32 '<>7__wrap2'
    .field /*0400008D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .method /*0600022E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunHubAsync>d__17'::MoveNext

    .method /*0600022F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C00029F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunHubAsync>d__17'::SetStateMachine

  } // end of class '<RunHubAsync>d__17'

  .class /*02000076*/ auto ansi sealed nested private beforefieldinit '<SendCloseAsync>d__19'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001A6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000036*/ 
      .custom /*0C0000F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400008E*/ public int32 '<>1__state'
    .field /*0400008F*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000090*/ public class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception
    .field /*04000091*/ public class Microsoft.AspNetCore.SignalR.HubConnectionHandler`1/*0200001A*/<!THub> '<>4__this'
    .field /*04000092*/ public bool allowReconnect
    .field /*04000093*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*04000094*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__1'
    .method /*06000230*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SendCloseAsync>d__19'::MoveNext

    .method /*06000231*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002A0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SendCloseAsync>d__19'::SetStateMachine

  } // end of class '<SendCloseAsync>d__19'

  .method /*060000C0*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/ 
          get_SystemClock() cil managed
  // SIG: 20 00 12 38
  {
    .custom /*0C00021A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandler`1::get_SystemClock

  .method /*060000C1*/ assembly hidebysig specialname 
          instance void  set_SystemClock(class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/ 'value') cil managed
  // SIG: 20 01 01 12 38
  {
    .custom /*0C00021C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandler`1::set_SystemClock

  .method /*060000C2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               class Microsoft.AspNetCore.SignalR.IHubProtocolResolver/*02000033*/ protocolResolver,
                               class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IOptions`1/*01000051*/<class Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/> globalHubOptions,
                               class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IOptions`1/*01000051*/<class Microsoft.AspNetCore.SignalR.HubOptions`1/*02000026*/<!THub>> hubOptions,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILoggerFactory/*01000037*/ loggerFactory,
                               class Microsoft.AspNetCore.SignalR.IUserIdProvider/*02000035*/ userIdProvider,
                               class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceScopeFactory/*01000052*/ serviceScopeFactory) cil managed
  // SIG: 20 07 01 15 12 7C 01 13 00 12 80 CC 15 12 81 45 01 12 80 88 15 12 81 45 01 15 12 80 98 01 13 00
  //      12 80 DD 12 80 D4 12 81 49
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandler`1::.ctor

  .method /*060000C3*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnConnectedAsync(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.ConnectionContext/*01000036*/ connection) cil managed
  // SIG: 20 01 12 80 A9 12 80 D9
  {
    .custom /*0C00021E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 4B 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..KMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        48 75 62 43 6F 6E 6E 65 63 74 69 6F 6E 48 61 6E   // HubConnectionHan
                                                                                                                                                                                                                                        64 6C 65 72 60 31 2B 3C 4F 6E 43 6F 6E 6E 65 63   // dler`1+<OnConnec
                                                                                                                                                                                                                                        74 65 64 41 73 79 6E 63 3E 64 5F 5F 31 36 00 00 ) // tedAsync>d__16..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandler`1::OnConnectedAsync

  .property /*17000036*/ instance callconv(8) class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/
          SystemClock()
  {
    .get instance class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/ Microsoft.AspNetCore.SignalR.HubConnectionHandler`1/*0200001A*/::get_SystemClock() /* 060000C0 */
    .set instance void Microsoft.AspNetCore.SignalR.HubConnectionHandler`1/*0200001A*/::set_SystemClock(class Microsoft.AspNetCore.Internal.ISystemClock/*0200000E*/) /* 060000C1 */
  } // end of property HubConnectionHandler`1::SystemClock
} // end of class Microsoft.AspNetCore.SignalR.HubConnectionHandler`1

.class /*0200001B*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.HubConnectionHandlerLog
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000C4*/ public hidebysig static 
          void  ErrorDispatchingHubEvent(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                         string hubMethod,
                                         class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 00 03 01 12 81 05 0E 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandlerLog::ErrorDispatchingHubEvent

  .method /*060000C5*/ public hidebysig static 
          void  ErrorProcessingRequest(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                       class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 00 02 01 12 81 05 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandlerLog::ErrorProcessingRequest

  .method /*060000C6*/ public hidebysig static 
          void  AbortFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                            class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 00 02 01 12 81 05 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandlerLog::AbortFailed

  .method /*060000C7*/ public hidebysig static 
          void  ErrorSendingClose(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                  class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 00 02 01 12 81 05 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandlerLog::ErrorSendingClose

  .method /*060000C8*/ public hidebysig static 
          void  ConnectedStarting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger) cil managed
  // SIG: 00 01 01 12 81 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandlerLog::ConnectedStarting

  .method /*060000C9*/ public hidebysig static 
          void  ConnectedEnding(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger) cil managed
  // SIG: 00 01 01 12 81 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionHandlerLog::ConnectedEnding

} // end of class Microsoft.AspNetCore.SignalR.HubConnectionHandlerLog

.class /*0200001C*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubConnectionStore
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00009E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000A0:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000056*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .class /*02000077*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>/*1B000002*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000030*/
  {
    .custom /*0C0001A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0001A9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>/*1B000002*/
    .custom /*0C000100:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/
    .custom /*0C000104:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000030*/
    .custom /*0C000109:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000095*/ private initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000059*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>> _enumerator
    .method /*06000232*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubConnectionStore/*0200001C*/ hubConnectionList) cil managed
    // SIG: 20 01 01 12 70
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::.ctor

    .method /*06000233*/ public hidebysig newslot specialname virtual final 
            instance class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ 
            get_Current() cil managed
    // SIG: 20 00 12 60
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*06000234*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/::get_Current /*01000057::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method /*06000235*/ public hidebysig newslot virtual final 
            instance void  Dispose() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Dispose

    .method /*06000236*/ public hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .method /*06000237*/ public hidebysig newslot virtual final 
            instance void  Reset() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::Reset

    .property /*17000080*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/
            Current()
    {
      .get instance class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ Microsoft.AspNetCore.SignalR.HubConnectionStore/*0200001C*//Enumerator/*02000077*/::get_Current() /* 06000233 */
    } // end of property Enumerator::Current
    .property /*17000081*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.SignalR.HubConnectionStore/*0200001C*//Enumerator/*02000077*/::System.Collections.IEnumerator.get_Current() /* 06000234 */
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .method /*060000CA*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ 
          get_Item(string connectionId) cil managed
  // SIG: 20 01 12 60 0E
  {
    .param [0]/*080001F1*/ 
    .custom /*0C000287:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionStore::get_Item

  .method /*060000CB*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionStore::get_Count

  .method /*060000CC*/ public hidebysig instance void 
          Add(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 01 12 60
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionStore::Add

  .method /*060000CD*/ public hidebysig instance void 
          Remove(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 01 12 60
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionStore::Remove

  .method /*060000CE*/ public hidebysig instance valuetype Microsoft.AspNetCore.SignalR.HubConnectionStore/*0200001C*//Enumerator/*02000077*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 81 DC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionStore::GetEnumerator

  .method /*060000CF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionStore::.ctor

  .property /*17000037*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/
          Item(string)
  {
    .custom /*0C0000FC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ Microsoft.AspNetCore.SignalR.HubConnectionStore/*0200001C*/::get_Item(string) /* 060000CA */
  } // end of property HubConnectionStore::Item
  .property /*17000038*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.AspNetCore.SignalR.HubConnectionStore/*0200001C*/::get_Count() /* 060000CB */
  } // end of property HubConnectionStore::Count
} // end of class Microsoft.AspNetCore.SignalR.HubConnectionStore

.class /*0200001D*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubInvocationContext
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000D0*/ assembly hidebysig specialname 
          instance class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ 
          get_ObjectMethodExecutor() cil managed
  // SIG: 20 00 12 24
  {
    .custom /*0C000224:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::get_ObjectMethodExecutor

  .method /*060000D1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ context,
                               class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ serviceProvider,
                               class Microsoft.AspNetCore.SignalR.Hub/*02000015*/ hub,
                               class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ hubMethod,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<object> hubMethodArguments) cil managed
  // SIG: 20 05 01 12 58 12 81 69 12 54 12 71 15 12 80 B5 01 1C
  {
    .param [5]/*080001F9*/ 
    .custom /*0C000288:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::.ctor

  .method /*060000D2*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ objectMethodExecutor,
                               class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ context,
                               class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ serviceProvider,
                               class Microsoft.AspNetCore.SignalR.Hub/*02000015*/ hub,
                               object[] hubMethodArguments) cil managed
  // SIG: 20 05 01 12 24 12 58 12 81 69 12 54 1D 1C
  {
    .param [5]/*080001FE*/ 
    .custom /*0C000289:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::.ctor

  .method /*060000D3*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ 
          get_Context() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C000226:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::get_Context

  .method /*060000D4*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.Hub/*02000015*/ 
          get_Hub() cil managed
  // SIG: 20 00 12 54
  {
    .custom /*0C000228:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::get_Hub

  .method /*060000D5*/ public hidebysig specialname 
          instance string  get_HubMethodName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::get_HubMethodName

  .method /*060000D6*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<object> 
          get_HubMethodArguments() cil managed
  // SIG: 20 00 15 12 80 B5 01 1C
  {
    .custom /*0C000229:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*080001FF*/ 
    .custom /*0C00028A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::get_HubMethodArguments

  .method /*060000D7*/ public hidebysig specialname 
          instance class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ 
          get_ServiceProvider() cil managed
  // SIG: 20 00 12 81 69
  {
    .custom /*0C00022A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::get_ServiceProvider

  .method /*060000D8*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ 
          get_HubMethod() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00022B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubInvocationContext::get_HubMethod

  .property /*17000039*/ instance callconv(8) class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/
          ObjectMethodExecutor()
  {
    .get instance class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/::get_ObjectMethodExecutor() /* 060000D0 */
  } // end of property HubInvocationContext::ObjectMethodExecutor
  .property /*1700003A*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/
          Context()
  {
    .get instance class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/::get_Context() /* 060000D3 */
  } // end of property HubInvocationContext::Context
  .property /*1700003B*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.Hub/*02000015*/
          Hub()
  {
    .get instance class Microsoft.AspNetCore.SignalR.Hub/*02000015*/ Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/::get_Hub() /* 060000D4 */
  } // end of property HubInvocationContext::Hub
  .property /*1700003C*/ instance callconv(8) string
          HubMethodName()
  {
    .get instance string Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/::get_HubMethodName() /* 060000D5 */
  } // end of property HubInvocationContext::HubMethodName
  .property /*1700003D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<object>
          HubMethodArguments()
  {
    .custom /*0C000113:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<object> Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/::get_HubMethodArguments() /* 060000D6 */
  } // end of property HubInvocationContext::HubMethodArguments
  .property /*1700003E*/ instance callconv(8) class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/
          ServiceProvider()
  {
    .get instance class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/::get_ServiceProvider() /* 060000D7 */
  } // end of property HubInvocationContext::ServiceProvider
  .property /*1700003F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/
          HubMethod()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/::get_HubMethod() /* 060000D8 */
  } // end of property HubInvocationContext::HubMethod
} // end of class Microsoft.AspNetCore.SignalR.HubInvocationContext

.class /*0200001E*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.HubLifetimeContext
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000D9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ context,
                               class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ serviceProvider,
                               class Microsoft.AspNetCore.SignalR.Hub/*02000015*/ hub) cil managed
  // SIG: 20 03 01 12 58 12 81 69 12 54
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubLifetimeContext::.ctor

  .method /*060000DA*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ 
          get_Context() cil managed
  // SIG: 20 00 12 58
  {
    .custom /*0C00022C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubLifetimeContext::get_Context

  .method /*060000DB*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.Hub/*02000015*/ 
          get_Hub() cil managed
  // SIG: 20 00 12 54
  {
    .custom /*0C00022D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubLifetimeContext::get_Hub

  .method /*060000DC*/ public hidebysig specialname 
          instance class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ 
          get_ServiceProvider() cil managed
  // SIG: 20 00 12 81 69
  {
    .custom /*0C00022E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubLifetimeContext::get_ServiceProvider

  .property /*17000040*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/
          Context()
  {
    .get instance class Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/ Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/::get_Context() /* 060000DA */
  } // end of property HubLifetimeContext::Context
  .property /*17000041*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.Hub/*02000015*/
          Hub()
  {
    .get instance class Microsoft.AspNetCore.SignalR.Hub/*02000015*/ Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/::get_Hub() /* 060000DB */
  } // end of property HubLifetimeContext::Hub
  .property /*17000042*/ instance callconv(8) class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/
          ServiceProvider()
  {
    .get instance class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/::get_ServiceProvider() /* 060000DC */
  } // end of property HubLifetimeContext::ServiceProvider
} // end of class Microsoft.AspNetCore.SignalR.HubLifetimeContext

.class /*0200001F*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubLifetimeManager`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000AA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type THub /*2A000003*/ 
    .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000DD*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnConnectedAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 12 80 A9 12 60
  {
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::OnConnectedAsync

  .method /*060000DE*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnDisconnectedAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 12 80 A9 12 60
  {
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::OnDisconnectedAsync

  .method /*060000DF*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAllAsync(string methodName,
                       object[] args,
                       [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .param [2]/*08000206*/ 
    .custom /*0C00028B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000207*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendAllAsync

  .method /*060000E0*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendAllExceptAsync(string methodName,
                             object[] args,
                             class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds,
                             [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 0E 1D 1C 15 12 80 B5 01 0E 11 80 AD
  {
    .param [2]/*08000209*/ 
    .custom /*0C00028C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*0800020B*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendAllExceptAsync

  .method /*060000E1*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendConnectionAsync(string connectionId,
                              string methodName,
                              object[] args,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*0800020E*/ 
    .custom /*0C00028D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*0800020F*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendConnectionAsync

  .method /*060000E2*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendConnectionsAsync(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds,
                               string methodName,
                               object[] args,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 15 12 80 B5 01 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*08000212*/ 
    .custom /*0C00028E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000213*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendConnectionsAsync

  .method /*060000E3*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendGroupAsync(string groupName,
                         string methodName,
                         object[] args,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*08000216*/ 
    .custom /*0C000290:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000217*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendGroupAsync

  .method /*060000E4*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendGroupsAsync(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames,
                          string methodName,
                          object[] args,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 15 12 80 B5 01 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*0800021A*/ 
    .custom /*0C000291:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*0800021B*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendGroupsAsync

  .method /*060000E5*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendGroupExceptAsync(string groupName,
                               string methodName,
                               object[] args,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 05 12 80 A9 0E 0E 1D 1C 15 12 80 B5 01 0E 11 80 AD
  {
    .param [3]/*0800021E*/ 
    .custom /*0C000294:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [5]/*08000220*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendGroupExceptAsync

  .method /*060000E6*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendUserAsync(string userId,
                        string methodName,
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*08000223*/ 
    .custom /*0C000298:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000224*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendUserAsync

  .method /*060000E7*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendUsersAsync(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds,
                         string methodName,
                         object[] args,
                         [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 A9 15 12 80 B5 01 0E 0E 1D 1C 11 80 AD
  {
    .param [3]/*08000227*/ 
    .custom /*0C00029B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [4]/*08000228*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::SendUsersAsync

  .method /*060000E8*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          AddToGroupAsync(string connectionId,
                          string groupName,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 0E 11 80 AD
  {
    .param [3]/*0800022B*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::AddToGroupAsync

  .method /*060000E9*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          RemoveFromGroupAsync(string connectionId,
                               string groupName,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 0E 11 80 AD
  {
    .param [3]/*0800022E*/  = nullref
    // Method begins at RVA 0x0
  } // end of method HubLifetimeManager`1::RemoveFromGroupAsync

  .method /*060000EA*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubLifetimeManager`1::.ctor

} // end of class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1

.class /*02000020*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubMetadata
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000AD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000EB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/ hubType) cil managed
  // SIG: 20 01 01 12 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMetadata::.ctor

  .method /*060000EC*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          get_HubType() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C00022F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMetadata::get_HubType

  .property /*17000043*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
          HubType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.AspNetCore.SignalR.HubMetadata/*02000020*/::get_HubType() /* 060000EC */
  } // end of property HubMetadata::HubType
} // end of class Microsoft.AspNetCore.SignalR.HubMetadata

.class /*02000021*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubMethodNameAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C0000AF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B1:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 40 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..@.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 01 )                               // rited.
  .method /*060000ED*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000230:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodNameAttribute::get_Name

  .method /*060000EE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string name) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodNameAttribute::.ctor

  .property /*17000044*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.AspNetCore.SignalR.HubMethodNameAttribute/*02000021*/::get_Name() /* 060000ED */
  } // end of property HubMethodNameAttribute::Name
} // end of class Microsoft.AspNetCore.SignalR.HubMethodNameAttribute

.class /*02000022*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*060000EF*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> 
          get_HandshakeTimeout() cil managed
  // SIG: 20 00 15 11 81 3D 01 11 81 01
  {
    .custom /*0C000231:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_HandshakeTimeout

  .method /*060000F0*/ public hidebysig specialname 
          instance void  set_HandshakeTimeout(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 3D 01 11 81 01
  {
    .custom /*0C000232:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_HandshakeTimeout

  .method /*060000F1*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> 
          get_KeepAliveInterval() cil managed
  // SIG: 20 00 15 11 81 3D 01 11 81 01
  {
    .custom /*0C000233:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_KeepAliveInterval

  .method /*060000F2*/ public hidebysig specialname 
          instance void  set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 3D 01 11 81 01
  {
    .custom /*0C000234:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_KeepAliveInterval

  .method /*060000F3*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> 
          get_ClientTimeoutInterval() cil managed
  // SIG: 20 00 15 11 81 3D 01 11 81 01
  {
    .custom /*0C000235:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_ClientTimeoutInterval

  .method /*060000F4*/ public hidebysig specialname 
          instance void  set_ClientTimeoutInterval(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> 'value') cil managed
  // SIG: 20 01 01 15 11 81 3D 01 11 81 01
  {
    .custom /*0C000236:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_ClientTimeoutInterval

  .method /*060000F5*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<string> 
          get_SupportedProtocols() cil managed
  // SIG: 20 00 15 12 81 6D 01 0E
  {
    .custom /*0C000237:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000234*/ 
    .custom /*0C0002A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_SupportedProtocols

  .method /*060000F6*/ public hidebysig specialname 
          instance void  set_SupportedProtocols(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<string> 'value') cil managed
  // SIG: 20 01 01 15 12 81 6D 01 0E
  {
    .custom /*0C000238:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*08000235*/ 
    .custom /*0C0002A2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_SupportedProtocols

  .method /*060000F7*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64> 
          get_MaximumReceiveMessageSize() cil managed
  // SIG: 20 00 15 11 81 3D 01 0A
  {
    .custom /*0C000239:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_MaximumReceiveMessageSize

  .method /*060000F8*/ public hidebysig specialname 
          instance void  set_MaximumReceiveMessageSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 81 3D 01 0A
  {
    .custom /*0C00023A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_MaximumReceiveMessageSize

  .method /*060000F9*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<bool> 
          get_EnableDetailedErrors() cil managed
  // SIG: 20 00 15 11 81 3D 01 02
  {
    .custom /*0C00023B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_EnableDetailedErrors

  .method /*060000FA*/ public hidebysig specialname 
          instance void  set_EnableDetailedErrors(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 81 3D 01 02
  {
    .custom /*0C00023C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_EnableDetailedErrors

  .method /*060000FB*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int32> 
          get_StreamBufferCapacity() cil managed
  // SIG: 20 00 15 11 81 3D 01 08
  {
    .custom /*0C00023D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_StreamBufferCapacity

  .method /*060000FC*/ public hidebysig specialname 
          instance void  set_StreamBufferCapacity(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int32> 'value') cil managed
  // SIG: 20 01 01 15 11 81 3D 01 08
  {
    .custom /*0C00023E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_StreamBufferCapacity

  .method /*060000FD*/ assembly hidebysig specialname 
          instance class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/> 
          get_HubFilters() cil managed
  // SIG: 20 00 15 12 81 71 01 12 80 C8
  {
    .custom /*0C00023F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000239*/ 
    .custom /*0C0002A3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_HubFilters

  .method /*060000FE*/ assembly hidebysig specialname 
          instance void  set_HubFilters(class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 71 01 12 80 C8
  {
    .custom /*0C000240:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [1]/*0800023A*/ 
    .custom /*0C0002A4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_HubFilters

  .method /*060000FF*/ public hidebysig specialname 
          instance int32  get_MaximumParallelInvocationsPerClient() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::get_MaximumParallelInvocationsPerClient

  .method /*06000100*/ public hidebysig specialname 
          instance void  set_MaximumParallelInvocationsPerClient(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::set_MaximumParallelInvocationsPerClient

  .method /*06000101*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions::.ctor

  .property /*17000045*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/>
          HandshakeTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_HandshakeTimeout() /* 060000EF */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_HandshakeTimeout(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/>) /* 060000F0 */
  } // end of property HubOptions::HandshakeTimeout
  .property /*17000046*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/>
          KeepAliveInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_KeepAliveInterval() /* 060000F1 */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/>) /* 060000F2 */
  } // end of property HubOptions::KeepAliveInterval
  .property /*17000047*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/>
          ClientTimeoutInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/> Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_ClientTimeoutInterval() /* 060000F3 */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_ClientTimeoutInterval(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/>) /* 060000F4 */
  } // end of property HubOptions::ClientTimeoutInterval
  .property /*17000048*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<string>
          SupportedProtocols()
  {
    .custom /*0C000135:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<string> Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_SupportedProtocols() /* 060000F5 */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_SupportedProtocols(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<string>) /* 060000F6 */
  } // end of property HubOptions::SupportedProtocols
  .property /*17000049*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64>
          MaximumReceiveMessageSize()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64> Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_MaximumReceiveMessageSize() /* 060000F7 */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_MaximumReceiveMessageSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int64>) /* 060000F8 */
  } // end of property HubOptions::MaximumReceiveMessageSize
  .property /*1700004A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<bool>
          EnableDetailedErrors()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<bool> Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_EnableDetailedErrors() /* 060000F9 */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_EnableDetailedErrors(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<bool>) /* 060000FA */
  } // end of property HubOptions::EnableDetailedErrors
  .property /*1700004B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int32>
          StreamBufferCapacity()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int32> Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_StreamBufferCapacity() /* 060000FB */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_StreamBufferCapacity(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100004F*/<int32>) /* 060000FC */
  } // end of property HubOptions::StreamBufferCapacity
  .property /*1700004C*/ instance callconv(8) class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/>
          HubFilters()
  {
    .custom /*0C000145:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/> Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_HubFilters() /* 060000FD */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_HubFilters(class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/>) /* 060000FE */
  } // end of property HubOptions::HubFilters
  .property /*1700004D*/ instance callconv(8) int32
          MaximumParallelInvocationsPerClient()
  {
    .get instance int32 Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::get_MaximumParallelInvocationsPerClient() /* 060000FF */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/::set_MaximumParallelInvocationsPerClient(int32) /* 06000100 */
  } // end of property HubOptions::MaximumParallelInvocationsPerClient
} // end of class Microsoft.AspNetCore.SignalR.HubOptions

.class /*02000023*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.HubOptionsExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000B5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B7:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000102*/ public hidebysig static 
          void  AddFilter(class Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/ options,
                          class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/ hubFilter) cil managed
  // SIG: 00 02 01 12 80 88 12 80 C8
  {
    .custom /*0C000241:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsExtensions::AddFilter

  .method /*06000103*/ public hidebysig static 
          void  AddFilter<(Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/) TFilter>(class Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/ options) cil managed
  // SIG: 10 01 01 01 12 80 88
  {
    .custom /*0C000242:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TFilter /*2A00006E*/ 
      .custom /*0C00018C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
      .custom /*0C00018D:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100005E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100005D*/) /* 0A00001C */ = ( 01 00 03 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsExtensions::AddFilter

  .method /*06000104*/ public hidebysig static 
          void  AddFilter(class Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/ options,
                          class [System.Runtime/*23000001*/]System.Type/*01000018*/ filterType) cil managed
  // SIG: 00 02 01 12 80 88 12 61
  {
    .custom /*0C000243:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000240*/ 
    .custom /*0C0002A5:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100005E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100005D*/) /* 0A00001C */ = ( 01 00 03 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsExtensions::AddFilter

} // end of class Microsoft.AspNetCore.SignalR.HubOptionsExtensions

.class /*02000024*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubOptionsSetup
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IConfigureOptions`1/*0100005F*/<class Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/>/*1B000003*/
{
  .custom /*0C0000B9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IConfigureOptions`1/*0100005F*/<class Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/>/*1B000003*/
  .custom /*0C000027:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .class /*02000078*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001AB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000096*/ public static initonly class Microsoft.AspNetCore.SignalR.HubOptionsSetup/*02000024*//'<>c'/*02000078*/ '<>9'
    .field /*04000097*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.CustomAttributeData/*01000060*/,bool> '<>9__9_0'
    .method /*06000238*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000239*/ assembly hidebysig 
            instance bool  '<.ctor>b__9_0'(class [System.Runtime/*23000001*/]System.Reflection.CustomAttributeData/*01000060*/ a) cil managed
    // SIG: 20 01 02 12 81 81
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__9_0'

  } // end of class '<>c'

  .field /*04000016*/ static assembly literal int32 DefaultMaximumMessageSize = int32(0x00008000)
  .field /*04000017*/ static assembly literal int32 DefaultStreamBufferCapacity = int32(0x0000000A)
  .method /*06000105*/ assembly hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ 
          get_DefaultHandshakeTimeout() cil managed
  // SIG: 00 00 11 81 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsSetup::get_DefaultHandshakeTimeout

  .method /*06000106*/ assembly hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ 
          get_DefaultKeepAliveInterval() cil managed
  // SIG: 00 00 11 81 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsSetup::get_DefaultKeepAliveInterval

  .method /*06000107*/ assembly hidebysig specialname static 
          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ 
          get_DefaultClientTimeoutInterval() cil managed
  // SIG: 00 00 11 81 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsSetup::get_DefaultClientTimeoutInterval

  .method /*06000108*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/> protocols) cil managed
  // SIG: 20 01 01 15 12 80 D5 01 12 80 E9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsSetup::.ctor

  .method /*06000109*/ public hidebysig newslot virtual final 
          instance void  Configure(class Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/ options) cil managed
  // SIG: 20 01 01 12 80 88
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsSetup::Configure

  .property /*1700004E*/ callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/
          DefaultHandshakeTimeout()
  {
    .get valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ Microsoft.AspNetCore.SignalR.HubOptionsSetup/*02000024*/::get_DefaultHandshakeTimeout() /* 06000105 */
  } // end of property HubOptionsSetup::DefaultHandshakeTimeout
  .property /*1700004F*/ callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/
          DefaultKeepAliveInterval()
  {
    .get valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ Microsoft.AspNetCore.SignalR.HubOptionsSetup/*02000024*/::get_DefaultKeepAliveInterval() /* 06000106 */
  } // end of property HubOptionsSetup::DefaultKeepAliveInterval
  .property /*17000050*/ callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/
          DefaultClientTimeoutInterval()
  {
    .get valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000040*/ Microsoft.AspNetCore.SignalR.HubOptionsSetup/*02000024*/::get_DefaultClientTimeoutInterval() /* 06000107 */
  } // end of property HubOptionsSetup::DefaultClientTimeoutInterval
} // end of class Microsoft.AspNetCore.SignalR.HubOptionsSetup

.class /*02000025*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubOptionsSetup`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IConfigureOptions`1/*0100005F*/<class Microsoft.AspNetCore.SignalR.HubOptions`1/*02000026*/<!THub>>/*1B000004*/
{
  .custom /*0C0000BD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IConfigureOptions`1/*0100005F*/<class Microsoft.AspNetCore.SignalR.HubOptions`1/*02000026*/<!THub>>/*1B000004*/
  .custom /*0C00002D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .param type THub /*2A000004*/ 
    .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600010A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*2300000D*/]Microsoft.Extensions.Options.IOptions`1/*01000051*/<class Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/> options) cil managed
  // SIG: 20 01 01 15 12 81 45 01 12 80 88
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsSetup`1::.ctor

  .method /*0600010B*/ public hidebysig newslot virtual final 
          instance void  Configure(class Microsoft.AspNetCore.SignalR.HubOptions`1/*02000026*/<!THub> options) cil managed
  // SIG: 20 01 01 15 12 80 98 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptionsSetup`1::Configure

} // end of class Microsoft.AspNetCore.SignalR.HubOptionsSetup`1

.class /*02000026*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.HubOptions`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends Microsoft.AspNetCore.SignalR.HubOptions/*02000022*/
{
  .param constraint   THub/*2A000005*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*0600010C*/ assembly hidebysig specialname 
          instance bool  get_UserHasSetValues() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000244:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions`1::get_UserHasSetValues

  .method /*0600010D*/ assembly hidebysig specialname 
          instance void  set_UserHasSetValues(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000245:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions`1::set_UserHasSetValues

  .method /*0600010E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubOptions`1::.ctor

  .property /*17000051*/ instance callconv(8) bool
          UserHasSetValues()
  {
    .get instance bool Microsoft.AspNetCore.SignalR.HubOptions`1/*02000026*/::get_UserHasSetValues() /* 0600010C */
    .set instance void Microsoft.AspNetCore.SignalR.HubOptions`1/*02000026*/::set_UserHasSetValues(bool) /* 0600010D */
  } // end of property HubOptions`1::UserHasSetValues
} // end of class Microsoft.AspNetCore.SignalR.HubOptions`1

.class /*02000027*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Hub`1<class T>
       extends Microsoft.AspNetCore.SignalR.Hub/*02000015*/
{
  .custom /*0C0000C4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000C5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600010F*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<!T> 
          get_Clients() cil managed
  // SIG: 20 00 15 12 80 B0 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub`1::get_Clients

  .method /*06000110*/ public hidebysig specialname 
          instance void  set_Clients(class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<!T> 'value') cil managed
  // SIG: 20 01 01 15 12 80 B0 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub`1::set_Clients

  .method /*06000111*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Hub`1::.ctor

  .property /*17000052*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<!T>
          Clients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<!T> Microsoft.AspNetCore.SignalR.Hub`1/*02000027*/::get_Clients() /* 0600010F */
    .set instance void Microsoft.AspNetCore.SignalR.Hub`1/*02000027*/::set_Clients(class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<!T>) /* 06000110 */
  } // end of property Hub`1::Clients
} // end of class Microsoft.AspNetCore.SignalR.Hub`1

.class /*02000028*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IClientProxy
{
  .custom /*0C0000C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000112*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .param [2]/*08000248*/ 
    .custom /*0C0002A6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000249*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IClientProxy::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.IClientProxy

.class /*02000029*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IGroupManager
{
  .custom /*0C0000CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000113*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          AddToGroupAsync(string connectionId,
                          string groupName,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 0E 11 80 AD
  {
    .param [3]/*0800024C*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IGroupManager::AddToGroupAsync

  .method /*06000114*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          RemoveFromGroupAsync(string connectionId,
                               string groupName,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 0E 11 80 AD
  {
    .param [3]/*0800024F*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IGroupManager::RemoveFromGroupAsync

} // end of class Microsoft.AspNetCore.SignalR.IGroupManager

.class /*0200002A*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubActivator`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
{
  .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .param type THub /*2A000007*/ 
    .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000115*/ public hidebysig newslot abstract virtual 
          instance !THub  Create() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x0
  } // end of method IHubActivator`1::Create

  .method /*06000116*/ public hidebysig newslot abstract virtual 
          instance void  Release(!THub hub) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x0
  } // end of method IHubActivator`1::Release

} // end of class Microsoft.AspNetCore.SignalR.IHubActivator`1

.class /*0200002B*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubCallerClients
       implements class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000005*/,
                  class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000006*/
{
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000005*/
  .custom /*0C000031:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000006*/
  .custom /*0C000035:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
} // end of class Microsoft.AspNetCore.SignalR.IHubCallerClients

.class /*0200002C*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubCallerClients`1<T>
       implements class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T>/*1B000007*/
{
  .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T>/*1B000007*/
  .custom /*0C00003A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000008*/ 
    .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000117*/ public hidebysig newslot specialname abstract virtual 
          instance !T  get_Caller() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x0
  } // end of method IHubCallerClients`1::get_Caller

  .method /*06000118*/ public hidebysig newslot specialname abstract virtual 
          instance !T  get_Others() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x0
  } // end of method IHubCallerClients`1::get_Others

  .method /*06000119*/ public hidebysig newslot abstract virtual 
          instance !T  OthersInGroup(string groupName) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubCallerClients`1::OthersInGroup

  .property /*17000053*/ instance callconv(8) !T
          Caller()
  {
    .get instance !T Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/::get_Caller() /* 06000117 */
  } // end of property IHubCallerClients`1::Caller
  .property /*17000054*/ instance callconv(8) !T
          Others()
  {
    .get instance !T Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/::get_Others() /* 06000118 */
  } // end of property IHubCallerClients`1::Others
} // end of class Microsoft.AspNetCore.SignalR.IHubCallerClients`1

.class /*0200002D*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubClients
       implements class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000006*/
{
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000006*/
  .custom /*0C000040:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
} // end of class Microsoft.AspNetCore.SignalR.IHubClients

.class /*0200002E*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubClients`1<T>
{
  .custom /*0C0000DF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .param type T /*2A000009*/ 
    .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*0600011A*/ public hidebysig newslot specialname abstract virtual 
          instance !T  get_All() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::get_All

  .method /*0600011B*/ public hidebysig newslot abstract virtual 
          instance !T  AllExcept(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 01 13 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::AllExcept

  .method /*0600011C*/ public hidebysig newslot abstract virtual 
          instance !T  Client(string connectionId) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::Client

  .method /*0600011D*/ public hidebysig newslot abstract virtual 
          instance !T  Clients(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds) cil managed
  // SIG: 20 01 13 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::Clients

  .method /*0600011E*/ public hidebysig newslot abstract virtual 
          instance !T  Group(string groupName) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::Group

  .method /*0600011F*/ public hidebysig newslot abstract virtual 
          instance !T  Groups(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames) cil managed
  // SIG: 20 01 13 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::Groups

  .method /*06000120*/ public hidebysig newslot abstract virtual 
          instance !T  GroupExcept(string groupName,
                                   class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 02 13 00 0E 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::GroupExcept

  .method /*06000121*/ public hidebysig newslot abstract virtual 
          instance !T  User(string userId) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::User

  .method /*06000122*/ public hidebysig newslot abstract virtual 
          instance !T  Users(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds) cil managed
  // SIG: 20 01 13 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IHubClients`1::Users

  .property /*17000055*/ instance callconv(8) !T
          All()
  {
    .get instance !T Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/::get_All() /* 0600011A */
  } // end of property IHubClients`1::All
} // end of class Microsoft.AspNetCore.SignalR.IHubClients`1

.class /*0200002F*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubContext
{
  .custom /*0C0000E2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000123*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/ 
          get_Clients() cil managed
  // SIG: 20 00 12 80 B4
  {
    // Method begins at RVA 0x0
  } // end of method IHubContext::get_Clients

  .method /*06000124*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ 
          get_Groups() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x0
  } // end of method IHubContext::get_Groups

  .property /*17000056*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/
          Clients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/ Microsoft.AspNetCore.SignalR.IHubContext/*0200002F*/::get_Clients() /* 06000123 */
  } // end of property IHubContext::Clients
  .property /*17000057*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/
          Groups()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ Microsoft.AspNetCore.SignalR.IHubContext/*0200002F*/::get_Groups() /* 06000124 */
  } // end of property IHubContext::Groups
} // end of class Microsoft.AspNetCore.SignalR.IHubContext

.class /*02000030*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubContext`1<+ (Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
{
  .custom /*0C0000E5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .param type THub /*2A00000A*/ 
    .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000125*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/ 
          get_Clients() cil managed
  // SIG: 20 00 12 80 B4
  {
    // Method begins at RVA 0x0
  } // end of method IHubContext`1::get_Clients

  .method /*06000126*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ 
          get_Groups() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x0
  } // end of method IHubContext`1::get_Groups

  .property /*17000058*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/
          Clients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/ Microsoft.AspNetCore.SignalR.IHubContext`1/*02000030*/::get_Clients() /* 06000125 */
  } // end of property IHubContext`1::Clients
  .property /*17000059*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/
          Groups()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ Microsoft.AspNetCore.SignalR.IHubContext`1/*02000030*/::get_Groups() /* 06000126 */
  } // end of property IHubContext`1::Groups
} // end of class Microsoft.AspNetCore.SignalR.IHubContext`1

.class /*02000031*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubContext`2<(class Microsoft.AspNetCore.SignalR.Hub`1/*02000027*/<!T>/*1B000008*/) THub,class T>
{
  .custom /*0C0000E8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .param type THub /*2A00000B*/ 
    .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000127*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T> 
          get_Clients() cil managed
  // SIG: 20 00 15 12 80 B8 01 13 01
  {
    // Method begins at RVA 0x0
  } // end of method IHubContext`2::get_Clients

  .method /*06000128*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ 
          get_Groups() cil managed
  // SIG: 20 00 12 80 A4
  {
    // Method begins at RVA 0x0
  } // end of method IHubContext`2::get_Groups

  .property /*1700005A*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T>
          Clients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T> Microsoft.AspNetCore.SignalR.IHubContext`2/*02000031*/::get_Clients() /* 06000127 */
  } // end of property IHubContext`2::Clients
  .property /*1700005B*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/
          Groups()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ Microsoft.AspNetCore.SignalR.IHubContext`2/*02000031*/::get_Groups() /* 06000128 */
  } // end of property IHubContext`2::Groups
} // end of class Microsoft.AspNetCore.SignalR.IHubContext`2

.class /*02000032*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubFilter
{
  .custom /*0C0000EC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000129*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<object> 
          InvokeMethodAsync(class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/ invocationContext,
                            class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<object>> next) cil managed
  // SIG: 20 02 15 11 81 1D 01 1C 12 74 15 12 75 02 12 74 15 11 81 1D 01 1C
  {
    .param [0]/*0800025B*/ 
    .custom /*0C0002AC:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [2]/*0800025D*/ 
    .custom /*0C0002AE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IHubFilter::InvokeMethodAsync

  .method /*0600012A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnConnectedAsync(class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> next) cil managed
  // SIG: 20 02 12 80 A9 12 78 15 12 75 02 12 78 12 80 A9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IHubFilter::OnConnectedAsync

  .method /*0600012B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnDisconnectedAsync(class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context,
                              class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception,
                              class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Exception/*01000031*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> next) cil managed
  // SIG: 20 03 12 80 A9 12 78 12 80 C5 15 12 80 B9 03 12 78 12 80 C5 12 80 A9
  {
    .param [2]/*08000261*/ 
    .custom /*0C0002B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000262*/ 
    .custom /*0C0002B3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IHubFilter::OnDisconnectedAsync

} // end of class Microsoft.AspNetCore.SignalR.IHubFilter

.class /*02000033*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IHubProtocolResolver
{
  .custom /*0C0000F0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600012C*/ public hidebysig newslot specialname abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/> 
          get_AllProtocols() cil managed
  // SIG: 20 00 15 12 80 B5 01 12 80 E9
  {
    // Method begins at RVA 0x0
  } // end of method IHubProtocolResolver::get_AllProtocols

  .method /*0600012D*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/ 
          GetProtocol(string protocolName,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> supportedProtocols) cil managed
  // SIG: 20 02 12 80 E9 0E 15 12 80 B5 01 0E
  {
    .param [0]/*08000263*/ 
    .custom /*0C0002B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000265*/ 
    .custom /*0C0002B5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IHubProtocolResolver::GetProtocol

  .property /*1700005C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/>
          AllProtocols()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/> Microsoft.AspNetCore.SignalR.IHubProtocolResolver/*02000033*/::get_AllProtocols() /* 0600012C */
  } // end of property IHubProtocolResolver::AllProtocols
} // end of class Microsoft.AspNetCore.SignalR.IHubProtocolResolver

.class /*02000034*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.ISignalRServerBuilder
       implements [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.ISignalRBuilder/*01000061*/
{
} // end of class Microsoft.AspNetCore.SignalR.ISignalRServerBuilder

.class /*02000035*/ interface public abstract auto ansi Microsoft.AspNetCore.SignalR.IUserIdProvider
{
  .custom /*0C0000F4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600012E*/ public hidebysig newslot abstract virtual 
          instance string  GetUserId(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 0E 12 60
  {
    .param [0]/*08000266*/ 
    .custom /*0C0002B7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IUserIdProvider::GetUserId

} // end of class Microsoft.AspNetCore.SignalR.IUserIdProvider

.class /*02000036*/ public auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.SerializedHubMessage
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000F6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000F7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600012F*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/ 
          get_Message() cil managed
  // SIG: 20 00 12 80 F9
  {
    .custom /*0C000246:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000247:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SerializedHubMessage::get_Message

  .method /*06000130*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<valuetype Microsoft.AspNetCore.SignalR.SerializedMessage/*02000037*/> messages) cil managed
  // SIG: 20 01 01 15 12 80 B5 01 11 80 DC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SerializedHubMessage::.ctor

  .method /*06000131*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/ message) cil managed
  // SIG: 20 01 01 12 80 F9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SerializedHubMessage::.ctor

  .method /*06000132*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000062*/<uint8> 
          GetSerializedMessage(class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/ protocol) cil managed
  // SIG: 20 01 15 11 81 89 01 05 12 80 E9
  {
    .custom /*0C000248:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [1]/*0800026A*/ 
    .custom /*0C0002BA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SerializedHubMessage::GetSerializedMessage

  .method /*06000133*/ assembly hidebysig 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<valuetype Microsoft.AspNetCore.SignalR.SerializedMessage/*02000037*/> 
          GetAllSerializations() cil managed
  // SIG: 20 00 15 12 80 B5 01 11 80 DC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SerializedHubMessage::GetAllSerializations

  .property /*1700005D*/ instance callconv(8) class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/
          Message()
  {
    .custom /*0C000161:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/ Microsoft.AspNetCore.SignalR.SerializedHubMessage/*02000036*/::get_Message() /* 0600012F */
  } // end of property SerializedHubMessage::Message
} // end of class Microsoft.AspNetCore.SignalR.SerializedHubMessage

.class /*02000037*/ public sequential ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.SerializedMessage
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
{
  .custom /*0C0000FA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000019*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .field /*04000018*/ private initonly string '<ProtocolName>k__BackingField'
  .custom /*0C000086:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .field /*04000019*/ private initonly valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000062*/<uint8> '<Serialized>k__BackingField'
  .custom /*0C00008D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .method /*06000134*/ public hidebysig specialname 
          instance string  get_ProtocolName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000249:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00024A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SerializedMessage::get_ProtocolName

  .method /*06000135*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000062*/<uint8> 
          get_Serialized() cil managed
  // SIG: 20 00 15 11 81 89 01 05
  {
    .custom /*0C00024B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SerializedMessage::get_Serialized

  .method /*06000136*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string protocolName,
                               valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000062*/<uint8> serialized) cil managed
  // SIG: 20 02 01 0E 15 11 81 89 01 05
  {
    .param [1]/*0800026B*/ 
    .custom /*0C0002BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SerializedMessage::.ctor

  .property /*1700005E*/ instance callconv(8) string
          ProtocolName()
  {
    .custom /*0C000165:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.AspNetCore.SignalR.SerializedMessage/*02000037*/::get_ProtocolName() /* 06000134 */
  } // end of property SerializedMessage::ProtocolName
  .property /*1700005F*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000062*/<uint8>
          Serialized()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000062*/<uint8> Microsoft.AspNetCore.SignalR.SerializedMessage/*02000037*/::get_Serialized() /* 06000135 */
  } // end of property SerializedMessage::Serialized
} // end of class Microsoft.AspNetCore.SignalR.SerializedMessage

.class /*02000038*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.SignalRConnectionBuilderExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C0000FE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000137*/ public hidebysig static 
          class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.IConnectionBuilder/*01000063*/ 
          UseHub<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>(class [Microsoft.AspNetCore.Connections.Abstractions/*23000007*/]Microsoft.AspNetCore.Connections.IConnectionBuilder/*01000063*/ connectionBuilder) cil managed
  // SIG: 10 01 01 12 81 8D 12 81 8D
  {
    .custom /*0C00024C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00024D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type THub /*2A00006F*/ 
      .custom /*0C000190:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
      .custom /*0C000191:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100005E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100005D*/) /* 0A00001C */ = ( 01 00 0B 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignalRConnectionBuilderExtensions::UseHub

} // end of class Microsoft.AspNetCore.SignalR.SignalRConnectionBuilderExtensions

.class /*02000039*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.StreamTracker
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000102:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000103:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000079*/ interface abstract auto ansi nested private IStreamConverter
  {
    .method /*0600023A*/ public hidebysig newslot abstract virtual 
            instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
            GetItemType() cil managed
    // SIG: 20 00 12 61
    {
      // Method begins at RVA 0x0
    } // end of method IStreamConverter::GetItemType

    .method /*0600023B*/ public hidebysig newslot abstract virtual 
            instance object  GetReaderAsObject(class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'type') cil managed
    // SIG: 20 01 1C 12 61
    {
      // Method begins at RVA 0x0
    } // end of method IStreamConverter::GetReaderAsObject

    .method /*0600023C*/ public hidebysig newslot abstract virtual 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            WriteToStream(object item) cil managed
    // SIG: 20 01 12 80 A9 1C
    {
      // Method begins at RVA 0x0
    } // end of method IStreamConverter::WriteToStream

    .method /*0600023D*/ public hidebysig newslot abstract virtual 
            instance void  TryComplete(class [System.Runtime/*23000001*/]System.Exception/*01000031*/ ex) cil managed
    // SIG: 20 01 01 12 80 C5
    {
      // Method begins at RVA 0x0
    } // end of method IStreamConverter::TryComplete

  } // end of class IStreamConverter

  .class /*0200007A*/ auto ansi nested private beforefieldinit ChannelConverter`1<T>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements Microsoft.AspNetCore.SignalR.StreamTracker/*02000039*//IStreamConverter/*02000079*/
  {
    .method /*0600023E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 streamBufferCapacity) cil managed
    // SIG: 20 01 01 08
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelConverter`1::.ctor

    .method /*0600023F*/ public hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
            GetItemType() cil managed
    // SIG: 20 00 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelConverter`1::GetItemType

    .method /*06000240*/ public hidebysig newslot virtual final 
            instance object  GetReaderAsObject(class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'type') cil managed
    // SIG: 20 01 1C 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelConverter`1::GetReaderAsObject

    .method /*06000241*/ public hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            WriteToStream(object o) cil managed
    // SIG: 20 01 12 80 A9 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelConverter`1::WriteToStream

    .method /*06000242*/ public hidebysig newslot virtual final 
            instance void  TryComplete(class [System.Runtime/*23000001*/]System.Exception/*01000031*/ ex) cil managed
    // SIG: 20 01 01 12 80 C5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelConverter`1::TryComplete

  } // end of class ChannelConverter`1

  .class /*0200007B*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001B2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000098*/ public static initonly class Microsoft.AspNetCore.SignalR.StreamTracker/*02000039*//'<>c'/*0200007B*/ '<>9'
    .method /*06000243*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000244*/ assembly hidebysig 
            instance bool  '<.cctor>b__13_0'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__13_0'

  } // end of class '<>c'

  .method /*06000138*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 streamBufferCapacity) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamTracker::.ctor

  .method /*06000139*/ public hidebysig instance object 
          AddStream(string streamId,
                    class [System.Runtime/*23000001*/]System.Type/*01000018*/ itemType,
                    class [System.Runtime/*23000001*/]System.Type/*01000018*/ targetType) cil managed
  // SIG: 20 03 1C 0E 12 61 12 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamTracker::AddStream

  .method /*0600013A*/ public hidebysig instance bool 
          TryProcessItem(class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.StreamItemMessage/*01000064*/ message,
                         [out] class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/& task) cil managed
  // SIG: 20 02 02 12 81 91 10 12 80 A9
  {
    .param [2]/*08000273*/ 
    .custom /*0C0002BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0002BD:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000065*/::.ctor(bool) /* 0A00001D */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamTracker::TryProcessItem

  .method /*0600013B*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          GetStreamItemType(string streamId) cil managed
  // SIG: 20 01 12 61 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamTracker::GetStreamItemType

  .method /*0600013C*/ public hidebysig instance bool 
          TryComplete(class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.CompletionMessage/*01000066*/ message) cil managed
  // SIG: 20 01 02 12 81 99
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamTracker::TryComplete

  .method /*0600013D*/ public hidebysig instance void 
          CompleteAll(class [System.Runtime/*23000001*/]System.Exception/*01000031*/ ex) cil managed
  // SIG: 20 01 01 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamTracker::CompleteAll

} // end of class Microsoft.AspNetCore.SignalR.StreamTracker

.class /*0200003A*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.ReflectionHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000106:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000107:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200007C*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001B6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000099*/ public static initonly class Microsoft.AspNetCore.SignalR.ReflectionHelper/*0200003A*//'<>c'/*0200007C*/ '<>9'
    .field /*0400009A*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/,bool> '<>9__1_0'
    .method /*06000245*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000246*/ assembly hidebysig 
            instance bool  '<IsIAsyncEnumerable>b__1_0'(class [System.Runtime/*23000001*/]System.Type/*01000018*/ t) cil managed
    // SIG: 20 01 02 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<IsIAsyncEnumerable>b__1_0'

  } // end of class '<>c'

  .method /*0600013E*/ public hidebysig static 
          bool  IsStreamingType(class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'type',
                                [opt] bool mustBeDirectType) cil managed
  // SIG: 00 02 02 12 61 02
  {
    .param [2]/*08000278*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReflectionHelper::IsStreamingType

  .method /*0600013F*/ public hidebysig static 
          bool  IsIAsyncEnumerable(class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'type') cil managed
  // SIG: 00 01 02 12 61
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReflectionHelper::IsIAsyncEnumerable

} // end of class Microsoft.AspNetCore.SignalR.ReflectionHelper

.class /*0200003B*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.Internal.AsyncDisposableExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00010B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000140*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003D*/ 
          DisposeAsync(class [System.Runtime/*23000001*/]System.IDisposable/*01000030*/ disposable) cil managed
  // SIG: 00 01 11 80 F5 12 80 C1
  {
    .custom /*0C00024E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00024F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncDisposableExtensions::DisposeAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.AsyncDisposableExtensions

.class /*0200003C*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.DefaultHubActivator`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class Microsoft.AspNetCore.SignalR.IHubActivator`1/*0200002A*/<!THub>/*1B000009*/
{
  .custom /*0C00010D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00010E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubActivator`1/*0200002A*/<!THub>/*1B000009*/
  .custom /*0C000048:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type THub /*2A00000D*/ 
    .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200007D*/ auto ansi serializable sealed nested private beforefieldinit '<>c'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001B8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A00003F*/ 
      .custom /*0C00011A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400009B*/ public static initonly class Microsoft.AspNetCore.SignalR.Internal.DefaultHubActivator`1/*0200003C*//'<>c'/*0200007D*/<!THub> '<>9'
    .method /*06000247*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000248*/ assembly hidebysig 
            instance class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.ObjectFactory/*01000067*/ 
            '<.cctor>b__6_0'() cil managed
    // SIG: 20 00 12 81 9D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__6_0'

  } // end of class '<>c'

  .method /*06000141*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ serviceProvider) cil managed
  // SIG: 20 01 01 12 81 69
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubActivator`1::.ctor

  .method /*06000142*/ public hidebysig newslot virtual 
          instance !THub  Create() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubActivator`1::Create

  .method /*06000143*/ public hidebysig newslot virtual 
          instance void  Release(!THub hub) cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubActivator`1::Release

} // end of class Microsoft.AspNetCore.SignalR.Internal.DefaultHubActivator`1

.class /*0200003D*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.DefaultHubCallerContext
       extends Microsoft.AspNetCore.SignalR.HubCallerContext/*02000016*/
{
  .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000112:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000144*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 01 12 60
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubCallerContext::.ctor

  .method /*06000145*/ public hidebysig specialname virtual 
          instance string  get_ConnectionId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubCallerContext::get_ConnectionId

  .method /*06000146*/ public hidebysig specialname virtual 
          instance string  get_UserIdentifier() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000250:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubCallerContext::get_UserIdentifier

  .method /*06000147*/ public hidebysig specialname virtual 
          instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/ 
          get_User() cil managed
  // SIG: 20 00 12 80 C9
  {
    .custom /*0C000251:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubCallerContext::get_User

  .method /*06000148*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object> 
          get_Items() cil managed
  // SIG: 20 00 15 12 80 CD 02 1C 1C
  {
    .param [0]/*0800027E*/ 
    .custom /*0C0002BF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubCallerContext::get_Items

  .method /*06000149*/ public hidebysig specialname virtual 
          instance class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/ 
          get_Features() cil managed
  // SIG: 20 00 12 80 D1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubCallerContext::get_Features

  .method /*0600014A*/ public hidebysig specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ 
          get_ConnectionAborted() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubCallerContext::get_ConnectionAborted

  .method /*0600014B*/ public hidebysig virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubCallerContext::Abort

  .property /*17000060*/ instance callconv(8) string
          ConnectionId()
  {
    .get instance string Microsoft.AspNetCore.SignalR.Internal.DefaultHubCallerContext/*0200003D*/::get_ConnectionId() /* 06000145 */
  } // end of property DefaultHubCallerContext::ConnectionId
  .property /*17000061*/ instance callconv(8) string
          UserIdentifier()
  {
    .custom /*0C00016F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.SignalR.Internal.DefaultHubCallerContext/*0200003D*/::get_UserIdentifier() /* 06000146 */
  } // end of property DefaultHubCallerContext::UserIdentifier
  .property /*17000062*/ instance callconv(8) class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/
          User()
  {
    .custom /*0C000173:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/ Microsoft.AspNetCore.SignalR.Internal.DefaultHubCallerContext/*0200003D*/::get_User() /* 06000147 */
  } // end of property DefaultHubCallerContext::User
  .property /*17000063*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object>
          Items()
  {
    .custom /*0C000174:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000033*/<object,object> Microsoft.AspNetCore.SignalR.Internal.DefaultHubCallerContext/*0200003D*/::get_Items() /* 06000148 */
  } // end of property DefaultHubCallerContext::Items
  .property /*17000064*/ instance callconv(8) class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/
          Features()
  {
    .get instance class [Microsoft.Extensions.Features/*23000006*/]Microsoft.AspNetCore.Http.Features.IFeatureCollection/*01000034*/ Microsoft.AspNetCore.SignalR.Internal.DefaultHubCallerContext/*0200003D*/::get_Features() /* 06000149 */
  } // end of property DefaultHubCallerContext::Features
  .property /*17000065*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/
          ConnectionAborted()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ Microsoft.AspNetCore.SignalR.Internal.DefaultHubCallerContext/*0200003D*/::get_ConnectionAborted() /* 0600014A */
  } // end of property DefaultHubCallerContext::ConnectionAborted
} // end of class Microsoft.AspNetCore.SignalR.Internal.DefaultHubCallerContext

.class /*0200003E*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends class Microsoft.AspNetCore.SignalR.Internal.HubDispatcher`1/*0200004A*/<!THub>/*1B00000A*/
{
  .custom /*0C000115:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000116:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type THub /*2A00000E*/ 
    .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200007E*/ auto ansi sealed nested private beforefieldinit '<<Invoke>g__ExecuteInvocation|16_0>d'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001BA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000041*/ 
      .custom /*0C00011E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400009C*/ public int32 '<>1__state'
    .field /*0400009D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*0400009E*/ public class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub> dispatcher
    .field /*0400009F*/ public class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ methodExecutor
    .field /*040000A0*/ public !THub hub
    .field /*040000A1*/ public object[] arguments
    .field /*040000A2*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*040000A3*/ public valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000068*/ scope
    .field /*040000A4*/ public class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/ hubMethodInvocationMessage
    .field /*040000A5*/ public bool isStreamCall
    .field /*040000A6*/ public class Microsoft.AspNetCore.SignalR.IHubActivator`1/*0200002A*/<!THub> hubActivator
    .field /*040000A7*/ private class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger`1/*0100002C*/<class Microsoft.AspNetCore.SignalR.Internal.HubDispatcher`1/*0200004A*/<!THub>> '<logger>5__2'
    .field /*040000A8*/ private bool '<enableDetailedErrors>5__3'
    .field /*040000A9*/ private object '<result>5__4'
    .field /*040000AA*/ private object '<>7__wrap4'
    .field /*040000AB*/ private int32 '<>7__wrap5'
    .field /*040000AC*/ private object '<>7__wrap6'
    .field /*040000AD*/ private int32 '<>7__wrap7'
    .field /*040000AE*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<object> '<>u__1'
    .field /*040000AF*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__2'
    .field /*040000B0*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__3'
    .method /*06000249*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<Invoke>g__ExecuteInvocation|16_0>d'::MoveNext

    .method /*0600024A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002A7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<<Invoke>g__ExecuteInvocation|16_0>d'::SetStateMachine

  } // end of class '<<Invoke>g__ExecuteInvocation|16_0>d'

  .class /*0200007F*/ auto ansi serializable sealed nested private beforefieldinit '<>c'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001BC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000043*/ 
      .custom /*0C000125:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000B1*/ public static initonly class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*//'<>c'/*0200007F*/<!THub> '<>9'
    .field /*040000B2*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> '<>9__8_1'
    .field /*040000B3*/ public static class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Exception/*01000031*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> '<>9__8_2'
    .field /*040000B4*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.ValueTuple`4/*0100006B*/<class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub>,class Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/,class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/>,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> '<>9__15_0'
    .custom /*0C000209:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*0100006A*/::.ctor(string[]) /* 0A00001E */ = ( 01 00 04 00 00 00 FF 0A 64 65 73 63 72 69 70 74   // ........descript
                                                                                                                                                                                       6F 72 0A 63 6F 6E 6E 65 63 74 69 6F 6E 1A 68 75   // or.connection.hu
                                                                                                                                                                                       62 4D 65 74 68 6F 64 49 6E 76 6F 63 61 74 69 6F   // bMethodInvocatio
                                                                                                                                                                                       6E 4D 65 73 73 61 67 65 00 00 )                   // nMessage..
    .method /*0600024B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600024C*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            '<.ctor>b__8_1'(class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context) cil managed
    // SIG: 20 01 12 80 A9 12 78
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__8_1'

    .method /*0600024D*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            '<.ctor>b__8_2'(class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context,
                            class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
    // SIG: 20 02 12 80 A9 12 78 12 80 C5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__8_2'

    .method /*0600024E*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            '<ProcessInvocation>b__15_0'(valuetype [System.Runtime/*23000001*/]System.ValueTuple`4/*0100006B*/<class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub>,class Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/,class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/> state) cil managed
    // SIG: 20 01 12 80 A9 15 11 81 AD 04 15 12 80 F8 01 13 00 12 81 38 12 60 12 81 A5
    {
      .param [1]/*080003C8*/ 
      .custom /*0C0002EC:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*0100006A*/::.ctor(string[]) /* 0A00001E */ = ( 01 00 04 00 00 00 FF 0A 64 65 73 63 72 69 70 74   // ........descript
                                                                                                                                                                                         6F 72 0A 63 6F 6E 6E 65 63 74 69 6F 6E 1A 68 75   // or.connection.hu
                                                                                                                                                                                         62 4D 65 74 68 6F 64 49 6E 76 6F 63 61 74 69 6F   // bMethodInvocatio
                                                                                                                                                                                         6E 4D 65 73 73 61 67 65 00 00 )                   // nMessage..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<ProcessInvocation>b__15_0'

  } // end of class '<>c'

  .class /*02000080*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass8_0'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001BE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000045*/ 
      .custom /*0C00012C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000B5*/ public class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/ resolvedFilter
    .field /*040000B6*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<object>> nextFilter
    .field /*040000B7*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> connectedFilter
    .field /*040000B8*/ public class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Exception/*01000031*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> disconnectedFilter
    .method /*0600024F*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::.ctor

    .method /*06000250*/ assembly hidebysig 
            instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<object> 
            '<.ctor>b__3'(class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/ context) cil managed
    // SIG: 20 01 15 11 81 1D 01 1C 12 74
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::'<.ctor>b__3'

    .method /*06000251*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            '<.ctor>b__4'(class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context) cil managed
    // SIG: 20 01 12 80 A9 12 78
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::'<.ctor>b__4'

    .method /*06000252*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
            '<.ctor>b__5'(class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context,
                          class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
    // SIG: 20 02 12 80 A9 12 78 12 80 C5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass8_0'::'<.ctor>b__5'

  } // end of class '<>c__DisplayClass8_0'

  .class /*02000081*/ auto ansi sealed nested private beforefieldinit '<ExecuteMethod>d__21'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001C0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000047*/ 
      .custom /*0C000132:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000B9*/ public int32 '<>1__state'
    .field /*040000BA*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100006C*/<object> '<>t__builder'
    .field /*040000BB*/ public class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ methodExecutor
    .field /*040000BC*/ public class Microsoft.AspNetCore.SignalR.Hub/*02000015*/ hub
    .field /*040000BD*/ public object[] arguments
    .field /*040000BE*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .field /*040000BF*/ private valuetype Microsoft.Extensions.Internal.ObjectMethodExecutorAwaitable/*0200000A*//Awaiter/*02000065*/ '<>u__2'
    .method /*06000253*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteMethod>d__21'::MoveNext

    .method /*06000254*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002A8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteMethod>d__21'::SetStateMachine

  } // end of class '<ExecuteMethod>d__21'

  .class /*02000082*/ auto ansi sealed nested private beforefieldinit '<Invoke>d__16'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001C2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000049*/ 
      .custom /*0C000139:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000C0*/ public int32 '<>1__state'
    .field /*040000C1*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*040000C2*/ public class Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/ descriptor
    .field /*040000C3*/ public class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub> '<>4__this'
    .field /*040000C4*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*040000C5*/ public class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/ hubMethodInvocationMessage
    .field /*040000C6*/ public bool isStreamResponse
    .field /*040000C7*/ public bool isStreamCall
    .field /*040000C8*/ private class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ '<methodExecutor>5__2'
    .field /*040000C9*/ private bool '<disposeScope>5__3'
    .field /*040000CA*/ private valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000068*/ '<scope>5__4'
    .field /*040000CB*/ private class Microsoft.AspNetCore.SignalR.IHubActivator`1/*0200002A*/<!THub> '<hubActivator>5__5'
    .field /*040000CC*/ private !THub '<hub>5__6'
    .field /*040000CD*/ private object '<>7__wrap6'
    .field /*040000CE*/ private int32 '<>7__wrap7'
    .field /*040000CF*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000054*/<bool> '<>u__1'
    .field /*040000D0*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__2'
    .field /*040000D1*/ private object '<>7__wrap8'
    .field /*040000D2*/ private int32 '<>7__wrap9'
    .field /*040000D3*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__3'
    .method /*06000255*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<Invoke>d__16'::MoveNext

    .method /*06000256*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002A9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<Invoke>d__16'::SetStateMachine

  } // end of class '<Invoke>d__16'

  .class /*02000083*/ auto ansi sealed nested private beforefieldinit '<IsHubMethodAuthorizedSlow>d__25'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001C4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A00004B*/ 
      .custom /*0C000140:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000D4*/ public int32 '<>1__state'
    .field /*040000D5*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004A*/<bool> '<>t__builder'
    .field /*040000D6*/ public class [System.ComponentModel/*2300000E*/]System.IServiceProvider/*0100005A*/ provider
    .field /*040000D7*/ public class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<class [Microsoft.AspNetCore.Metadata/*23000010*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100006D*/> policies
    .field /*040000D8*/ public class [System.Security.Claims/*23000005*/]System.Security.Claims.ClaimsPrincipal/*01000032*/ principal
    .field /*040000D9*/ public class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/ resource
    .field /*040000DA*/ private class [Microsoft.AspNetCore.Authorization/*23000011*/]Microsoft.AspNetCore.Authorization.IAuthorizationService/*0100006E*/ '<authService>5__2'
    .field /*040000DB*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000054*/<class [Microsoft.AspNetCore.Authorization/*23000011*/]Microsoft.AspNetCore.Authorization.AuthorizationPolicy/*0100006F*/> '<>u__1'
    .field /*040000DC*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000054*/<class [Microsoft.AspNetCore.Authorization/*23000011*/]Microsoft.AspNetCore.Authorization.AuthorizationResult/*01000070*/> '<>u__2'
    .method /*06000257*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsHubMethodAuthorizedSlow>d__25'::MoveNext

    .method /*06000258*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002AA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsHubMethodAuthorizedSlow>d__25'::SetStateMachine

  } // end of class '<IsHubMethodAuthorizedSlow>d__25'

  .class /*02000084*/ auto ansi sealed nested private beforefieldinit '<OnConnectedAsync>d__9'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001C6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A00004D*/ 
      .custom /*0C00014A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000DD*/ public int32 '<>1__state'
    .field /*040000DE*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*040000DF*/ public class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub> '<>4__this'
    .field /*040000E0*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*040000E1*/ private valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000068*/ '<scope>5__2'
    .field /*040000E2*/ private class Microsoft.AspNetCore.SignalR.IHubActivator`1/*0200002A*/<!THub> '<hubActivator>5__3'
    .field /*040000E3*/ private !THub '<hub>5__4'
    .field /*040000E4*/ private object '<>7__wrap4'
    .field /*040000E5*/ private int32 '<>7__wrap5'
    .field /*040000E6*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .field /*040000E7*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__2'
    .method /*06000259*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnConnectedAsync>d__9'::MoveNext

    .method /*0600025A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002AB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnConnectedAsync>d__9'::SetStateMachine

  } // end of class '<OnConnectedAsync>d__9'

  .class /*02000085*/ auto ansi sealed nested private beforefieldinit '<OnDisconnectedAsync>d__10'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001C8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A00004F*/ 
      .custom /*0C00014E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000E8*/ public int32 '<>1__state'
    .field /*040000E9*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*040000EA*/ public class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub> '<>4__this'
    .field /*040000EB*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*040000EC*/ public class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception
    .field /*040000ED*/ private valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000068*/ '<scope>5__2'
    .field /*040000EE*/ private class Microsoft.AspNetCore.SignalR.IHubActivator`1/*0200002A*/<!THub> '<hubActivator>5__3'
    .field /*040000EF*/ private !THub '<hub>5__4'
    .field /*040000F0*/ private object '<>7__wrap4'
    .field /*040000F1*/ private int32 '<>7__wrap5'
    .field /*040000F2*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .field /*040000F3*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__2'
    .method /*0600025B*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnDisconnectedAsync>d__10'::MoveNext

    .method /*0600025C*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002AD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnDisconnectedAsync>d__10'::SetStateMachine

  } // end of class '<OnDisconnectedAsync>d__10'

  .class /*02000086*/ auto ansi sealed nested private beforefieldinit '<SendInvocationError>d__22'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001CA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000051*/ 
      .custom /*0C000150:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000F4*/ public int32 '<>1__state'
    .field /*040000F5*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*040000F6*/ public string invocationId
    .field /*040000F7*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*040000F8*/ public string errorMessage
    .field /*040000F9*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__1'
    .method /*0600025D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SendInvocationError>d__22'::MoveNext

    .method /*0600025E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002AF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<SendInvocationError>d__22'::SetStateMachine

  } // end of class '<SendInvocationError>d__22'

  .class /*02000087*/ auto ansi sealed nested private beforefieldinit '<StreamAsync>d__18'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001CC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000053*/ 
      .custom /*0C000152:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*040000FA*/ public int32 '<>1__state'
    .field /*040000FB*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*040000FC*/ public class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100003C*/ streamCts
    .field /*040000FD*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*040000FE*/ public string invocationId
    .field /*040000FF*/ public class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub> '<>4__this'
    .field /*04000100*/ public class Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/ descriptor
    .field /*04000101*/ public !THub hub
    .field /*04000102*/ public object[] arguments
    .field /*04000103*/ public valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000068*/ scope
    .field /*04000104*/ public class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/ hubMethodInvocationMessage
    .field /*04000105*/ public class Microsoft.AspNetCore.SignalR.IHubActivator`1/*0200002A*/<!THub> hubActivator
    .field /*04000106*/ private string '<error>5__2'
    .field /*04000107*/ private object '<>7__wrap2'
    .field /*04000108*/ private int32 '<>7__wrap3'
    .field /*04000109*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<object> '<enumerator>5__5'
    .field /*0400010A*/ private class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.StreamItemMessage/*01000064*/ '<streamItemMessage>5__6'
    .field /*0400010B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<object> '<>u__1'
    .field /*0400010C*/ private object '<>7__wrap6'
    .field /*0400010D*/ private int32 '<>7__wrap7'
    .field /*0400010E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__2'
    .field /*0400010F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<bool> '<>u__3'
    .method /*0600025F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StreamAsync>d__18'::MoveNext

    .method /*06000260*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002B0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<StreamAsync>d__18'::SetStateMachine

  } // end of class '<StreamAsync>d__18'

  .class /*02000088*/ auto ansi sealed nested private beforefieldinit '<ValidateInvocationMode>d__26'<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001CE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type THub /*2A000055*/ 
      .custom /*0C000155:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000110*/ public int32 '<>1__state'
    .field /*04000111*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004A*/<bool> '<>t__builder'
    .field /*04000112*/ public class Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/ hubMethodDescriptor
    .field /*04000113*/ public bool isStreamResponse
    .field /*04000114*/ public class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/ hubMethodInvocationMessage
    .field /*04000115*/ public class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub> '<>4__this'
    .field /*04000116*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .field /*04000117*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__1'
    .method /*06000261*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateInvocationMode>d__26'::MoveNext

    .method /*06000262*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002B2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateInvocationMode>d__26'::SetStateMachine

  } // end of class '<ValidateInvocationMode>d__26'

  .method /*0600014C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceScopeFactory/*01000052*/ serviceScopeFactory,
                               class Microsoft.AspNetCore.SignalR.IHubContext`1/*02000030*/<!THub> hubContext,
                               bool enableDetailedErrors,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger`1/*0100002C*/<class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub>> logger,
                               class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/> hubFilters) cil managed
  // SIG: 20 05 01 12 81 49 15 12 80 C0 01 13 00 02 15 12 80 B1 01 15 12 80 F8 01 13 00 15 12 81 71 01 12
  //      80 C8
  {
    .param [5]/*08000283*/ 
    .custom /*0C0002C3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcher`1::.ctor

  .method /*0600014D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnConnectedAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 12 80 A9 12 60
  {
    .custom /*0C000252:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 53 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..SMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        49 6E 74 65 72 6E 61 6C 2E 44 65 66 61 75 6C 74   // Internal.Default
                                                                                                                                                                                                                                        48 75 62 44 69 73 70 61 74 63 68 65 72 60 31 2B   // HubDispatcher`1+
                                                                                                                                                                                                                                        3C 4F 6E 43 6F 6E 6E 65 63 74 65 64 41 73 79 6E   // <OnConnectedAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 39 00 00 )                         // c>d__9..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcher`1::OnConnectedAsync

  .method /*0600014E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnDisconnectedAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                              class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 20 02 12 80 A9 12 60 12 80 C5
  {
    .custom /*0C000253:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 57 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..WMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        49 6E 74 65 72 6E 61 6C 2E 44 65 66 61 75 6C 74   // Internal.Default
                                                                                                                                                                                                                                        48 75 62 44 69 73 70 61 74 63 68 65 72 60 31 2B   // HubDispatcher`1+
                                                                                                                                                                                                                                        3C 4F 6E 44 69 73 63 6F 6E 6E 65 63 74 65 64 41   // <OnDisconnectedA
                                                                                                                                                                                                                                        73 79 6E 63 3E 64 5F 5F 31 30 00 00 )             // sync>d__10..
    .param [2]/*08000286*/ 
    .custom /*0C0002C7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcher`1::OnDisconnectedAsync

  .method /*0600014F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          DispatchMessageAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                               class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/ hubMessage) cil managed
  // SIG: 20 02 12 80 A9 12 60 12 80 F9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcher`1::DispatchMessageAsync

  .method /*06000150*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> 
          GetParameterTypes(string methodName) cil managed
  // SIG: 20 01 15 12 80 B5 01 12 61 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcher`1::GetParameterTypes

  .method /*06000151*/ assembly hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          '<Invoke>g__ExecuteInvocation|16_0'(class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1/*0200003E*/<!THub> dispatcher,
                                              class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ methodExecutor,
                                              !THub hub,
                                              object[] arguments,
                                              valuetype [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.AsyncServiceScope/*01000068*/ scope,
                                              class Microsoft.AspNetCore.SignalR.IHubActivator`1/*0200002A*/<!THub> hubActivator,
                                              class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                                              class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/ hubMethodInvocationMessage,
                                              bool isStreamCall) cil managed
  // SIG: 00 09 12 80 A9 15 12 80 F8 01 13 00 12 24 13 00 1D 1C 11 81 A1 15 12 80 A8 01 13 00 12 60 12 81
  //      A5 02
  {
    .custom /*0C000254:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 61 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..aMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        49 6E 74 65 72 6E 61 6C 2E 44 65 66 61 75 6C 74   // Internal.Default
                                                                                                                                                                                                                                        48 75 62 44 69 73 70 61 74 63 68 65 72 60 31 2B   // HubDispatcher`1+
                                                                                                                                                                                                                                        3C 3C 49 6E 76 6F 6B 65 3E 67 5F 5F 45 78 65 63   // <<Invoke>g__Exec
                                                                                                                                                                                                                                        75 74 65 49 6E 76 6F 63 61 74 69 6F 6E 7C 31 36   // uteInvocation|16
                                                                                                                                                                                                                                        5F 30 3E 64 00 00 )                               // _0>d..
    .custom /*0C000255:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [4]/*0800028D*/ 
    .custom /*0C0002CA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcher`1::'<Invoke>g__ExecuteInvocation|16_0'

} // end of class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher`1

.class /*0200003F*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcherLog
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000118:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000119:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000152*/ public hidebysig static 
          void  ReceivedHubInvocation(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                      class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.InvocationMessage/*01000072*/ invocationMessage) cil managed
  // SIG: 00 02 01 12 81 05 12 81 C9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::ReceivedHubInvocation

  .method /*06000153*/ public hidebysig static 
          void  UnsupportedMessageReceived(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                           string messageType) cil managed
  // SIG: 00 02 01 12 81 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::UnsupportedMessageReceived

  .method /*06000154*/ public hidebysig static 
          void  UnknownHubMethod(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                 string hubMethod) cil managed
  // SIG: 00 02 01 12 81 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::UnknownHubMethod

  .method /*06000155*/ public hidebysig static 
          void  HubMethodNotAuthorized(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                       string hubMethod) cil managed
  // SIG: 00 02 01 12 81 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::HubMethodNotAuthorized

  .method /*06000156*/ public hidebysig static 
          void  StreamingResult(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                string invocationId,
                                class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ objectMethodExecutor) cil managed
  // SIG: 00 03 01 12 81 05 0E 12 24
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::StreamingResult

  .method /*06000157*/ public hidebysig static 
          void  SendingResult(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                              string invocationId,
                              class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ objectMethodExecutor) cil managed
  // SIG: 00 03 01 12 81 05 0E 12 24
  {
    .param [2]/*0800029F*/ 
    .custom /*0C0002D3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::SendingResult

  .method /*06000158*/ public hidebysig static 
          void  FailedInvokingHubMethod(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                        string hubMethod,
                                        class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 00 03 01 12 81 05 0E 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::FailedInvokingHubMethod

  .method /*06000159*/ public hidebysig static 
          void  HubMethodBound(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                               string hubName,
                               string hubMethod) cil managed
  // SIG: 00 03 01 12 81 05 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::HubMethodBound

  .method /*0600015A*/ public hidebysig static 
          void  CancelStream(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                             string invocationId) cil managed
  // SIG: 00 02 01 12 81 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::CancelStream

  .method /*0600015B*/ public hidebysig static 
          void  UnexpectedCancel(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger) cil managed
  // SIG: 00 01 01 12 81 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::UnexpectedCancel

  .method /*0600015C*/ public hidebysig static 
          void  ReceivedStreamHubInvocation(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                            class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.StreamInvocationMessage/*01000073*/ invocationMessage) cil managed
  // SIG: 00 02 01 12 81 05 12 81 CD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::ReceivedStreamHubInvocation

  .method /*0600015D*/ public hidebysig static 
          void  StreamingMethodCalledWithInvoke(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                                class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/ invocationMessage) cil managed
  // SIG: 00 02 01 12 81 05 12 81 A5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::StreamingMethodCalledWithInvoke

  .method /*0600015E*/ public hidebysig static 
          void  NonStreamingMethodCalledWithStream(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                                   class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMethodInvocationMessage/*01000069*/ invocationMessage) cil managed
  // SIG: 00 02 01 12 81 05 12 81 A5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::NonStreamingMethodCalledWithStream

  .method /*0600015F*/ public hidebysig static 
          void  InvalidReturnValueFromStreamingMethod(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                                      string hubMethod) cil managed
  // SIG: 00 02 01 12 81 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::InvalidReturnValueFromStreamingMethod

  .method /*06000160*/ public hidebysig static 
          void  ReceivedStreamItem(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                   class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.StreamItemMessage/*01000064*/ message) cil managed
  // SIG: 00 02 01 12 81 05 12 81 91
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::ReceivedStreamItem

  .method /*06000161*/ public hidebysig static 
          void  StartingParameterStream(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                        string streamId) cil managed
  // SIG: 00 02 01 12 81 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::StartingParameterStream

  .method /*06000162*/ public hidebysig static 
          void  CompletingStream(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                 class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.CompletionMessage/*01000066*/ message) cil managed
  // SIG: 00 02 01 12 81 05 12 81 99
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::CompletingStream

  .method /*06000163*/ public hidebysig static 
          void  ClosingStreamWithBindingError(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                              class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.CompletionMessage/*01000066*/ message) cil managed
  // SIG: 00 02 01 12 81 05 12 81 99
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::ClosingStreamWithBindingError

  .method /*06000164*/ public hidebysig static 
          void  UnexpectedStreamCompletion(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger) cil managed
  // SIG: 00 01 01 12 81 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::UnexpectedStreamCompletion

  .method /*06000165*/ public hidebysig static 
          void  UnexpectedStreamItem(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger) cil managed
  // SIG: 00 01 01 12 81 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::UnexpectedStreamItem

  .method /*06000166*/ public hidebysig static 
          void  InvalidHubParameters(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                     string hubMethod,
                                     class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 00 03 01 12 81 05 0E 12 80 C5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::InvalidHubParameters

  .method /*06000167*/ public hidebysig static 
          void  InvocationIdInUse(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                  string InvocationId) cil managed
  // SIG: 00 02 01 12 81 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubDispatcherLog::InvocationIdInUse

} // end of class Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcherLog

.class /*02000040*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.DefaultHubProtocolResolver
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IHubProtocolResolver/*02000033*/
{
  .custom /*0C00011B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00011C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.SignalR.IHubProtocolResolver/*02000033*/
  .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000089*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .method /*06000263*/ public hidebysig static 
            void  RegisteredSignalRProtocol(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                            string protocolName,
                                            class [System.Runtime/*23000001*/]System.Type/*01000018*/ implementationType) cil managed
    // SIG: 00 03 01 12 81 05 0E 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::RegisteredSignalRProtocol

    .method /*06000264*/ public hidebysig static 
            void  FoundImplementationForProtocol(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000041*/ logger,
                                                 string protocolName) cil managed
    // SIG: 00 02 01 12 81 05 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::FoundImplementationForProtocol

  } // end of class Log

  .method /*06000168*/ public hidebysig newslot specialname virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/> 
          get_AllProtocols() cil managed
  // SIG: 20 00 15 12 80 B5 01 12 80 E9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubProtocolResolver::get_AllProtocols

  .method /*06000169*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/> availableProtocols,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger`1/*0100002C*/<class Microsoft.AspNetCore.SignalR.Internal.DefaultHubProtocolResolver/*02000040*/> logger) cil managed
  // SIG: 20 02 01 15 12 80 D5 01 12 80 E9 15 12 80 B1 01 12 81 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubProtocolResolver::.ctor

  .method /*0600016A*/ public hidebysig newslot virtual 
          instance class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/ 
          GetProtocol(string protocolName,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> supportedProtocols) cil managed
  // SIG: 20 02 12 80 E9 0E 15 12 80 B5 01 0E
  {
    .param [0]/*080002C3*/ 
    .custom /*0C0002D5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080002C5*/ 
    .custom /*0C0002D6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultHubProtocolResolver::GetProtocol

  .property /*17000066*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/>
          AllProtocols()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.IHubProtocol/*0100003A*/> Microsoft.AspNetCore.SignalR.Internal.DefaultHubProtocolResolver/*02000040*/::get_AllProtocols() /* 06000168 */
  } // end of property DefaultHubProtocolResolver::AllProtocols
} // end of class Microsoft.AspNetCore.SignalR.Internal.DefaultHubProtocolResolver

.class /*02000041*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.DynamicClientProxy
       extends [System.Linq.Expressions/*23000002*/]System.Dynamic.DynamicObject/*01000074*/
{
  .method /*0600016B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ clientProxy) cil managed
  // SIG: 20 01 01 12 80 A0
  {
    .custom /*0C000256:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicClientProxy::.ctor

  .method /*0600016C*/ public hidebysig virtual 
          instance bool  TryInvokeMember(class [System.Linq.Expressions/*23000002*/]System.Dynamic.InvokeMemberBinder/*01000075*/ binder,
                                         object[] args,
                                         [out] object& result) cil managed
  // SIG: 20 03 02 12 81 D5 1D 1C 10 1C
  {
    .custom /*0C000257:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080002C7*/ 
    .custom /*0C0002D7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DynamicClientProxy::TryInvokeMember

} // end of class Microsoft.AspNetCore.SignalR.Internal.DynamicClientProxy

.class /*02000042*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.Internal.ErrorMessageHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*0600016D*/ assembly hidebysig static 
          string  BuildErrorMessage(string message,
                                    class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception,
                                    bool includeExceptionDetails) cil managed
  // SIG: 00 03 0E 0E 12 80 C5 02
  {
    .custom /*0C000258:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ErrorMessageHelper::BuildErrorMessage

} // end of class Microsoft.AspNetCore.SignalR.Internal.ErrorMessageHelper

.class /*02000043*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.GroupManager`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/
{
  .custom /*0C000122:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000123:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/
  .custom /*0C000052:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type THub /*2A00000F*/ 
    .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600016E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager) cil managed
  // SIG: 20 01 01 15 12 7C 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupManager`1::.ctor

  .method /*0600016F*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          AddToGroupAsync(string connectionId,
                          string groupName,
                          [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 0E 11 80 AD
  {
    .param [3]/*080002D0*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupManager`1::AddToGroupAsync

  .method /*06000170*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          RemoveFromGroupAsync(string connectionId,
                               string groupName,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 0E 11 80 AD
  {
    .param [3]/*080002D3*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupManager`1::RemoveFromGroupAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.GroupManager`1

.class /*02000044*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubCallerClients
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/,
                  class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000005*/,
                  class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000006*/
{
  .custom /*0C000126:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000127:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/
  .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000005*/
  .custom /*0C000056:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000006*/
  .custom /*0C00005D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*06000171*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/ hubClients,
                               string connectionId) cil managed
  // SIG: 20 02 01 12 80 B4 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::.ctor

  .method /*06000172*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          get_Caller() cil managed
  // SIG: 20 00 12 80 A0
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::get_Caller

  .method /*06000173*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          get_Others() cil managed
  // SIG: 20 00 12 80 A0
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::get_Others

  .method /*06000174*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          get_All() cil managed
  // SIG: 20 00 12 80 A0
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::get_All

  .method /*06000175*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          AllExcept(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 01 12 80 A0 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::AllExcept

  .method /*06000176*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Client(string connectionId) cil managed
  // SIG: 20 01 12 80 A0 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::Client

  .method /*06000177*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Group(string groupName) cil managed
  // SIG: 20 01 12 80 A0 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::Group

  .method /*06000178*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Groups(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames) cil managed
  // SIG: 20 01 12 80 A0 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::Groups

  .method /*06000179*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          OthersInGroup(string groupName) cil managed
  // SIG: 20 01 12 80 A0 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::OthersInGroup

  .method /*0600017A*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          GroupExcept(string groupName,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 02 12 80 A0 0E 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::GroupExcept

  .method /*0600017B*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          User(string userId) cil managed
  // SIG: 20 01 12 80 A0 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::User

  .method /*0600017C*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Clients(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds) cil managed
  // SIG: 20 01 12 80 A0 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::Clients

  .method /*0600017D*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Users(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds) cil managed
  // SIG: 20 01 12 80 A0 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubCallerClients::Users

  .property /*17000067*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
          Caller()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ Microsoft.AspNetCore.SignalR.Internal.HubCallerClients/*02000044*/::get_Caller() /* 06000172 */
  } // end of property HubCallerClients::Caller
  .property /*17000068*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
          Others()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ Microsoft.AspNetCore.SignalR.Internal.HubCallerClients/*02000044*/::get_Others() /* 06000173 */
  } // end of property HubCallerClients::Others
  .property /*17000069*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
          All()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ Microsoft.AspNetCore.SignalR.Internal.HubCallerClients/*02000044*/::get_All() /* 06000174 */
  } // end of property HubCallerClients::All
} // end of class Microsoft.AspNetCore.SignalR.Internal.HubCallerClients

.class /*02000045*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubClients`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/,
                  class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000006*/
{
  .custom /*0C00012A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00012B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/
  .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/>/*1B000006*/
  .custom /*0C000065:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type THub /*2A000010*/ 
    .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600017E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager) cil managed
  // SIG: 20 01 01 15 12 7C 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::.ctor

  .method /*0600017F*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          get_All() cil managed
  // SIG: 20 00 12 80 A0
  {
    .custom /*0C000259:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::get_All

  .method /*06000180*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          AllExcept(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 01 12 80 A0 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::AllExcept

  .method /*06000181*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Client(string connectionId) cil managed
  // SIG: 20 01 12 80 A0 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::Client

  .method /*06000182*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Group(string groupName) cil managed
  // SIG: 20 01 12 80 A0 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::Group

  .method /*06000183*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          GroupExcept(string groupName,
                      class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 02 12 80 A0 0E 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::GroupExcept

  .method /*06000184*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Clients(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds) cil managed
  // SIG: 20 01 12 80 A0 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::Clients

  .method /*06000185*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Groups(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames) cil managed
  // SIG: 20 01 12 80 A0 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::Groups

  .method /*06000186*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          User(string userId) cil managed
  // SIG: 20 01 12 80 A0 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::User

  .method /*06000187*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ 
          Users(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds) cil managed
  // SIG: 20 01 12 80 A0 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`1::Users

  .property /*1700006A*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
          All()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ Microsoft.AspNetCore.SignalR.Internal.HubClients`1/*02000045*/::get_All() /* 0600017F */
  } // end of property HubClients`1::All
} // end of class Microsoft.AspNetCore.SignalR.Internal.HubClients`1

.class /*02000046*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubClients`2<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub,T>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T>/*1B00000B*/
{
  .custom /*0C00012D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00012E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T>/*1B00000B*/
  .custom /*0C00006A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type THub /*2A000011*/ 
    .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type T /*2A000012*/ 
    .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*06000188*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager) cil managed
  // SIG: 20 01 01 15 12 7C 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::.ctor

  .method /*06000189*/ public hidebysig newslot specialname virtual final 
          instance !T  get_All() cil managed
  // SIG: 20 00 13 01
  {
    .custom /*0C00025A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::get_All

  .method /*0600018A*/ public hidebysig newslot virtual final 
          instance !T  AllExcept(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 01 13 01 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::AllExcept

  .method /*0600018B*/ public hidebysig newslot virtual 
          instance !T  Client(string connectionId) cil managed
  // SIG: 20 01 13 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::Client

  .method /*0600018C*/ public hidebysig newslot virtual final 
          instance !T  Clients(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds) cil managed
  // SIG: 20 01 13 01 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::Clients

  .method /*0600018D*/ public hidebysig newslot virtual 
          instance !T  Group(string groupName) cil managed
  // SIG: 20 01 13 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::Group

  .method /*0600018E*/ public hidebysig newslot virtual final 
          instance !T  GroupExcept(string groupName,
                                   class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 02 13 01 0E 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::GroupExcept

  .method /*0600018F*/ public hidebysig newslot virtual final 
          instance !T  Groups(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames) cil managed
  // SIG: 20 01 13 01 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::Groups

  .method /*06000190*/ public hidebysig newslot virtual 
          instance !T  User(string userId) cil managed
  // SIG: 20 01 13 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::User

  .method /*06000191*/ public hidebysig newslot virtual 
          instance !T  Users(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds) cil managed
  // SIG: 20 01 13 01 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubClients`2::Users

  .property /*1700006B*/ instance callconv(8) !T
          All()
  {
    .get instance !T Microsoft.AspNetCore.SignalR.Internal.HubClients`2/*02000046*/::get_All() /* 06000189 */
  } // end of property HubClients`2::All
} // end of class Microsoft.AspNetCore.SignalR.Internal.HubClients`2

.class /*02000047*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubConnectionBinder`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.IInvocationBinder/*01000076*/
{
  .custom /*0C000130:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000131:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.IInvocationBinder/*01000076*/
  .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type THub /*2A000013*/ 
    .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000192*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.Internal.HubDispatcher`1/*0200004A*/<!THub> dispatcher,
                               class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 02 01 15 12 81 28 01 13 00 12 60
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionBinder`1::.ctor

  .method /*06000193*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> 
          GetParameterTypes(string methodName) cil managed
  // SIG: 20 01 15 12 80 B5 01 12 61 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionBinder`1::GetParameterTypes

  .method /*06000194*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          GetReturnType(string invocationId) cil managed
  // SIG: 20 01 12 61 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionBinder`1::GetReturnType

  .method /*06000195*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          GetStreamItemType(string streamId) cil managed
  // SIG: 20 01 12 61 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubConnectionBinder`1::GetStreamItemType

} // end of class Microsoft.AspNetCore.SignalR.Internal.HubConnectionBinder`1

.class /*02000048*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubContext`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IHubContext/*0200002F*/,
                  class Microsoft.AspNetCore.SignalR.IHubContext`1/*02000030*/<!THub>/*1B00000C*/
{
  .custom /*0C000133:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000134:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.SignalR.IHubContext/*0200002F*/
  .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubContext`1/*02000030*/<!THub>/*1B00000C*/
  .custom /*0C00007A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type THub /*2A000014*/ 
    .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000196*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager) cil managed
  // SIG: 20 01 01 15 12 7C 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubContext`1::.ctor

  .method /*06000197*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/ 
          get_Clients() cil managed
  // SIG: 20 00 12 80 B4
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubContext`1::get_Clients

  .method /*06000198*/ public hidebysig newslot specialname virtual 
          instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ 
          get_Groups() cil managed
  // SIG: 20 00 12 80 A4
  {
    .custom /*0C00025B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubContext`1::get_Groups

  .property /*1700006C*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/
          Clients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IHubClients/*0200002D*/ Microsoft.AspNetCore.SignalR.Internal.HubContext`1/*02000048*/::get_Clients() /* 06000197 */
  } // end of property HubContext`1::Clients
  .property /*1700006D*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/
          Groups()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ Microsoft.AspNetCore.SignalR.Internal.HubContext`1/*02000048*/::get_Groups() /* 06000198 */
  } // end of property HubContext`1::Groups
} // end of class Microsoft.AspNetCore.SignalR.Internal.HubContext`1

.class /*02000049*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubContext`2<(class Microsoft.AspNetCore.SignalR.Hub`1/*02000027*/<!T>/*1B000008*/) THub,class T>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class Microsoft.AspNetCore.SignalR.IHubContext`2/*02000031*/<!THub,!T>/*1B00000D*/
{
  .custom /*0C000137:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000138:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubContext`2/*02000031*/<!THub,!T>/*1B00000D*/
  .custom /*0C000081:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .param type THub /*2A000015*/ 
    .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000199*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager) cil managed
  // SIG: 20 01 01 15 12 7C 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubContext`2::.ctor

  .method /*0600019A*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T> 
          get_Clients() cil managed
  // SIG: 20 00 15 12 80 B8 01 13 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubContext`2::get_Clients

  .method /*0600019B*/ public hidebysig newslot specialname virtual 
          instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ 
          get_Groups() cil managed
  // SIG: 20 00 12 80 A4
  {
    .custom /*0C00025C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubContext`2::get_Groups

  .property /*1700006E*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T>
          Clients()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T> Microsoft.AspNetCore.SignalR.Internal.HubContext`2/*02000049*/::get_Clients() /* 0600019A */
  } // end of property HubContext`2::Clients
  .property /*1700006F*/ instance callconv(8) class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/
          Groups()
  {
    .get instance class Microsoft.AspNetCore.SignalR.IGroupManager/*02000029*/ Microsoft.AspNetCore.SignalR.Internal.HubContext`2/*02000049*/::get_Groups() /* 0600019B */
  } // end of property HubContext`2::Groups
} // end of class Microsoft.AspNetCore.SignalR.Internal.HubContext`2

.class /*0200004A*/ private abstract auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubDispatcher`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00013A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00013B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type THub /*2A000017*/ 
    .custom /*0C000083:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600019C*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnConnectedAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection) cil managed
  // SIG: 20 01 12 80 A9 12 60
  {
    // Method begins at RVA 0x0
  } // end of method HubDispatcher`1::OnConnectedAsync

  .method /*0600019D*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnDisconnectedAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                              class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception) cil managed
  // SIG: 20 02 12 80 A9 12 60 12 80 C5
  {
    .param [2]/*080002FD*/ 
    .custom /*0C0002D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method HubDispatcher`1::OnDisconnectedAsync

  .method /*0600019E*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          DispatchMessageAsync(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
                               class [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.Protocol.HubMessage/*0100003E*/ hubMessage) cil managed
  // SIG: 20 02 12 80 A9 12 60 12 80 F9
  {
    // Method begins at RVA 0x0
  } // end of method HubDispatcher`1::DispatchMessageAsync

  .method /*0600019F*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> 
          GetParameterTypes(string name) cil managed
  // SIG: 20 01 15 12 80 B5 01 12 61 0E
  {
    // Method begins at RVA 0x0
  } // end of method HubDispatcher`1::GetParameterTypes

  .method /*060001A0*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubDispatcher`1::.ctor

} // end of class Microsoft.AspNetCore.SignalR.Internal.HubDispatcher`1

.class /*0200004B*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubFilterFactory
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/
{
  .custom /*0C00013D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00013E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/
  .custom /*0C00008A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200008A*/ auto ansi sealed nested private beforefieldinit '<InvokeMethodAsync>d__3'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001D1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000118*/ public int32 '<>1__state'
    .field /*04000119*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100006C*/<object> '<>t__builder'
    .field /*0400011A*/ public class Microsoft.AspNetCore.SignalR.Internal.HubFilterFactory/*0200004B*/ '<>4__this'
    .field /*0400011B*/ public class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/ invocationContext
    .field /*0400011C*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<object>> next
    .field /*0400011D*/ private class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/ '<filter>5__2'
    .field /*0400011E*/ private bool '<owned>5__3'
    .field /*0400011F*/ private object '<>7__wrap3'
    .field /*04000120*/ private int32 '<>7__wrap4'
    .field /*04000121*/ private object '<>7__wrap5'
    .field /*04000122*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000049*/<object> '<>u__1'
    .field /*04000123*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__2'
    .method /*06000265*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeMethodAsync>d__3'::MoveNext

    .method /*06000266*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002B6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeMethodAsync>d__3'::SetStateMachine

  } // end of class '<InvokeMethodAsync>d__3'

  .class /*0200008B*/ auto ansi sealed nested private beforefieldinit '<OnConnectedAsync>d__4'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001D3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000124*/ public int32 '<>1__state'
    .field /*04000125*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000126*/ public class Microsoft.AspNetCore.SignalR.Internal.HubFilterFactory/*0200004B*/ '<>4__this'
    .field /*04000127*/ public class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context
    .field /*04000128*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> next
    .field /*04000129*/ private class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/ '<filter>5__2'
    .field /*0400012A*/ private bool '<owned>5__3'
    .field /*0400012B*/ private object '<>7__wrap3'
    .field /*0400012C*/ private int32 '<>7__wrap4'
    .field /*0400012D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .field /*0400012E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__2'
    .method /*06000267*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnConnectedAsync>d__4'::MoveNext

    .method /*06000268*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002B8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnConnectedAsync>d__4'::SetStateMachine

  } // end of class '<OnConnectedAsync>d__4'

  .class /*0200008C*/ auto ansi sealed nested private beforefieldinit '<OnDisconnectedAsync>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001D5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400012F*/ public int32 '<>1__state'
    .field /*04000130*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*04000131*/ public class Microsoft.AspNetCore.SignalR.Internal.HubFilterFactory/*0200004B*/ '<>4__this'
    .field /*04000132*/ public class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context
    .field /*04000133*/ public class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception
    .field /*04000134*/ public class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Exception/*01000031*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> next
    .field /*04000135*/ private class Microsoft.AspNetCore.SignalR.IHubFilter/*02000032*/ '<filter>5__2'
    .field /*04000136*/ private bool '<owned>5__3'
    .field /*04000137*/ private object '<>7__wrap3'
    .field /*04000138*/ private int32 '<>7__wrap4'
    .field /*04000139*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .field /*0400013A*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000055*/ '<>u__2'
    .method /*06000269*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnDisconnectedAsync>d__5'::MoveNext

    .method /*0600026A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002B9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<OnDisconnectedAsync>d__5'::SetStateMachine

  } // end of class '<OnDisconnectedAsync>d__5'

  .method /*060001A1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/ filterType) cil managed
  // SIG: 20 01 01 12 61
  {
    .param [1]/*08000301*/ 
    .custom /*0C0002D9:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100005E*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100005D*/) /* 0A00001C */ = ( 01 00 03 00 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubFilterFactory::.ctor

  .method /*060001A2*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<object> 
          InvokeMethodAsync(class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/ invocationContext,
                            class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubInvocationContext/*0200001D*/,valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<object>> next) cil managed
  // SIG: 20 02 15 11 81 1D 01 1C 12 74 15 12 75 02 12 74 15 11 81 1D 01 1C
  {
    .custom /*0C00025D:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        49 6E 74 65 72 6E 61 6C 2E 48 75 62 46 69 6C 74   // Internal.HubFilt
                                                                                                                                                                                                                                        65 72 46 61 63 74 6F 72 79 2B 3C 49 6E 76 6F 6B   // erFactory+<Invok
                                                                                                                                                                                                                                        65 4D 65 74 68 6F 64 41 73 79 6E 63 3E 64 5F 5F   // eMethodAsync>d__
                                                                                                                                                                                                                                        33 00 00 )                                        // 3..
    .param [0]/*08000302*/ 
    .custom /*0C0002DA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [2]/*08000304*/ 
    .custom /*0C0002DB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubFilterFactory::InvokeMethodAsync

  .method /*060001A3*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnConnectedAsync(class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> next) cil managed
  // SIG: 20 02 12 80 A9 12 78 15 12 75 02 12 78 12 80 A9
  {
    .custom /*0C00025E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 4D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..MMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        49 6E 74 65 72 6E 61 6C 2E 48 75 62 46 69 6C 74   // Internal.HubFilt
                                                                                                                                                                                                                                        65 72 46 61 63 74 6F 72 79 2B 3C 4F 6E 43 6F 6E   // erFactory+<OnCon
                                                                                                                                                                                                                                        6E 65 63 74 65 64 41 73 79 6E 63 3E 64 5F 5F 34   // nectedAsync>d__4
                                                                                                                                                                                                                                        00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubFilterFactory::OnConnectedAsync

  .method /*060001A4*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          OnDisconnectedAsync(class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/ context,
                              class [System.Runtime/*23000001*/]System.Exception/*01000031*/ exception,
                              class [System.Runtime/*23000001*/]System.Func`3/*0100002E*/<class Microsoft.AspNetCore.SignalR.HubLifetimeContext/*0200001E*/,class [System.Runtime/*23000001*/]System.Exception/*01000031*/,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> next) cil managed
  // SIG: 20 03 12 80 A9 12 78 12 80 C5 15 12 80 B9 03 12 78 12 80 C5 12 80 A9
  {
    .custom /*0C00025F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100003F*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000016 */ = ( 01 00 50 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..PMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                        49 6E 74 65 72 6E 61 6C 2E 48 75 62 46 69 6C 74   // Internal.HubFilt
                                                                                                                                                                                                                                        65 72 46 61 63 74 6F 72 79 2B 3C 4F 6E 44 69 73   // erFactory+<OnDis
                                                                                                                                                                                                                                        63 6F 6E 6E 65 63 74 65 64 41 73 79 6E 63 3E 64   // connectedAsync>d
                                                                                                                                                                                                                                        5F 5F 35 00 00 )                                  // __5..
    .param [2]/*08000308*/ 
    .custom /*0C0002DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000309*/ 
    .custom /*0C0002DD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 01 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubFilterFactory::OnDisconnectedAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.HubFilterFactory

.class /*0200004C*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubGroupList
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000078*/<class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>>/*1B00000E*/,
                  class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>>/*1B00000F*/,
                  [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000077*/
{
  .custom /*0C000141:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000142:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000143:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*01000056*/::.ctor(string) /* 0A00001A */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyCollection`1/*01000078*/<class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>>/*1B00000E*/
  .custom /*0C000091:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 01 01 01 00 00 ) 
  .interfaceimpl type class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>>/*1B00000F*/
  .custom /*0C000097:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 00 01 01 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000077*/
  .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200008D*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001D7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400013B*/ public static initonly class Microsoft.AspNetCore.SignalR.Internal.HubGroupList/*0200004C*//'<>c'/*0200008D*/ '<>9'
    .field /*0400013C*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000059*/<string,class Microsoft.AspNetCore.SignalR.Internal.GroupConnectionList/*0200004D*/>,string> '<>9__6_1'
    .method /*0600026B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600026C*/ assembly hidebysig 
            instance string  '<RemoveDisconnectedConnection>b__6_1'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000059*/<string,class Microsoft.AspNetCore.SignalR.Internal.GroupConnectionList/*0200004D*/> x) cil managed
    // SIG: 20 01 0E 15 11 81 65 02 0E 12 81 34
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<RemoveDisconnectedConnection>b__6_1'

  } // end of class '<>c'

  .class /*0200008E*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass11_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001D9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400013D*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .method /*0600026D*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass11_0'::.ctor

    .method /*0600026E*/ assembly hidebysig 
            instance class Microsoft.AspNetCore.SignalR.Internal.GroupConnectionList/*0200004D*/ 
            '<CreateOrUpdateGroupWithConnection>b__0'(string _) cil managed
    // SIG: 20 01 12 81 34 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass11_0'::'<CreateOrUpdateGroupWithConnection>b__0'

    .method /*0600026F*/ assembly hidebysig 
            instance class Microsoft.AspNetCore.SignalR.Internal.GroupConnectionList/*0200004D*/ 
            '<CreateOrUpdateGroupWithConnection>b__1'(string key,
                                                      class Microsoft.AspNetCore.SignalR.Internal.GroupConnectionList/*0200004D*/ oldCollection) cil managed
    // SIG: 20 02 12 81 34 0E 12 81 34
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass11_0'::'<CreateOrUpdateGroupWithConnection>b__1'

  } // end of class '<>c__DisplayClass11_0'

  .class /*0200008F*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass12_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001DB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400013E*/ public class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection
    .method /*06000270*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass12_0'::.ctor

    .method /*06000271*/ assembly hidebysig 
            instance class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ 
            '<AddConnectionToGroup>b__0'(string _,
                                         class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ __) cil managed
    // SIG: 20 02 12 60 0E 12 60
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass12_0'::'<AddConnectionToGroup>b__0'

  } // end of class '<>c__DisplayClass12_0'

  .class /*02000090*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass6_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001DE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400013F*/ public string connectionId
    .method /*06000272*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_0'::.ctor

    .method /*06000273*/ assembly hidebysig 
            instance bool  '<RemoveDisconnectedConnection>b__0'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000059*/<string,class Microsoft.AspNetCore.SignalR.Internal.GroupConnectionList/*0200004D*/> x) cil managed
    // SIG: 20 01 02 15 11 81 65 02 0E 12 81 34
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass6_0'::'<RemoveDisconnectedConnection>b__0'

  } // end of class '<>c__DisplayClass6_0'

  .method /*060001A5*/ public hidebysig specialname 
          instance class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/> 
          get_Item(string groupName) cil managed
  // SIG: 20 01 15 12 80 ED 02 0E 12 60 0E
  {
    .param [0]/*0800030A*/ 
    .custom /*0C0002DE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubGroupList::get_Item

  .method /*060001A6*/ public hidebysig instance void 
          Add(class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/ connection,
              string groupName) cil managed
  // SIG: 20 02 01 12 60 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubGroupList::Add

  .method /*060001A7*/ public hidebysig instance void 
          Remove(string connectionId,
                 string groupName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubGroupList::Remove

  .method /*060001A8*/ public hidebysig instance void 
          RemoveDisconnectedConnection(string connectionId) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubGroupList::RemoveDisconnectedConnection

  .method /*060001A9*/ public hidebysig newslot specialname virtual final 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubGroupList::get_Count

  .method /*060001AA*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>> 
          GetEnumerator() cil managed
  // SIG: 20 00 15 12 81 61 01 15 12 80 ED 02 0E 12 60
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubGroupList::GetEnumerator

  .method /*060001AB*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/ 
          System.Collections.IEnumerable.GetEnumerator() cil managed
  // SIG: 20 00 12 81 5D
  {
    .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000077*/::GetEnumerator /*01000077::0A00001F*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubGroupList::System.Collections.IEnumerable.GetEnumerator

  .method /*060001AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubGroupList::.ctor

  .property /*17000070*/ instance callconv(8) class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>
          Item(string)
  {
    .custom /*0C000194:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 01 00 00 ) 
    .get instance class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/> Microsoft.AspNetCore.SignalR.Internal.HubGroupList/*0200004C*/::get_Item(string) /* 060001A5 */
  } // end of property HubGroupList::Item
  .property /*17000071*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.AspNetCore.SignalR.Internal.HubGroupList/*0200004C*/::get_Count() /* 060001A9 */
  } // end of property HubGroupList::Count
} // end of class Microsoft.AspNetCore.SignalR.Internal.HubGroupList

.class /*0200004D*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.GroupConnectionList
       extends class [System.Collections.Concurrent/*2300000B*/]System.Collections.Concurrent.ConcurrentDictionary`2/*0100003B*/<string,class Microsoft.AspNetCore.SignalR.HubConnectionContext/*02000018*/>/*1B000010*/
{
  .custom /*0C000147:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000148:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .method /*060001AD*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupConnectionList::Equals

  .method /*060001AE*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupConnectionList::GetHashCode

  .method /*060001AF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupConnectionList::.ctor

} // end of class Microsoft.AspNetCore.SignalR.Internal.GroupConnectionList

.class /*0200004E*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00014B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00014C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000091*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001E0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000140*/ public static initonly class Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*//'<>c'/*02000091*/ '<>9'
    .field /*04000141*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/,class [System.Runtime/*23000001*/]System.Type/*01000018*/> '<>9__4_1'
    .field /*04000142*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/,class [System.Runtime/*23000001*/]System.Type/*01000018*/> '<>9__4_2'
    .method /*06000274*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000275*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
            '<.ctor>b__4_1'(class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/ p) cil managed
    // SIG: 20 01 12 61 12 7D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__4_1'

    .method /*06000276*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
            '<.ctor>b__4_2'(class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/ p) cil managed
    // SIG: 20 01 12 61 12 7D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__4_2'

    .method /*06000277*/ assembly hidebysig 
            instance bool  '<.cctor>b__35_0'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__35_0'

    .method /*06000278*/ assembly hidebysig 
            instance bool  '<.cctor>b__35_1'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__35_1'

  } // end of class '<>c'

  .method /*060001B0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ methodExecutor,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [Microsoft.AspNetCore.Metadata/*23000010*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100006D*/> policies) cil managed
  // SIG: 20 02 01 12 24 15 12 80 D5 01 12 81 B5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::.ctor

  .method /*060001B1*/ public hidebysig specialname 
          instance class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> 
          get_StreamingParameters() cil managed
  // SIG: 20 00 15 12 81 71 01 12 61
  {
    .custom /*0C000260:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000314*/ 
    .custom /*0C0002DF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_StreamingParameters

  .method /*060001B2*/ public hidebysig specialname 
          instance class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ 
          get_MethodExecutor() cil managed
  // SIG: 20 00 12 24
  {
    .custom /*0C000261:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_MethodExecutor

  .method /*060001B3*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> 
          get_ParameterTypes() cil managed
  // SIG: 20 00 15 12 80 B5 01 12 61
  {
    .custom /*0C000262:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_ParameterTypes

  .method /*060001B4*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> 
          get_OriginalParameterTypes() cil managed
  // SIG: 20 00 15 12 80 B5 01 12 61
  {
    .custom /*0C000263:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param [0]/*08000315*/ 
    .custom /*0C0002E0:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_OriginalParameterTypes

  .method /*060001B5*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          get_NonAsyncReturnType() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000264:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_NonAsyncReturnType

  .method /*060001B6*/ public hidebysig specialname 
          instance bool  get_IsStreamResponse() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_IsStreamResponse

  .method /*060001B7*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
          get_StreamReturnType() cil managed
  // SIG: 20 00 12 61
  {
    .custom /*0C000265:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000266:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_StreamReturnType

  .method /*060001B8*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<class [Microsoft.AspNetCore.Metadata/*23000010*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100006D*/> 
          get_Policies() cil managed
  // SIG: 20 00 15 12 81 6D 01 12 81 B5
  {
    .custom /*0C000267:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_Policies

  .method /*060001B9*/ public hidebysig specialname 
          instance bool  get_HasSyntheticArguments() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000268:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::get_HasSyntheticArguments

  .method /*060001BA*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<object> 
          FromReturnedStream(object 'stream',
                             valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 81 C5 01 1C 1C 11 80 AD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubMethodDescriptor::FromReturnedStream

  .property /*17000072*/ instance callconv(8) class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/>
          StreamingParameters()
  {
    .custom /*0C00019B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Collections/*2300000F*/]System.Collections.Generic.List`1/*0100005C*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_StreamingParameters() /* 060001B1 */
  } // end of property HubMethodDescriptor::StreamingParameters
  .property /*17000073*/ instance callconv(8) class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/
          MethodExecutor()
  {
    .get instance class Microsoft.Extensions.Internal.ObjectMethodExecutor/*02000009*/ Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_MethodExecutor() /* 060001B2 */
  } // end of property HubMethodDescriptor::MethodExecutor
  .property /*17000074*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/>
          ParameterTypes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_ParameterTypes() /* 060001B3 */
  } // end of property HubMethodDescriptor::ParameterTypes
  .property /*17000075*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/>
          OriginalParameterTypes()
  {
    .custom /*0C0001A4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_OriginalParameterTypes() /* 060001B4 */
  } // end of property HubMethodDescriptor::OriginalParameterTypes
  .property /*17000076*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
          NonAsyncReturnType()
  {
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_NonAsyncReturnType() /* 060001B5 */
  } // end of property HubMethodDescriptor::NonAsyncReturnType
  .property /*17000077*/ instance callconv(8) bool
          IsStreamResponse()
  {
    .get instance bool Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_IsStreamResponse() /* 060001B6 */
  } // end of property HubMethodDescriptor::IsStreamResponse
  .property /*17000078*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
          StreamReturnType()
  {
    .custom /*0C0001AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_StreamReturnType() /* 060001B7 */
  } // end of property HubMethodDescriptor::StreamReturnType
  .property /*17000079*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<class [Microsoft.AspNetCore.Metadata/*23000010*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100006D*/>
          Policies()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100005B*/<class [Microsoft.AspNetCore.Metadata/*23000010*/]Microsoft.AspNetCore.Authorization.IAuthorizeData/*0100006D*/> Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_Policies() /* 060001B8 */
  } // end of property HubMethodDescriptor::Policies
  .property /*1700007A*/ instance callconv(8) bool
          HasSyntheticArguments()
  {
    .get instance bool Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor/*0200004E*/::get_HasSyntheticArguments() /* 060001B9 */
  } // end of property HubMethodDescriptor::HasSyntheticArguments
} // end of class Microsoft.AspNetCore.SignalR.Internal.HubMethodDescriptor

.class /*0200004F*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.Internal.HubReflectionHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .class /*02000092*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001E2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000143*/ public static initonly class Microsoft.AspNetCore.SignalR.Internal.HubReflectionHelper/*0200004F*//'<>c'/*02000092*/ '<>9'
    .field /*04000144*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/,bool> '<>9__1_1'
    .method /*06000279*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600027A*/ assembly hidebysig 
            instance bool  '<GetHubMethods>b__1_1'(class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ m) cil managed
    // SIG: 20 01 02 12 71
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetHubMethods>b__1_1'

  } // end of class '<>c'

  .class /*02000093*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass1_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001E4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000145*/ public class [System.Runtime/*23000001*/]System.Type/*01000018*/ hubType
    .method /*0600027B*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::.ctor

    .method /*0600027C*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/> 
            '<GetHubMethods>b__0'(class [System.Runtime/*23000001*/]System.Type/*01000018*/ i) cil managed
    // SIG: 20 01 15 12 80 D5 01 12 71 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass1_0'::'<GetHubMethods>b__0'

  } // end of class '<>c__DisplayClass1_0'

  .method /*060001BB*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/> 
          GetHubMethods(class [System.Runtime/*23000001*/]System.Type/*01000018*/ hubType) cil managed
  // SIG: 00 01 15 12 80 D5 01 12 71 12 61
  {
    .custom /*0C000269:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HubReflectionHelper::GetHubMethods

} // end of class Microsoft.AspNetCore.SignalR.Internal.HubReflectionHelper

.class /*02000050*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.UserProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A000018*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001BC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               string userId) cil managed
  // SIG: 20 02 01 15 12 7C 01 13 00 0E
  {
    .custom /*0C00026A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UserProxy`1::.ctor

  .method /*060001BD*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C00026B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800031C*/ 
    .custom /*0C0002E1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*0800031D*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UserProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.UserProxy`1

.class /*02000051*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.MultipleUserProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A000019*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001BE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds) cil managed
  // SIG: 20 02 01 15 12 7C 01 13 00 15 12 80 B5 01 0E
  {
    .custom /*0C00026C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipleUserProxy`1::.ctor

  .method /*060001BF*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C00026D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000321*/ 
    .custom /*0C0002E2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000322*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipleUserProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.MultipleUserProxy`1

.class /*02000052*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.GroupProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A00001A*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001C0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               string groupName) cil managed
  // SIG: 20 02 01 15 12 7C 01 13 00 0E
  {
    .custom /*0C00026E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupProxy`1::.ctor

  .method /*060001C1*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C00026F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000326*/ 
    .custom /*0C0002E3:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000327*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.GroupProxy`1

.class /*02000053*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.MultipleGroupProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A00001B*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001C2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames) cil managed
  // SIG: 20 02 01 15 12 7C 01 13 00 15 12 80 B5 01 0E
  {
    .custom /*0C000270:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipleGroupProxy`1::.ctor

  .method /*060001C3*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C000271:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800032B*/ 
    .custom /*0C0002E4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*0800032C*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipleGroupProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.MultipleGroupProxy`1

.class /*02000054*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.GroupExceptProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A00001C*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001C4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               string groupName,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 03 01 15 12 7C 01 13 00 0E 15 12 80 B5 01 0E
  {
    .custom /*0C000272:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupExceptProxy`1::.ctor

  .method /*060001C5*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C000273:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000331*/ 
    .custom /*0C0002E5:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000332*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GroupExceptProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.GroupExceptProxy`1

.class /*02000055*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.AllClientProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A00001D*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001C6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager) cil managed
  // SIG: 20 01 01 15 12 7C 01 13 00
  {
    .custom /*0C000274:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AllClientProxy`1::.ctor

  .method /*060001C7*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C000275:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000335*/ 
    .custom /*0C0002E6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000336*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AllClientProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.AllClientProxy`1

.class /*02000056*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.AllClientsExceptProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A00001E*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 02 01 15 12 7C 01 13 00 15 12 80 B5 01 0E
  {
    .custom /*0C000277:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AllClientsExceptProxy`1::.ctor

  .method /*060001C9*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C000278:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800033A*/ 
    .custom /*0C0002E7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*0800033B*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AllClientsExceptProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.AllClientsExceptProxy`1

.class /*02000057*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.SingleClientProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A00001F*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001CA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               string connectionId) cil managed
  // SIG: 20 02 01 15 12 7C 01 13 00 0E
  {
    .custom /*0C000279:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SingleClientProxy`1::.ctor

  .method /*060001CB*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C00027A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800033F*/ 
    .custom /*0C0002E8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000340*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SingleClientProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.SingleClientProxy`1

.class /*02000058*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.MultipleClientProxy`1<(Microsoft.AspNetCore.SignalR.Hub/*02000015*/) THub>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/
{
  .param constraint   THub/*2A000020*/ , Microsoft.AspNetCore.SignalR.Hub/*02000015*/
    .custom /*0C000098:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  .method /*060001CC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.HubLifetimeManager`1/*0200001F*/<!THub> lifetimeManager,
                               class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds) cil managed
  // SIG: 20 02 01 15 12 7C 01 13 00 15 12 80 B5 01 0E
  {
    .custom /*0C00027B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipleClientProxy`1::.ctor

  .method /*060001CD*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          SendCoreAsync(string 'method',
                        object[] args,
                        [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 A9 0E 1D 1C 11 80 AD
  {
    .custom /*0C00027C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000344*/ 
    .custom /*0C0002E9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [3]/*08000345*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipleClientProxy`1::SendCoreAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.MultipleClientProxy`1

.class /*02000059*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.Internal.SemaphoreSlimExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00015B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000094*/ auto ansi sealed nested private beforefieldinit '<RunSlowAsync>d__1`1'<TState>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001E7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000146*/ public int32 '<>1__state'
    .field /*04000147*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004A*/<class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> '<>t__builder'
    .field /*04000148*/ public class [System.Threading/*23000008*/]System.Threading.SemaphoreSlim/*01000038*/ semaphoreSlim
    .field /*04000149*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<!TState,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> callback
    .field /*0400014A*/ public !TState state
    .field /*0400014B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .method /*0600027D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunSlowAsync>d__1`1'::MoveNext

    .method /*0600027E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002BE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunSlowAsync>d__1`1'::SetStateMachine

  } // end of class '<RunSlowAsync>d__1`1'

  .class /*02000095*/ auto ansi sealed nested private beforefieldinit '<RunTask>d__2`1'<TState>
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
  {
    .custom /*0C0001E8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400014C*/ public int32 '<>1__state'
    .field /*0400014D*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000043*/ '<>t__builder'
    .field /*0400014E*/ public class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<!TState,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> callback
    .field /*0400014F*/ public !TState state
    .field /*04000150*/ public class [System.Threading/*23000008*/]System.Threading.SemaphoreSlim/*01000038*/ semaphoreSlim
    .field /*04000151*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000044*/ '<>u__1'
    .method /*0600027F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunTask>d__2`1'::MoveNext

    .method /*06000280*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
    // SIG: 20 01 01 12 81 09
    {
      .custom /*0C0002C0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<RunTask>d__2`1'::SetStateMachine

  } // end of class '<RunTask>d__2`1'

  .method /*060001CE*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/ 
          RunAsync<TState>(class [System.Threading/*23000008*/]System.Threading.SemaphoreSlim/*01000038*/ semaphoreSlim,
                           class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<!!TState,class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002A*/> callback,
                           !!TState state) cil managed
  // SIG: 10 01 03 12 80 A9 12 80 E1 15 12 75 02 1E 00 12 80 A9 1E 00
  {
    .custom /*0C00027D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00027E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TState /*2A000070*/ 
      .custom /*0C000195:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SemaphoreSlimExtensions::RunAsync

} // end of class Microsoft.AspNetCore.SignalR.Internal.SemaphoreSlimExtensions

.class /*0200005A*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.SignalRServerBuilder
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.SignalR.ISignalRServerBuilder/*02000034*/,
                  [Microsoft.AspNetCore.SignalR.Common/*2300000A*/]Microsoft.AspNetCore.SignalR.ISignalRBuilder/*01000061*/
{
  .method /*060001CF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000028*/ services) cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .custom /*0C00027F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignalRServerBuilder::.ctor

  .method /*060001D0*/ public hidebysig newslot specialname virtual final 
          instance class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000028*/ 
          get_Services() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C000280:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000281:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignalRServerBuilder::get_Services

  .property /*1700007B*/ instance callconv(8) class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000028*/
          Services()
  {
    .custom /*0C0001B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000003*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000028*/ Microsoft.AspNetCore.SignalR.Internal.SignalRServerBuilder/*0200005A*/::get_Services() /* 060001D0 */
  } // end of property SignalRServerBuilder::Services
} // end of class Microsoft.AspNetCore.SignalR.Internal.SignalRServerBuilder

.class /*0200005B*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.SignalRCoreMarkerService
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*060001D1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignalRCoreMarkerService::.ctor

} // end of class Microsoft.AspNetCore.SignalR.Internal.SignalRCoreMarkerService

.class /*0200005C*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.Internal.TypeBaseEnumerationExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00015F:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000096*/ auto ansi sealed nested private beforefieldinit '<AllBaseTypes>d__0'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/>/*1B000012*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000077*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/>/*1B000011*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000030*/
  {
    .custom /*0C0001E9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000152*/ public class [System.Runtime/*23000001*/]System.Type/*01000018*/ '<>3__type'
    .method /*06000281*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C0002C1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AllBaseTypes>d__0'::.ctor

    .method /*06000282*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0002C2:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000030*/::Dispose /*01000030::0A000021*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AllBaseTypes>d__0'::System.IDisposable.Dispose

    .method /*06000283*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/::MoveNext /*01000057::0A000022*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AllBaseTypes>d__0'::MoveNext

    .method /*06000284*/ private hidebysig newslot specialname virtual final 
            instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
            'System.Collections.Generic.IEnumerator<System.Type>.get_Current'() cil managed
    // SIG: 20 00 12 61
    {
      .custom /*0C0002C4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/>/*1B000011*/::get_Current() /* 0A000023 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AllBaseTypes>d__0'::'System.Collections.Generic.IEnumerator<System.Type>.get_Current'

    .method /*06000285*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0002C5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/::Reset /*01000057::0A000024*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AllBaseTypes>d__0'::System.Collections.IEnumerator.Reset

    .method /*06000286*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C0002C6:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/::get_Current /*01000057::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AllBaseTypes>d__0'::System.Collections.IEnumerator.get_Current

    .method /*06000287*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> 
            'System.Collections.Generic.IEnumerable<System.Type>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 81 61 01 12 61
    {
      .custom /*0C0002C8:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/>/*1B000012*/::GetEnumerator() /* 0A000025 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AllBaseTypes>d__0'::'System.Collections.Generic.IEnumerable<System.Type>.GetEnumerator'

    .method /*06000288*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 81 5D
    {
      .custom /*0C0002C9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000077*/::GetEnumerator /*01000077::0A00001F*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<AllBaseTypes>d__0'::System.Collections.IEnumerable.GetEnumerator

    .property /*17000082*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000018*/
            'System.Collections.Generic.IEnumerator<System.Type>.Current'()
    {
      .get instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ Microsoft.AspNetCore.SignalR.Internal.TypeBaseEnumerationExtensions/*0200005C*//'<AllBaseTypes>d__0'/*02000096*/::'System.Collections.Generic.IEnumerator<System.Type>.get_Current'() /* 06000284 */
    } // end of property '<AllBaseTypes>d__0'::'System.Collections.Generic.IEnumerator<System.Type>.Current'
    .property /*17000083*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.SignalR.Internal.TypeBaseEnumerationExtensions/*0200005C*//'<AllBaseTypes>d__0'/*02000096*/::System.Collections.IEnumerator.get_Current() /* 06000286 */
    } // end of property '<AllBaseTypes>d__0'::System.Collections.IEnumerator.Current
  } // end of class '<AllBaseTypes>d__0'

  .method /*060001D2*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/> 
          AllBaseTypes(class [System.Runtime/*23000001*/]System.Type/*01000018*/ 'type') cil managed
  // SIG: 00 01 15 12 80 D5 01 12 61 12 61
  {
    .custom /*0C000282:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000283:0A000020*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IteratorStateMachineAttribute/*01000079*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000018*/) /* 0A000020 */ = ( 01 00 56 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..VMicrosoft.Asp
                                                                                                                                                                                                                                           4E 65 74 43 6F 72 65 2E 53 69 67 6E 61 6C 52 2E   // NetCore.SignalR.
                                                                                                                                                                                                                                           49 6E 74 65 72 6E 61 6C 2E 54 79 70 65 42 61 73   // Internal.TypeBas
                                                                                                                                                                                                                                           65 45 6E 75 6D 65 72 61 74 69 6F 6E 45 78 74 65   // eEnumerationExte
                                                                                                                                                                                                                                           6E 73 69 6F 6E 73 2B 3C 41 6C 6C 42 61 73 65 54   // nsions+<AllBaseT
                                                                                                                                                                                                                                           79 70 65 73 3E 64 5F 5F 30 00 00 )                // ypes>d__0..
    .custom /*0C000284:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypeBaseEnumerationExtensions::AllBaseTypes

} // end of class Microsoft.AspNetCore.SignalR.Internal.TypeBaseEnumerationExtensions

.class /*0200005D*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.Internal.TypedClientBuilder`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .param type T /*2A000021*/ 
    .custom /*0C0000B2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .class /*02000097*/ auto ansi serializable sealed nested private beforefieldinit '<>c'<T>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C0001EB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type T /*2A000065*/ 
      .custom /*0C000178:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .field /*04000153*/ public static initonly class Microsoft.AspNetCore.SignalR.Internal.TypedClientBuilder`1/*0200005D*//'<>c'/*02000097*/<!T> '<>9'
    .field /*04000154*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/,class [System.Runtime/*23000001*/]System.Type/*01000018*/> '<>9__11_0'
    .field /*04000155*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/,bool> '<>9__11_1'
    .field /*04000156*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class [System.Runtime/*23000001*/]System.Type/*01000018*/,string> '<>9__11_2'
    .method /*06000289*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600028A*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Type/*01000018*/ 
            '<BuildMethod>b__11_0'(class [System.Runtime/*23000001*/]System.Reflection.ParameterInfo/*0100001F*/ param) cil managed
    // SIG: 20 01 12 61 12 7D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<BuildMethod>b__11_0'

    .method /*0600028B*/ assembly hidebysig 
            instance bool  '<BuildMethod>b__11_1'(class [System.Runtime/*23000001*/]System.Type/*01000018*/ p) cil managed
    // SIG: 20 01 02 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<BuildMethod>b__11_1'

    .method /*0600028C*/ assembly hidebysig 
            instance string  '<BuildMethod>b__11_2'(class [System.Runtime/*23000001*/]System.Type/*01000018*/ p) cil managed
    // SIG: 20 01 0E 12 61
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<BuildMethod>b__11_2'

    .method /*0600028D*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Func`2/*0100001D*/<class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/,!T> 
            '<.cctor>b__15_0'() cil managed
    // SIG: 20 00 15 12 75 02 12 80 A0 13 00
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.cctor>b__15_0'

  } // end of class '<>c'

  .class /*02000098*/ auto ansi sealed nested private beforefieldinit '<GetAllInterfaceMethods>d__9'<T>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/>/*1B000014*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000077*/,
                    class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/>/*1B000013*/,
                    [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/,
                    [System.Runtime/*23000001*/]System.IDisposable/*01000030*/
  {
    .custom /*0C0001EE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .param type T /*2A000066*/ 
      .custom /*0C00017A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .field /*04000157*/ public class [System.Runtime/*23000001*/]System.Type/*01000018*/ '<>3__interfaceType'
    .method /*0600028E*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(int32 '<>1__state') cil managed
    // SIG: 20 01 01 08
    {
      .custom /*0C0002CB:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetAllInterfaceMethods>d__9'::.ctor

    .method /*0600028F*/ private hidebysig newslot virtual final 
            instance void  System.IDisposable.Dispose() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0002CC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.IDisposable/*01000030*/::Dispose /*01000030::0A000021*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetAllInterfaceMethods>d__9'::System.IDisposable.Dispose

    .method /*06000290*/ private hidebysig newslot virtual final 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/::MoveNext /*01000057::0A000022*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetAllInterfaceMethods>d__9'::MoveNext

    .method /*06000291*/ private hidebysig newslot specialname virtual final 
            instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ 
            'System.Collections.Generic.IEnumerator<System.Reflection.MethodInfo>.get_Current'() cil managed
    // SIG: 20 00 12 71
    {
      .custom /*0C0002CD:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override  method instance !0 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/>/*1B000013*/::get_Current() /* 0A000026 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetAllInterfaceMethods>d__9'::'System.Collections.Generic.IEnumerator<System.Reflection.MethodInfo>.get_Current'

    .method /*06000292*/ private hidebysig newslot virtual final 
            instance void  System.Collections.IEnumerator.Reset() cil managed
    // SIG: 20 00 01
    {
      .custom /*0C0002CE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/::Reset /*01000057::0A000024*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetAllInterfaceMethods>d__9'::System.Collections.IEnumerator.Reset

    .method /*06000293*/ private hidebysig newslot specialname virtual final 
            instance object  System.Collections.IEnumerator.get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C0002CF:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/::get_Current /*01000057::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetAllInterfaceMethods>d__9'::System.Collections.IEnumerator.get_Current

    .method /*06000294*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/> 
            'System.Collections.Generic.IEnumerable<System.Reflection.MethodInfo>.GetEnumerator'() cil managed
    // SIG: 20 00 15 12 81 61 01 12 71
    {
      .custom /*0C0002D0:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override  method instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000058*/<!0> class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000035*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/>/*1B000014*/::GetEnumerator() /* 0A000027 */
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetAllInterfaceMethods>d__9'::'System.Collections.Generic.IEnumerable<System.Reflection.MethodInfo>.GetEnumerator'

    .method /*06000295*/ private hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000057*/ 
            System.Collections.IEnumerable.GetEnumerator() cil managed
    // SIG: 20 00 12 81 5D
    {
      .custom /*0C0002D1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000077*/::GetEnumerator /*01000077::0A00001F*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetAllInterfaceMethods>d__9'::System.Collections.IEnumerable.GetEnumerator

    .property /*17000084*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/
            'System.Collections.Generic.IEnumerator<System.Reflection.MethodInfo>.Current'()
    {
      .get instance class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100001C*/ Microsoft.AspNetCore.SignalR.Internal.TypedClientBuilder`1/*0200005D*//'<GetAllInterfaceMethods>d__9'/*02000098*/::'System.Collections.Generic.IEnumerator<System.Reflection.MethodInfo>.get_Current'() /* 06000291 */
    } // end of property '<GetAllInterfaceMethods>d__9'::'System.Collections.Generic.IEnumerator<System.Reflection.MethodInfo>.Current'
    .property /*17000085*/ instance callconv(8) object
            System.Collections.IEnumerator.Current()
    {
      .get instance object Microsoft.AspNetCore.SignalR.Internal.TypedClientBuilder`1/*0200005D*//'<GetAllInterfaceMethods>d__9'/*02000098*/::System.Collections.IEnumerator.get_Current() /* 06000293 */
    } // end of property '<GetAllInterfaceMethods>d__9'::System.Collections.IEnumerator.Current
  } // end of class '<GetAllInterfaceMethods>d__9'

  .method /*060001D3*/ public hidebysig static 
          !T  Build(class Microsoft.AspNetCore.SignalR.IClientProxy/*02000028*/ proxy) cil managed
  // SIG: 00 01 13 00 12 80 A0
  {
    .custom /*0C000286:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedClientBuilder`1::Build

  .method /*060001D4*/ public hidebysig static 
          void  Validate() cil managed
  // SIG: 00 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedClientBuilder`1::Validate

} // end of class Microsoft.AspNetCore.SignalR.Internal.TypedClientBuilder`1

.class /*0200005E*/ private auto ansi beforefieldinit Microsoft.AspNetCore.SignalR.Internal.TypedHubClients`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<!T>/*1B000015*/,
                  class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T>/*1B000007*/
{
  .custom /*0C000163:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000164:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubCallerClients`1/*0200002C*/<!T>/*1B000015*/
  .custom /*0C0000C6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type class Microsoft.AspNetCore.SignalR.IHubClients`1/*0200002E*/<!T>/*1B000007*/
  .custom /*0C0000C9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .param type T /*2A000022*/ 
    .custom /*0C0000B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .method /*060001D5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.SignalR.IHubCallerClients/*0200002B*/ dynamicContext) cil managed
  // SIG: 20 01 01 12 80 AC
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::.ctor

  .method /*060001D6*/ public hidebysig newslot specialname virtual final 
          instance !T  get_All() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::get_All

  .method /*060001D7*/ public hidebysig newslot specialname virtual final 
          instance !T  get_Caller() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::get_Caller

  .method /*060001D8*/ public hidebysig newslot specialname virtual final 
          instance !T  get_Others() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::get_Others

  .method /*060001D9*/ public hidebysig newslot virtual final 
          instance !T  AllExcept(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 01 13 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::AllExcept

  .method /*060001DA*/ public hidebysig newslot virtual final 
          instance !T  Client(string connectionId) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::Client

  .method /*060001DB*/ public hidebysig newslot virtual final 
          instance !T  Group(string groupName) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::Group

  .method /*060001DC*/ public hidebysig newslot virtual final 
          instance !T  GroupExcept(string groupName,
                                   class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> excludedConnectionIds) cil managed
  // SIG: 20 02 13 00 0E 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::GroupExcept

  .method /*060001DD*/ public hidebysig newslot virtual final 
          instance !T  Clients(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> connectionIds) cil managed
  // SIG: 20 01 13 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::Clients

  .method /*060001DE*/ public hidebysig newslot virtual final 
          instance !T  Groups(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> groupNames) cil managed
  // SIG: 20 01 13 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::Groups

  .method /*060001DF*/ public hidebysig newslot virtual final 
          instance !T  OthersInGroup(string groupName) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::OthersInGroup

  .method /*060001E0*/ public hidebysig newslot virtual final 
          instance !T  User(string userId) cil managed
  // SIG: 20 01 13 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::User

  .method /*060001E1*/ public hidebysig newslot virtual final 
          instance !T  Users(class [System.Runtime/*23000001*/]System.Collections.Generic.IReadOnlyList`1/*0100002D*/<string> userIds) cil managed
  // SIG: 20 01 13 00 15 12 80 B5 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TypedHubClients`1::Users

  .property /*1700007C*/ instance callconv(8) !T
          All()
  {
    .get instance !T Microsoft.AspNetCore.SignalR.Internal.TypedHubClients`1/*0200005E*/::get_All() /* 060001D6 */
  } // end of property TypedHubClients`1::All
  .property /*1700007D*/ instance callconv(8) !T
          Caller()
  {
    .get instance !T Microsoft.AspNetCore.SignalR.Internal.TypedHubClients`1/*0200005E*/::get_Caller() /* 060001D7 */
  } // end of property TypedHubClients`1::Caller
  .property /*1700007E*/ instance callconv(8) !T
          Others()
  {
    .get instance !T Microsoft.AspNetCore.SignalR.Internal.TypedHubClients`1/*0200005E*/::get_Others() /* 060001D8 */
  } // end of property TypedHubClients`1::Others
} // end of class Microsoft.AspNetCore.SignalR.Internal.TypedHubClients`1

.class /*0200005F*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.SignalR.Internal.AsyncEnumerableAdapters
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000167:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000168:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000099*/ auto ansi nested private beforefieldinit ChannelAsyncEnumerator`1<T>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<object>/*1B000016*/,
                    [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100007C*/
  {
    .class /*0200009C*/ auto ansi sealed nested private beforefieldinit '<MoveNextAsyncAwaited>d__8'<T>
           extends [System.Runtime/*23000001*/]System.ValueType/*0100001A*/
           implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/
    {
      .custom /*0C0001F6:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .field /*04000158*/ public int32 '<>1__state'
      .field /*04000159*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*0100004A*/<bool> '<>t__builder'
      .field /*0400015A*/ public class Microsoft.AspNetCore.SignalR.Internal.AsyncEnumerableAdapters/*0200005F*//ChannelAsyncEnumerator`1/*02000099*/<!T> '<>4__this'
      .field /*0400015B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable`1/*0100007D*//ConfiguredValueTaskAwaiter/*0100007E*/<bool> '<>u__1'
      .method /*060002A0*/ private hidebysig newslot virtual final 
              instance void  MoveNext() cil managed
      // SIG: 20 00 01
      {
        .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::MoveNext /*01000042::0A000017*/ 
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<MoveNextAsyncAwaited>d__8'::MoveNext

      .method /*060002A1*/ private hidebysig newslot virtual final 
              instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/ stateMachine) cil managed
      // SIG: 20 01 01 12 81 09
      {
        .custom /*0C0002D4:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000045*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
        .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000042*/::SetStateMachine /*01000042::0A000018*/ 
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<MoveNextAsyncAwaited>d__8'::SetStateMachine

    } // end of class '<MoveNextAsyncAwaited>d__8'

    .method /*06000296*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Threading.Channels/*23000012*/]System.Threading.Channels.ChannelReader`1/*0100007B*/<!T> channel,
                                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
    // SIG: 20 02 01 15 12 81 ED 01 13 00 11 80 AD
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelAsyncEnumerator`1::.ctor

    .method /*06000297*/ public hidebysig newslot specialname virtual final 
            instance object  get_Current() cil managed
    // SIG: 20 00 1C
    {
      .custom /*0C0002D2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelAsyncEnumerator`1::get_Current

    .method /*06000298*/ public hidebysig newslot virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<bool> 
            MoveNextAsync() cil managed
    // SIG: 20 00 15 11 81 1D 01 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelAsyncEnumerator`1::MoveNextAsync

    .method /*06000299*/ public hidebysig newslot virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003D*/ 
            DisposeAsync() cil managed
    // SIG: 20 00 11 80 F5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ChannelAsyncEnumerator`1::DisposeAsync

    .property /*17000086*/ instance callconv(8) object
            Current()
    {
      .get instance object Microsoft.AspNetCore.SignalR.Internal.AsyncEnumerableAdapters/*0200005F*//ChannelAsyncEnumerator`1/*02000099*/::get_Current() /* 06000297 */
    } // end of property ChannelAsyncEnumerator`1::Current
  } // end of class ChannelAsyncEnumerator`1

  .class /*0200009A*/ auto ansi nested private beforefieldinit CancelableTypedAsyncEnumerable`1<TResult>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*0100007A*/<!TResult>/*1B000017*/
  {
    .class /*0200009D*/ auto ansi nested private beforefieldinit CancelableEnumerator`1<TResult,T>
           extends [System.Runtime/*23000001*/]System.Object/*01000017*/
           implements class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<!T>/*1B000018*/,
                      [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100007C*/
    {
      .method /*060002A2*/ public hidebysig newslot specialname virtual final 
              instance !T  get_Current() cil managed
      // SIG: 20 00 13 01
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method CancelableEnumerator`1::get_Current

      .method /*060002A3*/ public hidebysig specialname rtspecialname 
              instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<!T> asyncEnumerator,
                                   valuetype [System.Runtime/*23000001*/]System.Threading.CancellationTokenRegistration/*0100004B*/ registration) cil managed
      // SIG: 20 02 01 15 12 81 C5 01 13 01 11 81 2D
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method CancelableEnumerator`1::.ctor

      .method /*060002A4*/ public hidebysig newslot virtual final 
              instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<bool> 
              MoveNextAsync() cil managed
      // SIG: 20 00 15 11 81 1D 01 02
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method CancelableEnumerator`1::MoveNextAsync

      .method /*060002A5*/ public hidebysig newslot virtual final 
              instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003D*/ 
              DisposeAsync() cil managed
      // SIG: 20 00 11 80 F5
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method CancelableEnumerator`1::DisposeAsync

      .property /*17000088*/ instance callconv(8) !T
              Current()
      {
        .get instance !T Microsoft.AspNetCore.SignalR.Internal.AsyncEnumerableAdapters/*0200005F*//CancelableTypedAsyncEnumerable`1/*0200009A*//CancelableEnumerator`1/*0200009D*/::get_Current() /* 060002A2 */
      } // end of property CancelableEnumerator`1::Current
    } // end of class CancelableEnumerator`1

    .class /*0200009E*/ auto ansi serializable sealed nested private beforefieldinit '<>c'<TResult>
           extends [System.Runtime/*23000001*/]System.Object/*01000017*/
    {
      .custom /*0C0001F7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
      .field /*0400015C*/ public static initonly class Microsoft.AspNetCore.SignalR.Internal.AsyncEnumerableAdapters/*0200005F*//CancelableTypedAsyncEnumerable`1/*0200009A*//'<>c'/*0200009E*/<!TResult> '<>9'
      .field /*0400015D*/ public static class [System.Runtime/*23000001*/]System.Action`1/*01000046*/<object> '<>9__3_0'
      .method /*060002A6*/ public hidebysig specialname rtspecialname 
              instance void  .ctor() cil managed
      // SIG: 20 00 01
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c'::.ctor

      .method /*060002A7*/ assembly hidebysig 
              instance void  '<GetAsyncEnumerator>b__3_0'(object ctsState) cil managed
      // SIG: 20 01 01 1C
      {
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<>c'::'<GetAsyncEnumerator>b__3_0'

    } // end of class '<>c'

    .method /*0600029A*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*0100007A*/<!TResult> asyncEnumerable,
                                 class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100003C*/ cts) cil managed
    // SIG: 20 02 01 15 12 81 E9 01 13 00 12 80 F1
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CancelableTypedAsyncEnumerable`1::.ctor

    .method /*0600029B*/ public hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<!TResult> 
            GetAsyncEnumerator([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
    // SIG: 20 01 15 12 81 C5 01 13 00 11 80 AD
    {
      .param [1]/*080003F5*/  = nullref
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method CancelableTypedAsyncEnumerable`1::GetAsyncEnumerator

  } // end of class CancelableTypedAsyncEnumerable`1

  .class /*0200009B*/ auto ansi nested private beforefieldinit BoxedAsyncEnumerator`1<T>
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
         implements class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<object>/*1B000016*/,
                    [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100007C*/
  {
    .method /*0600029C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<!T> asyncEnumerator) cil managed
    // SIG: 20 01 01 15 12 81 C5 01 13 00
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method BoxedAsyncEnumerator`1::.ctor

    .method /*0600029D*/ public hidebysig newslot specialname virtual final 
            instance object  get_Current() cil managed
    // SIG: 20 00 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method BoxedAsyncEnumerator`1::get_Current

    .method /*0600029E*/ public hidebysig newslot virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000047*/<bool> 
            MoveNextAsync() cil managed
    // SIG: 20 00 15 11 81 1D 01 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method BoxedAsyncEnumerator`1::MoveNextAsync

    .method /*0600029F*/ public hidebysig newslot virtual final 
            instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*0100003D*/ 
            DisposeAsync() cil managed
    // SIG: 20 00 11 80 F5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method BoxedAsyncEnumerator`1::DisposeAsync

    .property /*17000087*/ instance callconv(8) object
            Current()
    {
      .get instance object Microsoft.AspNetCore.SignalR.Internal.AsyncEnumerableAdapters/*0200005F*//BoxedAsyncEnumerator`1/*0200009B*/::get_Current() /* 0600029D */
    } // end of property BoxedAsyncEnumerator`1::Current
  } // end of class BoxedAsyncEnumerator`1

  .method /*060001E2*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<object> 
          MakeCancelableAsyncEnumerator<T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*0100007A*/<!!T> asyncEnumerable,
                                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 10 01 02 15 12 81 C5 01 1C 15 12 81 E9 01 1E 00 11 80 AD
  {
    .param type T /*2A000071*/ 
      .custom /*0C000198:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000357*/ 
    .custom /*0C0002EA:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*08000359*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncEnumerableAdapters::MakeCancelableAsyncEnumerator

  .method /*060001E3*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*0100007A*/<!!T> 
          MakeCancelableTypedAsyncEnumerable<T>(class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerable`1/*0100007A*/<!!T> asyncEnumerable,
                                                class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100003C*/ cts) cil managed
  // SIG: 10 01 02 15 12 81 E9 01 1E 00 15 12 81 E9 01 1E 00 12 80 F1
  {
    .param type T /*2A000072*/ 
      .custom /*0C00019C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncEnumerableAdapters::MakeCancelableTypedAsyncEnumerable

  .method /*060001E4*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IAsyncEnumerator`1/*01000071*/<object> 
          MakeAsyncEnumeratorFromChannel<T>(class [System.Threading.Channels/*23000012*/]System.Threading.Channels.ChannelReader`1/*0100007B*/<!!T> channel,
                                            [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002B*/ cancellationToken) cil managed
  // SIG: 10 01 02 15 12 81 C5 01 1C 15 12 81 ED 01 1E 00 11 80 AD
  {
    .param type T /*2A000073*/ 
      .custom /*0C00019F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800035C*/ 
    .custom /*0C0002EB:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [2]/*0800035E*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsyncEnumerableAdapters::MakeAsyncEnumeratorFromChannel

} // end of class Microsoft.AspNetCore.SignalR.Internal.AsyncEnumerableAdapters


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
