


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
// Time-date stamp:            0x90e7d807
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00004200
// Size of init.data:              0x00000a00
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000080b2
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
// Checksum:                       0x000123bc
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00008060 [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x000005b0] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00005000 [0x00002470] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00008044 [0x0000001c] address [size] of Debug Directory:           
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
//              0x000040b8 Virtual Size
//              0x00004000 Virtual Address
//              0x00004200 Size of Raw Data
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
//              0x000005b0 Virtual Size
//              0x0000a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00004800 Pointer to Raw Data
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
//              0x00004e00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000000b4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00004000 Import Address Table
//              0x000080a2 Import Name Table
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
// 0x00004054 [0x00003f70] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00007fc4 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001b9c Size
//              '#~' Name
//    Stream 2:
//              0x00001c08 Offset
//              0x00001858 Size
//              '#Strings' Name
//    Stream 3:
//              0x00003460 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00003464 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00003474 Offset
//              0x00000afc Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000002090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 29808
// PE header size       : 1024 (536 used)    ( 3.44%)
// PE additional info   : 10983             (36.85%)
// Num.of PE sections   : 4
// CLR header size     : 72                 ( 0.24%)
// CLR meta-data size  : 16240              (54.48%)
// CLR additional info : 128                ( 0.43%)
// CLR method headers  : 148                ( 0.50%)
// Managed code         : 296               ( 0.99%)
// Data                 : 2560              ( 8.59%)
// Unaccounted          : -1643             (-5.51%)

// Num.of PE sections   : 4
//   .mvid    - 512
//   .text    - 16896
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 16240
//   Module        -    1 (10 bytes)
//   TypeDef       -   35 (490 bytes)     7 interfaces, 0 explicit layout
//   TypeRef       -   64 (384 bytes)
//   MethodDef     -  185 (2590 bytes)    37 abstract, 0 native, 148 bodies
//   FieldDef      -   29 (174 bytes)     3 constant
//   MemberRef     -   25 (150 bytes)
//   ParamDef      -  152 (912 bytes)
//   MethodImpl    -    6 (36 bytes)
//   Constant      -    3 (18 bytes)
//   CustomAttribute-  167 (1002 bytes)
//   InterfaceImpl -   13 (52 bytes)
//   PropertyMap   -    8 (32 bytes)
//   Property      -   56 (336 bytes)
//   MethodSemantic-   86 (516 bytes)
//   TypeSpec      -    3 (6 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -   11 (220 bytes)
//   NestedClass   -    5 (20 bytes)
//   Strings       -  6232 bytes
//   Blobs         -  2812 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   206 bytes

// CLR additional info : 128

// CLR method headers : 148
//   Num.of method bodies  - 148
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 148

// Managed code : 296
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class AntiforgeryServiceCollectionExtensions (public) (abstract) (auto) (ansi) (sealed) 
// Class AntiforgeryOptions             (public) (auto) (ansi) 
// Class AntiforgeryTokenSet            (public) (auto) (ansi) 
// Class AntiforgeryValidationException (public) (auto) (ansi) 
// Interface IAntiforgery                   (public) (abstract) (auto) (ansi) 
// Interface IAntiforgeryAdditionalDataProvider (public) (abstract) (auto) (ansi) 
// Class AntiforgeryFeature             (auto) (ansi) 
// Class AntiforgeryLoggerExtensions    (abstract) (auto) (ansi) (sealed) 
// Class AntiforgeryOptionsSetup        (auto) (ansi) 
// Class AntiforgerySerializationContext (auto) (ansi) 
// Class AntiforgerySerializationContextPooledObjectPolicy (auto) (ansi) 
// Class AntiforgeryToken               (auto) (ansi) (sealed) 
// Class BinaryBlob                     (auto) (ansi) (sealed) 
// Class DefaultAntiforgery             (auto) (ansi) 
// Class DefaultAntiforgeryAdditionalDataProvider (auto) (ansi) 
// Class DefaultAntiforgeryTokenGenerator (auto) (ansi) 
// Class DefaultAntiforgeryTokenSerializer (auto) (ansi) 
// Class DefaultAntiforgeryTokenStore   (auto) (ansi) 
// Class DefaultClaimUidExtractor       (auto) (ansi) 
// Interface IAntiforgeryFeature            (abstract) (auto) (ansi) 
// Interface IAntiforgeryTokenGenerator     (abstract) (auto) (ansi) 
// Interface IAntiforgeryTokenSerializer    (abstract) (auto) (ansi) 
// Interface IAntiforgeryTokenStore         (abstract) (auto) (ansi) 
// Interface IClaimUidExtractor             (abstract) (auto) (ansi) 
// Class Resources                      (abstract) (auto) (ansi) (sealed) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
// Class <IsRequestValidAsync>d__8      (auto) (ansi) (sealed) (nested private) 
// Class <ValidateRequestAsync>d__9     (auto) (ansi) (sealed) (nested private) 
// Class <GetRequestTokensAsync>d__3    (auto) (ansi) (sealed) (nested private) 
// Class <>c                            (auto) (ansi) (sealed) (nested private) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ Microsoft.Extensions.DependencyInjection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.ComponentModel
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ Microsoft.Extensions.ObjectPool
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ Microsoft.AspNetCore.Http.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ Microsoft.Extensions.Logging.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ Microsoft.Extensions.Options
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000008*/ Microsoft.AspNetCore.DataProtection
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*23000009*/ Microsoft.AspNetCore.DataProtection.Abstractions
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000A*/ Microsoft.AspNetCore.Http.Features
{
  .publickeytoken = (AD B9 79 38 29 DD AE 60 )                         // ..y8)..`
  .ver 6:0:0:0
}
.assembly extern /*2300000B*/ System.Security.Claims
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ Microsoft.AspNetCore.Antiforgery
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
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 64 44 79 6E 61 6D 69 63 50 72 6F 78 79   // ...dDynamicProxy
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
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.InternalsVisibleToAttribute/*01000007*/::.ctor(string) /* 0A000006 */ = ( 01 00 81 71 4D 69 63 72 6F 73 6F 66 74 2E 41 73   // ...qMicrosoft.As
                                                                                                                                                                                    70 4E 65 74 43 6F 72 65 2E 41 6E 74 69 66 6F 72   // pNetCore.Antifor
                                                                                                                                                                                    67 65 72 79 2E 54 65 73 74 2C 20 50 75 62 6C 69   // gery.Test, Publi
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
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
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
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 7E 41 6E 20 61 6E 74 69 66 6F 72 67 65 72   // ..~An antiforger
                                                                                                                                                                       79 20 73 79 73 74 65 6D 20 66 6F 72 20 41 53 50   // y system for ASP
                                                                                                                                                                       2E 4E 45 54 20 43 6F 72 65 20 64 65 73 69 67 6E   // .NET Core design
                                                                                                                                                                       65 64 20 74 6F 20 67 65 6E 65 72 61 74 65 20 61   // ed to generate a
                                                                                                                                                                       6E 64 20 76 61 6C 69 64 61 74 65 20 74 6F 6B 65   // nd validate toke
                                                                                                                                                                       6E 73 20 74 6F 20 70 72 65 76 65 6E 74 20 43 72   // ns to prevent Cr
                                                                                                                                                                       6F 73 73 2D 53 69 74 65 20 52 65 71 75 65 73 74   // oss-Site Request
                                                                                                                                                                       20 46 6F 72 67 65 72 79 20 61 74 74 61 63 6B 73   //  Forgery attacks
                                                                                                                                                                       2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 32 32 2E 36 34 31 32 00   // ...6.0.222.6412.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 32 2B 31 64 63 66 37 61 63   // ...6.0.2+1dcf7ac
                                                                                                                                                                                66 61 63 66 30 66 65 31 35 34 61 64 63 63 32 33   // facf0fe154adcc23
                                                                                                                                                                                32 37 30 63 62 30 64 61 31 31 66 66 34 34 61 63   // 270cb0da11ff44ac
                                                                                                                                                                                65 00 00 )                                        // e..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 16 4D 69 63 72 6F 73 6F 66 74 20 41 53 50   // ...Microsoft ASP
                                                                                                                                                                   2E 4E 45 54 20 43 6F 72 65 00 00 )                // .NET Core..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 20 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // .. Microsoft.Asp
                                                                                                                                                                 4E 65 74 43 6F 72 65 2E 41 6E 74 69 66 6F 72 67   // NetCore.Antiforg
                                                                                                                                                                 65 72 79 00 00 )                                  // ery..
  .custom /*0C000016:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    24 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // $https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 61 73 70 6E 65   // com/dotnet/aspne
                                                                                                                                                                    74 63 6F 72 65 00 00 )                            // tcore..
  .custom /*0C000017:0A000011*/ instance void [System.Runtime/*23000001*/]System.Resources.NeutralResourcesLanguageAttribute/*01000012*/::.ctor(string) /* 0A000011 */ = ( 01 00 05 65 6E 2D 55 53 00 00 )                   // ...en-US..
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
.module Microsoft.AspNetCore.Antiforgery.dll
// MVID: {2e8d4350-981d-477c-b41b-66d458adda69}
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
  .custom /*0C000018:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
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
       extends [System.Runtime/*23000001*/]System.Attribute/*01000014*/
{
  .custom /*0C00001B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
  .custom /*0C000020:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
  .custom /*0C000024:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000016*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000015*/) /* 0A000013 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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

.class /*02000006*/ public abstract auto ansi sealed beforefieldinit Microsoft.Extensions.DependencyInjection.AntiforgeryServiceCollectionExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002B:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .class /*0200001F*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000068:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000007*/ public static initonly class Microsoft.Extensions.DependencyInjection.AntiforgeryServiceCollectionExtensions/*02000006*//'<>c'/*0200001F*/ '<>9'
    .field /*04000008*/ public static class [System.Runtime/*23000001*/]System.Func`2/*0100001A*/<class [System.ComponentModel/*23000003*/]System.IServiceProvider/*0100001B*/,class [Microsoft.Extensions.ObjectPool/*23000004*/]Microsoft.Extensions.ObjectPool.ObjectPool`1/*0100001C*/<class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/>> '<>9__0_0'
    .method /*060000AD*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*060000AE*/ assembly hidebysig 
            instance class [Microsoft.Extensions.ObjectPool/*23000004*/]Microsoft.Extensions.ObjectPool.ObjectPool`1/*0100001C*/<class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/> 
            '<AddAntiforgery>b__0_0'(class [System.ComponentModel/*23000003*/]System.IServiceProvider/*0100001B*/ serviceProvider) cil managed
    // SIG: 20 01 15 12 71 01 12 3C 12 6D
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<AddAntiforgery>b__0_0'

  } // end of class '<>c'

  .method /*06000006*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000002*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000018*/ 
          AddAntiforgery(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000002*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000018*/ services) cil managed
  // SIG: 00 01 12 61 12 61
  {
    .custom /*0C000028:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryServiceCollectionExtensions::AddAntiforgery

  .method /*06000007*/ public hidebysig static 
          class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000002*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000018*/ 
          AddAntiforgery(class [Microsoft.Extensions.DependencyInjection.Abstractions/*23000002*/]Microsoft.Extensions.DependencyInjection.IServiceCollection/*01000018*/ services,
                         class [System.Runtime/*23000001*/]System.Action`1/*01000019*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/> setupAction) cil managed
  // SIG: 00 02 12 61 12 61 15 12 65 01 12 1C
  {
    .custom /*0C00002D:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryServiceCollectionExtensions::AddAntiforgery

} // end of class Microsoft.Extensions.DependencyInjection.AntiforgeryServiceCollectionExtensions

.class /*02000007*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ public static initonly string DefaultCookiePrefix
  .method /*06000008*/ public hidebysig specialname 
          instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.CookieBuilder/*0100001D*/ 
          get_Cookie() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::get_Cookie

  .method /*06000009*/ public hidebysig specialname 
          instance void  set_Cookie(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.CookieBuilder/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 12 75
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::set_Cookie

  .method /*0600000A*/ public hidebysig specialname 
          instance string  get_FormFieldName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::get_FormFieldName

  .method /*0600000B*/ public hidebysig specialname 
          instance void  set_FormFieldName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::set_FormFieldName

  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_HeaderName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00003C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::get_HeaderName

  .method /*0600000D*/ public hidebysig specialname 
          instance void  set_HeaderName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000040:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::set_HeaderName

  .method /*0600000E*/ public hidebysig specialname 
          instance bool  get_SuppressXFrameOptionsHeader() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000044:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::get_SuppressXFrameOptionsHeader

  .method /*0600000F*/ public hidebysig specialname 
          instance void  set_SuppressXFrameOptionsHeader(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000048:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::set_SuppressXFrameOptionsHeader

  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptions::.ctor

  .property /*17000001*/ instance callconv(8) class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.CookieBuilder/*0100001D*/
          Cookie()
  {
    .get instance class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.CookieBuilder/*0100001D*/ Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/::get_Cookie() /* 06000008 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/::set_Cookie(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.CookieBuilder/*0100001D*/) /* 06000009 */
  } // end of property AntiforgeryOptions::Cookie
  .property /*17000002*/ instance callconv(8) string
          FormFieldName()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/::get_FormFieldName() /* 0600000A */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/::set_FormFieldName(string) /* 0600000B */
  } // end of property AntiforgeryOptions::FormFieldName
  .property /*17000003*/ instance callconv(8) string
          HeaderName()
  {
    .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/::get_HeaderName() /* 0600000C */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/::set_HeaderName(string) /* 0600000D */
  } // end of property AntiforgeryOptions::HeaderName
  .property /*17000004*/ instance callconv(8) bool
          SuppressXFrameOptionsHeader()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/::get_SuppressXFrameOptionsHeader() /* 0600000E */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/::set_SuppressXFrameOptionsHeader(bool) /* 0600000F */
  } // end of property AntiforgeryOptions::SuppressXFrameOptionsHeader
} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions

.class /*02000008*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000031:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string requestToken,
                               string cookieToken,
                               string formFieldName,
                               string headerName) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    .param [3]/*0800000A*/ 
    .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryTokenSet::.ctor

  .method /*06000012*/ public hidebysig specialname 
          instance string  get_RequestToken() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00004F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryTokenSet::get_RequestToken

  .method /*06000013*/ public hidebysig specialname 
          instance string  get_FormFieldName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000053:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000054:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryTokenSet::get_FormFieldName

  .method /*06000014*/ public hidebysig specialname 
          instance string  get_HeaderName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000057:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryTokenSet::get_HeaderName

  .method /*06000015*/ public hidebysig specialname 
          instance string  get_CookieToken() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000058:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryTokenSet::get_CookieToken

  .property /*17000005*/ instance callconv(8) string
          RequestToken()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/::get_RequestToken() /* 06000012 */
  } // end of property AntiforgeryTokenSet::RequestToken
  .property /*17000006*/ instance callconv(8) string
          FormFieldName()
  {
    .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/::get_FormFieldName() /* 06000013 */
  } // end of property AntiforgeryTokenSet::FormFieldName
  .property /*17000007*/ instance callconv(8) string
          HeaderName()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/::get_HeaderName() /* 06000014 */
  } // end of property AntiforgeryTokenSet::HeaderName
  .property /*17000008*/ instance callconv(8) string
          CookieToken()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/::get_CookieToken() /* 06000015 */
  } // end of property AntiforgeryTokenSet::CookieToken
} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet

.class /*02000009*/ public auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException
       extends [System.Runtime/*23000001*/]System.Exception/*0100001E*/
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000016*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryValidationException::.ctor

  .method /*06000017*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 79
  {
    .param [2]/*0800000E*/ 
    .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryValidationException::.ctor

} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException

.class /*0200000A*/ interface public abstract auto ansi Microsoft.AspNetCore.Antiforgery.IAntiforgery
{
  .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000018*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/ 
          GetAndStoreTokens(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 12 20 12 7D
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgery::GetAndStoreTokens

  .method /*06000019*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/ 
          GetTokens(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 12 20 12 7D
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgery::GetTokens

  .method /*0600001A*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000020*/<bool> 
          IsRequestValidAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 15 12 80 81 01 02 12 7D
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgery::IsRequestValidAsync

  .method /*0600001B*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          ValidateRequestAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 12 80 85 12 7D
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgery::ValidateRequestAsync

  .method /*0600001C*/ public hidebysig newslot abstract virtual 
          instance void  SetCookieTokenAndHeader(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgery::SetCookieTokenAndHeader

} // end of class Microsoft.AspNetCore.Antiforgery.IAntiforgery

.class /*0200000B*/ interface public abstract auto ansi Microsoft.AspNetCore.Antiforgery.IAntiforgeryAdditionalDataProvider
{
  .custom /*0C00003A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600001D*/ public hidebysig newslot abstract virtual 
          instance string  GetAdditionalData(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ context) cil managed
  // SIG: 20 01 0E 12 7D
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryAdditionalDataProvider::GetAdditionalData

  .method /*0600001E*/ public hidebysig newslot abstract virtual 
          instance bool  ValidateAdditionalData(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ context,
                                                string additionalData) cil managed
  // SIG: 20 02 02 12 7D 0E
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryAdditionalDataProvider::ValidateAdditionalData

} // end of class Microsoft.AspNetCore.Antiforgery.IAntiforgeryAdditionalDataProvider

.class /*0200000C*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/
{
  .custom /*0C00003D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001F*/ public hidebysig newslot specialname virtual final 
          instance bool  get_HaveDeserializedCookieToken() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000067:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_HaveDeserializedCookieToken

  .method /*06000020*/ public hidebysig newslot specialname virtual final 
          instance void  set_HaveDeserializedCookieToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000069:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_HaveDeserializedCookieToken

  .method /*06000021*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          get_CookieToken() cil managed
  // SIG: 20 00 12 44
  {
    .custom /*0C00006B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_CookieToken

  .method /*06000022*/ public hidebysig newslot specialname virtual final 
          instance void  set_CookieToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C00006D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_CookieToken

  .method /*06000023*/ public hidebysig newslot specialname virtual final 
          instance bool  get_HaveDeserializedRequestToken() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00006F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_HaveDeserializedRequestToken

  .method /*06000024*/ public hidebysig newslot specialname virtual final 
          instance void  set_HaveDeserializedRequestToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000071:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_HaveDeserializedRequestToken

  .method /*06000025*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          get_RequestToken() cil managed
  // SIG: 20 00 12 44
  {
    .custom /*0C000072:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_RequestToken

  .method /*06000026*/ public hidebysig newslot specialname virtual final 
          instance void  set_RequestToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C000074:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_RequestToken

  .method /*06000027*/ public hidebysig newslot specialname virtual final 
          instance bool  get_HaveGeneratedNewCookieToken() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000075:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_HaveGeneratedNewCookieToken

  .method /*06000028*/ public hidebysig newslot specialname virtual final 
          instance void  set_HaveGeneratedNewCookieToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000076:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_HaveGeneratedNewCookieToken

  .method /*06000029*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          get_NewCookieToken() cil managed
  // SIG: 20 00 12 44
  {
    .custom /*0C000078:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_NewCookieToken

  .method /*0600002A*/ public hidebysig newslot specialname virtual final 
          instance void  set_NewCookieToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C000079:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_NewCookieToken

  .method /*0600002B*/ public hidebysig newslot specialname virtual final 
          instance string  get_NewCookieTokenString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_NewCookieTokenString

  .method /*0600002C*/ public hidebysig newslot specialname virtual final 
          instance void  set_NewCookieTokenString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_NewCookieTokenString

  .method /*0600002D*/ public hidebysig newslot specialname virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          get_NewRequestToken() cil managed
  // SIG: 20 00 12 44
  {
    .custom /*0C00007C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_NewRequestToken

  .method /*0600002E*/ public hidebysig newslot specialname virtual final 
          instance void  set_NewRequestToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    .custom /*0C00007D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_NewRequestToken

  .method /*0600002F*/ public hidebysig newslot specialname virtual final 
          instance string  get_NewRequestTokenString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_NewRequestTokenString

  .method /*06000030*/ public hidebysig newslot specialname virtual final 
          instance void  set_NewRequestTokenString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_NewRequestTokenString

  .method /*06000031*/ public hidebysig newslot specialname virtual final 
          instance bool  get_HaveStoredNewCookieToken() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C000080:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::get_HaveStoredNewCookieToken

  .method /*06000032*/ public hidebysig newslot specialname virtual final 
          instance void  set_HaveStoredNewCookieToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000081:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::set_HaveStoredNewCookieToken

  .method /*06000033*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryFeature::.ctor

  .property /*17000009*/ instance callconv(8) bool
          HaveDeserializedCookieToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_HaveDeserializedCookieToken() /* 0600001F */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_HaveDeserializedCookieToken(bool) /* 06000020 */
  } // end of property AntiforgeryFeature::HaveDeserializedCookieToken
  .property /*1700000A*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/
          CookieToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_CookieToken() /* 06000021 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_CookieToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/) /* 06000022 */
  } // end of property AntiforgeryFeature::CookieToken
  .property /*1700000B*/ instance callconv(8) bool
          HaveDeserializedRequestToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_HaveDeserializedRequestToken() /* 06000023 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_HaveDeserializedRequestToken(bool) /* 06000024 */
  } // end of property AntiforgeryFeature::HaveDeserializedRequestToken
  .property /*1700000C*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/
          RequestToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_RequestToken() /* 06000025 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_RequestToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/) /* 06000026 */
  } // end of property AntiforgeryFeature::RequestToken
  .property /*1700000D*/ instance callconv(8) bool
          HaveGeneratedNewCookieToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_HaveGeneratedNewCookieToken() /* 06000027 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_HaveGeneratedNewCookieToken(bool) /* 06000028 */
  } // end of property AntiforgeryFeature::HaveGeneratedNewCookieToken
  .property /*1700000E*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/
          NewCookieToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_NewCookieToken() /* 06000029 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_NewCookieToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/) /* 0600002A */
  } // end of property AntiforgeryFeature::NewCookieToken
  .property /*1700000F*/ instance callconv(8) string
          NewCookieTokenString()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_NewCookieTokenString() /* 0600002B */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_NewCookieTokenString(string) /* 0600002C */
  } // end of property AntiforgeryFeature::NewCookieTokenString
  .property /*17000010*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/
          NewRequestToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_NewRequestToken() /* 0600002D */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_NewRequestToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/) /* 0600002E */
  } // end of property AntiforgeryFeature::NewRequestToken
  .property /*17000011*/ instance callconv(8) string
          NewRequestTokenString()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_NewRequestTokenString() /* 0600002F */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_NewRequestTokenString(string) /* 06000030 */
  } // end of property AntiforgeryFeature::NewRequestTokenString
  .property /*17000012*/ instance callconv(8) bool
          HaveStoredNewCookieToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::get_HaveStoredNewCookieToken() /* 06000031 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature/*0200000C*/::set_HaveStoredNewCookieToken(bool) /* 06000032 */
  } // end of property AntiforgeryFeature::HaveStoredNewCookieToken
} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgeryFeature

.class /*0200000D*/ private abstract auto ansi sealed Microsoft.AspNetCore.Antiforgery.AntiforgeryLoggerExtensions
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000041:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000043:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
  .method /*06000034*/ public hidebysig static 
          void  ValidationFailed(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger,
                                 string message) cil managed
  // SIG: 00 02 01 12 80 89 0E
  {
    .custom /*0C000082:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::ValidationFailed

  .method /*06000035*/ public hidebysig static 
          void  ValidatedAntiforgeryToken(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger) cil managed
  // SIG: 00 01 01 12 80 89
  {
    .custom /*0C000083:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::ValidatedAntiforgeryToken

  .method /*06000036*/ public hidebysig static 
          void  MissingCookieToken(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger,
                                   string cookieName) cil managed
  // SIG: 00 02 01 12 80 89 0E
  {
    .custom /*0C000084:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [2]/*08000025*/ 
    .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::MissingCookieToken

  .method /*06000037*/ public hidebysig static 
          void  MissingRequestToken(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger,
                                    string formFieldName,
                                    string headerName) cil managed
  // SIG: 00 03 01 12 80 89 0E 0E
  {
    .custom /*0C000085:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    .param [3]/*08000028*/ 
    .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::MissingRequestToken

  .method /*06000038*/ public hidebysig static 
          void  NewCookieToken(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger) cil managed
  // SIG: 00 01 01 12 80 89
  {
    .custom /*0C000086:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::NewCookieToken

  .method /*06000039*/ public hidebysig static 
          void  ReusedCookieToken(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger) cil managed
  // SIG: 00 01 01 12 80 89
  {
    .custom /*0C000087:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::ReusedCookieToken

  .method /*0600003A*/ public hidebysig static 
          void  TokenDeserializeException(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger,
                                          class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ exception) cil managed
  // SIG: 00 02 01 12 80 89 12 79
  {
    .custom /*0C000088:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::TokenDeserializeException

  .method /*0600003B*/ public hidebysig static 
          void  ResponseCacheHeadersOverridenToNoCache(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger) cil managed
  // SIG: 00 01 01 12 80 89
  {
    .custom /*0C000089:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::ResponseCacheHeadersOverridenToNoCache

  .method /*0600003C*/ public hidebysig static 
          void  FailedToDeserialzeTokens(class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILogger/*01000022*/ logger,
                                         class [System.Runtime/*23000001*/]System.Exception/*0100001E*/ exception) cil managed
  // SIG: 00 02 01 12 80 89 12 79
  {
    .custom /*0C00008A:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ExtensionAttribute/*01000001*/::.ctor() /* 0A000001 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryLoggerExtensions::FailedToDeserialzeTokens

} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgeryLoggerExtensions

.class /*0200000E*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.AntiforgeryOptionsSetup
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [Microsoft.Extensions.Options/*23000007*/]Microsoft.Extensions.Options.IConfigureOptions`1/*01000023*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/>/*1B000001*/
{
  .custom /*0C000045:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [Microsoft.Extensions.Options/*23000007*/]Microsoft.Extensions.Options.IConfigureOptions`1/*01000023*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/>/*1B000001*/
  .custom /*0C00001A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000007*/]Microsoft.Extensions.Options.IOptions`1/*01000024*/<class [Microsoft.AspNetCore.DataProtection/*23000008*/]Microsoft.AspNetCore.DataProtection.DataProtectionOptions/*01000025*/> dataProtectionOptions) cil managed
  // SIG: 20 01 01 15 12 80 91 01 12 80 95
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptionsSetup::.ctor

  .method /*0600003E*/ public hidebysig newslot virtual final 
          instance void  Configure(class Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/ options) cil managed
  // SIG: 20 01 01 12 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryOptionsSetup::Configure

} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgeryOptionsSetup

.class /*0200000F*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C000049:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600003F*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.MemoryStream/*01000026*/ 
          get_Stream() cil managed
  // SIG: 20 00 12 80 99
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContext::get_Stream

  .method /*06000040*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.BinaryReader/*01000027*/ 
          get_Reader() cil managed
  // SIG: 20 00 12 80 9D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContext::get_Reader

  .method /*06000041*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.BinaryWriter/*01000028*/ 
          get_Writer() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContext::get_Writer

  .method /*06000042*/ public hidebysig instance char[] 
          GetChars(int32 count) cil managed
  // SIG: 20 01 1D 03 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContext::GetChars

  .method /*06000043*/ public hidebysig instance void 
          Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContext::Reset

  .method /*06000044*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContext::.ctor

  .property /*17000013*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.MemoryStream/*01000026*/
          Stream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.MemoryStream/*01000026*/ Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/::get_Stream() /* 0600003F */
  } // end of property AntiforgerySerializationContext::Stream
  .property /*17000014*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.BinaryReader/*01000027*/
          Reader()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.BinaryReader/*01000027*/ Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/::get_Reader() /* 06000040 */
  } // end of property AntiforgerySerializationContext::Reader
  .property /*17000015*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.BinaryWriter/*01000028*/
          Writer()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.BinaryWriter/*01000028*/ Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/::get_Writer() /* 06000041 */
  } // end of property AntiforgerySerializationContext::Writer
} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext

.class /*02000010*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContextPooledObjectPolicy
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [Microsoft.Extensions.ObjectPool/*23000004*/]Microsoft.Extensions.ObjectPool.IPooledObjectPolicy`1/*01000029*/<class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/>/*1B000002*/
{
  .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type class [Microsoft.Extensions.ObjectPool/*23000004*/]Microsoft.Extensions.ObjectPool.IPooledObjectPolicy`1/*01000029*/<class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/>/*1B000002*/
  .custom /*0C00001E:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*06000045*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/ 
          Create() cil managed
  // SIG: 20 00 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContextPooledObjectPolicy::Create

  .method /*06000046*/ public hidebysig newslot virtual final 
          instance bool  Return(class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/ obj) cil managed
  // SIG: 20 01 02 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContextPooledObjectPolicy::Return

  .method /*06000047*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgerySerializationContextPooledObjectPolicy::.ctor

} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContextPooledObjectPolicy

.class /*02000011*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Antiforgery.AntiforgeryToken
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .custom /*0C00004D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000005*/ static assembly literal int32 SecurityTokenBitLength = int32(0x00000080)
  .field /*04000006*/ static assembly literal int32 ClaimUidBitLength = int32(0x00000100)
  .method /*06000048*/ public hidebysig specialname 
          instance string  get_AdditionalData() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::get_AdditionalData

  .method /*06000049*/ public hidebysig specialname 
          instance void  set_AdditionalData(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00008C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::set_AdditionalData

  .method /*0600004A*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/ 
          get_ClaimUid() cil managed
  // SIG: 20 00 12 48
  {
    .custom /*0C00008D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::get_ClaimUid

  .method /*0600004B*/ public hidebysig specialname 
          instance void  set_ClaimUid(class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/ 'value') cil managed
  // SIG: 20 01 01 12 48
  {
    .custom /*0C00008E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::set_ClaimUid

  .method /*0600004C*/ public hidebysig specialname 
          instance bool  get_IsCookieToken() cil managed
  // SIG: 20 00 02
  {
    .custom /*0C00008F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::get_IsCookieToken

  .method /*0600004D*/ public hidebysig specialname 
          instance void  set_IsCookieToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C000090:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::set_IsCookieToken

  .method /*0600004E*/ public hidebysig specialname 
          instance class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/ 
          get_SecurityToken() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::get_SecurityToken

  .method /*0600004F*/ public hidebysig specialname 
          instance void  set_SecurityToken(class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/ 'value') cil managed
  // SIG: 20 01 01 12 48
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::set_SecurityToken

  .method /*06000050*/ public hidebysig specialname 
          instance string  get_Username() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::get_Username

  .method /*06000051*/ public hidebysig specialname 
          instance void  set_Username(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::set_Username

  .method /*06000052*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AntiforgeryToken::.ctor

  .property /*17000016*/ instance callconv(8) string
          AdditionalData()
  {
    .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::get_AdditionalData() /* 06000048 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::set_AdditionalData(string) /* 06000049 */
  } // end of property AntiforgeryToken::AdditionalData
  .property /*17000017*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/
          ClaimUid()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/ Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::get_ClaimUid() /* 0600004A */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::set_ClaimUid(class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/) /* 0600004B */
  } // end of property AntiforgeryToken::ClaimUid
  .property /*17000018*/ instance callconv(8) bool
          IsCookieToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::get_IsCookieToken() /* 0600004C */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::set_IsCookieToken(bool) /* 0600004D */
  } // end of property AntiforgeryToken::IsCookieToken
  .property /*17000019*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/
          SecurityToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/ Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::get_SecurityToken() /* 0600004E */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::set_SecurityToken(class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/) /* 0600004F */
  } // end of property AntiforgeryToken::SecurityToken
  .property /*1700001A*/ instance callconv(8) string
          Username()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::get_Username() /* 06000050 */
    .set instance void Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/::set_Username(string) /* 06000051 */
  } // end of property AntiforgeryToken::Username
} // end of class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken

.class /*02000012*/ private auto ansi sealed beforefieldinit Microsoft.AspNetCore.Antiforgery.BinaryBlob
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002B*/<class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/>/*1B000003*/
{
  .custom /*0C000050:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000052:0A000014*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerDisplayAttribute/*0100002A*/::.ctor(string) /* 0A000014 */ = ( 01 00 10 7B 44 65 62 75 67 67 65 72 53 74 72 69   // ...{DebuggerStri
                                                                                                                                                                    6E 67 7D 00 00 )                                  // ng}..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002B*/<class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/>/*1B000003*/
  .custom /*0C000023:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*06000053*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 bitLength) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryBlob::.ctor

  .method /*06000054*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 bitLength,
                               uint8[] data) cil managed
  // SIG: 20 02 01 08 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryBlob::.ctor

  .method /*06000055*/ public hidebysig specialname 
          instance int32  get_BitLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryBlob::get_BitLength

  .method /*06000056*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryBlob::Equals

  .method /*06000057*/ public hidebysig newslot virtual final 
          instance bool  Equals(class Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/ other) cil managed
  // SIG: 20 01 02 12 48
  {
    .custom /*0C000094:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryBlob::Equals

  .method /*06000058*/ public hidebysig instance uint8[] 
          GetData() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryBlob::GetData

  .method /*06000059*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method BinaryBlob::GetHashCode

  .property /*1700001B*/ instance callconv(8) int32
          BitLength()
  {
    .get instance int32 Microsoft.AspNetCore.Antiforgery.BinaryBlob/*02000012*/::get_BitLength() /* 06000055 */
  } // end of property BinaryBlob::BitLength
} // end of class Microsoft.AspNetCore.Antiforgery.BinaryBlob

.class /*02000013*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.DefaultAntiforgery
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Antiforgery.IAntiforgery/*0200000A*/
{
  .custom /*0C000055:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Antiforgery.IAntiforgery/*0200000A*/
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000020*/ auto ansi sealed nested private beforefieldinit '<IsRequestValidAsync>d__8'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002F*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/
  {
    .custom /*0C00006A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000009*/ public int32 '<>1__state'
    .field /*0400000A*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000031*/<bool> '<>t__builder'
    .field /*0400000B*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext
    .field /*0400000C*/ public class Microsoft.AspNetCore.Antiforgery.DefaultAntiforgery/*02000013*/ '<>4__this'
    .field /*0400000D*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000032*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/> '<>u__1'
    .method /*060000AF*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/::MoveNext /*01000030::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsRequestValidAsync>d__8'::MoveNext

    .method /*060000B0*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 C1
    {
      .custom /*0C0000A5:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000033*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/::SetStateMachine /*01000030::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<IsRequestValidAsync>d__8'::SetStateMachine

  } // end of class '<IsRequestValidAsync>d__8'

  .class /*02000021*/ auto ansi sealed nested private beforefieldinit '<ValidateRequestAsync>d__9'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002F*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/
  {
    .custom /*0C00006C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*0400000E*/ public int32 '<>1__state'
    .field /*0400000F*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder/*01000034*/ '<>t__builder'
    .field /*04000010*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext
    .field /*04000011*/ public class Microsoft.AspNetCore.Antiforgery.DefaultAntiforgery/*02000013*/ '<>4__this'
    .field /*04000012*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000032*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/> '<>u__1'
    .method /*060000B1*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/::MoveNext /*01000030::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateRequestAsync>d__9'::MoveNext

    .method /*060000B2*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 C1
    {
      .custom /*0C0000A6:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000033*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/::SetStateMachine /*01000030::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<ValidateRequestAsync>d__9'::SetStateMachine

  } // end of class '<ValidateRequestAsync>d__9'

  .method /*0600005A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000007*/]Microsoft.Extensions.Options.IOptions`1/*01000024*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/> antiforgeryOptionsAccessor,
                               class Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenGenerator/*0200001A*/ tokenGenerator,
                               class Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenSerializer/*0200001B*/ tokenSerializer,
                               class Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenStore/*0200001C*/ tokenStore,
                               class [Microsoft.Extensions.Logging.Abstractions/*23000006*/]Microsoft.Extensions.Logging.ILoggerFactory/*0100002C*/ loggerFactory) cil managed
  // SIG: 20 05 01 15 12 80 91 01 12 1C 12 68 12 6C 12 70 12 80 B1
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgery::.ctor

  .method /*0600005B*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/ 
          GetAndStoreTokens(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 12 20 12 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgery::GetAndStoreTokens

  .method /*0600005C*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/ 
          GetTokens(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 12 20 12 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgery::GetTokens

  .method /*0600005D*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000020*/<bool> 
          IsRequestValidAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 15 12 80 81 01 02 12 7D
  {
    .custom /*0C000096:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002E*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002D*/) /* 0A000015 */ = ( 01 00 4D 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..MMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 6E 74 69 66 6F 72 67   // NetCore.Antiforg
                                                                                                                                                                                                                                        65 72 79 2E 44 65 66 61 75 6C 74 41 6E 74 69 66   // ery.DefaultAntif
                                                                                                                                                                                                                                        6F 72 67 65 72 79 2B 3C 49 73 52 65 71 75 65 73   // orgery+<IsReques
                                                                                                                                                                                                                                        74 56 61 6C 69 64 41 73 79 6E 63 3E 64 5F 5F 38   // tValidAsync>d__8
                                                                                                                                                                                                                                        00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgery::IsRequestValidAsync

  .method /*0600005E*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000021*/ 
          ValidateRequestAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 12 80 85 12 7D
  {
    .custom /*0C000097:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002E*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002D*/) /* 0A000015 */ = ( 01 00 4E 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..NMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 6E 74 69 66 6F 72 67   // NetCore.Antiforg
                                                                                                                                                                                                                                        65 72 79 2E 44 65 66 61 75 6C 74 41 6E 74 69 66   // ery.DefaultAntif
                                                                                                                                                                                                                                        6F 72 67 65 72 79 2B 3C 56 61 6C 69 64 61 74 65   // orgery+<Validate
                                                                                                                                                                                                                                        52 65 71 75 65 73 74 41 73 79 6E 63 3E 64 5F 5F   // RequestAsync>d__
                                                                                                                                                                                                                                        39 00 00 )                                        // 9..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgery::ValidateRequestAsync

  .method /*0600005F*/ public hidebysig newslot virtual final 
          instance void  SetCookieTokenAndHeader(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgery::SetCookieTokenAndHeader

  .method /*06000060*/ family hidebysig newslot virtual 
          instance void  SetDoNotCacheHeaders(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgery::SetDoNotCacheHeaders

} // end of class Microsoft.AspNetCore.Antiforgery.DefaultAntiforgery

.class /*02000014*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryAdditionalDataProvider
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Antiforgery.IAntiforgeryAdditionalDataProvider/*0200000B*/
{
  .method /*06000061*/ public hidebysig newslot virtual final 
          instance string  GetAdditionalData(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ context) cil managed
  // SIG: 20 01 0E 12 7D
  {
    .custom /*0C000099:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryAdditionalDataProvider::GetAdditionalData

  .method /*06000062*/ public hidebysig newslot virtual final 
          instance bool  ValidateAdditionalData(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ context,
                                                string additionalData) cil managed
  // SIG: 20 02 02 12 7D 0E
  {
    .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryAdditionalDataProvider::ValidateAdditionalData

  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryAdditionalDataProvider::.ctor

} // end of class Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryAdditionalDataProvider

.class /*02000015*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryTokenGenerator
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenGenerator/*0200001A*/
{
  .custom /*0C000059:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenGenerator/*0200001A*/
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class Microsoft.AspNetCore.Antiforgery.IClaimUidExtractor/*0200001D*/ claimUidExtractor,
                               class Microsoft.AspNetCore.Antiforgery.IAntiforgeryAdditionalDataProvider/*0200000B*/ additionalDataProvider) cil managed
  // SIG: 20 02 01 12 74 12 2C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenGenerator::.ctor

  .method /*06000065*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          GenerateCookieToken() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenGenerator::GenerateCookieToken

  .method /*06000066*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          GenerateRequestToken(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext,
                               class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ cookieToken) cil managed
  // SIG: 20 02 12 44 12 7D 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenGenerator::GenerateRequestToken

  .method /*06000067*/ public hidebysig newslot virtual final 
          instance bool  IsCookieTokenValid(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ cookieToken) cil managed
  // SIG: 20 01 02 12 44
  {
    .custom /*0C00009C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenGenerator::IsCookieTokenValid

  .method /*06000068*/ public hidebysig newslot virtual final 
          instance bool  TryValidateTokenSet(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext,
                                             class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ cookieToken,
                                             class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ requestToken,
                                             [out] string& message) cil managed
  // SIG: 20 04 02 12 7D 12 44 12 44 10 0E
  {
    .param [4]/*08000054*/ 
    .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000092:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000035*/::.ctor(bool) /* 0A000019 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenGenerator::TryValidateTokenSet

} // end of class Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryTokenGenerator

.class /*02000016*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryTokenSerializer
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenSerializer/*0200001B*/
{
  .custom /*0C00005B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenSerializer/*0200001B*/
  .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.AspNetCore.DataProtection.Abstractions/*23000009*/]Microsoft.AspNetCore.DataProtection.IDataProtectionProvider/*01000036*/ provider,
                               class [Microsoft.Extensions.ObjectPool/*23000004*/]Microsoft.Extensions.ObjectPool.ObjectPool`1/*0100001C*/<class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/> pool) cil managed
  // SIG: 20 02 01 12 80 D9 15 12 71 01 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenSerializer::.ctor

  .method /*0600006A*/ public hidebysig newslot virtual final 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          Deserialize(string serializedToken) cil managed
  // SIG: 20 01 12 44 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenSerializer::Deserialize

  .method /*0600006B*/ public hidebysig newslot virtual final 
          instance string  Serialize(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ token) cil managed
  // SIG: 20 01 0E 12 44
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenSerializer::Serialize

} // end of class Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryTokenSerializer

.class /*02000017*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryTokenStore
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenStore/*0200001C*/
{
  .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenStore/*0200001C*/
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000022*/ auto ansi sealed nested private beforefieldinit '<GetRequestTokensAsync>d__3'
         extends [System.Runtime/*23000001*/]System.ValueType/*0100002F*/
         implements [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/
  {
    .custom /*0C00006E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000013*/ public int32 '<>1__state'
    .field /*04000014*/ public valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1/*01000031*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/> '<>t__builder'
    .field /*04000015*/ public class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext
    .field /*04000016*/ public class Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryTokenStore/*02000017*/ '<>4__this'
    .field /*04000017*/ private string '<cookieToken>5__2'
    .field /*04000018*/ private valuetype [System.Runtime/*23000001*/]System.Runtime.CompilerServices.TaskAwaiter`1/*01000032*/<class [Microsoft.AspNetCore.Http.Features/*2300000A*/]Microsoft.AspNetCore.Http.IFormCollection/*01000037*/> '<>u__1'
    .method /*060000B3*/ private hidebysig newslot virtual final 
            instance void  MoveNext() cil managed
    // SIG: 20 00 01
    {
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/::MoveNext /*01000030::0A000016*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetRequestTokensAsync>d__3'::MoveNext

    .method /*060000B4*/ private hidebysig newslot virtual final 
            instance void  SetStateMachine(class [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/ stateMachine) cil managed
    // SIG: 20 01 01 12 80 C1
    {
      .custom /*0C0000A7:0A000018*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggerHiddenAttribute/*01000033*/::.ctor() /* 0A000018 */ = ( 01 00 00 00 ) 
      .override [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IAsyncStateMachine/*01000030*/::SetStateMachine /*01000030::0A000017*/ 
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<GetRequestTokensAsync>d__3'::SetStateMachine

  } // end of class '<GetRequestTokensAsync>d__3'

  .method /*0600006C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.Options/*23000007*/]Microsoft.Extensions.Options.IOptions`1/*01000024*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryOptions/*02000007*/> optionsAccessor) cil managed
  // SIG: 20 01 01 15 12 80 91 01 12 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenStore::.ctor

  .method /*0600006D*/ public hidebysig newslot virtual final 
          instance string  GetCookieToken(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 0E 12 7D
  {
    .param [0]/*0800005A*/ 
    .custom /*0C000095:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenStore::GetCookieToken

  .method /*0600006E*/ public hidebysig newslot virtual final 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000020*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/> 
          GetRequestTokensAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 15 12 80 81 01 12 20 12 7D
  {
    .custom /*0C00009D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.AsyncStateMachineAttribute/*0100002E*/::.ctor(class [System.Runtime/*23000001*/]System.Type/*0100002D*/) /* 0A000015 */ = ( 01 00 59 4D 69 63 72 6F 73 6F 66 74 2E 41 73 70   // ..YMicrosoft.Asp
                                                                                                                                                                                                                                        4E 65 74 43 6F 72 65 2E 41 6E 74 69 66 6F 72 67   // NetCore.Antiforg
                                                                                                                                                                                                                                        65 72 79 2E 44 65 66 61 75 6C 74 41 6E 74 69 66   // ery.DefaultAntif
                                                                                                                                                                                                                                        6F 72 67 65 72 79 54 6F 6B 65 6E 53 74 6F 72 65   // orgeryTokenStore
                                                                                                                                                                                                                                        2B 3C 47 65 74 52 65 71 75 65 73 74 54 6F 6B 65   // +<GetRequestToke
                                                                                                                                                                                                                                        6E 73 41 73 79 6E 63 3E 64 5F 5F 33 00 00 )       // nsAsync>d__3..
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenStore::GetRequestTokensAsync

  .method /*0600006F*/ public hidebysig newslot virtual final 
          instance void  SaveCookieToken(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext,
                                         string token) cil managed
  // SIG: 20 02 01 12 7D 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultAntiforgeryTokenStore::SaveCookieToken

} // end of class Microsoft.AspNetCore.Antiforgery.DefaultAntiforgeryTokenStore

.class /*02000018*/ private auto ansi beforefieldinit Microsoft.AspNetCore.Antiforgery.DefaultClaimUidExtractor
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
       implements Microsoft.AspNetCore.Antiforgery.IClaimUidExtractor/*0200001D*/
{
  .custom /*0C000060:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type Microsoft.AspNetCore.Antiforgery.IClaimUidExtractor/*0200001D*/
  .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*02000023*/ auto ansi serializable sealed nested private beforefieldinit '<>c'
         extends [System.Runtime/*23000001*/]System.Object/*01000017*/
  {
    .custom /*0C000070:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    .field /*04000019*/ public static initonly class Microsoft.AspNetCore.Antiforgery.DefaultClaimUidExtractor/*02000018*//'<>c'/*02000023*/ '<>9'
    .field /*0400001A*/ public static class [System.Runtime/*23000001*/]System.Predicate`1/*0100003C*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/> '<>9__3_1'
    .field /*0400001B*/ public static class [System.Runtime/*23000001*/]System.Predicate`1/*0100003C*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/> '<>9__3_2'
    .field /*0400001C*/ public static class [System.Runtime/*23000001*/]System.Predicate`1/*0100003C*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/> '<>9__3_3'
    .field /*0400001D*/ public static class [System.Runtime/*23000001*/]System.Comparison`1/*0100003E*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/> '<>9__3_0'
    .method /*060000B5*/ public hidebysig specialname rtspecialname 
            instance void  .ctor() cil managed
    // SIG: 20 00 01
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::.ctor

    .method /*060000B6*/ assembly hidebysig 
            instance bool  '<GetUniqueIdentifierParameters>b__3_1'(class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/ claim) cil managed
    // SIG: 20 01 02 12 80 F5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetUniqueIdentifierParameters>b__3_1'

    .method /*060000B7*/ assembly hidebysig 
            instance bool  '<GetUniqueIdentifierParameters>b__3_2'(class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/ claim) cil managed
    // SIG: 20 01 02 12 80 F5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetUniqueIdentifierParameters>b__3_2'

    .method /*060000B8*/ assembly hidebysig 
            instance bool  '<GetUniqueIdentifierParameters>b__3_3'(class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/ claim) cil managed
    // SIG: 20 01 02 12 80 F5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetUniqueIdentifierParameters>b__3_3'

    .method /*060000B9*/ assembly hidebysig 
            instance int32  '<GetUniqueIdentifierParameters>b__3_0'(class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/ a,
                                                                    class [System.Security.Claims/*2300000B*/]System.Security.Claims.Claim/*0100003D*/ b) cil managed
    // SIG: 20 02 08 12 80 F5 12 80 F5
    {
      // Method begins at RVA 0x4050
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method '<>c'::'<GetUniqueIdentifierParameters>b__3_0'

  } // end of class '<>c'

  .method /*06000070*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [Microsoft.Extensions.ObjectPool/*23000004*/]Microsoft.Extensions.ObjectPool.ObjectPool`1/*0100001C*/<class Microsoft.AspNetCore.Antiforgery.AntiforgerySerializationContext/*0200000F*/> pool) cil managed
  // SIG: 20 01 01 15 12 71 01 12 3C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultClaimUidExtractor::.ctor

  .method /*06000071*/ public hidebysig newslot virtual final 
          instance string  ExtractClaimUid(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ claimsPrincipal) cil managed
  // SIG: 20 01 0E 12 80 E1
  {
    .param [0]/*08000060*/ 
    .custom /*0C000098:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultClaimUidExtractor::ExtractClaimUid

  .method /*06000072*/ public hidebysig static 
          class [System.Runtime/*23000001*/]System.Collections.Generic.IList`1/*01000039*/<string> 
          GetUniqueIdentifierParameters(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*0100003A*/<class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsIdentity/*0100003B*/> claimsIdentities) cil managed
  // SIG: 00 01 15 12 80 E5 01 0E 15 12 80 E9 01 12 80 ED
  {
    .param [0]/*08000062*/ 
    .custom /*0C00009B:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DefaultClaimUidExtractor::GetUniqueIdentifierParameters

} // end of class Microsoft.AspNetCore.Antiforgery.DefaultClaimUidExtractor

.class /*02000019*/ interface private abstract auto ansi Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature
{
  .custom /*0C000062:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .method /*06000073*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          get_CookieToken() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_CookieToken

  .method /*06000074*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_CookieToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_CookieToken

  .method /*06000075*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_HaveDeserializedCookieToken() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_HaveDeserializedCookieToken

  .method /*06000076*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_HaveDeserializedCookieToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_HaveDeserializedCookieToken

  .method /*06000077*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_HaveDeserializedRequestToken() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_HaveDeserializedRequestToken

  .method /*06000078*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_HaveDeserializedRequestToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_HaveDeserializedRequestToken

  .method /*06000079*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_HaveGeneratedNewCookieToken() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_HaveGeneratedNewCookieToken

  .method /*0600007A*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_HaveGeneratedNewCookieToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_HaveGeneratedNewCookieToken

  .method /*0600007B*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_HaveStoredNewCookieToken() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_HaveStoredNewCookieToken

  .method /*0600007C*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_HaveStoredNewCookieToken(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_HaveStoredNewCookieToken

  .method /*0600007D*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          get_NewCookieToken() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_NewCookieToken

  .method /*0600007E*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_NewCookieToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_NewCookieToken

  .method /*0600007F*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_NewCookieTokenString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_NewCookieTokenString

  .method /*06000080*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_NewCookieTokenString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_NewCookieTokenString

  .method /*06000081*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          get_NewRequestToken() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_NewRequestToken

  .method /*06000082*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_NewRequestToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_NewRequestToken

  .method /*06000083*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_NewRequestTokenString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_NewRequestTokenString

  .method /*06000084*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_NewRequestTokenString(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_NewRequestTokenString

  .method /*06000085*/ public hidebysig newslot specialname abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          get_RequestToken() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::get_RequestToken

  .method /*06000086*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_RequestToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryFeature::set_RequestToken

  .property /*1700001C*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/
          CookieToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_CookieToken() /* 06000073 */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_CookieToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/) /* 06000074 */
  } // end of property IAntiforgeryFeature::CookieToken
  .property /*1700001D*/ instance callconv(8) bool
          HaveDeserializedCookieToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_HaveDeserializedCookieToken() /* 06000075 */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_HaveDeserializedCookieToken(bool) /* 06000076 */
  } // end of property IAntiforgeryFeature::HaveDeserializedCookieToken
  .property /*1700001E*/ instance callconv(8) bool
          HaveDeserializedRequestToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_HaveDeserializedRequestToken() /* 06000077 */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_HaveDeserializedRequestToken(bool) /* 06000078 */
  } // end of property IAntiforgeryFeature::HaveDeserializedRequestToken
  .property /*1700001F*/ instance callconv(8) bool
          HaveGeneratedNewCookieToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_HaveGeneratedNewCookieToken() /* 06000079 */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_HaveGeneratedNewCookieToken(bool) /* 0600007A */
  } // end of property IAntiforgeryFeature::HaveGeneratedNewCookieToken
  .property /*17000020*/ instance callconv(8) bool
          HaveStoredNewCookieToken()
  {
    .get instance bool Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_HaveStoredNewCookieToken() /* 0600007B */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_HaveStoredNewCookieToken(bool) /* 0600007C */
  } // end of property IAntiforgeryFeature::HaveStoredNewCookieToken
  .property /*17000021*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/
          NewCookieToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_NewCookieToken() /* 0600007D */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_NewCookieToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/) /* 0600007E */
  } // end of property IAntiforgeryFeature::NewCookieToken
  .property /*17000022*/ instance callconv(8) string
          NewCookieTokenString()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_NewCookieTokenString() /* 0600007F */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_NewCookieTokenString(string) /* 06000080 */
  } // end of property IAntiforgeryFeature::NewCookieTokenString
  .property /*17000023*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/
          NewRequestToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_NewRequestToken() /* 06000081 */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_NewRequestToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/) /* 06000082 */
  } // end of property IAntiforgeryFeature::NewRequestToken
  .property /*17000024*/ instance callconv(8) string
          NewRequestTokenString()
  {
    .get instance string Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_NewRequestTokenString() /* 06000083 */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_NewRequestTokenString(string) /* 06000084 */
  } // end of property IAntiforgeryFeature::NewRequestTokenString
  .property /*17000025*/ instance callconv(8) class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/
          RequestToken()
  {
    .get instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::get_RequestToken() /* 06000085 */
    .set instance void Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature/*02000019*/::set_RequestToken(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/) /* 06000086 */
  } // end of property IAntiforgeryFeature::RequestToken
} // end of class Microsoft.AspNetCore.Antiforgery.IAntiforgeryFeature

.class /*0200001A*/ interface private abstract auto ansi Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenGenerator
{
  .custom /*0C000063:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000087*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          GenerateCookieToken() cil managed
  // SIG: 20 00 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenGenerator::GenerateCookieToken

  .method /*06000088*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          GenerateRequestToken(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext,
                               class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ cookieToken) cil managed
  // SIG: 20 02 12 44 12 7D 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenGenerator::GenerateRequestToken

  .method /*06000089*/ public hidebysig newslot abstract virtual 
          instance bool  IsCookieTokenValid(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ cookieToken) cil managed
  // SIG: 20 01 02 12 44
  {
    .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenGenerator::IsCookieTokenValid

  .method /*0600008A*/ public hidebysig newslot abstract virtual 
          instance bool  TryValidateTokenSet(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext,
                                             class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ cookieToken,
                                             class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ requestToken,
                                             [out] string& message) cil managed
  // SIG: 20 04 02 12 7D 12 44 12 44 10 0E
  {
    .param [4]/*08000074*/ 
    .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00009F:0A000019*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000035*/::.ctor(bool) /* 0A000019 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenGenerator::TryValidateTokenSet

} // end of class Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenGenerator

.class /*0200001B*/ interface private abstract auto ansi Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenSerializer
{
  .custom /*0C000064:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600008B*/ public hidebysig newslot abstract virtual 
          instance class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ 
          Deserialize(string serializedToken) cil managed
  // SIG: 20 01 12 44 0E
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenSerializer::Deserialize

  .method /*0600008C*/ public hidebysig newslot abstract virtual 
          instance string  Serialize(class Microsoft.AspNetCore.Antiforgery.AntiforgeryToken/*02000011*/ token) cil managed
  // SIG: 20 01 0E 12 44
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenSerializer::Serialize

} // end of class Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenSerializer

.class /*0200001C*/ interface private abstract auto ansi Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenStore
{
  .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600008D*/ public hidebysig newslot abstract virtual 
          instance string  GetCookieToken(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 0E 12 7D
  {
    .param [0]/*08000077*/ 
    .custom /*0C0000A0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenStore::GetCookieToken

  .method /*0600008E*/ public hidebysig newslot abstract virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000020*/<class Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet/*02000008*/> 
          GetRequestTokensAsync(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext) cil managed
  // SIG: 20 01 15 12 80 81 01 12 20 12 7D
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenStore::GetRequestTokensAsync

  .method /*0600008F*/ public hidebysig newslot abstract virtual 
          instance void  SaveCookieToken(class [Microsoft.AspNetCore.Http.Abstractions/*23000005*/]Microsoft.AspNetCore.Http.HttpContext/*0100001F*/ httpContext,
                                         string token) cil managed
  // SIG: 20 02 01 12 7D 0E
  {
    // Method begins at RVA 0x0
  } // end of method IAntiforgeryTokenStore::SaveCookieToken

} // end of class Microsoft.AspNetCore.Antiforgery.IAntiforgeryTokenStore

.class /*0200001D*/ interface private abstract auto ansi Microsoft.AspNetCore.Antiforgery.IClaimUidExtractor
{
  .custom /*0C000066:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000090*/ public hidebysig newslot abstract virtual 
          instance string  ExtractClaimUid(class [System.Security.Claims/*2300000B*/]System.Security.Claims.ClaimsPrincipal/*01000038*/ claimsPrincipal) cil managed
  // SIG: 20 01 0E 12 80 E1
  {
    .param [0]/*0800007C*/ 
    .custom /*0C0000A1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IClaimUidExtractor::ExtractClaimUid

} // end of class Microsoft.AspNetCore.Antiforgery.IClaimUidExtractor

.class /*0200001E*/ private abstract auto ansi sealed beforefieldinit Microsoft.AspNetCore.Antiforgery.Resources
       extends [System.Runtime/*23000001*/]System.Object/*01000017*/
{
  .method /*06000091*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100003F*/ 
          get_ResourceManager() cil managed
  // SIG: 00 00 12 80 FD
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_ResourceManager

  .method /*06000092*/ assembly hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000040*/ 
          get_Culture() cil managed
  // SIG: 00 00 12 81 01
  {
    .custom /*0C0000A3:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Culture

  .method /*06000093*/ assembly hidebysig specialname static 
          void  set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000040*/ 'value') cil managed
  // SIG: 00 01 01 12 81 01
  {
    .custom /*0C0000A4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000013*/::.ctor() /* 0A000012 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::set_Culture

  .method /*06000094*/ assembly hidebysig static 
          string  GetResourceString(string resourceKey,
                                    [opt] string defaultValue) cil managed aggressiveinlining
  // SIG: 00 02 0E 0E 0E
  {
    .param [2]/*08000080*/  = nullref
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::GetResourceString

  .method /*06000095*/ assembly hidebysig specialname static 
          string  get_AntiforgeryTokenValidator_AuthenticatedUserWithoutUsername() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_AntiforgeryTokenValidator_AuthenticatedUserWithoutUsername

  .method /*06000096*/ assembly hidebysig static 
          string  FormatAntiforgeryTokenValidator_AuthenticatedUserWithoutUsername(object p0,
                                                                                   object p1,
                                                                                   object p2,
                                                                                   object p3,
                                                                                   object p4,
                                                                                   object p5) cil managed
  // SIG: 00 06 0E 1C 1C 1C 1C 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatAntiforgeryTokenValidator_AuthenticatedUserWithoutUsername

  .method /*06000097*/ assembly hidebysig specialname static 
          string  get_AntiforgeryToken_AdditionalDataCheckFailed() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_AntiforgeryToken_AdditionalDataCheckFailed

  .method /*06000098*/ assembly hidebysig specialname static 
          string  get_AntiforgeryToken_ClaimUidMismatch() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_AntiforgeryToken_ClaimUidMismatch

  .method /*06000099*/ assembly hidebysig specialname static 
          string  get_AntiforgeryToken_DeserializationFailed() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_AntiforgeryToken_DeserializationFailed

  .method /*0600009A*/ assembly hidebysig specialname static 
          string  get_AntiforgeryToken_SecurityTokenMismatch() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_AntiforgeryToken_SecurityTokenMismatch

  .method /*0600009B*/ assembly hidebysig specialname static 
          string  get_AntiforgeryToken_TokensSwapped() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_AntiforgeryToken_TokensSwapped

  .method /*0600009C*/ assembly hidebysig specialname static 
          string  get_AntiforgeryToken_UnableToReadRequest() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_AntiforgeryToken_UnableToReadRequest

  .method /*0600009D*/ assembly hidebysig specialname static 
          string  get_AntiforgeryToken_UsernameMismatch() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_AntiforgeryToken_UsernameMismatch

  .method /*0600009E*/ assembly hidebysig static 
          string  FormatAntiforgeryToken_UsernameMismatch(object p0,
                                                          object p1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatAntiforgeryToken_UsernameMismatch

  .method /*0600009F*/ assembly hidebysig specialname static 
          string  get_Antiforgery_CookieToken_IsInvalid() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Antiforgery_CookieToken_IsInvalid

  .method /*060000A0*/ assembly hidebysig specialname static 
          string  get_Antiforgery_CookieToken_MustBeProvided() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Antiforgery_CookieToken_MustBeProvided

  .method /*060000A1*/ assembly hidebysig static 
          string  FormatAntiforgery_CookieToken_MustBeProvided(object p0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatAntiforgery_CookieToken_MustBeProvided

  .method /*060000A2*/ assembly hidebysig specialname static 
          string  get_Antiforgery_CookieToken_MustBeProvided_Generic() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Antiforgery_CookieToken_MustBeProvided_Generic

  .method /*060000A3*/ assembly hidebysig specialname static 
          string  get_Antiforgery_FormToken_MustBeProvided() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Antiforgery_FormToken_MustBeProvided

  .method /*060000A4*/ assembly hidebysig static 
          string  FormatAntiforgery_FormToken_MustBeProvided(object p0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatAntiforgery_FormToken_MustBeProvided

  .method /*060000A5*/ assembly hidebysig specialname static 
          string  get_Antiforgery_HeaderToken_MustBeProvided() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Antiforgery_HeaderToken_MustBeProvided

  .method /*060000A6*/ assembly hidebysig static 
          string  FormatAntiforgery_HeaderToken_MustBeProvided(object p0) cil managed
  // SIG: 00 01 0E 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatAntiforgery_HeaderToken_MustBeProvided

  .method /*060000A7*/ assembly hidebysig specialname static 
          string  get_Antiforgery_RequestToken_MustBeProvided() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Antiforgery_RequestToken_MustBeProvided

  .method /*060000A8*/ assembly hidebysig static 
          string  FormatAntiforgery_RequestToken_MustBeProvided(object p0,
                                                                object p1) cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatAntiforgery_RequestToken_MustBeProvided

  .method /*060000A9*/ assembly hidebysig specialname static 
          string  get_Antiforgery_RequestToken_MustBeProvided_Generic() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Antiforgery_RequestToken_MustBeProvided_Generic

  .method /*060000AA*/ assembly hidebysig specialname static 
          string  get_Antiforgery_RequiresSSL() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_Antiforgery_RequiresSSL

  .method /*060000AB*/ assembly hidebysig static 
          string  FormatAntiforgery_RequiresSSL(object optionName,
                                                object 'value') cil managed
  // SIG: 00 02 0E 1C 1C
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::FormatAntiforgery_RequiresSSL

  .method /*060000AC*/ assembly hidebysig specialname static 
          string  get_ArgumentCannotBeNullOrEmpty() cil managed
  // SIG: 00 00 0E
  {
    // Method begins at RVA 0x4050
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Resources::get_ArgumentCannotBeNullOrEmpty

  .property /*17000026*/ callconv(8) class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100003F*/
          ResourceManager()
  {
    .get class [System.Runtime/*23000001*/]System.Resources.ResourceManager/*0100003F*/ Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_ResourceManager() /* 06000091 */
  } // end of property Resources::ResourceManager
  .property /*17000027*/ callconv(8) class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000040*/
          Culture()
  {
    .get class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000040*/ Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Culture() /* 06000092 */
    .set void Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::set_Culture(class [System.Runtime/*23000001*/]System.Globalization.CultureInfo/*01000040*/) /* 06000093 */
  } // end of property Resources::Culture
  .property /*17000028*/ callconv(8) string
          AntiforgeryTokenValidator_AuthenticatedUserWithoutUsername()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_AntiforgeryTokenValidator_AuthenticatedUserWithoutUsername() /* 06000095 */
  } // end of property Resources::AntiforgeryTokenValidator_AuthenticatedUserWithoutUsername
  .property /*17000029*/ callconv(8) string
          AntiforgeryToken_AdditionalDataCheckFailed()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_AntiforgeryToken_AdditionalDataCheckFailed() /* 06000097 */
  } // end of property Resources::AntiforgeryToken_AdditionalDataCheckFailed
  .property /*1700002A*/ callconv(8) string
          AntiforgeryToken_ClaimUidMismatch()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_AntiforgeryToken_ClaimUidMismatch() /* 06000098 */
  } // end of property Resources::AntiforgeryToken_ClaimUidMismatch
  .property /*1700002B*/ callconv(8) string
          AntiforgeryToken_DeserializationFailed()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_AntiforgeryToken_DeserializationFailed() /* 06000099 */
  } // end of property Resources::AntiforgeryToken_DeserializationFailed
  .property /*1700002C*/ callconv(8) string
          AntiforgeryToken_SecurityTokenMismatch()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_AntiforgeryToken_SecurityTokenMismatch() /* 0600009A */
  } // end of property Resources::AntiforgeryToken_SecurityTokenMismatch
  .property /*1700002D*/ callconv(8) string
          AntiforgeryToken_TokensSwapped()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_AntiforgeryToken_TokensSwapped() /* 0600009B */
  } // end of property Resources::AntiforgeryToken_TokensSwapped
  .property /*1700002E*/ callconv(8) string
          AntiforgeryToken_UnableToReadRequest()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_AntiforgeryToken_UnableToReadRequest() /* 0600009C */
  } // end of property Resources::AntiforgeryToken_UnableToReadRequest
  .property /*1700002F*/ callconv(8) string
          AntiforgeryToken_UsernameMismatch()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_AntiforgeryToken_UsernameMismatch() /* 0600009D */
  } // end of property Resources::AntiforgeryToken_UsernameMismatch
  .property /*17000030*/ callconv(8) string
          Antiforgery_CookieToken_IsInvalid()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Antiforgery_CookieToken_IsInvalid() /* 0600009F */
  } // end of property Resources::Antiforgery_CookieToken_IsInvalid
  .property /*17000031*/ callconv(8) string
          Antiforgery_CookieToken_MustBeProvided()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Antiforgery_CookieToken_MustBeProvided() /* 060000A0 */
  } // end of property Resources::Antiforgery_CookieToken_MustBeProvided
  .property /*17000032*/ callconv(8) string
          Antiforgery_CookieToken_MustBeProvided_Generic()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Antiforgery_CookieToken_MustBeProvided_Generic() /* 060000A2 */
  } // end of property Resources::Antiforgery_CookieToken_MustBeProvided_Generic
  .property /*17000033*/ callconv(8) string
          Antiforgery_FormToken_MustBeProvided()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Antiforgery_FormToken_MustBeProvided() /* 060000A3 */
  } // end of property Resources::Antiforgery_FormToken_MustBeProvided
  .property /*17000034*/ callconv(8) string
          Antiforgery_HeaderToken_MustBeProvided()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Antiforgery_HeaderToken_MustBeProvided() /* 060000A5 */
  } // end of property Resources::Antiforgery_HeaderToken_MustBeProvided
  .property /*17000035*/ callconv(8) string
          Antiforgery_RequestToken_MustBeProvided()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Antiforgery_RequestToken_MustBeProvided() /* 060000A7 */
  } // end of property Resources::Antiforgery_RequestToken_MustBeProvided
  .property /*17000036*/ callconv(8) string
          Antiforgery_RequestToken_MustBeProvided_Generic()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Antiforgery_RequestToken_MustBeProvided_Generic() /* 060000A9 */
  } // end of property Resources::Antiforgery_RequestToken_MustBeProvided_Generic
  .property /*17000037*/ callconv(8) string
          Antiforgery_RequiresSSL()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_Antiforgery_RequiresSSL() /* 060000AA */
  } // end of property Resources::Antiforgery_RequiresSSL
  .property /*17000038*/ callconv(8) string
          ArgumentCannotBeNullOrEmpty()
  {
    .get string Microsoft.AspNetCore.Antiforgery.Resources/*0200001E*/::get_ArgumentCannotBeNullOrEmpty() /* 060000AC */
  } // end of property Resources::ArgumentCannotBeNullOrEmpty
} // end of class Microsoft.AspNetCore.Antiforgery.Resources


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
