


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
// Time-date stamp:            0xd71a7dda
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00007000
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000ad12
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
// Checksum:                       0x0000aab9
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000acc0 [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x00000570] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00007e00 [0x00002470] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000aca4 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00006e28 Virtual Size
//              0x00004000 Virtual Address
//              0x00007000 Size of Raw Data
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
//              0x0000c000 Virtual Address
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
//              0x0000e000 Virtual Address
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
//              0x0000a000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000d14
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x0000ad02 Import Name Table
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
// 0x00004054 [0x00006bd0] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000ac24 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x000037c8 Size
//              '#~' Name
//    Stream 2:
//              0x00003834 Offset
//              0x00001b7c Size
//              '#Strings' Name
//    Stream 3:
//              0x000053b0 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000053b4 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000053c4 Offset
//              0x0000180c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000020923a0df57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 41584
// PE header size       : 1024 (536 used)    ( 2.46%)
// PE additional info   : 10919             (26.26%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.17%)
// CLR meta-data size  : 27600              (66.37%)
// CLR additional info : 128                ( 0.31%)
// CLR method headers  : 340                ( 0.82%)
// Managed code         : 680               ( 1.64%)
// Data                 : 2560              ( 6.16%)
// Unaccounted          : -1739             (-4.18%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 28672
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 27600
//   Module        -    1 (10 bytes)
//   TypeDef       -   76 (1064 bytes)    0 interfaces, 1 explicit layout
//   TypeRef       -   84 (504 bytes)
//   MethodDef     -  340 (4760 bytes)    0 abstract, 0 native, 340 bodies
//   FieldDef      -  263 (1578 bytes)    7 constant
//   MemberRef     -   27 (162 bytes)
//   ParamDef      -  360 (2160 bytes)
//   MethodImpl    -   70 (420 bytes)
//   Constant      -   25 (150 bytes)
//   CustomAttribute-  301 (1806 bytes)
//   ClassLayout   -    1 (8 bytes)
//   InterfaceImpl -   37 (148 bytes)
//   PropertyMap   -   19 (76 bytes)
//   Property      -   73 (438 bytes)
//   MethodSemantic-   92 (552 bytes)
//   Security      -    1 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    6 (120 bytes)
//   NestedClass   -   43 (172 bytes)
//   FieldRVA      -    3 (18 bytes)
//   Strings       -  7033 bytes
//   Blobs         -  6156 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   217 bytes

// CLR additional info : 128

// CLR method headers : 340
//   Num.of method bodies  - 340
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 340

// Managed code : 680
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class EncoderResources               (abstract) (auto) (ansi) (sealed) 
// Class Base64UrlTextEncoder           (public) (abstract) (auto) (ansi) (sealed) 
// Class BufferedReadStream             (public) (auto) (ansi) 
// Class FileBufferingReadStream        (public) (auto) (ansi) 
// Class FileBufferingWriteStream       (public) (auto) (ansi) (sealed) 
// Class FileMultipartSection           (public) (auto) (ansi) 
// Class FormMultipartSection           (public) (auto) (ansi) 
// Class FormPipeReader                 (public) (auto) (ansi) 
// Class FormReader                     (public) (auto) (ansi) 
// Class HttpRequestStreamReader        (public) (auto) (ansi) 
// Class HttpResponseStreamWriter       (public) (auto) (ansi) 
// Class KeyValueAccumulator            (public) (sequential) (ansi) (sealed) 
// Class MultipartBoundary              (auto) (ansi) 
// Class MultipartReader                (public) (auto) (ansi) 
// Class MultipartReaderStream          (auto) (ansi) (sealed) 
// Class MultipartSection               (public) (auto) (ansi) 
// Class MultipartSectionConverterExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class MultipartSectionStreamExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class PagedByteBuffer                (auto) (ansi) (sealed) 
// Class QueryHelpers                   (public) (abstract) (auto) (ansi) (sealed) 
// Class ReasonPhrases                  (public) (abstract) (auto) (ansi) (sealed) 
// Class StreamHelperExtensions         (public) (abstract) (auto) (ansi) (sealed) 
// Class QueryStringEnumerable          (public) (sequential) (ansi) (sealed) 
// Class WebEncoders                    (public) (abstract) (auto) (ansi) (sealed) 
// Class Resources                      (abstract) (auto) (ansi) (sealed) 
// Class AspNetCoreTempDirectory        (abstract) (auto) (ansi) (sealed) 
// Class UrlDecoder                     (auto) (ansi) 
// Class <PrivateImplementationDetails> (auto) (ansi) (sealed) 
// Class <EnsureBufferedAsync>d__36     (auto) (ansi) (sealed) (nested private) 
// Class <EnsureBufferedAsync>d__38     (auto) (ansi) (sealed) (nested private) 
// Class <ReadAsync>d__34               (auto) (ansi) (sealed) (nested private) 
// Class <ReadLineAsync>d__40           (auto) (ansi) (sealed) (nested private) 
// Class <>c__DisplayClass45_0          (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__47            (auto) (ansi) (sealed) (nested private) 
// Class <ReadAsync>d__40               (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__41            (auto) (ansi) (sealed) (nested private) 
// Class <DrainBufferAsync>d__38        (auto) (ansi) (sealed) (nested private) 
// Class <DrainBufferAsync>d__39        (auto) (ansi) (sealed) (nested private) 
// Class <WriteAsync>d__33              (auto) (ansi) (sealed) (nested private) 
// Class <WriteAsync>d__34              (auto) (ansi) (sealed) (nested private) 
// Class <ReadFormAsync>d__26           (auto) (ansi) (sealed) (nested private) 
// Class <BufferAsync>d__41             (auto) (ansi) (sealed) (nested private) 
// Class <ReadFormAsync>d__43           (auto) (ansi) (sealed) (nested private) 
// Class <ReadNextPairAsync>d__33       (auto) (ansi) (sealed) (nested private) 
// Class <ReadNextPairAsyncImpl>d__34   (auto) (ansi) (sealed) (nested private) 
// Class ReadLineStepResult             (sequential) (ansi) (sealed) (nested private) 
// Class <ReadAsync>d__23               (auto) (ansi) (sealed) (nested private) 
// Class <ReadIntoBufferAsync>d__28     (auto) (ansi) (sealed) (nested private) 
// Class <ReadLineAsync>d__24           (auto) (ansi) (sealed) (nested private) 
// Class <ReadToEndAsync>d__29          (auto) (ansi) (sealed) (nested private) 
// Class <DisposeAsync>d__34            (auto) (ansi) (sealed) (nested private) 
// Class <FlushInternalAsync>d__36      (auto) (ansi) (sealed) (nested private) 
// Class <WriteAsyncAwaited>d__22       (auto) (ansi) (sealed) (nested private) 
// Class <WriteAsyncAwaited>d__24       (auto) (ansi) (sealed) (nested private) 
// Class <WriteAsyncAwaited>d__26       (auto) (ansi) (sealed) (nested private) 
// Class <WriteAsyncAwaited>d__28       (auto) (ansi) (sealed) (nested private) 
// Class <WriteLineAsyncAwaited>d__30   (auto) (ansi) (sealed) (nested private) 
// Class <ReadHeadersAsync>d__21        (auto) (ansi) (sealed) (nested private) 
// Class <ReadNextSectionAsync>d__20    (auto) (ansi) (sealed) (nested private) 
// Class <ReadAsync>d__36               (auto) (ansi) (sealed) (nested private) 
// Class <ReadAsStringAsync>d__0        (auto) (ansi) (sealed) (nested private) 
// Class <MoveToAsync>d__21             (auto) (ansi) (sealed) (nested private) 
// Class <MoveToAsync>d__22             (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <DrainAsync>d__3               (auto) (ansi) (sealed) (nested private) 
// Class EncodedNameValuePair           (sequential) (ansi) (sealed) (nested public) 
// Class Enumerator                     (sequential) (ansi) (sealed) (nested public) 
// Class SpanHelper                     (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class __StaticArrayInitTypeSize=256  (explicit) (ansi) (sealed) (nested private) 
// Class <<CopyToAsync>g__CopyToAsyncImpl|0>d (auto) (ansi) (sealed) (nested private) 
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
.assembly extern /*23000003*/ Microsoft.Net.Http.Headers
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.Extensions.Primitives
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ System.Memory
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.WebUtilities
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
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 73 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...sMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C   // pNetCore.WebUtil
                                                                                                                                                                                    69 74 69 65 73 2E 54 65 73 74 73 2C 20 50 75 62   // ities.Tests, Pub
                                                                                                                                                                                    6C 69 63 4B 65 79 3D 30 30 32 34 30 30 30 30 30   // licKey=002400000
                                                                                                                                                                                    34 38 30 30 30 30 30 39 34 30 30 30 30 30 30 30   // 4800000940000000
                                                                                                                                                                                    36 30 32 30 30 30 30 30 30 32 34 30 30 30 30 35   // 6020000002400005
                                                                                                                                                                                    32 35 33 34 31 33 31 30 30 30 34 30 30 30 30 30   // 2534131000400000
                                                                                                                                                                                    31 30 30 30 31 30 30 66 33 33 61 32 39 30 34 34   // 1000100f33a29044
                                                                                                                                                                                    66 61 39 64 37 34 30 63 39 62 33 32 31 33 61 39   // fa9d740c9b3213a9
                                                                                                                                                                                    33 65 35 37 63 38 34 62 34 37 32 63 38 34 65 30   // 3e57c84b472c84e0
                                                                                                                                                                                    62 38 61 30 65 31 61 65 34 38 65 36 37 61 39 66   // b8a0e1ae48e67a9f
                                                                                                                                                                                    38 66 36 64 65 39 64 35 66 37 66 33 64 35 32 61   // 8f6de9d5f7f3d52a
                                                                                                                                                                                    63 32 33 65 34 38 61 63 35 31 38 30 31 66 31 64   // c23e48ac51801f1d
                                                                                                                                                                                    63 39 35 30 61 62 65 39 30 31 64 61 33 34 64 32   // c950abe901da34d2
                                                                                                                                                                                    61 39 65 33 62 61 61 64 62 31 34 31 61 31 37 63   // a9e3baadb141a17c
                                                                                                                                                                                    37 37 65 66 33 63 35 36 35 64 64 35 65 65 35 30   // 77ef3c565dd5ee50
                                                                                                                                                                                    35 34 62 39 31 63 66 36 33 62 62 33 63 36 61 62   // 54b91cf63bb3c6ab
                                                                                                                                                                                    38 33 66 37 32 61 62 33 61 61 66 65 39 33 64 30   // 83f72ab3aafe93d0
                                                                                                                                                                                    66 63 33 63 32 33 34 38 62 37 36 34 66 61 66 62   // fc3c2348b764fafb
                                                                                                                                                                                    30 62 31 63 30 37 33 33 64 65 35 31 34 35 39 61   // 0b1c0733de51459a
                                                                                                                                                                                    65 61 62 34 36 35 38 30 33 38 34 62 66 39 64 37   // eab46580384bf9d7
                                                                                                                                                                                    34 63 34 65 32 38 31 36 34 62 37 63 64 65 32 34   // 4c4e28164b7cde24
                                                                                                                                                                                    37 66 38 39 31 62 61 30 37 38 39 31 63 39 64 38   // 7f891ba07891c9d8
                                                                                                                                                                                    37 32 61 64 32 62 62 00 00 )                      // 72ad2bb..
  .custom /*0C00000B:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 7D 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...}Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C   // pNetCore.WebUtil
                                                                                                                                                                                    69 74 69 65 73 2E 4D 69 63 72 6F 62 65 6E 63 68   // ities.Microbench
                                                                                                                                                                                    6D 61 72 6B 73 2C 20 50 75 62 6C 69 63 4B 65 79   // marks, PublicKey
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
  .custom /*0C00000C:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
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
  .custom /*0C000013:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 5E 41 53 50 2E 4E 45 54 20 43 6F 72 65 20   // ..^ASP.NET Core 
                                                                                                                                                                       75 74 69 6C 69 74 69 65 73 2C 20 73 75 63 68 20   // utilities, such 
                                                                                                                                                                       61 73 20 66 6F 72 20 77 6F 72 6B 69 6E 67 20 77   // as for working w
                                                                                                                                                                       69 74 68 20 66 6F 72 6D 73 2C 20 6D 75 6C 74 69   // ith forms, multi
                                                                                                                                                                       70 61 72 74 20 6D 65 73 73 61 67 65 73 2C 20 61   // part messages, a
                                                                                                                                                                       6E 64 20 71 75 65 72 79 20 73 74 72 69 6E 67 73   // nd query strings
                                                                                                                                                                       2E 00 00 )                                        // ...
  .custom /*0C000014:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000015:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000016:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 21 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..!Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                 74 69 65 73 00 00 )                               // ties..
  .custom /*0C000018:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000019:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.module Microsoft.AspNetCore.WebUtilities.dll
// MVID: {b2011336-5e24-4ebd-8203-81d421df13ba}
.custom /*0C000002:0A000013*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
.custom /*0C000003:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00001A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
  .custom /*0C000020:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
  .custom /*0C000023:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Microsoft.Extensions.WebEncoders.Sources.EncoderResources
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ static assembly literal string WebEncoders_InvalidCountOffsetOrLength = "Invalid {0}, {1} or {2} length."
  .field /*04000005*/ static assembly literal string WebEncoders_MalformedInput = "Malformed input: {0} is an invalid input length."
  .method /*06000006*/ assembly hidebysig static 
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

  .method /*06000007*/ assembly hidebysig static 
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

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000028:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000008*/ public hidebysig static 
          string  Encode(uint8[] data) cil managed
  // SIG: 00 01 0E 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Base64UrlTextEncoder::Encode

  .method /*06000009*/ public hidebysig static 
          uint8[]  Decode(string text) cil managed
  // SIG: 00 01 1D 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Base64UrlTextEncoder::Decode

} // end of class Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder

.class /*02000008*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.BufferedReadStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/
{
  .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000022*/ auto ansi sealed nested private beforefieldinit '<EnsureBufferedAsync>d__36'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00005E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000014*/ public int32 '<>1__state'
    .field /*04000015*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<bool> '<>t__builder'
    .field /*04000016*/ public class Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/ '<>4__this'
    .field /*04000017*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000018*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<int32> '<>u__1'
    .method /*060000FC*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<EnsureBufferedAsync>d__36'::MoveNext

    .method /*060000FD*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C0000FC:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<EnsureBufferedAsync>d__36'::SetStateMachine

  } // end of class '<EnsureBufferedAsync>d__36'

  .class /*02000023*/ auto ansi sealed nested private beforefieldinit '<EnsureBufferedAsync>d__38'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000060:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000019*/ public int32 '<>1__state'
    .field /*0400001A*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<bool> '<>t__builder'
    .field /*0400001B*/ public int32 minCount
    .field /*0400001C*/ public class Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/ '<>4__this'
    .field /*0400001D*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*0400001E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<int32> '<>u__1'
    .method /*060000FE*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<EnsureBufferedAsync>d__38'::MoveNext

    .method /*060000FF*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C0000FE:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<EnsureBufferedAsync>d__38'::SetStateMachine

  } // end of class '<EnsureBufferedAsync>d__38'

  .class /*02000024*/ auto ansi sealed nested private beforefieldinit '<ReadAsync>d__34'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000061:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400001F*/ public int32 '<>1__state'
    .field /*04000020*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100002B*/<int32> '<>t__builder'
    .field /*04000021*/ public class Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/ '<>4__this'
    .field /*04000022*/ public valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<uint8> buffer
    .field /*04000023*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000024*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<int32> '<>u__1'
    .method /*06000100*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__34'::MoveNext

    .method /*06000101*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000100:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__34'::SetStateMachine

  } // end of class '<ReadAsync>d__34'

  .class /*02000025*/ auto ansi sealed nested private beforefieldinit '<ReadLineAsync>d__40'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000062:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000025*/ public int32 '<>1__state'
    .field /*04000026*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<string> '<>t__builder'
    .field /*04000027*/ public class Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/ '<>4__this'
    .field /*04000028*/ public int32 lengthLimit
    .field /*04000029*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*0400002A*/ private class [System.Runtime/*23000001*/]System.IO.MemoryStream/*0100002C*/ '<builder>5__2'
    .field /*0400002B*/ private bool '<foundCR>5__3'
    .field /*0400002C*/ private bool '<foundCRLF>5__4'
    .field /*0400002D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<bool> '<>u__1'
    .method /*06000102*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLineAsync>d__40'::MoveNext

    .method /*06000103*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000101:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLineAsync>d__40'::SetStateMachine

  } // end of class '<ReadLineAsync>d__40'

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ inner,
                               int32 bufferSize) cil managed
  // SIG: 20 02 01 12 6D 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ inner,
                               int32 bufferSize,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> bytePool) cil managed
  // SIG: 20 03 01 12 6D 08 15 12 71 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::.ctor

  .method /*0600000C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*0100001D*/<uint8> 
          get_BufferedData() cil managed
  // SIG: 20 00 15 11 75 01 05
  {
    .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::get_BufferedData

  .method /*0600000D*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::get_CanRead

  .method /*0600000E*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::get_CanSeek

  .method /*0600000F*/ public hidebysig specialname virtual 
          instance bool  get_CanTimeout() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::get_CanTimeout

  .method /*06000010*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::get_CanWrite

  .method /*06000011*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::get_Length

  .method /*06000012*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::get_Position

  .method /*06000013*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::set_Position

  .method /*06000014*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*0100001E*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::Seek

  .method /*06000015*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::SetLength

  .method /*06000016*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::Dispose

  .method /*06000017*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::Flush

  .method /*06000018*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 01 12 7D 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::FlushAsync

  .method /*06000019*/ public hidebysig virtual 
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
  } // end of method BufferedReadStream::Write

  .method /*0600001A*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 04 12 7D 1D 05 08 08 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::WriteAsync

  .method /*0600001B*/ public hidebysig virtual 
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
  } // end of method BufferedReadStream::Read

  .method /*0600001C*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 85 01 08 1D 05 08 08 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::ReadAsync

  .method /*0600001D*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000024*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<uint8> buffer,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 91 01 08 15 11 80 95 01 05 11 80 81
  {
    .custom /*0C000055:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000056:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 45 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..EMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 42 75 66 66 65 72 65 64 52 65 61   // ties.BufferedRea
                                                                                                                                                                                                                                        64 53 74 72 65 61 6D 2B 3C 52 65 61 64 41 73 79   // dStream+<ReadAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 33 34 00 00 )                   // nc>d__34..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::ReadAsync

  .method /*0600001E*/ public hidebysig instance bool 
          EnsureBuffered() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::EnsureBuffered

  .method /*0600001F*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<bool> 
          EnsureBufferedAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 80 85 01 02 11 80 81
  {
    .custom /*0C000059:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..OMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 42 75 66 66 65 72 65 64 52 65 61   // ties.BufferedRea
                                                                                                                                                                                                                                        64 53 74 72 65 61 6D 2B 3C 45 6E 73 75 72 65 42   // dStream+<EnsureB
                                                                                                                                                                                                                                        75 66 66 65 72 65 64 41 73 79 6E 63 3E 64 5F 5F   // ufferedAsync>d__
                                                                                                                                                                                                                                        33 36 00 00 )                                     // 36..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::EnsureBufferedAsync

  .method /*06000020*/ public hidebysig instance bool 
          EnsureBuffered(int32 minCount) cil managed
  // SIG: 20 01 02 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::EnsureBuffered

  .method /*06000021*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<bool> 
          EnsureBufferedAsync(int32 minCount,
                              valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 80 85 01 02 08 11 80 81
  {
    .custom /*0C00005C:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..OMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 42 75 66 66 65 72 65 64 52 65 61   // ties.BufferedRea
                                                                                                                                                                                                                                        64 53 74 72 65 61 6D 2B 3C 45 6E 73 75 72 65 42   // dStream+<EnsureB
                                                                                                                                                                                                                                        75 66 66 65 72 65 64 41 73 79 6E 63 3E 64 5F 5F   // ufferedAsync>d__
                                                                                                                                                                                                                                        33 38 00 00 )                                     // 38..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::EnsureBufferedAsync

  .method /*06000022*/ public hidebysig instance string 
          ReadLine(int32 lengthLimit) cil managed
  // SIG: 20 01 0E 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::ReadLine

  .method /*06000023*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<string> 
          ReadLineAsync(int32 lengthLimit,
                        valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 15 12 80 85 01 0E 08 11 80 81
  {
    .custom /*0C00005F:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 49 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..IMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 42 75 66 66 65 72 65 64 52 65 61   // ties.BufferedRea
                                                                                                                                                                                                                                        64 53 74 72 65 61 6D 2B 3C 52 65 61 64 4C 69 6E   // dStream+<ReadLin
                                                                                                                                                                                                                                        65 41 73 79 6E 63 3E 64 5F 5F 34 30 00 00 )       // eAsync>d__40..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BufferedReadStream::ReadLineAsync

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*0100001D*/<uint8>
          BufferedData()
  {
    .custom /*0C000004:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.ArraySegment`1/*0100001D*/<uint8> Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/::get_BufferedData() /* 0600000C */
  } // end of property BufferedReadStream::BufferedData
  .property /*17000002*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/::get_CanRead() /* 0600000D */
  } // end of property BufferedReadStream::CanRead
  .property /*17000003*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/::get_CanSeek() /* 0600000E */
  } // end of property BufferedReadStream::CanSeek
  .property /*17000004*/ instance callconv(8) bool
          CanTimeout()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/::get_CanTimeout() /* 0600000F */
  } // end of property BufferedReadStream::CanTimeout
  .property /*17000005*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/::get_CanWrite() /* 06000010 */
  } // end of property BufferedReadStream::CanWrite
  .property /*17000006*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/::get_Length() /* 06000011 */
  } // end of property BufferedReadStream::Length
  .property /*17000007*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/::get_Position() /* 06000012 */
    .set instance void Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/::set_Position(int64) /* 06000013 */
  } // end of property BufferedReadStream::Position
} // end of class Microsoft.AspNetCore.WebUtilities.BufferedReadStream

.class /*02000009*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000026*/ auto ansi sealed nested private beforefieldinit '<>c__DisplayClass45_0'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C000063:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .class /*0200004C*/ auto ansi sealed nested private beforefieldinit '<<CopyToAsync>g__CopyToAsyncImpl|0>d'
           extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
           implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
    {
      .field /*04000102*/ public int32 '<>1__state'
      .field /*04000103*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
      .field /*04000104*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*//'<>c__DisplayClass45_0'/*02000026*/ '<>4__this'
      .field /*04000105*/ private uint8[] '<buffer>5__2'
      .field /*04000106*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<int32> '<>u__1'
      .field /*04000107*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__2'
      .method /*06000153*/ private hidebysig newslot virtual final 
              instance void  MoveNext() cil managed
      // SIG: 20 00 01
      {
        .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<<CopyToAsync>g__CopyToAsyncImpl|0>d'::MoveNext

      .method /*06000154*/ private hidebysig newslot virtual final 
              instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
      // SIG: 20 01 01 12 80 9D
      {
        .custom /*0C00012D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
        .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<<CopyToAsync>g__CopyToAsyncImpl|0>d'::SetStateMachine

    } // end of class '<<CopyToAsync>g__CopyToAsyncImpl|0>d'

    .field /*0400002E*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/ '<>4__this'
    .field /*0400002F*/ public int32 bufferSize
    .field /*04000030*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000031*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ destination
    .method /*06000104*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass45_0'::.ctor

    .method /*06000105*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
            '<CopyToAsync>g__CopyToAsyncImpl|0'() cil managed
    // SIG: 20 00 12 7D
    {
      .custom /*0C000102:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 74 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..tMicrosoft.Asp
                                                                                                                                                                                                                                          4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                          74 69 65 73 2E 46 69 6C 65 42 75 66 66 65 72 69   // ties.FileBufferi
                                                                                                                                                                                                                                          6E 67 52 65 61 64 53 74 72 65 61 6D 2B 3C 3E 63   // ngReadStream+<>c
                                                                                                                                                                                                                                          5F 5F 44 69 73 70 6C 61 79 43 6C 61 73 73 34 35   // __DisplayClass45
                                                                                                                                                                                                                                          5F 30 2B 3C 3C 43 6F 70 79 54 6F 41 73 79 6E 63   // _0+<<CopyToAsync
                                                                                                                                                                                                                                          3E 67 5F 5F 43 6F 70 79 54 6F 41 73 79 6E 63 49   // >g__CopyToAsyncI
                                                                                                                                                                                                                                          6D 70 6C 7C 30 3E 64 00 00 )                      // mpl|0>d..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c__DisplayClass45_0'::'<CopyToAsync>g__CopyToAsyncImpl|0'

  } // end of class '<>c__DisplayClass45_0'

  .class /*02000027*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__47'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000064:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000032*/ public int32 '<>1__state'
    .field /*04000033*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000034*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/ '<>4__this'
    .field /*04000035*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__1'
    .method /*06000106*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__47'::MoveNext

    .method /*06000107*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000104:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__47'::SetStateMachine

  } // end of class '<DisposeAsync>d__47'

  .class /*02000028*/ auto ansi sealed nested private beforefieldinit '<ReadAsync>d__40'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000065:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000036*/ public int32 '<>1__state'
    .field /*04000037*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100002B*/<int32> '<>t__builder'
    .field /*04000038*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/ '<>4__this'
    .field /*04000039*/ public valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<uint8> buffer
    .field /*0400003A*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*0400003B*/ private int32 '<read>5__2'
    .field /*0400003C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<int32> '<>u__1'
    .field /*0400003D*/ private class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ '<oldBuffer>5__3'
    .field /*0400003E*/ private uint8[] '<rentedBuffer>5__4'
    .field /*0400003F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__2'
    .method /*06000108*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__40'::MoveNext

    .method /*06000109*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000106:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__40'::SetStateMachine

  } // end of class '<ReadAsync>d__40'

  .method /*06000024*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ inner,
                               int32 memoryThreshold) cil managed
  // SIG: 20 02 01 12 6D 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::.ctor

  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ inner,
                               int32 memoryThreshold,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> bufferLimit,
                               class [System.Runtime/*23000001*/]System.Func`1/*0100002F*/<string> tempFileDirectoryAccessor) cil managed
  // SIG: 20 04 01 12 6D 08 15 11 80 B9 01 0A 15 12 80 BD 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::.ctor

  .method /*06000026*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ inner,
                               int32 memoryThreshold,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> bufferLimit,
                               class [System.Runtime/*23000001*/]System.Func`1/*0100002F*/<string> tempFileDirectoryAccessor,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> bytePool) cil managed
  // SIG: 20 05 01 12 6D 08 15 11 80 B9 01 0A 15 12 80 BD 01 0E 15 12 71 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::.ctor

  .method /*06000027*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ inner,
                               int32 memoryThreshold,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> bufferLimit,
                               string tempFileDirectory) cil managed
  // SIG: 20 04 01 12 6D 08 15 11 80 B9 01 0A 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::.ctor

  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ inner,
                               int32 memoryThreshold,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> bufferLimit,
                               string tempFileDirectory,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> bytePool) cil managed
  // SIG: 20 05 01 12 6D 08 15 11 80 B9 01 0A 0E 15 12 71 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::.ctor

  .method /*06000029*/ public hidebysig specialname 
          instance int32  get_MemoryThreshold() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::get_MemoryThreshold

  .method /*0600002A*/ public hidebysig specialname 
          instance bool  get_InMemory() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::get_InMemory

  .method /*0600002B*/ public hidebysig specialname 
          instance string  get_TempFileName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000068:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::get_TempFileName

  .method /*0600002C*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::get_CanRead

  .method /*0600002D*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::get_CanSeek

  .method /*0600002E*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::get_CanWrite

  .method /*0600002F*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::get_Length

  .method /*06000030*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::get_Position

  .method /*06000031*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::set_Position

  .method /*06000032*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*0100001E*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::Seek

  .method /*06000033*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<uint8> buffer) cil managed
  // SIG: 20 01 08 15 11 80 C1 01 05
  {
    .custom /*0C000071:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::Read

  .method /*06000034*/ public hidebysig virtual 
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
  } // end of method FileBufferingReadStream::Read

  .method /*06000035*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 85 01 08 1D 05 08 08 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::ReadAsync

  .method /*06000036*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000024*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 91 01 08 15 11 80 95 01 05 11 80 81
  {
    .custom /*0C000075:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000076:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..JMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 69 6C 65 42 75 66 66 65 72 69   // ties.FileBufferi
                                                                                                                                                                                                                                        6E 67 52 65 61 64 53 74 72 65 61 6D 2B 3C 52 65   // ngReadStream+<Re
                                                                                                                                                                                                                                        61 64 41 73 79 6E 63 3E 64 5F 5F 34 30 00 00 )    // adAsync>d__40..
    .param [2]/*08000049*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::ReadAsync

  .method /*06000037*/ public hidebysig virtual 
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
  } // end of method FileBufferingReadStream::Write

  .method /*06000038*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 04 12 7D 1D 05 08 08 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::WriteAsync

  .method /*06000039*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::SetLength

  .method /*0600003A*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::Flush

  .method /*0600003B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ destination,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 03 12 7D 12 6D 08 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::CopyToAsync

  .method /*0600003C*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::Dispose

  .method /*0600003D*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000031*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 C5
  {
    .custom /*0C000080:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..MMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 69 6C 65 42 75 66 66 65 72 69   // ties.FileBufferi
                                                                                                                                                                                                                                        6E 67 52 65 61 64 53 74 72 65 61 6D 2B 3C 44 69   // ngReadStream+<Di
                                                                                                                                                                                                                                        73 70 6F 73 65 41 73 79 6E 63 3E 64 5F 5F 34 37   // sposeAsync>d__47
                                                                                                                                                                                                                                        00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingReadStream::DisposeAsync

  .property /*17000008*/ instance callconv(8) int32
          MemoryThreshold()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::get_MemoryThreshold() /* 06000029 */
  } // end of property FileBufferingReadStream::MemoryThreshold
  .property /*17000009*/ instance callconv(8) bool
          InMemory()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::get_InMemory() /* 0600002A */
  } // end of property FileBufferingReadStream::InMemory
  .property /*1700000A*/ instance callconv(8) string
          TempFileName()
  {
    .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::get_TempFileName() /* 0600002B */
  } // end of property FileBufferingReadStream::TempFileName
  .property /*1700000B*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::get_CanRead() /* 0600002C */
  } // end of property FileBufferingReadStream::CanRead
  .property /*1700000C*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::get_CanSeek() /* 0600002D */
  } // end of property FileBufferingReadStream::CanSeek
  .property /*1700000D*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::get_CanWrite() /* 0600002E */
  } // end of property FileBufferingReadStream::CanWrite
  .property /*1700000E*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::get_Length() /* 0600002F */
  } // end of property FileBufferingReadStream::Length
  .property /*1700000F*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::get_Position() /* 06000030 */
    .set instance void Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream/*02000009*/::set_Position(int64) /* 06000031 */
  } // end of property FileBufferingReadStream::Position
} // end of class Microsoft.AspNetCore.WebUtilities.FileBufferingReadStream

.class /*0200000A*/ public auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/
{
  .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000029*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__41'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000066:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000040*/ public int32 '<>1__state'
    .field /*04000041*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000042*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/ '<>4__this'
    .field /*04000043*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__1'
    .method /*0600010A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__41'::MoveNext

    .method /*0600010B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000108:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__41'::SetStateMachine

  } // end of class '<DisposeAsync>d__41'

  .class /*0200002A*/ auto ansi sealed nested private beforefieldinit '<DrainBufferAsync>d__38'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000067:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000044*/ public int32 '<>1__state'
    .field /*04000045*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*04000046*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/ '<>4__this'
    .field /*04000047*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ destination
    .field /*04000048*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000049*/ private class [System.Runtime/*23000001*/]System.IO.FileStream/*01000035*/ '<readStream>5__2'
    .field /*0400004A*/ private object '<>7__wrap2'
    .field /*0400004B*/ private int32 '<>7__wrap3'
    .field /*0400004C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .field /*0400004D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__2'
    .method /*0600010C*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DrainBufferAsync>d__38'::MoveNext

    .method /*0600010D*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00010B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DrainBufferAsync>d__38'::SetStateMachine

  } // end of class '<DrainBufferAsync>d__38'

  .class /*0200002B*/ auto ansi sealed nested private beforefieldinit '<DrainBufferAsync>d__39'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000069:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400004E*/ public int32 '<>1__state'
    .field /*0400004F*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*04000050*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/ '<>4__this'
    .field /*04000051*/ public class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000037*/ destination
    .field /*04000052*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000053*/ private class [System.Runtime/*23000001*/]System.IO.FileStream/*01000035*/ '<readStream>5__2'
    .field /*04000054*/ private object '<>7__wrap2'
    .field /*04000055*/ private int32 '<>7__wrap3'
    .field /*04000056*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .field /*04000057*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__2'
    .method /*0600010E*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DrainBufferAsync>d__39'::MoveNext

    .method /*0600010F*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00010C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DrainBufferAsync>d__39'::SetStateMachine

  } // end of class '<DrainBufferAsync>d__39'

  .class /*0200002C*/ auto ansi sealed nested private beforefieldinit '<WriteAsync>d__33'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00006A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000058*/ public int32 '<>1__state'
    .field /*04000059*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*0400005A*/ public uint8[] buffer
    .field /*0400005B*/ public int32 offset
    .field /*0400005C*/ public int32 count
    .field /*0400005D*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/ '<>4__this'
    .field /*0400005E*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*0400005F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__1'
    .method /*06000110*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsync>d__33'::MoveNext

    .method /*06000111*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00010D:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsync>d__33'::SetStateMachine

  } // end of class '<WriteAsync>d__33'

  .class /*0200002D*/ auto ansi sealed nested private beforefieldinit '<WriteAsync>d__34'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00006B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000060*/ public int32 '<>1__state'
    .field /*04000061*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000062*/ public class Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/ '<>4__this'
    .field /*04000063*/ public valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<uint8> buffer
    .field /*04000064*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000065*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .field /*04000066*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__2'
    .method /*06000112*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsync>d__34'::MoveNext

    .method /*06000113*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00010E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsync>d__34'::SetStateMachine

  } // end of class '<WriteAsync>d__34'

  .method /*0600003E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor([opt] int32 memoryThreshold,
                               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> bufferLimit,
                               [opt] class [System.Runtime/*23000001*/]System.Func`1/*0100002F*/<string> tempFileDirectoryAccessor) cil managed
  // SIG: 20 03 01 08 15 11 80 B9 01 0A 15 12 80 BD 01 0E
  {
    .param [1]/*08000056*/  = int32(0x00008000)
    .param [2]/*08000057*/  = nullref
    .param [3]/*08000058*/  = nullref
    .custom /*0C000098:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::.ctor

  .method /*0600003F*/ public hidebysig specialname 
          instance int32  get_MemoryThreshold() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_MemoryThreshold

  .method /*06000040*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_CanRead

  .method /*06000041*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_CanSeek

  .method /*06000042*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_CanWrite

  .method /*06000043*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_Length

  .method /*06000044*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_Position

  .method /*06000045*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::set_Position

  .method /*06000046*/ assembly hidebysig specialname 
          instance class Microsoft.AspNetCore.WebUtilities.PagedByteBuffer/*02000018*/ 
          get_PagedByteBuffer() cil managed
  // SIG: 20 00 12 60
  {
    .custom /*0C00008A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_PagedByteBuffer

  .method /*06000047*/ assembly hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.FileStream/*01000035*/ 
          get_FileStream() cil managed
  // SIG: 20 00 12 80 D5
  {
    .custom /*0C00008C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00008D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_FileStream

  .method /*06000048*/ assembly hidebysig specialname 
          instance bool  get_Disposed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00008F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::get_Disposed

  .method /*06000049*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*0100001E*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::Seek

  .method /*0600004A*/ public hidebysig virtual 
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
  } // end of method FileBufferingWriteStream::Read

  .method /*0600004B*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 85 01 08 1D 05 08 08 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::ReadAsync

  .method /*0600004C*/ public hidebysig virtual 
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
  } // end of method FileBufferingWriteStream::Write

  .method /*0600004D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 04 12 7D 1D 05 08 08 11 80 81
  {
    .custom /*0C000091:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4C 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..LMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 69 6C 65 42 75 66 66 65 72 69   // ties.FileBufferi
                                                                                                                                                                                                                                        6E 67 57 72 69 74 65 53 74 72 65 61 6D 2B 3C 57   // ngWriteStream+<W
                                                                                                                                                                                                                                        72 69 74 65 41 73 79 6E 63 3E 64 5F 5F 33 33 00   // riteAsync>d__33.
                                                                                                                                                                                                                                        00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::WriteAsync

  .method /*0600004E*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000031*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 11 80 C5 15 11 80 D9 01 05 11 80 81
  {
    .custom /*0C000092:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000093:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4C 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..LMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 69 6C 65 42 75 66 66 65 72 69   // ties.FileBufferi
                                                                                                                                                                                                                                        6E 67 57 72 69 74 65 53 74 72 65 61 6D 2B 3C 57   // ngWriteStream+<W
                                                                                                                                                                                                                                        72 69 74 65 41 73 79 6E 63 3E 64 5F 5F 33 34 00   // riteAsync>d__34.
                                                                                                                                                                                                                                        00 ) 
    .param [2]/*0800006B*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::WriteAsync

  .method /*0600004F*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::Flush

  .method /*06000050*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 01 12 7D 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::FlushAsync

  .method /*06000051*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::SetLength

  .method /*06000052*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          DrainBufferAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ destination,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 12 7D 12 6D 11 80 81
  {
    .custom /*0C000094:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 52 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..RMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 69 6C 65 42 75 66 66 65 72 69   // ties.FileBufferi
                                                                                                                                                                                                                                        6E 67 57 72 69 74 65 53 74 72 65 61 6D 2B 3C 44   // ngWriteStream+<D
                                                                                                                                                                                                                                        72 61 69 6E 42 75 66 66 65 72 41 73 79 6E 63 3E   // rainBufferAsync>
                                                                                                                                                                                                                                        64 5F 5F 33 38 00 00 )                            // d__38..
    .param [2]/*0800006F*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::DrainBufferAsync

  .method /*06000053*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          DrainBufferAsync(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000037*/ destination,
                           [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 12 7D 12 80 DD 11 80 81
  {
    .custom /*0C000095:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 52 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..RMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 69 6C 65 42 75 66 66 65 72 69   // ties.FileBufferi
                                                                                                                                                                                                                                        6E 67 57 72 69 74 65 53 74 72 65 61 6D 2B 3C 44   // ngWriteStream+<D
                                                                                                                                                                                                                                        72 61 69 6E 42 75 66 66 65 72 41 73 79 6E 63 3E   // rainBufferAsync>
                                                                                                                                                                                                                                        64 5F 5F 33 39 00 00 )                            // d__39..
    .param [2]/*08000071*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::DrainBufferAsync

  .method /*06000054*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::Dispose

  .method /*06000055*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000031*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 C5
  {
    .custom /*0C000096:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 69 6C 65 42 75 66 66 65 72 69   // ties.FileBufferi
                                                                                                                                                                                                                                        6E 67 57 72 69 74 65 53 74 72 65 61 6D 2B 3C 44   // ngWriteStream+<D
                                                                                                                                                                                                                                        69 73 70 6F 73 65 41 73 79 6E 63 3E 64 5F 5F 34   // isposeAsync>d__4
                                                                                                                                                                                                                                        31 00 00 )                                        // 1..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileBufferingWriteStream::DisposeAsync

  .property /*17000010*/ instance callconv(8) int32
          MemoryThreshold()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_MemoryThreshold() /* 0600003F */
  } // end of property FileBufferingWriteStream::MemoryThreshold
  .property /*17000011*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_CanRead() /* 06000040 */
  } // end of property FileBufferingWriteStream::CanRead
  .property /*17000012*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_CanSeek() /* 06000041 */
  } // end of property FileBufferingWriteStream::CanSeek
  .property /*17000013*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_CanWrite() /* 06000042 */
  } // end of property FileBufferingWriteStream::CanWrite
  .property /*17000014*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_Length() /* 06000043 */
  } // end of property FileBufferingWriteStream::Length
  .property /*17000015*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_Position() /* 06000044 */
    .set instance void Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::set_Position(int64) /* 06000045 */
  } // end of property FileBufferingWriteStream::Position
  .property /*17000016*/ instance callconv(8) class Microsoft.AspNetCore.WebUtilities.PagedByteBuffer/*02000018*/
          PagedByteBuffer()
  {
    .get instance class Microsoft.AspNetCore.WebUtilities.PagedByteBuffer/*02000018*/ Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_PagedByteBuffer() /* 06000046 */
  } // end of property FileBufferingWriteStream::PagedByteBuffer
  .property /*17000017*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.FileStream/*01000035*/
          FileStream()
  {
    .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.IO.FileStream/*01000035*/ Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_FileStream() /* 06000047 */
  } // end of property FileBufferingWriteStream::FileStream
  .property /*17000018*/ instance callconv(8) bool
          Disposed()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream/*0200000A*/::get_Disposed() /* 06000048 */
  } // end of property FileBufferingWriteStream::Disposed
} // end of class Microsoft.AspNetCore.WebUtilities.FileBufferingWriteStream

.class /*0200000B*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.FileMultipartSection
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000056*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section) cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileMultipartSection::.ctor

  .method /*06000057*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section,
                               class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.ContentDispositionHeaderValue/*01000039*/ header) cil managed
  // SIG: 20 02 01 12 54 12 80 E5
  {
    .param [2]/*08000075*/ 
    .custom /*0C0000AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileMultipartSection::.ctor

  .method /*06000058*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ 
          get_Section() cil managed
  // SIG: 20 00 12 54
  {
    .custom /*0C000097:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileMultipartSection::get_Section

  .method /*06000059*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 
          get_FileStream() cil managed
  // SIG: 20 00 12 6D
  {
    .custom /*0C000099:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileMultipartSection::get_FileStream

  .method /*0600005A*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00009A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileMultipartSection::get_Name

  .method /*0600005B*/ public hidebysig specialname 
          instance string  get_FileName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00009B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileMultipartSection::get_FileName

  .property /*17000019*/ instance callconv(8) class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/
          Section()
  {
    .get instance class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ Microsoft.AspNetCore.WebUtilities.FileMultipartSection/*0200000B*/::get_Section() /* 06000058 */
  } // end of property FileMultipartSection::Section
  .property /*1700001A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/
          FileStream()
  {
    .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ Microsoft.AspNetCore.WebUtilities.FileMultipartSection/*0200000B*/::get_FileStream() /* 06000059 */
  } // end of property FileMultipartSection::FileStream
  .property /*1700001B*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.AspNetCore.WebUtilities.FileMultipartSection/*0200000B*/::get_Name() /* 0600005A */
  } // end of property FileMultipartSection::Name
  .property /*1700001C*/ instance callconv(8) string
          FileName()
  {
    .get instance string Microsoft.AspNetCore.WebUtilities.FileMultipartSection/*0200000B*/::get_FileName() /* 0600005B */
  } // end of property FileMultipartSection::FileName
} // end of class Microsoft.AspNetCore.WebUtilities.FileMultipartSection

.class /*0200000C*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.FormMultipartSection
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section) cil managed
  // SIG: 20 01 01 12 54
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormMultipartSection::.ctor

  .method /*0600005D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section,
                               class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.ContentDispositionHeaderValue/*01000039*/ header) cil managed
  // SIG: 20 02 01 12 54 12 80 E5
  {
    .param [2]/*08000078*/ 
    .custom /*0C0000B0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormMultipartSection::.ctor

  .method /*0600005E*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ 
          get_Section() cil managed
  // SIG: 20 00 12 54
  {
    .custom /*0C00009C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormMultipartSection::get_Section

  .method /*0600005F*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00009D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormMultipartSection::get_Name

  .method /*06000060*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<string> 
          GetValueAsync() cil managed
  // SIG: 20 00 15 12 80 85 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormMultipartSection::GetValueAsync

  .property /*1700001D*/ instance callconv(8) class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/
          Section()
  {
    .get instance class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ Microsoft.AspNetCore.WebUtilities.FormMultipartSection/*0200000C*/::get_Section() /* 0600005E */
  } // end of property FormMultipartSection::Section
  .property /*1700001E*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.AspNetCore.WebUtilities.FormMultipartSection/*0200000C*/::get_Name() /* 0600005F */
  } // end of property FormMultipartSection::Name
} // end of class Microsoft.AspNetCore.WebUtilities.FormMultipartSection

.class /*0200000D*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.FormPipeReader
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .class /*0200002E*/ auto ansi sealed nested private beforefieldinit '<ReadFormAsync>d__26'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00006C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000067*/ public int32 '<>1__state'
    .field /*04000068*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>> '<>t__builder'
    .field /*04000069*/ public class Microsoft.AspNetCore.WebUtilities.FormPipeReader/*0200000D*/ '<>4__this'
    .field /*0400006A*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*0400006B*/ private valuetype Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator/*02000011*/ '<accumulator>5__2'
    .field /*0400006C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.ReadResult/*0100003F*/> '<>u__1'
    .method /*06000114*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadFormAsync>d__26'::MoveNext

    .method /*06000115*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00010F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadFormAsync>d__26'::SetStateMachine

  } // end of class '<ReadFormAsync>d__26'

  .method /*06000061*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*0100003A*/ pipeReader) cil managed
  // SIG: 20 01 01 12 80 E9
  {
    .custom /*0C00009E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::.ctor

  .method /*06000062*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeReader/*0100003A*/ pipeReader,
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding) cil managed
  // SIG: 20 02 01 12 80 E9 12 80 ED
  {
    .custom /*0C00009F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::.ctor

  .method /*06000063*/ public hidebysig specialname 
          instance int32  get_ValueCountLimit() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000A0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::get_ValueCountLimit

  .method /*06000064*/ public hidebysig specialname 
          instance void  set_ValueCountLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000A1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::set_ValueCountLimit

  .method /*06000065*/ public hidebysig specialname 
          instance int32  get_KeyLengthLimit() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000A2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::get_KeyLengthLimit

  .method /*06000066*/ public hidebysig specialname 
          instance void  set_KeyLengthLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000A3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::set_KeyLengthLimit

  .method /*06000067*/ public hidebysig specialname 
          instance int32  get_ValueLengthLimit() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000A4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::get_ValueLengthLimit

  .method /*06000068*/ public hidebysig specialname 
          instance void  set_ValueLengthLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000A5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::set_ValueLengthLimit

  .method /*06000069*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>> 
          ReadFormAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 80 85 01 15 12 80 F1 02 0E 11 80 F5 11 80 81
  {
    .custom /*0C0000A6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000A7:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 45 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..EMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 6F 72 6D 50 69 70 65 52 65 61   // ties.FormPipeRea
                                                                                                                                                                                                                                        64 65 72 2B 3C 52 65 61 64 46 6F 72 6D 41 73 79   // der+<ReadFormAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 32 36 00 00 )                   // nc>d__26..
    .param [1]/*0800007F*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::ReadFormAsync

  .method /*0600006A*/ assembly hidebysig 
          instance void  ParseFormValues(valuetype [System.Memory/*23000006*/]System.Buffers.ReadOnlySequence`1/*0100003E*/<uint8>& buffer,
                                         valuetype Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator/*02000011*/& accumulator,
                                         bool isFinalBlock) cil managed noinlining
  // SIG: 20 03 01 10 15 11 80 F9 01 05 10 11 44 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormPipeReader::ParseFormValues

  .property /*1700001F*/ instance callconv(8) int32
          ValueCountLimit()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.FormPipeReader/*0200000D*/::get_ValueCountLimit() /* 06000063 */
    .set instance void Microsoft.AspNetCore.WebUtilities.FormPipeReader/*0200000D*/::set_ValueCountLimit(int32) /* 06000064 */
  } // end of property FormPipeReader::ValueCountLimit
  .property /*17000020*/ instance callconv(8) int32
          KeyLengthLimit()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.FormPipeReader/*0200000D*/::get_KeyLengthLimit() /* 06000065 */
    .set instance void Microsoft.AspNetCore.WebUtilities.FormPipeReader/*0200000D*/::set_KeyLengthLimit(int32) /* 06000066 */
  } // end of property FormPipeReader::KeyLengthLimit
  .property /*17000021*/ instance callconv(8) int32
          ValueLengthLimit()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.FormPipeReader/*0200000D*/::get_ValueLengthLimit() /* 06000067 */
    .set instance void Microsoft.AspNetCore.WebUtilities.FormPipeReader/*0200000D*/::set_ValueLengthLimit(int32) /* 06000068 */
  } // end of property FormPipeReader::ValueLengthLimit
} // end of class Microsoft.AspNetCore.WebUtilities.FormPipeReader

.class /*0200000E*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.FormReader
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000040*/
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000040*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002F*/ auto ansi sealed nested private beforefieldinit '<BufferAsync>d__41'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00006D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400006D*/ public int32 '<>1__state'
    .field /*0400006E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*0400006F*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000070*/ public class Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/ '<>4__this'
    .field /*04000071*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<int32> '<>u__1'
    .method /*06000116*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<BufferAsync>d__41'::MoveNext

    .method /*06000117*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000110:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<BufferAsync>d__41'::SetStateMachine

  } // end of class '<BufferAsync>d__41'

  .class /*02000030*/ auto ansi sealed nested private beforefieldinit '<ReadFormAsync>d__43'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00006E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000072*/ public int32 '<>1__state'
    .field /*04000073*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>> '<>t__builder'
    .field /*04000074*/ public class Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/ '<>4__this'
    .field /*04000075*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000076*/ private valuetype Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator/*02000011*/ '<accumulator>5__2'
    .field /*04000077*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .method /*06000118*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadFormAsync>d__43'::MoveNext

    .method /*06000119*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000111:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadFormAsync>d__43'::SetStateMachine

  } // end of class '<ReadFormAsync>d__43'

  .class /*02000031*/ auto ansi sealed nested private beforefieldinit '<ReadNextPairAsync>d__33'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00006F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000078*/ public int32 '<>1__state'
    .field /*04000079*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,string>>> '<>t__builder'
    .field /*0400007A*/ public class Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/ '<>4__this'
    .field /*0400007B*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*0400007C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .method /*0600011A*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadNextPairAsync>d__33'::MoveNext

    .method /*0600011B*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000112:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadNextPairAsync>d__33'::SetStateMachine

  } // end of class '<ReadNextPairAsync>d__33'

  .class /*02000032*/ auto ansi sealed nested private beforefieldinit '<ReadNextPairAsyncImpl>d__34'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000070:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400007D*/ public int32 '<>1__state'
    .field /*0400007E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*0400007F*/ public class Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/ '<>4__this'
    .field /*04000080*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*04000081*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .method /*0600011C*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadNextPairAsyncImpl>d__34'::MoveNext

    .method /*0600011D*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000113:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadNextPairAsyncImpl>d__34'::SetStateMachine

  } // end of class '<ReadNextPairAsyncImpl>d__34'

  .field /*04000006*/ public static literal int32 DefaultValueCountLimit = int32(0x00000400)
  .field /*04000007*/ public static literal int32 DefaultKeyLengthLimit = int32(0x00000800)
  .field /*04000008*/ public static literal int32 DefaultValueLengthLimit = int32(0x00400000)
  .method /*0600006B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string data) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::.ctor

  .method /*0600006C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string data,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<char> charPool) cil managed
  // SIG: 20 02 01 0E 15 12 71 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::.ctor

  .method /*0600006D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::.ctor

  .method /*0600006E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding) cil managed
  // SIG: 20 02 01 12 6D 12 80 ED
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::.ctor

  .method /*0600006F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<char> charPool) cil managed
  // SIG: 20 03 01 12 6D 12 80 ED 15 12 71 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::.ctor

  .method /*06000070*/ public hidebysig specialname 
          instance int32  get_ValueCountLimit() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000A8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::get_ValueCountLimit

  .method /*06000071*/ public hidebysig specialname 
          instance void  set_ValueCountLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000A9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::set_ValueCountLimit

  .method /*06000072*/ public hidebysig specialname 
          instance int32  get_KeyLengthLimit() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000AA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::get_KeyLengthLimit

  .method /*06000073*/ public hidebysig specialname 
          instance void  set_KeyLengthLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000AB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::set_KeyLengthLimit

  .method /*06000074*/ public hidebysig specialname 
          instance int32  get_ValueLengthLimit() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000AC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::get_ValueLengthLimit

  .method /*06000075*/ public hidebysig specialname 
          instance void  set_ValueLengthLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000AD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::set_ValueLengthLimit

  .method /*06000076*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,string>> 
          ReadNextPair() cil managed
  // SIG: 20 00 15 11 80 B9 01 15 11 81 05 02 0E 0E
  {
    .param [0]/*0800008F*/ 
    .custom /*0C0000BD:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::ReadNextPair

  .method /*06000077*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,string>>> 
          ReadNextPairAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 80 85 01 15 11 80 B9 01 15 11 81 05 02 0E 0E 11 80 81
  {
    .custom /*0C0000AF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 45 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..EMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 6F 72 6D 52 65 61 64 65 72 2B   // ties.FormReader+
                                                                                                                                                                                                                                        3C 52 65 61 64 4E 65 78 74 50 61 69 72 41 73 79   // <ReadNextPairAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 33 33 00 00 )                   // nc>d__33..
    .param [0]/*08000090*/ 
    .custom /*0C0000BF:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 01 00 00 ) 
    .param [1]/*08000091*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::ReadNextPairAsync

  .method /*06000078*/ public hidebysig instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> 
          ReadForm() cil managed
  // SIG: 20 00 15 12 80 F1 02 0E 11 80 F5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::ReadForm

  .method /*06000079*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>> 
          ReadFormAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 80 85 01 15 12 80 F1 02 0E 11 80 F5 11 80 81
  {
    .custom /*0C0000B1:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 41 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..AMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 46 6F 72 6D 52 65 61 64 65 72 2B   // ties.FormReader+
                                                                                                                                                                                                                                        3C 52 65 61 64 46 6F 72 6D 41 73 79 6E 63 3E 64   // <ReadFormAsync>d
                                                                                                                                                                                                                                        5F 5F 34 33 00 00 )                               // __43..
    .param [1]/*08000092*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::ReadFormAsync

  .method /*0600007A*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FormReader::Dispose

  .property /*17000022*/ instance callconv(8) int32
          ValueCountLimit()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/::get_ValueCountLimit() /* 06000070 */
    .set instance void Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/::set_ValueCountLimit(int32) /* 06000071 */
  } // end of property FormReader::ValueCountLimit
  .property /*17000023*/ instance callconv(8) int32
          KeyLengthLimit()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/::get_KeyLengthLimit() /* 06000072 */
    .set instance void Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/::set_KeyLengthLimit(int32) /* 06000073 */
  } // end of property FormReader::KeyLengthLimit
  .property /*17000024*/ instance callconv(8) int32
          ValueLengthLimit()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/::get_ValueLengthLimit() /* 06000074 */
    .set instance void Microsoft.AspNetCore.WebUtilities.FormReader/*0200000E*/::set_ValueLengthLimit(int32) /* 06000075 */
  } // end of property FormReader::ValueLengthLimit
} // end of class Microsoft.AspNetCore.WebUtilities.FormReader

.class /*0200000F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader
       extends [System.Runtime/*23000001*/]System.IO.TextReader/*01000042*/
{
  .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000033*/ sequential ansi sealed nested private beforefieldinit ReadLineStepResult
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
  {
    .custom /*0C000072:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000043*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    .field /*04000082*/ public static initonly valuetype Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*//ReadLineStepResult/*02000033*/ Done
    .field /*04000083*/ public static initonly valuetype Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*//ReadLineStepResult/*02000033*/ Continue
    .field /*04000084*/ private initonly bool '<Completed>k__BackingField'
    .custom /*0C0000B5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000085*/ private initonly string '<Result>k__BackingField'
    .custom /*0C0000B7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .method /*0600011E*/ public hidebysig static 
            valuetype Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*//ReadLineStepResult/*02000033*/ 
            FromResult(string 'value') cil managed
    // SIG: 00 01 11 80 CC 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ReadLineStepResult::FromResult

    .method /*0600011F*/ public hidebysig specialname 
            instance bool  get_Completed() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C000114:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ReadLineStepResult::get_Completed

    .method /*06000120*/ public hidebysig specialname 
            instance string  get_Result() cil managed
    // SIG: 20 00 0E
    {
      .custom /*0C000115:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method ReadLineStepResult::get_Result

    .property /*17000045*/ instance callconv(8) bool
            Completed()
    {
      .get instance bool Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*//ReadLineStepResult/*02000033*/::get_Completed() /* 0600011F */
    } // end of property ReadLineStepResult::Completed
    .property /*17000046*/ instance callconv(8) string
            Result()
    {
      .get instance string Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*//ReadLineStepResult/*02000033*/::get_Result() /* 06000120 */
    } // end of property ReadLineStepResult::Result
  } // end of class ReadLineStepResult

  .class /*02000034*/ auto ansi sealed nested private beforefieldinit '<ReadAsync>d__23'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000073:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000086*/ public int32 '<>1__state'
    .field /*04000087*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder`1/*0100002B*/<int32> '<>t__builder'
    .field /*04000088*/ public class Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*/ '<>4__this'
    .field /*04000089*/ public valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<char> buffer
    .field /*0400008A*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*0400008B*/ private int32 '<count>5__2'
    .field /*0400008C*/ private int32 '<charsRead>5__3'
    .field /*0400008D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<int32> '<>u__1'
    .field /*0400008E*/ private int32 '<charsRemaining>5__4'
    .field /*0400008F*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<int32> '<>u__2'
    .method /*06000121*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__23'::MoveNext

    .method /*06000122*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000116:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__23'::SetStateMachine

  } // end of class '<ReadAsync>d__23'

  .class /*02000035*/ auto ansi sealed nested private beforefieldinit '<ReadIntoBufferAsync>d__28'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000074:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000090*/ public int32 '<>1__state'
    .field /*04000091*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<int32> '<>t__builder'
    .field /*04000092*/ public class Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*/ '<>4__this'
    .field /*04000093*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable`1/*01000044*//ConfiguredValueTaskAwaiter/*01000045*/<int32> '<>u__1'
    .method /*06000123*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadIntoBufferAsync>d__28'::MoveNext

    .method /*06000124*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000117:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadIntoBufferAsync>d__28'::SetStateMachine

  } // end of class '<ReadIntoBufferAsync>d__28'

  .class /*02000036*/ auto ansi sealed nested private beforefieldinit '<ReadLineAsync>d__24'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000077:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000094*/ public int32 '<>1__state'
    .field /*04000095*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<string> '<>t__builder'
    .field /*04000096*/ public class Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*/ '<>4__this'
    .field /*04000097*/ private class [System.Runtime/*23000001*/]System.Text.StringBuilder/*01000046*/ '<sb>5__2'
    .field /*04000098*/ private bool '<consumeLineFeed>5__3'
    .field /*04000099*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<int32> '<>u__1'
    .method /*06000125*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLineAsync>d__24'::MoveNext

    .method /*06000126*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000118:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadLineAsync>d__24'::SetStateMachine

  } // end of class '<ReadLineAsync>d__24'

  .class /*02000037*/ auto ansi sealed nested private beforefieldinit '<ReadToEndAsync>d__29'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000078:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400009A*/ public int32 '<>1__state'
    .field /*0400009B*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<string> '<>t__builder'
    .field /*0400009C*/ public class Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader/*0200000F*/ '<>4__this'
    .field /*0400009D*/ private class [System.Runtime/*23000001*/]System.Text.StringBuilder/*01000046*/ '<sb>5__2'
    .field /*0400009E*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1/*01000047*//ConfiguredTaskAwaiter/*01000048*/<int32> '<>u__1'
    .method /*06000127*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadToEndAsync>d__29'::MoveNext

    .method /*06000128*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000119:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadToEndAsync>d__29'::SetStateMachine

  } // end of class '<ReadToEndAsync>d__29'

  .method /*0600007B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding) cil managed
  // SIG: 20 02 01 12 6D 12 80 ED
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::.ctor

  .method /*0600007C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding,
                               int32 bufferSize) cil managed
  // SIG: 20 03 01 12 6D 12 80 ED 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::.ctor

  .method /*0600007D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding,
                               int32 bufferSize,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> bytePool,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<char> charPool) cil managed
  // SIG: 20 05 01 12 6D 12 80 ED 08 15 12 71 01 05 15 12 71 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::.ctor

  .method /*0600007E*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::Dispose

  .method /*0600007F*/ public hidebysig virtual 
          instance int32  Peek() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::Peek

  .method /*06000080*/ public hidebysig virtual 
          instance int32  Read() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::Read

  .method /*06000081*/ public hidebysig virtual 
          instance int32  Read(char[] buffer,
                               int32 index,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 03 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::Read

  .method /*06000082*/ public hidebysig virtual 
          instance int32  Read(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<char> buffer) cil managed
  // SIG: 20 01 08 15 11 80 C1 01 03
  {
    .custom /*0C0000B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::Read

  .method /*06000083*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<int32> 
          ReadAsync(char[] buffer,
                    int32 index,
                    int32 count) cil managed
  // SIG: 20 03 15 12 80 85 01 08 1D 03 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::ReadAsync

  .method /*06000084*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000024*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000025*/<char> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 91 01 08 15 11 80 95 01 03 11 80 81
  {
    .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0000B4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..JMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 48 74 74 70 52 65 71 75 65 73 74   // ties.HttpRequest
                                                                                                                                                                                                                                        53 74 72 65 61 6D 52 65 61 64 65 72 2B 3C 52 65   // StreamReader+<Re
                                                                                                                                                                                                                                        61 64 41 73 79 6E 63 3E 64 5F 5F 32 33 00 00 )    // adAsync>d__23..
    .param [2]/*080000A6*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::ReadAsync

  .method /*06000085*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<string> 
          ReadLineAsync() cil managed
  // SIG: 20 00 15 12 80 85 01 0E
  {
    .custom /*0C0000B6:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 48 74 74 70 52 65 71 75 65 73 74   // ties.HttpRequest
                                                                                                                                                                                                                                        53 74 72 65 61 6D 52 65 61 64 65 72 2B 3C 52 65   // StreamReader+<Re
                                                                                                                                                                                                                                        61 64 4C 69 6E 65 41 73 79 6E 63 3E 64 5F 5F 32   // adLineAsync>d__2
                                                                                                                                                                                                                                        34 00 00 )                                        // 4..
    .param [0]/*080000A7*/ 
    .custom /*0C0000C8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::ReadLineAsync

  .method /*06000086*/ public hidebysig virtual 
          instance string  ReadLine() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000B8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::ReadLine

  .method /*06000087*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<string> 
          ReadToEndAsync() cil managed
  // SIG: 20 00 15 12 80 85 01 0E
  {
    .custom /*0C0000B9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..OMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 48 74 74 70 52 65 71 75 65 73 74   // ties.HttpRequest
                                                                                                                                                                                                                                        53 74 72 65 61 6D 52 65 61 64 65 72 2B 3C 52 65   // StreamReader+<Re
                                                                                                                                                                                                                                        61 64 54 6F 45 6E 64 41 73 79 6E 63 3E 64 5F 5F   // adToEndAsync>d__
                                                                                                                                                                                                                                        32 39 00 00 )                                     // 29..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestStreamReader::ReadToEndAsync

} // end of class Microsoft.AspNetCore.WebUtilities.HttpRequestStreamReader

.class /*02000010*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter
       extends [System.Runtime/*23000001*/]System.IO.TextWriter/*01000049*/
{
  .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000038*/ auto ansi sealed nested private beforefieldinit '<DisposeAsync>d__34'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000079:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*0400009F*/ public int32 '<>1__state'
    .field /*040000A0*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*040000A1*/ public class Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter/*02000010*/ '<>4__this'
    .field /*040000A2*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .field /*040000A3*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__2'
    .method /*06000129*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__34'::MoveNext

    .method /*0600012A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00011A:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DisposeAsync>d__34'::SetStateMachine

  } // end of class '<DisposeAsync>d__34'

  .class /*02000039*/ auto ansi sealed nested private beforefieldinit '<FlushInternalAsync>d__36'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00007B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000A4*/ public int32 '<>1__state'
    .field /*040000A5*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000A6*/ public class Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter/*02000010*/ '<>4__this'
    .field /*040000A7*/ public bool flushEncoder
    .field /*040000A8*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__1'
    .method /*0600012B*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FlushInternalAsync>d__36'::MoveNext

    .method /*0600012C*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00011B:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<FlushInternalAsync>d__36'::SetStateMachine

  } // end of class '<FlushInternalAsync>d__36'

  .class /*0200003A*/ auto ansi sealed nested private beforefieldinit '<WriteAsyncAwaited>d__22'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00007D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000A9*/ public int32 '<>1__state'
    .field /*040000AA*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000AB*/ public class Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter/*02000010*/ '<>4__this'
    .field /*040000AC*/ public char 'value'
    .field /*040000AD*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .method /*0600012D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsyncAwaited>d__22'::MoveNext

    .method /*0600012E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00011C:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsyncAwaited>d__22'::SetStateMachine

  } // end of class '<WriteAsyncAwaited>d__22'

  .class /*0200003B*/ auto ansi sealed nested private beforefieldinit '<WriteAsyncAwaited>d__24'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00007E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000AE*/ public int32 '<>1__state'
    .field /*040000AF*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000B0*/ public class Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter/*02000010*/ '<>4__this'
    .field /*040000B1*/ public char[] values
    .field /*040000B2*/ public int32 index
    .field /*040000B3*/ public int32 count
    .field /*040000B4*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .method /*0600012F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsyncAwaited>d__24'::MoveNext

    .method /*06000130*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00011E:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsyncAwaited>d__24'::SetStateMachine

  } // end of class '<WriteAsyncAwaited>d__24'

  .class /*0200003C*/ auto ansi sealed nested private beforefieldinit '<WriteAsyncAwaited>d__26'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00007F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000B5*/ public int32 '<>1__state'
    .field /*040000B6*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000B7*/ public string 'value'
    .field /*040000B8*/ public class Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter/*02000010*/ '<>4__this'
    .field /*040000B9*/ private int32 '<count>5__2'
    .field /*040000BA*/ private int32 '<index>5__3'
    .field /*040000BB*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .method /*06000131*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsyncAwaited>d__26'::MoveNext

    .method /*06000132*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C00011F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsyncAwaited>d__26'::SetStateMachine

  } // end of class '<WriteAsyncAwaited>d__26'

  .class /*0200003D*/ auto ansi sealed nested private beforefieldinit '<WriteAsyncAwaited>d__28'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000081:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000BC*/ public int32 '<>1__state'
    .field /*040000BD*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000BE*/ public valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> 'value'
    .field /*040000BF*/ public class Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter/*02000010*/ '<>4__this'
    .field /*040000C0*/ private int32 '<remaining>5__2'
    .field /*040000C1*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .method /*06000133*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsyncAwaited>d__28'::MoveNext

    .method /*06000134*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000120:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteAsyncAwaited>d__28'::SetStateMachine

  } // end of class '<WriteAsyncAwaited>d__28'

  .class /*0200003E*/ auto ansi sealed nested private beforefieldinit '<WriteLineAsyncAwaited>d__30'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000082:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000C2*/ public int32 '<>1__state'
    .field /*040000C3*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000C4*/ public class Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter/*02000010*/ '<>4__this'
    .field /*040000C5*/ public valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> 'value'
    .field /*040000C6*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .method /*06000135*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteLineAsyncAwaited>d__30'::MoveNext

    .method /*06000136*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000121:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteLineAsyncAwaited>d__30'::SetStateMachine

  } // end of class '<WriteLineAsyncAwaited>d__30'

  .field /*04000009*/ static assembly literal int32 DefaultBufferSize = int32(0x00004000)
  .method /*06000088*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding) cil managed
  // SIG: 20 02 01 12 6D 12 80 ED
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::.ctor

  .method /*06000089*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding,
                               int32 bufferSize) cil managed
  // SIG: 20 03 01 12 6D 12 80 ED 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::.ctor

  .method /*0600008A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ encoding,
                               int32 bufferSize,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> bytePool,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<char> charPool) cil managed
  // SIG: 20 05 01 12 6D 12 80 ED 08 15 12 71 01 05 15 12 71 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::.ctor

  .method /*0600008B*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ 
          get_Encoding() cil managed
  // SIG: 20 00 12 80 ED
  {
    .custom /*0C0000BA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::get_Encoding

  .method /*0600008C*/ public hidebysig virtual 
          instance void  Write(char 'value') cil managed
  // SIG: 20 01 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::Write

  .method /*0600008D*/ public hidebysig virtual 
          instance void  Write(char[] values,
                               int32 index,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 03 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::Write

  .method /*0600008E*/ public hidebysig virtual 
          instance void  Write(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100004A*/<char> 'value') cil managed
  // SIG: 20 01 01 15 11 81 29 01 03
  {
    .custom /*0C0000BB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::Write

  .method /*0600008F*/ public hidebysig virtual 
          instance void  Write(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000BC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::Write

  .method /*06000090*/ public hidebysig virtual 
          instance void  WriteLine(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100004A*/<char> 'value') cil managed
  // SIG: 20 01 01 15 11 81 29 01 03
  {
    .custom /*0C0000BE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::WriteLine

  .method /*06000091*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteAsync(char 'value') cil managed
  // SIG: 20 01 12 7D 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::WriteAsync

  .method /*06000092*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteAsync(char[] values,
                     int32 index,
                     int32 count) cil managed
  // SIG: 20 03 12 7D 1D 03 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::WriteAsync

  .method /*06000093*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteAsync(string 'value') cil managed
  // SIG: 20 01 12 7D 0E
  {
    .param [1]/*080000BD*/ 
    .custom /*0C0000D2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::WriteAsync

  .method /*06000094*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> 'value',
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 12 7D 15 11 80 D9 01 03 11 80 81
  {
    .custom /*0C0000C0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000BE*/ 
    .custom /*0C0000D3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C0*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::WriteAsync

  .method /*06000095*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteLineAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> 'value',
                         [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 12 7D 15 11 80 D9 01 03 11 80 81
  {
    .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080000C1*/ 
    .custom /*0C0000D5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000C3*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::WriteLineAsync

  .method /*06000096*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::Flush

  .method /*06000097*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          FlushAsync() cil managed
  // SIG: 20 00 12 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::FlushAsync

  .method /*06000098*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::Dispose

  .method /*06000099*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000031*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 C5
  {
    .custom /*0C0000C2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 48 74 74 70 52 65 73 70 6F 6E 73   // ties.HttpRespons
                                                                                                                                                                                                                                        65 53 74 72 65 61 6D 57 72 69 74 65 72 2B 3C 44   // eStreamWriter+<D
                                                                                                                                                                                                                                        69 73 70 6F 73 65 41 73 79 6E 63 3E 64 5F 5F 33   // isposeAsync>d__3
                                                                                                                                                                                                                                        34 00 00 )                                        // 4..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpResponseStreamWriter::DisposeAsync

  .property /*17000025*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/
          Encoding()
  {
    .get instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100003B*/ Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter/*02000010*/::get_Encoding() /* 0600008B */
  } // end of property HttpResponseStreamWriter::Encoding
} // end of class Microsoft.AspNetCore.WebUtilities.HttpResponseStreamWriter

.class /*02000011*/ public sequential ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator
       extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
{
  .custom /*0C00003D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*0400000A*/ private class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> _accumulator
  .field /*0400000B*/ private class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,class [System.Collections/*23000004*/]System.Collections.Generic.List`1/*0100004B*/<string>> _expandingAccumulator
  .field /*0400000C*/ private int32 '<ValueCount>k__BackingField'
  .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .method /*0600009A*/ public hidebysig instance void 
          Append(string key,
                 string 'value') cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyValueAccumulator::Append

  .method /*0600009B*/ public hidebysig specialname 
          instance bool  get_HasValues() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyValueAccumulator::get_HasValues

  .method /*0600009C*/ public hidebysig specialname 
          instance int32  get_KeyCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyValueAccumulator::get_KeyCount

  .method /*0600009D*/ public hidebysig specialname 
          instance int32  get_ValueCount() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000C3:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000043*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    .custom /*0C0000C4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyValueAccumulator::get_ValueCount

  .method /*0600009E*/ public hidebysig instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> 
          GetResults() cil managed
  // SIG: 20 00 15 12 80 F1 02 0E 11 80 F5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method KeyValueAccumulator::GetResults

  .property /*17000026*/ instance callconv(8) bool
          HasValues()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator/*02000011*/::get_HasValues() /* 0600009B */
  } // end of property KeyValueAccumulator::HasValues
  .property /*17000027*/ instance callconv(8) int32
          KeyCount()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator/*02000011*/::get_KeyCount() /* 0600009C */
  } // end of property KeyValueAccumulator::KeyCount
  .property /*17000028*/ instance callconv(8) int32
          ValueCount()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator/*02000011*/::get_ValueCount() /* 0600009D */
  } // end of property KeyValueAccumulator::ValueCount
} // end of class Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator

.class /*02000012*/ private auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.MultipartBoundary
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600009F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string boundary,
                               [opt] bool expectLeadingCrlf) cil managed
  // SIG: 20 02 01 0E 02
  {
    .param [2]/*080000C8*/  = bool(true)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartBoundary::.ctor

  .method /*060000A0*/ public hidebysig instance int32 
          GetSkipValue(uint8 input) cil managed
  // SIG: 20 01 08 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartBoundary::GetSkipValue

  .method /*060000A1*/ public hidebysig specialname 
          instance bool  get_ExpectLeadingCrlf() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartBoundary::get_ExpectLeadingCrlf

  .method /*060000A2*/ public hidebysig specialname 
          instance void  set_ExpectLeadingCrlf(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartBoundary::set_ExpectLeadingCrlf

  .method /*060000A3*/ public hidebysig specialname 
          instance uint8[]  get_BoundaryBytes() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C0000C5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartBoundary::get_BoundaryBytes

  .method /*060000A4*/ public hidebysig specialname 
          instance int32  get_FinalBoundaryLength() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000C6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartBoundary::get_FinalBoundaryLength

  .property /*17000029*/ instance callconv(8) bool
          ExpectLeadingCrlf()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.MultipartBoundary/*02000012*/::get_ExpectLeadingCrlf() /* 060000A1 */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartBoundary/*02000012*/::set_ExpectLeadingCrlf(bool) /* 060000A2 */
  } // end of property MultipartBoundary::ExpectLeadingCrlf
  .property /*1700002A*/ instance callconv(8) uint8[]
          BoundaryBytes()
  {
    .get instance uint8[] Microsoft.AspNetCore.WebUtilities.MultipartBoundary/*02000012*/::get_BoundaryBytes() /* 060000A3 */
  } // end of property MultipartBoundary::BoundaryBytes
  .property /*1700002B*/ instance callconv(8) int32
          FinalBoundaryLength()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.MultipartBoundary/*02000012*/::get_FinalBoundaryLength() /* 060000A4 */
  } // end of property MultipartBoundary::FinalBoundaryLength
} // end of class Microsoft.AspNetCore.WebUtilities.MultipartBoundary

.class /*02000013*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.MultipartReader
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000041:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003F*/ auto ansi sealed nested private beforefieldinit '<ReadHeadersAsync>d__21'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000083:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000C7*/ public int32 '<>1__state'
    .field /*040000C8*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>> '<>t__builder'
    .field /*040000C9*/ public class Microsoft.AspNetCore.WebUtilities.MultipartReader/*02000013*/ '<>4__this'
    .field /*040000CA*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*040000CB*/ private int32 '<totalSize>5__2'
    .field /*040000CC*/ private valuetype Microsoft.AspNetCore.WebUtilities.KeyValueAccumulator/*02000011*/ '<accumulator>5__3'
    .field /*040000CD*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<string> '<>u__1'
    .method /*06000137*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadHeadersAsync>d__21'::MoveNext

    .method /*06000138*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000122:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadHeadersAsync>d__21'::SetStateMachine

  } // end of class '<ReadHeadersAsync>d__21'

  .class /*02000040*/ auto ansi sealed nested private beforefieldinit '<ReadNextSectionAsync>d__20'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000084:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000CE*/ public int32 '<>1__state'
    .field /*040000CF*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/> '<>t__builder'
    .field /*040000D0*/ public class Microsoft.AspNetCore.WebUtilities.MultipartReader/*02000013*/ '<>4__this'
    .field /*040000D1*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*040000D2*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*01000038*/ '<>u__1'
    .field /*040000D3*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>> '<>u__2'
    .method /*06000139*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadNextSectionAsync>d__20'::MoveNext

    .method /*0600013A*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000123:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadNextSectionAsync>d__20'::SetStateMachine

  } // end of class '<ReadNextSectionAsync>d__20'

  .field /*0400000D*/ public static literal int32 DefaultHeadersCountLimit = int32(0x00000010)
  .field /*0400000E*/ public static literal int32 DefaultHeadersLengthLimit = int32(0x00004000)
  .method /*060000A5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string boundary,
                               class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream') cil managed
  // SIG: 20 02 01 0E 12 6D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::.ctor

  .method /*060000A6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string boundary,
                               class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                               int32 bufferSize) cil managed
  // SIG: 20 03 01 0E 12 6D 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::.ctor

  .method /*060000A7*/ public hidebysig specialname 
          instance int32  get_HeadersCountLimit() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000C7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::get_HeadersCountLimit

  .method /*060000A8*/ public hidebysig specialname 
          instance void  set_HeadersCountLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000C9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::set_HeadersCountLimit

  .method /*060000A9*/ public hidebysig specialname 
          instance int32  get_HeadersLengthLimit() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000CA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::get_HeadersLengthLimit

  .method /*060000AA*/ public hidebysig specialname 
          instance void  set_HeadersLengthLimit(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C0000CB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::set_HeadersLengthLimit

  .method /*060000AB*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> 
          get_BodyLengthLimit() cil managed
  // SIG: 20 00 15 11 80 B9 01 0A
  {
    .custom /*0C0000CC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::get_BodyLengthLimit

  .method /*060000AC*/ public hidebysig specialname 
          instance void  set_BodyLengthLimit(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 80 B9 01 0A
  {
    .custom /*0C0000CD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::set_BodyLengthLimit

  .method /*060000AD*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/> 
          ReadNextSectionAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 01 15 12 80 85 01 12 54 11 80 81
  {
    .custom /*0C0000CE:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 4D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..MMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 4D 75 6C 74 69 70 61 72 74 52 65   // ties.MultipartRe
                                                                                                                                                                                                                                        61 64 65 72 2B 3C 52 65 61 64 4E 65 78 74 53 65   // ader+<ReadNextSe
                                                                                                                                                                                                                                        63 74 69 6F 6E 41 73 79 6E 63 3E 64 5F 5F 32 30   // ctionAsync>d__20
                                                                                                                                                                                                                                        00 00 ) 
    .param [0]/*080000D3*/ 
    .custom /*0C0000E8:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 01 02 00 00 ) 
    .param [1]/*080000D4*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReader::ReadNextSectionAsync

  .property /*1700002C*/ instance callconv(8) int32
          HeadersCountLimit()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.MultipartReader/*02000013*/::get_HeadersCountLimit() /* 060000A7 */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartReader/*02000013*/::set_HeadersCountLimit(int32) /* 060000A8 */
  } // end of property MultipartReader::HeadersCountLimit
  .property /*1700002D*/ instance callconv(8) int32
          HeadersLengthLimit()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.MultipartReader/*02000013*/::get_HeadersLengthLimit() /* 060000A9 */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartReader/*02000013*/::set_HeadersLengthLimit(int32) /* 060000AA */
  } // end of property MultipartReader::HeadersLengthLimit
  .property /*1700002E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64>
          BodyLengthLimit()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> Microsoft.AspNetCore.WebUtilities.MultipartReader/*02000013*/::get_BodyLengthLimit() /* 060000AB */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartReader/*02000013*/::set_BodyLengthLimit(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64>) /* 060000AC */
  } // end of property MultipartReader::BodyLengthLimit
} // end of class Microsoft.AspNetCore.WebUtilities.MultipartReader

.class /*02000014*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.MultipartReaderStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/
{
  .custom /*0C000043:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000041*/ auto ansi sealed nested private beforefieldinit '<ReadAsync>d__36'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000085:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000D4*/ public int32 '<>1__state'
    .field /*040000D5*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<int32> '<>t__builder'
    .field /*040000D6*/ public class Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/ '<>4__this'
    .field /*040000D7*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*040000D8*/ public uint8[] buffer
    .field /*040000D9*/ public int32 offset
    .field /*040000DA*/ public int32 count
    .field /*040000DB*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<bool> '<>u__1'
    .field /*040000DC*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<string> '<>u__2'
    .method /*0600013B*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__36'::MoveNext

    .method /*0600013C*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000124:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsync>d__36'::SetStateMachine

  } // end of class '<ReadAsync>d__36'

  .method /*060000AE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/ 'stream',
                               class Microsoft.AspNetCore.WebUtilities.MultipartBoundary/*02000012*/ boundary) cil managed
  // SIG: 20 02 01 12 20 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::.ctor

  .method /*060000AF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.WebUtilities.BufferedReadStream/*02000008*/ 'stream',
                               class Microsoft.AspNetCore.WebUtilities.MultipartBoundary/*02000012*/ boundary,
                               class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> bytePool) cil managed
  // SIG: 20 03 01 12 20 12 48 15 12 71 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::.ctor

  .method /*060000B0*/ public hidebysig specialname 
          instance bool  get_FinalBoundaryFound() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000CF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::get_FinalBoundaryFound

  .method /*060000B1*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> 
          get_LengthLimit() cil managed
  // SIG: 20 00 15 11 80 B9 01 0A
  {
    .custom /*0C0000D0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::get_LengthLimit

  .method /*060000B2*/ public hidebysig specialname 
          instance void  set_LengthLimit(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 80 B9 01 0A
  {
    .custom /*0C0000D1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::set_LengthLimit

  .method /*060000B3*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::get_CanRead

  .method /*060000B4*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::get_CanSeek

  .method /*060000B5*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::get_CanWrite

  .method /*060000B6*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::get_Length

  .method /*060000B7*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::get_Position

  .method /*060000B8*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::set_Position

  .method /*060000B9*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*0100001E*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::Seek

  .method /*060000BA*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::SetLength

  .method /*060000BB*/ public hidebysig virtual 
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
  } // end of method MultipartReaderStream::Write

  .method /*060000BC*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 04 12 7D 1D 05 08 08 11 80 81
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::WriteAsync

  .method /*060000BD*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::Flush

  .method /*060000BE*/ public hidebysig virtual 
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
  } // end of method MultipartReaderStream::Read

  .method /*060000BF*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 85 01 08 1D 05 08 08 11 80 81
  {
    .custom /*0C0000D4:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 48 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..HMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 4D 75 6C 74 69 70 61 72 74 52 65   // ties.MultipartRe
                                                                                                                                                                                                                                        61 64 65 72 53 74 72 65 61 6D 2B 3C 52 65 61 64   // aderStream+<Read
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 33 36 00 00 )          // Async>d__36..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartReaderStream::ReadAsync

  .property /*1700002F*/ instance callconv(8) bool
          FinalBoundaryFound()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::get_FinalBoundaryFound() /* 060000B0 */
  } // end of property MultipartReaderStream::FinalBoundaryFound
  .property /*17000030*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64>
          LengthLimit()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::get_LengthLimit() /* 060000B1 */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::set_LengthLimit(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64>) /* 060000B2 */
  } // end of property MultipartReaderStream::LengthLimit
  .property /*17000031*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::get_CanRead() /* 060000B3 */
  } // end of property MultipartReaderStream::CanRead
  .property /*17000032*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::get_CanSeek() /* 060000B4 */
  } // end of property MultipartReaderStream::CanSeek
  .property /*17000033*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::get_CanWrite() /* 060000B5 */
  } // end of property MultipartReaderStream::CanWrite
  .property /*17000034*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::get_Length() /* 060000B6 */
  } // end of property MultipartReaderStream::Length
  .property /*17000035*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::get_Position() /* 060000B7 */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartReaderStream/*02000014*/::set_Position(int64) /* 060000B8 */
  } // end of property MultipartReaderStream::Position
} // end of class Microsoft.AspNetCore.WebUtilities.MultipartReaderStream

.class /*02000015*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebUtilities.MultipartSection
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000045:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000C0*/ public hidebysig specialname 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::get_ContentType

  .method /*060000C1*/ public hidebysig specialname 
          instance string  get_ContentDisposition() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::get_ContentDisposition

  .method /*060000C2*/ public hidebysig specialname 
          instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> 
          get_Headers() cil managed
  // SIG: 20 00 15 12 80 F1 02 0E 11 80 F5
  {
    .custom /*0C0000D6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .param [0]/*080000ED*/ 
    .custom /*0C0000F4:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::get_Headers

  .method /*060000C3*/ public hidebysig specialname 
          instance void  set_Headers(class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> 'value') cil managed
  // SIG: 20 01 01 15 12 80 F1 02 0E 11 80 F5
  {
    .custom /*0C0000D7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .param [1]/*080000EE*/ 
    .custom /*0C0000F6:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::set_Headers

  .method /*060000C4*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 
          get_Body() cil managed
  // SIG: 20 00 12 6D
  {
    .custom /*0C0000D8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000D9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::get_Body

  .method /*060000C5*/ public hidebysig specialname 
          instance void  set_Body(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6D
  {
    .custom /*0C0000DA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000DB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::set_Body

  .method /*060000C6*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> 
          get_BaseStreamOffset() cil managed
  // SIG: 20 00 15 11 80 B9 01 0A
  {
    .custom /*0C0000DC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::get_BaseStreamOffset

  .method /*060000C7*/ public hidebysig specialname 
          instance void  set_BaseStreamOffset(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> 'value') cil managed
  // SIG: 20 01 01 15 11 80 B9 01 0A
  {
    .custom /*0C0000DD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::set_BaseStreamOffset

  .method /*060000C8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSection::.ctor

  .property /*17000036*/ instance callconv(8) string
          ContentType()
  {
    .get instance string Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/::get_ContentType() /* 060000C0 */
  } // end of property MultipartSection::ContentType
  .property /*17000037*/ instance callconv(8) string
          ContentDisposition()
  {
    .get instance string Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/::get_ContentDisposition() /* 060000C1 */
  } // end of property MultipartSection::ContentDisposition
  .property /*17000038*/ instance callconv(8) class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>
          Headers()
  {
    .custom /*0C00007A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/::get_Headers() /* 060000C2 */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/::set_Headers(class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>) /* 060000C3 */
  } // end of property MultipartSection::Headers
  .property /*17000039*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/
          Body()
  {
    .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/::get_Body() /* 060000C4 */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/::set_Body(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 060000C5 */
  } // end of property MultipartSection::Body
  .property /*1700003A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64>
          BaseStreamOffset()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/::get_BaseStreamOffset() /* 060000C6 */
    .set instance void Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/::set_BaseStreamOffset(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64>) /* 060000C7 */
  } // end of property MultipartSection::BaseStreamOffset
} // end of class Microsoft.AspNetCore.WebUtilities.MultipartSection

.class /*02000016*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.MultipartSectionConverterExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000047:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000049:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*060000C9*/ public hidebysig static 
          class Microsoft.AspNetCore.WebUtilities.FileMultipartSection/*0200000B*/ 
          AsFileSection(class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section) cil managed
  // SIG: 00 01 12 2C 12 54
  {
    .custom /*0C0000DE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*080000F1*/ 
    .custom /*0C0000F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSectionConverterExtensions::AsFileSection

  .method /*060000CA*/ public hidebysig static 
          class Microsoft.AspNetCore.WebUtilities.FormMultipartSection/*0200000C*/ 
          AsFormDataSection(class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section) cil managed
  // SIG: 00 01 12 30 12 54
  {
    .custom /*0C0000DF:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*080000F3*/ 
    .custom /*0C0000F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSectionConverterExtensions::AsFormDataSection

  .method /*060000CB*/ public hidebysig static 
          class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.ContentDispositionHeaderValue/*01000039*/ 
          GetContentDispositionHeader(class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section) cil managed
  // SIG: 00 01 12 80 E5 12 54
  {
    .custom /*0C0000E0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*080000F5*/ 
    .custom /*0C0000FA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSectionConverterExtensions::GetContentDispositionHeader

} // end of class Microsoft.AspNetCore.WebUtilities.MultipartSectionConverterExtensions

.class /*02000017*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.MultipartSectionStreamExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00004A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000042*/ auto ansi sealed nested private beforefieldinit '<ReadAsStringAsync>d__0'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000086:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000DD*/ public int32 '<>1__state'
    .field /*040000DE*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000028*/<string> '<>t__builder'
    .field /*040000DF*/ public class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section
    .field /*040000E0*/ private class [System.Runtime/*23000001*/]System.IO.StreamReader/*0100004C*/ '<reader>5__2'
    .field /*040000E1*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*0100002D*/<string> '<>u__1'
    .method /*0600013D*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsStringAsync>d__0'::MoveNext

    .method /*0600013E*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000125:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ReadAsStringAsync>d__0'::SetStateMachine

  } // end of class '<ReadAsStringAsync>d__0'

  .method /*060000CC*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000021*/<string> 
          ReadAsStringAsync(class Microsoft.AspNetCore.WebUtilities.MultipartSection/*02000015*/ section) cil managed
  // SIG: 00 01 15 12 80 85 01 0E 12 54
  {
    .custom /*0C0000E1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000E2:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 5A 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..ZMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 4D 75 6C 74 69 70 61 72 74 53 65   // ties.MultipartSe
                                                                                                                                                                                                                                        63 74 69 6F 6E 53 74 72 65 61 6D 45 78 74 65 6E   // ctionStreamExten
                                                                                                                                                                                                                                        73 69 6F 6E 73 2B 3C 52 65 61 64 41 73 53 74 72   // sions+<ReadAsStr
                                                                                                                                                                                                                                        69 6E 67 41 73 79 6E 63 3E 64 5F 5F 30 00 00 )    // ingAsync>d__0..
    .custom /*0C0000E3:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MultipartSectionStreamExtensions::ReadAsStringAsync

} // end of class Microsoft.AspNetCore.WebUtilities.MultipartSectionStreamExtensions

.class /*02000018*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.PagedByteBuffer
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000040*/
{
  .custom /*0C00004C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000040*/
  .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000043*/ auto ansi sealed nested private beforefieldinit '<MoveToAsync>d__21'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000087:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000E2*/ public int32 '<>1__state'
    .field /*040000E3*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000E4*/ public class Microsoft.AspNetCore.WebUtilities.PagedByteBuffer/*02000018*/ '<>4__this'
    .field /*040000E5*/ public class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000037*/ writer
    .field /*040000E6*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*040000E7*/ private int32 '<i>5__2'
    .field /*040000E8*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<valuetype [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.FlushResult/*0100004D*/> '<>u__1'
    .method /*0600013F*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<MoveToAsync>d__21'::MoveNext

    .method /*06000140*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000126:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<MoveToAsync>d__21'::SetStateMachine

  } // end of class '<MoveToAsync>d__21'

  .class /*02000044*/ auto ansi sealed nested private beforefieldinit '<MoveToAsync>d__22'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C000088:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000E9*/ public int32 '<>1__state'
    .field /*040000EA*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000EB*/ public class Microsoft.AspNetCore.WebUtilities.PagedByteBuffer/*02000018*/ '<>4__this'
    .field /*040000EC*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream'
    .field /*040000ED*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*040000EE*/ private int32 '<i>5__2'
    .field /*040000EF*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000033*/ '<>u__1'
    .method /*06000141*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<MoveToAsync>d__22'::MoveNext

    .method /*06000142*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000127:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<MoveToAsync>d__22'::SetStateMachine

  } // end of class '<MoveToAsync>d__22'

  .field /*0400000F*/ static assembly literal int32 PageSize = int32(0x00000400)
  .method /*060000CD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> arrayPool) cil managed
  // SIG: 20 01 01 15 12 71 01 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::.ctor

  .method /*060000CE*/ public hidebysig specialname 
          instance int32  get_Length() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C0000E4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::get_Length

  .method /*060000CF*/ assembly hidebysig specialname 
          instance bool  get_Disposed() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000E5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::get_Disposed

  .method /*060000D0*/ assembly hidebysig specialname 
          instance class [System.Collections/*23000004*/]System.Collections.Generic.List`1/*0100004B*/<uint8[]> 
          get_Pages() cil managed
  // SIG: 20 00 15 12 81 2D 01 1D 05
  {
    .custom /*0C0000E6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::get_Pages

  .method /*060000D1*/ public hidebysig instance void 
          Add(uint8[] buffer,
              int32 offset,
              int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::Add

  .method /*060000D2*/ public hidebysig instance void 
          Add(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<uint8> memory) cil managed
  // SIG: 20 01 01 15 11 80 D9 01 05
  {
    .custom /*0C0000E7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::Add

  .method /*060000D3*/ public hidebysig instance void 
          MoveTo(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream') cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::MoveTo

  .method /*060000D4*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          MoveToAsync(class [System.IO.Pipelines/*23000002*/]System.IO.Pipelines.PipeWriter/*01000037*/ writer,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 12 7D 12 80 DD 11 80 81
  {
    .custom /*0C0000E9:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 44 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..DMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 50 61 67 65 64 42 79 74 65 42 75   // ties.PagedByteBu
                                                                                                                                                                                                                                        66 66 65 72 2B 3C 4D 6F 76 65 54 6F 41 73 79 6E   // ffer+<MoveToAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 32 31 00 00 )                      // c>d__21..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::MoveToAsync

  .method /*060000D5*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          MoveToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 20 02 12 7D 12 6D 11 80 81
  {
    .custom /*0C0000EA:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 44 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..DMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 50 61 67 65 64 42 79 74 65 42 75   // ties.PagedByteBu
                                                                                                                                                                                                                                        66 66 65 72 2B 3C 4D 6F 76 65 54 6F 41 73 79 6E   // ffer+<MoveToAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 32 32 00 00 )                      // c>d__22..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::MoveToAsync

  .method /*060000D6*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PagedByteBuffer::Dispose

  .property /*1700003B*/ instance callconv(8) int32
          Length()
  {
    .get instance int32 Microsoft.AspNetCore.WebUtilities.PagedByteBuffer/*02000018*/::get_Length() /* 060000CE */
  } // end of property PagedByteBuffer::Length
  .property /*1700003C*/ instance callconv(8) bool
          Disposed()
  {
    .get instance bool Microsoft.AspNetCore.WebUtilities.PagedByteBuffer/*02000018*/::get_Disposed() /* 060000CF */
  } // end of property PagedByteBuffer::Disposed
  .property /*1700003D*/ instance callconv(8) class [System.Collections/*23000004*/]System.Collections.Generic.List`1/*0100004B*/<uint8[]>
          Pages()
  {
    .get instance class [System.Collections/*23000004*/]System.Collections.Generic.List`1/*0100004B*/<uint8[]> Microsoft.AspNetCore.WebUtilities.PagedByteBuffer/*02000018*/::get_Pages() /* 060000D0 */
  } // end of property PagedByteBuffer::Pages
} // end of class Microsoft.AspNetCore.WebUtilities.PagedByteBuffer

.class /*02000019*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.QueryHelpers
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00004E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000045*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C000089:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000F0*/ public static initonly class Microsoft.AspNetCore.WebUtilities.QueryHelpers/*02000019*//'<>c'/*02000045*/ '<>9'
    .field /*040000F1*/ public static class [System.Runtime/*23000001*/]System.Func`2/*01000050*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>,class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004F*/<string>> '<>9__2_0'
    .field /*040000F2*/ public static class [System.Runtime/*23000001*/]System.Func`3/*01000051*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>,string,valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,string>> '<>9__2_1'
    .method /*06000143*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000144*/ assembly hidebysig 
            instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004F*/<string> 
            '<AddQueryString>b__2_0'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> kvp) cil managed
    // SIG: 20 01 15 12 81 3D 01 0E 15 11 81 05 02 0E 11 80 F5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AddQueryString>b__2_0'

    .method /*06000145*/ assembly hidebysig 
            instance valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,string> 
            '<AddQueryString>b__2_1'(valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> kvp,
                                     string v) cil managed
    // SIG: 20 02 15 11 81 05 02 0E 0E 15 11 81 05 02 0E 11 80 F5 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AddQueryString>b__2_1'

  } // end of class '<>c'

  .method /*060000D7*/ public hidebysig static 
          string  AddQueryString(string uri,
                                 string name,
                                 string 'value') cil managed
  // SIG: 00 03 0E 0E 0E 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryHelpers::AddQueryString

  .method /*060000D8*/ public hidebysig static 
          string  AddQueryString(string uri,
                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100004E*/<string,string> queryString) cil managed
  // SIG: 00 02 0E 0E 15 12 81 39 02 0E 0E
  {
    .param [2]/*08000106*/ 
    .custom /*0C000103:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryHelpers::AddQueryString

  .method /*060000D9*/ public hidebysig static 
          string  AddQueryString(string uri,
                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/>> queryString) cil managed
  // SIG: 00 02 0E 0E 15 12 81 3D 01 15 11 81 05 02 0E 11 80 F5
  {
    .param [2]/*08000108*/ 
    .custom /*0C000105:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryHelpers::AddQueryString

  .method /*060000DA*/ public hidebysig static 
          string  AddQueryString(string uri,
                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100004F*/<valuetype [System.Runtime/*23000001*/]System.Collections.Generic.KeyValuePair`2/*01000041*/<string,string>> queryString) cil managed
  // SIG: 00 02 0E 0E 15 12 81 3D 01 15 11 81 05 02 0E 0E
  {
    .param [2]/*0800010A*/ 
    .custom /*0C000107:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryHelpers::AddQueryString

  .method /*060000DB*/ public hidebysig static 
          class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> 
          ParseQuery(string queryString) cil managed
  // SIG: 00 01 15 12 80 F1 02 0E 11 80 F5 0E
  {
    .param [1]/*0800010B*/ 
    .custom /*0C000109:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryHelpers::ParseQuery

  .method /*060000DC*/ public hidebysig static 
          class [System.Collections/*23000004*/]System.Collections.Generic.Dictionary`2/*0100003C*/<string,valuetype [Microsoft.Extensions.Primitives/*23000005*/]Microsoft.Extensions.Primitives.StringValues/*0100003D*/> 
          ParseNullableQuery(string queryString) cil managed
  // SIG: 00 01 15 12 80 F1 02 0E 11 80 F5 0E
  {
    .custom /*0C0000EB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800010C*/ 
    .custom /*0C00010A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryHelpers::ParseNullableQuery

} // end of class Microsoft.AspNetCore.WebUtilities.QueryHelpers

.class /*0200001A*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.ReasonPhrases
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*060000DD*/ public hidebysig static 
          string  GetReasonPhrase(int32 statusCode) cil managed
  // SIG: 00 01 0E 08
  {
    .custom /*0C0000EC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ReasonPhrases::GetReasonPhrase

} // end of class Microsoft.AspNetCore.WebUtilities.ReasonPhrases

.class /*0200001B*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.StreamHelperExtensions
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000051:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000052:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000053:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*02000046*/ auto ansi sealed nested private beforefieldinit '<DrainAsync>d__3'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/
  {
    .custom /*0C00008B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000F3*/ public int32 '<>1__state'
    .field /*040000F4*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000032*/ '<>t__builder'
    .field /*040000F5*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken
    .field /*040000F6*/ public class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> bytePool
    .field /*040000F7*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream'
    .field /*040000F8*/ public valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> limit
    .field /*040000F9*/ private uint8[] '<buffer>5__2'
    .field /*040000FA*/ private int64 '<total>5__3'
    .field /*040000FB*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*01000029*/<int32> '<>u__1'
    .method /*06000146*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::MoveNext /*01000027::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DrainAsync>d__3'::MoveNext

    .method /*06000147*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 9D
    {
      .custom /*0C000128:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*0100002A*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000027*/::SetStateMachine /*01000027::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<DrainAsync>d__3'::SetStateMachine

  } // end of class '<DrainAsync>d__3'

  .method /*060000DE*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          DrainAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 00 02 12 7D 12 6D 11 80 81
  {
    .custom /*0C0000ED:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamHelperExtensions::DrainAsync

  .method /*060000DF*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          DrainAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                     valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> limit,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 00 03 12 7D 12 6D 15 11 80 B9 01 0A 11 80 81
  {
    .custom /*0C0000EE:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamHelperExtensions::DrainAsync

  .method /*060000E0*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001F*/ 
          DrainAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 'stream',
                     class [System.Runtime/*23000001*/]System.Buffers.ArrayPool`1/*0100001C*/<uint8> bytePool,
                     valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002E*/<int64> limit,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000020*/ cancellationToken) cil managed
  // SIG: 00 04 12 7D 12 6D 15 12 71 01 05 15 11 80 B9 01 0A 11 80 81
  {
    .custom /*0C0000EF:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000023*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000022*/) /* 0A000016 */ = ( 01 00 49 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..IMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 57 65 62 55 74 69 6C 69   // NetCore.WebUtili
                                                                                                                                                                                                                                        74 69 65 73 2E 53 74 72 65 61 6D 48 65 6C 70 65   // ties.StreamHelpe
                                                                                                                                                                                                                                        72 45 78 74 65 6E 73 69 6F 6E 73 2B 3C 44 72 61   // rExtensions+<Dra
                                                                                                                                                                                                                                        69 6E 41 73 79 6E 63 3E 64 5F 5F 33 00 00 )       // inAsync>d__3..
    .custom /*0C0000F0:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamHelperExtensions::DrainAsync

} // end of class Microsoft.AspNetCore.WebUtilities.StreamHelperExtensions

.class /*0200001C*/ public sequential ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable
       extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
{
  .custom /*0C000054:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000043*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
  .class /*02000047*/ sequential ansi sealed nested public beforefieldinit EncodedNameValuePair
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
  {
    .custom /*0C00008E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000043*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
    .field /*040000FC*/ private initonly valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> '<EncodedName>k__BackingField'
    .custom /*0C0000FB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*040000FD*/ private initonly valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> '<EncodedValue>k__BackingField'
    .custom /*0C0000FD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .method /*06000148*/ public hidebysig specialname 
            instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> 
            get_EncodedName() cil managed
    // SIG: 20 00 15 11 80 D9 01 03
    {
      .custom /*0C000129:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method EncodedNameValuePair::get_EncodedName

    .method /*06000149*/ public hidebysig specialname 
            instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> 
            get_EncodedValue() cil managed
    // SIG: 20 00 15 11 80 D9 01 03
    {
      .custom /*0C00012A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method EncodedNameValuePair::get_EncodedValue

    .method /*0600014A*/ assembly hidebysig specialname rtspecialname 
            instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> encodedName,
                                 valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> encodedValue) cil managed
    // SIG: 20 02 01 15 11 80 D9 01 03 15 11 80 D9 01 03
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method EncodedNameValuePair::.ctor

    .method /*0600014B*/ public hidebysig 
            instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> 
            DecodeName() cil managed
    // SIG: 20 00 15 11 80 D9 01 03
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method EncodedNameValuePair::DecodeName

    .method /*0600014C*/ public hidebysig 
            instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> 
            DecodeValue() cil managed
    // SIG: 20 00 15 11 80 D9 01 03
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method EncodedNameValuePair::DecodeValue

    .property /*17000047*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char>
            EncodedName()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable/*0200001C*//EncodedNameValuePair/*02000047*/::get_EncodedName() /* 06000148 */
    } // end of property EncodedNameValuePair::EncodedName
    .property /*17000048*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char>
            EncodedValue()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable/*0200001C*//EncodedNameValuePair/*02000047*/::get_EncodedValue() /* 06000149 */
    } // end of property EncodedNameValuePair::EncodedValue
  } // end of class EncodedNameValuePair

  .class /*02000048*/ sequential ansi sealed nested public beforefieldinit Enumerator
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
  {
    .field /*040000FE*/ private valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> _query
    .field /*040000FF*/ private valuetype Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable/*0200001C*//EncodedNameValuePair/*02000047*/ '<Current>k__BackingField'
    .custom /*0C0000FF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .method /*0600014D*/ assembly hidebysig specialname rtspecialname 
            instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> query) cil managed
    // SIG: 20 01 01 15 11 80 D9 01 03
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::.ctor

    .method /*0600014E*/ public hidebysig specialname 
            instance valuetype Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable/*0200001C*//EncodedNameValuePair/*02000047*/ 
            get_Current() cil managed
    // SIG: 20 00 11 81 1C
    {
      .custom /*0C00012B:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000043*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .custom /*0C00012C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::get_Current

    .method /*0600014F*/ public hidebysig 
            instance bool  MoveNext() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Enumerator::MoveNext

    .property /*17000049*/ instance callconv(8) valuetype Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable/*0200001C*//EncodedNameValuePair/*02000047*/
            Current()
    {
      .get instance valuetype Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable/*0200001C*//EncodedNameValuePair/*02000047*/ Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable/*0200001C*//Enumerator/*02000048*/::get_Current() /* 0600014E */
    } // end of property Enumerator::Current
  } // end of class Enumerator

  .class /*02000049*/ abstract auto ansi sealed nested private beforefieldinit SpanHelper
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .method /*06000150*/ public hidebysig static 
            string  ReplacePlusWithSpace(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100004A*/<char> span) cil managed aggressiveinlining
    // SIG: 00 01 0E 15 11 81 29 01 03
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method SpanHelper::ReplacePlusWithSpace

  } // end of class SpanHelper

  .field /*04000010*/ private initonly valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> _queryString
  .method /*060000E1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string queryString) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000F1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryStringEnumerable::.ctor

  .method /*060000E2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000036*/<char> queryString) cil managed
  // SIG: 20 01 01 15 11 80 D9 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryStringEnumerable::.ctor

  .method /*060000E3*/ public hidebysig instance valuetype Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable/*0200001C*//Enumerator/*02000048*/ 
          GetEnumerator() cil managed
  // SIG: 20 00 11 81 20
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method QueryStringEnumerable::GetEnumerator

} // end of class Microsoft.AspNetCore.WebUtilities.QueryStringEnumerable

.class /*0200001D*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.WebEncoders
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C000057:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000E4*/ public hidebysig static 
          uint8[]  Base64UrlDecode(string input) cil managed
  // SIG: 00 01 1D 05 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlDecode

  .method /*060000E5*/ public hidebysig static 
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

  .method /*060000E6*/ public hidebysig static 
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

  .method /*060000E7*/ public hidebysig static 
          int32  GetArraySizeRequiredToDecode(int32 count) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::GetArraySizeRequiredToDecode

  .method /*060000E8*/ public hidebysig static 
          string  Base64UrlEncode(uint8[] input) cil managed
  // SIG: 00 01 0E 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlEncode

  .method /*060000E9*/ public hidebysig static 
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

  .method /*060000EA*/ public hidebysig static 
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

  .method /*060000EB*/ public hidebysig static 
          int32  GetArraySizeRequiredToEncode(int32 count) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::GetArraySizeRequiredToEncode

  .method /*060000EC*/ public hidebysig static 
          string  Base64UrlEncode(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100004A*/<uint8> input) cil managed
  // SIG: 00 01 0E 15 11 81 29 01 05
  {
    .custom /*0C0000F2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0000F3:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.SkipLocalsInitAttribute/*01000052*/::.ctor() /* 0A00001B */ = ( 01 00 00 00 ) 
    .param [0]/*0800012E*/ 
    .custom /*0C00011D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebEncoders::Base64UrlEncode

} // end of class Microsoft.AspNetCore.WebUtilities.WebEncoders

.class /*0200001E*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebUtilities.Resources
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*060000ED*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000053*/ 
          get_ResourceManager() cil managed
  // SIG: 00 00 12 81 4D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_ResourceManager

  .method /*060000EE*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000054*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 81 51
  {
    .custom /*0C0000F5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Culture

  .method /*060000EF*/ assembly hidebysig specialname static 
          void  set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000054*/ 'value') cil managed
  // SIG: 00 01 01 12 81 51
  {
    .custom /*0C0000F7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::set_Culture

  .method /*060000F0*/ assembly hidebysig static 
          string  GetResourceString(string resourceKey,
                                    [opt] string defaultValue) cil managed aggressiveinlining
  // SIG: 00 02 0E 0E 0E
  {
    .param [2]/*08000132*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::GetResourceString

  .method /*060000F1*/ assembly hidebysig specialname static 
          string  get_HttpRequestStreamReader_StreamNotReadable() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_HttpRequestStreamReader_StreamNotReadable

  .method /*060000F2*/ assembly hidebysig specialname static 
          string  get_HttpResponseStreamWriter_StreamNotWritable() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_HttpResponseStreamWriter_StreamNotWritable

  .method /*060000F3*/ assembly hidebysig specialname static 
          string  get_WebEncoders_InvalidCountOffsetOrLength() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_WebEncoders_InvalidCountOffsetOrLength

  .method /*060000F4*/ assembly hidebysig static 
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
  } // end of method Resources::FormatWebEncoders_InvalidCountOffsetOrLength

  .property /*1700003E*/ callconv(8) class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000053*/
          ResourceManager()
  {
    .get class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*01000053*/ Microsoft.AspNetCore.WebUtilities.Resources/*0200001E*/::get_ResourceManager() /* 060000ED */
  } // end of property Resources::ResourceManager
  .property /*1700003F*/ callconv(8) class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000054*/
          Culture()
  {
    .get class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000054*/ Microsoft.AspNetCore.WebUtilities.Resources/*0200001E*/::get_Culture() /* 060000EE */
    .set void Microsoft.AspNetCore.WebUtilities.Resources/*0200001E*/::set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000054*/) /* 060000EF */
  } // end of property Resources::Culture
  .property /*17000040*/ callconv(8) string
          HttpRequestStreamReader_StreamNotReadable()
  {
    .get string Microsoft.AspNetCore.WebUtilities.Resources/*0200001E*/::get_HttpRequestStreamReader_StreamNotReadable() /* 060000F1 */
  } // end of property Resources::HttpRequestStreamReader_StreamNotReadable
  .property /*17000041*/ callconv(8) string
          HttpResponseStreamWriter_StreamNotWritable()
  {
    .get string Microsoft.AspNetCore.WebUtilities.Resources/*0200001E*/::get_HttpResponseStreamWriter_StreamNotWritable() /* 060000F2 */
  } // end of property Resources::HttpResponseStreamWriter_StreamNotWritable
  .property /*17000042*/ callconv(8) string
          WebEncoders_InvalidCountOffsetOrLength()
  {
    .get string Microsoft.AspNetCore.WebUtilities.Resources/*0200001E*/::get_WebEncoders_InvalidCountOffsetOrLength() /* 060000F3 */
  } // end of property Resources::WebEncoders_InvalidCountOffsetOrLength
} // end of class Microsoft.AspNetCore.WebUtilities.Resources

.class /*0200001F*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.AspNetCoreTempDirectory
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200004A*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
  {
    .custom /*0C000090:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000100*/ public static initonly class Microsoft.AspNetCore.Internal.AspNetCoreTempDirectory/*0200001F*//'<>c'/*0200004A*/ '<>9'
    .field /*04000101*/ public static class [System.Runtime/*23000001*/]System.Func`1/*0100002F*/<string> '<>9__4_0'
    .method /*06000151*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000152*/ assembly hidebysig 
            instance string  '<get_TempDirectoryFactory>b__4_0'() cil managed
    // SIG: 20 00 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<get_TempDirectoryFactory>b__4_0'

  } // end of class '<>c'

  .method /*060000F5*/ public hidebysig specialname static 
          string  get_TempDirectory() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AspNetCoreTempDirectory::get_TempDirectory

  .method /*060000F6*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Func`1/*0100002F*/<string> 
          get_TempDirectoryFactory() cil managed
  // SIG: 00 00 15 12 80 BD 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AspNetCoreTempDirectory::get_TempDirectoryFactory

  .property /*17000043*/ callconv(8) string
          TempDirectory()
  {
    .get string Microsoft.AspNetCore.Internal.AspNetCoreTempDirectory/*0200001F*/::get_TempDirectory() /* 060000F5 */
  } // end of property AspNetCoreTempDirectory::TempDirectory
  .property /*17000044*/ callconv(8) class [System.Runtime/*23000001*/]System.Func`1/*0100002F*/<string>
          TempDirectoryFactory()
  {
    .get class [System.Runtime/*23000001*/]System.Func`1/*0100002F*/<string> Microsoft.AspNetCore.Internal.AspNetCoreTempDirectory/*0200001F*/::get_TempDirectoryFactory() /* 060000F6 */
  } // end of property AspNetCoreTempDirectory::TempDirectoryFactory
} // end of class Microsoft.AspNetCore.Internal.AspNetCoreTempDirectory

.class /*02000020*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Internal.UrlDecoder
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .method /*060000F7*/ public hidebysig static 
          int32  DecodeRequestLine(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100004A*/<uint8> source,
                                   valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<uint8> destination,
                                   bool isFormEncoding) cil managed
  // SIG: 00 03 08 15 11 81 29 01 05 15 11 80 C1 01 05 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UrlDecoder::DecodeRequestLine

  .method /*060000F8*/ public hidebysig static 
          int32  DecodeInPlace(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<uint8> buffer,
                               bool isFormEncoding) cil managed
  // SIG: 00 02 08 15 11 80 C1 01 05 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UrlDecoder::DecodeInPlace

  .method /*060000F9*/ public hidebysig static 
          int32  DecodeRequestLine(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100004A*/<char> source,
                                   valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<char> destination) cil managed
  // SIG: 00 02 08 15 11 81 29 01 03 15 11 80 C1 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UrlDecoder::DecodeRequestLine

  .method /*060000FA*/ public hidebysig static 
          int32  DecodeInPlace(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000030*/<char> buffer) cil managed
  // SIG: 00 01 08 15 11 80 C1 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UrlDecoder::DecodeInPlace

  .method /*060000FB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UrlDecoder::.ctor

} // end of class Microsoft.AspNetCore.Internal.UrlDecoder

.class /*02000021*/ private auto ansi sealed '<PrivateImplementationDetails>'
       extends [System.Runtime/*23000001*/]System.Object/*0100001A*/
{
  .custom /*0C00005D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .class /*0200004B*/ explicit ansi sealed nested private '__StaticArrayInitTypeSize=256'
         extends [System.Runtime/*23000001*/]System.ValueType/*01000026*/
  {
    .pack 1
    .size 256
  } // end of class '__StaticArrayInitTypeSize=256'

  .field /*04000011*/ static assembly initonly valuetype '<PrivateImplementationDetails>'/*02000021*//'__StaticArrayInitTypeSize=256'/*0200004B*/ '21244F82B210125632917591768F6BF22EB6861F80C6C25A25BD26DFB580EA7B' at I_0000AD18
  .field /*04000012*/ static assembly initonly uint8 '380918B946A526640A40DF5DCED6516794F3D97BBD9E6BB553D037C4439F31C3' at I_0000AE18
  .field /*04000013*/ static assembly initonly uint8 '951DCEE3A7A4F3AAC67EC76A2CE4469CC76DF650F134BF2572BF60A65C982338' at I_0000AE20
} // end of class '<PrivateImplementationDetails>'


// =============================================================

.data cil I_0000AD18 = bytearray (
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 00 01 02 03 04 05 06 07 08 09 FF FF FF FF FF FF
                 FF 0A 0B 0C 0D 0E 0F FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF 0A 0B 0C 0D 0E 0F FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF
                 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF) 
.data cil I_0000AE18 = bytearray (
                 3D)                                              // =
.data cil I_0000AE19 = int8[7]
.data cil I_0000AE20 = bytearray (
                 26)                                              // &
// *********** DISASSEMBLY COMPLETE ***********************
