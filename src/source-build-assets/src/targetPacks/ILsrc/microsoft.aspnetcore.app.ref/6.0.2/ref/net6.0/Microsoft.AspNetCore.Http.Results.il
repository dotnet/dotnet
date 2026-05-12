


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
// Time-date stamp:            0x8f051a26
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00006000
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00009e7a
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
// Checksum:                       0x0000efa2
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00009e28 [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x0000053c] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00006e00 [0x00002470] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00009e0c [0x0000001c] address [size] of Debug Directory:           
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
//              0x00005e80 Virtual Size
//              0x00004000 Virtual Address
//              0x00006000 Size of Raw Data
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
//              0x0000053c Virtual Size
//              0x0000a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00006600 Pointer to Raw Data
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
//              0x00006c00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000e7c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00009e6a Import Name Table
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
// 0x00004054 [0x00005d38] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00009d8c [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00002fa4 Size
//              '#~' Name
//    Stream 2:
//              0x00003010 Offset
//              0x000017a4 Size
//              '#Strings' Name
//    Stream 3:
//              0x000047b4 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x000047b8 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x000047c8 Offset
//              0x00001570 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000020903a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 37488
// PE header size       : 1024 (536 used)    ( 2.73%)
// PE additional info   : 10867             (28.99%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.19%)
// CLR meta-data size  : 23864              (63.66%)
// CLR additional info : 128                ( 0.34%)
// CLR method headers  : 261                ( 0.70%)
// Managed code         : 522               ( 1.39%)
// Data                 : 2560              ( 6.83%)
// Unaccounted          : -1810             (-4.83%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 24576
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 23864
//   Module        -    1 (10 bytes)
//   TypeDef       -   66 (924 bytes)     2 interfaces, 0 explicit layout
//   TypeRef       -   71 (426 bytes)
//   MethodDef     -  264 (3696 bytes)    3 abstract, 0 native, 261 bodies
//   FieldDef      -   70 (420 bytes)     2 constant
//   MemberRef     -   31 (186 bytes)
//   ParamDef      -  366 (2196 bytes)
//   MethodImpl    -   15 (90 bytes)
//   Constant      -   78 (468 bytes)
//   CustomAttribute-  382 (2292 bytes)
//   InterfaceImpl -   23 (92 bytes)
//   PropertyMap   -   23 (92 bytes)
//   Property      -   56 (336 bytes)
//   MethodSemantic-   93 (558 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   10 (200 bytes)
//   NestedClass   -   23 (92 bytes)
//   Strings       -  6052 bytes
//   Blobs         -  5488 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   204 bytes

// CLR additional info : 128

// CLR method headers : 261
//   Num.of method bodies  - 261
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 261

// Managed code : 522
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class ResponseContentTypeHelper      (abstract) (auto) (ansi) (sealed) 
// Class FileResultHelper               (abstract) (auto) (ansi) (sealed) 
// Class FileResultInfo                 (sequential) (ansi) (sealed) 
// Interface IFileResultLogger              (abstract) (auto) (ansi) 
// Class SharedUrlHelper                (abstract) (auto) (ansi) (sealed) 
// Class RangeHelper                    (abstract) (auto) (ansi) (sealed) 
// Interface IResultExtensions              (public) (abstract) (auto) (ansi) 
// Class ResultExtensions               (auto) (ansi) (sealed) 
// Class Results                        (public) (abstract) (auto) (ansi) (sealed) 
// Class StreamCopyOperationInternal    (abstract) (auto) (ansi) (sealed) 
// Class ProblemDetailsDefaults         (abstract) (auto) (ansi) (sealed) 
// Class AcceptedAtRouteResult          (auto) (ansi) (sealed) 
// Class AcceptedResult                 (auto) (ansi) (sealed) 
// Class BadRequestObjectResult         (auto) (ansi) (sealed) 
// Class ChallengeResult                (auto) (ansi) (sealed) 
// Class ConflictObjectResult           (auto) (ansi) (sealed) 
// Class ContentResult                  (auto) (ansi) (sealed) 
// Class CreatedAtRouteResult           (auto) (ansi) (sealed) 
// Class CreatedResult                  (auto) (ansi) (sealed) 
// Class FileContentResult              (auto) (ansi) (sealed) 
// Class FileResult                     (abstract) (auto) (ansi) 
// Class FileStreamResult               (auto) (ansi) (sealed) 
// Class ForbidResult                   (auto) (ansi) (sealed) 
// Class JsonResult                     (auto) (ansi) (sealed) 
// Class LocalRedirectResult            (auto) (ansi) (sealed) 
// Class NoContentResult                (auto) (ansi) 
// Class NotFoundObjectResult           (auto) (ansi) (sealed) 
// Class ObjectResult                   (auto) (ansi) 
// Class OkObjectResult                 (auto) (ansi) (sealed) 
// Class PhysicalFileResult             (auto) (ansi) (sealed) 
// Class RedirectResult                 (auto) (ansi) (sealed) 
// Class RedirectToRouteResult          (auto) (ansi) (sealed) 
// Class SignInResult                   (auto) (ansi) (sealed) 
// Class SignOutResult                  (auto) (ansi) (sealed) 
// Class StatusCodeResult               (auto) (ansi) 
// Class UnauthorizedResult             (auto) (ansi) (sealed) 
// Class UnprocessableEntityObjectResult (auto) (ansi) (sealed) 
// Class VirtualFileResult              (auto) (ansi) (sealed) 
// Class PreconditionState              (auto) (ansi) (sealed) (nested assembly) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested assembly) 
// Class <WriteFileAsync>d__2           (auto) (ansi) (sealed) (nested private) 
// Class <CopyToAsync>d__2              (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__14            (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__14            (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested family) 
// Class <ExecuteAsync>d__4             (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__14            (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class FileInfoWrapper                (sequential) (ansi) (sealed) (nested assembly) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
// Class <ExecuteAsync>d__14            (auto) (ansi) (sealed) (nested private) 
// Class Log                            (abstract) (auto) (ansi) (sealed) (nested private) 
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
.assembly extern /*23000003*/ Microsoft.Net.Http.Headers
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.AspNetCore.Http.Extensions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ Microsoft.AspNetCore.Authentication.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ System.Security.Claims
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ System.Text.Json
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*2300000A*/ Microsoft.Extensions.FileProviders.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Http.Results
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

  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000005*/::.ctor() /* 0A000004 */ = ( 01 00 00 00 ) 
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000006*/::.ctor(string) /* 0A000005 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 73 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...sMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 52 65   // pNetCore.Http.Re
                                                                                                                                                                                    73 75 6C 74 73 2E 54 65 73 74 73 2C 20 50 75 62   // sults.Tests, Pub
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
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 44 41 53 50 2E 4E 45 54 20 43 6F 72 65 20   // ..DASP.NET Core 
                                                                                                                                                                       74 79 70 65 73 20 74 68 61 74 20 69 6D 70 6C 65   // types that imple
                                                                                                                                                                       6D 65 6E 74 20 4D 69 63 72 6F 73 6F 66 74 2E 41   // ment Microsoft.A
                                                                                                                                                                       73 70 4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 49   // spNetCore.Http.I
                                                                                                                                                                       52 65 73 75 6C 74 2E 00 00 )                      // Result...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 21 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..!Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 52 65 73   // NetCore.Http.Res
                                                                                                                                                                 75 6C 74 73 00 00 )                               // ults..
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
.module Microsoft.AspNetCore.Http.Results.dll
// MVID: {5ef3676a-419d-4518-a45a-4ce72f44ea3e}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
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
  .custom /*0C00001A:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
  .custom /*0C000028:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000029:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.ResponseContentTypeHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ public hidebysig static 
          void  ResolveContentTypeAndEncoding(string actionResultContentType,
                                              string httpResponseContentType,
                                              valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000018*/<string,class [System.Runtime/*23000001*/]System.Text.Encoding/*01000019*/> 'default',
                                              class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<string,class [System.Runtime/*23000001*/]System.Text.Encoding/*01000019*/> getEncoding,
                                              [out] string& resolvedContentType,
                                              [out] class [System.Runtime/*23000001*/]System.Text.Encoding/*01000019*/& resolvedContentTypeEncoding) cil managed
  // SIG: 00 06 01 0E 0E 15 11 61 02 0E 12 65 15 12 69 02 0E 12 65 10 0E 10 12 65
  {
    .param [1]/*08000001*/ 
    .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000002*/ 
    .custom /*0C000018:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000003*/ 
    .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000017*/::.ctor(string[]) /* 0A000013 */ = ( 01 00 02 00 00 00 12 64 65 66 61 75 6C 74 43 6F   // .......defaultCo
                                                                                                                                                                                       6E 74 65 6E 74 54 79 70 65 0F 64 65 66 61 75 6C   // ntentType.defaul
                                                                                                                                                                                       74 45 6E 63 6F 64 69 6E 67 00 00 )                // tEncoding..
    .custom /*0C00001E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 00 01 01 00 00 ) 
    .param [4]/*08000004*/ 
    .custom /*0C000024:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ResponseContentTypeHelper::ResolveContentTypeAndEncoding

  .method /*06000007*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Text.Encoding/*01000019*/ 
          GetEncoding(string mediaType) cil managed
  // SIG: 00 01 12 65 0E
  {
    .param [0]/*08000007*/ 
    .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ResponseContentTypeHelper::GetEncoding

} // end of class Microsoft.AspNetCore.Internal.ResponseContentTypeHelper

.class /*02000007*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.FileResultHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002C*/ auto ansi sealed nested assembly PreconditionState
         extends [System.Runtime/*23000001*/]System.Enum/*01000028*/
  {
    .custom /*0C000097:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .field /*0400000A*/ public specialname rtspecialname int32 value__
    .field /*0400000B*/ public static literal valuetype Microsoft.AspNetCore.Internal.FileResultHelper/*02000007*//PreconditionState/*0200002C*/ Unspecified = int32(0x00000000)
    .field /*0400000C*/ public static literal valuetype Microsoft.AspNetCore.Internal.FileResultHelper/*02000007*//PreconditionState/*0200002C*/ NotModified = int32(0x00000001)
    .field /*0400000D*/ public static literal valuetype Microsoft.AspNetCore.Internal.FileResultHelper/*02000007*//PreconditionState/*0200002C*/ ShouldProcess = int32(0x00000002)
    .field /*0400000E*/ public static literal valuetype Microsoft.AspNetCore.Internal.FileResultHelper/*02000007*//PreconditionState/*0200002C*/ PreconditionFailed = int32(0x00000003)
  } // end of class PreconditionState

  .class /*0200002D*/ abstract auto ansi sealed nested assembly beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C000098:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*060000DF*/ public hidebysig static 
            void  WritingRangeToBody(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger) cil managed
    // SIG: 00 01 01 12 80 99
    {
      .custom /*0C000150:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 11 00 00 00 01 00 00 00 31 57 72 69 74 69   // ..........1Writi
                                                                                                                                                                                                           6E 67 20 74 68 65 20 72 65 71 75 65 73 74 65 64   // ng the requested
                                                                                                                                                                                                           20 72 61 6E 67 65 20 6F 66 20 62 79 74 65 73 20   //  range of bytes 
                                                                                                                                                                                                           74 6F 20 74 68 65 20 62 6F 64 79 2E 01 00 54 0E   // to the body...T.
                                                                                                                                                                                                           09 45 76 65 6E 74 4E 61 6D 65 12 57 72 69 74 69   // .EventName.Writi
                                                                                                                                                                                                           6E 67 52 61 6E 67 65 54 6F 42 6F 64 79 )          // ngRangeToBody
      .custom /*0C000151:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::WritingRangeToBody

    .method /*060000E0*/ public hidebysig static 
            void  IfMatchPreconditionFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                            class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ currentETag) cil managed
    // SIG: 00 02 01 12 80 99 12 80 95
    {
      .custom /*0C000153:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 22 00 00 00 01 00 00 00 80 88 43 75 72 72   // ..".........Curr
                                                                                                                                                                                                           65 6E 74 20 72 65 71 75 65 73 74 27 73 20 49 66   // ent request's If
                                                                                                                                                                                                           2D 4D 61 74 63 68 20 68 65 61 64 65 72 20 63 68   // -Match header ch
                                                                                                                                                                                                           65 63 6B 20 66 61 69 6C 65 64 20 61 73 20 74 68   // eck failed as th
                                                                                                                                                                                                           65 20 66 69 6C 65 27 73 20 63 75 72 72 65 6E 74   // e file's current
                                                                                                                                                                                                           20 65 74 61 67 20 27 7B 43 75 72 72 65 6E 74 45   //  etag '{CurrentE
                                                                                                                                                                                                           54 61 67 7D 27 20 64 6F 65 73 20 6E 6F 74 20 6D   // Tag}' does not m
                                                                                                                                                                                                           61 74 63 68 20 77 69 74 68 20 61 6E 79 20 6F 66   // atch with any of
                                                                                                                                                                                                           20 74 68 65 20 73 75 70 70 6C 69 65 64 20 65 74   //  the supplied et
                                                                                                                                                                                                           61 67 73 2E 01 00 54 0E 09 45 76 65 6E 74 4E 61   // ags...T..EventNa
                                                                                                                                                                                                           6D 65 19 49 66 4D 61 74 63 68 50 72 65 63 6F 6E   // me.IfMatchPrecon
                                                                                                                                                                                                           64 69 74 69 6F 6E 46 61 69 6C 65 64 )             // ditionFailed
      .custom /*0C000154:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::IfMatchPreconditionFailed

    .method /*060000E1*/ public hidebysig static 
            void  IfUnmodifiedSincePreconditionFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                                      valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
                                                      valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> ifUnmodifiedSinceDate) cil managed
    // SIG: 00 03 01 12 80 99 15 11 80 8D 01 11 80 91 15 11 80 8D 01 11 80 91
    {
      .custom /*0C000155:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 23 00 00 00 01 00 00 00 80 A6 43 75 72 72   // ..#.........Curr
                                                                                                                                                                                                           65 6E 74 20 72 65 71 75 65 73 74 27 73 20 49 66   // ent request's If
                                                                                                                                                                                                           2D 55 6E 6D 6F 64 69 66 69 65 64 2D 53 69 6E 63   // -Unmodified-Sinc
                                                                                                                                                                                                           65 20 68 65 61 64 65 72 20 63 68 65 63 6B 20 66   // e header check f
                                                                                                                                                                                                           61 69 6C 65 64 20 61 73 20 74 68 65 20 66 69 6C   // ailed as the fil
                                                                                                                                                                                                           65 20 77 61 73 20 6D 6F 64 69 66 69 65 64 20 28   // e was modified (
                                                                                                                                                                                                           61 74 20 27 7B 6C 61 73 74 4D 6F 64 69 66 69 65   // at '{lastModifie
                                                                                                                                                                                                           64 7D 27 29 20 61 66 74 65 72 20 74 68 65 20 49   // d}') after the I
                                                                                                                                                                                                           66 2D 55 6E 6D 6F 64 69 66 69 65 64 2D 53 69 6E   // f-Unmodified-Sin
                                                                                                                                                                                                           63 65 20 64 61 74 65 20 27 7B 49 66 55 6E 6D 6F   // ce date '{IfUnmo
                                                                                                                                                                                                           64 69 66 69 65 64 53 69 6E 63 65 44 61 74 65 7D   // difiedSinceDate}
                                                                                                                                                                                                           27 2E 01 00 54 0E 09 45 76 65 6E 74 4E 61 6D 65   // '...T..EventName
                                                                                                                                                                                                           23 49 66 55 6E 6D 6F 64 69 66 69 65 64 53 69 6E   // #IfUnmodifiedSin
                                                                                                                                                                                                           63 65 50 72 65 63 6F 6E 64 69 74 69 6F 6E 46 61   // cePreconditionFa
                                                                                                                                                                                                           69 6C 65 64 )                                     // iled
      .custom /*0C000156:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::IfUnmodifiedSincePreconditionFailed

    .method /*060000E2*/ public hidebysig static 
            void  IfRangeLastModifiedPreconditionFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                                        valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
                                                        valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> IfRangeLastModified) cil managed
    // SIG: 00 03 01 12 80 99 15 11 80 8D 01 11 80 91 15 11 80 8D 01 11 80 91
    {
      .custom /*0C000158:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 24 00 00 00 01 00 00 00 80 83 43 6F 75 6C   // ..$.........Coul
                                                                                                                                                                                                           64 20 6E 6F 74 20 73 65 72 76 65 20 72 61 6E 67   // d not serve rang
                                                                                                                                                                                                           65 20 61 73 20 74 68 65 20 66 69 6C 65 20 77 61   // e as the file wa
                                                                                                                                                                                                           73 20 6D 6F 64 69 66 69 65 64 20 28 61 74 20 7B   // s modified (at {
                                                                                                                                                                                                           4C 61 73 74 4D 6F 64 69 66 69 65 64 7D 29 20 61   // LastModified}) a
                                                                                                                                                                                                           66 74 65 72 20 74 68 65 20 69 66 2D 52 61 6E 67   // fter the if-Rang
                                                                                                                                                                                                           65 27 73 20 6C 61 73 74 20 6D 6F 64 69 66 69 65   // e's last modifie
                                                                                                                                                                                                           64 20 64 61 74 65 20 27 7B 49 66 52 61 6E 67 65   // d date '{IfRange
                                                                                                                                                                                                           4C 61 73 74 4D 6F 64 69 66 69 65 64 7D 27 2E 01   // LastModified}'..
                                                                                                                                                                                                           00 54 0E 09 45 76 65 6E 74 4E 61 6D 65 25 49 66   // .T..EventName%If
                                                                                                                                                                                                           52 61 6E 67 65 4C 61 73 74 4D 6F 64 69 66 69 65   // RangeLastModifie
                                                                                                                                                                                                           64 50 72 65 63 6F 6E 64 69 74 69 6F 6E 46 61 69   // dPreconditionFai
                                                                                                                                                                                                           6C 65 64 )                                        // led
      .custom /*0C000159:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::IfRangeLastModifiedPreconditionFailed

    .method /*060000E3*/ public hidebysig static 
            void  IfRangeETagPreconditionFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                                class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ currentETag,
                                                class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ IfRangeETag) cil managed
    // SIG: 00 03 01 12 80 99 12 80 95 12 80 95
    {
      .custom /*0C00015A:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 25 00 00 00 01 00 00 00 72 43 6F 75 6C 64   // ..%.......rCould
                                                                                                                                                                                                           20 6E 6F 74 20 73 65 72 76 65 20 72 61 6E 67 65   //  not serve range
                                                                                                                                                                                                           20 61 73 20 74 68 65 20 66 69 6C 65 27 73 20 63   //  as the file's c
                                                                                                                                                                                                           75 72 72 65 6E 74 20 65 74 61 67 20 27 7B 43 75   // urrent etag '{Cu
                                                                                                                                                                                                           72 72 65 6E 74 45 54 61 67 7D 27 20 64 6F 65 73   // rrentETag}' does
                                                                                                                                                                                                           20 6E 6F 74 20 6D 61 74 63 68 20 74 68 65 20 49   //  not match the I
                                                                                                                                                                                                           66 2D 52 61 6E 67 65 20 65 74 61 67 20 27 7B 49   // f-Range etag '{I
                                                                                                                                                                                                           66 52 61 6E 67 65 45 54 61 67 7D 27 2E 01 00 54   // fRangeETag}'...T
                                                                                                                                                                                                           0E 09 45 76 65 6E 74 4E 61 6D 65 1D 49 66 52 61   // ..EventName.IfRa
                                                                                                                                                                                                           6E 67 65 45 54 61 67 50 72 65 63 6F 6E 64 69 74   // ngeETagPrecondit
                                                                                                                                                                                                           69 6F 6E 46 61 69 6C 65 64 )                      // ionFailed
      .custom /*0C00015B:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::IfRangeETagPreconditionFailed

    .method /*060000E4*/ public hidebysig static 
            void  NotEnabledForRangeProcessing(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger) cil managed
    // SIG: 00 01 01 12 80 99
    {
      .custom /*0C00015C:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 26 00 00 00 01 00 00 00 80 91 54 68 65 20   // ..&.........The 
                                                                                                                                                                                                           66 69 6C 65 20 72 65 73 75 6C 74 20 68 61 73 20   // file result has 
                                                                                                                                                                                                           6E 6F 74 20 62 65 65 6E 20 65 6E 61 62 6C 65 64   // not been enabled
                                                                                                                                                                                                           20 66 6F 72 20 70 72 6F 63 65 73 73 69 6E 67 20   //  for processing 
                                                                                                                                                                                                           72 61 6E 67 65 20 72 65 71 75 65 73 74 73 2E 20   // range requests. 
                                                                                                                                                                                                           54 6F 20 65 6E 61 62 6C 65 20 69 74 2C 20 73 65   // To enable it, se
                                                                                                                                                                                                           74 20 74 68 65 20 45 6E 61 62 6C 65 52 61 6E 67   // t the EnableRang
                                                                                                                                                                                                           65 50 72 6F 63 65 73 73 69 6E 67 20 70 72 6F 70   // eProcessing prop
                                                                                                                                                                                                           65 72 74 79 20 6F 6E 20 74 68 65 20 72 65 73 75   // erty on the resu
                                                                                                                                                                                                           6C 74 20 74 6F 20 27 74 72 75 65 27 2E 01 00 54   // lt to 'true'...T
                                                                                                                                                                                                           0E 09 45 76 65 6E 74 4E 61 6D 65 1C 4E 6F 74 45   // ..EventName.NotE
                                                                                                                                                                                                           6E 61 62 6C 65 64 46 6F 72 52 61 6E 67 65 50 72   // nabledForRangePr
                                                                                                                                                                                                           6F 63 65 73 73 69 6E 67 )                         // ocessing
      .custom /*0C00015D:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::NotEnabledForRangeProcessing

  } // end of class Log

  .class /*0200002E*/ auto ansi sealed nested private beforefieldinit '<WriteFileAsync>d__2'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C00009A:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400000F*/ public int32 '<>1__state'
    .field /*04000010*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000011*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ context
    .field /*04000012*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ fileStream
    .field /*04000013*/ public class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.RangeItemHeaderValue/*01000020*/ range
    .field /*04000014*/ public int64 rangeLength
    .field /*04000015*/ private class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ '<>7__wrap1'
    .field /*04000016*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .method /*060000E5*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteFileAsync>d__2'::MoveNext

    .method /*060000E6*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C00015E:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<WriteFileAsync>d__2'::SetStateMachine

  } // end of class '<WriteFileAsync>d__2'

  .method /*06000008*/ assembly hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          WriteFileAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ context,
                         class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ fileStream,
                         class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.RangeItemHeaderValue/*01000020*/ range,
                         int64 rangeLength) cil managed
  // SIG: 00 04 12 75 12 79 12 7D 12 80 81 0A
  {
    .custom /*0C000035:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100001C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0A000014 */ = ( 01 00 43 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..CMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 49 6E 74 65 72 6E 61 6C   // NetCore.Internal
                                                                                                                                                                                                                                        2E 46 69 6C 65 52 65 73 75 6C 74 48 65 6C 70 65   // .FileResultHelpe
                                                                                                                                                                                                                                        72 2B 3C 57 72 69 74 65 46 69 6C 65 41 73 79 6E   // r+<WriteFileAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 32 00 00 )                         // c>d__2..
    .param [3]/*0800000B*/ 
    .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultHelper::WriteFileAsync

  .method /*06000009*/ assembly hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ValueTuple`3/*01000021*/<class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.RangeItemHeaderValue/*01000020*/,int64,bool> 
          SetHeadersAndLog(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext,
                           [in] valuetype Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/& result,
                           valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int64> fileLength,
                           bool enableRangeProcessing,
                           valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
                           class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ etag,
                           class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger) cil managed
  // SIG: 00 07 15 11 80 85 03 12 80 81 0A 02 12 79 10 11 20 15 11 80 8D 01 0A 02 15 11 80 8D 01 11 80 91
  //      12 80 95 12 80 99
  {
    .param [0]/*0800000D*/ 
    .custom /*0C00004B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000017*/::.ctor(string[]) /* 0A000013 */ = ( 01 00 03 00 00 00 05 72 61 6E 67 65 0B 72 61 6E   // .......range.ran
                                                                                                                                                                                       67 65 4C 65 6E 67 74 68 09 73 65 72 76 65 42 6F   // geLength.serveBo
                                                                                                                                                                                       64 79 00 00 )                                     // dy..
    .custom /*0C00004C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    .param [2]/*0800000F*/ 
    .custom /*0C000055:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .param [6]/*08000013*/ 
    .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultHelper::SetHeadersAndLog

  .method /*0600000A*/ assembly hidebysig static 
          bool  IfRangeValid(class [Microsoft.AspNetCore.Http.Extensions/*23000005*/]Microsoft.AspNetCore.Http.Headers.RequestHeaders/*01000027*/ httpRequestHeaders,
                             valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
                             class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ etag,
                             class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger) cil managed
  // SIG: 00 04 02 12 80 9D 15 11 80 8D 01 11 80 91 12 80 95 12 80 99
  {
    .param [3]/*08000017*/ 
    .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultHelper::IfRangeValid

  .method /*0600000B*/ assembly hidebysig static 
          valuetype Microsoft.AspNetCore.Internal.FileResultHelper/*02000007*//PreconditionState/*0200002C*/ 
          GetPreconditionState(class [Microsoft.AspNetCore.Http.Extensions/*23000005*/]Microsoft.AspNetCore.Http.Headers.RequestHeaders/*01000027*/ httpRequestHeaders,
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
                               class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ etag,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger) cil managed
  // SIG: 00 04 11 80 B0 12 80 9D 15 11 80 8D 01 11 80 91 12 80 95 12 80 99
  {
    .param [3]/*0800001B*/ 
    .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultHelper::GetPreconditionState

} // end of class Microsoft.AspNetCore.Internal.FileResultHelper

.class /*02000008*/ private sequential ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.FileResultInfo
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000039:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
  .field /*04000004*/ private initonly string '<ContentType>k__BackingField'
  .custom /*0C000020:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .field /*04000005*/ private initonly string '<FileDownloadName>k__BackingField'
  .custom /*0C000027:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .field /*04000006*/ private initonly valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> '<LastModified>k__BackingField'
  .custom /*0C00002C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .field /*04000007*/ private initonly class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ '<EntityTag>k__BackingField'
  .custom /*0C000030:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .field /*04000008*/ private initonly bool '<EnableRangeProcessing>k__BackingField'
  .custom /*0C000036:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000048:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::get_ContentType

  .method /*0600000D*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_ContentType(string 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 0E
  {
    .custom /*0C00004A:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::set_ContentType

  .method /*0600000E*/ public hidebysig specialname 
          instance string  get_FileDownloadName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00004E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::get_FileDownloadName

  .method /*0600000F*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_FileDownloadName(string 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 0E
  {
    .custom /*0C000052:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::set_FileDownloadName

  .method /*06000010*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> 
          get_LastModified() cil managed
  // SIG: 20 00 15 11 80 8D 01 11 80 91
  {
    .custom /*0C000056:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::get_LastModified

  .method /*06000011*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_LastModified(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 15 11 80 8D 01 11 80 91
  {
    .custom /*0C000058:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::set_LastModified

  .method /*06000012*/ public hidebysig specialname 
          instance class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ 
          get_EntityTag() cil managed
  // SIG: 20 00 12 80 95
  {
    .custom /*0C00005B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00005C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::get_EntityTag

  .method /*06000013*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_EntityTag(class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 12 80 95
  {
    .custom /*0C00005F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000060:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::set_EntityTag

  .method /*06000014*/ public hidebysig specialname 
          instance bool  get_EnableRangeProcessing() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000062:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::get_EnableRangeProcessing

  .method /*06000015*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_EnableRangeProcessing(bool 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 02
  {
    .custom /*0C000065:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResultInfo::set_EnableRangeProcessing

  .property /*17000001*/ instance callconv(8) string
          ContentType()
  {
    .get instance string Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::get_ContentType() /* 0600000C */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::set_ContentType(string) /* 0600000D */
  } // end of property FileResultInfo::ContentType
  .property /*17000002*/ instance callconv(8) string
          FileDownloadName()
  {
    .get instance string Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::get_FileDownloadName() /* 0600000E */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::set_FileDownloadName(string) /* 0600000F */
  } // end of property FileResultInfo::FileDownloadName
  .property /*17000003*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/>
          LastModified()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::get_LastModified() /* 06000010 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::set_LastModified(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/>) /* 06000011 */
  } // end of property FileResultInfo::LastModified
  .property /*17000004*/ instance callconv(8) class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/
          EntityTag()
  {
    .custom /*0C000026:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::get_EntityTag() /* 06000012 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::set_EntityTag(class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/) /* 06000013 */
  } // end of property FileResultInfo::EntityTag
  .property /*17000005*/ instance callconv(8) bool
          EnableRangeProcessing()
  {
    .get instance bool Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::get_EnableRangeProcessing() /* 06000014 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Internal.FileResultInfo/*02000008*/::set_EnableRangeProcessing(bool) /* 06000015 */
  } // end of property FileResultInfo::EnableRangeProcessing
} // end of class Microsoft.AspNetCore.Internal.FileResultInfo

.class /*02000009*/ interface private abstract auto ansi Microsoft.AspNetCore.Internal.IFileResultLogger
{
  .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000016*/ public hidebysig newslot abstract virtual 
          instance void  IfUnmodifiedSincePreconditionFailed(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
                                                             valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> ifUnmodifiedSinceDate) cil managed
  // SIG: 20 02 01 15 11 80 8D 01 11 80 91 15 11 80 8D 01 11 80 91
  {
    // Method begins at RVA 0x0
  } // end of method IFileResultLogger::IfUnmodifiedSincePreconditionFailed

  .method /*06000017*/ public hidebysig newslot abstract virtual 
          instance void  IfMatchPreconditionFailed(class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ etag) cil managed
  // SIG: 20 01 01 12 80 95
  {
    // Method begins at RVA 0x0
  } // end of method IFileResultLogger::IfMatchPreconditionFailed

  .method /*06000018*/ public hidebysig newslot abstract virtual 
          instance void  NotEnabledForRangeProcessing() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method IFileResultLogger::NotEnabledForRangeProcessing

} // end of class Microsoft.AspNetCore.Internal.IFileResultLogger

.class /*0200000A*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.SharedUrlHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000019*/ assembly hidebysig static 
          string  Content(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext,
                          string contentPath) cil managed
  // SIG: 00 02 0E 12 79 0E
  {
    .param [0]/*08000025*/ 
    .custom /*0C000086:0A00001B*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullIfNotNullAttribute/*01000032*/::.ctor(string) /* 0A00001B */ = ( 01 00 0B 63 6F 6E 74 65 6E 74 50 61 74 68 00 00 ) // ...contentPath..
    .param [1]/*08000026*/ 
    .custom /*0C00008A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SharedUrlHelper::Content

  .method /*0600001A*/ assembly hidebysig static 
          bool  IsLocalUrl(string url) cil managed
  // SIG: 00 01 02 0E
  {
    .param [1]/*08000028*/ 
    .custom /*0C00008F:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000033*/::.ctor(bool) /* 0A00001C */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SharedUrlHelper::IsLocalUrl

  .method /*0600001B*/ assembly hidebysig static 
          bool  '<IsLocalUrl>g__HasControlCharacter|1_0'(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000034*/<char> readOnlySpan) cil managed
  // SIG: 00 01 02 15 11 80 D1 01 03
  {
    .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C000074:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SharedUrlHelper::'<IsLocalUrl>g__HasControlCharacter|1_0'

} // end of class Microsoft.AspNetCore.Internal.SharedUrlHelper

.class /*0200000B*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Internal.RangeHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000043:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000044:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001C*/ public hidebysig static 
          valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000018*/<bool,class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.RangeItemHeaderValue/*01000020*/> 
          ParseRange(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ context,
                     class [Microsoft.AspNetCore.Http.Extensions/*23000005*/]Microsoft.AspNetCore.Http.Headers.RequestHeaders/*01000027*/ requestHeaders,
                     int64 length,
                     class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger) cil managed
  // SIG: 00 04 15 11 61 02 02 12 80 81 12 79 12 80 9D 0A 12 80 99
  {
    .param [0]/*0800002A*/ 
    .custom /*0C000092:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000017*/::.ctor(string[]) /* 0A000013 */ = ( 01 00 02 00 00 00 0E 69 73 52 61 6E 67 65 52 65   // .......isRangeRe
                                                                                                                                                                                       71 75 65 73 74 05 72 61 6E 67 65 00 00 )          // quest.range..
    .custom /*0C000093:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHelper::ParseRange

  .method /*0600001D*/ assembly hidebysig static 
          class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.RangeItemHeaderValue/*01000020*/ 
          NormalizeRange(class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.RangeItemHeaderValue/*01000020*/ range,
                         int64 length) cil managed
  // SIG: 00 02 12 80 81 12 80 81 0A
  {
    .param [0]/*0800002F*/ 
    .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RangeHelper::NormalizeRange

} // end of class Microsoft.AspNetCore.Internal.RangeHelper

.class /*0200000C*/ interface public abstract auto ansi Microsoft.AspNetCore.Http.IResultExtensions
{
} // end of class Microsoft.AspNetCore.Http.IResultExtensions

.class /*0200000D*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.ResultExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements Microsoft.AspNetCore.Http.IResultExtensions/*0200000C*/
{
  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ResultExtensions::.ctor

} // end of class Microsoft.AspNetCore.Http.ResultExtensions

.class /*0200000E*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Results
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001F*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Challenge([opt] class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties,
                    [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
  // SIG: 00 02 12 80 D5 12 80 D9 15 12 80 DD 01 0E
  {
    .param [1]/*08000032*/  = nullref
    .custom /*0C0000A0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000033*/  = nullref
    .custom /*0C0000A2:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Challenge

  .method /*06000020*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Forbid([opt] class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties,
                 [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
  // SIG: 00 02 12 80 D5 12 80 D9 15 12 80 DD 01 0E
  {
    .param [1]/*08000034*/  = nullref
    .custom /*0C0000A5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000035*/  = nullref
    .custom /*0C0000A7:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Forbid

  .method /*06000021*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          SignIn(class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ principal,
                 [opt] class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties,
                 [opt] string authenticationScheme) cil managed
  // SIG: 00 03 12 80 D5 12 80 E1 12 80 D9 0E
  {
    .param [2]/*08000037*/  = nullref
    .custom /*0C0000AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000038*/  = nullref
    .custom /*0C0000AB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::SignIn

  .method /*06000022*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          SignOut([opt] class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties,
                  [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
  // SIG: 00 02 12 80 D5 12 80 D9 15 12 80 DD 01 0E
  {
    .param [1]/*08000039*/  = nullref
    .custom /*0C0000AD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800003A*/  = nullref
    .custom /*0C0000AE:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::SignOut

  .method /*06000023*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Content(string content,
                  [opt] string contentType,
                  [opt] class [System.Runtime/*23000001*/]System.Text.Encoding/*01000019*/ contentEncoding) cil managed
  // SIG: 00 03 12 80 D5 0E 0E 12 65
  {
    .param [2]/*0800003C*/  = nullref
    .custom /*0C0000B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800003D*/  = nullref
    .custom /*0C0000B7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Content

  .method /*06000024*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Text(string content,
               [opt] string contentType,
               [opt] class [System.Runtime/*23000001*/]System.Text.Encoding/*01000019*/ contentEncoding) cil managed
  // SIG: 00 03 12 80 D5 0E 0E 12 65
  {
    .param [2]/*0800003F*/  = nullref
    .custom /*0C0000BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000040*/  = nullref
    .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Text

  .method /*06000025*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Content(string content,
                  class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.MediaTypeHeaderValue/*01000039*/ contentType) cil managed
  // SIG: 00 02 12 80 D5 0E 12 80 E5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Content

  .method /*06000026*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Json(object data,
               [opt] class [System.Text.Json/*23000008*/]System.Text.Json.JsonSerializerOptions/*0100003A*/ options,
               [opt] string contentType,
               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> statusCode) cil managed
  // SIG: 00 04 12 80 D5 1C 12 80 E9 0E 15 11 80 8D 01 08
  {
    .custom /*0C000087:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000043*/ 
    .custom /*0C0000BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000045*/  = nullref
    .param [3]/*08000046*/  = nullref
    .param [4]/*08000047*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Json

  .method /*06000027*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          File(uint8[] fileContents,
               [opt] string contentType,
               [opt] string fileDownloadName,
               [opt] bool enableRangeProcessing,
               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
               [opt] class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ entityTag) cil managed
  // SIG: 00 06 12 80 D5 1D 05 0E 0E 02 15 11 80 8D 01 11 80 91 12 80 95
  {
    .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000048*/ 
    .custom /*0C0000C4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000049*/ 
    .custom /*0C0000C6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800004A*/  = nullref
    .param [3]/*0800004B*/  = nullref
    .param [4]/*0800004C*/  = bool(false)
    .param [5]/*0800004D*/  = nullref
    .param [6]/*0800004E*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::File

  .method /*06000028*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Bytes(uint8[] contents,
                [opt] string contentType,
                [opt] string fileDownloadName,
                [opt] bool enableRangeProcessing,
                [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
                [opt] class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ entityTag) cil managed
  // SIG: 00 06 12 80 D5 1D 05 0E 0E 02 15 11 80 8D 01 11 80 91 12 80 95
  {
    .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800004F*/ 
    .custom /*0C0000CB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000050*/ 
    .custom /*0C0000CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000051*/  = nullref
    .param [3]/*08000052*/  = nullref
    .param [4]/*08000053*/  = bool(false)
    .param [5]/*08000054*/  = nullref
    .param [6]/*08000055*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Bytes

  .method /*06000029*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          File(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ fileStream,
               [opt] string contentType,
               [opt] string fileDownloadName,
               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
               [opt] class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ entityTag,
               [opt] bool enableRangeProcessing) cil managed
  // SIG: 00 06 12 80 D5 12 7D 0E 0E 15 11 80 8D 01 11 80 91 12 80 95 02
  {
    .custom /*0C000090:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000056*/ 
    .custom /*0C0000D2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000057*/ 
    .custom /*0C0000D5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000058*/  = nullref
    .param [3]/*08000059*/  = nullref
    .param [4]/*0800005A*/  = nullref
    .param [5]/*0800005B*/  = nullref
    .param [6]/*0800005C*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::File

  .method /*0600002A*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Stream(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ 'stream',
                 [opt] string contentType,
                 [opt] string fileDownloadName,
                 [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
                 [opt] class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ entityTag,
                 [opt] bool enableRangeProcessing) cil managed
  // SIG: 00 06 12 80 D5 12 7D 0E 0E 15 11 80 8D 01 11 80 91 12 80 95 02
  {
    .custom /*0C000091:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800005D*/ 
    .custom /*0C0000DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800005E*/ 
    .custom /*0C0000DE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800005F*/  = nullref
    .param [3]/*08000060*/  = nullref
    .param [4]/*08000061*/  = nullref
    .param [5]/*08000062*/  = nullref
    .param [6]/*08000063*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Stream

  .method /*0600002B*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          File(string path,
               [opt] string contentType,
               [opt] string fileDownloadName,
               [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> lastModified,
               [opt] class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ entityTag,
               [opt] bool enableRangeProcessing) cil managed
  // SIG: 00 06 12 80 D5 0E 0E 0E 15 11 80 8D 01 11 80 91 12 80 95 02
  {
    .custom /*0C000094:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000064*/ 
    .custom /*0C0000E3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000065*/ 
    .custom /*0C0000E5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000066*/  = nullref
    .param [3]/*08000067*/  = nullref
    .param [4]/*08000068*/  = nullref
    .param [5]/*08000069*/  = nullref
    .param [6]/*0800006A*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::File

  .method /*0600002C*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Redirect(string url,
                   [opt] bool permanent,
                   [opt] bool preserveMethod) cil managed
  // SIG: 00 03 12 80 D5 0E 02 02
  {
    .param [2]/*0800006C*/  = bool(false)
    .param [3]/*0800006D*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Redirect

  .method /*0600002D*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          LocalRedirect(string localUrl,
                        [opt] bool permanent,
                        [opt] bool preserveMethod) cil managed
  // SIG: 00 03 12 80 D5 0E 02 02
  {
    .param [2]/*0800006F*/  = bool(false)
    .param [3]/*08000070*/  = bool(false)
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::LocalRedirect

  .method /*0600002E*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          RedirectToRoute([opt] string routeName,
                          [opt] object routeValues,
                          [opt] bool permanent,
                          [opt] bool preserveMethod,
                          [opt] string fragment) cil managed
  // SIG: 00 05 12 80 D5 0E 1C 02 02 0E
  {
    .custom /*0C000099:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000071*/ 
    .custom /*0C0000ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000072*/  = nullref
    .param [2]/*08000073*/  = nullref
    .param [3]/*08000074*/  = bool(false)
    .param [4]/*08000075*/  = bool(false)
    .param [5]/*08000076*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::RedirectToRoute

  .method /*0600002F*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          StatusCode(int32 statusCode) cil managed
  // SIG: 00 01 12 80 D5 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::StatusCode

  .method /*06000030*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          NotFound([opt] object 'value') cil managed
  // SIG: 00 01 12 80 D5 1C
  {
    .param [1]/*08000078*/  = nullref
    .custom /*0C0000F4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::NotFound

  .method /*06000031*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Unauthorized() cil managed
  // SIG: 00 00 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Unauthorized

  .method /*06000032*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          BadRequest([opt] object 'error') cil managed
  // SIG: 00 01 12 80 D5 1C
  {
    .param [1]/*08000079*/  = nullref
    .custom /*0C0000F7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::BadRequest

  .method /*06000033*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Conflict([opt] object 'error') cil managed
  // SIG: 00 01 12 80 D5 1C
  {
    .param [1]/*0800007A*/  = nullref
    .custom /*0C0000F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Conflict

  .method /*06000034*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          NoContent() cil managed
  // SIG: 00 00 12 80 D5
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::NoContent

  .method /*06000035*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Ok([opt] object 'value') cil managed
  // SIG: 00 01 12 80 D5 1C
  {
    .param [1]/*0800007B*/  = nullref
    .custom /*0C0000FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Ok

  .method /*06000036*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          UnprocessableEntity([opt] object 'error') cil managed
  // SIG: 00 01 12 80 D5 1C
  {
    .param [1]/*0800007C*/  = nullref
    .custom /*0C0000FC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::UnprocessableEntity

  .method /*06000037*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Problem([opt] string detail,
                  [opt] string 'instance',
                  [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> statusCode,
                  [opt] string title,
                  [opt] string 'type',
                  [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100003B*/<string,object> extensions) cil managed
  // SIG: 00 06 12 80 D5 0E 0E 15 11 80 8D 01 08 0E 0E 15 12 80 ED 02 0E 1C
  {
    .custom /*0C0000A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800007D*/ 
    .custom /*0C0000FE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800007E*/  = nullref
    .param [2]/*0800007F*/  = nullref
    .param [3]/*08000080*/  = nullref
    .param [4]/*08000081*/  = nullref
    .param [5]/*08000082*/  = nullref
    .param [6]/*08000083*/  = nullref
    .custom /*0C000101:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Problem

  .method /*06000038*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Problem(class [Microsoft.AspNetCore.Http.Extensions/*23000005*/]Microsoft.AspNetCore.Mvc.ProblemDetails/*0100003C*/ problemDetails) cil managed
  // SIG: 00 01 12 80 D5 12 80 F1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Problem

  .method /*06000039*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          ValidationProblem(class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100003B*/<string,string[]> errors,
                            [opt] string detail,
                            [opt] string 'instance',
                            [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> statusCode,
                            [opt] string title,
                            [opt] string 'type',
                            [opt] class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*0100003B*/<string,object> extensions) cil managed
  // SIG: 00 07 12 80 D5 15 12 80 ED 02 0E 1D 0E 0E 0E 15 11 80 8D 01 08 0E 0E 15 12 80 ED 02 0E 1C
  {
    .custom /*0C0000AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000085*/ 
    .custom /*0C000103:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000086*/ 
    .custom /*0C000105:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000087*/  = nullref
    .param [3]/*08000088*/  = nullref
    .param [4]/*08000089*/  = nullref
    .param [5]/*0800008A*/  = nullref
    .param [6]/*0800008B*/  = nullref
    .param [7]/*0800008C*/  = nullref
    .custom /*0C00010E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 02 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::ValidationProblem

  .method /*0600003A*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Created(string uri,
                  object 'value') cil managed
  // SIG: 00 02 12 80 D5 0E 1C
  {
    .param [2]/*0800008E*/ 
    .custom /*0C000111:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Created

  .method /*0600003B*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Created(class [System.Runtime/*23000001*/]System.Uri/*0100003D*/ uri,
                  object 'value') cil managed
  // SIG: 00 02 12 80 D5 12 80 F5 1C
  {
    .param [2]/*08000090*/ 
    .custom /*0C000114:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Created

  .method /*0600003C*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          CreatedAtRoute([opt] string routeName,
                         [opt] object routeValues,
                         [opt] object 'value') cil managed
  // SIG: 00 03 12 80 D5 0E 1C 1C
  {
    .custom /*0C0000B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000091*/ 
    .custom /*0C000116:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000092*/  = nullref
    .param [2]/*08000093*/  = nullref
    .param [3]/*08000094*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::CreatedAtRoute

  .method /*0600003D*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          Accepted([opt] string uri,
                   [opt] object 'value') cil managed
  // SIG: 00 02 12 80 D5 0E 1C
  {
    .custom /*0C0000B5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000095*/ 
    .custom /*0C000117:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000096*/  = nullref
    .param [2]/*08000097*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::Accepted

  .method /*0600003E*/ public hidebysig static 
          class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/ 
          AcceptedAtRoute([opt] string routeName,
                          [opt] object routeValues,
                          [opt] object 'value') cil managed
  // SIG: 00 03 12 80 D5 0E 1C 1C
  {
    .custom /*0C0000B8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000098*/ 
    .custom /*0C00011A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000099*/  = nullref
    .param [2]/*0800009A*/  = nullref
    .param [3]/*0800009B*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::AcceptedAtRoute

  .method /*0600003F*/ public hidebysig specialname static 
          class Microsoft.AspNetCore.Http.IResultExtensions/*0200000C*/ 
          get_Extensions() cil managed
  // SIG: 00 00 12 30
  {
    .custom /*0C0000BA:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Results::get_Extensions

  .property /*17000006*/ callconv(8) class Microsoft.AspNetCore.Http.IResultExtensions/*0200000C*/
          Extensions()
  {
    .get class Microsoft.AspNetCore.Http.IResultExtensions/*0200000C*/ Microsoft.AspNetCore.Http.Results/*0200000E*/::get_Extensions() /* 0600003F */
  } // end of property Results::Extensions
} // end of class Microsoft.AspNetCore.Http.Results

.class /*0200000F*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.StreamCopyOperationInternal
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000053:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200002F*/ auto ansi sealed nested private beforefieldinit '<CopyToAsync>d__2'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C00009B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000017*/ public int32 '<>1__state'
    .field /*04000018*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000019*/ public valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int64> count
    .field /*0400001A*/ public int32 bufferSize
    .field /*0400001B*/ public valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003E*/ cancel
    .field /*0400001C*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ source
    .field /*0400001D*/ public class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ destination
    .field /*0400001E*/ private valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int64> '<bytesRemaining>5__2'
    .field /*0400001F*/ private uint8[] '<buffer>5__3'
    .field /*04000020*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter`1/*0100003F*/<int32> '<>u__1'
    .field /*04000021*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000040*/ '<>u__2'
    .method /*060000E7*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CopyToAsync>d__2'::MoveNext

    .method /*060000E8*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C00015F:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<CopyToAsync>d__2'::SetStateMachine

  } // end of class '<CopyToAsync>d__2'

  .method /*06000040*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ source,
                      class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ destination,
                      valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int64> count,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003E*/ cancel) cil managed
  // SIG: 00 04 12 75 12 7D 12 7D 15 11 80 8D 01 0A 11 80 F9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamCopyOperationInternal::CopyToAsync

  .method /*06000041*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          CopyToAsync(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ source,
                      class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ destination,
                      valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int64> count,
                      int32 bufferSize,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100003E*/ cancel) cil managed
  // SIG: 00 05 12 75 12 7D 12 7D 15 11 80 8D 01 0A 08 11 80 F9
  {
    .custom /*0C0000BD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100001C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0A000014 */ = ( 01 00 47 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..GMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 53 74 72   // NetCore.Http.Str
                                                                                                                                                                                                                                        65 61 6D 43 6F 70 79 4F 70 65 72 61 74 69 6F 6E   // eamCopyOperation
                                                                                                                                                                                                                                        49 6E 74 65 72 6E 61 6C 2B 3C 43 6F 70 79 54 6F   // Internal+<CopyTo
                                                                                                                                                                                                                                        41 73 79 6E 63 3E 64 5F 5F 32 00 00 )             // Async>d__2..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StreamCopyOperationInternal::CopyToAsync

} // end of class Microsoft.AspNetCore.Http.StreamCopyOperationInternal

.class /*02000010*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Extensions.ProblemDetailsDefaults
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .field /*04000009*/ public static initonly class [System.Collections/*23000009*/]System.Collections.Generic.Dictionary`2/*01000041*/<int32,valuetype [System.Runtime/*23000001*/]System.ValueTuple`2/*01000018*/<string,string>> Defaults
  .custom /*0C00003C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TupleElementNamesAttribute/*01000017*/::.ctor(string[]) /* 0A000013 */ = ( 01 00 02 00 00 00 04 54 79 70 65 05 54 69 74 6C   // .......Type.Titl
                                                                                                                                                                                     65 00 00 )                                        // e..
  .custom /*0C00003D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 01 00 01 01 00 00 ) 
} // end of class Microsoft.AspNetCore.Http.Extensions.ProblemDetailsDefaults

.class /*02000011*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.AcceptedAtRouteResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .custom /*0C000059:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000042*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object routeValues,
                               object 'value') cil managed
  // SIG: 20 02 01 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedAtRouteResult::.ctor

  .method /*06000043*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string routeName,
                               object routeValues,
                               object 'value') cil managed
  // SIG: 20 03 01 0E 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedAtRouteResult::.ctor

  .method /*06000044*/ public hidebysig specialname 
          instance string  get_RouteName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C0:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedAtRouteResult::get_RouteName

  .method /*06000045*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/ 
          get_RouteValues() cil managed
  // SIG: 20 00 12 81 09
  {
    .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000C2:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedAtRouteResult::get_RouteValues

  .method /*06000046*/ family hidebysig virtual 
          instance void  ConfigureResponseHeaders(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ context) cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C0000C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedAtRouteResult::ConfigureResponseHeaders

  .property /*17000007*/ instance callconv(8) string
          RouteName()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.AcceptedAtRouteResult/*02000011*/::get_RouteName() /* 06000044 */
  } // end of property AcceptedAtRouteResult::RouteName
  .property /*17000008*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/
          RouteValues()
  {
    .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/ Microsoft.AspNetCore.Http.Result.AcceptedAtRouteResult/*02000011*/::get_RouteValues() /* 06000045 */
  } // end of property AcceptedAtRouteResult::RouteValues
} // end of class Microsoft.AspNetCore.Http.Result.AcceptedAtRouteResult

.class /*02000012*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.AcceptedResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .custom /*0C00005D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000047*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedResult::.ctor

  .method /*06000048*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string location,
                               object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedResult::.ctor

  .method /*06000049*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Uri/*0100003D*/ locationUri,
                               object 'value') cil managed
  // SIG: 20 02 01 12 80 F5 1C
  {
    .custom /*0C0000C5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000AE*/ 
    .custom /*0C00012A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedResult::.ctor

  .method /*0600004A*/ public hidebysig specialname 
          instance string  get_Location() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C7:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedResult::get_Location

  .method /*0600004B*/ public hidebysig specialname 
          instance void  set_Location(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C8:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedResult::set_Location

  .method /*0600004C*/ family hidebysig virtual 
          instance void  ConfigureResponseHeaders(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ context) cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C0000C9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AcceptedResult::ConfigureResponseHeaders

  .property /*17000009*/ instance callconv(8) string
          Location()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.AcceptedResult/*02000012*/::get_Location() /* 0600004A */
    .set instance void Microsoft.AspNetCore.Http.Result.AcceptedResult/*02000012*/::set_Location(string) /* 0600004B */
  } // end of property AcceptedResult::Location
} // end of class Microsoft.AspNetCore.Http.Result.AcceptedResult

.class /*02000013*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.BadRequestObjectResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .method /*0600004D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'error') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000CA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BadRequestObjectResult::.ctor

} // end of class Microsoft.AspNetCore.Http.Result.BadRequestObjectResult

.class /*02000014*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.ChallengeResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000063:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000030*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*060000E9*/ public hidebysig static 
            void  ChallengeResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                           class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
    // SIG: 00 02 01 12 80 99 15 12 80 DD 01 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ChallengeResultExecuting

  } // end of class Log

  .class /*02000031*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__14'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C00009E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000022*/ public int32 '<>1__state'
    .field /*04000023*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000024*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext
    .field /*04000025*/ public class Microsoft.AspNetCore.Http.Result.ChallengeResult/*02000014*/ '<>4__this'
    .field /*04000026*/ private class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerator`1/*01000043*/<string> '<>7__wrap1'
    .field /*04000027*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .method /*060000EA*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__14'::MoveNext

    .method /*060000EB*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C000160:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__14'::SetStateMachine

  } // end of class '<ExecuteAsync>d__14'

  .method /*0600004E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::.ctor

  .method /*0600004F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string authenticationScheme) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::.ctor

  .method /*06000050*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
  // SIG: 20 01 01 15 12 80 DD 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::.ctor

  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 01 01 12 80 D9
  {
    .custom /*0C0000CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::.ctor

  .method /*06000052*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string authenticationScheme,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 02 01 0E 12 80 D9
  {
    .param [2]/*080000B6*/ 
    .custom /*0C00012D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::.ctor

  .method /*06000053*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 02 01 15 12 80 DD 01 0E 12 80 D9
  {
    .param [2]/*080000B8*/ 
    .custom /*0C00012E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::.ctor

  .method /*06000054*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> 
          get_AuthenticationSchemes() cil managed
  // SIG: 20 00 15 12 80 DD 01 0E
  {
    .custom /*0C0000CE:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::get_AuthenticationSchemes

  .method /*06000055*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_AuthenticationSchemes(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 15 12 80 DD 01 0E
  {
    .custom /*0C0000CF:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::set_AuthenticationSchemes

  .method /*06000056*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 80 D9
  {
    .custom /*0C0000D0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D1:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::get_Properties

  .method /*06000057*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_Properties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 12 80 D9
  {
    .custom /*0C0000D3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000D4:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::set_Properties

  .method /*06000058*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .custom /*0C0000D6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100001C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0A000014 */ = ( 01 00 44 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..DMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 52 65 73   // NetCore.Http.Res
                                                                                                                                                                                                                                        75 6C 74 2E 43 68 61 6C 6C 65 6E 67 65 52 65 73   // ult.ChallengeRes
                                                                                                                                                                                                                                        75 6C 74 2B 3C 45 78 65 63 75 74 65 41 73 79 6E   // ult+<ExecuteAsyn
                                                                                                                                                                                                                                        63 3E 64 5F 5F 31 34 00 00 )                      // c>d__14..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChallengeResult::ExecuteAsync

  .property /*1700000A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string>
          AuthenticationSchemes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> Microsoft.AspNetCore.Http.Result.ChallengeResult/*02000014*/::get_AuthenticationSchemes() /* 06000054 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.ChallengeResult/*02000014*/::set_AuthenticationSchemes(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string>) /* 06000055 */
  } // end of property ChallengeResult::AuthenticationSchemes
  .property /*1700000B*/ instance callconv(8) class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/
          Properties()
  {
    .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ Microsoft.AspNetCore.Http.Result.ChallengeResult/*02000014*/::get_Properties() /* 06000056 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.ChallengeResult/*02000014*/::set_Properties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/) /* 06000057 */
  } // end of property ChallengeResult::Properties
} // end of class Microsoft.AspNetCore.Http.Result.ChallengeResult

.class /*02000015*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.ConflictObjectResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .method /*06000059*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'error') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ConflictObjectResult::.ctor

} // end of class Microsoft.AspNetCore.Http.Result.ConflictObjectResult

.class /*02000016*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.ContentResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000066:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000032*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*060000EC*/ assembly hidebysig static 
            void  ContentResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                         string contentType) cil managed
    // SIG: 00 02 01 12 80 99 0E
    {
      .custom /*0C000161:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 01 00 00 00 02 00 00 00 47 45 78 65 63 75   // ..........GExecu
                                                                                                                                                                                                           74 69 6E 67 20 43 6F 6E 74 65 6E 74 52 65 73 75   // ting ContentResu
                                                                                                                                                                                                           6C 74 20 77 69 74 68 20 48 54 54 50 20 52 65 73   // lt with HTTP Res
                                                                                                                                                                                                           70 6F 6E 73 65 20 43 6F 6E 74 65 6E 74 54 79 70   // ponse ContentTyp
                                                                                                                                                                                                           65 20 6F 66 20 7B 43 6F 6E 74 65 6E 74 54 79 70   // e of {ContentTyp
                                                                                                                                                                                                           65 7D 01 00 54 0E 09 45 76 65 6E 74 4E 61 6D 65   // e}..T..EventName
                                                                                                                                                                                                           16 43 6F 6E 74 65 6E 74 52 65 73 75 6C 74 45 78   // .ContentResultEx
                                                                                                                                                                                                           65 63 75 74 69 6E 67 )                            // ecuting
      .custom /*0C000162:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ContentResultExecuting

  } // end of class Log

  .class /*02000033*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__14'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0000A1:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000028*/ public int32 '<>1__state'
    .field /*04000029*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*0400002A*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext
    .field /*0400002B*/ public class Microsoft.AspNetCore.Http.Result.ContentResult/*02000016*/ '<>4__this'
    .field /*0400002C*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .method /*060000ED*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__14'::MoveNext

    .method /*060000EE*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C000163:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__14'::SetStateMachine

  } // end of class '<ExecuteAsync>d__14'

  .method /*0600005A*/ public hidebysig specialname 
          instance string  get_Content() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000D8:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentResult::get_Content

  .method /*0600005B*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_Content(string 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 0E
  {
    .custom /*0C0000D9:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentResult::set_Content

  .method /*0600005C*/ public hidebysig specialname 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000DA:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentResult::get_ContentType

  .method /*0600005D*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_ContentType(string 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 0E
  {
    .custom /*0C0000DB:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentResult::set_ContentType

  .method /*0600005E*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> 
          get_StatusCode() cil managed
  // SIG: 20 00 15 11 80 8D 01 08
  {
    .custom /*0C0000DD:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentResult::get_StatusCode

  .method /*0600005F*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_StatusCode(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 15 11 80 8D 01 08
  {
    .custom /*0C0000DF:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentResult::set_StatusCode

  .method /*06000060*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .custom /*0C0000E0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000E1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100001C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0A000014 */ = ( 01 00 42 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..BMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 52 65 73   // NetCore.Http.Res
                                                                                                                                                                                                                                        75 6C 74 2E 43 6F 6E 74 65 6E 74 52 65 73 75 6C   // ult.ContentResul
                                                                                                                                                                                                                                        74 2B 3C 45 78 65 63 75 74 65 41 73 79 6E 63 3E   // t+<ExecuteAsync>
                                                                                                                                                                                                                                        64 5F 5F 31 34 00 00 )                            // d__14..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentResult::ExecuteAsync

  .method /*06000061*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentResult::.ctor

  .property /*1700000C*/ instance callconv(8) string
          Content()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.ContentResult/*02000016*/::get_Content() /* 0600005A */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.ContentResult/*02000016*/::set_Content(string) /* 0600005B */
  } // end of property ContentResult::Content
  .property /*1700000D*/ instance callconv(8) string
          ContentType()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.ContentResult/*02000016*/::get_ContentType() /* 0600005C */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.ContentResult/*02000016*/::set_ContentType(string) /* 0600005D */
  } // end of property ContentResult::ContentType
  .property /*1700000E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32>
          StatusCode()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> Microsoft.AspNetCore.Http.Result.ContentResult/*02000016*/::get_StatusCode() /* 0600005E */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.ContentResult/*02000016*/::set_StatusCode(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32>) /* 0600005F */
  } // end of property ContentResult::StatusCode
} // end of class Microsoft.AspNetCore.Http.Result.ContentResult

.class /*02000017*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.CreatedAtRouteResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000062*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object routeValues,
                               object 'value') cil managed
  // SIG: 20 02 01 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedAtRouteResult::.ctor

  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string routeName,
                               object routeValues,
                               object 'value') cil managed
  // SIG: 20 03 01 0E 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedAtRouteResult::.ctor

  .method /*06000064*/ public hidebysig specialname 
          instance string  get_RouteName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E2:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedAtRouteResult::get_RouteName

  .method /*06000065*/ public hidebysig specialname 
          instance void  set_RouteName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000E4:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedAtRouteResult::set_RouteName

  .method /*06000066*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/ 
          get_RouteValues() cil managed
  // SIG: 20 00 12 81 09
  {
    .custom /*0C0000E6:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedAtRouteResult::get_RouteValues

  .method /*06000067*/ public hidebysig specialname 
          instance void  set_RouteValues(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/ 'value') cil managed
  // SIG: 20 01 01 12 81 09
  {
    .custom /*0C0000E7:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedAtRouteResult::set_RouteValues

  .method /*06000068*/ family hidebysig virtual 
          instance void  ConfigureResponseHeaders(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ context) cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C0000E8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedAtRouteResult::ConfigureResponseHeaders

  .property /*1700000F*/ instance callconv(8) string
          RouteName()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.CreatedAtRouteResult/*02000017*/::get_RouteName() /* 06000064 */
    .set instance void Microsoft.AspNetCore.Http.Result.CreatedAtRouteResult/*02000017*/::set_RouteName(string) /* 06000065 */
  } // end of property CreatedAtRouteResult::RouteName
  .property /*17000010*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/
          RouteValues()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/ Microsoft.AspNetCore.Http.Result.CreatedAtRouteResult/*02000017*/::get_RouteValues() /* 06000066 */
    .set instance void Microsoft.AspNetCore.Http.Result.CreatedAtRouteResult/*02000017*/::set_RouteValues(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/) /* 06000067 */
  } // end of property CreatedAtRouteResult::RouteValues
} // end of class Microsoft.AspNetCore.Http.Result.CreatedAtRouteResult

.class /*02000018*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.CreatedResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .custom /*0C00006C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string location,
                               object 'value') cil managed
  // SIG: 20 02 01 0E 1C
  {
    .param [2]/*080000CA*/ 
    .custom /*0C000140:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedResult::.ctor

  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Uri/*0100003D*/ location,
                               object 'value') cil managed
  // SIG: 20 02 01 12 80 F5 1C
  {
    .param [2]/*080000CC*/ 
    .custom /*0C000141:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedResult::.ctor

  .method /*0600006B*/ public hidebysig specialname 
          instance string  get_Location() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000E9:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedResult::get_Location

  .method /*0600006C*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_Location(string 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 0E
  {
    .custom /*0C0000EA:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedResult::set_Location

  .method /*0600006D*/ family hidebysig virtual 
          instance void  ConfigureResponseHeaders(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ context) cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CreatedResult::ConfigureResponseHeaders

  .property /*17000011*/ instance callconv(8) string
          Location()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.CreatedResult/*02000018*/::get_Location() /* 0600006B */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.CreatedResult/*02000018*/::set_Location(string) /* 0600006C */
  } // end of property CreatedResult::Location
} // end of class Microsoft.AspNetCore.Http.Result.CreatedResult

.class /*02000019*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.FileContentResult
       extends Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C00006E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] fileContents,
                               string contentType) cil managed
  // SIG: 20 02 01 1D 05 0E
  {
    .param [2]/*080000D0*/ 
    .custom /*0C000143:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileContentResult::.ctor

  .method /*0600006F*/ public hidebysig specialname 
          instance uint8[]  get_FileContents() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C0000EB:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileContentResult::get_FileContents

  .method /*06000070*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_FileContents(uint8[] 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 1D 05
  {
    .custom /*0C0000EC:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileContentResult::set_FileContents

  .method /*06000071*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileContentResult::ExecuteAsync

  .property /*17000012*/ instance callconv(8) uint8[]
          FileContents()
  {
    .get instance uint8[] Microsoft.AspNetCore.Http.Result.FileContentResult/*02000019*/::get_FileContents() /* 0600006F */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.FileContentResult/*02000019*/::set_FileContents(uint8[]) /* 06000070 */
  } // end of property FileContentResult::FileContents
} // end of class Microsoft.AspNetCore.Http.Result.FileContentResult

.class /*0200001A*/ private abstract auto ansi beforefieldinit Microsoft.AspNetCore.Http.Result.FileResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000070:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000034*/ abstract auto ansi sealed nested family beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C0000A4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .method /*060000EF*/ public hidebysig static 
            void  ExecutingFileResult(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                      class Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/ fileResult) cil managed
    // SIG: 00 02 01 12 80 99 12 68
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ExecutingFileResult

    .method /*060000F0*/ public hidebysig static 
            void  ExecutingFileResult(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                      class Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/ fileResult,
                                      string fileName) cil managed
    // SIG: 00 03 01 12 80 99 12 68 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ExecutingFileResult

  } // end of class Log

  .method /*06000072*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string contentType) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000EE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::.ctor

  .method /*06000073*/ public hidebysig specialname 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000EF:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::get_ContentType

  .method /*06000074*/ public hidebysig specialname 
          instance string  get_FileDownloadName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::get_FileDownloadName

  .method /*06000075*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_FileDownloadName(string 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 0E
  {
    .param [1]/*080000D4*/ 
    .custom /*0C00014A:0A00001D*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000044*/::.ctor() /* 0A00001D */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::set_FileDownloadName

  .method /*06000076*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> 
          get_LastModified() cil managed
  // SIG: 20 00 15 11 80 8D 01 11 80 91
  {
    .custom /*0C0000F0:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::get_LastModified

  .method /*06000077*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_LastModified(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 15 11 80 8D 01 11 80 91
  {
    .custom /*0C0000F1:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::set_LastModified

  .method /*06000078*/ public hidebysig specialname 
          instance class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ 
          get_EntityTag() cil managed
  // SIG: 20 00 12 80 95
  {
    .custom /*0C0000F2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F3:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::get_EntityTag

  .method /*06000079*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_EntityTag(class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 12 80 95
  {
    .custom /*0C0000F5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000F6:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::set_EntityTag

  .method /*0600007A*/ public hidebysig specialname 
          instance bool  get_EnableRangeProcessing() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C0000F8:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::get_EnableRangeProcessing

  .method /*0600007B*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_EnableRangeProcessing(bool 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 02
  {
    .custom /*0C0000FA:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileResult::set_EnableRangeProcessing

  .property /*17000013*/ instance callconv(8) string
          ContentType()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::get_ContentType() /* 06000073 */
  } // end of property FileResult::ContentType
  .property /*17000014*/ instance callconv(8) string
          FileDownloadName()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::get_FileDownloadName() /* 06000074 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::set_FileDownloadName(string) /* 06000075 */
  } // end of property FileResult::FileDownloadName
  .property /*17000015*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/>
          LastModified()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/> Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::get_LastModified() /* 06000076 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::set_LastModified(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/>) /* 06000077 */
  } // end of property FileResult::LastModified
  .property /*17000016*/ instance callconv(8) class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/
          EntityTag()
  {
    .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/ Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::get_EntityTag() /* 06000078 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::set_EntityTag(class [Microsoft.Net.Http.Headers/*23000003*/]Microsoft.Net.Http.Headers.EntityTagHeaderValue/*01000025*/) /* 06000079 */
  } // end of property FileResult::EntityTag
  .property /*17000017*/ instance callconv(8) bool
          EnableRangeProcessing()
  {
    .get instance bool Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::get_EnableRangeProcessing() /* 0600007A */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/::set_EnableRangeProcessing(bool) /* 0600007B */
  } // end of property FileResult::EnableRangeProcessing
} // end of class Microsoft.AspNetCore.Http.Result.FileResult

.class /*0200001B*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.FileStreamResult
       extends Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000075:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000035*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__4'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0000A6:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400002D*/ public int32 '<>1__state'
    .field /*0400002E*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*0400002F*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext
    .field /*04000030*/ public class Microsoft.AspNetCore.Http.Result.FileStreamResult/*0200001B*/ '<>4__this'
    .field /*04000031*/ private class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ '<>7__wrap1'
    .field /*04000032*/ private object '<>7__wrap2'
    .field /*04000033*/ private int32 '<>7__wrap3'
    .field /*04000034*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .field /*04000035*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ValueTaskAwaiter/*01000040*/ '<>u__2'
    .method /*060000F1*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__4'::MoveNext

    .method /*060000F2*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C000164:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__4'::SetStateMachine

  } // end of class '<ExecuteAsync>d__4'

  .method /*0600007C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ fileStream,
                               string contentType) cil managed
  // SIG: 20 02 01 12 7D 0E
  {
    .param [2]/*080000D9*/ 
    .custom /*0C00014E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileStreamResult::.ctor

  .method /*0600007D*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ 
          get_FileStream() cil managed
  // SIG: 20 00 12 7D
  {
    .custom /*0C0000FD:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileStreamResult::get_FileStream

  .method /*0600007E*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .custom /*0C0000FF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100001C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0A000014 */ = ( 01 00 44 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..DMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 52 65 73   // NetCore.Http.Res
                                                                                                                                                                                                                                        75 6C 74 2E 46 69 6C 65 53 74 72 65 61 6D 52 65   // ult.FileStreamRe
                                                                                                                                                                                                                                        73 75 6C 74 2B 3C 45 78 65 63 75 74 65 41 73 79   // sult+<ExecuteAsy
                                                                                                                                                                                                                                        6E 63 3E 64 5F 5F 34 00 00 )                      // nc>d__4..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileStreamResult::ExecuteAsync

  .property /*17000018*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/
          FileStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001F*/ Microsoft.AspNetCore.Http.Result.FileStreamResult/*0200001B*/::get_FileStream() /* 0600007D */
  } // end of property FileStreamResult::FileStream
} // end of class Microsoft.AspNetCore.Http.Result.FileStreamResult

.class /*0200001C*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.ForbidResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000078:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000036*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*060000F3*/ public hidebysig static 
            void  ForbidResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                        class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
    // SIG: 00 02 01 12 80 99 15 12 80 DD 01 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ForbidResultExecuting

  } // end of class Log

  .class /*02000037*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__14'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0000A9:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000036*/ public int32 '<>1__state'
    .field /*04000037*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000038*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext
    .field /*04000039*/ public class Microsoft.AspNetCore.Http.Result.ForbidResult/*0200001C*/ '<>4__this'
    .field /*0400003A*/ private int32 '<i>5__2'
    .field /*0400003B*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .method /*060000F4*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__14'::MoveNext

    .method /*060000F5*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C000165:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__14'::SetStateMachine

  } // end of class '<ExecuteAsync>d__14'

  .method /*0600007F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::.ctor

  .method /*06000080*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string authenticationScheme) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::.ctor

  .method /*06000081*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
  // SIG: 20 01 01 15 12 80 DD 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::.ctor

  .method /*06000082*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 01 01 12 80 D9
  {
    .custom /*0C000100:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::.ctor

  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string authenticationScheme,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 02 01 0E 12 80 D9
  {
    .param [2]/*080000DF*/ 
    .custom /*0C000152:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::.ctor

  .method /*06000084*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 02 01 15 12 80 DD 01 0E 12 80 D9
  {
    .param [2]/*080000E1*/ 
    .custom /*0C000157:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::.ctor

  .method /*06000085*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> 
          get_AuthenticationSchemes() cil managed
  // SIG: 20 00 15 12 80 DD 01 0E
  {
    .custom /*0C000102:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::get_AuthenticationSchemes

  .method /*06000086*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_AuthenticationSchemes(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 15 12 80 DD 01 0E
  {
    .custom /*0C000104:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::set_AuthenticationSchemes

  .method /*06000087*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 80 D9
  {
    .custom /*0C000106:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000107:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::get_Properties

  .method /*06000088*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_Properties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 12 80 D9
  {
    .custom /*0C000108:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000109:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::set_Properties

  .method /*06000089*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .custom /*0C00010A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100001C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0A000014 */ = ( 01 00 41 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..AMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 52 65 73   // NetCore.Http.Res
                                                                                                                                                                                                                                        75 6C 74 2E 46 6F 72 62 69 64 52 65 73 75 6C 74   // ult.ForbidResult
                                                                                                                                                                                                                                        2B 3C 45 78 65 63 75 74 65 41 73 79 6E 63 3E 64   // +<ExecuteAsync>d
                                                                                                                                                                                                                                        5F 5F 31 34 00 00 )                               // __14..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ForbidResult::ExecuteAsync

  .property /*17000019*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string>
          AuthenticationSchemes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> Microsoft.AspNetCore.Http.Result.ForbidResult/*0200001C*/::get_AuthenticationSchemes() /* 06000085 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.ForbidResult/*0200001C*/::set_AuthenticationSchemes(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string>) /* 06000086 */
  } // end of property ForbidResult::AuthenticationSchemes
  .property /*1700001A*/ instance callconv(8) class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/
          Properties()
  {
    .custom /*0C000072:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ Microsoft.AspNetCore.Http.Result.ForbidResult/*0200001C*/::get_Properties() /* 06000087 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.ForbidResult/*0200001C*/::set_Properties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/) /* 06000088 */
  } // end of property ForbidResult::Properties
} // end of class Microsoft.AspNetCore.Http.Result.ForbidResult

.class /*0200001D*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.JsonResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C00007A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000038*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*060000F6*/ public hidebysig static 
            void  JsonResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                      object 'value') cil managed
    // SIG: 00 02 01 12 80 99 1C
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::JsonResultExecuting

  } // end of class Log

  .method /*0600008A*/ public hidebysig specialname 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00010B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::get_ContentType

  .method /*0600008B*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_ContentType(string 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 0E
  {
    .custom /*0C00010C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::set_ContentType

  .method /*0600008C*/ public hidebysig specialname 
          instance class [System.Text.Json/*23000008*/]System.Text.Json.JsonSerializerOptions/*0100003A*/ 
          get_JsonSerializerOptions() cil managed
  // SIG: 20 00 12 80 E9
  {
    .custom /*0C00010D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::get_JsonSerializerOptions

  .method /*0600008D*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_JsonSerializerOptions(class [System.Text.Json/*23000008*/]System.Text.Json.JsonSerializerOptions/*0100003A*/ 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 12 80 E9
  {
    .custom /*0C00010F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::set_JsonSerializerOptions

  .method /*0600008E*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> 
          get_StatusCode() cil managed
  // SIG: 20 00 15 11 80 8D 01 08
  {
    .custom /*0C000110:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::get_StatusCode

  .method /*0600008F*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_StatusCode(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 15 11 80 8D 01 08
  {
    .custom /*0C000112:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::set_StatusCode

  .method /*06000090*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C000113:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::get_Value

  .method /*06000091*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_Value(object 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 1C
  {
    .custom /*0C000115:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::set_Value

  .method /*06000092*/ private hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          Microsoft.AspNetCore.Http.IResult.ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .override [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/::ExecuteAsync /*01000035::0A00001E*/ 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::Microsoft.AspNetCore.Http.IResult.ExecuteAsync

  .method /*06000093*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method JsonResult::.ctor

  .property /*1700001B*/ instance callconv(8) string
          ContentType()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.JsonResult/*0200001D*/::get_ContentType() /* 0600008A */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.JsonResult/*0200001D*/::set_ContentType(string) /* 0600008B */
  } // end of property JsonResult::ContentType
  .property /*1700001C*/ instance callconv(8) class [System.Text.Json/*23000008*/]System.Text.Json.JsonSerializerOptions/*0100003A*/
          JsonSerializerOptions()
  {
    .get instance class [System.Text.Json/*23000008*/]System.Text.Json.JsonSerializerOptions/*0100003A*/ Microsoft.AspNetCore.Http.Result.JsonResult/*0200001D*/::get_JsonSerializerOptions() /* 0600008C */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.JsonResult/*0200001D*/::set_JsonSerializerOptions(class [System.Text.Json/*23000008*/]System.Text.Json.JsonSerializerOptions/*0100003A*/) /* 0600008D */
  } // end of property JsonResult::JsonSerializerOptions
  .property /*1700001D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32>
          StatusCode()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> Microsoft.AspNetCore.Http.Result.JsonResult/*0200001D*/::get_StatusCode() /* 0600008E */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.JsonResult/*0200001D*/::set_StatusCode(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32>) /* 0600008F */
  } // end of property JsonResult::StatusCode
  .property /*1700001E*/ instance callconv(8) object
          Value()
  {
    .get instance object Microsoft.AspNetCore.Http.Result.JsonResult/*0200001D*/::get_Value() /* 06000090 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.JsonResult/*0200001D*/::set_Value(object) /* 06000091 */
  } // end of property JsonResult::Value
} // end of class Microsoft.AspNetCore.Http.Result.JsonResult

.class /*0200001E*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.LocalRedirectResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C00007C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000039*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*060000F7*/ public hidebysig static 
            void  LocalRedirectResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                               string destination) cil managed
    // SIG: 00 02 01 12 80 99 0E
    {
      .custom /*0C000166:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 01 00 00 00 02 00 00 00 3C 45 78 65 63 75   // ..........<Execu
                                                                                                                                                                                                           74 69 6E 67 20 4C 6F 63 61 6C 52 65 64 69 72 65   // ting LocalRedire
                                                                                                                                                                                                           63 74 52 65 73 75 6C 74 2C 20 72 65 64 69 72 65   // ctResult, redire
                                                                                                                                                                                                           63 74 69 6E 67 20 74 6F 20 7B 44 65 73 74 69 6E   // cting to {Destin
                                                                                                                                                                                                           61 74 69 6F 6E 7D 2E 01 00 54 0E 09 45 76 65 6E   // ation}...T..Even
                                                                                                                                                                                                           74 4E 61 6D 65 1C 4C 6F 63 61 6C 52 65 64 69 72   // tName.LocalRedir
                                                                                                                                                                                                           65 63 74 52 65 73 75 6C 74 45 78 65 63 75 74 69   // ectResultExecuti
                                                                                                                                                                                                           6E 67 )                                           // ng
      .custom /*0C000167:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::LocalRedirectResultExecuting

  } // end of class Log

  .method /*06000094*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string localUrl) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalRedirectResult::.ctor

  .method /*06000095*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string localUrl,
                               bool permanent) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalRedirectResult::.ctor

  .method /*06000096*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string localUrl,
                               bool permanent,
                               bool preserveMethod) cil managed
  // SIG: 20 03 01 0E 02 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalRedirectResult::.ctor

  .method /*06000097*/ public hidebysig specialname 
          instance bool  get_Permanent() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000118:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalRedirectResult::get_Permanent

  .method /*06000098*/ public hidebysig specialname 
          instance bool  get_PreserveMethod() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000119:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalRedirectResult::get_PreserveMethod

  .method /*06000099*/ public hidebysig specialname 
          instance string  get_Url() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00011B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalRedirectResult::get_Url

  .method /*0600009A*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LocalRedirectResult::ExecuteAsync

  .property /*1700001F*/ instance callconv(8) bool
          Permanent()
  {
    .get instance bool Microsoft.AspNetCore.Http.Result.LocalRedirectResult/*0200001E*/::get_Permanent() /* 06000097 */
  } // end of property LocalRedirectResult::Permanent
  .property /*17000020*/ instance callconv(8) bool
          PreserveMethod()
  {
    .get instance bool Microsoft.AspNetCore.Http.Result.LocalRedirectResult/*0200001E*/::get_PreserveMethod() /* 06000098 */
  } // end of property LocalRedirectResult::PreserveMethod
  .property /*17000021*/ instance callconv(8) string
          Url()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.LocalRedirectResult/*0200001E*/::get_Url() /* 06000099 */
  } // end of property LocalRedirectResult::Url
} // end of class Microsoft.AspNetCore.Http.Result.LocalRedirectResult

.class /*0200001F*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Result.NoContentResult
       extends Microsoft.AspNetCore.Http.Result.StatusCodeResult/*02000028*/
{
  .method /*0600009B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NoContentResult::.ctor

} // end of class Microsoft.AspNetCore.Http.Result.NoContentResult

.class /*02000020*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.NotFoundObjectResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .method /*0600009C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00011C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NotFoundObjectResult::.ctor

} // end of class Microsoft.AspNetCore.Http.Result.NotFoundObjectResult

.class /*02000021*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Result.ObjectResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003A*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*060000F8*/ public hidebysig static 
            void  ObjectResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                        object 'value',
                                        valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> statusCode) cil managed
    // SIG: 00 03 01 12 80 99 1C 15 11 80 8D 01 08
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::ObjectResultExecuting

  } // end of class Log

  .method /*0600009D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::.ctor

  .method /*0600009E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'value',
                               valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> statusCode) cil managed
  // SIG: 20 02 01 1C 15 11 80 8D 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::.ctor

  .method /*0600009F*/ public hidebysig specialname 
          instance object  get_Value() cil managed
  // SIG: 20 00 1C
  {
    .custom /*0C00011D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::get_Value

  .method /*060000A0*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> 
          get_StatusCode() cil managed
  // SIG: 20 00 15 11 80 8D 01 08
  {
    .custom /*0C00011E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::get_StatusCode

  .method /*060000A1*/ public hidebysig specialname 
          instance void  set_StatusCode(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> 'value') cil managed
  // SIG: 20 01 01 15 11 80 8D 01 08
  {
    .custom /*0C00011F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::set_StatusCode

  .method /*060000A2*/ public hidebysig specialname 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000120:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::get_ContentType

  .method /*060000A3*/ public hidebysig specialname 
          instance void  set_ContentType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000121:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::set_ContentType

  .method /*060000A4*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .custom /*0C000122:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::ExecuteAsync

  .method /*060000A5*/ family hidebysig newslot virtual 
          instance void  OnFormatting(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C000123:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::OnFormatting

  .method /*060000A6*/ family hidebysig newslot virtual 
          instance void  ConfigureResponseHeaders(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C000124:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ObjectResult::ConfigureResponseHeaders

  .property /*17000022*/ instance callconv(8) object
          Value()
  {
    .get instance object Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/::get_Value() /* 0600009F */
  } // end of property ObjectResult::Value
  .property /*17000023*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32>
          StatusCode()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32> Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/::get_StatusCode() /* 060000A0 */
    .set instance void Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/::set_StatusCode(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*01000023*/<int32>) /* 060000A1 */
  } // end of property ObjectResult::StatusCode
  .property /*17000024*/ instance callconv(8) string
          ContentType()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/::get_ContentType() /* 060000A2 */
    .set instance void Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/::set_ContentType(string) /* 060000A3 */
  } // end of property ObjectResult::ContentType
} // end of class Microsoft.AspNetCore.Http.Result.ObjectResult

.class /*02000022*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.OkObjectResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .method /*060000A7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'value') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C000125:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method OkObjectResult::.ctor

} // end of class Microsoft.AspNetCore.Http.Result.OkObjectResult

.class /*02000023*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.PhysicalFileResult
       extends Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000080:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003B*/ sequential ansi sealed nested assembly beforefieldinit FileInfoWrapper
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
  {
    .custom /*0C0000AF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C0000B0:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*01000022*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    .field /*0400003C*/ private initonly bool '<Exists>k__BackingField'
    .custom /*0C0000B2:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400003D*/ private initonly int64 '<Length>k__BackingField'
    .custom /*0C0000B6:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400003E*/ private initonly valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/ '<LastWriteTimeUtc>k__BackingField'
    .custom /*0C0000B9:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .method /*060000F9*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(string path) cil managed
    // SIG: 20 01 01 0E
    {
      .custom /*0C000168:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileInfoWrapper::.ctor

    .method /*060000FA*/ public hidebysig specialname 
            instance bool  get_Exists() cil managed
    // SIG: 20 00 02
    {
      .custom /*0C000169:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileInfoWrapper::get_Exists

    .method /*060000FB*/ public hidebysig specialname 
            instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
            set_Exists(bool 'value') cil managed
    // SIG: 20 01 1F 80 C5 01 02
    {
      .custom /*0C00016A:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileInfoWrapper::set_Exists

    .method /*060000FC*/ public hidebysig specialname 
            instance int64  get_Length() cil managed
    // SIG: 20 00 0A
    {
      .custom /*0C00016B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileInfoWrapper::get_Length

    .method /*060000FD*/ public hidebysig specialname 
            instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
            set_Length(int64 'value') cil managed
    // SIG: 20 01 1F 80 C5 01 0A
    {
      .custom /*0C00016D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileInfoWrapper::set_Length

    .method /*060000FE*/ public hidebysig specialname 
            instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/ 
            get_LastWriteTimeUtc() cil managed
    // SIG: 20 00 11 80 91
    {
      .custom /*0C00016E:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileInfoWrapper::get_LastWriteTimeUtc

    .method /*060000FF*/ public hidebysig specialname 
            instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
            set_LastWriteTimeUtc(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/ 'value') cil managed
    // SIG: 20 01 1F 80 C5 01 11 80 91
    {
      .custom /*0C00016F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method FileInfoWrapper::set_LastWriteTimeUtc

    .property /*17000036*/ instance callconv(8) bool
            Exists()
    {
      .get instance bool Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/::get_Exists() /* 060000FA */
      .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/::set_Exists(bool) /* 060000FB */
    } // end of property FileInfoWrapper::Exists
    .property /*17000037*/ instance callconv(8) int64
            Length()
    {
      .get instance int64 Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/::get_Length() /* 060000FC */
      .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/::set_Length(int64) /* 060000FD */
    } // end of property FileInfoWrapper::Length
    .property /*17000038*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/
            LastWriteTimeUtc()
    {
      .get instance valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/ Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/::get_LastWriteTimeUtc() /* 060000FE */
      .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/::set_LastWriteTimeUtc(valuetype [System.Runtime/*23000001*/]System.DateTimeOffset/*01000024*/) /* 060000FF */
    } // end of property FileInfoWrapper::LastWriteTimeUtc
  } // end of class FileInfoWrapper

  .class /*0200003C*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C0000B3:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*0400003F*/ public static initonly class Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//'<>c'/*0200003C*/ '<>9'
    .field /*04000040*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<string,valuetype Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/> '<>9__0_0'
    .method /*06000100*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*06000101*/ assembly hidebysig 
            instance valuetype Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/ 
            '<.ctor>b__0_0'(string path) cil managed
    // SIG: 20 01 11 80 EC 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<.ctor>b__0_0'

  } // end of class '<>c'

  .method /*060000A8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               string contentType) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*080000FC*/ 
    .custom /*0C00016C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PhysicalFileResult::.ctor

  .method /*060000A9*/ public hidebysig specialname 
          instance string  get_FileName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000126:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PhysicalFileResult::get_FileName

  .method /*060000AA*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<string,valuetype Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/> 
          get_GetFileInfoWrapper() cil managed
  // SIG: 20 00 15 12 69 02 0E 11 80 EC
  {
    .custom /*0C000127:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PhysicalFileResult::get_GetFileInfoWrapper

  .method /*060000AB*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_GetFileInfoWrapper(class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<string,valuetype Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/> 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 15 12 69 02 0E 11 80 EC
  {
    .custom /*0C000128:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PhysicalFileResult::set_GetFileInfoWrapper

  .method /*060000AC*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PhysicalFileResult::ExecuteAsync

  .property /*17000025*/ instance callconv(8) string
          FileName()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*/::get_FileName() /* 060000A9 */
  } // end of property PhysicalFileResult::FileName
  .property /*17000026*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<string,valuetype Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/>
          GetFileInfoWrapper()
  {
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<string,valuetype Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/> Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*/::get_GetFileInfoWrapper() /* 060000AA */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*/::set_GetFileInfoWrapper(class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<string,valuetype Microsoft.AspNetCore.Http.Result.PhysicalFileResult/*02000023*//FileInfoWrapper/*0200003B*/>) /* 060000AB */
  } // end of property PhysicalFileResult::GetFileInfoWrapper
} // end of class Microsoft.AspNetCore.Http.Result.PhysicalFileResult

.class /*02000024*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.RedirectResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000083:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003D*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*06000102*/ public hidebysig static 
            void  RedirectResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                          string destination) cil managed
    // SIG: 00 02 01 12 80 99 0E
    {
      .custom /*0C000170:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 01 00 00 00 02 00 00 00 37 45 78 65 63 75   // ..........7Execu
                                                                                                                                                                                                           74 69 6E 67 20 52 65 64 69 72 65 63 74 52 65 73   // ting RedirectRes
                                                                                                                                                                                                           75 6C 74 2C 20 72 65 64 69 72 65 63 74 69 6E 67   // ult, redirecting
                                                                                                                                                                                                           20 74 6F 20 7B 44 65 73 74 69 6E 61 74 69 6F 6E   //  to {Destination
                                                                                                                                                                                                           7D 2E 01 00 54 0E 09 45 76 65 6E 74 4E 61 6D 65   // }...T..EventName
                                                                                                                                                                                                           17 52 65 64 69 72 65 63 74 52 65 73 75 6C 74 45   // .RedirectResultE
                                                                                                                                                                                                           78 65 63 75 74 69 6E 67 )                         // xecuting
      .custom /*0C000171:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::RedirectResultExecuting

  } // end of class Log

  .method /*060000AD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string url,
                               bool permanent,
                               bool preserveMethod) cil managed
  // SIG: 20 03 01 0E 02 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectResult::.ctor

  .method /*060000AE*/ public hidebysig specialname 
          instance bool  get_Permanent() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000129:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectResult::get_Permanent

  .method /*060000AF*/ public hidebysig specialname 
          instance bool  get_PreserveMethod() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00012B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectResult::get_PreserveMethod

  .method /*060000B0*/ public hidebysig specialname 
          instance string  get_Url() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00012C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectResult::get_Url

  .method /*060000B1*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectResult::ExecuteAsync

  .property /*17000027*/ instance callconv(8) bool
          Permanent()
  {
    .get instance bool Microsoft.AspNetCore.Http.Result.RedirectResult/*02000024*/::get_Permanent() /* 060000AE */
  } // end of property RedirectResult::Permanent
  .property /*17000028*/ instance callconv(8) bool
          PreserveMethod()
  {
    .get instance bool Microsoft.AspNetCore.Http.Result.RedirectResult/*02000024*/::get_PreserveMethod() /* 060000AF */
  } // end of property RedirectResult::PreserveMethod
  .property /*17000029*/ instance callconv(8) string
          Url()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.RedirectResult/*02000024*/::get_Url() /* 060000B0 */
  } // end of property RedirectResult::Url
} // end of class Microsoft.AspNetCore.Http.Result.RedirectResult

.class /*02000025*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.RedirectToRouteResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000084:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000085:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003E*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*06000103*/ public hidebysig static 
            void  RedirectToRouteResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                                 string destination,
                                                 string routeName) cil managed
    // SIG: 00 03 01 12 80 99 0E 0E
    {
      .custom /*0C000172:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 01 00 00 00 02 00 00 00 55 45 78 65 63 75   // ..........UExecu
                                                                                                                                                                                                           74 69 6E 67 20 52 65 64 69 72 65 63 74 54 6F 52   // ting RedirectToR
                                                                                                                                                                                                           6F 75 74 65 52 65 73 75 6C 74 2C 20 72 65 64 69   // outeResult, redi
                                                                                                                                                                                                           72 65 63 74 69 6E 67 20 74 6F 20 7B 44 65 73 74   // recting to {Dest
                                                                                                                                                                                                           69 6E 61 74 69 6F 6E 7D 20 66 72 6F 6D 20 72 6F   // ination} from ro
                                                                                                                                                                                                           75 74 65 20 7B 52 6F 75 74 65 4E 61 6D 65 7D 2E   // ute {RouteName}.
                                                                                                                                                                                                           01 00 54 0E 09 45 76 65 6E 74 4E 61 6D 65 1E 52   // ..T..EventName.R
                                                                                                                                                                                                           65 64 69 72 65 63 74 54 6F 52 6F 75 74 65 52 65   // edirectToRouteRe
                                                                                                                                                                                                           73 75 6C 74 45 78 65 63 75 74 69 6E 67 )          // sultExecuting
      .custom /*0C000173:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::RedirectToRouteResultExecuting

  } // end of class Log

  .method /*060000B2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object routeValues) cil managed
  // SIG: 20 01 01 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::.ctor

  .method /*060000B3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string routeName,
                               object routeValues) cil managed
  // SIG: 20 02 01 0E 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::.ctor

  .method /*060000B4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string routeName,
                               object routeValues,
                               bool permanent) cil managed
  // SIG: 20 03 01 0E 1C 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::.ctor

  .method /*060000B5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string routeName,
                               object routeValues,
                               bool permanent,
                               bool preserveMethod) cil managed
  // SIG: 20 04 01 0E 1C 02 02
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::.ctor

  .method /*060000B6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string routeName,
                               object routeValues,
                               string fragment) cil managed
  // SIG: 20 03 01 0E 1C 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::.ctor

  .method /*060000B7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string routeName,
                               object routeValues,
                               bool permanent,
                               string fragment) cil managed
  // SIG: 20 04 01 0E 1C 02 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::.ctor

  .method /*060000B8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string routeName,
                               object routeValues,
                               bool permanent,
                               bool preserveMethod,
                               string fragment) cil managed
  // SIG: 20 05 01 0E 1C 02 02 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::.ctor

  .method /*060000B9*/ public hidebysig specialname 
          instance string  get_RouteName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00012F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::get_RouteName

  .method /*060000BA*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/ 
          get_RouteValues() cil managed
  // SIG: 20 00 12 81 09
  {
    .custom /*0C000130:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::get_RouteValues

  .method /*060000BB*/ public hidebysig specialname 
          instance bool  get_Permanent() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000131:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::get_Permanent

  .method /*060000BC*/ public hidebysig specialname 
          instance bool  get_PreserveMethod() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000132:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::get_PreserveMethod

  .method /*060000BD*/ public hidebysig specialname 
          instance string  get_Fragment() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000133:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::get_Fragment

  .method /*060000BE*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .custom /*0C000134:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RedirectToRouteResult::ExecuteAsync

  .property /*1700002A*/ instance callconv(8) string
          RouteName()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.RedirectToRouteResult/*02000025*/::get_RouteName() /* 060000B9 */
  } // end of property RedirectToRouteResult::RouteName
  .property /*1700002B*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/
          RouteValues()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000042*/ Microsoft.AspNetCore.Http.Result.RedirectToRouteResult/*02000025*/::get_RouteValues() /* 060000BA */
  } // end of property RedirectToRouteResult::RouteValues
  .property /*1700002C*/ instance callconv(8) bool
          Permanent()
  {
    .get instance bool Microsoft.AspNetCore.Http.Result.RedirectToRouteResult/*02000025*/::get_Permanent() /* 060000BB */
  } // end of property RedirectToRouteResult::Permanent
  .property /*1700002D*/ instance callconv(8) bool
          PreserveMethod()
  {
    .get instance bool Microsoft.AspNetCore.Http.Result.RedirectToRouteResult/*02000025*/::get_PreserveMethod() /* 060000BC */
  } // end of property RedirectToRouteResult::PreserveMethod
  .property /*1700002E*/ instance callconv(8) string
          Fragment()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.RedirectToRouteResult/*02000025*/::get_Fragment() /* 060000BD */
  } // end of property RedirectToRouteResult::Fragment
} // end of class Microsoft.AspNetCore.Http.Result.RedirectToRouteResult

.class /*02000026*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.SignInResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000088:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200003F*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*06000104*/ public hidebysig static 
            void  SignInResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                        string scheme,
                                        class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ principal) cil managed
    // SIG: 00 03 01 12 80 99 0E 12 80 E1
    {
      .custom /*0C000174:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 01 00 00 00 02 00 00 00 66 45 78 65 63 75   // ..........fExecu
                                                                                                                                                                                                           74 69 6E 67 20 53 69 67 6E 49 6E 52 65 73 75 6C   // ting SignInResul
                                                                                                                                                                                                           74 20 77 69 74 68 20 61 75 74 68 65 6E 74 69 63   // t with authentic
                                                                                                                                                                                                           61 74 69 6F 6E 20 73 63 68 65 6D 65 20 28 7B 53   // ation scheme ({S
                                                                                                                                                                                                           63 68 65 6D 65 7D 29 20 61 6E 64 20 74 68 65 20   // cheme}) and the 
                                                                                                                                                                                                           66 6F 6C 6C 6F 77 69 6E 67 20 70 72 69 6E 63 69   // following princi
                                                                                                                                                                                                           70 61 6C 3A 20 7B 50 72 69 6E 63 69 70 61 6C 7D   // pal: {Principal}
                                                                                                                                                                                                           2E 01 00 54 0E 09 45 76 65 6E 74 4E 61 6D 65 15   // ...T..EventName.
                                                                                                                                                                                                           53 69 67 6E 49 6E 52 65 73 75 6C 74 45 78 65 63   // SignInResultExec
                                                                                                                                                                                                           75 74 69 6E 67 )                                  // uting
      .custom /*0C000175:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::SignInResultExecuting

  } // end of class Log

  .method /*060000BF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ principal) cil managed
  // SIG: 20 01 01 12 80 E1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::.ctor

  .method /*060000C0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string authenticationScheme,
                               class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ principal) cil managed
  // SIG: 20 02 01 0E 12 80 E1
  {
    .param [1]/*0800011B*/ 
    .custom /*0C000179:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::.ctor

  .method /*060000C1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ principal,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 02 01 12 80 E1 12 80 D9
  {
    .param [2]/*0800011E*/ 
    .custom /*0C00017A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::.ctor

  .method /*060000C2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string authenticationScheme,
                               class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ principal,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 03 01 0E 12 80 E1 12 80 D9
  {
    .custom /*0C000135:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000120*/ 
    .custom /*0C00017B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::.ctor

  .method /*060000C3*/ public hidebysig specialname 
          instance string  get_AuthenticationScheme() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000136:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000137:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::get_AuthenticationScheme

  .method /*060000C4*/ public hidebysig specialname 
          instance void  set_AuthenticationScheme(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000138:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000139:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::set_AuthenticationScheme

  .method /*060000C5*/ public hidebysig specialname 
          instance class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ 
          get_Principal() cil managed
  // SIG: 20 00 12 80 E1
  {
    .custom /*0C00013A:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::get_Principal

  .method /*060000C6*/ public hidebysig specialname 
          instance void  set_Principal(class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ 'value') cil managed
  // SIG: 20 01 01 12 80 E1
  {
    .custom /*0C00013B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::set_Principal

  .method /*060000C7*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 80 D9
  {
    .custom /*0C00013C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00013D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::get_Properties

  .method /*060000C8*/ public hidebysig specialname 
          instance void  set_Properties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D9
  {
    .custom /*0C00013E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00013F:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::set_Properties

  .method /*060000C9*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignInResult::ExecuteAsync

  .property /*1700002F*/ instance callconv(8) string
          AuthenticationScheme()
  {
    .custom /*0C00009D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.Http.Result.SignInResult/*02000026*/::get_AuthenticationScheme() /* 060000C3 */
    .set instance void Microsoft.AspNetCore.Http.Result.SignInResult/*02000026*/::set_AuthenticationScheme(string) /* 060000C4 */
  } // end of property SignInResult::AuthenticationScheme
  .property /*17000030*/ instance callconv(8) class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/
          Principal()
  {
    .get instance class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ Microsoft.AspNetCore.Http.Result.SignInResult/*02000026*/::get_Principal() /* 060000C5 */
    .set instance void Microsoft.AspNetCore.Http.Result.SignInResult/*02000026*/::set_Principal(class [System.Security.Claims/*23000007*/]System.Security.Claims.ClaimsPrincipal/*01000038*/) /* 060000C6 */
  } // end of property SignInResult::Principal
  .property /*17000031*/ instance callconv(8) class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/
          Properties()
  {
    .custom /*0C00009F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ Microsoft.AspNetCore.Http.Result.SignInResult/*02000026*/::get_Properties() /* 060000C7 */
    .set instance void Microsoft.AspNetCore.Http.Result.SignInResult/*02000026*/::set_Properties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/) /* 060000C8 */
  } // end of property SignInResult::Properties
} // end of class Microsoft.AspNetCore.Http.Result.SignInResult

.class /*02000027*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.SignOutResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C00008C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000040*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*06000105*/ public hidebysig static 
            void  SignOutResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                         class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
    // SIG: 00 02 01 12 80 99 15 12 80 DD 01 0E
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::SignOutResultExecuting

  } // end of class Log

  .class /*02000041*/ auto ansi sealed nested private beforefieldinit '<ExecuteAsync>d__14'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002C*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/
  {
    .custom /*0C0000BE:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    .field /*04000041*/ public int32 '<>1__state'
    .field /*04000042*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*0100002E*/ '<>t__builder'
    .field /*04000043*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext
    .field /*04000044*/ public class Microsoft.AspNetCore.Http.Result.SignOutResult/*02000027*/ '<>4__this'
    .field /*04000045*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter/*0100002F*/ '<>u__1'
    .field /*04000046*/ private int32 '<i>5__2'
    .method /*06000106*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::MoveNext /*0100002D::0A000018*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__14'::MoveNext

    .method /*06000107*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 B5
    {
      .custom /*0C000176:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000030*/::.ctor() /* 0A00001A */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*0100002D*/::SetStateMachine /*0100002D::0A000019*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ExecuteAsync>d__14'::SetStateMachine

  } // end of class '<ExecuteAsync>d__14'

  .method /*060000CA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::.ctor

  .method /*060000CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 01 01 12 80 D9
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::.ctor

  .method /*060000CC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string authenticationScheme) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::.ctor

  .method /*060000CD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes) cil managed
  // SIG: 20 01 01 15 12 80 DD 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::.ctor

  .method /*060000CE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string authenticationScheme,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 02 01 0E 12 80 D9
  {
    .param [2]/*0800012A*/ 
    .custom /*0C00017C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::.ctor

  .method /*060000CF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> authenticationSchemes,
                               class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ properties) cil managed
  // SIG: 20 02 01 15 12 80 DD 01 0E 12 80 D9
  {
    .param [2]/*0800012C*/ 
    .custom /*0C00017D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::.ctor

  .method /*060000D0*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> 
          get_AuthenticationSchemes() cil managed
  // SIG: 20 00 15 12 80 DD 01 0E
  {
    .custom /*0C000142:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::get_AuthenticationSchemes

  .method /*060000D1*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_AuthenticationSchemes(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 15 12 80 DD 01 0E
  {
    .custom /*0C000144:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::set_AuthenticationSchemes

  .method /*060000D2*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ 
          get_Properties() cil managed
  // SIG: 20 00 12 80 D9
  {
    .custom /*0C000145:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000146:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::get_Properties

  .method /*060000D3*/ public hidebysig specialname 
          instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) 
          set_Properties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ 'value') cil managed
  // SIG: 20 01 1F 80 C5 01 12 80 D9
  {
    .custom /*0C000147:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000148:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::set_Properties

  .method /*060000D4*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .custom /*0C000149:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100001C*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100001B*/) /* 0A000014 */ = ( 01 00 42 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..BMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 48 74 74 70 2E 52 65 73   // NetCore.Http.Res
                                                                                                                                                                                                                                        75 6C 74 2E 53 69 67 6E 4F 75 74 52 65 73 75 6C   // ult.SignOutResul
                                                                                                                                                                                                                                        74 2B 3C 45 78 65 63 75 74 65 41 73 79 6E 63 3E   // t+<ExecuteAsync>
                                                                                                                                                                                                                                        64 5F 5F 31 34 00 00 )                            // d__14..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignOutResult::ExecuteAsync

  .property /*17000032*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string>
          AuthenticationSchemes()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string> Microsoft.AspNetCore.Http.Result.SignOutResult/*02000027*/::get_AuthenticationSchemes() /* 060000D0 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.SignOutResult/*02000027*/::set_AuthenticationSchemes(class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000037*/<string>) /* 060000D1 */
  } // end of property SignOutResult::AuthenticationSchemes
  .property /*17000033*/ instance callconv(8) class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/
          Properties()
  {
    .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/ Microsoft.AspNetCore.Http.Result.SignOutResult/*02000027*/::get_Properties() /* 060000D2 */
    .set instance void modreq([System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsExternalInit/*01000031*/) Microsoft.AspNetCore.Http.Result.SignOutResult/*02000027*/::set_Properties(class [Microsoft.AspNetCore.Authentication.Abstractions/*23000006*/]Microsoft.AspNetCore.Authentication.AuthenticationProperties/*01000036*/) /* 060000D3 */
  } // end of property SignOutResult::Properties
} // end of class Microsoft.AspNetCore.Http.Result.SignOutResult

.class /*02000028*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Http.Result.StatusCodeResult
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .class /*02000042*/ abstract auto ansi sealed nested private beforefieldinit Log
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .method /*06000108*/ public hidebysig static 
            void  StatusCodeResultExecuting(class [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.ILogger/*01000026*/ logger,
                                            int32 statusCode) cil managed
    // SIG: 00 02 01 12 80 99 08
    {
      .custom /*0C000177:0A000016*/ instance void [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LoggerMessageAttribute/*0100002A*/::.ctor(int32,
                                                                                                                                                                                valuetype [Microsoft.Extensions.Logging.Abstractions/*23000004*/]Microsoft.Extensions.Logging.LogLevel/*01000029*/,
                                                                                                                                                                                string) /* 0A000016 */ = ( 01 00 01 00 00 00 02 00 00 00 42 45 78 65 63 75   // ..........BExecu
                                                                                                                                                                                                           74 69 6E 67 20 53 74 61 74 75 73 43 6F 64 65 52   // ting StatusCodeR
                                                                                                                                                                                                           65 73 75 6C 74 2C 20 73 65 74 74 69 6E 67 20 48   // esult, setting H
                                                                                                                                                                                                           54 54 50 20 73 74 61 74 75 73 20 63 6F 64 65 20   // TTP status code 
                                                                                                                                                                                                           7B 53 74 61 74 75 73 43 6F 64 65 7D 2E 01 00 54   // {StatusCode}...T
                                                                                                                                                                                                           0E 09 45 76 65 6E 74 4E 61 6D 65 19 53 74 61 74   // ..EventName.Stat
                                                                                                                                                                                                           75 73 43 6F 64 65 52 65 73 75 6C 74 45 78 65 63   // usCodeResultExec
                                                                                                                                                                                                           75 74 69 6E 67 )                                  // uting
      .custom /*0C000178:0A000017*/ instance void [System.Runtime/*23000001*/]System.CodeDom.Compiler.GeneratedCodeAttribute/*0100002B*/::.ctor(string,
                                                                                                                                                string) /* 0A000017 */ = ( 01 00 27 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..'Microsoft.Ext
                                                                                                                                                                           65 6E 73 69 6F 6E 73 2E 4C 6F 67 67 69 6E 67 2E   // ensions.Logging.
                                                                                                                                                                           47 65 6E 65 72 61 74 6F 72 73 0A 36 2E 30 2E 36   // Generators.6.0.6
                                                                                                                                                                           2E 36 34 30 36 00 00 )                            // .6406..
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method Log::StatusCodeResultExecuting

  } // end of class Log

  .method /*060000D5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 statusCode) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StatusCodeResult::.ctor

  .method /*060000D6*/ public hidebysig specialname 
          instance int32  get_StatusCode() cil managed
  // SIG: 20 00 08
  {
    .custom /*0C00014B:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StatusCodeResult::get_StatusCode

  .method /*060000D7*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    .custom /*0C00014C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method StatusCodeResult::ExecuteAsync

  .property /*17000034*/ instance callconv(8) int32
          StatusCode()
  {
    .get instance int32 Microsoft.AspNetCore.Http.Result.StatusCodeResult/*02000028*/::get_StatusCode() /* 060000D6 */
  } // end of property StatusCodeResult::StatusCode
} // end of class Microsoft.AspNetCore.Http.Result.StatusCodeResult

.class /*02000029*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.UnauthorizedResult
       extends Microsoft.AspNetCore.Http.Result.StatusCodeResult/*02000028*/
{
  .method /*060000D8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnauthorizedResult::.ctor

} // end of class Microsoft.AspNetCore.Http.Result.UnauthorizedResult

.class /*0200002A*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.UnprocessableEntityObjectResult
       extends Microsoft.AspNetCore.Http.Result.ObjectResult/*02000021*/
{
  .method /*060000D9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'error') cil managed
  // SIG: 20 01 01 1C
  {
    .custom /*0C00014D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method UnprocessableEntityObjectResult::.ctor

} // end of class Microsoft.AspNetCore.Http.Result.UnprocessableEntityObjectResult

.class /*0200002B*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Http.Result.VirtualFileResult
       extends Microsoft.AspNetCore.Http.Result.FileResult/*0200001A*/
       implements [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
{
  .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000096:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.IResult/*01000035*/
  .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000DA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               string contentType) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000134*/ 
    .custom /*0C00017E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualFileResult::.ctor

  .method /*060000DB*/ public hidebysig specialname 
          instance string  get_FileName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualFileResult::get_FileName

  .method /*060000DC*/ public hidebysig specialname 
          instance void  set_FileName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00014F:0A00001F*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.MemberNotNullAttribute/*01000045*/::.ctor(string) /* 0A00001F */ = ( 01 00 09 5F 66 69 6C 65 4E 61 6D 65 00 00 )       // ..._fileName..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualFileResult::set_FileName

  .method /*060000DD*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100001D*/ 
          ExecuteAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000002*/]Microsoft.AspNetCore.Http.HttpContext/*0100001E*/ httpContext) cil managed
  // SIG: 20 01 12 75 12 79
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualFileResult::ExecuteAsync

  .method /*060000DE*/ assembly hidebysig 
          instance class [Microsoft.Extensions.FileProviders.Abstractions/*2300000A*/]Microsoft.Extensions.FileProviders.IFileInfo/*01000046*/ 
          GetFileInformation(class [Microsoft.Extensions.FileProviders.Abstractions/*2300000A*/]Microsoft.Extensions.FileProviders.IFileProvider/*01000047*/ fileProvider) cil managed
  // SIG: 20 01 12 81 19 12 81 1D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualFileResult::GetFileInformation

  .property /*17000035*/ instance callconv(8) string
          FileName()
  {
    .get instance string Microsoft.AspNetCore.Http.Result.VirtualFileResult/*0200002B*/::get_FileName() /* 060000DB */
    .set instance void Microsoft.AspNetCore.Http.Result.VirtualFileResult/*0200002B*/::set_FileName(string) /* 060000DC */
  } // end of property VirtualFileResult::FileName
} // end of class Microsoft.AspNetCore.Http.Result.VirtualFileResult


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
