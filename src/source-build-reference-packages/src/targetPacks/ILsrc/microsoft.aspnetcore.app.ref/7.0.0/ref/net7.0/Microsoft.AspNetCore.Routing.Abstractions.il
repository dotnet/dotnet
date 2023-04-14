
//  Microsoft (R) .NET IL Disassembler.  Version 5.0.0


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
// Time-date stamp:            0xda7a89cc
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00002c00
// Size of init.data:              0x00000c00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00006a1a
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
// Checksum:                       0x00008a4e
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000069c8 [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000664] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00003c00 [0x00002880] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000069ac [0x0000001c] address [size] of Debug Directory:           
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
//              0x00002a20 Virtual Size
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
//              0x00000664 Virtual Size
//              0x00008000 Virtual Address
//              0x00000800 Size of Raw Data
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
//              0x00003a00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000a1c
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x00006a0a Import Name Table
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
// 0x00004054 [0x000028d8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000692c [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001094 Size
//              '#~' Name
//    Stream 2:
//              0x00001100 Offset
//              0x00000ca8 Size
//              '#Strings' Name
//    Stream 3:
//              0x00001da8 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00001dac Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00001dbc Offset
//              0x00000b1c Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000068901a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 25728
// PE header size       : 1024 (536 used)    ( 3.98%)
// PE additional info   : 12203             (47.43%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.28%)
// CLR meta-data size  : 10456              (40.64%)
// CLR additional info : 128                ( 0.50%)
// CLR method headers  : 62                 ( 0.24%)
// Managed code         : 124               ( 0.48%)
// Data                 : 3072              (11.94%)
// Unaccounted          : -1413             (-5.49%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 11264
//   .rsrc    - 2048
//   .reloc   - 512

// CLR meta-data size  : 10456
//   Module        -    1 (10 bytes)
//   TypeDef       -   24 (336 bytes)     6 interfaces, 0 explicit layout
//   TypeRef       -   49 (294 bytes)
//   MethodDef     -   77 (1078 bytes)    11 abstract, 0 native, 62 bodies
//   FieldDef      -   11 (66 bytes)      0 constant
//   MemberRef     -   23 (138 bytes)
//   ParamDef      -  107 (642 bytes)
//   Constant      -   18 (108 bytes)
//   CustomAttribute-  137 (822 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    7 (28 bytes)
//   Property      -   21 (126 bytes)
//   MethodSemantic-   30 (180 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    5 (100 bytes)
//   ExportedType  -    9 (126 bytes)
//   NestedClass   -    3 (12 bytes)
//   GenericParam  -    6 (48 bytes)
//   Strings       -  3239 bytes
//   Blobs         -  2844 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   209 bytes

// CLR additional info : 128

// CLR method headers : 62
//   Num.of method bodies  - 62
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 62

// Managed code : 124
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class RefSafetyRulesAttribute        (auto) (ansi) (sealed) 
// Class PropertyHelper                 (auto) (ansi) (sealed) 
// Interface IOutboundParameterTransformer  (public) (abstract) (auto) (ansi) 
// Interface IParameterPolicy               (public) (abstract) (auto) (ansi) 
// Interface IRouteConstraint               (public) (abstract) (auto) (ansi) 
// Interface IRouteHandler                  (public) (abstract) (auto) (ansi) 
// Interface IRouter                        (public) (abstract) (auto) (ansi) 
// Interface IRoutingFeature                (public) (abstract) (auto) (ansi) 
// Class LinkGenerator                  (public) (abstract) (auto) (ansi) 
// Class LinkOptions                    (public) (auto) (ansi) 
// Class RouteContext                   (public) (auto) (ansi) 
// Class RouteData                      (public) (auto) (ansi) 
// Class RouteDirection                 (public) (auto) (ansi) (sealed) 
// Class RoutingHttpContextExtensions   (public) (abstract) (auto) (ansi) (sealed) 
// Class VirtualPathContext             (public) (auto) (ansi) 
// Class VirtualPathData                (public) (auto) (ansi) 
// Class ByRefFunc`2                    (auto) (ansi) (sealed) (nested private) 
// Class MetadataUpdateHandler          (abstract) (auto) (ansi) (sealed) (nested assembly) 
// Class RouteDataSnapshot              (sequential) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000002*/ System.Runtime.Loader
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000003*/ System.Collections.Concurrent
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000004*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 7:0:0:0
}
.assembly extern /*23000005*/ Microsoft.AspNetCore.Http.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 7:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Routing.Abstractions
{
  .custom /*0C000003:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000004:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000002*/::.ctor(int32) /* 0A000002 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000003*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000003*/::.ctor() /* 0A000003 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000004*//DebuggingModes/*01000005*/) /* 0A000004 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 7F 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ....Microsoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 4D 76 63 2E 52 6F 75   // pNetCore.Mvc.Rou
                                                                                                                                                                                    74 69 6E 67 2E 41 62 73 74 72 61 63 74 69 6F 6E   // ting.Abstraction
                                                                                                                                                                                    73 2E 54 65 73 74 73 2C 20 50 75 62 6C 69 63 4B   // s.Tests, PublicK
                                                                                                                                                                                    65 79 3D 30 30 32 34 30 30 30 30 30 34 38 30 30   // ey=0024000004800
                                                                                                                                                                                    30 30 30 39 34 30 30 30 30 30 30 30 36 30 32 30   // 0009400000006020
                                                                                                                                                                                    30 30 30 30 30 32 34 30 30 30 30 35 32 35 33 34   // 0000024000052534
                                                                                                                                                                                    31 33 31 30 30 30 34 30 30 30 30 30 31 30 30 30   // 1310004000001000
                                                                                                                                                                                    31 30 30 66 33 33 61 32 39 30 34 34 66 61 39 64   // 100f33a29044fa9d
                                                                                                                                                                                    37 34 30 63 39 62 33 32 31 33 61 39 33 65 35 37   // 740c9b3213a93e57
                                                                                                                                                                                    63 38 34 62 34 37 32 63 38 34 65 30 62 38 61 30   // c84b472c84e0b8a0
                                                                                                                                                                                    65 31 61 65 34 38 65 36 37 61 39 66 38 66 36 64   // e1ae48e67a9f8f6d
                                                                                                                                                                                    65 39 64 35 66 37 66 33 64 35 32 61 63 32 33 65   // e9d5f7f3d52ac23e
                                                                                                                                                                                    34 38 61 63 35 31 38 30 31 66 31 64 63 39 35 30   // 48ac51801f1dc950
                                                                                                                                                                                    61 62 65 39 30 31 64 61 33 34 64 32 61 39 65 33   // abe901da34d2a9e3
                                                                                                                                                                                    62 61 61 64 62 31 34 31 61 31 37 63 37 37 65 66   // baadb141a17c77ef
                                                                                                                                                                                    33 63 35 36 35 64 64 35 65 65 35 30 35 34 62 39   // 3c565dd5ee5054b9
                                                                                                                                                                                    31 63 66 36 33 62 62 33 63 36 61 62 38 33 66 37   // 1cf63bb3c6ab83f7
                                                                                                                                                                                    32 61 62 33 61 61 66 65 39 33 64 30 66 63 33 63   // 2ab3aafe93d0fc3c
                                                                                                                                                                                    32 33 34 38 62 37 36 34 66 61 66 62 30 62 31 63   // 2348b764fafb0b1c
                                                                                                                                                                                    30 37 33 33 64 65 35 31 34 35 39 61 65 61 62 34   // 0733de51459aeab4
                                                                                                                                                                                    36 35 38 30 33 38 34 62 66 39 64 37 34 63 34 65   // 6580384bf9d74c4e
                                                                                                                                                                                    32 38 31 36 34 62 37 63 64 65 32 34 37 66 38 39   // 28164b7cde247f89
                                                                                                                                                                                    31 62 61 30 37 38 39 31 63 39 64 38 37 32 61 64   // 1ba07891c9d872ad
                                                                                                                                                                                    32 62 62 00 00 )                                  // 2bb..
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 64 44 79 6E 61 6D 69 63 50 72 6F 78 79   // ...dDynamicProxy
                                                                                                                                                                                    47 65 6E 41 73 73 65 6D 62 6C 79 32 2C 20 50 75   // GenAssembly2, Pu
                                                                                                                                                                                    62 6C 69 63 4B 65 79 3D 30 30 32 34 30 30 30 30   // blicKey=00240000
                                                                                                                                                                                    30 34 38 30 30 30 30 30 39 34 30 30 30 30 30 30   // 0480000094000000
                                                                                                                                                                                    30 36 30 32 30 30 30 30 30 30 32 34 30 30 30 30   // 0602000000240000
                                                                                                                                                                                    35 32 35 33 34 31 33 31 30 30 30 34 30 30 30 30   // 5253413100040000
                                                                                                                                                                                    30 31 30 30 30 31 30 30 63 35 34 37 63 61 63 33   // 01000100c547cac3
                                                                                                                                                                                    37 61 62 64 39 39 63 38 64 62 32 32 35 65 66 32   // 7abd99c8db225ef2
                                                                                                                                                                                    66 36 63 38 61 33 36 30 32 66 33 62 33 36 30 36   // f6c8a3602f3b3606
                                                                                                                                                                                    63 63 39 38 39 31 36 30 35 64 30 32 62 61 61 35   // cc9891605d02baa5
                                                                                                                                                                                    36 31 30 34 66 34 63 66 63 30 37 33 34 61 61 33   // 6104f4cfc0734aa3
                                                                                                                                                                                    39 62 39 33 62 66 37 38 35 32 66 37 64 39 32 36   // 9b93bf7852f7d926
                                                                                                                                                                                    36 36 35 34 37 35 33 63 63 32 39 37 65 37 64 32   // 6654753cc297e7d2
                                                                                                                                                                                    65 64 66 65 30 62 61 63 31 63 64 63 66 39 66 37   // edfe0bac1cdcf9f7
                                                                                                                                                                                    31 37 32 34 31 35 35 30 65 30 61 37 62 31 39 31   // 17241550e0a7b191
                                                                                                                                                                                    31 39 35 62 37 36 36 37 62 62 34 66 36 34 62 63   // 195b7667bb4f64bc
                                                                                                                                                                                    62 38 65 32 31 32 31 33 38 30 66 64 31 64 39 64   // b8e2121380fd1d9d
                                                                                                                                                                                    34 36 61 64 32 64 39 32 64 32 64 31 35 36 30 35   // 46ad2d92d2d15605
                                                                                                                                                                                    30 39 33 39 32 34 63 63 65 61 66 37 34 63 34 38   // 093924cceaf74c48
                                                                                                                                                                                    36 31 65 66 66 36 32 61 62 66 36 39 62 39 32 39   // 61eff62abf69b929
                                                                                                                                                                                    31 65 64 30 61 33 34 30 65 31 31 33 62 65 31 31   // 1ed0a340e113be11
                                                                                                                                                                                    65 36 61 37 64 33 31 31 33 65 39 32 34 38 34 63   // e6a7d3113e92484c
                                                                                                                                                                                    66 37 30 34 35 63 63 37 00 00 )                   // f7045cc7..
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime.Loader/*23000002*/]System.Reflection.Metadata.MetadataUpdateHandlerAttribute/*01000009*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*01000008*/) /* 0A000007 */ = ( 01 00 42 4D 69 63 72 6F 73 6F 66 74 2E 45 78 74   // ..BMicrosoft.Ext
                                                                                                                                                                                                                                            65 6E 73 69 6F 6E 73 2E 49 6E 74 65 72 6E 61 6C   // ensions.Internal
                                                                                                                                                                                                                                            2E 50 72 6F 70 65 72 74 79 48 65 6C 70 65 72 2B   // .PropertyHelper+
                                                                                                                                                                                                                                            4D 65 74 61 64 61 74 61 55 70 64 61 74 65 48 61   // MetadataUpdateHa
                                                                                                                                                                                                                                            6E 64 6C 65 72 00 00 )                            // ndler..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*0100000A*/::.ctor(string) /* 0A000008 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 37 2E 30 01 00 54 0E 14   // ersion=v7.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 37 2E 30 )          // Name..NET 7.0
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0A 43 6F 6D 6D 69 74 48 61 73 68 28 62 62   // ...CommitHash(bb
                                                                                                                                                                    30 31 62 62 66 34 34 33 33 65 32 37 32 38 39 62   // 01bbf4433e27289b
                                                                                                                                                                    39 39 30 30 31 62 37 64 65 36 61 35 38 32 38 37   // 99001b7de6a58287
                                                                                                                                                                    39 64 31 38 33 35 00 00 )                         // 9d1835..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0F 53 6F 75 72 63 65 43 6F 6D 6D 69 74 55   // ...SourceCommitU
                                                                                                                                                                    72 6C 52 68 74 74 70 73 3A 2F 2F 67 69 74 68 75   // rlRhttps://githu
                                                                                                                                                                    62 2E 63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70   // b.com/dotnet/asp
                                                                                                                                                                    6E 65 74 63 6F 72 65 2F 74 72 65 65 2F 62 62 30   // netcore/tree/bb0
                                                                                                                                                                    31 62 62 66 34 34 33 33 65 32 37 32 38 39 62 39   // 1bbf4433e27289b9
                                                                                                                                                                    39 30 30 31 62 37 64 65 36 61 35 38 32 38 37 39   // 9001b7de6a582879
                                                                                                                                                                    64 31 38 33 35 00 00 )                            // d1835..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000B*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000C*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyConfigurationAttribute/*0100000D*/::.ctor(string) /* 0A00000B */ = ( 01 00 07 52 65 6C 65 61 73 65 00 00 )             // ...Release..
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000E*/::.ctor(string) /* 0A00000C */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000F*/::.ctor(string) /* 0A00000D */ = ( 01 00 80 C1 41 53 50 2E 4E 45 54 20 43 6F 72 65   // ....ASP.NET Core
                                                                                                                                                                       20 61 62 73 74 72 61 63 74 69 6F 6E 73 20 66 6F   //  abstractions fo
                                                                                                                                                                       72 20 72 6F 75 74 69 6E 67 20 72 65 71 75 65 73   // r routing reques
                                                                                                                                                                       74 73 20 74 6F 20 61 70 70 6C 69 63 61 74 69 6F   // ts to applicatio
                                                                                                                                                                       6E 20 6C 6F 67 69 63 20 61 6E 64 20 66 6F 72 20   // n logic and for 
                                                                                                                                                                       67 65 6E 65 72 61 74 69 6E 67 20 6C 69 6E 6B 73   // generating links
                                                                                                                                                                       2E 0D 0A 43 6F 6D 6D 6F 6E 6C 79 20 75 73 65 64   // ...Commonly used
                                                                                                                                                                       20 74 79 70 65 73 3A 0D 0A 4D 69 63 72 6F 73 6F   //  types:..Microso
                                                                                                                                                                       66 74 2E 41 73 70 4E 65 74 43 6F 72 65 2E 52 6F   // ft.AspNetCore.Ro
                                                                                                                                                                       75 74 69 6E 67 2E 49 52 6F 75 74 65 72 0D 0A 4D   // uting.IRouter..M
                                                                                                                                                                       69 63 72 6F 73 6F 66 74 2E 41 73 70 4E 65 74 43   // icrosoft.AspNetC
                                                                                                                                                                       6F 72 65 2E 52 6F 75 74 69 6E 67 2E 52 6F 75 74   // ore.Routing.Rout
                                                                                                                                                                       65 44 61 74 61 00 00 )                            // eData..
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*01000010*/::.ctor(string) /* 0A00000E */ = ( 01 00 0C 37 2E 30 2E 32 32 2E 35 31 38 31 39 00   // ...7.0.22.51819.
                                                                                                                                                                       00 ) 
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*01000011*/::.ctor(string) /* 0A00000F */ = ( 01 00 2E 37 2E 30 2E 30 2B 62 62 30 31 62 62 66   // ...7.0.0+bb01bbf
                                                                                                                                                                                34 34 33 33 65 32 37 32 38 39 62 39 39 30 30 31   // 4433e27289b99001
                                                                                                                                                                                62 37 64 65 36 61 35 38 32 38 37 39 64 31 38 33   // b7de6a582879d183
                                                                                                                                                                                35 00 00 )                                        // 5..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000012*/::.ctor(string) /* 0A000010 */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000017:0A000011*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000013*/::.ctor(string) /* 0A000011 */ = ( 01 00 29 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..)Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 52 6F 75 74 69 6E 67 2E   // NetCore.Routing.
                                                                                                                                                                 41 62 73 74 72 61 63 74 69 6F 6E 73 00 00 )       // Abstractions..
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
  .ver 7:0:0:0
}
.class extern /*27000001*/ forwarder Microsoft.AspNetCore.Http.Endpoint
{
  .assembly extern Microsoft.AspNetCore.Http.Abstractions /*23000005*/ 
}
.class extern /*27000002*/ forwarder Microsoft.AspNetCore.Http.EndpointMetadataCollection
{
  .assembly extern Microsoft.AspNetCore.Http.Abstractions /*23000005*/ 
}
.class extern /*27000003*/ Enumerator
{
  .class extern Microsoft.AspNetCore.Http.EndpointMetadataCollection /*27000002*/ 
}
.class extern /*27000004*/ forwarder Microsoft.AspNetCore.Http.Features.IEndpointFeature
{
  .assembly extern Microsoft.AspNetCore.Http.Abstractions /*23000005*/ 
}
.class extern /*27000005*/ forwarder Microsoft.AspNetCore.Http.Features.IRouteValuesFeature
{
  .assembly extern Microsoft.AspNetCore.Http.Abstractions /*23000005*/ 
}
.class extern /*27000006*/ forwarder Microsoft.AspNetCore.Routing.RouteValueDictionary
{
  .assembly extern Microsoft.AspNetCore.Http.Abstractions /*23000005*/ 
}
.class extern /*27000007*/ Enumerator
{
  .class extern Microsoft.AspNetCore.Routing.RouteValueDictionary /*27000006*/ 
}
.class extern /*27000008*/ PropertyStorage
{
  .class extern Microsoft.AspNetCore.Routing.RouteValueDictionary /*27000006*/ 
}
.class extern /*27000009*/ MetadataUpdateHandler
{
  .class extern Microsoft.AspNetCore.Routing.RouteValueDictionary /*27000006*/ 
}
.module Microsoft.AspNetCore.Routing.Abstractions.dll
// MVID: {4c990145-6154-4a98-bfa6-6fdba618adf2}
.custom /*0C000001:06000006*/ instance void System.Runtime.CompilerServices.RefSafetyRulesAttribute/*02000006*/::.ctor(int32) /* 06000006 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 01 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F156107C000


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
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
  .custom /*0C000027:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C000028:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000029:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.RefSafetyRulesAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000016*/
{
  .custom /*0C00002A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
  .custom /*0C00002B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00002C:0A000014*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000017*/) /* 0A000014 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000004*/ public initonly int32 Version
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 A_1) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RefSafetyRulesAttribute::.ctor

} // end of class System.Runtime.CompilerServices.RefSafetyRulesAttribute

.class /*02000007*/ private auto ansi sealed beforefieldinit Microsoft.Extensions.Internal.PropertyHelper
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00002D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000016*/ auto ansi sealed nested private ByRefFunc`2<TDeclaringType,TValue>
         extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000023*/
  {
    .method /*06000047*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(object 'object',
                                 native int 'method') runtime managed
    // SIG: 20 02 01 1C 18
    {
    } // end of method ByRefFunc`2::.ctor

    .method /*06000048*/ public hidebysig newslot virtual 
            instance !TValue  Invoke(!TDeclaringType& arg) runtime managed
    // SIG: 20 01 13 01 10 13 00
    {
    } // end of method ByRefFunc`2::Invoke

    .method /*06000049*/ public hidebysig newslot virtual 
            instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ 
            BeginInvoke(!TDeclaringType& arg,
                        class [System.Runtime/*23000001*/]System.AsyncCallback/*01000025*/ callback,
                        object 'object') runtime managed
    // SIG: 20 03 12 80 91 10 13 00 12 80 95 1C
    {
    } // end of method ByRefFunc`2::BeginInvoke

    .method /*0600004A*/ public hidebysig newslot virtual 
            instance !TValue  EndInvoke(!TDeclaringType& arg,
                                        class [System.Runtime/*23000001*/]System.IAsyncResult/*01000024*/ result) runtime managed
    // SIG: 20 02 13 01 10 13 00 12 80 91
    {
    } // end of method ByRefFunc`2::EndInvoke

  } // end of class ByRefFunc`2

  .class /*02000017*/ abstract auto ansi sealed nested assembly beforefieldinit MetadataUpdateHandler
         extends [System.Runtime/*23000001*/]System.Object/*01000019*/
  {
    .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .method /*0600004B*/ public hidebysig static 
            void  ClearCache(class [System.Runtime/*23000001*/]System.Type/*01000008*/[] _) cil managed
    // SIG: 00 01 01 1D 12 21
    {
      .param [1]/*08000067*/ 
      .custom /*0C000087:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method MetadataUpdateHandler::ClearCache

  } // end of class MetadataUpdateHandler

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001A*/ 'property') cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001A*/ 
          get_Property() cil managed
  // SIG: 20 00 12 69
  {
    .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::get_Property

  .method /*06000009*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000032:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::get_Name

  .method /*0600000A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Func`2/*0100001C*/<object,object> 
          get_ValueGetter() cil managed
  // SIG: 20 00 15 12 71 02 1C 1C
  {
    .custom /*0C000035:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1A 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 20 73 61 66 65 2E 00 00 )    // ot trim safe...
    .param [0]/*08000002*/ 
    .custom /*0C00001C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::get_ValueGetter

  .method /*0600000B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<object,object> 
          get_ValueSetter() cil managed
  // SIG: 20 00 15 12 75 02 1C 1C
  {
    .custom /*0C000037:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1A 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 20 73 61 66 65 2E 00 00 )    // ot trim safe...
    .param [0]/*08000003*/ 
    .custom /*0C000020:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::get_ValueSetter

  .method /*0600000C*/ public hidebysig instance object 
          GetValue(object 'instance') cil managed
  // SIG: 20 01 1C 1C
  {
    .custom /*0C000039:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1A 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 20 73 61 66 65 2E 00 00 )    // ot trim safe...
    .param [0]/*08000004*/ 
    .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::GetValue

  .method /*0600000D*/ public hidebysig instance void 
          SetValue(object 'instance',
                   object 'value') cil managed
  // SIG: 20 02 01 1C 1C
  {
    .custom /*0C00003C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1A 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 20 73 61 66 65 2E 00 00 )    // ot trim safe...
    .param [2]/*08000007*/ 
    .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::SetValue

  .method /*0600000E*/ public hidebysig static 
          class Microsoft.Extensions.Internal.PropertyHelper/*02000007*/[] 
          GetProperties(class [System.Runtime/*23000001*/]System.Type/*01000008*/ 'type') cil managed
  // SIG: 00 01 1D 12 1C 12 21
  {
    .custom /*0C00003E:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1A 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 20 73 61 66 65 2E 00 00 )    // ot trim safe...
    .param [1]/*08000008*/ 
    .custom /*0C000031:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001E*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::GetProperties

  .method /*0600000F*/ public hidebysig static 
          class Microsoft.Extensions.Internal.PropertyHelper/*02000007*/[] 
          GetVisibleProperties(class [System.Runtime/*23000001*/]System.Type/*01000008*/ 'type') cil managed
  // SIG: 00 01 1D 12 1C 12 21
  {
    .custom /*0C000042:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1A 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 20 73 61 66 65 2E 00 00 )    // ot trim safe...
    .param [1]/*08000009*/ 
    .custom /*0C000033:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001E*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::GetVisibleProperties

  .method /*06000010*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Func`2/*0100001C*/<object,object> 
          MakeFastPropertyGetter(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001A*/ propertyInfo) cil managed
  // SIG: 00 01 15 12 71 02 1C 1C 12 69
  {
    .custom /*0C000043:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1D 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 6D 65 72 20 73 61 66 65 2E   // ot trimmer safe.
                                                                                                                                                                                            00 00 ) 
    .param [0]/*0800000A*/ 
    .custom /*0C000036:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::MakeFastPropertyGetter

  .method /*06000011*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Func`2/*0100001C*/<object,object> 
          MakeNullSafeFastPropertyGetter(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001A*/ propertyInfo) cil managed
  // SIG: 00 01 15 12 71 02 1C 1C 12 69
  {
    .custom /*0C000047:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1D 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 6D 65 72 20 73 61 66 65 2E   // ot trimmer safe.
                                                                                                                                                                                            00 00 ) 
    .param [0]/*0800000C*/ 
    .custom /*0C00003B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::MakeNullSafeFastPropertyGetter

  .method /*06000012*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<object,object> 
          MakeFastPropertySetter(class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001A*/ propertyInfo) cil managed
  // SIG: 00 01 15 12 75 02 1C 1C 12 69
  {
    .custom /*0C00004B:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1D 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 6D 65 72 20 73 61 66 65 2E   // ot trimmer safe.
                                                                                                                                                                                            00 00 ) 
    .param [0]/*0800000E*/ 
    .custom /*0C000041:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::MakeFastPropertySetter

  .method /*06000013*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IDictionary`2/*01000020*/<string,object> 
          ObjectToDictionary(object 'value') cil managed
  // SIG: 00 01 15 12 80 81 02 0E 1C 1C
  {
    .custom /*0C00004C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00004D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 32 4D 65 74 68 6F 64 20 75 73 65 73 20 72   // ..2Method uses r
                                                                                                                                                                                            65 66 6C 65 63 74 69 6F 6E 20 74 6F 20 67 65 6E   // eflection to gen
                                                                                                                                                                                            65 72 61 74 65 20 74 68 65 20 64 69 63 74 69 6F   // erate the dictio
                                                                                                                                                                                            6E 61 72 79 2E 00 00 )                            // nary...
    .param [0]/*08000010*/ 
    .custom /*0C000046:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::ObjectToDictionary

  .method /*06000014*/ public hidebysig static 
          class Microsoft.Extensions.Internal.PropertyHelper/*02000007*/[] 
          GetVisibleProperties(class [System.Runtime/*23000001*/]System.Type/*01000008*/ 'type',
                               class [System.Collections.Concurrent/*23000003*/]System.Collections.Concurrent.ConcurrentDictionary`2/*01000021*/<class [System.Runtime/*23000001*/]System.Type/*01000008*/,class Microsoft.Extensions.Internal.PropertyHelper/*02000007*/[]> allPropertiesCache,
                               class [System.Collections.Concurrent/*23000003*/]System.Collections.Concurrent.ConcurrentDictionary`2/*01000021*/<class [System.Runtime/*23000001*/]System.Type/*01000008*/,class Microsoft.Extensions.Internal.PropertyHelper/*02000007*/[]> visiblePropertiesCache) cil managed
  // SIG: 00 03 1D 12 1C 12 21 15 12 80 85 02 12 21 1D 12 1C 15 12 80 85 02 12 21 1D 12 1C
  {
    .custom /*0C000052:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1A 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 20 73 61 66 65 2E 00 00 )    // ot trim safe...
    .param [2]/*08000013*/ 
    .custom /*0C000051:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    .param [3]/*08000014*/ 
    .custom /*0C000055:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::GetVisibleProperties

  .method /*06000015*/ public hidebysig static 
          class Microsoft.Extensions.Internal.PropertyHelper/*02000007*/[] 
          GetProperties(class [System.Runtime/*23000001*/]System.Type/*01000008*/ 'type',
                        class [System.Collections.Concurrent/*23000003*/]System.Collections.Concurrent.ConcurrentDictionary`2/*01000021*/<class [System.Runtime/*23000001*/]System.Type/*01000008*/,class Microsoft.Extensions.Internal.PropertyHelper/*02000007*/[]> cache) cil managed
  // SIG: 00 02 1D 12 1C 12 21 15 12 80 85 02 12 21 1D 12 1C
  {
    .custom /*0C000056:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100001B*/::.ctor(string) /* 0A000015 */ = ( 01 00 1A 54 68 69 73 20 41 50 49 20 69 73 20 6E   // ...This API is n
                                                                                                                                                                                            6F 74 20 74 72 69 6D 20 73 61 66 65 2E 00 00 )    // ot trim safe...
    .param [2]/*08000016*/ 
    .custom /*0C00005A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 04 00 00 00 02 01 01 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::GetProperties

  .method /*06000016*/ assembly hidebysig static 
          void  '<GetProperties>g__AddInterestingProperties|40_0'(class [System.Collections/*23000004*/]System.Collections.Generic.List`1/*01000022*/<class Microsoft.Extensions.Internal.PropertyHelper/*02000007*/> propertyHelpers,
                                                                  class [System.Runtime/*23000001*/]System.Type/*01000008*/ 'type') cil managed
  // SIG: 00 02 01 15 12 80 89 01 12 1C 12 21
  {
    .custom /*0C000059:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    .param [2]/*08000018*/ 
    .custom /*0C000060:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes/*0100001E*/) /* 0A000016 */ = ( 01 00 00 06 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PropertyHelper::'<GetProperties>g__AddInterestingProperties|40_0'

  .property /*17000001*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001A*/
          Property()
  {
    .get instance class [System.Runtime/*23000001*/]System.Reflection.PropertyInfo/*0100001A*/ Microsoft.Extensions.Internal.PropertyHelper/*02000007*/::get_Property() /* 06000008 */
  } // end of property PropertyHelper::Property
  .property /*17000002*/ instance callconv(8) string
          Name()
  {
    .get instance string Microsoft.Extensions.Internal.PropertyHelper/*02000007*/::get_Name() /* 06000009 */
  } // end of property PropertyHelper::Name
  .property /*17000003*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Func`2/*0100001C*/<object,object>
          ValueGetter()
  {
    .custom /*0C000021:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Func`2/*0100001C*/<object,object> Microsoft.Extensions.Internal.PropertyHelper/*02000007*/::get_ValueGetter() /* 0600000A */
  } // end of property PropertyHelper::ValueGetter
  .property /*17000004*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<object,object>
          ValueSetter()
  {
    .custom /*0C000026:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 03 00 00 00 01 01 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Action`2/*0100001D*/<object,object> Microsoft.Extensions.Internal.PropertyHelper/*02000007*/::get_ValueSetter() /* 0600000B */
  } // end of property PropertyHelper::ValueSetter
} // end of class Microsoft.Extensions.Internal.PropertyHelper

.class /*02000008*/ interface public abstract auto ansi Microsoft.AspNetCore.Routing.IOutboundParameterTransformer
       implements Microsoft.AspNetCore.Routing.IParameterPolicy/*02000009*/
{
  .method /*06000017*/ public hidebysig newslot abstract virtual 
          instance string  TransformOutbound(object 'value') cil managed
  // SIG: 20 01 0E 1C
  {
    .custom /*0C00005B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IOutboundParameterTransformer::TransformOutbound

} // end of class Microsoft.AspNetCore.Routing.IOutboundParameterTransformer

.class /*02000009*/ interface public abstract auto ansi Microsoft.AspNetCore.Routing.IParameterPolicy
{
} // end of class Microsoft.AspNetCore.Routing.IParameterPolicy

.class /*0200000A*/ interface public abstract auto ansi Microsoft.AspNetCore.Routing.IRouteConstraint
       implements Microsoft.AspNetCore.Routing.IParameterPolicy/*02000009*/
{
  .method /*06000018*/ public hidebysig newslot abstract virtual 
          instance bool  Match(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext,
                               class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/ route,
                               string routeKey,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values,
                               valuetype Microsoft.AspNetCore.Routing.RouteDirection/*02000012*/ routeDirection) cil managed
  // SIG: 20 05 02 12 80 99 12 30 0E 12 80 9D 11 48
  {
    .custom /*0C00005D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800001A*/ 
    .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*0800001B*/ 
    .custom /*0C000062:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IRouteConstraint::Match

} // end of class Microsoft.AspNetCore.Routing.IRouteConstraint

.class /*0200000B*/ interface public abstract auto ansi Microsoft.AspNetCore.Routing.IRouteHandler
{
  .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000019*/ public hidebysig newslot abstract virtual 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000028*/ 
          GetRequestHandler(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext,
                            class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ routeData) cil managed
  // SIG: 20 02 12 80 A1 12 80 99 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IRouteHandler::GetRequestHandler

} // end of class Microsoft.AspNetCore.Routing.IRouteHandler

.class /*0200000C*/ interface public abstract auto ansi Microsoft.AspNetCore.Routing.IRouter
{
  .custom /*0C00003A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600001A*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000029*/ 
          RouteAsync(class Microsoft.AspNetCore.Routing.RouteContext/*02000010*/ context) cil managed
  // SIG: 20 01 12 80 A5 12 40
  {
    // Method begins at RVA 0x0
  } // end of method IRouter::RouteAsync

  .method /*0600001B*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Routing.VirtualPathData/*02000015*/ 
          GetVirtualPath(class Microsoft.AspNetCore.Routing.VirtualPathContext/*02000014*/ context) cil managed
  // SIG: 20 01 12 54 12 50
  {
    .param [0]/*08000022*/ 
    .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IRouter::GetVirtualPath

} // end of class Microsoft.AspNetCore.Routing.IRouter

.class /*0200000D*/ interface public abstract auto ansi Microsoft.AspNetCore.Routing.IRoutingFeature
{
  .custom /*0C00003D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .method /*0600001C*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ 
          get_RouteData() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IRoutingFeature::get_RouteData

  .method /*0600001D*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_RouteData(class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IRoutingFeature::set_RouteData

  .property /*17000005*/ instance callconv(8) class Microsoft.AspNetCore.Routing.RouteData/*02000011*/
          RouteData()
  {
    .get instance class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ Microsoft.AspNetCore.Routing.IRoutingFeature/*0200000D*/::get_RouteData() /* 0600001C */
    .set instance void Microsoft.AspNetCore.Routing.IRoutingFeature/*0200000D*/::set_RouteData(class Microsoft.AspNetCore.Routing.RouteData/*02000011*/) /* 0600001D */
  } // end of property IRoutingFeature::RouteData
} // end of class Microsoft.AspNetCore.Routing.IRoutingFeature

.class /*0200000E*/ public abstract auto ansi beforefieldinit Microsoft.AspNetCore.Routing.LinkGenerator
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001E*/ public hidebysig newslot abstract virtual 
          instance string  GetPathByAddress<TAddress>(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext,
                                                      !!TAddress address,
                                                      class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values,
                                                      [opt] class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ ambientValues,
                                                      [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.PathString/*0100002B*/> pathBase,
                                                      [opt] valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.FragmentString/*0100002C*/ fragment,
                                                      [opt] class Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/ options) cil managed
  // SIG: 30 01 07 0E 12 80 99 1E 00 12 80 9D 12 80 9D 15 11 80 A9 01 11 80 AD 11 80 B1 12 3C
  {
    .param [1]/*08000025*/ 
    .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000026*/ 
    .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000027*/ 
    .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000028*/  = nullref
    .param [5]/*08000029*/  = nullref
    .param [6]/*0800002A*/  = nullref
    .param [7]/*0800002B*/  = nullref
    // Method begins at RVA 0x0
  } // end of method LinkGenerator::GetPathByAddress

  .method /*0600001F*/ public hidebysig newslot abstract virtual 
          instance string  GetPathByAddress<TAddress>(!!TAddress address,
                                                      class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values,
                                                      [opt] valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.PathString/*0100002B*/ pathBase,
                                                      [opt] valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.FragmentString/*0100002C*/ fragment,
                                                      [opt] class Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/ options) cil managed
  // SIG: 30 01 05 0E 1E 00 12 80 9D 11 80 AD 11 80 B1 12 3C
  {
    .param [1]/*0800002C*/ 
    .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800002D*/ 
    .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*0800002E*/  = nullref
    .param [4]/*0800002F*/  = nullref
    .param [5]/*08000030*/  = nullref
    // Method begins at RVA 0x0
  } // end of method LinkGenerator::GetPathByAddress

  .method /*06000020*/ public hidebysig newslot abstract virtual 
          instance string  GetUriByAddress<TAddress>(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext,
                                                     !!TAddress address,
                                                     class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values,
                                                     [opt] class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ ambientValues,
                                                     [opt] string scheme,
                                                     [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HostString/*0100002D*/> host,
                                                     [opt] valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.PathString/*0100002B*/> pathBase,
                                                     [opt] valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.FragmentString/*0100002C*/ fragment,
                                                     [opt] class Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/ options) cil managed
  // SIG: 30 01 09 0E 12 80 99 1E 00 12 80 9D 12 80 9D 0E 15 11 80 A9 01 11 80 B5 15 11 80 A9 01 11 80 AD
  //      11 80 B1 12 3C
  {
    .param [1]/*08000031*/ 
    .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000032*/ 
    .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000033*/ 
    .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000034*/  = nullref
    .param [5]/*08000035*/  = nullref
    .param [6]/*08000036*/  = nullref
    .param [7]/*08000037*/  = nullref
    .param [8]/*08000038*/  = nullref
    .param [9]/*08000039*/  = nullref
    // Method begins at RVA 0x0
  } // end of method LinkGenerator::GetUriByAddress

  .method /*06000021*/ public hidebysig newslot abstract virtual 
          instance string  GetUriByAddress<TAddress>(!!TAddress address,
                                                     class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values,
                                                     string scheme,
                                                     valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HostString/*0100002D*/ host,
                                                     [opt] valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.PathString/*0100002B*/ pathBase,
                                                     [opt] valuetype [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.FragmentString/*0100002C*/ fragment,
                                                     [opt] class Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/ options) cil managed
  // SIG: 30 01 07 0E 1E 00 12 80 9D 0E 11 80 B5 11 80 AD 11 80 B1 12 3C
  {
    .param [1]/*0800003A*/ 
    .custom /*0C00007A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800003B*/ 
    .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800003E*/  = nullref
    .param [6]/*0800003F*/  = nullref
    .param [7]/*08000040*/  = nullref
    // Method begins at RVA 0x0
  } // end of method LinkGenerator::GetUriByAddress

  .method /*06000022*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkGenerator::.ctor

} // end of class Microsoft.AspNetCore.Routing.LinkGenerator

.class /*0200000F*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Routing.LinkOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .method /*06000023*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> 
          get_LowercaseUrls() cil managed
  // SIG: 20 00 15 11 80 A9 01 02
  {
    .custom /*0C000064:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkOptions::get_LowercaseUrls

  .method /*06000024*/ public hidebysig specialname 
          instance void  set_LowercaseUrls(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 80 A9 01 02
  {
    .custom /*0C000065:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkOptions::set_LowercaseUrls

  .method /*06000025*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> 
          get_LowercaseQueryStrings() cil managed
  // SIG: 20 00 15 11 80 A9 01 02
  {
    .custom /*0C000066:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkOptions::get_LowercaseQueryStrings

  .method /*06000026*/ public hidebysig specialname 
          instance void  set_LowercaseQueryStrings(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 80 A9 01 02
  {
    .custom /*0C000068:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkOptions::set_LowercaseQueryStrings

  .method /*06000027*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> 
          get_AppendTrailingSlash() cil managed
  // SIG: 20 00 15 11 80 A9 01 02
  {
    .custom /*0C00006A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkOptions::get_AppendTrailingSlash

  .method /*06000028*/ public hidebysig specialname 
          instance void  set_AppendTrailingSlash(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> 'value') cil managed
  // SIG: 20 01 01 15 11 80 A9 01 02
  {
    .custom /*0C00006C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkOptions::set_AppendTrailingSlash

  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkOptions::.ctor

  .property /*17000006*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool>
          LowercaseUrls()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/::get_LowercaseUrls() /* 06000023 */
    .set instance void Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/::set_LowercaseUrls(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool>) /* 06000024 */
  } // end of property LinkOptions::LowercaseUrls
  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool>
          LowercaseQueryStrings()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/::get_LowercaseQueryStrings() /* 06000025 */
    .set instance void Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/::set_LowercaseQueryStrings(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool>) /* 06000026 */
  } // end of property LinkOptions::LowercaseQueryStrings
  .property /*17000008*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool>
          AppendTrailingSlash()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool> Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/::get_AppendTrailingSlash() /* 06000027 */
    .set instance void Microsoft.AspNetCore.Routing.LinkOptions/*0200000F*/::set_AppendTrailingSlash(valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002A*/<bool>) /* 06000028 */
  } // end of property LinkOptions::AppendTrailingSlash
} // end of class Microsoft.AspNetCore.Routing.LinkOptions

.class /*02000010*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Routing.RouteContext
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext) cil managed
  // SIG: 20 01 01 12 80 99
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteContext::.ctor

  .method /*0600002B*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000028*/ 
          get_Handler() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00006E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteContext::get_Handler

  .method /*0600002C*/ public hidebysig specialname 
          instance void  set_Handler(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000070:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteContext::set_Handler

  .method /*0600002D*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ 
          get_HttpContext() cil managed
  // SIG: 20 00 12 80 99
  {
    .custom /*0C000072:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteContext::get_HttpContext

  .method /*0600002E*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ 
          get_RouteData() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteContext::get_RouteData

  .method /*0600002F*/ public hidebysig specialname 
          instance void  set_RouteData(class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteContext::set_RouteData

  .property /*17000009*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000028*/
          Handler()
  {
    .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000028*/ Microsoft.AspNetCore.Routing.RouteContext/*02000010*/::get_Handler() /* 0600002B */
    .set instance void Microsoft.AspNetCore.Routing.RouteContext/*02000010*/::set_Handler(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.RequestDelegate/*01000028*/) /* 0600002C */
  } // end of property RouteContext::Handler
  .property /*1700000A*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/
          HttpContext()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ Microsoft.AspNetCore.Routing.RouteContext/*02000010*/::get_HttpContext() /* 0600002D */
  } // end of property RouteContext::HttpContext
  .property /*1700000B*/ instance callconv(8) class Microsoft.AspNetCore.Routing.RouteData/*02000011*/
          RouteData()
  {
    .get instance class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ Microsoft.AspNetCore.Routing.RouteContext/*02000010*/::get_RouteData() /* 0600002E */
    .set instance void Microsoft.AspNetCore.Routing.RouteContext/*02000010*/::set_RouteData(class Microsoft.AspNetCore.Routing.RouteData/*02000011*/) /* 0600002F */
  } // end of property RouteContext::RouteData
} // end of class Microsoft.AspNetCore.Routing.RouteContext

.class /*02000011*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Routing.RouteData
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000048:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000018*/ sequential ansi sealed nested public beforefieldinit RouteDataSnapshot
         extends [System.Runtime/*23000001*/]System.ValueType/*01000030*/
  {
    .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .custom /*0C00005F:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100002F*/::.ctor() /* 0A000017 */ = ( 01 00 00 00 ) 
    .field /*04000008*/ private initonly class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ _routeData
    .field /*04000009*/ private initonly class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ _dataTokens
    .field /*0400000A*/ private initonly class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100002E*/<class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/> _routers
    .field /*0400000B*/ private initonly class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ _values
    .method /*0600004C*/ public hidebysig specialname rtspecialname 
            instance void  .ctor(class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ routeData,
                                 class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ dataTokens,
                                 class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100002E*/<class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/> routers,
                                 class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values) cil managed
    // SIG: 20 04 01 12 44 12 80 9D 15 12 80 B9 01 12 30 12 80 9D
    {
      .custom /*0C000084:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
      .param [1]/*08000068*/ 
      .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
      .param [3]/*0800006A*/ 
      .custom /*0C000089:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method RouteDataSnapshot::.ctor

    .method /*0600004D*/ public hidebysig 
            instance void  Restore() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method RouteDataSnapshot::Restore

  } // end of class RouteDataSnapshot

  .method /*06000030*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteData::.ctor

  .method /*06000031*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ other) cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteData::.ctor

  .method /*06000032*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values) cil managed
  // SIG: 20 01 01 12 80 9D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteData::.ctor

  .method /*06000033*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ 
          get_DataTokens() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteData::get_DataTokens

  .method /*06000034*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100002E*/<class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/> 
          get_Routers() cil managed
  // SIG: 20 00 15 12 80 B9 01 12 30
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteData::get_Routers

  .method /*06000035*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ 
          get_Values() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteData::get_Values

  .method /*06000036*/ public hidebysig instance valuetype Microsoft.AspNetCore.Routing.RouteData/*02000011*//RouteDataSnapshot/*02000018*/ 
          PushState(class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/ router,
                    class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values,
                    class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ dataTokens) cil managed
  // SIG: 20 03 11 60 12 30 12 80 9D 12 80 9D
  {
    .custom /*0C000077:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RouteData::PushState

  .property /*1700000C*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/
          DataTokens()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ Microsoft.AspNetCore.Routing.RouteData/*02000011*/::get_DataTokens() /* 06000033 */
  } // end of property RouteData::DataTokens
  .property /*1700000D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100002E*/<class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/>
          Routers()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*0100002E*/<class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/> Microsoft.AspNetCore.Routing.RouteData/*02000011*/::get_Routers() /* 06000034 */
  } // end of property RouteData::Routers
  .property /*1700000E*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/
          Values()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ Microsoft.AspNetCore.Routing.RouteData/*02000011*/::get_Values() /* 06000035 */
  } // end of property RouteData::Values
} // end of class Microsoft.AspNetCore.Routing.RouteData

.class /*02000012*/ public auto ansi sealed Microsoft.AspNetCore.Routing.RouteDirection
       extends [System.Runtime/*23000001*/]System.Enum/*01000031*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype Microsoft.AspNetCore.Routing.RouteDirection/*02000012*/ IncomingRequest = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype Microsoft.AspNetCore.Routing.RouteDirection/*02000012*/ UrlGeneration = int32(0x00000001)
} // end of class Microsoft.AspNetCore.Routing.RouteDirection

.class /*02000013*/ public abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Routing.RoutingHttpContextExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C00004E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000050:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000037*/ public hidebysig static 
          class Microsoft.AspNetCore.Routing.RouteData/*02000011*/ 
          GetRouteData(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext) cil managed
  // SIG: 00 01 12 44 12 80 99
  {
    .custom /*0C000078:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RoutingHttpContextExtensions::GetRouteData

  .method /*06000038*/ public hidebysig static 
          object  GetRouteValue(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext,
                                string key) cil managed
  // SIG: 00 02 1C 12 80 99 0E
  {
    .custom /*0C000079:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [0]/*0800004D*/ 
    .custom /*0C000085:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RoutingHttpContextExtensions::GetRouteValue

} // end of class Microsoft.AspNetCore.Routing.RoutingHttpContextExtensions

.class /*02000014*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Routing.VirtualPathContext
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000053:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000039*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ ambientValues,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values) cil managed
  // SIG: 20 03 01 12 80 99 12 80 9D 12 80 9D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathContext::.ctor

  .method /*0600003A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ httpContext,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ ambientValues,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ values,
                               string routeName) cil managed
  // SIG: 20 04 01 12 80 99 12 80 9D 12 80 9D 0E
  {
    .param [4]/*08000056*/ 
    .custom /*0C000086:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathContext::.ctor

  .method /*0600003B*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ 
          get_AmbientValues() cil managed
  // SIG: 20 00 12 80 9D
  {
    .custom /*0C00007B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathContext::get_AmbientValues

  .method /*0600003C*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ 
          get_HttpContext() cil managed
  // SIG: 20 00 12 80 99
  {
    .custom /*0C00007D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathContext::get_HttpContext

  .method /*0600003D*/ public hidebysig specialname 
          instance string  get_RouteName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00007F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathContext::get_RouteName

  .method /*0600003E*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ 
          get_Values() cil managed
  // SIG: 20 00 12 80 9D
  {
    .custom /*0C000080:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathContext::get_Values

  .method /*0600003F*/ public hidebysig specialname 
          instance void  set_Values(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ 'value') cil managed
  // SIG: 20 01 01 12 80 9D
  {
    .custom /*0C000081:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathContext::set_Values

  .property /*1700000F*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/
          AmbientValues()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ Microsoft.AspNetCore.Routing.VirtualPathContext/*02000014*/::get_AmbientValues() /* 0600003B */
  } // end of property VirtualPathContext::AmbientValues
  .property /*17000010*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/
          HttpContext()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*01000026*/ Microsoft.AspNetCore.Routing.VirtualPathContext/*02000014*/::get_HttpContext() /* 0600003C */
  } // end of property VirtualPathContext::HttpContext
  .property /*17000011*/ instance callconv(8) string
          RouteName()
  {
    .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.Routing.VirtualPathContext/*02000014*/::get_RouteName() /* 0600003D */
  } // end of property VirtualPathContext::RouteName
  .property /*17000012*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/
          Values()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ Microsoft.AspNetCore.Routing.VirtualPathContext/*02000014*/::get_Values() /* 0600003E */
    .set instance void Microsoft.AspNetCore.Routing.VirtualPathContext/*02000014*/::set_Values(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/) /* 0600003F */
  } // end of property VirtualPathContext::Values
} // end of class Microsoft.AspNetCore.Routing.VirtualPathContext

.class /*02000015*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Routing.VirtualPathData
       extends [System.Runtime/*23000001*/]System.Object/*01000019*/
{
  .custom /*0C000057:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000040*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/ router,
                               string virtualPath) cil managed
  // SIG: 20 02 01 12 30 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathData::.ctor

  .method /*06000041*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/ router,
                               string virtualPath,
                               class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ dataTokens) cil managed
  // SIG: 20 03 01 12 30 0E 12 80 9D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathData::.ctor

  .method /*06000042*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ 
          get_DataTokens() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathData::get_DataTokens

  .method /*06000043*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/ 
          get_Router() cil managed
  // SIG: 20 00 12 30
  {
    .custom /*0C000082:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathData::get_Router

  .method /*06000044*/ public hidebysig specialname 
          instance void  set_Router(class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/ 'value') cil managed
  // SIG: 20 01 01 12 30
  {
    .custom /*0C000083:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000015*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathData::set_Router

  .method /*06000045*/ public hidebysig specialname 
          instance string  get_VirtualPath() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathData::get_VirtualPath

  .method /*06000046*/ public hidebysig specialname 
          instance void  set_VirtualPath(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method VirtualPathData::set_VirtualPath

  .property /*17000013*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/
          DataTokens()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Routing.RouteValueDictionary/*01000027*/ Microsoft.AspNetCore.Routing.VirtualPathData/*02000015*/::get_DataTokens() /* 06000042 */
  } // end of property VirtualPathData::DataTokens
  .property /*17000014*/ instance callconv(8) class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/
          Router()
  {
    .get instance class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/ Microsoft.AspNetCore.Routing.VirtualPathData/*02000015*/::get_Router() /* 06000043 */
    .set instance void Microsoft.AspNetCore.Routing.VirtualPathData/*02000015*/::set_Router(class Microsoft.AspNetCore.Routing.IRouter/*0200000C*/) /* 06000044 */
  } // end of property VirtualPathData::Router
  .property /*17000015*/ instance callconv(8) string
          VirtualPath()
  {
    .get instance string Microsoft.AspNetCore.Routing.VirtualPathData/*02000015*/::get_VirtualPath() /* 06000045 */
    .set instance void Microsoft.AspNetCore.Routing.VirtualPathData/*02000015*/::set_VirtualPath(string) /* 06000046 */
  } // end of property VirtualPathData::VirtualPath
} // end of class Microsoft.AspNetCore.Routing.VirtualPathData


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
