


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
// Time-date stamp:            0xa2a221ff
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00005600
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000941e
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
// Checksum:                       0x0000fb00
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000093cc [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x000004ec] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00006400 [0x00002470] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000093b0 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00005424 Virtual Size
//              0x00004000 Virtual Address
//              0x00005600 Size of Raw Data
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
//              0x000004ec Virtual Size
//              0x0000a000 Virtual Address
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
//              0x0000c000 Virtual Address
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
//              0x00009000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000420
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x0000940e Import Name Table
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
// 0x00004054 [0x000052dc] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00009330 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00002958 Size
//              '#~' Name
//    Stream 2:
//              0x000029c4 Offset
//              0x000016b0 Size
//              '#Strings' Name
//    Stream 3:
//              0x00004074 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00004078 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00004088 Offset
//              0x00001254 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000006090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 34928
// PE header size       : 1024 (536 used)    ( 2.93%)
// PE additional info   : 10787             (30.88%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.21%)
// CLR meta-data size  : 21212              (60.73%)
// CLR additional info : 128                ( 0.37%)
// CLR method headers  : 180                ( 0.52%)
// Managed code         : 360               ( 1.03%)
// Data                 : 2560              ( 7.33%)
// Unaccounted          : -1395             (-3.99%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 22016
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 21212
//   Module        -    1 (10 bytes)
//   TypeDef       -   64 (896 bytes)     11 interfaces, 0 explicit layout
//   TypeRef       -   71 (426 bytes)
//   MethodDef     -  204 (2856 bytes)    24 abstract, 0 native, 180 bodies
//   FieldDef      -  111 (666 bytes)     4 constant
//   MemberRef     -   30 (180 bytes)
//   ParamDef      -  302 (1812 bytes)
//   MethodImpl    -   24 (144 bytes)
//   Constant      -   12 (72 bytes)
//   CustomAttribute-  357 (2142 bytes)
//   InterfaceImpl -   42 (168 bytes)
//   PropertyMap   -   18 (72 bytes)
//   Property      -   33 (198 bytes)
//   MethodSemantic-   37 (222 bytes)
//   TypeSpec      -    6 (12 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   NestedClass   -   20 (80 bytes)
//   GenericParam  -   53 (424 bytes)
//   Strings       -  5806 bytes
//   Blobs         -  4692 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   212 bytes

// CLR additional info : 128

// CLR method headers : 180
//   Num.of method bodies  - 180
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 180

// Managed code : 360
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class LinkerFlags                    (abstract) (auto) (ansi) (sealed) 
// Class DotNetObjectReference          (public) (abstract) (auto) (ansi) (sealed) 
// Class DotNetObjectReference`1        (public) (auto) (ansi) (sealed) 
// Class DotNetStreamReference          (public) (auto) (ansi) (sealed) 
// Interface IJSInProcessObjectReference    (public) (abstract) (auto) (ansi) 
// Interface IJSInProcessRuntime            (public) (abstract) (auto) (ansi) 
// Interface IJSObjectReference             (public) (abstract) (auto) (ansi) 
// Interface IJSRuntime                     (public) (abstract) (auto) (ansi) 
// Interface IJSStreamReference             (public) (abstract) (auto) (ansi) 
// Interface IJSUnmarshalledObjectReference (public) (abstract) (auto) (ansi) 
// Interface IJSUnmarshalledRuntime         (public) (abstract) (auto) (ansi) 
// Class JSCallResultType               (public) (auto) (ansi) (sealed) 
// Class JSDisconnectedException        (public) (auto) (ansi) (sealed) 
// Class JSException                    (public) (auto) (ansi) 
// Class JSInProcessObjectReferenceExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class JSInProcessRuntime             (public) (abstract) (auto) (ansi) 
// Class JSInProcessRuntimeExtensions   (public) (abstract) (auto) (ansi) (sealed) 
// Class JSInvokableAttribute           (public) (auto) (ansi) (sealed) 
// Class JSObjectReferenceExtensions    (public) (abstract) (auto) (ansi) (sealed) 
// Class JSRuntime                      (public) (abstract) (auto) (ansi) 
// Class JSRuntimeExtensions            (public) (abstract) (auto) (ansi) (sealed) 
// Class JSCallResultTypeHelper         (abstract) (auto) (ansi) (sealed) 
// Class ByteArrayJsonConverter         (auto) (ansi) (sealed) 
// Class DotNetDispatcher               (public) (abstract) (auto) (ansi) (sealed) 
// Class DotNetInvocationInfo           (public) (sequential) (ansi) (sealed) 
// Class DotNetInvocationResult         (public) (sequential) (ansi) (sealed) 
// Class DotNetObjectReferenceJsonConverter`1 (auto) (ansi) (sealed) 
// Class DotNetObjectReferenceJsonConverterFactory (auto) (ansi) (sealed) 
// Class DotNetStreamReferenceJsonConverter (auto) (ansi) (sealed) 
// Interface IDotNetObjectReference         (abstract) (auto) (ansi) 
// Interface IJSVoidResult                  (public) (abstract) (auto) (ansi) 
// Class JSObjectReferenceJsonConverter (auto) (ansi) (sealed) 
// Class JSStreamReferenceJsonConverter (auto) (ansi) (sealed) 
// Class TaskGenericsUtil               (abstract) (auto) (ansi) (sealed) 
// Class ArrayBuilder`1                 (auto) (ansi) 
// Class JSInProcessObjectReference     (public) (auto) (ansi) 
// Class JSObjectReference              (public) (auto) (ansi) 
// Class JSObjectReferenceJsonWorker    (public) (abstract) (auto) (ansi) (sealed) 
// Class JSStreamReference              (public) (auto) (ansi) (sealed) 
// Class <InvokeAsync>d__4`1            (auto) (ansi) (sealed) (nested private) 
// Class <InvokeVoidAsync>d__0          (auto) (ansi) (sealed) (nested private) 
// Class <InvokeVoidAsync>d__3          (auto) (ansi) (sealed) (nested private) 
// Class <InvokeVoidAsync>d__5          (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass17_0`1        (auto) (ansi) (sealed) (nested private) 
// Class <InvokeAsync>d__16`1           (auto) (ansi) (sealed) (nested private) 
// Class <InvokeAsync>d__4`1            (auto) (ansi) (sealed) (nested private) 
// Class <InvokeVoidAsync>d__0          (auto) (ansi) (sealed) (nested private) 
// Class <InvokeVoidAsync>d__3          (auto) (ansi) (sealed) (nested private) 
// Class <InvokeVoidAsync>d__5          (auto) (ansi) (sealed) (nested private) 
// Class AssemblyKey                    (sequential) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass5_0           (auto) (ansi) (sealed) (nested private) 
// Interface ITcsResultSetter               (abstract) (auto) (ansi) (nested private) 
// Interface ITaskResultGetter              (abstract) (auto) (ansi) (nested private) 
// Class TaskResultGetter`1             (auto) (ansi) (nested private) 
// Class VoidTaskResultGetter           (auto) (ansi) (nested private) 
// Class TcsResultSetter`1              (auto) (ansi) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__11            (auto) (ansi) (sealed) (nested private) 
// Class <Microsoft-JSInterop-IJSStreamReference-OpenReadStreamAsync>d__5 (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Runtime.Loader
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Text.Json
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.JSInterop
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
  .custom /*0C000008:0A000006*/ instance void [System.Runtime.Loader/*23000002*/]System.Reflection.Metadata.MetadataUpdateHandlerAttribute/*01000008*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000006 */ = ( 01 00 33 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..3Microsoft.JSI
                                                                                                                                                                                                                                            6E 74 65 72 6F 70 2E 49 6E 66 72 61 73 74 72 75   // nterop.Infrastru
                                                                                                                                                                                                                                            63 74 75 72 65 2E 44 6F 74 4E 65 74 44 69 73 70   // cture.DotNetDisp
                                                                                                                                                                                                                                            61 74 63 68 65 72 00 00 )                         // atcher..
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000009*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*0100000A*/::.ctor(string) /* 0A000008 */ = ( 01 00 81 65 4D 69 63 72 6F 73 6F 66 74 2E 4A 53   // ...eMicrosoft.JS
                                                                                                                                                                                    49 6E 74 65 72 6F 70 2E 54 65 73 74 73 2C 20 50   // Interop.Tests, P
                                                                                                                                                                                    75 62 6C 69 63 4B 65 79 3D 30 30 32 34 30 30 30   // ublicKey=0024000
                                                                                                                                                                                    30 30 34 38 30 30 30 30 30 39 34 30 30 30 30 30   // 0048000009400000
                                                                                                                                                                                    30 30 36 30 32 30 30 30 30 30 30 32 34 30 30 30   // 0060200000024000
                                                                                                                                                                                    30 35 32 35 33 34 31 33 31 30 30 30 34 30 30 30   // 0525341310004000
                                                                                                                                                                                    30 30 31 30 30 30 31 30 30 66 33 33 61 32 39 30   // 001000100f33a290
                                                                                                                                                                                    34 34 66 61 39 64 37 34 30 63 39 62 33 32 31 33   // 44fa9d740c9b3213
                                                                                                                                                                                    61 39 33 65 35 37 63 38 34 62 34 37 32 63 38 34   // a93e57c84b472c84
                                                                                                                                                                                    65 30 62 38 61 30 65 31 61 65 34 38 65 36 37 61   // e0b8a0e1ae48e67a
                                                                                                                                                                                    39 66 38 66 36 64 65 39 64 35 66 37 66 33 64 35   // 9f8f6de9d5f7f3d5
                                                                                                                                                                                    32 61 63 32 33 65 34 38 61 63 35 31 38 30 31 66   // 2ac23e48ac51801f
                                                                                                                                                                                    31 64 63 39 35 30 61 62 65 39 30 31 64 61 33 34   // 1dc950abe901da34
                                                                                                                                                                                    64 32 61 39 65 33 62 61 61 64 62 31 34 31 61 31   // d2a9e3baadb141a1
                                                                                                                                                                                    37 63 37 37 65 66 33 63 35 36 35 64 64 35 65 65   // 7c77ef3c565dd5ee
                                                                                                                                                                                    35 30 35 34 62 39 31 63 66 36 33 62 62 33 63 36   // 5054b91cf63bb3c6
                                                                                                                                                                                    61 62 38 33 66 37 32 61 62 33 61 61 66 65 39 33   // ab83f72ab3aafe93
                                                                                                                                                                                    64 30 66 63 33 63 32 33 34 38 62 37 36 34 66 61   // d0fc3c2348b764fa
                                                                                                                                                                                    66 62 30 62 31 63 30 37 33 33 64 65 35 31 34 35   // fb0b1c0733de5145
                                                                                                                                                                                    39 61 65 61 62 34 36 35 38 30 33 38 34 62 66 39   // 9aeab46580384bf9
                                                                                                                                                                                    64 37 34 63 34 65 32 38 31 36 34 62 37 63 64 65   // d74c4e28164b7cde
                                                                                                                                                                                    32 34 37 66 38 39 31 62 61 30 37 38 39 31 63 39   // 247f891ba07891c9
                                                                                                                                                                                    64 38 37 32 61 64 32 62 62 00 00 )                // d872ad2bb..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*0100000A*/::.ctor(string) /* 0A000008 */ = ( 01 00 81 7D 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...}Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 43 6F 6D 70 6F 6E 65   // pNetCore.Compone
                                                                                                                                                                                    6E 74 73 2E 57 65 62 41 73 73 65 6D 62 6C 79 2E   // nts.WebAssembly.
                                                                                                                                                                                    54 65 73 74 73 2C 20 50 75 62 6C 69 63 4B 65 79   // Tests, PublicKey
                                                                                                                                                                                    3D 30 30 32 34 30 30 30 30 30 34 38 30 30 30 30   // =002400000480000
                                                                                                                                                                                    30 39 34 30 30 30 30 30 30 30 36 30 32 30 30 30   // 0940000000602000
                                                                                                                                                                                    30 30 30 32 34 30 30 30 30 35 32 35 33 34 31 33   // 0002400005253413
                                                                                                                                                                                    31 30 30 30 34 30 30 30 30 30 31 30 30 30 31 30   // 1000400000100010
                                                                                                                                                                                    30 66 33 33 61 32 39 30 34 34 66 61 39 64 37 34   // 0f33a29044fa9d74
                                                                                                                                                                                    30 63 39 62 33 32 31 33 61 39 33 65 35 37 63 38   // 0c9b3213a93e57c8
                                                                                                                                                                                    34 62 34 37 32 63 38 34 65 30 62 38 61 30 65 31   // 4b472c84e0b8a0e1
                                                                                                                                                                                    61 65 34 38 65 36 37 61 39 66 38 66 36 64 65 39   // ae48e67a9f8f6de9
                                                                                                                                                                                    64 35 66 37 66 33 64 35 32 61 63 32 33 65 34 38   // d5f7f3d52ac23e48
                                                                                                                                                                                    61 63 35 31 38 30 31 66 31 64 63 39 35 30 61 62   // ac51801f1dc950ab
                                                                                                                                                                                    65 39 30 31 64 61 33 34 64 32 61 39 65 33 62 61   // e901da34d2a9e3ba
                                                                                                                                                                                    61 64 62 31 34 31 61 31 37 63 37 37 65 66 33 63   // adb141a17c77ef3c
                                                                                                                                                                                    35 36 35 64 64 35 65 65 35 30 35 34 62 39 31 63   // 565dd5ee5054b91c
                                                                                                                                                                                    66 36 33 62 62 33 63 36 61 62 38 33 66 37 32 61   // f63bb3c6ab83f72a
                                                                                                                                                                                    62 33 61 61 66 65 39 33 64 30 66 63 33 63 32 33   // b3aafe93d0fc3c23
                                                                                                                                                                                    34 38 62 37 36 34 66 61 66 62 30 62 31 63 30 37   // 48b764fafb0b1c07
                                                                                                                                                                                    33 33 64 65 35 31 34 35 39 61 65 61 62 34 36 35   // 33de51459aeab465
                                                                                                                                                                                    38 30 33 38 34 62 66 39 64 37 34 63 34 65 32 38   // 80384bf9d74c4e28
                                                                                                                                                                                    31 36 34 62 37 63 64 65 32 34 37 66 38 39 31 62   // 164b7cde247f891b
                                                                                                                                                                                    61 30 37 38 39 31 63 39 64 38 37 32 61 64 32 62   // a07891c9d872ad2b
                                                                                                                                                                                    62 00 00 )                                        // b..
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000C*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000D*/::.ctor(string) /* 0A00000B */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000E*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000F*/::.ctor(string) /* 0A00000D */ = ( 01 00 47 41 62 73 74 72 61 63 74 69 6F 6E 73 20   // ..GAbstractions 
                                                                                                                                                                       61 6E 64 20 66 65 61 74 75 72 65 73 20 66 6F 72   // and features for
                                                                                                                                                                       20 69 6E 74 65 72 6F 70 20 62 65 74 77 65 65 6E   //  interop between
                                                                                                                                                                       20 2E 4E 45 54 20 61 6E 64 20 4A 61 76 61 53 63   //  .NET and JavaSc
                                                                                                                                                                       72 69 70 74 20 63 6F 64 65 2E 00 00 )             // ript code...
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000010*/::.ctor(string) /* 0A00000E */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000011*/::.ctor(string) /* 0A00000F */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000012*/::.ctor(string) /* 0A000010 */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000017:0A000011*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000013*/::.ctor(string) /* 0A000011 */ = ( 01 00 13 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ...Microsoft.JSI
                                                                                                                                                                 6E 74 65 72 6F 70 00 00 )                         // nterop..
  .custom /*0C000018:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000019:0A000012*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000014*/::.ctor(string) /* 0A000012 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.module Microsoft.JSInterop.dll
// MVID: {5d487e95-3dc9-46f2-9961-ae91d918b762}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C00001A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C00002A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002B:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.LinkerFlags
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .field /*04000004*/ public static literal valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/ JsonSerialized = int32(0x00000223)
  .field /*04000005*/ public static literal valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/ Component = int32(0xFFFFFFFF)
} // end of class Microsoft.AspNetCore.Internal.LinkerFlags

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.DotNetObjectReference
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .method /*06000006*/ public hidebysig static 
          class Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/<!!TValue> 
          Create<class TValue>(!!TValue 'value') cil managed
  // SIG: 10 01 01 15 12 20 01 1E 00 1E 00
  {
    .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference::Create

} // end of class Microsoft.JSInterop.DotNetObjectReference

.class /*02000008*/ public auto ansi sealed beforefieldinit Microsoft.JSInterop.DotNetObjectReference`1<class TValue>
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements Microsoft.JSInterop.Infrastructure.IDotNetObjectReference/*02000023*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.JSInterop.Infrastructure.IDotNetObjectReference/*02000023*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000007*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(!TValue 'value') cil managed
  // SIG: 20 01 01 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance !TValue  get_Value() cil managed
  // SIG: 20 00 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::get_Value

  .method /*06000009*/ assembly hidebysig specialname 
          instance int64  get_ObjectId() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::get_ObjectId

  .method /*0600000A*/ assembly hidebysig specialname 
          instance void  set_ObjectId(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::set_ObjectId

  .method /*0600000B*/ assembly hidebysig specialname 
          instance class Microsoft.JSInterop.JSRuntime/*02000019*/ 
          get_JSRuntime() cil managed
  // SIG: 20 00 12 64
  {
    .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::get_JSRuntime

  .method /*0600000C*/ assembly hidebysig specialname 
          instance void  set_JSRuntime(class Microsoft.JSInterop.JSRuntime/*02000019*/ 'value') cil managed
  // SIG: 20 01 01 12 64
  {
    .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::set_JSRuntime

  .method /*0600000D*/ private hidebysig newslot specialname virtual final 
          instance object  Microsoft.JSInterop.Infrastructure.IDotNetObjectReference.get_Value() cil managed
  // SIG: 20 00 1C
  {
    .override Microsoft.JSInterop.Infrastructure.IDotNetObjectReference/*02000023*/::get_Value /*02000023::06000076*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::Microsoft.JSInterop.Infrastructure.IDotNetObjectReference.get_Value

  .method /*0600000E*/ assembly hidebysig specialname 
          instance bool  get_Disposed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000045:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::get_Disposed

  .method /*0600000F*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::Dispose

  .method /*06000010*/ assembly hidebysig 
          instance void  ThrowIfDisposed() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReference`1::ThrowIfDisposed

  .property /*17000001*/ instance callconv(8) !TValue
          Value()
  {
    .get instance !TValue Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/::get_Value() /* 06000008 */
  } // end of property DotNetObjectReference`1::Value
  .property /*17000002*/ instance callconv(8) int64
          ObjectId()
  {
    .get instance int64 Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/::get_ObjectId() /* 06000009 */
    .set instance void Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/::set_ObjectId(int64) /* 0600000A */
  } // end of property DotNetObjectReference`1::ObjectId
  .property /*17000003*/ instance callconv(8) class Microsoft.JSInterop.JSRuntime/*02000019*/
          JSRuntime()
  {
    .custom /*0C000021:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class Microsoft.JSInterop.JSRuntime/*02000019*/ Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/::get_JSRuntime() /* 0600000B */
    .set instance void Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/::set_JSRuntime(class Microsoft.JSInterop.JSRuntime/*02000019*/) /* 0600000C */
  } // end of property DotNetObjectReference`1::JSRuntime
  .property /*17000004*/ instance callconv(8) object
          Microsoft.JSInterop.Infrastructure.IDotNetObjectReference.Value()
  {
    .get instance object Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/::Microsoft.JSInterop.Infrastructure.IDotNetObjectReference.get_Value() /* 0600000D */
  } // end of property DotNetObjectReference`1::Microsoft.JSInterop.Infrastructure.IDotNetObjectReference.Value
  .property /*17000005*/ instance callconv(8) bool
          Disposed()
  {
    .get instance bool Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/::get_Disposed() /* 0600000E */
  } // end of property DotNetObjectReference`1::Disposed
} // end of class Microsoft.JSInterop.DotNetObjectReference`1

.class /*02000009*/ public auto ansi sealed beforefieldinit Microsoft.JSInterop.DotNetStreamReference
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C000036:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C000020:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 'stream',
                               [opt] bool leaveOpen) cil managed
  // SIG: 20 02 01 12 71 02
  {
    .param [2]/*08000006*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetStreamReference::.ctor

  .method /*06000012*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ 
          get_Stream() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000053:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetStreamReference::get_Stream

  .method /*06000013*/ public hidebysig specialname 
          instance bool  get_LeaveOpen() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000058:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetStreamReference::get_LeaveOpen

  .method /*06000014*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetStreamReference::Dispose

  .property /*17000006*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/
          Stream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/ Microsoft.JSInterop.DotNetStreamReference/*02000009*/::get_Stream() /* 06000012 */
  } // end of property DotNetStreamReference::Stream
  .property /*17000007*/ instance callconv(8) bool
          LeaveOpen()
  {
    .get instance bool Microsoft.JSInterop.DotNetStreamReference/*02000009*/::get_LeaveOpen() /* 06000013 */
  } // end of property DotNetStreamReference::LeaveOpen
} // end of class Microsoft.JSInterop.DotNetStreamReference

.class /*0200000A*/ interface public abstract auto ansi Microsoft.JSInterop.IJSInProcessObjectReference
       implements Microsoft.JSInterop.IJSObjectReference/*0200000C*/,
                  [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001D*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .method /*06000015*/ public hidebysig newslot abstract virtual 
          instance !!TValue  Invoke<TValue>(string identifier,
                                            object[] args) cil managed
  // SIG: 30 01 02 1E 00 0E 1D 1C
  {
    .custom /*0C000062:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000063:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 5E 4A 53 4F 4E 20 73 65 72 69 61 6C 69 7A   // ..^JSON serializ
                                                                                                                                                                                            61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72 69   // ation and deseri
                                                                                                                                                                                            61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74 20   // alization might 
                                                                                                                                                                                            72 65 71 75 69 72 65 20 74 79 70 65 73 20 74 68   // require types th
                                                                                                                                                                                            61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74 61   // at cannot be sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65 64   // tically analyzed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param type TValue /*2A000003*/ 
      .custom /*0C000022:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C000023:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [2]/*08000008*/ 
    .custom /*0C000032:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSInProcessObjectReference::Invoke

} // end of class Microsoft.JSInterop.IJSInProcessObjectReference

.class /*0200000B*/ interface public abstract auto ansi Microsoft.JSInterop.IJSInProcessRuntime
       implements Microsoft.JSInterop.IJSRuntime/*0200000D*/
{
  .method /*06000016*/ public hidebysig newslot abstract virtual 
          instance !!TResult  Invoke<TResult>(string identifier,
                                              object[] args) cil managed
  // SIG: 30 01 02 1E 00 0E 1D 1C
  {
    .custom /*0C000067:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000068:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 5E 4A 53 4F 4E 20 73 65 72 69 61 6C 69 7A   // ..^JSON serializ
                                                                                                                                                                                            61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72 69   // ation and deseri
                                                                                                                                                                                            61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74 20   // alization might 
                                                                                                                                                                                            72 65 71 75 69 72 65 20 74 79 70 65 73 20 74 68   // require types th
                                                                                                                                                                                            61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74 61   // at cannot be sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65 64   // tically analyzed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param type TResult /*2A000004*/ 
      .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C000028:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [2]/*0800000A*/ 
    .custom /*0C000039:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSInProcessRuntime::Invoke

} // end of class Microsoft.JSInterop.IJSInProcessRuntime

.class /*0200000C*/ interface public abstract auto ansi Microsoft.JSInterop.IJSObjectReference
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001D*/
{
  .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001D*/
  .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000017*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(string identifier,
                              object[] args) cil managed
  // SIG: 30 01 02 15 11 80 85 01 1E 00 0E 1D 1C
  {
    .param type TValue /*2A000005*/ 
      .custom /*0C00002C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*0800000B*/ 
    .custom /*0C00003D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800000C*/ 
    .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSObjectReference::InvokeAsync

  .method /*06000018*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(string identifier,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                              object[] args) cil managed
  // SIG: 30 01 03 15 11 80 85 01 1E 00 0E 11 80 89 1D 1C
  {
    .param type TValue /*2A000006*/ 
      .custom /*0C00002E:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*0800000E*/ 
    .custom /*0C000047:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800000F*/ 
    .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSObjectReference::InvokeAsync

} // end of class Microsoft.JSInterop.IJSObjectReference

.class /*0200000D*/ interface public abstract auto ansi Microsoft.JSInterop.IJSRuntime
{
  .custom /*0C000043:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .method /*06000019*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(string identifier,
                              object[] args) cil managed
  // SIG: 30 01 02 15 11 80 85 01 1E 00 0E 1D 1C
  {
    .param type TValue /*2A000007*/ 
      .custom /*0C00002F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000012*/ 
    .custom /*0C000055:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000013*/ 
    .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSRuntime::InvokeAsync

  .method /*0600001A*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(string identifier,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                              object[] args) cil managed
  // SIG: 30 01 03 15 11 80 85 01 1E 00 0E 11 80 89 1D 1C
  {
    .param type TValue /*2A000008*/ 
      .custom /*0C000035:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000015*/ 
    .custom /*0C000065:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000016*/ 
    .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSRuntime::InvokeAsync

} // end of class Microsoft.JSInterop.IJSRuntime

.class /*0200000E*/ interface public abstract auto ansi Microsoft.JSInterop.IJSStreamReference
       implements [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001D*/
{
  .method /*0600001B*/ public hidebysig newslot specialname abstract virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x0
  } // end of method IJSStreamReference::get_Length

  .method /*0600001C*/ public hidebysig newslot abstract virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          OpenReadStreamAsync([opt] int64 maxAllowedSize,
                              [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 85 01 12 71 0A 11 80 89
  {
    .param [0]/*08000019*/ 
    .custom /*0C000076:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*0800001A*/  = int64(0x7D000)
    .param [2]/*0800001B*/  = nullref
    // Method begins at RVA 0x0
  } // end of method IJSStreamReference::OpenReadStreamAsync

  .property /*17000008*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 Microsoft.JSInterop.IJSStreamReference/*0200000E*/::get_Length() /* 0600001B */
  } // end of property IJSStreamReference::Length
} // end of class Microsoft.JSInterop.IJSStreamReference

.class /*0200000F*/ interface public abstract auto ansi Microsoft.JSInterop.IJSUnmarshalledObjectReference
       implements Microsoft.JSInterop.IJSInProcessObjectReference/*0200000A*/,
                  Microsoft.JSInterop.IJSObjectReference/*0200000C*/,
                  [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001D*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .method /*0600001D*/ public hidebysig newslot abstract virtual 
          instance !!TResult  InvokeUnmarshalled<TResult>(string identifier) cil managed
  // SIG: 30 01 01 1E 00 0E
  {
    .custom /*0C000080:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param type TResult /*2A000009*/ 
      .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSUnmarshalledObjectReference::InvokeUnmarshalled

  .method /*0600001E*/ public hidebysig newslot abstract virtual 
          instance !!TResult  InvokeUnmarshalled<T0,TResult>(string identifier,
                                                             !!T0 arg0) cil managed
  // SIG: 30 02 02 1E 01 0E 1E 00
  {
    .custom /*0C000085:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param type T0 /*2A00000A*/ 
      .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00000B*/ 
      .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSUnmarshalledObjectReference::InvokeUnmarshalled

  .method /*0600001F*/ public hidebysig newslot abstract virtual 
          instance !!TResult  InvokeUnmarshalled<T0,T1,TResult>(string identifier,
                                                                !!T0 arg0,
                                                                !!T1 arg1) cil managed
  // SIG: 30 03 03 1E 02 0E 1E 00 1E 01
  {
    .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param type T0 /*2A00000C*/ 
      .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T1 /*2A00000D*/ 
      .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00000E*/ 
      .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSUnmarshalledObjectReference::InvokeUnmarshalled

  .method /*06000020*/ public hidebysig newslot abstract virtual 
          instance !!TResult  InvokeUnmarshalled<T0,T1,T2,TResult>(string identifier,
                                                                   !!T0 arg0,
                                                                   !!T1 arg1,
                                                                   !!T2 arg2) cil managed
  // SIG: 30 04 04 1E 03 0E 1E 00 1E 01 1E 02
  {
    .custom /*0C000090:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param type T0 /*2A000010*/ 
      .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T1 /*2A000011*/ 
      .custom /*0C000052:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A000012*/ 
      .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000013*/ 
      .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSUnmarshalledObjectReference::InvokeUnmarshalled

} // end of class Microsoft.JSInterop.IJSUnmarshalledObjectReference

.class /*02000010*/ interface public abstract auto ansi Microsoft.JSInterop.IJSUnmarshalledRuntime
{
  .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000021*/ public hidebysig newslot abstract virtual 
          instance !!TResult  InvokeUnmarshalled<TResult>(string identifier) cil managed
  // SIG: 30 01 01 1E 00 0E
  {
    .param type TResult /*2A000014*/ 
      .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSUnmarshalledRuntime::InvokeUnmarshalled

  .method /*06000022*/ public hidebysig newslot abstract virtual 
          instance !!TResult  InvokeUnmarshalled<T0,TResult>(string identifier,
                                                             !!T0 arg0) cil managed
  // SIG: 30 02 02 1E 01 0E 1E 00
  {
    .param type T0 /*2A000015*/ 
      .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000016*/ 
      .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSUnmarshalledRuntime::InvokeUnmarshalled

  .method /*06000023*/ public hidebysig newslot abstract virtual 
          instance !!TResult  InvokeUnmarshalled<T0,T1,TResult>(string identifier,
                                                                !!T0 arg0,
                                                                !!T1 arg1) cil managed
  // SIG: 30 03 03 1E 02 0E 1E 00 1E 01
  {
    .param type T0 /*2A000017*/ 
      .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T1 /*2A000018*/ 
      .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A000019*/ 
      .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSUnmarshalledRuntime::InvokeUnmarshalled

  .method /*06000024*/ public hidebysig newslot abstract virtual 
          instance !!TResult  InvokeUnmarshalled<T0,T1,T2,TResult>(string identifier,
                                                                   !!T0 arg0,
                                                                   !!T1 arg1,
                                                                   !!T2 arg2) cil managed
  // SIG: 30 04 04 1E 03 0E 1E 00 1E 01 1E 02
  {
    .param type T0 /*2A00001A*/ 
      .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T1 /*2A00001B*/ 
      .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type T2 /*2A00001C*/ 
      .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param type TResult /*2A00001D*/ 
      .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IJSUnmarshalledRuntime::InvokeUnmarshalled

} // end of class Microsoft.JSInterop.IJSUnmarshalledRuntime

.class /*02000011*/ public auto ansi sealed Microsoft.JSInterop.JSCallResultType
       extends [System.Runtime/*23000001*/]System.Enum/*01000023*/
{
  .field /*04000006*/ public specialname rtspecialname int32 value__
  .field /*04000007*/ public static literal valuetype Microsoft.JSInterop.JSCallResultType/*02000011*/ Default = int32(0x00000000)
  .field /*04000008*/ public static literal valuetype Microsoft.JSInterop.JSCallResultType/*02000011*/ JSObjectReference = int32(0x00000001)
  .field /*04000009*/ public static literal valuetype Microsoft.JSInterop.JSCallResultType/*02000011*/ JSStreamReference = int32(0x00000002)
  .field /*0400000A*/ public static literal valuetype Microsoft.JSInterop.JSCallResultType/*02000011*/ JSVoidResult = int32(0x00000003)
} // end of class Microsoft.JSInterop.JSCallResultType

.class /*02000012*/ public auto ansi sealed beforefieldinit Microsoft.JSInterop.JSDisconnectedException
       extends [System.Runtime/*23000001*/]System.Exception/*01000024*/
{
  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00009B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSDisconnectedException::.ctor

} // end of class Microsoft.JSInterop.JSDisconnectedException

.class /*02000013*/ public auto ansi beforefieldinit Microsoft.JSInterop.JSException
       extends [System.Runtime/*23000001*/]System.Exception/*01000024*/
{
  .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000026*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSException::.ctor

  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000024*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 91
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSException::.ctor

} // end of class Microsoft.JSInterop.JSException

.class /*02000014*/ public abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.JSInProcessObjectReferenceExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000060:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000028*/ public hidebysig static 
          void  InvokeVoid(class Microsoft.JSInterop.IJSInProcessObjectReference/*0200000A*/ jsObjectReference,
                           string identifier,
                           object[] args) cil managed
  // SIG: 00 03 01 12 28 0E 1D 1C
  {
    .custom /*0C0000A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000A9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000AA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessageAttribute/*01000025*/::.ctor(string,
                                                                                                                                                                     string) /* 0A000018 */ = ( 01 00 12 52 65 66 6C 65 63 74 69 6F 6E 41 6E 61   // ...ReflectionAna
                                                                                                                                                                                                6C 79 73 69 73 1F 49 4C 32 30 32 36 3A 52 65 71   // lysis.IL2026:Req
                                                                                                                                                                                                75 69 72 65 73 55 6E 72 65 66 65 72 65 6E 63 65   // uiresUnreference
                                                                                                                                                                                                64 43 6F 64 65 01 00 54 0E 0D 4A 75 73 74 69 66   // dCode..T..Justif
                                                                                                                                                                                                69 63 61 74 69 6F 6E 34 54 68 65 20 6D 65 74 68   // ication4The meth
                                                                                                                                                                                                6F 64 20 72 65 74 75 72 6E 73 20 76 6F 69 64 2C   // od returns void,
                                                                                                                                                                                                20 73 6F 20 6E 6F 74 68 69 6E 67 20 69 73 20 64   //  so nothing is d
                                                                                                                                                                                                65 73 65 72 69 61 6C 69 7A 65 64 2E )             // eserialized.
    .param [3]/*08000036*/ 
    .custom /*0C0000D8:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C0000D9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessObjectReferenceExtensions::InvokeVoid

} // end of class Microsoft.JSInterop.JSInProcessObjectReferenceExtensions

.class /*02000015*/ public abstract auto ansi beforefieldinit Microsoft.JSInterop.JSInProcessRuntime
       extends Microsoft.JSInterop.JSRuntime/*02000019*/
       implements Microsoft.JSInterop.IJSInProcessRuntime/*0200000B*/,
                  Microsoft.JSInterop.IJSRuntime/*0200000D*/
{
  .method /*06000029*/ assembly hidebysig 
          instance !!TValue  Invoke<TValue>(string identifier,
                                            int64 targetInstanceId,
                                            object[] args) cil managed
  // SIG: 30 01 03 1E 00 0E 0A 1D 1C
  {
    .custom /*0C0000AD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000AE:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 5E 4A 53 4F 4E 20 73 65 72 69 61 6C 69 7A   // ..^JSON serializ
                                                                                                                                                                                            61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72 69   // ation and deseri
                                                                                                                                                                                            61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74 20   // alization might 
                                                                                                                                                                                            72 65 71 75 69 72 65 20 74 79 70 65 73 20 74 68   // require types th
                                                                                                                                                                                            61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74 61   // at cannot be sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65 64   // tically analyzed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param type TValue /*2A00001F*/ 
      .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C00008F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [3]/*08000039*/ 
    .custom /*0C0000DF:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessRuntime::Invoke

  .method /*0600002A*/ public hidebysig newslot virtual final 
          instance !!TValue  Invoke<TValue>(string identifier,
                                            object[] args) cil managed
  // SIG: 30 01 02 1E 00 0E 1D 1C
  {
    .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000B1:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 5E 4A 53 4F 4E 20 73 65 72 69 61 6C 69 7A   // ..^JSON serializ
                                                                                                                                                                                            61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72 69   // ation and deseri
                                                                                                                                                                                            61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74 20   // alization might 
                                                                                                                                                                                            72 65 71 75 69 72 65 20 74 79 70 65 73 20 74 68   // require types th
                                                                                                                                                                                            61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74 61   // at cannot be sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65 64   // tically analyzed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param type TValue /*2A000020*/ 
      .custom /*0C000093:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C000094:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [2]/*0800003B*/ 
    .custom /*0C0000E3:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C0000E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessRuntime::Invoke

  .method /*0600002B*/ family hidebysig newslot virtual 
          instance string  InvokeJS(string identifier,
                                    string argsJson) cil managed
  // SIG: 20 02 0E 0E 0E
  {
    .custom /*0C0000B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800003C*/ 
    .custom /*0C0000E6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessRuntime::InvokeJS

  .method /*0600002C*/ family hidebysig newslot abstract virtual 
          instance string  InvokeJS(string identifier,
                                    string argsJson,
                                    valuetype Microsoft.JSInterop.JSCallResultType/*02000011*/ resultType,
                                    int64 targetInstanceId) cil managed
  // SIG: 20 04 0E 0E 0E 11 44 0A
  {
    .custom /*0C0000B4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800003E*/ 
    .custom /*0C0000EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method JSInProcessRuntime::InvokeJS

  .method /*0600002D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessRuntime::.ctor

} // end of class Microsoft.JSInterop.JSInProcessRuntime

.class /*02000016*/ public abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.JSInProcessRuntimeExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000069:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600002E*/ public hidebysig static 
          void  InvokeVoid(class Microsoft.JSInterop.IJSInProcessRuntime/*0200000B*/ jsRuntime,
                           string identifier,
                           object[] args) cil managed
  // SIG: 00 03 01 12 2C 0E 1D 1C
  {
    .custom /*0C0000B8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000B9:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .custom /*0C0000BA:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessageAttribute/*01000025*/::.ctor(string,
                                                                                                                                                                     string) /* 0A000018 */ = ( 01 00 12 52 65 66 6C 65 63 74 69 6F 6E 41 6E 61   // ...ReflectionAna
                                                                                                                                                                                                6C 79 73 69 73 1F 49 4C 32 30 32 36 3A 52 65 71   // lysis.IL2026:Req
                                                                                                                                                                                                75 69 72 65 73 55 6E 72 65 66 65 72 65 6E 63 65   // uiresUnreference
                                                                                                                                                                                                64 43 6F 64 65 01 00 54 0E 0D 4A 75 73 74 69 66   // dCode..T..Justif
                                                                                                                                                                                                69 63 61 74 69 6F 6E 34 54 68 65 20 6D 65 74 68   // ication4The meth
                                                                                                                                                                                                6F 64 20 72 65 74 75 72 6E 73 20 76 6F 69 64 2C   // od returns void,
                                                                                                                                                                                                20 73 6F 20 6E 6F 74 68 69 6E 67 20 69 73 20 64   //  so nothing is d
                                                                                                                                                                                                65 73 65 72 69 61 6C 69 7A 65 64 2E )             // eserialized.
    .param [3]/*08000044*/ 
    .custom /*0C0000EF:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessRuntimeExtensions::InvokeVoid

} // end of class Microsoft.JSInterop.JSInProcessRuntimeExtensions

.class /*02000017*/ public auto ansi sealed beforefieldinit Microsoft.JSInterop.JSInvokableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C00006C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00006D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00006E:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 40 00 00 00 01 00 54 02 0D 41 6C 6C 6F 77   // ..@.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 01 )                      // Multiple.
  .method /*0600002F*/ public hidebysig specialname 
          instance string  get_Identifier() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000BC:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInvokableAttribute::get_Identifier

  .method /*06000030*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInvokableAttribute::.ctor

  .method /*06000031*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string identifier) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInvokableAttribute::.ctor

  .property /*17000009*/ instance callconv(8) string
          Identifier()
  {
    .get instance string Microsoft.JSInterop.JSInvokableAttribute/*02000017*/::get_Identifier() /* 0600002F */
  } // end of property JSInvokableAttribute::Identifier
} // end of class Microsoft.JSInterop.JSInvokableAttribute

.class /*02000018*/ public abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.JSObjectReferenceExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000070:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000072:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200002D*/ auto ansi sealed nested private beforefieldinit '<InvokeAsync>d__4`1'<TValue>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000B6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000019*/ public int32 '<>1__state'
    .field /*0400001A*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100002B*/<!TValue> '<>t__builder'
    .field /*0400001B*/ public class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference
    .field /*0400001C*/ public valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/ timeout
    .field /*0400001D*/ public string identifier
    .field /*0400001E*/ public object[] args
    .field /*0400001F*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100002C*/ '<cancellationTokenSource>5__2'
    .field /*04000020*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<!TValue> '<>u__1'
    .method /*0600009E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeAsync>d__4`1'::MoveNext

    .method /*0600009F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C000144:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeAsync>d__4`1'::SetStateMachine

  } // end of class '<InvokeAsync>d__4`1'

  .class /*0200002E*/ auto ansi sealed nested private beforefieldinit '<InvokeVoidAsync>d__0'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000BB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000021*/ public int32 '<>1__state'
    .field /*04000022*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*0100002F*/ '<>t__builder'
    .field /*04000023*/ public class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference
    .field /*04000024*/ public string identifier
    .field /*04000025*/ public object[] args
    .field /*04000026*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<class Microsoft.JSInterop.Infrastructure.IJSVoidResult/*02000024*/> '<>u__1'
    .method /*060000A0*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__0'::MoveNext

    .method /*060000A1*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C000145:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__0'::SetStateMachine

  } // end of class '<InvokeVoidAsync>d__0'

  .class /*0200002F*/ auto ansi sealed nested private beforefieldinit '<InvokeVoidAsync>d__3'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000BD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000027*/ public int32 '<>1__state'
    .field /*04000028*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*0100002F*/ '<>t__builder'
    .field /*04000029*/ public class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference
    .field /*0400002A*/ public string identifier
    .field /*0400002B*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken
    .field /*0400002C*/ public object[] args
    .field /*0400002D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<class Microsoft.JSInterop.Infrastructure.IJSVoidResult/*02000024*/> '<>u__1'
    .method /*060000A2*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__3'::MoveNext

    .method /*060000A3*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C000148:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__3'::SetStateMachine

  } // end of class '<InvokeVoidAsync>d__3'

  .class /*02000030*/ auto ansi sealed nested private beforefieldinit '<InvokeVoidAsync>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000C0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*0400002E*/ public int32 '<>1__state'
    .field /*0400002F*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*0100002F*/ '<>t__builder'
    .field /*04000030*/ public class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference
    .field /*04000031*/ public valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/ timeout
    .field /*04000032*/ public string identifier
    .field /*04000033*/ public object[] args
    .field /*04000034*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100002C*/ '<cancellationTokenSource>5__2'
    .field /*04000035*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<class Microsoft.JSInterop.Infrastructure.IJSVoidResult/*02000024*/> '<>u__1'
    .method /*060000A4*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__5'::MoveNext

    .method /*060000A5*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C00014A:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__5'::SetStateMachine

  } // end of class '<InvokeVoidAsync>d__5'

  .method /*06000032*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000027*/ 
          InvokeVoidAsync(class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference,
                          string identifier,
                          object[] args) cil managed
  // SIG: 00 03 11 80 9D 12 30 0E 1D 1C
  {
    .custom /*0C0000C7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 45 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..EMicrosoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 4F 62 6A 65 63 74 52   // nterop.JSObjectR
                                                                                                                                                                                                                                        65 66 65 72 65 6E 63 65 45 78 74 65 6E 73 69 6F   // eferenceExtensio
                                                                                                                                                                                                                                        6E 73 2B 3C 49 6E 76 6F 6B 65 56 6F 69 64 41 73   // ns+<InvokeVoidAs
                                                                                                                                                                                                                                        79 6E 63 3E 64 5F 5F 30 00 00 )                   // ync>d__0..
    .custom /*0C0000C8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000048*/ 
    .custom /*0C0000F2:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceExtensions::InvokeVoidAsync

  .method /*06000033*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference,
                              string identifier,
                              object[] args) cil managed
  // SIG: 10 01 03 15 11 80 85 01 1E 00 12 30 0E 1D 1C
  {
    .custom /*0C0000CA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000025*/ 
      .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C00009F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000049*/ 
    .custom /*0C0000F4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*0800004C*/ 
    .custom /*0C0000F5:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C0000F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceExtensions::InvokeAsync

  .method /*06000034*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference,
                              string identifier,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                              object[] args) cil managed
  // SIG: 10 01 04 15 11 80 85 01 1E 00 12 30 0E 11 80 89 1D 1C
  {
    .custom /*0C0000CE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000026*/ 
      .custom /*0C0000A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C0000A3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*0800004D*/ 
    .custom /*0C0000F9:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [4]/*08000051*/ 
    .custom /*0C000100:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000101:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceExtensions::InvokeAsync

  .method /*06000035*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000027*/ 
          InvokeVoidAsync(class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference,
                          string identifier,
                          valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                          object[] args) cil managed
  // SIG: 00 04 11 80 9D 12 30 0E 11 80 89 1D 1C
  {
    .custom /*0C0000D1:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 45 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..EMicrosoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 4F 62 6A 65 63 74 52   // nterop.JSObjectR
                                                                                                                                                                                                                                        65 66 65 72 65 6E 63 65 45 78 74 65 6E 73 69 6F   // eferenceExtensio
                                                                                                                                                                                                                                        6E 73 2B 3C 49 6E 76 6F 6B 65 56 6F 69 64 41 73   // ns+<InvokeVoidAs
                                                                                                                                                                                                                                        79 6E 63 3E 64 5F 5F 33 00 00 )                   // ync>d__3..
    .custom /*0C0000D2:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*08000055*/ 
    .custom /*0C000106:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000107:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceExtensions::InvokeVoidAsync

  .method /*06000036*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference,
                              string identifier,
                              valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/ timeout,
                              object[] args) cil managed
  // SIG: 10 01 04 15 11 80 85 01 1E 00 12 30 0E 11 80 A1 1D 1C
  {
    .custom /*0C0000D5:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 43 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..CMicrosoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 4F 62 6A 65 63 74 52   // nterop.JSObjectR
                                                                                                                                                                                                                                        65 66 65 72 65 6E 63 65 45 78 74 65 6E 73 69 6F   // eferenceExtensio
                                                                                                                                                                                                                                        6E 73 2B 3C 49 6E 76 6F 6B 65 41 73 79 6E 63 3E   // ns+<InvokeAsync>
                                                                                                                                                                                                                                        64 5F 5F 34 60 31 00 00 )                         // d__4`1..
    .custom /*0C0000D6:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000027*/ 
      .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C0000A7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000056*/ 
    .custom /*0C000108:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [4]/*0800005A*/ 
    .custom /*0C000109:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C00010A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceExtensions::InvokeAsync

  .method /*06000037*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000027*/ 
          InvokeVoidAsync(class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ jsObjectReference,
                          string identifier,
                          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/ timeout,
                          object[] args) cil managed
  // SIG: 00 04 11 80 9D 12 30 0E 11 80 A1 1D 1C
  {
    .custom /*0C0000DA:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 45 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..EMicrosoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 4F 62 6A 65 63 74 52   // nterop.JSObjectR
                                                                                                                                                                                                                                        65 66 65 72 65 6E 63 65 45 78 74 65 6E 73 69 6F   // eferenceExtensio
                                                                                                                                                                                                                                        6E 73 2B 3C 49 6E 76 6F 6B 65 56 6F 69 64 41 73   // ns+<InvokeVoidAs
                                                                                                                                                                                                                                        79 6E 63 3E 64 5F 5F 35 00 00 )                   // ync>d__5..
    .custom /*0C0000DB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*0800005E*/ 
    .custom /*0C00010D:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C00010E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceExtensions::InvokeVoidAsync

} // end of class Microsoft.JSInterop.JSObjectReferenceExtensions

.class /*02000019*/ public abstract auto ansi beforefieldinit Microsoft.JSInterop.JSRuntime
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements Microsoft.JSInterop.IJSRuntime/*0200000D*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C000074:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.JSInterop.IJSRuntime/*0200000D*/
  .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000031*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass17_0`1'<TValue>
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
  {
    .custom /*0C0000C4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000036*/ public class [System.Runtime/*23000001*/]System.Threading.Tasks.TaskCompletionSource`1/*01000037*/<!TValue> tcs
    .field /*04000037*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken
    .field /*04000038*/ public class Microsoft.JSInterop.JSRuntime/*02000019*/ '<>4__this'
    .field /*04000039*/ public int64 taskId
    .method /*060000A6*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass17_0`1'::.ctor

    .method /*060000A7*/ assembly hidebysig 
            instance void  '<InvokeAsync>b__0'() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass17_0`1'::'<InvokeAsync>b__0'

  } // end of class '<>c__DisplayClass17_0`1'

  .class /*02000032*/ auto ansi sealed nested private beforefieldinit '<InvokeAsync>d__16`1'<TValue>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000C9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*0400003A*/ public int32 '<>1__state'
    .field /*0400003B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100002B*/<!TValue> '<>t__builder'
    .field /*0400003C*/ public class Microsoft.JSInterop.JSRuntime/*02000019*/ '<>4__this'
    .field /*0400003D*/ public int64 targetInstanceId
    .field /*0400003E*/ public string identifier
    .field /*0400003F*/ public object[] args
    .field /*04000040*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100002C*/ '<cts>5__2'
    .field /*04000041*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<!TValue> '<>u__1'
    .method /*060000A8*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeAsync>d__16`1'::MoveNext

    .method /*060000A9*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C00014B:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeAsync>d__16`1'::SetStateMachine

  } // end of class '<InvokeAsync>d__16`1'

  .field /*0400000B*/ assembly initonly class Microsoft.JSInterop.Infrastructure.ArrayBuilder`1/*02000028*/<uint8[]> ByteArraysToBeRevived
  .method /*06000038*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::.ctor

  .method /*06000039*/ famorassem hidebysig specialname 
          instance class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ 
          get_JsonSerializerOptions() cil managed
  // SIG: 20 00 12 80 C1
  {
    .custom /*0C0000DE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::get_JsonSerializerOptions

  .method /*0600003A*/ family hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/> 
          get_DefaultAsyncTimeout() cil managed
  // SIG: 20 00 15 11 80 C5 01 11 80 A1
  {
    .custom /*0C0000E1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::get_DefaultAsyncTimeout

  .method /*0600003B*/ family hidebysig specialname 
          instance void  set_DefaultAsyncTimeout(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 C5 01 11 80 A1
  {
    .custom /*0C0000E2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::set_DefaultAsyncTimeout

  .method /*0600003C*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(string identifier,
                              object[] args) cil managed
  // SIG: 30 01 02 15 11 80 85 01 1E 00 0E 1D 1C
  {
    .custom /*0C0000E5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param type TValue /*2A000029*/ 
      .custom /*0C0000AF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000060*/ 
    .custom /*0C000112:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000061*/ 
    .custom /*0C000115:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::InvokeAsync

  .method /*0600003D*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(string identifier,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                              object[] args) cil managed
  // SIG: 30 01 03 15 11 80 85 01 1E 00 0E 11 80 89 1D 1C
  {
    .custom /*0C0000E7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param type TValue /*2A00002B*/ 
      .custom /*0C0000B3:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000063*/ 
    .custom /*0C000118:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000064*/ 
    .custom /*0C00011A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::InvokeAsync

  .method /*0600003E*/ assembly hidebysig 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(int64 targetInstanceId,
                              string identifier,
                              object[] args) cil managed
  // SIG: 30 01 03 15 11 80 85 01 1E 00 0A 0E 1D 1C
  {
    .custom /*0C0000E8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000E9:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 32 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..2Microsoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 52 75 6E 74 69 6D 65   // nterop.JSRuntime
                                                                                                                                                                                                                                        2B 3C 49 6E 76 6F 6B 65 41 73 79 6E 63 3E 64 5F   // +<InvokeAsync>d_
                                                                                                                                                                                                                                        5F 31 36 60 31 00 00 )                            // _16`1..
    .param type TValue /*2A00002C*/ 
      .custom /*0C0000B5:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000067*/ 
    .custom /*0C00011D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*08000069*/ 
    .custom /*0C000120:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::InvokeAsync

  .method /*0600003F*/ assembly hidebysig 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(int64 targetInstanceId,
                              string identifier,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                              object[] args) cil managed
  // SIG: 30 01 04 15 11 80 85 01 1E 00 0A 0E 11 80 89 1D 1C
  {
    .custom /*0C0000EC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param type TValue /*2A00002D*/ 
      .custom /*0C0000B7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*0800006B*/ 
    .custom /*0C000122:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [2]/*0800006D*/ 
    .custom /*0C000123:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::InvokeAsync

  .method /*06000040*/ family hidebysig newslot virtual 
          instance void  BeginInvokeJS(int64 taskId,
                                       string identifier,
                                       string argsJson) cil managed
  // SIG: 20 03 01 0A 0E 0E
  {
    .param [3]/*08000072*/ 
    .custom /*0C000125:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::BeginInvokeJS

  .method /*06000041*/ family hidebysig newslot abstract virtual 
          instance void  BeginInvokeJS(int64 taskId,
                                       string identifier,
                                       string argsJson,
                                       valuetype Microsoft.JSInterop.JSCallResultType/*02000011*/ resultType,
                                       int64 targetInstanceId) cil managed
  // SIG: 20 05 01 0A 0E 0E 11 44 0A
  {
    .param [3]/*08000075*/ 
    .custom /*0C000127:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method JSRuntime::BeginInvokeJS

  .method /*06000042*/ famorassem hidebysig newslot abstract virtual 
          instance void  EndInvokeDotNet(valuetype Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo/*0200001E*/ invocationInfo,
                                         [in] valuetype Microsoft.JSInterop.Infrastructure.DotNetInvocationResult/*0200001F*/& modreq([System.Runtime/*23000001*/]System.Runtime.InteropServices.InAttribute/*01000033*/) invocationResult) cil managed
  // SIG: 20 02 01 11 78 1F 80 CD 10 11 7C
  {
    .param [2]/*08000079*/ 
    .custom /*0C000128:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000032*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method JSRuntime::EndInvokeDotNet

  .method /*06000043*/ famorassem hidebysig newslot virtual 
          instance void  SendByteArray(int32 id,
                                       uint8[] data) cil managed
  // SIG: 20 02 01 08 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::SendByteArray

  .method /*06000044*/ famorassem hidebysig newslot virtual 
          instance void  ReceiveByteArray(int32 id,
                                          uint8[] data) cil managed
  // SIG: 20 02 01 08 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::ReceiveByteArray

  .method /*06000045*/ famorassem hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000034*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          ReadJSDataAsStreamAsync(class Microsoft.JSInterop.IJSStreamReference/*0200000E*/ jsStreamReference,
                                  int64 totalLength,
                                  [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken) cil managed
  // SIG: 20 03 15 12 80 D1 01 12 71 12 38 0A 11 80 89
  {
    .param [3]/*08000080*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::ReadJSDataAsStreamAsync

  .method /*06000046*/ assembly hidebysig 
          instance bool  EndInvokeJS(int64 taskId,
                                     bool succeeded,
                                     valuetype [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonReader/*01000035*/& jsonReader) cil managed
  // SIG: 20 03 02 0A 02 10 11 80 D5
  {
    .custom /*0C0000F1:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessageAttribute/*01000025*/::.ctor(string,
                                                                                                                                                                     string) /* 0A000018 */ = ( 01 00 12 52 65 66 6C 65 63 74 69 6F 6E 41 6E 61   // ...ReflectionAna
                                                                                                                                                                                                6C 79 73 69 73 1F 49 4C 32 30 37 32 3A 52 65 71   // lysis.IL2072:Req
                                                                                                                                                                                                75 69 72 65 73 55 6E 72 65 66 65 72 65 6E 63 65   // uiresUnreference
                                                                                                                                                                                                64 43 6F 64 65 01 00 54 0E 0D 4A 75 73 74 69 66   // dCode..T..Justif
                                                                                                                                                                                                69 63 61 74 69 6F 6E 49 57 65 20 65 6E 66 6F 72   // icationIWe enfor
                                                                                                                                                                                                63 65 20 74 72 69 6D 6D 65 72 20 61 74 74 72 69   // ce trimmer attri
                                                                                                                                                                                                62 75 74 65 73 20 66 6F 72 20 4A 53 4F 4E 20 64   // butes for JSON d
                                                                                                                                                                                                65 73 65 72 69 61 6C 69 7A 65 64 20 74 79 70 65   // eserialized type
                                                                                                                                                                                                73 20 6F 6E 20 49 6E 76 6F 6B 65 41 73 79 6E 63   // s on InvokeAsync
                                                                                                                                                                                                2E )                                              // .
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::EndInvokeJS

  .method /*06000047*/ famorassem hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000036*/ 
          TransmitStreamAsync(int64 streamId,
                              class Microsoft.JSInterop.DotNetStreamReference/*02000009*/ dotNetStreamReference) cil managed
  // SIG: 20 02 12 80 D9 0A 12 24
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::TransmitStreamAsync

  .method /*06000048*/ assembly hidebysig 
          instance int64  BeginTransmittingStream(class Microsoft.JSInterop.DotNetStreamReference/*02000009*/ dotNetStreamReference) cil managed
  // SIG: 20 01 0A 12 24
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::BeginTransmittingStream

  .method /*06000049*/ assembly hidebysig 
          instance int64  TrackObjectReference<class TValue>(class Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/<!!TValue> dotNetObjectReference) cil managed
  // SIG: 30 01 01 0A 15 12 20 01 1E 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::TrackObjectReference

  .method /*0600004A*/ assembly hidebysig 
          instance class Microsoft.JSInterop.Infrastructure.IDotNetObjectReference/*02000023*/ 
          GetObjectReference(int64 dotNetObjectId) cil managed
  // SIG: 20 01 12 80 8C 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::GetObjectReference

  .method /*0600004B*/ assembly hidebysig 
          instance void  ReleaseObjectReference(int64 dotNetObjectId) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::ReleaseObjectReference

  .method /*0600004C*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntime::Dispose

  .property /*1700000A*/ instance callconv(8) class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/
          JsonSerializerOptions()
  {
    .get instance class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ Microsoft.JSInterop.JSRuntime/*02000019*/::get_JsonSerializerOptions() /* 06000039 */
  } // end of property JSRuntime::JsonSerializerOptions
  .property /*1700000B*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/>
          DefaultAsyncTimeout()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/> Microsoft.JSInterop.JSRuntime/*02000019*/::get_DefaultAsyncTimeout() /* 0600003A */
    .set instance void Microsoft.JSInterop.JSRuntime/*02000019*/::set_DefaultAsyncTimeout(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000031*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/>) /* 0600003B */
  } // end of property JSRuntime::DefaultAsyncTimeout
} // end of class Microsoft.JSInterop.JSRuntime

.class /*0200001A*/ public abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.JSRuntimeExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000078:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00007A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000033*/ auto ansi sealed nested private beforefieldinit '<InvokeAsync>d__4`1'<TValue>
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000CB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000042*/ public int32 '<>1__state'
    .field /*04000043*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100002B*/<!TValue> '<>t__builder'
    .field /*04000044*/ public class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime
    .field /*04000045*/ public valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/ timeout
    .field /*04000046*/ public string identifier
    .field /*04000047*/ public object[] args
    .field /*04000048*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100002C*/ '<cancellationTokenSource>5__2'
    .field /*04000049*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<!TValue> '<>u__1'
    .method /*060000AA*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeAsync>d__4`1'::MoveNext

    .method /*060000AB*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C00014C:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeAsync>d__4`1'::SetStateMachine

  } // end of class '<InvokeAsync>d__4`1'

  .class /*02000034*/ auto ansi sealed nested private beforefieldinit '<InvokeVoidAsync>d__0'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000CF:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*0400004A*/ public int32 '<>1__state'
    .field /*0400004B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*0100002F*/ '<>t__builder'
    .field /*0400004C*/ public class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime
    .field /*0400004D*/ public string identifier
    .field /*0400004E*/ public object[] args
    .field /*0400004F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<class Microsoft.JSInterop.Infrastructure.IJSVoidResult/*02000024*/> '<>u__1'
    .method /*060000AC*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__0'::MoveNext

    .method /*060000AD*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C00014E:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__0'::SetStateMachine

  } // end of class '<InvokeVoidAsync>d__0'

  .class /*02000035*/ auto ansi sealed nested private beforefieldinit '<InvokeVoidAsync>d__3'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000D3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000050*/ public int32 '<>1__state'
    .field /*04000051*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*0100002F*/ '<>t__builder'
    .field /*04000052*/ public class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime
    .field /*04000053*/ public string identifier
    .field /*04000054*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken
    .field /*04000055*/ public object[] args
    .field /*04000056*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<class Microsoft.JSInterop.Infrastructure.IJSVoidResult/*02000024*/> '<>u__1'
    .method /*060000AE*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__3'::MoveNext

    .method /*060000AF*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C000150:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__3'::SetStateMachine

  } // end of class '<InvokeVoidAsync>d__3'

  .class /*02000036*/ auto ansi sealed nested private beforefieldinit '<InvokeVoidAsync>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000D7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000057*/ public int32 '<>1__state'
    .field /*04000058*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*0100002F*/ '<>t__builder'
    .field /*04000059*/ public class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime
    .field /*0400005A*/ public valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/ timeout
    .field /*0400005B*/ public string identifier
    .field /*0400005C*/ public object[] args
    .field /*0400005D*/ private class [System.Runtime/*23000001*/]System.Threading.CancellationTokenSource/*0100002C*/ '<cancellationTokenSource>5__2'
    .field /*0400005E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100002D*/<class Microsoft.JSInterop.Infrastructure.IJSVoidResult/*02000024*/> '<>u__1'
    .method /*060000B0*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__5'::MoveNext

    .method /*060000B1*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C000151:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<InvokeVoidAsync>d__5'::SetStateMachine

  } // end of class '<InvokeVoidAsync>d__5'

  .method /*0600004D*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000027*/ 
          InvokeVoidAsync(class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime,
                          string identifier,
                          object[] args) cil managed
  // SIG: 00 03 11 80 9D 12 34 0E 1D 1C
  {
    .custom /*0C0000F7:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 3D 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..=Microsoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 52 75 6E 74 69 6D 65   // nterop.JSRuntime
                                                                                                                                                                                                                                        45 78 74 65 6E 73 69 6F 6E 73 2B 3C 49 6E 76 6F   // Extensions+<Invo
                                                                                                                                                                                                                                        6B 65 56 6F 69 64 41 73 79 6E 63 3E 64 5F 5F 30   // keVoidAsync>d__0
                                                                                                                                                                                                                                        00 00 ) 
    .custom /*0C0000F8:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*0800008C*/ 
    .custom /*0C000129:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C00012A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntimeExtensions::InvokeVoidAsync

  .method /*0600004E*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime,
                              string identifier,
                              object[] args) cil managed
  // SIG: 10 01 03 15 11 80 85 01 1E 00 12 34 0E 1D 1C
  {
    .custom /*0C0000FA:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A00002F*/ 
      .custom /*0C0000BE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C0000BF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*0800008D*/ 
    .custom /*0C00012B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*08000090*/ 
    .custom /*0C00012F:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000130:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntimeExtensions::InvokeAsync

  .method /*0600004F*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime,
                              string identifier,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                              object[] args) cil managed
  // SIG: 10 01 04 15 11 80 85 01 1E 00 12 34 0E 11 80 89 1D 1C
  {
    .custom /*0C0000FB:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000030*/ 
      .custom /*0C0000C1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C0000C2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000091*/ 
    .custom /*0C000132:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [4]/*08000095*/ 
    .custom /*0C000137:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000138:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntimeExtensions::InvokeAsync

  .method /*06000050*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000027*/ 
          InvokeVoidAsync(class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime,
                          string identifier,
                          valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                          object[] args) cil managed
  // SIG: 00 04 11 80 9D 12 34 0E 11 80 89 1D 1C
  {
    .custom /*0C0000FC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 3D 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..=Microsoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 52 75 6E 74 69 6D 65   // nterop.JSRuntime
                                                                                                                                                                                                                                        45 78 74 65 6E 73 69 6F 6E 73 2B 3C 49 6E 76 6F   // Extensions+<Invo
                                                                                                                                                                                                                                        6B 65 56 6F 69 64 41 73 79 6E 63 3E 64 5F 5F 33   // keVoidAsync>d__3
                                                                                                                                                                                                                                        00 00 ) 
    .custom /*0C0000FD:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*08000099*/ 
    .custom /*0C00013B:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C00013C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntimeExtensions::InvokeVoidAsync

  .method /*06000051*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime,
                              string identifier,
                              valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/ timeout,
                              object[] args) cil managed
  // SIG: 10 01 04 15 11 80 85 01 1E 00 12 34 0E 11 80 A1 1D 1C
  {
    .custom /*0C0000FE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 3B 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..;Microsoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 52 75 6E 74 69 6D 65   // nterop.JSRuntime
                                                                                                                                                                                                                                        45 78 74 65 6E 73 69 6F 6E 73 2B 3C 49 6E 76 6F   // Extensions+<Invo
                                                                                                                                                                                                                                        6B 65 41 73 79 6E 63 3E 64 5F 5F 34 60 31 00 00 ) // keAsync>d__4`1..
    .custom /*0C0000FF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param type TValue /*2A000031*/ 
      .custom /*0C0000C5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C0000C6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*0800009A*/ 
    .custom /*0C00013E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [4]/*0800009E*/ 
    .custom /*0C000142:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000143:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntimeExtensions::InvokeAsync

  .method /*06000052*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000027*/ 
          InvokeVoidAsync(class Microsoft.JSInterop.IJSRuntime/*0200000D*/ jsRuntime,
                          string identifier,
                          valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000028*/ timeout,
                          object[] args) cil managed
  // SIG: 00 04 11 80 9D 12 34 0E 11 80 A1 1D 1C
  {
    .custom /*0C000102:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 3D 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..=Microsoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 4A 53 52 75 6E 74 69 6D 65   // nterop.JSRuntime
                                                                                                                                                                                                                                        45 78 74 65 6E 73 69 6F 6E 73 2B 3C 49 6E 76 6F   // Extensions+<Invo
                                                                                                                                                                                                                                        6B 65 56 6F 69 64 41 73 79 6E 63 3E 64 5F 5F 35   // keVoidAsync>d__5
                                                                                                                                                                                                                                        00 00 ) 
    .custom /*0C000103:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [4]/*080000A2*/ 
    .custom /*0C000146:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000147:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSRuntimeExtensions::InvokeVoidAsync

} // end of class Microsoft.JSInterop.JSRuntimeExtensions

.class /*0200001B*/ private abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.JSCallResultTypeHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .method /*06000053*/ public hidebysig static 
          valuetype Microsoft.JSInterop.JSCallResultType/*02000011*/ 
          FromGeneric<TResult>() cil managed
  // SIG: 10 01 00 11 44
  {
    .custom /*0C000104:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSCallResultTypeHelper::FromGeneric

} // end of class Microsoft.JSInterop.JSCallResultTypeHelper

.class /*0200001C*/ private auto ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.ByteArrayJsonConverter
       extends class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonConverter`1/*01000038*/<uint8[]>/*1B000001*/
{
  .custom /*0C00007D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .field /*0400000C*/ static assembly initonly valuetype [System.Text.Json/*23000003*/]System.Text.Json.JsonEncodedText/*01000039*/ ByteArrayRefKey
  .method /*06000054*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSRuntime/*02000019*/ jSRuntime) cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayJsonConverter::.ctor

  .method /*06000055*/ public hidebysig specialname 
          instance class Microsoft.JSInterop.JSRuntime/*02000019*/ 
          get_JSRuntime() cil managed
  // SIG: 20 00 12 64
  {
    .custom /*0C000105:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayJsonConverter::get_JSRuntime

  .method /*06000056*/ public hidebysig virtual 
          instance uint8[]  Read(valuetype [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonReader/*01000035*/& reader,
                                 class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert,
                                 class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 1D 05 10 11 80 D5 12 1D 12 80 C1
  {
    .param [0]/*080000A4*/ 
    .custom /*0C000149:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayJsonConverter::Read

  .method /*06000057*/ public hidebysig virtual 
          instance void  Write(class [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonWriter/*0100003A*/ writer,
                               uint8[] 'value',
                               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 01 12 80 E9 1D 05 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ByteArrayJsonConverter::Write

  .property /*1700000C*/ instance callconv(8) class Microsoft.JSInterop.JSRuntime/*02000019*/
          JSRuntime()
  {
    .get instance class Microsoft.JSInterop.JSRuntime/*02000019*/ Microsoft.JSInterop.Infrastructure.ByteArrayJsonConverter/*0200001C*/::get_JSRuntime() /* 06000055 */
  } // end of property ByteArrayJsonConverter::JSRuntime
} // end of class Microsoft.JSInterop.Infrastructure.ByteArrayJsonConverter

.class /*0200001D*/ public abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.DotNetDispatcher
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000081:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000082:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000083:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessageAttribute/*01000025*/::.ctor(string,
                                                                                                                                                                   string) /* 0A000018 */ = ( 01 00 12 52 65 66 6C 65 63 74 69 6F 6E 41 6E 61   // ...ReflectionAna
                                                                                                                                                                                              6C 79 73 69 73 06 49 4C 32 30 37 30 01 00 54 0E   // lysis.IL2070..T.
                                                                                                                                                                                              0D 4A 75 73 74 69 66 69 63 61 74 69 6F 6E 6C 4C   // .JustificationlL
                                                                                                                                                                                              69 6E 6B 65 72 20 64 6F 65 73 20 6E 6F 74 20 70   // inker does not p
                                                                                                                                                                                              72 6F 70 6F 67 61 74 65 20 61 6E 6E 6F 74 61 74   // ropogate annotat
                                                                                                                                                                                              69 6F 6E 73 20 74 6F 20 67 65 6E 65 72 61 74 65   // ions to generate
                                                                                                                                                                                              64 20 73 74 61 74 65 20 6D 61 63 68 69 6E 65 2E   // d state machine.
                                                                                                                                                                                              20 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   //  https://github.
                                                                                                                                                                                              63 6F 6D 2F 6D 6F 6E 6F 2F 6C 69 6E 6B 65 72 2F   // com/mono/linker/
                                                                                                                                                                                              69 73 73 75 65 73 2F 31 34 30 33 )                // issues/1403
  .class /*02000037*/ sequential ansi sealed nested private beforefieldinit AssemblyKey
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100003E*/<valuetype Microsoft.JSInterop.Infrastructure.DotNetDispatcher/*0200001D*//AssemblyKey/*02000037*/>/*1B000002*/
  {
    .custom /*0C0000DC:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000032*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    .field /*0400005F*/ private initonly class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003F*/ '<Assembly>k__BackingField'
    .custom /*0C00010F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000060*/ private initonly string '<AssemblyName>k__BackingField'
    .custom /*0C000111:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .method /*060000B2*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003F*/ 'assembly') cil managed
    // SIG: 20 01 01 12 80 FD
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method AssemblyKey::.ctor

    .method /*060000B3*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(string assemblyName) cil managed
    // SIG: 20 01 01 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method AssemblyKey::.ctor

    .method /*060000B4*/ public hidebysig specialname 
            instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003F*/ 
            get_Assembly() cil managed
    // SIG: 20 00 12 80 FD
    {
      .custom /*0C000153:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method AssemblyKey::get_Assembly

    .method /*060000B5*/ public hidebysig specialname 
            instance string  get_AssemblyName() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C000154:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method AssemblyKey::get_AssemblyName

    .method /*060000B6*/ public hidebysig newslot virtual final 
            instance bool  Equals(valuetype Microsoft.JSInterop.Infrastructure.DotNetDispatcher/*0200001D*//AssemblyKey/*02000037*/ other) cil managed
    // SIG: 20 01 02 11 80 DC
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method AssemblyKey::Equals

    .method /*060000B7*/ public hidebysig virtual 
            instance int32  GetHashCode() cil managed
    // SIG: 20 00 08
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method AssemblyKey::GetHashCode

    .property /*1700001E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003F*/
            Assembly()
    {
      .get instance class [System.Runtime/*23000001*/]System.Reflection.Assembly/*0100003F*/ Microsoft.JSInterop.Infrastructure.DotNetDispatcher/*0200001D*//AssemblyKey/*02000037*/::get_Assembly() /* 060000B4 */
    } // end of property AssemblyKey::Assembly
    .property /*1700001F*/ instance callconv(8) string
            AssemblyName()
    {
      .get instance string Microsoft.JSInterop.Infrastructure.DotNetDispatcher/*0200001D*//AssemblyKey/*02000037*/::get_AssemblyName() /* 060000B5 */
    } // end of property AssemblyKey::AssemblyName
  } // end of class AssemblyKey

  .class /*02000038*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass5_0'
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
  {
    .custom /*0C0000DD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000061*/ public class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime
    .field /*04000062*/ public valuetype Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo/*0200001E*/ invocationInfo
    .method /*060000B8*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass5_0'::.ctor

    .method /*060000B9*/ assembly hidebysig 
            instance void  '<BeginInvokeDotNet>b__0'(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000036*/ t) cil managed
    // SIG: 20 01 01 12 80 D9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass5_0'::'<BeginInvokeDotNet>b__0'

  } // end of class '<>c__DisplayClass5_0'

  .field /*0400000D*/ static assembly initonly valuetype [System.Text.Json/*23000003*/]System.Text.Json.JsonEncodedText/*01000039*/ DotNetObjectRefKey
  .method /*06000058*/ public hidebysig static 
          string  Invoke(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime,
                         [in] valuetype Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo/*0200001E*/& invocationInfo,
                         string argsJson) cil managed
  // SIG: 00 03 0E 12 64 10 11 78 0E
  {
    .param [0]/*080000AB*/ 
    .custom /*0C00014D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*080000AD*/ 
    .custom /*0C00014F:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000032*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetDispatcher::Invoke

  .method /*06000059*/ public hidebysig static 
          void  BeginInvokeDotNet(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime,
                                  valuetype Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo/*0200001E*/ invocationInfo,
                                  string argsJson) cil managed
  // SIG: 00 03 01 12 64 11 78 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetDispatcher::BeginInvokeDotNet

  .method /*0600005A*/ assembly hidebysig static 
          object[]  ParseArguments(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime,
                                   string methodIdentifier,
                                   string arguments,
                                   class [System.Runtime/*23000001*/]System.Type/*01000007*/[] parameterTypes) cil managed
  // SIG: 00 04 1D 1C 12 64 0E 0E 1D 12 1D
  {
    .param [0]/*080000B2*/ 
    .custom /*0C000152:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetDispatcher::ParseArguments

  .method /*0600005B*/ public hidebysig static 
          void  EndInvokeJS(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime,
                            string arguments) cil managed
  // SIG: 00 02 01 12 64 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetDispatcher::EndInvokeJS

  .method /*0600005C*/ public hidebysig static 
          void  ReceiveByteArray(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime,
                                 int32 id,
                                 uint8[] data) cil managed
  // SIG: 00 03 01 12 64 08 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetDispatcher::ReceiveByteArray

  .method /*0600005D*/ assembly hidebysig static 
          bool  '<ParseArguments>g__IsIncorrectDotNetObjectRefUse|8_0'(class [System.Runtime/*23000001*/]System.Type/*01000007*/ parameterType,
                                                                       valuetype [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonReader/*01000035*/ jsonReader) cil managed
  // SIG: 00 02 02 12 1D 11 80 D5
  {
    .custom /*0C00010B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetDispatcher::'<ParseArguments>g__IsIncorrectDotNetObjectRefUse|8_0'

  .method /*0600005E*/ assembly hidebysig static 
          class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003B*/<string,valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*0100003C*/<class [System.Runtime/*23000001*/]System.Reflection.MethodInfo/*0100003D*/,class [System.Runtime/*23000001*/]System.Type/*01000007*/[]>> 
          '<GetCachedMethodInfo>g__ScanTypeForCallableMethods|12_0'(class [System.Runtime/*23000001*/]System.Type/*01000007*/ 'type') cil managed
  // SIG: 00 01 15 12 80 ED 02 0E 15 11 80 F1 02 12 80 F5 1D 12 1D 12 1D
  {
    .custom /*0C00010C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .param [0]/*080000BE*/ 
    .custom /*0C000155:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 06 00 00 00 01 01 00 01 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetDispatcher::'<GetCachedMethodInfo>g__ScanTypeForCallableMethods|12_0'

} // end of class Microsoft.JSInterop.Infrastructure.DotNetDispatcher

.class /*0200001E*/ public sequential ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
{
  .custom /*0C000086:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000087:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000088:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000032*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
  .field /*0400000E*/ private initonly string '<AssemblyName>k__BackingField'
  .custom /*0C000046:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*0400000F*/ private initonly string '<MethodIdentifier>k__BackingField'
  .custom /*0C00004A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000010*/ private initonly int64 '<DotNetObjectId>k__BackingField'
  .custom /*0C00004C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000011*/ private initonly string '<CallId>k__BackingField'
  .custom /*0C000050:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*0600005F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string assemblyName,
                               string methodIdentifier,
                               int64 dotNetObjectId,
                               string callId) cil managed
  // SIG: 20 04 01 0E 0E 0A 0E
  {
    .param [2]/*080000C1*/ 
    .custom /*0C000156:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationInfo::.ctor

  .method /*06000060*/ public hidebysig specialname 
          instance string  get_AssemblyName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000110:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationInfo::get_AssemblyName

  .method /*06000061*/ public hidebysig specialname 
          instance string  get_MethodIdentifier() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000113:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000114:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationInfo::get_MethodIdentifier

  .method /*06000062*/ public hidebysig specialname 
          instance int64  get_DotNetObjectId() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000116:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationInfo::get_DotNetObjectId

  .method /*06000063*/ public hidebysig specialname 
          instance string  get_CallId() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000117:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationInfo::get_CallId

  .property /*1700000D*/ instance callconv(8) string
          AssemblyName()
  {
    .get instance string Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo/*0200001E*/::get_AssemblyName() /* 06000060 */
  } // end of property DotNetInvocationInfo::AssemblyName
  .property /*1700000E*/ instance callconv(8) string
          MethodIdentifier()
  {
    .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo/*0200001E*/::get_MethodIdentifier() /* 06000061 */
  } // end of property DotNetInvocationInfo::MethodIdentifier
  .property /*1700000F*/ instance callconv(8) int64
          DotNetObjectId()
  {
    .get instance int64 Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo/*0200001E*/::get_DotNetObjectId() /* 06000062 */
  } // end of property DotNetInvocationInfo::DotNetObjectId
  .property /*17000010*/ instance callconv(8) string
          CallId()
  {
    .get instance string Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo/*0200001E*/::get_CallId() /* 06000063 */
  } // end of property DotNetInvocationInfo::CallId
} // end of class Microsoft.JSInterop.Infrastructure.DotNetInvocationInfo

.class /*0200001F*/ public sequential ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.DotNetInvocationResult
       extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
{
  .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00008D:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000032*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
  .field /*04000012*/ private initonly class [System.Runtime/*23000001*/]System.Exception/*01000024*/ '<Exception>k__BackingField'
  .custom /*0C000054:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000013*/ private initonly string '<ErrorKind>k__BackingField'
  .custom /*0C000059:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000014*/ private initonly string '<ResultJson>k__BackingField'
  .custom /*0C00005F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .field /*04000015*/ private initonly bool '<Success>k__BackingField'
  .custom /*0C000064:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .method /*06000064*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Exception/*01000024*/ exception,
                               string errorKind) cil managed
  // SIG: 20 02 01 12 80 91 0E
  {
    .custom /*0C000119:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C5*/ 
    .custom /*0C000157:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationResult::.ctor

  .method /*06000065*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(string resultJson) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationResult::.ctor

  .method /*06000066*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Exception/*01000024*/ 
          get_Exception() cil managed
  // SIG: 20 00 12 80 91
  {
    .custom /*0C00011B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationResult::get_Exception

  .method /*06000067*/ public hidebysig specialname 
          instance string  get_ErrorKind() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00011C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationResult::get_ErrorKind

  .method /*06000068*/ public hidebysig specialname 
          instance string  get_ResultJson() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00011E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationResult::get_ResultJson

  .method /*06000069*/ public hidebysig specialname 
          instance bool  get_Success() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00011F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetInvocationResult::get_Success

  .property /*17000011*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Exception/*01000024*/
          Exception()
  {
    .get instance class [System.Runtime/*23000001*/]System.Exception/*01000024*/ Microsoft.JSInterop.Infrastructure.DotNetInvocationResult/*0200001F*/::get_Exception() /* 06000066 */
  } // end of property DotNetInvocationResult::Exception
  .property /*17000012*/ instance callconv(8) string
          ErrorKind()
  {
    .get instance string Microsoft.JSInterop.Infrastructure.DotNetInvocationResult/*0200001F*/::get_ErrorKind() /* 06000067 */
  } // end of property DotNetInvocationResult::ErrorKind
  .property /*17000013*/ instance callconv(8) string
          ResultJson()
  {
    .get instance string Microsoft.JSInterop.Infrastructure.DotNetInvocationResult/*0200001F*/::get_ResultJson() /* 06000068 */
  } // end of property DotNetInvocationResult::ResultJson
  .property /*17000014*/ instance callconv(8) bool
          Success()
  {
    .get instance bool Microsoft.JSInterop.Infrastructure.DotNetInvocationResult/*0200001F*/::get_Success() /* 06000069 */
  } // end of property DotNetInvocationResult::Success
} // end of class Microsoft.JSInterop.Infrastructure.DotNetInvocationResult

.class /*02000020*/ private auto ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.DotNetObjectReferenceJsonConverter`1<class TValue>
       extends class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonConverter`1/*01000038*/<class Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/<!TValue>>/*1B000003*/
{
  .custom /*0C000091:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000092:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime) cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReferenceJsonConverter`1::.ctor

  .method /*0600006B*/ public hidebysig specialname 
          instance class Microsoft.JSInterop.JSRuntime/*02000019*/ 
          get_JSRuntime() cil managed
  // SIG: 20 00 12 64
  {
    .custom /*0C000121:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReferenceJsonConverter`1::get_JSRuntime

  .method /*0600006C*/ public hidebysig virtual 
          instance class Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/<!TValue> 
          Read(valuetype [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonReader/*01000035*/& reader,
               class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert,
               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 15 12 20 01 13 00 10 11 80 D5 12 1D 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReferenceJsonConverter`1::Read

  .method /*0600006D*/ public hidebysig virtual 
          instance void  Write(class [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonWriter/*0100003A*/ writer,
                               class Microsoft.JSInterop.DotNetObjectReference`1/*02000008*/<!TValue> 'value',
                               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 01 12 80 E9 15 12 20 01 13 00 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReferenceJsonConverter`1::Write

  .property /*17000015*/ instance callconv(8) class Microsoft.JSInterop.JSRuntime/*02000019*/
          JSRuntime()
  {
    .get instance class Microsoft.JSInterop.JSRuntime/*02000019*/ Microsoft.JSInterop.Infrastructure.DotNetObjectReferenceJsonConverter`1/*02000020*/::get_JSRuntime() /* 0600006B */
  } // end of property DotNetObjectReferenceJsonConverter`1::JSRuntime
} // end of class Microsoft.JSInterop.Infrastructure.DotNetObjectReferenceJsonConverter`1

.class /*02000021*/ private auto ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.DotNetObjectReferenceJsonConverterFactory
       extends [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonConverterFactory/*01000040*/
{
  .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000096:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime) cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReferenceJsonConverterFactory::.ctor

  .method /*0600006F*/ public hidebysig specialname 
          instance class Microsoft.JSInterop.JSRuntime/*02000019*/ 
          get_JSRuntime() cil managed
  // SIG: 20 00 12 64
  {
    .custom /*0C000124:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReferenceJsonConverterFactory::get_JSRuntime

  .method /*06000070*/ public hidebysig virtual 
          instance bool  CanConvert(class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert) cil managed
  // SIG: 20 01 02 12 1D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReferenceJsonConverterFactory::CanConvert

  .method /*06000071*/ public hidebysig virtual 
          instance class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonConverter/*01000041*/ 
          CreateConverter(class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert,
                          class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ jsonSerializerOptions) cil managed
  // SIG: 20 02 12 81 05 12 1D 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetObjectReferenceJsonConverterFactory::CreateConverter

  .property /*17000016*/ instance callconv(8) class Microsoft.JSInterop.JSRuntime/*02000019*/
          JSRuntime()
  {
    .get instance class Microsoft.JSInterop.JSRuntime/*02000019*/ Microsoft.JSInterop.Infrastructure.DotNetObjectReferenceJsonConverterFactory/*02000021*/::get_JSRuntime() /* 0600006F */
  } // end of property DotNetObjectReferenceJsonConverterFactory::JSRuntime
} // end of class Microsoft.JSInterop.Infrastructure.DotNetObjectReferenceJsonConverterFactory

.class /*02000022*/ private auto ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.DotNetStreamReferenceJsonConverter
       extends class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonConverter`1/*01000038*/<class Microsoft.JSInterop.DotNetStreamReference/*02000009*/>/*1B000004*/
{
  .custom /*0C000097:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000098:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*06000072*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime) cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetStreamReferenceJsonConverter::.ctor

  .method /*06000073*/ public hidebysig specialname 
          instance class Microsoft.JSInterop.JSRuntime/*02000019*/ 
          get_JSRuntime() cil managed
  // SIG: 20 00 12 64
  {
    .custom /*0C000126:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetStreamReferenceJsonConverter::get_JSRuntime

  .method /*06000074*/ public hidebysig virtual 
          instance class Microsoft.JSInterop.DotNetStreamReference/*02000009*/ 
          Read(valuetype [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonReader/*01000035*/& reader,
               class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert,
               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 12 24 10 11 80 D5 12 1D 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetStreamReferenceJsonConverter::Read

  .method /*06000075*/ public hidebysig virtual 
          instance void  Write(class [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonWriter/*0100003A*/ writer,
                               class Microsoft.JSInterop.DotNetStreamReference/*02000009*/ 'value',
                               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 01 12 80 E9 12 24 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DotNetStreamReferenceJsonConverter::Write

  .property /*17000017*/ instance callconv(8) class Microsoft.JSInterop.JSRuntime/*02000019*/
          JSRuntime()
  {
    .get instance class Microsoft.JSInterop.JSRuntime/*02000019*/ Microsoft.JSInterop.Infrastructure.DotNetStreamReferenceJsonConverter/*02000022*/::get_JSRuntime() /* 06000073 */
  } // end of property DotNetStreamReferenceJsonConverter::JSRuntime
} // end of class Microsoft.JSInterop.Infrastructure.DotNetStreamReferenceJsonConverter

.class /*02000023*/ interface private abstract auto ansi Microsoft.JSInterop.Infrastructure.IDotNetObjectReference
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C000099:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000076*/ public hidebysig newslot specialname abstract virtual 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    // Method begins at RVA 0x0
  } // end of method IDotNetObjectReference::get_Value

  .property /*17000018*/ instance callconv(8) object
          Value()
  {
    .get instance object Microsoft.JSInterop.Infrastructure.IDotNetObjectReference/*02000023*/::get_Value() /* 06000076 */
  } // end of property IDotNetObjectReference::Value
} // end of class Microsoft.JSInterop.Infrastructure.IDotNetObjectReference

.class /*02000024*/ interface public abstract auto ansi Microsoft.JSInterop.Infrastructure.IJSVoidResult
{
  .custom /*0C00009A:0A00001E*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000043*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000042*/) /* 0A00001E */ = ( 01 00 01 00 00 00 00 00 ) 
} // end of class Microsoft.JSInterop.Infrastructure.IJSVoidResult

.class /*02000025*/ private auto ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.JSObjectReferenceJsonConverter
       extends class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonConverter`1/*01000038*/<class Microsoft.JSInterop.IJSObjectReference/*0200000C*/>/*1B000005*/
{
  .custom /*0C00009C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*06000077*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime) cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceJsonConverter::.ctor

  .method /*06000078*/ public hidebysig virtual 
          instance bool  CanConvert(class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert) cil managed
  // SIG: 20 01 02 12 1D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceJsonConverter::CanConvert

  .method /*06000079*/ public hidebysig virtual 
          instance class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ 
          Read(valuetype [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonReader/*01000035*/& reader,
               class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert,
               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 12 30 10 11 80 D5 12 1D 12 80 C1
  {
    .param [0]/*080000DB*/ 
    .custom /*0C00015A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceJsonConverter::Read

  .method /*0600007A*/ public hidebysig virtual 
          instance void  Write(class [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonWriter/*0100003A*/ writer,
                               class Microsoft.JSInterop.IJSObjectReference/*0200000C*/ 'value',
                               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 01 12 80 E9 12 30 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceJsonConverter::Write

} // end of class Microsoft.JSInterop.Infrastructure.JSObjectReferenceJsonConverter

.class /*02000026*/ private auto ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.JSStreamReferenceJsonConverter
       extends class [System.Text.Json/*23000003*/]System.Text.Json.Serialization.JsonConverter`1/*01000038*/<class Microsoft.JSInterop.IJSStreamReference/*0200000E*/>/*1B000006*/
{
  .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A1:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*0600007B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime) cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSStreamReferenceJsonConverter::.ctor

  .method /*0600007C*/ public hidebysig virtual 
          instance bool  CanConvert(class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert) cil managed
  // SIG: 20 01 02 12 1D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSStreamReferenceJsonConverter::CanConvert

  .method /*0600007D*/ public hidebysig virtual 
          instance class Microsoft.JSInterop.IJSStreamReference/*0200000E*/ 
          Read(valuetype [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonReader/*01000035*/& reader,
               class [System.Runtime/*23000001*/]System.Type/*01000007*/ typeToConvert,
               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 12 38 10 11 80 D5 12 1D 12 80 C1
  {
    .param [0]/*080000E4*/ 
    .custom /*0C00015B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSStreamReferenceJsonConverter::Read

  .method /*0600007E*/ public hidebysig virtual 
          instance void  Write(class [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonWriter/*0100003A*/ writer,
                               class Microsoft.JSInterop.IJSStreamReference/*0200000E*/ 'value',
                               class [System.Text.Json/*23000003*/]System.Text.Json.JsonSerializerOptions/*01000030*/ options) cil managed
  // SIG: 20 03 01 12 80 E9 12 38 12 80 C1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSStreamReferenceJsonConverter::Write

} // end of class Microsoft.JSInterop.Infrastructure.JSStreamReferenceJsonConverter

.class /*02000027*/ private abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.Infrastructure.TaskGenericsUtil
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C0000A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000039*/ interface abstract auto ansi nested private ITcsResultSetter
  {
    .method /*060000BA*/ public hidebysig newslot specialname abstract virtual 
            instance class [System.Runtime/*23000001*/]System.Type/*01000007*/ 
            get_ResultType() cil managed
    // SIG: 20 00 12 1D
    {
      // Method begins at RVA 0x0
    } // end of method ITcsResultSetter::get_ResultType

    .method /*060000BB*/ public hidebysig newslot abstract virtual 
            instance void  SetResult(object taskCompletionSource,
                                     object result) cil managed
    // SIG: 20 02 01 1C 1C
    {
      // Method begins at RVA 0x0
    } // end of method ITcsResultSetter::SetResult

    .method /*060000BC*/ public hidebysig newslot abstract virtual 
            instance void  SetException(object taskCompletionSource,
                                        class [System.Runtime/*23000001*/]System.Exception/*01000024*/ exception) cil managed
    // SIG: 20 02 01 1C 12 80 91
    {
      // Method begins at RVA 0x0
    } // end of method ITcsResultSetter::SetException

    .property /*17000020*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000007*/
            ResultType()
    {
      .get instance class [System.Runtime/*23000001*/]System.Type/*01000007*/ Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//ITcsResultSetter/*02000039*/::get_ResultType() /* 060000BA */
    } // end of property ITcsResultSetter::ResultType
  } // end of class ITcsResultSetter

  .class /*0200003A*/ interface abstract auto ansi nested private ITaskResultGetter
  {
    .method /*060000BD*/ public hidebysig newslot abstract virtual 
            instance object  GetResult(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000036*/ task) cil managed
    // SIG: 20 01 1C 12 80 D9
    {
      // Method begins at RVA 0x0
    } // end of method ITaskResultGetter::GetResult

  } // end of class ITaskResultGetter

  .class /*0200003B*/ auto ansi nested private beforefieldinit TaskResultGetter`1<T>
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
         implements Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//ITaskResultGetter/*0200003A*/
  {
    .method /*060000BE*/ public hidebysig newslot virtual final 
            instance object  GetResult(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000036*/ task) cil managed
    // SIG: 20 01 1C 12 80 D9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TaskResultGetter`1::GetResult

    .method /*060000BF*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TaskResultGetter`1::.ctor

  } // end of class TaskResultGetter`1

  .class /*0200003C*/ auto ansi nested private beforefieldinit VoidTaskResultGetter
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
         implements Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//ITaskResultGetter/*0200003A*/
  {
    .method /*060000C0*/ public hidebysig newslot virtual final 
            instance object  GetResult(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000036*/ task) cil managed
    // SIG: 20 01 1C 12 80 D9
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method VoidTaskResultGetter::GetResult

    .method /*060000C1*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method VoidTaskResultGetter::.ctor

  } // end of class VoidTaskResultGetter

  .class /*0200003D*/ auto ansi nested private beforefieldinit TcsResultSetter`1<T>
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
         implements Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//ITcsResultSetter/*02000039*/
  {
    .method /*060000C2*/ public hidebysig newslot specialname virtual final 
            instance class [System.Runtime/*23000001*/]System.Type/*01000007*/ 
            get_ResultType() cil managed
    // SIG: 20 00 12 1D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TcsResultSetter`1::get_ResultType

    .method /*060000C3*/ public hidebysig newslot virtual final 
            instance void  SetResult(object tcs,
                                     object result) cil managed
    // SIG: 20 02 01 1C 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TcsResultSetter`1::SetResult

    .method /*060000C4*/ public hidebysig newslot virtual final 
            instance void  SetException(object tcs,
                                        class [System.Runtime/*23000001*/]System.Exception/*01000024*/ exception) cil managed
    // SIG: 20 02 01 1C 12 80 91
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TcsResultSetter`1::SetException

    .method /*060000C5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method TcsResultSetter`1::.ctor

    .property /*17000021*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Type/*01000007*/
            ResultType()
    {
      .get instance class [System.Runtime/*23000001*/]System.Type/*01000007*/ Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//TcsResultSetter`1/*0200003D*/::get_ResultType() /* 060000C2 */
    } // end of property TcsResultSetter`1::ResultType
  } // end of class TcsResultSetter`1

  .class /*0200003E*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
  {
    .custom /*0C0000EA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000063*/ public static initonly class Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//'<>c'/*0200003E*/ '<>9'
    .field /*04000064*/ public static class [System.Runtime/*23000001*/]System.Func`2/*01000044*/<class [System.Runtime/*23000001*/]System.Type/*01000007*/,class Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//ITaskResultGetter/*0200003A*/> '<>9__5_0'
    .field /*04000065*/ public static class [System.Runtime/*23000001*/]System.Func`2/*01000044*/<class [System.Runtime/*23000001*/]System.Type/*01000007*/,class Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//ITcsResultSetter/*02000039*/> '<>9__12_0'
    .method /*060000C6*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*060000C7*/ assembly hidebysig 
            instance class Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//ITaskResultGetter/*0200003A*/ 
            '<GetTaskResult>b__5_0'(class [System.Runtime/*23000001*/]System.Type/*01000007*/ taskInstanceType) cil managed
    // SIG: 20 01 12 80 E8 12 1D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetTaskResult>b__5_0'

    .method /*060000C8*/ assembly hidebysig 
            instance class Microsoft.JSInterop.Infrastructure.TaskGenericsUtil/*02000027*//ITcsResultSetter/*02000039*/ 
            '<CreateResultSetter>b__12_0'(class [System.Runtime/*23000001*/]System.Type/*01000007*/ tcsType) cil managed
    // SIG: 20 01 12 80 E4 12 1D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<CreateResultSetter>b__12_0'

  } // end of class '<>c'

  .method /*0600007F*/ public hidebysig static 
          void  SetTaskCompletionSourceResult(object taskCompletionSource,
                                              object result) cil managed
  // SIG: 00 02 01 1C 1C
  {
    .param [2]/*080000EC*/ 
    .custom /*0C00015C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TaskGenericsUtil::SetTaskCompletionSourceResult

  .method /*06000080*/ public hidebysig static 
          void  SetTaskCompletionSourceException(object taskCompletionSource,
                                                 class [System.Runtime/*23000001*/]System.Exception/*01000024*/ exception) cil managed
  // SIG: 00 02 01 1C 12 80 91
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TaskGenericsUtil::SetTaskCompletionSourceException

  .method /*06000081*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Type/*01000007*/ 
          GetTaskCompletionSourceResultType(object taskCompletionSource) cil managed
  // SIG: 00 01 12 1D 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TaskGenericsUtil::GetTaskCompletionSourceResultType

  .method /*06000082*/ public hidebysig static 
          object  GetTaskResult(class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000036*/ task) cil managed
  // SIG: 00 01 1C 12 80 D9
  {
    .param [0]/*080000F0*/ 
    .custom /*0C00015D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TaskGenericsUtil::GetTaskResult

} // end of class Microsoft.JSInterop.Infrastructure.TaskGenericsUtil

.class /*02000028*/ private auto ansi beforefieldinit Microsoft.JSInterop.Infrastructure.ArrayBuilder`1<T>
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .custom /*0C0000AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
  .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .param type T /*2A00001E*/ 
    .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .field /*04000016*/ family !T[] _items
  .field /*04000017*/ family int32 _itemsInUse
  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor([opt] int32 minCapacity,
                               [opt] class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*01000045*/<!T> arrayPool) cil managed
  // SIG: 20 02 01 08 15 12 81 15 01 13 00
  {
    .param [1]/*080000F2*/  = int32(0x00000020)
    .param [2]/*080000F3*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::.ctor

  .method /*06000084*/ public hidebysig specialname 
          instance int32  get_Count() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::get_Count

  .method /*06000085*/ public hidebysig specialname 
          instance !T[]  get_Buffer() cil managed
  // SIG: 20 00 1D 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::get_Buffer

  .method /*06000086*/ public hidebysig instance int32 
          Append([in] !T& item) cil managed aggressiveinlining
  // SIG: 20 01 08 10 13 00
  {
    .param [1]/*080000F4*/ 
    .custom /*0C00015E:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000032*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::Append

  .method /*06000087*/ assembly hidebysig 
          instance int32  Append(!T[] source,
                                 int32 startIndex,
                                 int32 length) cil managed
  // SIG: 20 03 08 1D 13 00 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::Append

  .method /*06000088*/ public hidebysig instance void 
          Overwrite(int32 index,
                    [in] !T& 'value') cil managed aggressiveinlining
  // SIG: 20 02 01 08 10 13 00
  {
    .param [2]/*080000F9*/ 
    .custom /*0C00015F:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000032*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::Overwrite

  .method /*06000089*/ public hidebysig instance void 
          RemoveLast() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::RemoveLast

  .method /*0600008A*/ public hidebysig instance void 
          InsertExpensive(int32 index,
                          !T 'value') cil managed
  // SIG: 20 02 01 08 13 00
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::InsertExpensive

  .method /*0600008B*/ public hidebysig instance void 
          Clear() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::Clear

  .method /*0600008C*/ family hidebysig instance void 
          GrowBuffer(int32 desiredCapacity) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::GrowBuffer

  .method /*0600008D*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ArrayBuilder`1::Dispose

  .property /*17000019*/ instance callconv(8) int32
          Count()
  {
    .get instance int32 Microsoft.JSInterop.Infrastructure.ArrayBuilder`1/*02000028*/::get_Count() /* 06000084 */
  } // end of property ArrayBuilder`1::Count
  .property /*1700001A*/ instance callconv(8) !T[]
          Buffer()
  {
    .get instance !T[] Microsoft.JSInterop.Infrastructure.ArrayBuilder`1/*02000028*/::get_Buffer() /* 06000085 */
  } // end of property ArrayBuilder`1::Buffer
} // end of class Microsoft.JSInterop.Infrastructure.ArrayBuilder`1

.class /*02000029*/ public auto ansi beforefieldinit Microsoft.JSInterop.Implementation.JSInProcessObjectReference
       extends Microsoft.JSInterop.Implementation.JSObjectReference/*0200002A*/
       implements Microsoft.JSInterop.IJSInProcessObjectReference/*0200000A*/,
                  Microsoft.JSInterop.IJSObjectReference/*0200000C*/,
                  [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001D*/,
                  [System.Runtime/*23000001*/]System.IDisposable/*0100001B*/
{
  .method /*0600008E*/ famorassem hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSInProcessRuntime/*02000015*/ jsRuntime,
                               int64 id) cil managed
  // SIG: 20 02 01 12 54 0A
  {
    .custom /*0C00012C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessObjectReference::.ctor

  .method /*0600008F*/ public hidebysig newslot virtual final 
          instance !!TValue  Invoke<TValue>(string identifier,
                                            object[] args) cil managed
  // SIG: 30 01 02 1E 00 0E 1D 1C
  {
    .custom /*0C00012D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00012E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 5E 4A 53 4F 4E 20 73 65 72 69 61 6C 69 7A   // ..^JSON serializ
                                                                                                                                                                                            61 74 69 6F 6E 20 61 6E 64 20 64 65 73 65 72 69   // ation and deseri
                                                                                                                                                                                            61 6C 69 7A 61 74 69 6F 6E 20 6D 69 67 68 74 20   // alization might 
                                                                                                                                                                                            72 65 71 75 69 72 65 20 74 79 70 65 73 20 74 68   // require types th
                                                                                                                                                                                            61 74 20 63 61 6E 6E 6F 74 20 62 65 20 73 74 61   // at cannot be sta
                                                                                                                                                                                            74 69 63 61 6C 6C 79 20 61 6E 61 6C 79 7A 65 64   // tically analyzed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param type TValue /*2A000033*/ 
      .custom /*0C0000CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
      .custom /*0C0000CD:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [2]/*08000100*/ 
    .custom /*0C000160:0A000017*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000020*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .custom /*0C000161:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessObjectReference::Invoke

  .method /*06000090*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSInProcessObjectReference::Dispose

} // end of class Microsoft.JSInterop.Implementation.JSInProcessObjectReference

.class /*0200002A*/ public auto ansi beforefieldinit Microsoft.JSInterop.Implementation.JSObjectReference
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
       implements Microsoft.JSInterop.IJSObjectReference/*0200000C*/,
                  [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001D*/
{
  .class /*0200003F*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__11'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000ED:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*04000066*/ public int32 '<>1__state'
    .field /*04000067*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*0100002F*/ '<>t__builder'
    .field /*04000068*/ public class Microsoft.JSInterop.Implementation.JSObjectReference/*0200002A*/ '<>4__this'
    .field /*04000069*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000046*/ '<>u__1'
    .method /*060000C9*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__11'::MoveNext

    .method /*060000CA*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C000158:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__11'::SetStateMachine

  } // end of class '<DisposeAsync>d__11'

  .method /*06000091*/ assembly hidebysig specialname 
          instance bool  get_Disposed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000131:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReference::get_Disposed

  .method /*06000092*/ assembly hidebysig specialname 
          instance void  set_Disposed(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000133:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReference::set_Disposed

  .method /*06000093*/ famorassem hidebysig specialname 
          instance int64  get_Id() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C000134:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReference::get_Id

  .method /*06000094*/ famorassem hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime,
                               int64 id) cil managed
  // SIG: 20 02 01 12 64 0A
  {
    .custom /*0C000135:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReference::.ctor

  .method /*06000095*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(string identifier,
                              object[] args) cil managed
  // SIG: 30 01 02 15 11 80 85 01 1E 00 0E 1D 1C
  {
    .custom /*0C000136:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param type TValue /*2A000034*/ 
      .custom /*0C0000D0:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000104*/ 
    .custom /*0C000162:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000105*/ 
    .custom /*0C000163:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReference::InvokeAsync

  .method /*06000096*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<!!TValue> 
          InvokeAsync<TValue>(string identifier,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken,
                              object[] args) cil managed
  // SIG: 30 01 03 15 11 80 85 01 1E 00 0E 11 80 89 1D 1C
  {
    .custom /*0C000139:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param type TValue /*2A000035*/ 
      .custom /*0C0000D4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001A*/) /* 0A000016 */ = ( 01 00 23 02 00 00 00 00 )                         // ..#.....
    .param [0]/*08000107*/ 
    .custom /*0C000164:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [1]/*08000108*/ 
    .custom /*0C000165:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReference::InvokeAsync

  .method /*06000097*/ public hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000027*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 9D
  {
    .custom /*0C00013A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 48 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..HMicrosoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 49 6D 70 6C 65 6D 65 6E 74   // nterop.Implement
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 4A 53 4F 62 6A 65 63 74 52 65   // ation.JSObjectRe
                                                                                                                                                                                                                                        66 65 72 65 6E 63 65 2B 3C 44 69 73 70 6F 73 65   // ference+<Dispose
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 31 31 00 00 )          // Async>d__11..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReference::DisposeAsync

  .method /*06000098*/ family hidebysig instance void 
          ThrowIfDisposed() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReference::ThrowIfDisposed

  .property /*1700001B*/ instance callconv(8) bool
          Disposed()
  {
    .get instance bool Microsoft.JSInterop.Implementation.JSObjectReference/*0200002A*/::get_Disposed() /* 06000091 */
    .set instance void Microsoft.JSInterop.Implementation.JSObjectReference/*0200002A*/::set_Disposed(bool) /* 06000092 */
  } // end of property JSObjectReference::Disposed
  .property /*1700001C*/ instance callconv(8) int64
          Id()
  {
    .get instance int64 Microsoft.JSInterop.Implementation.JSObjectReference/*0200002A*/::get_Id() /* 06000093 */
  } // end of property JSObjectReference::Id
} // end of class Microsoft.JSInterop.Implementation.JSObjectReference

.class /*0200002B*/ public abstract auto ansi sealed beforefieldinit Microsoft.JSInterop.Implementation.JSObjectReferenceJsonWorker
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .field /*04000018*/ static assembly initonly valuetype [System.Text.Json/*23000003*/]System.Text.Json.JsonEncodedText/*01000039*/ JSObjectIdKey
  .method /*06000099*/ public hidebysig static 
          int64  ReadJSObjectReferenceIdentifier(valuetype [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonReader/*01000035*/& reader) cil managed
  // SIG: 00 01 0A 10 11 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceJsonWorker::ReadJSObjectReferenceIdentifier

  .method /*0600009A*/ public hidebysig static 
          void  WriteJSObjectReference(class [System.Text.Json/*23000003*/]System.Text.Json.Utf8JsonWriter/*0100003A*/ writer,
                                       class Microsoft.JSInterop.Implementation.JSObjectReference/*0200002A*/ objectReference) cil managed
  // SIG: 00 02 01 12 80 E9 12 80 A8
  {
    .custom /*0C00013D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSObjectReferenceJsonWorker::WriteJSObjectReference

} // end of class Microsoft.JSInterop.Implementation.JSObjectReferenceJsonWorker

.class /*0200002C*/ public auto ansi sealed beforefieldinit Microsoft.JSInterop.Implementation.JSStreamReference
       extends Microsoft.JSInterop.Implementation.JSObjectReference/*0200002A*/
       implements Microsoft.JSInterop.IJSStreamReference/*0200000E*/,
                  [System.Runtime/*23000001*/]System.IAsyncDisposable/*0100001D*/
{
  .class /*02000040*/ auto ansi sealed nested private beforefieldinit '<Microsoft-JSInterop-IJSStreamReference-OpenReadStreamAsync>d__5'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000029*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/
  {
    .custom /*0C0000EE:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .field /*0400006A*/ public int32 '<>1__state'
    .field /*0400006B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100002B*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> '<>t__builder'
    .field /*0400006C*/ public class Microsoft.JSInterop.Implementation.JSStreamReference/*0200002C*/ '<>4__this'
    .field /*0400006D*/ public int64 maxAllowedSize
    .field /*0400006E*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken
    .field /*0400006F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000047*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> '<>u__1'
    .method /*060000CB*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::MoveNext /*0100002A::0A00001A*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<Microsoft-JSInterop-IJSStreamReference-OpenReadStreamAsync>d__5'::MoveNext

    .method /*060000CC*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 A9
    {
      .custom /*0C000159:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002E*/::.ctor() /* 0A00001C */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002A*/::SetStateMachine /*0100002A::0A00001B*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<Microsoft-JSInterop-IJSStreamReference-OpenReadStreamAsync>d__5'::SetStateMachine

  } // end of class '<Microsoft-JSInterop-IJSStreamReference-OpenReadStreamAsync>d__5'

  .method /*0600009B*/ public hidebysig newslot specialname virtual final 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    .custom /*0C00013F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSStreamReference::get_Length

  .method /*0600009C*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.JSInterop.JSRuntime/*02000019*/ jsRuntime,
                               int64 id,
                               int64 totalLength) cil managed
  // SIG: 20 03 01 12 64 0A 0A
  {
    .custom /*0C000140:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSStreamReference::.ctor

  .method /*0600009D*/ private hidebysig newslot virtual final 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000021*/<class [System.Runtime/*23000001*/]System.IO.Stream/*0100001C*/> 
          Microsoft.JSInterop.IJSStreamReference.OpenReadStreamAsync(int64 maxAllowedSize,
                                                                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000022*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 85 01 12 71 0A 11 80 89
  {
    .custom /*0C000141:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000026*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000007*/) /* 0A000019 */ = ( 01 00 75 4D 69 63 72 6F 73 6F 66 74 2E 4A 53 49   // ..uMicrosoft.JSI
                                                                                                                                                                                                                                        6E 74 65 72 6F 70 2E 49 6D 70 6C 65 6D 65 6E 74   // nterop.Implement
                                                                                                                                                                                                                                        61 74 69 6F 6E 2E 4A 53 53 74 72 65 61 6D 52 65   // ation.JSStreamRe
                                                                                                                                                                                                                                        66 65 72 65 6E 63 65 2B 3C 4D 69 63 72 6F 73 6F   // ference+<Microso
                                                                                                                                                                                                                                        66 74 2D 4A 53 49 6E 74 65 72 6F 70 2D 49 4A 53   // ft-JSInterop-IJS
                                                                                                                                                                                                                                        53 74 72 65 61 6D 52 65 66 65 72 65 6E 63 65 2D   // StreamReference-
                                                                                                                                                                                                                                        4F 70 65 6E 52 65 61 64 53 74 72 65 61 6D 41 73   // OpenReadStreamAs
                                                                                                                                                                                                                                        79 6E 63 3E 64 5F 5F 35 00 00 )                   // ync>d__5..
    .override Microsoft.JSInterop.IJSStreamReference/*0200000E*/::OpenReadStreamAsync /*0200000E::0600001C*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JSStreamReference::Microsoft.JSInterop.IJSStreamReference.OpenReadStreamAsync

  .property /*1700001D*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 Microsoft.JSInterop.Implementation.JSStreamReference/*0200002C*/::get_Length() /* 0600009B */
  } // end of property JSStreamReference::Length
} // end of class Microsoft.JSInterop.Implementation.JSStreamReference


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
