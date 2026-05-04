


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
// Time-date stamp:            0xdc80cb9e
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002c00
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00006b02
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
// Size of image:                  0x0000c000
// Size of headers:                0x00000400
// Checksum:                       0x0000df59
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00006ab0 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000530] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003a00 [0x00002470] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00006a94 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00002b30 Virtual Size
//              0x00004000 Virtual Address
//              0x00002c00 Size of Raw Data
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
//              0x00000530 Virtual Size
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00003200 Pointer to Raw Data
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
//              0x00003800 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000b04
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00006af2 Import Name Table
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
// 0x00004054 [0x000029c0] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00006a14 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00000d44 Size
//              '#~' Name
//    Stream 2:
//              0x00000db0 Offset
//              0x00000e68 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001c18 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001c1c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001c2c Offset
//              0x00000d94 Size
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

// File size            : 24176
// PE header size       : 1024 (536 used)    ( 4.24%)
// PE additional info   : 10855             (44.90%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.30%)
// CLR meta-data size  : 10688              (44.21%)
// CLR additional info : 128                ( 0.53%)
// CLR method headers  : 63                 ( 0.26%)
// Managed code         : 126               ( 0.52%)
// Data                 : 2560              (10.59%)
// Unaccounted          : -1340             (-5.54%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 11264
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 10688
//   Module        -    1 (10 bytes)
//   TypeDef       -   21 (294 bytes)     0 interfaces, 1 explicit layout
//   TypeRef       -   63 (378 bytes)
//   MethodDef     -   63 (882 bytes)     0 abstract, 0 native, 63 bodies
//   FieldDef      -   25 (150 bytes)     2 constant
//   MemberRef     -   32 (192 bytes)
//   ParamDef      -   64 (384 bytes)
//   MethodImpl    -    2 (12 bytes)
//   Constant      -    8 (48 bytes)
//   CustomAttribute-   90 (540 bytes)
//   ClassLayout   -    1 (8 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    4 (16 bytes)
//   Property      -   11 (66 bytes)
//   MethodSemantic-   17 (102 bytes)
//   Security      -    1 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    7 (140 bytes)
//   NestedClass   -    6 (24 bytes)
//   FieldRVA      -    1 (6 bytes)
//   Strings       -  3688 bytes
//   Blobs         -  3476 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   216 bytes

// CLR additional info : 128

// CLR method headers : 63
//   Num.of method bodies  - 63
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 63

// Managed code : 126
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class ValueStringBuilder             (sequential) (ansi) (sealed) 
// Class WebSocketMiddlewareExtensions  (public) (abstract) (auto) (ansi) (sealed) 
// Class WebSocketOptions               (public) (auto) (ansi) 
// Class Constants                      (abstract) (auto) (ansi) (sealed) 
// Class ExtendedWebSocketAcceptContext (public) (auto) (ansi) 
// Class HandshakeHelpers               (abstract) (auto) (ansi) (sealed) 
// Class WebSocketDeflateConstants      (abstract) (auto) (ansi) (sealed) 
// Class WebSocketMiddleware            (public) (auto) (ansi) 
// Class WebSocketsDependencyInjectionExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class <PrivateImplementationDetails> (auto) (ansi) (sealed) 
// Class Headers                        (abstract) (auto) (ansi) (sealed) (nested public) 
// Class UpgradeHandshake               (auto) (ansi) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class __StaticArrayInitTypeSize=36   (explicit) (ansi) (sealed) (nested private) 
// Class <AcceptAsync>d__8              (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.AspNetCore.Http.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ Microsoft.AspNetCore.Http.Features
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Net.WebSockets
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.WebSockets
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
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 81 71 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...qMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 57 65 62 53 6F 63 6B   // pNetCore.WebSock
                                                                                                                                                                                    65 74 73 2E 54 65 73 74 73 2C 20 50 75 62 6C 69   // ets.Tests, Publi
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
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 81 7B 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...{Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 57 65 62 53 6F 63 6B   // pNetCore.WebSock
                                                                                                                                                                                    65 74 73 2E 4D 69 63 72 6F 42 65 6E 63 68 6D 61   // ets.MicroBenchma
                                                                                                                                                                                    72 6B 73 2C 20 50 75 62 6C 69 63 4B 65 79 3D 30   // rks, PublicKey=0
                                                                                                                                                                                    30 32 34 30 30 30 30 30 34 38 30 30 30 30 30 39   // 0240000048000009
                                                                                                                                                                                    34 30 30 30 30 30 30 30 36 30 32 30 30 30 30 30   // 4000000060200000
                                                                                                                                                                                    30 32 34 30 30 30 30 35 32 35 33 34 31 33 31 30   // 0240000525341310
                                                                                                                                                                                    30 30 34 30 30 30 30 30 31 30 30 30 31 30 30 66   // 004000001000100f
                                                                                                                                                                                    33 33 61 32 39 30 34 34 66 61 39 64 37 34 30 63   // 33a29044fa9d740c
                                                                                                                                                                                    39 62 33 32 31 33 61 39 33 65 35 37 63 38 34 62   // 9b3213a93e57c84b
                                                                                                                                                                                    34 37 32 63 38 34 65 30 62 38 61 30 65 31 61 65   // 472c84e0b8a0e1ae
                                                                                                                                                                                    34 38 65 36 37 61 39 66 38 66 36 64 65 39 64 35   // 48e67a9f8f6de9d5
                                                                                                                                                                                    66 37 66 33 64 35 32 61 63 32 33 65 34 38 61 63   // f7f3d52ac23e48ac
                                                                                                                                                                                    35 31 38 30 31 66 31 64 63 39 35 30 61 62 65 39   // 51801f1dc950abe9
                                                                                                                                                                                    30 31 64 61 33 34 64 32 61 39 65 33 62 61 61 64   // 01da34d2a9e3baad
                                                                                                                                                                                    62 31 34 31 61 31 37 63 37 37 65 66 33 63 35 36   // b141a17c77ef3c56
                                                                                                                                                                                    35 64 64 35 65 65 35 30 35 34 62 39 31 63 66 36   // 5dd5ee5054b91cf6
                                                                                                                                                                                    33 62 62 33 63 36 61 62 38 33 66 37 32 61 62 33   // 3bb3c6ab83f72ab3
                                                                                                                                                                                    61 61 66 65 39 33 64 30 66 63 33 63 32 33 34 38   // aafe93d0fc3c2348
                                                                                                                                                                                    62 37 36 34 66 61 66 62 30 62 31 63 30 37 33 33   // b764fafb0b1c0733
                                                                                                                                                                                    64 65 35 31 34 35 39 61 65 61 62 34 36 35 38 30   // de51459aeab46580
                                                                                                                                                                                    33 38 34 62 66 39 64 37 34 63 34 65 32 38 31 36   // 384bf9d74c4e2816
                                                                                                                                                                                    34 62 37 63 64 65 32 34 37 66 38 39 31 62 61 30   // 4b7cde247f891ba0
                                                                                                                                                                                    37 38 39 31 63 39 64 38 37 32 61 64 32 62 62 00   // 7891c9d872ad2bb.
                                                                                                                                                                                    00 ) 
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 31 64   // ...CommitHash(1d
                                                                                                                                                                    63 66 37 61 63 66 61 63 66 30 66 65 31 35 34 61   // cf7acfacf0fe154a
                                                                                                                                                                    64 63 63 32 33 32 37 30 63 62 30 64 61 31 31 66   // dcc23270cb0da11f
                                                                                                                                                                    66 34 34 61 63 65 00 00 )                         // f44ace..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 31 64 63   // netcore/tree/1dc
                                                                                                                                                                    66 37 61 63 66 61 63 66 30 66 65 31 35 34 61 64   // f7acfacf0fe154ad
                                                                                                                                                                    63 63 32 33 32 37 30 63 62 30 64 61 31 31 66 66   // cc23270cb0da11ff
                                                                                                                                                                    34 34 61 63 65 00 00 )                            // 44ace..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 44 41 53 50 2E 4E 45 54 20 43 6F 72 65 20   // ..DASP.NET Core 
                                                                                                                                                                       77 65 62 20 73 6F 63 6B 65 74 20 6D 69 64 64 6C   // web socket middl
                                                                                                                                                                       65 77 61 72 65 20 66 6F 72 20 75 73 65 20 6F 6E   // eware for use on
                                                                                                                                                                       20 74 6F 70 20 6F 66 20 6F 70 61 71 75 65 20 73   //  top of opaque s
                                                                                                                                                                       65 72 76 65 72 73 2E 00 00 )                      // ervers...
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 1F 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ...Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 57 65 62 53 6F 63 6B 65   // NetCore.WebSocke
                                                                                                                                                                 74 73 00 00 )                                     // ts..
  .custom /*0C000016:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000017:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.module Microsoft.AspNetCore.WebSockets.dll
// MVID: {94dc7af1-ae24-4aa7-b821-900da9dfa755}
.custom /*0C000001:0A000013*/ instance void [System.Runtime/*23000001*/]System.Security.UnverifiableCodeAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000017*/
{
  .custom /*0C000018:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
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
  .custom /*0C00001A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
  .custom /*0C00001D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
  .custom /*0C000020:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000015*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000019*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000018*/) /* 0A000015 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private sequential ansi sealed beforefieldinit System.Text.ValueStringBuilder
       extends [System.Runtime/*23000001*/]System.ValueType/*0100001D*/
{
  .custom /*0C000023:0A000016*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsByRefLikeAttribute/*0100001A*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000017*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001B*/::.ctor(string,
                                                                                                                      bool) /* 0A000017 */ = ( 01 00 52 54 79 70 65 73 20 77 69 74 68 20 65 6D   // ..RTypes with em
                                                                                                                                               62 65 64 64 65 64 20 72 65 66 65 72 65 6E 63 65   // bedded reference
                                                                                                                                               73 20 61 72 65 20 6E 6F 74 20 73 75 70 70 6F 72   // s are not suppor
                                                                                                                                               74 65 64 20 69 6E 20 74 68 69 73 20 76 65 72 73   // ted in this vers
                                                                                                                                               69 6F 6E 20 6F 66 20 79 6F 75 72 20 63 6F 6D 70   // ion of your comp
                                                                                                                                               69 6C 65 72 2E 01 00 00 )                         // iler....
  .custom /*0C000025:0A000018*/ instance void [System.Runtime/*23000001*/]System.Reflection.DefaultMemberAttribute/*0100001C*/::.ctor(string) /* 0A000018 */ = ( 01 00 04 49 74 65 6D 00 00 )                      // ...Item..
  .field /*04000004*/ private char[] _arrayToReturnToPool
  .field /*04000005*/ private valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001E*/<char> _chars
  .field /*04000006*/ private int32 _pos
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001E*/<char> initialBuffer) cil managed
  // SIG: 20 01 01 15 11 79 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 initialCapacity) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance int32  get_Length() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::get_Length

  .method /*06000009*/ public hidebysig specialname 
          instance void  set_Length(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::set_Length

  .method /*0600000A*/ public hidebysig specialname 
          instance int32  get_Capacity() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::get_Capacity

  .method /*0600000B*/ public hidebysig instance void 
          EnsureCapacity(int32 capacity) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::EnsureCapacity

  .method /*0600000C*/ public hidebysig instance char& 
          GetPinnableReference() cil managed
  // SIG: 20 00 10 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::GetPinnableReference

  .method /*0600000D*/ public hidebysig instance char& 
          GetPinnableReference(bool terminate) cil managed
  // SIG: 20 01 10 03 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::GetPinnableReference

  .method /*0600000E*/ public hidebysig specialname 
          instance char&  get_Item(int32 index) cil managed
  // SIG: 20 01 10 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::get_Item

  .method /*0600000F*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::ToString

  .method /*06000010*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001E*/<char> 
          get_RawChars() cil managed
  // SIG: 20 00 15 11 79 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::get_RawChars

  .method /*06000011*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<char> 
          AsSpan(bool terminate) cil managed
  // SIG: 20 01 15 11 7D 01 03 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::AsSpan

  .method /*06000012*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<char> 
          AsSpan() cil managed
  // SIG: 20 00 15 11 7D 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::AsSpan

  .method /*06000013*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<char> 
          AsSpan(int32 start) cil managed
  // SIG: 20 01 15 11 7D 01 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::AsSpan

  .method /*06000014*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<char> 
          AsSpan(int32 start,
                 int32 length) cil managed
  // SIG: 20 02 15 11 7D 01 03 08 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::AsSpan

  .method /*06000015*/ public hidebysig instance bool 
          TryCopyTo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001E*/<char> destination,
                    [out] int32& charsWritten) cil managed
  // SIG: 20 02 02 15 11 79 01 03 10 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::TryCopyTo

  .method /*06000016*/ public hidebysig instance void 
          Insert(int32 index,
                 char 'value',
                 int32 count) cil managed
  // SIG: 20 03 01 08 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::Insert

  .method /*06000017*/ public hidebysig instance void 
          Insert(int32 index,
                 string s) cil managed
  // SIG: 20 02 01 08 0E
  {
    .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::Insert

  .method /*06000018*/ public hidebysig instance void 
          Append(char c) cil managed aggressiveinlining
  // SIG: 20 01 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::Append

  .method /*06000019*/ public hidebysig instance void 
          Append(string s) cil managed aggressiveinlining
  // SIG: 20 01 01 0E
  {
    .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::Append

  .method /*0600001A*/ public hidebysig instance void 
          Append(char c,
                 int32 count) cil managed
  // SIG: 20 02 01 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::Append

  .method /*0600001B*/ public hidebysig instance void 
          Append(char* 'value',
                 int32 length) cil managed
  // SIG: 20 02 01 0F 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::Append

  .method /*0600001C*/ public hidebysig instance void 
          Append(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<char> 'value') cil managed
  // SIG: 20 01 01 15 11 7D 01 03
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::Append

  .method /*0600001D*/ public hidebysig instance valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001E*/<char> 
          AppendSpan(int32 length) cil managed aggressiveinlining
  // SIG: 20 01 15 11 79 01 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::AppendSpan

  .method /*0600001E*/ public hidebysig instance void 
          Dispose() cil managed aggressiveinlining
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ValueStringBuilder::Dispose

  .property /*17000001*/ instance callconv(8) int32
          Length()
  {
    .get instance int32 System.Text.ValueStringBuilder/*02000006*/::get_Length() /* 06000008 */
    .set instance void System.Text.ValueStringBuilder/*02000006*/::set_Length(int32) /* 06000009 */
  } // end of property ValueStringBuilder::Length
  .property /*17000002*/ instance callconv(8) int32
          Capacity()
  {
    .get instance int32 System.Text.ValueStringBuilder/*02000006*/::get_Capacity() /* 0600000A */
  } // end of property ValueStringBuilder::Capacity
  .property /*17000003*/ instance callconv(8) char&
          Item(int32)
  {
    .get instance char& System.Text.ValueStringBuilder/*02000006*/::get_Item(int32) /* 0600000E */
  } // end of property ValueStringBuilder::Item
  .property /*17000004*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001E*/<char>
          RawChars()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001E*/<char> System.Text.ValueStringBuilder/*02000006*/::get_RawChars() /* 06000010 */
  } // end of property ValueStringBuilder::RawChars
} // end of class System.Text.ValueStringBuilder

.class /*02000007*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Builder.WebSocketMiddlewareExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000020*/
{
  .custom /*0C000027:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000029:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*0600001F*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000021*/ 
          UseWebSockets(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000021*/ app) cil managed
  // SIG: 00 01 12 80 85 12 80 85
  {
    .custom /*0C00003D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketMiddlewareExtensions::UseWebSockets

  .method /*06000020*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000021*/ 
          UseWebSockets(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Builder.IApplicationBuilder/*01000021*/ app,
                        class Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/ options) cil managed
  // SIG: 00 02 12 80 85 12 80 85 12 20
  {
    .custom /*0C00003E:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketMiddlewareExtensions::UseWebSockets

} // end of class Microsoft.AspNetCore.Builder.WebSocketMiddlewareExtensions

.class /*02000008*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Builder.WebSocketOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000020*/
{
  .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000021*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::.ctor

  .method /*06000022*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/ 
          get_KeepAliveInterval() cil managed
  // SIG: 20 00 11 80 89
  {
    .custom /*0C00003F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::get_KeepAliveInterval

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/ 'value') cil managed
  // SIG: 20 01 01 11 80 89
  {
    .custom /*0C000040:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::set_KeepAliveInterval

  .method /*06000024*/ public hidebysig specialname 
          instance int32  get_ReceiveBufferSize() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C000042:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::get_ReceiveBufferSize

  .method /*06000025*/ public hidebysig specialname 
          instance void  set_ReceiveBufferSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    .custom /*0C000043:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::set_ReceiveBufferSize

  .method /*06000026*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000023*/<string> 
          get_AllowedOrigins() cil managed
  // SIG: 20 00 15 12 80 8D 01 0E
  {
    .custom /*0C000044:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketOptions::get_AllowedOrigins

  .property /*17000005*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/
          KeepAliveInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/ Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/::get_KeepAliveInterval() /* 06000022 */
    .set instance void Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/::set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/) /* 06000023 */
  } // end of property WebSocketOptions::KeepAliveInterval
  .property /*17000006*/ instance callconv(8) int32
          ReceiveBufferSize()
  {
    .custom /*0C000026:0A000019*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001B*/::.ctor(string) /* 0A000019 */ = ( 01 00 4C 53 65 74 74 69 6E 67 20 74 68 69 73 20   // ..LSetting this 
                                                                                                                                                   70 72 6F 70 65 72 74 79 20 68 61 73 20 6E 6F 20   // property has no 
                                                                                                                                                   65 66 66 65 63 74 2E 20 49 74 20 77 69 6C 6C 20   // effect. It will 
                                                                                                                                                   62 65 20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20   // be removed in a 
                                                                                                                                                   66 75 74 75 72 65 20 76 65 72 73 69 6F 6E 2E 00   // future version..
                                                                                                                                                   00 ) 
    .get instance int32 Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/::get_ReceiveBufferSize() /* 06000024 */
    .set instance void Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/::set_ReceiveBufferSize(int32) /* 06000025 */
  } // end of property WebSocketOptions::ReceiveBufferSize
  .property /*17000007*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000023*/<string>
          AllowedOrigins()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000023*/<string> Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/::get_AllowedOrigins() /* 06000026 */
  } // end of property WebSocketOptions::AllowedOrigins
} // end of class Microsoft.AspNetCore.Builder.WebSocketOptions

.class /*02000009*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebSockets.Constants
       extends [System.Runtime/*23000001*/]System.Object/*01000020*/
{
  .class /*02000010*/ abstract auto ansi sealed nested public beforefieldinit Headers
         extends [System.Runtime/*23000001*/]System.Object/*01000020*/
  {
    .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400000E*/ public static literal string UpgradeWebSocket = "websocket"
    .field /*0400000F*/ public static literal string SupportedVersion = "13"
  } // end of class Headers

} // end of class Microsoft.AspNetCore.WebSockets.Constants

.class /*0200000A*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebSockets.ExtendedWebSocketAcceptContext
       extends [Microsoft.AspNetCore.Http.Features/*23000003*/]Microsoft.AspNetCore.Http.WebSocketAcceptContext/*01000024*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002F:0A000019*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001B*/::.ctor(string) /* 0A000019 */ = ( 01 00 80 8F 54 68 69 73 20 74 79 70 65 20 69 73   // ....This type is
                                                                                                                                                 20 6F 62 73 6F 6C 65 74 65 20 61 6E 64 20 77 69   //  obsolete and wi
                                                                                                                                                 6C 6C 20 62 65 20 72 65 6D 6F 76 65 64 20 69 6E   // ll be removed in
                                                                                                                                                 20 61 20 66 75 74 75 72 65 20 76 65 72 73 69 6F   //  a future versio
                                                                                                                                                 6E 2E 20 54 68 65 20 72 65 63 6F 6D 6D 65 6E 64   // n. The recommend
                                                                                                                                                 65 64 20 61 6C 74 65 72 6E 61 74 69 76 65 20 69   // ed alternative i
                                                                                                                                                 73 20 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70 4E   // s Microsoft.AspN
                                                                                                                                                 65 74 43 6F 72 65 2E 48 74 74 70 2E 57 65 62 53   // etCore.Http.WebS
                                                                                                                                                 6F 63 6B 65 74 41 63 63 65 70 74 43 6F 6E 74 65   // ocketAcceptConte
                                                                                                                                                 78 74 2E 00 00 )                                  // xt...
  .method /*06000027*/ public hidebysig specialname virtual 
          instance string  get_SubProtocol() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000045:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedWebSocketAcceptContext::get_SubProtocol

  .method /*06000028*/ public hidebysig specialname virtual 
          instance void  set_SubProtocol(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000046:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedWebSocketAcceptContext::set_SubProtocol

  .method /*06000029*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int32> 
          get_ReceiveBufferSize() cil managed
  // SIG: 20 00 15 11 80 95 01 08
  {
    .custom /*0C000047:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedWebSocketAcceptContext::get_ReceiveBufferSize

  .method /*0600002A*/ public hidebysig specialname 
          instance void  set_ReceiveBufferSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int32> 'value') cil managed
  // SIG: 20 01 01 15 11 80 95 01 08
  {
    .custom /*0C000048:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedWebSocketAcceptContext::set_ReceiveBufferSize

  .method /*0600002B*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/> 
          get_KeepAliveInterval() cil managed
  // SIG: 20 00 15 11 80 95 01 11 80 89
  {
    .custom /*0C00004A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedWebSocketAcceptContext::get_KeepAliveInterval

  .method /*0600002C*/ public hidebysig specialname 
          instance void  set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/> 'value') cil managed
  // SIG: 20 01 01 15 11 80 95 01 11 80 89
  {
    .custom /*0C00004D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedWebSocketAcceptContext::set_KeepAliveInterval

  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedWebSocketAcceptContext::.ctor

  .property /*17000008*/ instance callconv(8) string
          SubProtocol()
  {
    .get instance string Microsoft.AspNetCore.WebSockets.ExtendedWebSocketAcceptContext/*0200000A*/::get_SubProtocol() /* 06000027 */
    .set instance void Microsoft.AspNetCore.WebSockets.ExtendedWebSocketAcceptContext/*0200000A*/::set_SubProtocol(string) /* 06000028 */
  } // end of property ExtendedWebSocketAcceptContext::SubProtocol
  .property /*17000009*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int32>
          ReceiveBufferSize()
  {
    .custom /*0C00002C:0A000019*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001B*/::.ctor(string) /* 0A000019 */ = ( 01 00 4C 53 65 74 74 69 6E 67 20 74 68 69 73 20   // ..LSetting this 
                                                                                                                                                   70 72 6F 70 65 72 74 79 20 68 61 73 20 6E 6F 20   // property has no 
                                                                                                                                                   65 66 66 65 63 74 2E 20 49 74 20 77 69 6C 6C 20   // effect. It will 
                                                                                                                                                   62 65 20 72 65 6D 6F 76 65 64 20 69 6E 20 61 20   // be removed in a 
                                                                                                                                                   66 75 74 75 72 65 20 76 65 72 73 69 6F 6E 2E 00   // future version..
                                                                                                                                                   00 ) 
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int32> Microsoft.AspNetCore.WebSockets.ExtendedWebSocketAcceptContext/*0200000A*/::get_ReceiveBufferSize() /* 06000029 */
    .set instance void Microsoft.AspNetCore.WebSockets.ExtendedWebSocketAcceptContext/*0200000A*/::set_ReceiveBufferSize(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int32>) /* 0600002A */
  } // end of property ExtendedWebSocketAcceptContext::ReceiveBufferSize
  .property /*1700000A*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/>
          KeepAliveInterval()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/> Microsoft.AspNetCore.WebSockets.ExtendedWebSocketAcceptContext/*0200000A*/::get_KeepAliveInterval() /* 0600002B */
    .set instance void Microsoft.AspNetCore.WebSockets.ExtendedWebSocketAcceptContext/*0200000A*/::set_KeepAliveInterval(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/>) /* 0600002C */
  } // end of property ExtendedWebSocketAcceptContext::KeepAliveInterval
} // end of class Microsoft.AspNetCore.WebSockets.ExtendedWebSocketAcceptContext

.class /*0200000B*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebSockets.HandshakeHelpers
       extends [System.Runtime/*23000001*/]System.Object/*01000020*/
{
  .method /*0600002E*/ public hidebysig static 
          void  GenerateResponseHeaders(string key,
                                        string subProtocol,
                                        class [Microsoft.AspNetCore.Http.Features/*23000003*/]Microsoft.AspNetCore.Http.IHeaderDictionary/*01000026*/ headers) cil managed
  // SIG: 00 03 01 0E 0E 12 80 99
  {
    .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000023*/ 
    .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandshakeHelpers::GenerateResponseHeaders

  .method /*0600002F*/ public hidebysig static 
          bool  IsRequestKeyValid(string 'value') cil managed
  // SIG: 00 01 02 0E
  {
    .custom /*0C000050:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandshakeHelpers::IsRequestKeyValid

  .method /*06000030*/ public hidebysig static 
          string  CreateResponseKey(string requestKey) cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C000051:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandshakeHelpers::CreateResponseKey

  .method /*06000031*/ public hidebysig static 
          bool  ParseDeflateOptions(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<char> extension,
                                    bool serverContextTakeover,
                                    int32 serverMaxWindowBits,
                                    [out] class [System.Net.WebSockets/*23000004*/]System.Net.WebSockets.WebSocketDeflateOptions/*01000027*/& parsedOptions,
                                    [out] string& response) cil managed
  // SIG: 00 05 02 15 11 7D 01 03 02 08 10 12 80 9D 10 0E
  {
    .param [4]/*0800002A*/ 
    .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800002B*/ 
    .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00004C:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000028*/::.ctor(bool) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandshakeHelpers::ParseDeflateOptions

  .method /*06000032*/ assembly hidebysig static 
          bool  '<ParseDeflateOptions>g__ParseWindowBits|5_0'(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001F*/<char> 'value',
                                                              string propertyName,
                                                              [out] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000025*/<int32>& parsedValue) cil managed
  // SIG: 00 03 02 15 11 7D 01 03 0E 10 15 11 80 95 01 08
  {
    .custom /*0C000052:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .param [2]/*0800002D*/ 
    .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HandshakeHelpers::'<ParseDeflateOptions>g__ParseWindowBits|5_0'

} // end of class Microsoft.AspNetCore.WebSockets.HandshakeHelpers

.class /*0200000C*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebSockets.WebSocketDeflateConstants
       extends [System.Runtime/*23000001*/]System.Object/*01000020*/
{
  .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000007*/ public static literal int32 MaxExtensionLength = int32(0x00000080)
  .field /*04000008*/ public static literal string Extension = "permessage-deflate"
  .field /*04000009*/ public static literal string ClientMaxWindowBits = "client_max_window_bits"
  .field /*0400000A*/ public static literal string ClientNoContextTakeover = "client_no_context_takeover"
  .field /*0400000B*/ public static literal string ServerMaxWindowBits = "server_max_window_bits"
  .field /*0400000C*/ public static literal string ServerNoContextTakeover = "server_no_context_takeover"
} // end of class Microsoft.AspNetCore.WebSockets.WebSocketDeflateConstants

.class /*0200000D*/ public auto ansi beforefieldinit Microsoft.AspNetCore.WebSockets.WebSocketMiddleware
       extends [System.Runtime/*23000001*/]System.Object/*01000020*/
{
  .custom /*0C000032:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000011*/ auto ansi nested private beforefieldinit UpgradeHandshake
         extends [System.Runtime/*23000001*/]System.Object/*01000020*/
         implements [Microsoft.AspNetCore.Http.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IHttpWebSocketFeature/*0100002E*/
  {
    .class /*02000015*/ auto ansi sealed nested private beforefieldinit '<AcceptAsync>d__8'
           extends [System.Runtime/*23000001*/]System.ValueType/*0100001D*/
           implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/
    {
      .custom /*0C00003A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
      .field /*04000012*/ public int32 '<>1__state'
      .field /*04000013*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000036*/<class [System.Net.WebSockets/*23000004*/]System.Net.WebSockets.WebSocket/*01000034*/> '<>t__builder'
      .field /*04000014*/ public class Microsoft.AspNetCore.WebSockets.WebSocketMiddleware/*0200000D*//UpgradeHandshake/*02000011*/ '<>4__this'
      .field /*04000015*/ public class [Microsoft.AspNetCore.Http.Features/*23000003*/]Microsoft.AspNetCore.Http.WebSocketAcceptContext/*01000024*/ acceptContext
      .field /*04000016*/ private string '<subProtocol>5__2'
      .field /*04000017*/ private valuetype [System.Runtime/*23000001*/]System.TimeSpan/*01000022*/ '<keepAliveInterval>5__3'
      .field /*04000018*/ private class [System.Net.WebSockets/*23000004*/]System.Net.WebSockets.WebSocketDeflateOptions/*01000027*/ '<deflateOptions>5__4'
      .field /*04000019*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000037*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000038*/> '<>u__1'
      .method /*0600003E*/ private hidebysig newslot virtual final 
              instance void  MoveNext() cil managed
      // SIG: 20 00 01
      {
        .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::MoveNext /*01000035::0A00001C*/ 
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<AcceptAsync>d__8'::MoveNext

      .method /*0600003F*/ private hidebysig newslot virtual final 
              instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/ stateMachine) cil managed
      // SIG: 20 01 01 12 80 D5
      {
        .custom /*0C00005A:0A00001E*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000039*/::.ctor() /* 0A00001E */ = ( 01 00 00 00 ) 
        .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000035*/::SetStateMachine /*01000035::0A00001D*/ 
        // Method begins at RVA 0x4050
        // Code size       2 (0x2)
        .maxstack  8
        IL_0000:  /* 14   |                  */ ldnull
        IL_0001:  /* 7A   |                  */ throw
      } // end of method '<AcceptAsync>d__8'::SetStateMachine

    } // end of class '<AcceptAsync>d__8'

    .method /*06000036*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100002D*/ context,
                                 class [Microsoft.AspNetCore.Http.Features/*23000003*/]Microsoft.AspNetCore.Http.Features.IHttpUpgradeFeature/*0100002F*/ upgradeFeature,
                                 class Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/ options,
                                 class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000030*/ logger) cil managed
    // SIG: 20 04 01 12 80 B5 12 80 BD 12 20 12 80 C1
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method UpgradeHandshake::.ctor

    .method /*06000037*/ public hidebysig newslot specialname virtual final 
            instance bool  get_IsWebSocketRequest() cil managed
    // SIG: 20 00 02
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method UpgradeHandshake::get_IsWebSocketRequest

    .method /*06000038*/ public hidebysig newslot virtual final 
            instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000033*/<class [System.Net.WebSockets/*23000004*/]System.Net.WebSockets.WebSocket/*01000034*/> 
            AcceptAsync(class [Microsoft.AspNetCore.Http.Features/*23000003*/]Microsoft.AspNetCore.Http.WebSocketAcceptContext/*01000024*/ acceptContext) cil managed
    // SIG: 20 01 15 12 80 CD 01 12 80 D1 12 80 91
    {
      .custom /*0C000055:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*01000032*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000031*/) /* 0A00001B */ = ( 01 00 56 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..VMicrosoft.Asp
                                                                                                                                                                                                                                          4E 65 74 43 6F 72 65 2E 57 65 62 53 6F 63 6B 65   // NetCore.WebSocke
                                                                                                                                                                                                                                          74 73 2E 57 65 62 53 6F 63 6B 65 74 4D 69 64 64   // ts.WebSocketMidd
                                                                                                                                                                                                                                          6C 65 77 61 72 65 2B 55 70 67 72 61 64 65 48 61   // leware+UpgradeHa
                                                                                                                                                                                                                                          6E 64 73 68 61 6B 65 2B 3C 41 63 63 65 70 74 41   // ndshake+<AcceptA
                                                                                                                                                                                                                                          73 79 6E 63 3E 64 5F 5F 38 00 00 )                // sync>d__8..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method UpgradeHandshake::AcceptAsync

    .method /*06000039*/ public hidebysig static 
            bool  CheckSupportedWebSocketRequest(string 'method',
                                                 class [Microsoft.AspNetCore.Http.Features/*23000003*/]Microsoft.AspNetCore.Http.IHeaderDictionary/*01000026*/ requestHeaders) cil managed
    // SIG: 00 02 02 0E 12 80 99
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method UpgradeHandshake::CheckSupportedWebSocketRequest

    .property /*1700000B*/ instance callconv(8) bool
            IsWebSocketRequest()
    {
      .get instance bool Microsoft.AspNetCore.WebSockets.WebSocketMiddleware/*0200000D*//UpgradeHandshake/*02000011*/::get_IsWebSocketRequest() /* 06000037 */
    } // end of property UpgradeHandshake::IsWebSocketRequest
  } // end of class UpgradeHandshake

  .class /*02000012*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000020*/
  {
    .method /*0600003A*/ public hidebysig static 
            void  CompressionAccepted(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000030*/ logger,
                                      string compressionResponse) cil managed
    // SIG: 00 02 01 12 80 C1 0E
    {
      .custom /*0C000056:0A00001F*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100003B*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.LogLevel/*0100003A*/,
                                                                                                                                                                                string) /* 0A00001F */ = ( 01 00 01 00 00 00 01 00 00 00 4F 57 65 62 53 6F   // ..........OWebSo
                                                                                                                                                                                                           63 6B 65 74 20 63 6F 6D 70 72 65 73 73 69 6F 6E   // cket compression
                                                                                                                                                                                                           20 6E 65 67 6F 74 69 61 74 69 6F 6E 20 61 63 63   //  negotiation acc
                                                                                                                                                                                                           65 70 74 65 64 20 77 69 74 68 20 76 61 6C 75 65   // epted with value
                                                                                                                                                                                                           73 20 27 7B 43 6F 6D 70 72 65 73 73 69 6F 6E 52   // s '{CompressionR
                                                                                                                                                                                                           65 73 70 6F 6E 73 65 7D 27 2E 01 00 54 0E 09 45   // esponse}'...T..E
                                                                                                                                                                                                           76 65 6E 74 4E 61 6D 65 13 43 6F 6D 70 72 65 73   // ventName.Compres
                                                                                                                                                                                                           73 69 6F 6E 41 63 63 65 70 74 65 64 )             // sionAccepted
      .custom /*0C000057:0A000020*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100003C*/::.ctor(string,
                                                                                                                                                string) /* 0A000020 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::CompressionAccepted

    .method /*0600003B*/ public hidebysig static 
            void  CompressionNotAccepted(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000030*/ logger) cil managed
    // SIG: 00 01 01 12 80 C1
    {
      .custom /*0C000058:0A00001F*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100003B*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.LogLevel/*0100003A*/,
                                                                                                                                                                                string) /* 0A00001F */ = ( 01 00 02 00 00 00 01 00 00 00 2F 43 6F 6D 70 72   // ........../Compr
                                                                                                                                                                                                           65 73 73 69 6F 6E 20 6E 65 67 6F 74 69 61 74 69   // ession negotiati
                                                                                                                                                                                                           6F 6E 20 6E 6F 74 20 61 63 63 65 70 74 65 64 20   // on not accepted 
                                                                                                                                                                                                           62 79 20 73 65 72 76 65 72 2E 01 00 54 0E 09 45   // by server...T..E
                                                                                                                                                                                                           76 65 6E 74 4E 61 6D 65 16 43 6F 6D 70 72 65 73   // ventName.Compres
                                                                                                                                                                                                           73 69 6F 6E 4E 6F 74 41 63 63 65 70 74 65 64 )    // sionNotAccepted
      .custom /*0C000059:0A000020*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100003C*/::.ctor(string,
                                                                                                                                                string) /* 0A000020 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::CompressionNotAccepted

  } // end of class Log

  .class /*02000013*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000020*/
  {
    .custom /*0C000039:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
    .field /*04000010*/ public static initonly class Microsoft.AspNetCore.WebSockets.WebSocketMiddleware/*0200000D*//'<>c'/*02000013*/ '<>9'
    .field /*04000011*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100003D*/<string,string> '<>9__5_0'
    .method /*0600003C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*0600003D*/ assembly hidebysig 
            instance string  '<.ctor>b__5_0'(string o) cil managed
    // SIG: 20 01 0E 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__5_0'

  } // end of class '<>c'

  .method /*06000033*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000029*/ next,
                               class [Microsoft.Extensions.Options/*23000005*/]Microsoft.Extensions.Options.IOptions`1/*0100002A*/<class Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/> options,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100002B*/ loggerFactory) cil managed
  // SIG: 20 03 01 12 80 A5 15 12 80 A9 01 12 20 12 80 AD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketMiddleware::.ctor

  .method /*06000034*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002C*/ 
          Invoke(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100002D*/ context) cil managed
  // SIG: 20 01 12 80 B1 12 80 B5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketMiddleware::Invoke

} // end of class Microsoft.AspNetCore.WebSockets.WebSocketMiddleware

.class /*0200000E*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.WebSockets.WebSocketsDependencyInjectionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000020*/
{
  .custom /*0C000034:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000035*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000007*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100003E*/ 
          AddWebSockets(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000007*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*0100003E*/ services,
                        class [System.Runtime/*23000001*/]System.Action`1/*0100003F*/<class Microsoft.AspNetCore.Builder.WebSocketOptions/*02000008*/> configure) cil managed
  // SIG: 00 02 12 80 F9 12 80 F9 15 12 80 FD 01 12 20
  {
    .custom /*0C000053:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000054:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebSocketsDependencyInjectionExtensions::AddWebSockets

} // end of class Microsoft.AspNetCore.WebSockets.WebSocketsDependencyInjectionExtensions

.class /*0200000F*/ private auto ansi sealed '<PrivateImplementationDetails>'
       extends [System.Runtime/*23000001*/]System.Object/*01000020*/
{
  .custom /*0C000036:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000016*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .class /*02000014*/ explicit ansi sealed nested private '__StaticArrayInitTypeSize=36'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100001D*/
  {
    .pack 1
    .size 36
  } // end of class '__StaticArrayInitTypeSize=36'

  .field /*0400000D*/ static assembly initonly valuetype '<PrivateImplementationDetails>'/*0200000F*//'__StaticArrayInitTypeSize=36'/*02000014*/ '7F44193A6EDEA087F5C0811306EFAE871C412B46EBEE9A179FB3ACF0CB5144A3' at I_00006B08
} // end of class '<PrivateImplementationDetails>'


// =============================================================

.data cil I_00006B08 = bytearray (
                 32 35 38 45 41 46 41 35 2D 45 39 31 34 2D 34 37  // 258EAFA5-E914-47
                 44 41 2D 39 35 43 41 2D 43 35 41 42 30 44 43 38  // DA-95CA-C5AB0DC8
                 35 42 31 31)                                     // 5B11
// *********** DISASSEMBLY COMPLETE ***********************
