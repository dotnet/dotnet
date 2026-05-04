


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
// Number of sections:         0x0003
// Time-date stamp:            0xddea7881
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00006200
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000081a6
// Base of code:                   0x00002000
// Base of data:                   0x0000a000
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
// Size of headers:                0x00000200
// Checksum:                       0x000138cc
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00008153 [0x0000004f] address [size] of Import Directory:          
// 0x0000a000 [0x00000474] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00006c00 [0x00002480] address [size] of Security Directory:        
// 0x0000c000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00008070 [0x00000054] address [size] of Debug Directory:           
// 0x00000000 [0x00000000] address [size] of Architecture Specific:     
// 0x00000000 [0x00000000] address [size] of Global Pointer:            
// 0x00000000 [0x00000000] address [size] of TLS Directory:             
// 0x00000000 [0x00000000] address [size] of Load Config Directory:     
// 0x00000000 [0x00000000] address [size] of Bound Import Directory:    
// 0x00002000 [0x00000008] address [size] of Import Address Table:      
// 0x00000000 [0x00000000] address [size] of Delay Load IAT:            
// 0x00002008 [0x00000048] address [size] of CLR Header:                


// Image sections:
//              .text
//              0x000061ac Virtual Size
//              0x00002000 Virtual Address
//              0x00006200 Size of Raw Data
//              0x00000200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x60000020 Characteristics
//                         CNT_CODE
//                         MEM_EXECUTE
//                         MEM_READ

//              .rsrc
//              0x00000474 Virtual Size
//              0x0000a000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00006400 Pointer to Raw Data
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
//              0x00006a00 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x000001a8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00008195 Import Name Table
//              0          Time Date Stamp
//              0          Index of First Forwarder Reference
//
//              0x0000  _CorDllMain

// Delay Load Import Address Table
// No data.

// Entry point code:
//FF 25 00 20 00 10 


// ----- CLR Header:
// Header size:                        0x00000048
// Major runtime version:              0x0002
// Minor runtime version:              0x0005
// 0x000024d8 [0x00005b18] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00007ff0 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x0000326c Size
//              '#~' Name
//    Stream 2:
//              0x000032d8 Offset
//              0x00001b8c Size
//              '#Strings' Name
//    Stream 3:
//              0x00004e64 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00004e68 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00004e78 Offset
//              0x00000ca0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000028903a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 36992
// PE header size       : 512 (496 used)    ( 1.38%)
// PE additional info   : 10739             (29.03%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.19%)
// CLR meta-data size  : 23320              (63.04%)
// CLR additional info : 128                ( 0.35%)
// CLR method headers  : 356                ( 0.96%)
// Managed code         : 801               ( 2.17%)
// Data                 : 2048              ( 5.54%)
// Unaccounted          : -984              (-2.66%)

// Num.of PE sections   : 3
//   .text    - 25088
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 23320
//   Module        -    1 (10 bytes)
//   TypeDef       -   31 (434 bytes)     3 interfaces, 0 explicit layout
//   TypeRef       -   62 (372 bytes)
//   MethodDef     -  366 (5124 bytes)    6 abstract, 0 native, 356 bodies
//   FieldDef      -  101 (606 bytes)     0 constant
//   MemberRef     -   29 (174 bytes)
//   ParamDef      -  258 (1548 bytes)
//   MethodImpl    -    8 (48 bytes)
//   Constant      -   94 (564 bytes)
//   CustomAttribute-  219 (1314 bytes)
//   InterfaceImpl -   10 (40 bytes)
//   PropertyMap   -   14 (56 bytes)
//   Property      -  148 (888 bytes)
//   MethodSemantic-  238 (1428 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    7 (140 bytes)
//   ExportedType  -    2 (28 bytes)
//   NestedClass   -    3 (12 bytes)
//   Strings       -  7049 bytes
//   Blobs         -  3232 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   211 bytes

// CLR additional info : 128

// CLR method headers : 356
//   Num.of method bodies  - 356
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 356

// Managed code : 801
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class AuthenticationManager          (public) (auto) (ansi) 
// Class Authorization                  (public) (auto) (ansi) 
// Class FileWebRequest                 (public) (auto) (ansi) 
// Class FileWebResponse                (public) (auto) (ansi) 
// Class FtpStatusCode                  (public) (auto) (ansi) (sealed) 
// Class FtpWebRequest                  (public) (auto) (ansi) (sealed) 
// Class FtpWebResponse                 (public) (auto) (ansi) 
// Class GlobalProxySelection           (public) (auto) (ansi) 
// Class HttpContinueDelegate           (public) (auto) (ansi) (sealed) 
// Class HttpWebRequest                 (public) (auto) (ansi) 
// Class HttpWebResponse                (public) (auto) (ansi) 
// Interface IAuthenticationModule          (public) (abstract) (auto) (ansi) 
// Interface ICredentialPolicy              (public) (abstract) (auto) (ansi) 
// Interface IWebRequestCreate              (public) (abstract) (auto) (ansi) 
// Class ProtocolViolationException     (public) (auto) (ansi) 
// Class WebException                   (public) (auto) (ansi) 
// Class WebExceptionStatus             (public) (auto) (ansi) (sealed) 
// Class WebRequest                     (public) (abstract) (auto) (ansi) 
// Class WebRequestMethods              (public) (abstract) (auto) (ansi) (sealed) 
// Class WebResponse                    (public) (abstract) (auto) (ansi) 
// Class HttpCacheAgeControl            (public) (auto) (ansi) (sealed) 
// Class HttpRequestCacheLevel          (public) (auto) (ansi) (sealed) 
// Class HttpRequestCachePolicy         (public) (auto) (ansi) 
// Class File                           (abstract) (auto) (ansi) (sealed) (nested public) 
// Class Ftp                            (abstract) (auto) (ansi) (sealed) (nested public) 
// Class Http                           (abstract) (auto) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Collections.Specialized
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Net.WebHeaderCollection
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ System.Security.Cryptography.X509Certificates
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ System.Net.ServicePoint
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly extern /*23000007*/ System.Net.Security
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.Requests
{
  .custom instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute::.ctor(string,
                                                                                                       string) = ( 01 00 06 73 6F 75 72 63 65 1F 73 6F 75 72 63 65   // ...source.source
                                                                                                                   2D 62 75 69 6C 64 2D 72 65 66 65 72 65 6E 63 65   // -build-reference
                                                                                                                   2D 70 61 63 6B 61 67 65 73 00 00 )                // -packages..
  .custom /*0C000004:0A000001*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilationRelaxationsAttribute/*01000001*/::.ctor(int32) /* 0A000001 */ = ( 01 00 08 00 00 00 00 00 ) 
  .custom /*0C000005:0A000002*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute/*01000002*/::.ctor() /* 0A000002 */ = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                                                                                                63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom /*0C000006:0A000003*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*/::.ctor(valuetype [System.Runtime/*23000001*/]System.Diagnostics.DebuggableAttribute/*01000003*//DebuggingModes/*01000004*/) /* 0A000003 */ = ( 01 00 02 00 00 00 00 00 ) 

  .custom /*0C000007:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 52 65   // ...System.Net.Re
                                                                                                                                                                        71 75 65 73 74 73 00 00 )                         // quests..
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 52 65   // ...System.Net.Re
                                                                                                                                                                       71 75 65 73 74 73 00 00 )                         // quests..
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 52 65   // ...System.Net.Re
                                                                                                                                                                 71 75 65 73 74 73 00 00 )                         // quests..
  .custom /*0C000017:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                07 D1 FA 57 C4 AE D9 F0 A3 2E 84 AA 0F AE FD 0D   // ...W............
                E9 E8 FD 6A EC 8F 87 FB 03 76 6C 83 4C 99 92 1E   // ...j.....vl.L...
                B2 3B E7 9A D9 D5 DC C1 DD 9A D2 36 13 21 02 90   // .;.........6.!..
                0B 72 3C F9 80 95 7F C4 E1 77 10 8F C6 07 77 4F   // .r<......w....wO
                29 E8 32 0E 92 EA 05 EC E4 E8 21 C0 A5 EF E8 F1   // ).2.......!.....
                64 5C 4C 0C 93 C1 AB 99 28 5D 62 2C AA 65 2C 1D   // d\L.....(]b,.e,.
                FA D6 3D 74 5D 6F 2D E5 F1 7E 5E AF 0F C4 96 3D   // ..=t]o-..~^....=
                26 1C 8A 12 43 65 18 20 6D C0 93 34 4D 5A D2 93 ) // &...Ce. m..4MZ..
  .hash algorithm 0x00008004
  .ver 6:0:0:0
}
.class extern /*27000001*/ forwarder System.Net.HttpRequestHeader
{
  .assembly extern System.Net.WebHeaderCollection /*23000004*/ 
}
.class extern /*27000002*/ forwarder System.Net.WebHeaderCollection
{
  .assembly extern System.Net.WebHeaderCollection /*23000004*/ 
}
.module System.Net.Requests.dll
// MVID: {db95821d-7333-48c9-95bd-2d62cb7b691d}
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
  .custom /*0C000018:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000019:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001C:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001D:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000001*/ public initonly uint8[] NullableFlags
  .method /*06000002*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x2058
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)00003E       */ newarr     [System.Runtime/*23000001*/]System.Byte/*0100003E*/
    IL_000d:  /* 25   |                  */ dup
    IL_000e:  /* 16   |                  */ ldc.i4.0
    IL_000f:  /* 03   |                  */ ldarg.1
    IL_0010:  /* 9C   |                  */ stelem.i1
    IL_0011:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_0016:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] A_1) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2070
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000020:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000002*/ public initonly uint8 Flag
  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8 A_1) cil managed
  // SIG: 20 01 01 05
  {
    // Method begins at RVA 0x207f
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000024:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000025:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
                                                                                                                                                                                                                          4D 75 6C 74 69 70 6C 65 00 54 02 09 49 6E 68 65   // Multiple.T..Inhe
                                                                                                                                                                                                                          72 69 74 65 64 00 )                               // rited.
  .field /*04000003*/ public initonly bool IncludesInternals
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(bool A_1) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x208e
    // Code size       14 (0xe)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public auto ansi beforefieldinit System.Net.AuthenticationManager
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000028:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000029:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AuthenticationManager::.ctor

  .method /*06000007*/ public hidebysig specialname static 
          class System.Net.ICredentialPolicy/*02000012*/ 
          get_CredentialPolicy() cil managed
  // SIG: 00 00 12 48
  {
    .custom /*0C00002B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationManager::get_CredentialPolicy

  .method /*06000008*/ public hidebysig specialname static 
          void  set_CredentialPolicy(class System.Net.ICredentialPolicy/*02000012*/ 'value') cil managed
  // SIG: 00 01 01 12 48
  {
    .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AuthenticationManager::set_CredentialPolicy

  .method /*06000009*/ public hidebysig specialname static 
          class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/ 
          get_CustomTargetNameDictionary() cil managed
  // SIG: 00 00 12 5D
  {
    // Method begins at RVA 0x20aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationManager::get_CustomTargetNameDictionary

  .method /*0600000A*/ public hidebysig specialname static 
          class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000018*/ 
          get_RegisteredModules() cil managed
  // SIG: 00 00 12 61
  {
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationManager::get_RegisteredModules

  .method /*0600000B*/ public hidebysig static 
          class System.Net.Authorization/*02000007*/ 
          Authenticate(string challenge,
                       class System.Net.WebRequest/*02000017*/ 'request',
                       class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ credentials) cil managed
  // SIG: 00 03 12 1C 0E 12 5C 12 69
  {
    .custom /*0C000039:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 7D 54 68 65 20 41 75 74 68 65 6E 74 69 63   // ..}The Authentic
                                                                                                                                                   61 74 69 6F 6E 4D 61 6E 61 67 65 72 20 41 75 74   // ationManager Aut
                                                                                                                                                   68 65 6E 74 69 63 61 74 65 20 61 6E 64 20 50 72   // henticate and Pr
                                                                                                                                                   65 41 75 74 68 65 6E 74 69 63 61 74 65 20 6D 65   // eAuthenticate me
                                                                                                                                                   74 68 6F 64 73 20 61 72 65 20 6E 6F 74 20 73 75   // thods are not su
                                                                                                                                                   70 70 6F 72 74 65 64 20 61 6E 64 20 74 68 72 6F   // pported and thro
                                                                                                                                                   77 20 50 6C 61 74 66 6F 72 6D 4E 6F 74 53 75 70   // w PlatformNotSup
                                                                                                                                                   70 6F 72 74 65 64 45 78 63 65 70 74 69 6F 6E 2E   // portedException.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 30 39 54 0E 09 55   // d.SYSLIB0009T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    .param [0]/*08000002*/ 
    .custom /*0C00001A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationManager::Authenticate

  .method /*0600000C*/ public hidebysig static 
          class System.Net.Authorization/*02000007*/ 
          PreAuthenticate(class System.Net.WebRequest/*02000017*/ 'request',
                          class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ credentials) cil managed
  // SIG: 00 02 12 1C 12 5C 12 69
  {
    .custom /*0C00003D:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 7D 54 68 65 20 41 75 74 68 65 6E 74 69 63   // ..}The Authentic
                                                                                                                                                   61 74 69 6F 6E 4D 61 6E 61 67 65 72 20 41 75 74   // ationManager Aut
                                                                                                                                                   68 65 6E 74 69 63 61 74 65 20 61 6E 64 20 50 72   // henticate and Pr
                                                                                                                                                   65 41 75 74 68 65 6E 74 69 63 61 74 65 20 6D 65   // eAuthenticate me
                                                                                                                                                   74 68 6F 64 73 20 61 72 65 20 6E 6F 74 20 73 75   // thods are not su
                                                                                                                                                   70 70 6F 72 74 65 64 20 61 6E 64 20 74 68 72 6F   // pported and thro
                                                                                                                                                   77 20 50 6C 61 74 66 6F 72 6D 4E 6F 74 53 75 70   // w PlatformNotSup
                                                                                                                                                   70 6F 72 74 65 64 45 78 63 65 70 74 69 6F 6E 2E   // portedException.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 30 39 54 0E 09 55   // d.SYSLIB0009T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    .param [0]/*08000006*/ 
    .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticationManager::PreAuthenticate

  .method /*0600000D*/ public hidebysig static 
          void  Register(class System.Net.IAuthenticationModule/*02000011*/ authenticationModule) cil managed
  // SIG: 00 01 01 12 44
  {
    // Method begins at RVA 0x20b6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AuthenticationManager::Register

  .method /*0600000E*/ public hidebysig static 
          void  Unregister(class System.Net.IAuthenticationModule/*02000011*/ authenticationModule) cil managed
  // SIG: 00 01 01 12 44
  {
    // Method begins at RVA 0x20b8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AuthenticationManager::Unregister

  .method /*0600000F*/ public hidebysig static 
          void  Unregister(string authenticationScheme) cil managed
  // SIG: 00 01 01 0E
  {
    // Method begins at RVA 0x20ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AuthenticationManager::Unregister

  .property /*17000001*/ callconv(8) class System.Net.ICredentialPolicy/*02000012*/
          CredentialPolicy()
  {
    .custom /*0C000003:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get class System.Net.ICredentialPolicy/*02000012*/ System.Net.AuthenticationManager/*02000006*/::get_CredentialPolicy() /* 06000007 */
    .set void System.Net.AuthenticationManager/*02000006*/::set_CredentialPolicy(class System.Net.ICredentialPolicy/*02000012*/) /* 06000008 */
  } // end of property AuthenticationManager::CredentialPolicy
  .property /*17000002*/ callconv(8) class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/
          CustomTargetNameDictionary()
  {
    .get class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/ System.Net.AuthenticationManager/*02000006*/::get_CustomTargetNameDictionary() /* 06000009 */
  } // end of property AuthenticationManager::CustomTargetNameDictionary
  .property /*17000003*/ callconv(8) class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000018*/
          RegisteredModules()
  {
    .get class [System.Runtime/*23000001*/]System.Collections.IEnumerator/*01000018*/ System.Net.AuthenticationManager/*02000006*/::get_RegisteredModules() /* 0600000A */
  } // end of property AuthenticationManager::RegisteredModules
} // end of class System.Net.AuthenticationManager

.class /*02000007*/ public auto ansi beforefieldinit System.Net.Authorization
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000010*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string token) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20bc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Authorization::.ctor

  .method /*06000011*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string token,
                               bool finished) cil managed
  // SIG: 20 02 01 0E 02
  {
    // Method begins at RVA 0x20c4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Authorization::.ctor

  .method /*06000012*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string token,
                               bool finished,
                               string connectionGroupId) cil managed
  // SIG: 20 03 01 0E 02 0E
  {
    // Method begins at RVA 0x20cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Authorization::.ctor

  .method /*06000013*/ public hidebysig specialname 
          instance bool  get_Complete() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Authorization::get_Complete

  .method /*06000014*/ public hidebysig specialname 
          instance string  get_ConnectionGroupId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Authorization::get_ConnectionGroupId

  .method /*06000015*/ public hidebysig specialname 
          instance string  get_Message() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Authorization::get_Message

  .method /*06000016*/ public hidebysig specialname 
          instance bool  get_MutuallyAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Authorization::get_MutuallyAuthenticated

  .method /*06000017*/ public hidebysig specialname 
          instance void  set_MutuallyAuthenticated(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Authorization::set_MutuallyAuthenticated

  .method /*06000018*/ public hidebysig specialname 
          instance string[]  get_ProtectionRealm() cil managed
  // SIG: 20 00 1D 0E
  {
    .param [0]/*08000013*/ 
    .custom /*0C00004C:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Authorization::get_ProtectionRealm

  .method /*06000019*/ public hidebysig specialname 
          instance void  set_ProtectionRealm(string[] 'value') cil managed
  // SIG: 20 01 01 1D 0E
  {
    .param [1]/*08000014*/ 
    .custom /*0C00004D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    // Method begins at RVA 0x20e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Authorization::set_ProtectionRealm

  .property /*17000004*/ instance callconv(8) bool
          Complete()
  {
    .get instance bool System.Net.Authorization/*02000007*/::get_Complete() /* 06000013 */
  } // end of property Authorization::Complete
  .property /*17000005*/ instance callconv(8) string
          ConnectionGroupId()
  {
    .get instance string System.Net.Authorization/*02000007*/::get_ConnectionGroupId() /* 06000014 */
  } // end of property Authorization::ConnectionGroupId
  .property /*17000006*/ instance callconv(8) string
          Message()
  {
    .get instance string System.Net.Authorization/*02000007*/::get_Message() /* 06000015 */
  } // end of property Authorization::Message
  .property /*17000007*/ instance callconv(8) bool
          MutuallyAuthenticated()
  {
    .get instance bool System.Net.Authorization/*02000007*/::get_MutuallyAuthenticated() /* 06000016 */
    .set instance void System.Net.Authorization/*02000007*/::set_MutuallyAuthenticated(bool) /* 06000017 */
  } // end of property Authorization::MutuallyAuthenticated
  .property /*17000008*/ instance callconv(8) string[]
          ProtectionRealm()
  {
    .custom /*0C000033:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 02 01 00 00 ) 
    .get instance string[] System.Net.Authorization/*02000007*/::get_ProtectionRealm() /* 06000018 */
    .set instance void System.Net.Authorization/*02000007*/::set_ProtectionRealm(string[]) /* 06000019 */
  } // end of property Authorization::ProtectionRealm
} // end of class System.Net.Authorization

.class /*02000008*/ public auto ansi beforefieldinit System.Net.FileWebRequest
       extends System.Net.WebRequest/*02000017*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
{
  .custom /*0C000030:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000031:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C000057:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 35 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E   // ..5Serialization
                                                                                                                                                   20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63   //  has been deprec
                                                                                                                                                   61 74 65 64 20 66 6F 72 20 46 69 6C 65 57 65 62   // ated for FileWeb
                                                                                                                                                   52 65 71 75 65 73 74 2E 00 00 )                   // Request...
    // Method begins at RVA 0x20e7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00011F       */ call       instance void System.Net.WebRequest/*02000017*/::.ctor() /* 0600011F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::.ctor

  .method /*0600001B*/ public hidebysig specialname virtual 
          instance string  get_ConnectionGroupName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000058:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_ConnectionGroupName

  .method /*0600001C*/ public hidebysig specialname virtual 
          instance void  set_ConnectionGroupName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000059:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_ConnectionGroupName

  .method /*0600001D*/ public hidebysig specialname virtual 
          instance int64  get_ContentLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_ContentLength

  .method /*0600001E*/ public hidebysig specialname virtual 
          instance void  set_ContentLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x20f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_ContentLength

  .method /*0600001F*/ public hidebysig specialname virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_ContentType

  .method /*06000020*/ public hidebysig specialname virtual 
          instance void  set_ContentType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_ContentType

  .method /*06000021*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 69
  {
    .custom /*0C000060:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_Credentials

  .method /*06000022*/ public hidebysig specialname virtual 
          instance void  set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ 'value') cil managed
  // SIG: 20 01 01 12 69
  {
    .custom /*0C000062:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2101
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_Credentials

  .method /*06000023*/ public hidebysig specialname virtual 
          instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_Headers

  .method /*06000024*/ public hidebysig specialname virtual 
          instance string  get_Method() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_Method

  .method /*06000025*/ public hidebysig specialname virtual 
          instance void  set_Method(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2109
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_Method

  .method /*06000026*/ public hidebysig specialname virtual 
          instance bool  get_PreAuthenticate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_PreAuthenticate

  .method /*06000027*/ public hidebysig specialname virtual 
          instance void  set_PreAuthenticate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x210e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_PreAuthenticate

  .method /*06000028*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 
          get_Proxy() cil managed
  // SIG: 20 00 12 7D
  {
    .custom /*0C000065:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2110
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_Proxy

  .method /*06000029*/ public hidebysig specialname virtual 
          instance void  set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 'value') cil managed
  // SIG: 20 01 01 12 7D
  {
    .custom /*0C000067:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2113
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_Proxy

  .method /*0600002A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_RequestUri() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x2115
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_RequestUri

  .method /*0600002B*/ public hidebysig specialname virtual 
          instance int32  get_Timeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_Timeout

  .method /*0600002C*/ public hidebysig specialname virtual 
          instance void  set_Timeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x211b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_Timeout

  .method /*0600002D*/ public hidebysig specialname virtual 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::get_UseDefaultCredentials

  .method /*0600002E*/ public hidebysig specialname virtual 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2120
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::set_UseDefaultCredentials

  .method /*0600002F*/ public hidebysig virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2122
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::Abort

  .method /*06000030*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginGetRequestStream(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                                object state) cil managed
  // SIG: 20 02 12 80 85 12 80 89 1C
  {
    .custom /*0C000069:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000020*/ 
    .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2124
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::BeginGetRequestStream

  .method /*06000031*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginGetResponse(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                           object state) cil managed
  // SIG: 20 02 12 80 85 12 80 89 1C
  {
    .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000023*/ 
    .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2127
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::BeginGetResponse

  .method /*06000032*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          EndGetRequestStream(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult) cil managed
  // SIG: 20 01 12 80 8D 12 80 85
  {
    // Method begins at RVA 0x212a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::EndGetRequestStream

  .method /*06000033*/ public hidebysig virtual 
          instance class System.Net.WebResponse/*02000019*/ 
          EndGetResponse(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult) cil managed
  // SIG: 20 01 12 64 12 80 85
  {
    // Method begins at RVA 0x212d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::EndGetResponse

  .method /*06000034*/ family hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    // Method begins at RVA 0x2130
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::GetObjectData

  .method /*06000035*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetRequestStream() cil managed
  // SIG: 20 00 12 80 8D
  {
    // Method begins at RVA 0x2132
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::GetRequestStream

  .method /*06000036*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/> 
          GetRequestStreamAsync() cil managed
  // SIG: 20 00 15 12 80 91 01 12 80 8D
  {
    // Method begins at RVA 0x2135
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::GetRequestStreamAsync

  .method /*06000037*/ public hidebysig virtual 
          instance class System.Net.WebResponse/*02000019*/ 
          GetResponse() cil managed
  // SIG: 20 00 12 64
  {
    // Method begins at RVA 0x2138
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::GetResponse

  .method /*06000038*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<class System.Net.WebResponse/*02000019*/> 
          GetResponseAsync() cil managed
  // SIG: 20 00 15 12 80 91 01 12 64
  {
    // Method begins at RVA 0x213b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebRequest::GetResponseAsync

  .method /*06000039*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/::GetObjectData /*0100001B::0A000014*/ 
    // Method begins at RVA 0x213e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebRequest::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000009*/ instance callconv(8) string
          ConnectionGroupName()
  {
    .custom /*0C000037:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.FileWebRequest/*02000008*/::get_ConnectionGroupName() /* 0600001B */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_ConnectionGroupName(string) /* 0600001C */
  } // end of property FileWebRequest::ConnectionGroupName
  .property /*1700000A*/ instance callconv(8) int64
          ContentLength()
  {
    .get instance int64 System.Net.FileWebRequest/*02000008*/::get_ContentLength() /* 0600001D */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_ContentLength(int64) /* 0600001E */
  } // end of property FileWebRequest::ContentLength
  .property /*1700000B*/ instance callconv(8) string
          ContentType()
  {
    .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.FileWebRequest/*02000008*/::get_ContentType() /* 0600001F */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_ContentType(string) /* 06000020 */
  } // end of property FileWebRequest::ContentType
  .property /*1700000C*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/
          Credentials()
  {
    .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ System.Net.FileWebRequest/*02000008*/::get_Credentials() /* 06000021 */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/) /* 06000022 */
  } // end of property FileWebRequest::Credentials
  .property /*1700000D*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/
          Headers()
  {
    .get instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ System.Net.FileWebRequest/*02000008*/::get_Headers() /* 06000023 */
  } // end of property FileWebRequest::Headers
  .property /*1700000E*/ instance callconv(8) string
          Method()
  {
    .get instance string System.Net.FileWebRequest/*02000008*/::get_Method() /* 06000024 */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_Method(string) /* 06000025 */
  } // end of property FileWebRequest::Method
  .property /*1700000F*/ instance callconv(8) bool
          PreAuthenticate()
  {
    .get instance bool System.Net.FileWebRequest/*02000008*/::get_PreAuthenticate() /* 06000026 */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_PreAuthenticate(bool) /* 06000027 */
  } // end of property FileWebRequest::PreAuthenticate
  .property /*17000010*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/
          Proxy()
  {
    .custom /*0C000048:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ System.Net.FileWebRequest/*02000008*/::get_Proxy() /* 06000028 */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/) /* 06000029 */
  } // end of property FileWebRequest::Proxy
  .property /*17000011*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          RequestUri()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.FileWebRequest/*02000008*/::get_RequestUri() /* 0600002A */
  } // end of property FileWebRequest::RequestUri
  .property /*17000012*/ instance callconv(8) int32
          Timeout()
  {
    .get instance int32 System.Net.FileWebRequest/*02000008*/::get_Timeout() /* 0600002B */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_Timeout(int32) /* 0600002C */
  } // end of property FileWebRequest::Timeout
  .property /*17000013*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .get instance bool System.Net.FileWebRequest/*02000008*/::get_UseDefaultCredentials() /* 0600002D */
    .set instance void System.Net.FileWebRequest/*02000008*/::set_UseDefaultCredentials(bool) /* 0600002E */
  } // end of property FileWebRequest::UseDefaultCredentials
} // end of class System.Net.FileWebRequest

.class /*02000009*/ public auto ansi beforefieldinit System.Net.FileWebResponse
       extends System.Net.WebResponse/*02000019*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
  .custom /*0C00001B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600003A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C00006E:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 36 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E   // ..6Serialization
                                                                                                                                                   20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63   //  has been deprec
                                                                                                                                                   61 74 65 64 20 66 6F 72 20 46 69 6C 65 57 65 62   // ated for FileWeb
                                                                                                                                                   52 65 73 70 6F 6E 73 65 2E 00 00 )                // Response...
    // Method begins at RVA 0x2140
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000152       */ call       instance void System.Net.WebResponse/*02000019*/::.ctor() /* 06000152 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FileWebResponse::.ctor

  .method /*0600003B*/ public hidebysig specialname virtual 
          instance int64  get_ContentLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2148
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebResponse::get_ContentLength

  .method /*0600003C*/ public hidebysig specialname virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x214b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebResponse::get_ContentType

  .method /*0600003D*/ public hidebysig specialname virtual 
          instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x214e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebResponse::get_Headers

  .method /*0600003E*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_ResponseUri() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x2151
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebResponse::get_ResponseUri

  .method /*0600003F*/ public hidebysig specialname virtual 
          instance bool  get_SupportsHeaders() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2154
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebResponse::get_SupportsHeaders

  .method /*06000040*/ public hidebysig virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2157
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebResponse::Close

  .method /*06000041*/ family hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    // Method begins at RVA 0x2159
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebResponse::GetObjectData

  .method /*06000042*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetResponseStream() cil managed
  // SIG: 20 00 12 80 8D
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FileWebResponse::GetResponseStream

  .method /*06000043*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/::GetObjectData /*0100001B::0A000014*/ 
    // Method begins at RVA 0x215e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FileWebResponse::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000014*/ instance callconv(8) int64
          ContentLength()
  {
    .get instance int64 System.Net.FileWebResponse/*02000009*/::get_ContentLength() /* 0600003B */
  } // end of property FileWebResponse::ContentLength
  .property /*17000015*/ instance callconv(8) string
          ContentType()
  {
    .get instance string System.Net.FileWebResponse/*02000009*/::get_ContentType() /* 0600003C */
  } // end of property FileWebResponse::ContentType
  .property /*17000016*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/
          Headers()
  {
    .get instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ System.Net.FileWebResponse/*02000009*/::get_Headers() /* 0600003D */
  } // end of property FileWebResponse::Headers
  .property /*17000017*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          ResponseUri()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.FileWebResponse/*02000009*/::get_ResponseUri() /* 0600003E */
  } // end of property FileWebResponse::ResponseUri
  .property /*17000018*/ instance callconv(8) bool
          SupportsHeaders()
  {
    .get instance bool System.Net.FileWebResponse/*02000009*/::get_SupportsHeaders() /* 0600003F */
  } // end of property FileWebResponse::SupportsHeaders
} // end of class System.Net.FileWebResponse

.class /*0200000A*/ public auto ansi sealed System.Net.FtpStatusCode
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*04000004*/ public specialname rtspecialname int32 value__
  .field /*04000005*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ Undefined = int32(0x00000000)
  .field /*04000006*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ RestartMarker = int32(0x0000006E)
  .field /*04000007*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ServiceTemporarilyNotAvailable = int32(0x00000078)
  .field /*04000008*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ DataAlreadyOpen = int32(0x0000007D)
  .field /*04000009*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ OpeningData = int32(0x00000096)
  .field /*0400000A*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ CommandOK = int32(0x000000C8)
  .field /*0400000B*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ CommandExtraneous = int32(0x000000CA)
  .field /*0400000C*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ DirectoryStatus = int32(0x000000D4)
  .field /*0400000D*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ FileStatus = int32(0x000000D5)
  .field /*0400000E*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ SystemType = int32(0x000000D7)
  .field /*0400000F*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ SendUserCommand = int32(0x000000DC)
  .field /*04000010*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ClosingControl = int32(0x000000DD)
  .field /*04000011*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ClosingData = int32(0x000000E2)
  .field /*04000012*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ EnteringPassive = int32(0x000000E3)
  .field /*04000013*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ LoggedInProceed = int32(0x000000E6)
  .field /*04000014*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ServerWantsSecureSession = int32(0x000000EA)
  .field /*04000015*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ FileActionOK = int32(0x000000FA)
  .field /*04000016*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ PathnameCreated = int32(0x00000101)
  .field /*04000017*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ SendPasswordCommand = int32(0x0000014B)
  .field /*04000018*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ NeedLoginAccount = int32(0x0000014C)
  .field /*04000019*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ FileCommandPending = int32(0x0000015E)
  .field /*0400001A*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ServiceNotAvailable = int32(0x000001A5)
  .field /*0400001B*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ CantOpenData = int32(0x000001A9)
  .field /*0400001C*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ConnectionClosed = int32(0x000001AA)
  .field /*0400001D*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ActionNotTakenFileUnavailableOrBusy = int32(0x000001C2)
  .field /*0400001E*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ActionAbortedLocalProcessingError = int32(0x000001C3)
  .field /*0400001F*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ActionNotTakenInsufficientSpace = int32(0x000001C4)
  .field /*04000020*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ CommandSyntaxError = int32(0x000001F4)
  .field /*04000021*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ArgumentSyntaxError = int32(0x000001F5)
  .field /*04000022*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ CommandNotImplemented = int32(0x000001F6)
  .field /*04000023*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ BadCommandSequence = int32(0x000001F7)
  .field /*04000024*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ NotLoggedIn = int32(0x00000212)
  .field /*04000025*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ AccountNeeded = int32(0x00000214)
  .field /*04000026*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ActionNotTakenFileUnavailable = int32(0x00000226)
  .field /*04000027*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ActionAbortedUnknownPageType = int32(0x00000227)
  .field /*04000028*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ FileActionAborted = int32(0x00000228)
  .field /*04000029*/ public static literal valuetype System.Net.FtpStatusCode/*0200000A*/ ActionNotTakenFilenameNotAllowed = int32(0x00000229)
} // end of class System.Net.FtpStatusCode

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.Net.FtpWebRequest
       extends System.Net.WebRequest/*02000017*/
{
  .custom /*0C00003A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000044*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2160
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00011F       */ call       instance void System.Net.WebRequest/*02000017*/::.ctor() /* 0600011F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::.ctor

  .method /*06000045*/ public hidebysig specialname 
          instance class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/ 
          get_ClientCertificates() cil managed
  // SIG: 20 00 12 80 99
  {
    .custom /*0C000072:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2168
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_ClientCertificates

  .method /*06000046*/ public hidebysig specialname 
          instance void  set_ClientCertificates(class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 12 80 99
  {
    .custom /*0C000074:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x216b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_ClientCertificates

  .method /*06000047*/ public hidebysig specialname virtual 
          instance string  get_ConnectionGroupName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_ConnectionGroupName

  .method /*06000048*/ public hidebysig specialname virtual 
          instance void  set_ConnectionGroupName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2170
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_ConnectionGroupName

  .method /*06000049*/ public hidebysig specialname virtual 
          instance int64  get_ContentLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2172
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_ContentLength

  .method /*0600004A*/ public hidebysig specialname virtual 
          instance void  set_ContentLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2175
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_ContentLength

  .method /*0600004B*/ public hidebysig specialname 
          instance int64  get_ContentOffset() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x2177
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_ContentOffset

  .method /*0600004C*/ public hidebysig specialname 
          instance void  set_ContentOffset(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x217a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_ContentOffset

  .method /*0600004D*/ public hidebysig specialname virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_ContentType

  .method /*0600004E*/ public hidebysig specialname virtual 
          instance void  set_ContentType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x217f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_ContentType

  .method /*0600004F*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x2181
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_Credentials

  .method /*06000050*/ public hidebysig specialname virtual 
          instance void  set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ 'value') cil managed
  // SIG: 20 01 01 12 69
  {
    .param [1]/*08000037*/ 
    .custom /*0C00006D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000027*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2184
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_Credentials

  .method /*06000051*/ public hidebysig specialname static 
          class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ 
          get_DefaultCachePolicy() cil managed
  // SIG: 00 00 12 80 A1
  {
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_DefaultCachePolicy

  .method /*06000052*/ public hidebysig specialname static 
          void  set_DefaultCachePolicy(class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ 'value') cil managed
  // SIG: 00 01 01 12 80 A1
  {
    // Method begins at RVA 0x2189
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_DefaultCachePolicy

  .method /*06000053*/ public hidebysig specialname 
          instance bool  get_EnableSsl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x218b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_EnableSsl

  .method /*06000054*/ public hidebysig specialname 
          instance void  set_EnableSsl(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x218e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_EnableSsl

  .method /*06000055*/ public hidebysig specialname virtual 
          instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C00007A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2190
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_Headers

  .method /*06000056*/ public hidebysig specialname virtual 
          instance void  set_Headers(class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C00007C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2193
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_Headers

  .method /*06000057*/ public hidebysig specialname 
          instance bool  get_KeepAlive() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2195
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_KeepAlive

  .method /*06000058*/ public hidebysig specialname 
          instance void  set_KeepAlive(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2198
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_KeepAlive

  .method /*06000059*/ public hidebysig specialname virtual 
          instance string  get_Method() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_Method

  .method /*0600005A*/ public hidebysig specialname virtual 
          instance void  set_Method(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x219d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_Method

  .method /*0600005B*/ public hidebysig specialname virtual 
          instance bool  get_PreAuthenticate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_PreAuthenticate

  .method /*0600005C*/ public hidebysig specialname virtual 
          instance void  set_PreAuthenticate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21a2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_PreAuthenticate

  .method /*0600005D*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 
          get_Proxy() cil managed
  // SIG: 20 00 12 7D
  {
    // Method begins at RVA 0x21a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_Proxy

  .method /*0600005E*/ public hidebysig specialname virtual 
          instance void  set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 'value') cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x21a7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_Proxy

  .method /*0600005F*/ public hidebysig specialname 
          instance int32  get_ReadWriteTimeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21a9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_ReadWriteTimeout

  .method /*06000060*/ public hidebysig specialname 
          instance void  set_ReadWriteTimeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21ac
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_ReadWriteTimeout

  .method /*06000061*/ public hidebysig specialname 
          instance string  get_RenameTo() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_RenameTo

  .method /*06000062*/ public hidebysig specialname 
          instance void  set_RenameTo(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*08000040*/ 
    .custom /*0C000071:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000027*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21b1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_RenameTo

  .method /*06000063*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_RequestUri() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_RequestUri

  .method /*06000064*/ public hidebysig specialname 
          instance class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*01000029*/ 
          get_ServicePoint() cil managed
  // SIG: 20 00 12 80 A5
  {
    .custom /*0C000083:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_ServicePoint

  .method /*06000065*/ public hidebysig specialname virtual 
          instance int32  get_Timeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_Timeout

  .method /*06000066*/ public hidebysig specialname virtual 
          instance void  set_Timeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21bc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_Timeout

  .method /*06000067*/ public hidebysig specialname 
          instance bool  get_UseBinary() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_UseBinary

  .method /*06000068*/ public hidebysig specialname 
          instance void  set_UseBinary(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21c1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_UseBinary

  .method /*06000069*/ public hidebysig specialname virtual 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_UseDefaultCredentials

  .method /*0600006A*/ public hidebysig specialname virtual 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_UseDefaultCredentials

  .method /*0600006B*/ public hidebysig specialname 
          instance bool  get_UsePassive() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::get_UsePassive

  .method /*0600006C*/ public hidebysig specialname 
          instance void  set_UsePassive(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21cb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::set_UsePassive

  .method /*0600006D*/ public hidebysig virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebRequest::Abort

  .method /*0600006E*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginGetRequestStream(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                                object state) cil managed
  // SIG: 20 02 12 80 85 12 80 89 1C
  {
    .param [0]/*08000045*/ 
    .custom /*0C000073:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::BeginGetRequestStream

  .method /*0600006F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginGetResponse(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                           object state) cil managed
  // SIG: 20 02 12 80 85 12 80 89 1C
  {
    .param [0]/*08000048*/ 
    .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::BeginGetResponse

  .method /*06000070*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          EndGetRequestStream(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult) cil managed
  // SIG: 20 01 12 80 8D 12 80 85
  {
    .custom /*0C000085:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::EndGetRequestStream

  .method /*06000071*/ public hidebysig virtual 
          instance class System.Net.WebResponse/*02000019*/ 
          EndGetResponse(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult) cil managed
  // SIG: 20 01 12 64 12 80 85
  {
    .custom /*0C000086:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::EndGetResponse

  .method /*06000072*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetRequestStream() cil managed
  // SIG: 20 00 12 80 8D
  {
    .custom /*0C000087:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::GetRequestStream

  .method /*06000073*/ public hidebysig virtual 
          instance class System.Net.WebResponse/*02000019*/ 
          GetResponse() cil managed
  // SIG: 20 00 12 64
  {
    .custom /*0C000088:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebRequest::GetResponse

  .property /*17000019*/ instance callconv(8) class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/
          ClientCertificates()
  {
    .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/ System.Net.FtpWebRequest/*0200000B*/::get_ClientCertificates() /* 06000045 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_ClientCertificates(class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/) /* 06000046 */
  } // end of property FtpWebRequest::ClientCertificates
  .property /*1700001A*/ instance callconv(8) string
          ConnectionGroupName()
  {
    .get instance string System.Net.FtpWebRequest/*0200000B*/::get_ConnectionGroupName() /* 06000047 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_ConnectionGroupName(string) /* 06000048 */
  } // end of property FtpWebRequest::ConnectionGroupName
  .property /*1700001B*/ instance callconv(8) int64
          ContentLength()
  {
    .get instance int64 System.Net.FtpWebRequest/*0200000B*/::get_ContentLength() /* 06000049 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_ContentLength(int64) /* 0600004A */
  } // end of property FtpWebRequest::ContentLength
  .property /*1700001C*/ instance callconv(8) int64
          ContentOffset()
  {
    .get instance int64 System.Net.FtpWebRequest/*0200000B*/::get_ContentOffset() /* 0600004B */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_ContentOffset(int64) /* 0600004C */
  } // end of property FtpWebRequest::ContentOffset
  .property /*1700001D*/ instance callconv(8) string
          ContentType()
  {
    .get instance string System.Net.FtpWebRequest/*0200000B*/::get_ContentType() /* 0600004D */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_ContentType(string) /* 0600004E */
  } // end of property FtpWebRequest::ContentType
  .property /*1700001E*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/
          Credentials()
  {
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ System.Net.FtpWebRequest/*0200000B*/::get_Credentials() /* 0600004F */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/) /* 06000050 */
  } // end of property FtpWebRequest::Credentials
  .property /*1700001F*/ callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/
          DefaultCachePolicy()
  {
    .get class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ System.Net.FtpWebRequest/*0200000B*/::get_DefaultCachePolicy() /* 06000051 */
    .set void System.Net.FtpWebRequest/*0200000B*/::set_DefaultCachePolicy(class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/) /* 06000052 */
  } // end of property FtpWebRequest::DefaultCachePolicy
  .property /*17000020*/ instance callconv(8) bool
          EnableSsl()
  {
    .get instance bool System.Net.FtpWebRequest/*0200000B*/::get_EnableSsl() /* 06000053 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_EnableSsl(bool) /* 06000054 */
  } // end of property FtpWebRequest::EnableSsl
  .property /*17000021*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/
          Headers()
  {
    .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ System.Net.FtpWebRequest/*0200000B*/::get_Headers() /* 06000055 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_Headers(class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/) /* 06000056 */
  } // end of property FtpWebRequest::Headers
  .property /*17000022*/ instance callconv(8) bool
          KeepAlive()
  {
    .get instance bool System.Net.FtpWebRequest/*0200000B*/::get_KeepAlive() /* 06000057 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_KeepAlive(bool) /* 06000058 */
  } // end of property FtpWebRequest::KeepAlive
  .property /*17000023*/ instance callconv(8) string
          Method()
  {
    .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Net.FtpWebRequest/*0200000B*/::get_Method() /* 06000059 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_Method(string) /* 0600005A */
  } // end of property FtpWebRequest::Method
  .property /*17000024*/ instance callconv(8) bool
          PreAuthenticate()
  {
    .get instance bool System.Net.FtpWebRequest/*0200000B*/::get_PreAuthenticate() /* 0600005B */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_PreAuthenticate(bool) /* 0600005C */
  } // end of property FtpWebRequest::PreAuthenticate
  .property /*17000025*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/
          Proxy()
  {
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ System.Net.FtpWebRequest/*0200000B*/::get_Proxy() /* 0600005D */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/) /* 0600005E */
  } // end of property FtpWebRequest::Proxy
  .property /*17000026*/ instance callconv(8) int32
          ReadWriteTimeout()
  {
    .get instance int32 System.Net.FtpWebRequest/*0200000B*/::get_ReadWriteTimeout() /* 0600005F */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_ReadWriteTimeout(int32) /* 06000060 */
  } // end of property FtpWebRequest::ReadWriteTimeout
  .property /*17000027*/ instance callconv(8) string
          RenameTo()
  {
    .get instance string System.Net.FtpWebRequest/*0200000B*/::get_RenameTo() /* 06000061 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_RenameTo(string) /* 06000062 */
  } // end of property FtpWebRequest::RenameTo
  .property /*17000028*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          RequestUri()
  {
    .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.FtpWebRequest/*0200000B*/::get_RequestUri() /* 06000063 */
  } // end of property FtpWebRequest::RequestUri
  .property /*17000029*/ instance callconv(8) class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*01000029*/
          ServicePoint()
  {
    .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*01000029*/ System.Net.FtpWebRequest/*0200000B*/::get_ServicePoint() /* 06000064 */
  } // end of property FtpWebRequest::ServicePoint
  .property /*1700002A*/ instance callconv(8) int32
          Timeout()
  {
    .get instance int32 System.Net.FtpWebRequest/*0200000B*/::get_Timeout() /* 06000065 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_Timeout(int32) /* 06000066 */
  } // end of property FtpWebRequest::Timeout
  .property /*1700002B*/ instance callconv(8) bool
          UseBinary()
  {
    .get instance bool System.Net.FtpWebRequest/*0200000B*/::get_UseBinary() /* 06000067 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_UseBinary(bool) /* 06000068 */
  } // end of property FtpWebRequest::UseBinary
  .property /*1700002C*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .get instance bool System.Net.FtpWebRequest/*0200000B*/::get_UseDefaultCredentials() /* 06000069 */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_UseDefaultCredentials(bool) /* 0600006A */
  } // end of property FtpWebRequest::UseDefaultCredentials
  .property /*1700002D*/ instance callconv(8) bool
          UsePassive()
  {
    .get instance bool System.Net.FtpWebRequest/*0200000B*/::get_UsePassive() /* 0600006B */
    .set instance void System.Net.FtpWebRequest/*0200000B*/::set_UsePassive(bool) /* 0600006C */
  } // end of property FtpWebRequest::UsePassive
} // end of class System.Net.FtpWebRequest

.class /*0200000C*/ public auto ansi beforefieldinit System.Net.FtpWebResponse
       extends System.Net.WebResponse/*02000019*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100002A*/
{
  .custom /*0C00003E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100002A*/
  .custom /*0C00001F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000074*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21e1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000152       */ call       instance void System.Net.WebResponse/*02000019*/::.ctor() /* 06000152 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method FtpWebResponse::.ctor

  .method /*06000075*/ public hidebysig specialname 
          instance string  get_BannerMessage() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_BannerMessage

  .method /*06000076*/ public hidebysig specialname virtual 
          instance int64  get_ContentLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_ContentLength

  .method /*06000077*/ public hidebysig specialname 
          instance string  get_ExitMessage() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_ExitMessage

  .method /*06000078*/ public hidebysig specialname virtual 
          instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C000089:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_Headers

  .method /*06000079*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ 
          get_LastModified() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x21f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_LastModified

  .method /*0600007A*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_ResponseUri() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_ResponseUri

  .method /*0600007B*/ public hidebysig specialname 
          instance valuetype System.Net.FtpStatusCode/*0200000A*/ 
          get_StatusCode() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x21fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_StatusCode

  .method /*0600007C*/ public hidebysig specialname 
          instance string  get_StatusDescription() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_StatusDescription

  .method /*0600007D*/ public hidebysig specialname virtual 
          instance bool  get_SupportsHeaders() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2201
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_SupportsHeaders

  .method /*0600007E*/ public hidebysig specialname 
          instance string  get_WelcomeMessage() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2204
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::get_WelcomeMessage

  .method /*0600007F*/ public hidebysig virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2207
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method FtpWebResponse::Close

  .method /*06000080*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetResponseStream() cil managed
  // SIG: 20 00 12 80 8D
  {
    .custom /*0C000090:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method FtpWebResponse::GetResponseStream

  .property /*1700002E*/ instance callconv(8) string
          BannerMessage()
  {
    .get instance string System.Net.FtpWebResponse/*0200000C*/::get_BannerMessage() /* 06000075 */
  } // end of property FtpWebResponse::BannerMessage
  .property /*1700002F*/ instance callconv(8) int64
          ContentLength()
  {
    .get instance int64 System.Net.FtpWebResponse/*0200000C*/::get_ContentLength() /* 06000076 */
  } // end of property FtpWebResponse::ContentLength
  .property /*17000030*/ instance callconv(8) string
          ExitMessage()
  {
    .get instance string System.Net.FtpWebResponse/*0200000C*/::get_ExitMessage() /* 06000077 */
  } // end of property FtpWebResponse::ExitMessage
  .property /*17000031*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/
          Headers()
  {
    .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ System.Net.FtpWebResponse/*0200000C*/::get_Headers() /* 06000078 */
  } // end of property FtpWebResponse::Headers
  .property /*17000032*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/
          LastModified()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ System.Net.FtpWebResponse/*0200000C*/::get_LastModified() /* 06000079 */
  } // end of property FtpWebResponse::LastModified
  .property /*17000033*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          ResponseUri()
  {
    .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.FtpWebResponse/*0200000C*/::get_ResponseUri() /* 0600007A */
  } // end of property FtpWebResponse::ResponseUri
  .property /*17000034*/ instance callconv(8) valuetype System.Net.FtpStatusCode/*0200000A*/
          StatusCode()
  {
    .get instance valuetype System.Net.FtpStatusCode/*0200000A*/ System.Net.FtpWebResponse/*0200000C*/::get_StatusCode() /* 0600007B */
  } // end of property FtpWebResponse::StatusCode
  .property /*17000035*/ instance callconv(8) string
          StatusDescription()
  {
    .get instance string System.Net.FtpWebResponse/*0200000C*/::get_StatusDescription() /* 0600007C */
  } // end of property FtpWebResponse::StatusDescription
  .property /*17000036*/ instance callconv(8) bool
          SupportsHeaders()
  {
    .get instance bool System.Net.FtpWebResponse/*0200000C*/::get_SupportsHeaders() /* 0600007D */
  } // end of property FtpWebResponse::SupportsHeaders
  .property /*17000037*/ instance callconv(8) string
          WelcomeMessage()
  {
    .get instance string System.Net.FtpWebResponse/*0200000C*/::get_WelcomeMessage() /* 0600007E */
  } // end of property FtpWebResponse::WelcomeMessage
} // end of class System.Net.FtpWebResponse

.class /*0200000D*/ public auto ansi beforefieldinit System.Net.GlobalProxySelection
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000041:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000042:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000043:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 A4 47 6C 6F 62 61 6C 50 72 6F 78 79 53   // ....GlobalProxyS
                                                                                                                                                 65 6C 65 63 74 69 6F 6E 20 68 61 73 20 62 65 65   // election has bee
                                                                                                                                                 6E 20 64 65 70 72 65 63 61 74 65 64 2E 20 55 73   // n deprecated. Us
                                                                                                                                                 65 20 57 65 62 52 65 71 75 65 73 74 2E 44 65 66   // e WebRequest.Def
                                                                                                                                                 61 75 6C 74 57 65 62 50 72 6F 78 79 20 69 6E 73   // aultWebProxy ins
                                                                                                                                                 74 65 61 64 20 74 6F 20 61 63 63 65 73 73 20 61   // tead to access a
                                                                                                                                                 6E 64 20 73 65 74 20 74 68 65 20 67 6C 6F 62 61   // nd set the globa
                                                                                                                                                 6C 20 64 65 66 61 75 6C 74 20 70 72 6F 78 79 2E   // l default proxy.
                                                                                                                                                 20 55 73 65 20 27 6E 75 6C 6C 27 20 69 6E 73 74   //  Use 'null' inst
                                                                                                                                                 65 61 64 20 6F 66 20 47 65 74 45 6D 70 74 79 57   // ead of GetEmptyW
                                                                                                                                                 65 62 50 72 6F 78 79 2E 00 00 )                   // ebProxy...
  .method /*06000081*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x220c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000016*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method GlobalProxySelection::.ctor

  .method /*06000082*/ public hidebysig specialname static 
          class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 
          get_Select() cil managed
  // SIG: 00 00 12 7D
  {
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GlobalProxySelection::get_Select

  .method /*06000083*/ public hidebysig specialname static 
          void  set_Select(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 'value') cil managed
  // SIG: 00 01 01 12 7D
  {
    .param [1]/*0800004D*/ 
    .custom /*0C000076:0A000016*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*0100002C*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2217
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method GlobalProxySelection::set_Select

  .method /*06000084*/ public hidebysig static 
          class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 
          GetEmptyWebProxy() cil managed
  // SIG: 00 00 12 7D
  {
    // Method begins at RVA 0x2219
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method GlobalProxySelection::GetEmptyWebProxy

  .property /*17000038*/ callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/
          Select()
  {
    .get class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ System.Net.GlobalProxySelection/*0200000D*/::get_Select() /* 06000082 */
    .set void System.Net.GlobalProxySelection/*0200000D*/::set_Select(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/) /* 06000083 */
  } // end of property GlobalProxySelection::Select
} // end of class System.Net.GlobalProxySelection

.class /*0200000E*/ public auto ansi sealed System.Net.HttpContinueDelegate
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*0100002D*/
{
  .method /*06000085*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method HttpContinueDelegate::.ctor

  .method /*06000086*/ public hidebysig newslot virtual 
          instance void  Invoke(int32 StatusCode,
                                class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ httpHeaders) runtime managed
  // SIG: 20 02 01 08 12 79
  {
    .custom /*0C000093:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  } // end of method HttpContinueDelegate::Invoke

  .method /*06000087*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginInvoke(int32 StatusCode,
                      class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ httpHeaders,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 85 08 12 79 12 80 89 1C
  {
    .param [2]/*08000053*/ 
    .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method HttpContinueDelegate::BeginInvoke

  .method /*06000088*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ result) runtime managed
  // SIG: 20 01 01 12 80 85
  {
  } // end of method HttpContinueDelegate::EndInvoke

} // end of class System.Net.HttpContinueDelegate

.class /*0200000F*/ public auto ansi beforefieldinit System.Net.HttpWebRequest
       extends System.Net.WebRequest/*02000017*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
{
  .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
  .custom /*0C000023:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000089*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C000094:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000095:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 5C 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..\WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 02   // pClient instead.
                                                                                                                                                   00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49 64   // .T..DiagnosticId
                                                                                                                                                   0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55 72   // .SYSLIB0014T..Ur
                                                                                                                                                   6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F   // lFormat"https://
                                                                                                                                                   61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61   // aka.ms/dotnet-wa
                                                                                                                                                   72 6E 69 6E 67 73 2F 7B 30 7D )                   // rnings/{0}
    // Method begins at RVA 0x221c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00011F       */ call       instance void System.Net.WebRequest/*02000017*/::.ctor() /* 0600011F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::.ctor

  .method /*0600008A*/ public hidebysig specialname 
          instance string  get_Accept() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Accept

  .method /*0600008B*/ public hidebysig specialname 
          instance void  set_Accept(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2227
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Accept

  .method /*0600008C*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_Address() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C000096:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2229
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Address

  .method /*0600008D*/ public hidebysig newslot specialname virtual 
          instance bool  get_AllowAutoRedirect() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x222c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_AllowAutoRedirect

  .method /*0600008E*/ public hidebysig newslot specialname virtual 
          instance void  set_AllowAutoRedirect(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x222f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_AllowAutoRedirect

  .method /*0600008F*/ public hidebysig newslot specialname virtual 
          instance bool  get_AllowReadStreamBuffering() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2231
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_AllowReadStreamBuffering

  .method /*06000090*/ public hidebysig newslot specialname virtual 
          instance void  set_AllowReadStreamBuffering(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2234
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_AllowReadStreamBuffering

  .method /*06000091*/ public hidebysig newslot specialname virtual 
          instance bool  get_AllowWriteStreamBuffering() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_AllowWriteStreamBuffering

  .method /*06000092*/ public hidebysig newslot specialname virtual 
          instance void  set_AllowWriteStreamBuffering(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2239
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_AllowWriteStreamBuffering

  .method /*06000093*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000003*/]System.Net.DecompressionMethods/*0100002E*/ 
          get_AutomaticDecompression() cil managed
  // SIG: 20 00 11 80 B9
  {
    // Method begins at RVA 0x223b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_AutomaticDecompression

  .method /*06000094*/ public hidebysig specialname 
          instance void  set_AutomaticDecompression(valuetype [System.Net.Primitives/*23000003*/]System.Net.DecompressionMethods/*0100002E*/ 'value') cil managed
  // SIG: 20 01 01 11 80 B9
  {
    // Method begins at RVA 0x223e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_AutomaticDecompression

  .method /*06000095*/ public hidebysig specialname 
          instance class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/ 
          get_ClientCertificates() cil managed
  // SIG: 20 00 12 80 99
  {
    .custom /*0C000099:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2240
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ClientCertificates

  .method /*06000096*/ public hidebysig specialname 
          instance void  set_ClientCertificates(class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/ 'value') cil managed
  // SIG: 20 01 01 12 80 99
  {
    .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2243
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ClientCertificates

  .method /*06000097*/ public hidebysig specialname 
          instance string  get_Connection() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Connection

  .method /*06000098*/ public hidebysig specialname 
          instance void  set_Connection(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2248
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Connection

  .method /*06000099*/ public hidebysig specialname virtual 
          instance string  get_ConnectionGroupName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x224a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ConnectionGroupName

  .method /*0600009A*/ public hidebysig specialname virtual 
          instance void  set_ConnectionGroupName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x224d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ConnectionGroupName

  .method /*0600009B*/ public hidebysig specialname virtual 
          instance int64  get_ContentLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x224f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ContentLength

  .method /*0600009C*/ public hidebysig specialname virtual 
          instance void  set_ContentLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2252
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ContentLength

  .method /*0600009D*/ public hidebysig specialname virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2254
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ContentType

  .method /*0600009E*/ public hidebysig specialname virtual 
          instance void  set_ContentType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2257
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ContentType

  .method /*0600009F*/ public hidebysig specialname 
          instance class System.Net.HttpContinueDelegate/*0200000E*/ 
          get_ContinueDelegate() cil managed
  // SIG: 20 00 12 38
  {
    // Method begins at RVA 0x2259
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ContinueDelegate

  .method /*060000A0*/ public hidebysig specialname 
          instance void  set_ContinueDelegate(class System.Net.HttpContinueDelegate/*0200000E*/ 'value') cil managed
  // SIG: 20 01 01 12 38
  {
    // Method begins at RVA 0x225c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ContinueDelegate

  .method /*060000A1*/ public hidebysig specialname 
          instance int32  get_ContinueTimeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x225e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ContinueTimeout

  .method /*060000A2*/ public hidebysig specialname 
          instance void  set_ContinueTimeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2261
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ContinueTimeout

  .method /*060000A3*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002F*/ 
          get_CookieContainer() cil managed
  // SIG: 20 00 12 80 BD
  {
    // Method begins at RVA 0x2263
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_CookieContainer

  .method /*060000A4*/ public hidebysig newslot specialname virtual 
          instance void  set_CookieContainer(class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002F*/ 'value') cil managed
  // SIG: 20 01 01 12 80 BD
  {
    // Method begins at RVA 0x2266
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_CookieContainer

  .method /*060000A5*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 69
  {
    // Method begins at RVA 0x2268
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Credentials

  .method /*060000A6*/ public hidebysig specialname virtual 
          instance void  set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ 'value') cil managed
  // SIG: 20 01 01 12 69
  {
    // Method begins at RVA 0x226b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Credentials

  .method /*060000A7*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ 
          get_Date() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x226d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Date

  .method /*060000A8*/ public hidebysig specialname 
          instance void  set_Date(valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ 'value') cil managed
  // SIG: 20 01 01 11 80 AD
  {
    // Method begins at RVA 0x2270
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Date

  .method /*060000A9*/ public hidebysig specialname static 
          class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ 
          get_DefaultCachePolicy() cil managed
  // SIG: 00 00 12 80 A1
  {
    // Method begins at RVA 0x2272
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_DefaultCachePolicy

  .method /*060000AA*/ public hidebysig specialname static 
          void  set_DefaultCachePolicy(class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ 'value') cil managed
  // SIG: 00 01 01 12 80 A1
  {
    // Method begins at RVA 0x2275
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_DefaultCachePolicy

  .method /*060000AB*/ public hidebysig specialname static 
          int32  get_DefaultMaximumErrorResponseLength() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x2277
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_DefaultMaximumErrorResponseLength

  .method /*060000AC*/ public hidebysig specialname static 
          void  set_DefaultMaximumErrorResponseLength(int32 'value') cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x227a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_DefaultMaximumErrorResponseLength

  .method /*060000AD*/ public hidebysig specialname static 
          int32  get_DefaultMaximumResponseHeadersLength() cil managed
  // SIG: 00 00 08
  {
    // Method begins at RVA 0x227c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_DefaultMaximumResponseHeadersLength

  .method /*060000AE*/ public hidebysig specialname static 
          void  set_DefaultMaximumResponseHeadersLength(int32 'value') cil managed
  // SIG: 00 01 01 08
  {
    // Method begins at RVA 0x227f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_DefaultMaximumResponseHeadersLength

  .method /*060000AF*/ public hidebysig specialname 
          instance string  get_Expect() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2281
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Expect

  .method /*060000B0*/ public hidebysig specialname 
          instance void  set_Expect(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2284
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Expect

  .method /*060000B1*/ public hidebysig newslot specialname virtual 
          instance bool  get_HaveResponse() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2286
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_HaveResponse

  .method /*060000B2*/ public hidebysig specialname virtual 
          instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 79
  {
    .custom /*0C00009F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2289
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Headers

  .method /*060000B3*/ public hidebysig specialname virtual 
          instance void  set_Headers(class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Headers

  .method /*060000B4*/ public hidebysig specialname 
          instance string  get_Host() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Host

  .method /*060000B5*/ public hidebysig specialname 
          instance void  set_Host(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2291
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Host

  .method /*060000B6*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ 
          get_IfModifiedSince() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_IfModifiedSince

  .method /*060000B7*/ public hidebysig specialname 
          instance void  set_IfModifiedSince(valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ 'value') cil managed
  // SIG: 20 01 01 11 80 AD
  {
    // Method begins at RVA 0x2296
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_IfModifiedSince

  .method /*060000B8*/ public hidebysig specialname 
          instance bool  get_KeepAlive() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2298
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_KeepAlive

  .method /*060000B9*/ public hidebysig specialname 
          instance void  set_KeepAlive(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x229b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_KeepAlive

  .method /*060000BA*/ public hidebysig specialname 
          instance int32  get_MaximumAutomaticRedirections() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x229d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_MaximumAutomaticRedirections

  .method /*060000BB*/ public hidebysig specialname 
          instance void  set_MaximumAutomaticRedirections(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_MaximumAutomaticRedirections

  .method /*060000BC*/ public hidebysig specialname 
          instance int32  get_MaximumResponseHeadersLength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_MaximumResponseHeadersLength

  .method /*060000BD*/ public hidebysig specialname 
          instance void  set_MaximumResponseHeadersLength(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_MaximumResponseHeadersLength

  .method /*060000BE*/ public hidebysig specialname 
          instance string  get_MediaType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_MediaType

  .method /*060000BF*/ public hidebysig specialname 
          instance void  set_MediaType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_MediaType

  .method /*060000C0*/ public hidebysig specialname virtual 
          instance string  get_Method() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000A3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Method

  .method /*060000C1*/ public hidebysig specialname virtual 
          instance void  set_Method(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Method

  .method /*060000C2*/ public hidebysig specialname 
          instance bool  get_Pipelined() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Pipelined

  .method /*060000C3*/ public hidebysig specialname 
          instance void  set_Pipelined(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22b4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Pipelined

  .method /*060000C4*/ public hidebysig specialname virtual 
          instance bool  get_PreAuthenticate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_PreAuthenticate

  .method /*060000C5*/ public hidebysig specialname virtual 
          instance void  set_PreAuthenticate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_PreAuthenticate

  .method /*060000C6*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Version/*01000030*/ 
          get_ProtocolVersion() cil managed
  // SIG: 20 00 12 80 C1
  {
    .custom /*0C0000A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ProtocolVersion

  .method /*060000C7*/ public hidebysig specialname 
          instance void  set_ProtocolVersion(class [System.Runtime/*23000001*/]System.Version/*01000030*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C1
  {
    .custom /*0C0000A6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22be
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ProtocolVersion

  .method /*060000C8*/ public hidebysig specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 
          get_Proxy() cil managed
  // SIG: 20 00 12 7D
  {
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Proxy

  .method /*060000C9*/ public hidebysig specialname virtual 
          instance void  set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 'value') cil managed
  // SIG: 20 01 01 12 7D
  {
    // Method begins at RVA 0x22c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Proxy

  .method /*060000CA*/ public hidebysig specialname 
          instance int32  get_ReadWriteTimeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ReadWriteTimeout

  .method /*060000CB*/ public hidebysig specialname 
          instance void  set_ReadWriteTimeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ReadWriteTimeout

  .method /*060000CC*/ public hidebysig specialname 
          instance string  get_Referer() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Referer

  .method /*060000CD*/ public hidebysig specialname 
          instance void  set_Referer(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Referer

  .method /*060000CE*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_RequestUri() cil managed
  // SIG: 20 00 12 80 81
  {
    .custom /*0C0000A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_RequestUri

  .method /*060000CF*/ public hidebysig specialname 
          instance bool  get_SendChunked() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_SendChunked

  .method /*060000D0*/ public hidebysig specialname 
          instance void  set_SendChunked(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22d5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_SendChunked

  .method /*060000D1*/ public hidebysig specialname 
          instance class [System.Net.Security/*23000007*/]System.Net.Security.RemoteCertificateValidationCallback/*01000031*/ 
          get_ServerCertificateValidationCallback() cil managed
  // SIG: 20 00 12 80 C5
  {
    // Method begins at RVA 0x22d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ServerCertificateValidationCallback

  .method /*060000D2*/ public hidebysig specialname 
          instance void  set_ServerCertificateValidationCallback(class [System.Net.Security/*23000007*/]System.Net.Security.RemoteCertificateValidationCallback/*01000031*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C5
  {
    // Method begins at RVA 0x22da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_ServerCertificateValidationCallback

  .method /*060000D3*/ public hidebysig specialname 
          instance class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*01000029*/ 
          get_ServicePoint() cil managed
  // SIG: 20 00 12 80 A5
  {
    .custom /*0C0000A9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_ServicePoint

  .method /*060000D4*/ public hidebysig newslot specialname virtual 
          instance bool  get_SupportsCookieContainer() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_SupportsCookieContainer

  .method /*060000D5*/ public hidebysig specialname virtual 
          instance int32  get_Timeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x22e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_Timeout

  .method /*060000D6*/ public hidebysig specialname virtual 
          instance void  set_Timeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_Timeout

  .method /*060000D7*/ public hidebysig specialname 
          instance string  get_TransferEncoding() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22e7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_TransferEncoding

  .method /*060000D8*/ public hidebysig specialname 
          instance void  set_TransferEncoding(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22ea
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_TransferEncoding

  .method /*060000D9*/ public hidebysig specialname 
          instance bool  get_UnsafeAuthenticatedConnectionSharing() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_UnsafeAuthenticatedConnectionSharing

  .method /*060000DA*/ public hidebysig specialname 
          instance void  set_UnsafeAuthenticatedConnectionSharing(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_UnsafeAuthenticatedConnectionSharing

  .method /*060000DB*/ public hidebysig specialname virtual 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_UseDefaultCredentials

  .method /*060000DC*/ public hidebysig specialname virtual 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22f4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_UseDefaultCredentials

  .method /*060000DD*/ public hidebysig specialname 
          instance string  get_UserAgent() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::get_UserAgent

  .method /*060000DE*/ public hidebysig specialname 
          instance void  set_UserAgent(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::set_UserAgent

  .method /*060000DF*/ public hidebysig virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22fb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::Abort

  .method /*060000E0*/ public hidebysig instance void 
          AddRange(int32 range) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x22fd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::AddRange

  .method /*060000E1*/ public hidebysig instance void 
          AddRange(int32 from,
                   int32 'to') cil managed
  // SIG: 20 02 01 08 08
  {
    // Method begins at RVA 0x22ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::AddRange

  .method /*060000E2*/ public hidebysig instance void 
          AddRange(int64 range) cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2301
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::AddRange

  .method /*060000E3*/ public hidebysig instance void 
          AddRange(int64 from,
                   int64 'to') cil managed
  // SIG: 20 02 01 0A 0A
  {
    // Method begins at RVA 0x2303
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::AddRange

  .method /*060000E4*/ public hidebysig instance void 
          AddRange(string rangeSpecifier,
                   int32 range) cil managed
  // SIG: 20 02 01 0E 08
  {
    .custom /*0C0000AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2305
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::AddRange

  .method /*060000E5*/ public hidebysig instance void 
          AddRange(string rangeSpecifier,
                   int32 from,
                   int32 'to') cil managed
  // SIG: 20 03 01 0E 08 08
  {
    .custom /*0C0000AD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2307
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::AddRange

  .method /*060000E6*/ public hidebysig instance void 
          AddRange(string rangeSpecifier,
                   int64 range) cil managed
  // SIG: 20 02 01 0E 0A
  {
    .custom /*0C0000AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2309
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::AddRange

  .method /*060000E7*/ public hidebysig instance void 
          AddRange(string rangeSpecifier,
                   int64 from,
                   int64 'to') cil managed
  // SIG: 20 03 01 0E 0A 0A
  {
    .custom /*0C0000AF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x230b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::AddRange

  .method /*060000E8*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginGetRequestStream(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                                object state) cil managed
  // SIG: 20 02 12 80 85 12 80 89 1C
  {
    .param [0]/*08000091*/ 
    .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x230d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::BeginGetRequestStream

  .method /*060000E9*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginGetResponse(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                           object state) cil managed
  // SIG: 20 02 12 80 85 12 80 89 1C
  {
    .param [0]/*08000094*/ 
    .custom /*0C000098:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2310
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::BeginGetResponse

  .method /*060000EA*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          EndGetRequestStream(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult) cil managed
  // SIG: 20 01 12 80 8D 12 80 85
  {
    .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2313
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::EndGetRequestStream

  .method /*060000EB*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          EndGetRequestStream(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult,
                              [out] class [System.Net.Primitives/*23000003*/]System.Net.TransportContext/*01000032*/& context) cil managed
  // SIG: 20 02 12 80 8D 12 80 85 10 12 80 C9
  {
    .custom /*0C0000B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000099*/ 
    .custom /*0C00009B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2316
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::EndGetRequestStream

  .method /*060000EC*/ public hidebysig virtual 
          instance class System.Net.WebResponse/*02000019*/ 
          EndGetResponse(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult) cil managed
  // SIG: 20 01 12 64 12 80 85
  {
    .custom /*0C0000B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2319
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::EndGetResponse

  .method /*060000ED*/ family hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000B4:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 35 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E   // ..5Serialization
                                                                                                                                                   20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63   //  has been deprec
                                                                                                                                                   61 74 65 64 20 66 6F 72 20 48 74 74 70 57 65 62   // ated for HttpWeb
                                                                                                                                                   52 65 71 75 65 73 74 2E 00 00 )                   // Request...
    // Method begins at RVA 0x231c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::GetObjectData

  .method /*060000EE*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetRequestStream() cil managed
  // SIG: 20 00 12 80 8D
  {
    .custom /*0C0000B5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x231e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::GetRequestStream

  .method /*060000EF*/ public hidebysig instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetRequestStream([out] class [System.Net.Primitives/*23000003*/]System.Net.TransportContext/*01000032*/& context) cil managed
  // SIG: 20 01 12 80 8D 10 12 80 C9
  {
    .custom /*0C0000B6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800009D*/ 
    .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2321
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::GetRequestStream

  .method /*060000F0*/ public hidebysig virtual 
          instance class System.Net.WebResponse/*02000019*/ 
          GetResponse() cil managed
  // SIG: 20 00 12 64
  {
    .custom /*0C0000B7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2324
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebRequest::GetResponse

  .method /*060000F1*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000B8:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 35 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E   // ..5Serialization
                                                                                                                                                   20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63   //  has been deprec
                                                                                                                                                   61 74 65 64 20 66 6F 72 20 48 74 74 70 57 65 62   // ated for HttpWeb
                                                                                                                                                   52 65 71 75 65 73 74 2E 00 00 )                   // Request...
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/::GetObjectData /*0100001B::0A000014*/ 
    // Method begins at RVA 0x2327
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebRequest::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000039*/ instance callconv(8) string
          Accept()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_Accept() /* 0600008A */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Accept(string) /* 0600008B */
  } // end of property HttpWebRequest::Accept
  .property /*1700003A*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          Address()
  {
    .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.HttpWebRequest/*0200000F*/::get_Address() /* 0600008C */
  } // end of property HttpWebRequest::Address
  .property /*1700003B*/ instance callconv(8) bool
          AllowAutoRedirect()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_AllowAutoRedirect() /* 0600008D */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_AllowAutoRedirect(bool) /* 0600008E */
  } // end of property HttpWebRequest::AllowAutoRedirect
  .property /*1700003C*/ instance callconv(8) bool
          AllowReadStreamBuffering()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_AllowReadStreamBuffering() /* 0600008F */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_AllowReadStreamBuffering(bool) /* 06000090 */
  } // end of property HttpWebRequest::AllowReadStreamBuffering
  .property /*1700003D*/ instance callconv(8) bool
          AllowWriteStreamBuffering()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_AllowWriteStreamBuffering() /* 06000091 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_AllowWriteStreamBuffering(bool) /* 06000092 */
  } // end of property HttpWebRequest::AllowWriteStreamBuffering
  .property /*1700003E*/ instance callconv(8) valuetype [System.Net.Primitives/*23000003*/]System.Net.DecompressionMethods/*0100002E*/
          AutomaticDecompression()
  {
    .get instance valuetype [System.Net.Primitives/*23000003*/]System.Net.DecompressionMethods/*0100002E*/ System.Net.HttpWebRequest/*0200000F*/::get_AutomaticDecompression() /* 06000093 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_AutomaticDecompression(valuetype [System.Net.Primitives/*23000003*/]System.Net.DecompressionMethods/*0100002E*/) /* 06000094 */
  } // end of property HttpWebRequest::AutomaticDecompression
  .property /*1700003F*/ instance callconv(8) class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/
          ClientCertificates()
  {
    .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/ System.Net.HttpWebRequest/*0200000F*/::get_ClientCertificates() /* 06000095 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ClientCertificates(class [System.Security.Cryptography.X509Certificates/*23000005*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000026*/) /* 06000096 */
  } // end of property HttpWebRequest::ClientCertificates
  .property /*17000040*/ instance callconv(8) string
          Connection()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_Connection() /* 06000097 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Connection(string) /* 06000098 */
  } // end of property HttpWebRequest::Connection
  .property /*17000041*/ instance callconv(8) string
          ConnectionGroupName()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_ConnectionGroupName() /* 06000099 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ConnectionGroupName(string) /* 0600009A */
  } // end of property HttpWebRequest::ConnectionGroupName
  .property /*17000042*/ instance callconv(8) int64
          ContentLength()
  {
    .get instance int64 System.Net.HttpWebRequest/*0200000F*/::get_ContentLength() /* 0600009B */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ContentLength(int64) /* 0600009C */
  } // end of property HttpWebRequest::ContentLength
  .property /*17000043*/ instance callconv(8) string
          ContentType()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_ContentType() /* 0600009D */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ContentType(string) /* 0600009E */
  } // end of property HttpWebRequest::ContentType
  .property /*17000044*/ instance callconv(8) class System.Net.HttpContinueDelegate/*0200000E*/
          ContinueDelegate()
  {
    .get instance class System.Net.HttpContinueDelegate/*0200000E*/ System.Net.HttpWebRequest/*0200000F*/::get_ContinueDelegate() /* 0600009F */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ContinueDelegate(class System.Net.HttpContinueDelegate/*0200000E*/) /* 060000A0 */
  } // end of property HttpWebRequest::ContinueDelegate
  .property /*17000045*/ instance callconv(8) int32
          ContinueTimeout()
  {
    .get instance int32 System.Net.HttpWebRequest/*0200000F*/::get_ContinueTimeout() /* 060000A1 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ContinueTimeout(int32) /* 060000A2 */
  } // end of property HttpWebRequest::ContinueTimeout
  .property /*17000046*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002F*/
          CookieContainer()
  {
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002F*/ System.Net.HttpWebRequest/*0200000F*/::get_CookieContainer() /* 060000A3 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_CookieContainer(class [System.Net.Primitives/*23000003*/]System.Net.CookieContainer/*0100002F*/) /* 060000A4 */
  } // end of property HttpWebRequest::CookieContainer
  .property /*17000047*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/
          Credentials()
  {
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ System.Net.HttpWebRequest/*0200000F*/::get_Credentials() /* 060000A5 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/) /* 060000A6 */
  } // end of property HttpWebRequest::Credentials
  .property /*17000048*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/
          Date()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ System.Net.HttpWebRequest/*0200000F*/::get_Date() /* 060000A7 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Date(valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/) /* 060000A8 */
  } // end of property HttpWebRequest::Date
  .property /*17000049*/ callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/
          DefaultCachePolicy()
  {
    .get class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ System.Net.HttpWebRequest/*0200000F*/::get_DefaultCachePolicy() /* 060000A9 */
    .set void System.Net.HttpWebRequest/*0200000F*/::set_DefaultCachePolicy(class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/) /* 060000AA */
  } // end of property HttpWebRequest::DefaultCachePolicy
  .property /*1700004A*/ callconv(8) int32
          DefaultMaximumErrorResponseLength()
  {
    .get int32 System.Net.HttpWebRequest/*0200000F*/::get_DefaultMaximumErrorResponseLength() /* 060000AB */
    .set void System.Net.HttpWebRequest/*0200000F*/::set_DefaultMaximumErrorResponseLength(int32) /* 060000AC */
  } // end of property HttpWebRequest::DefaultMaximumErrorResponseLength
  .property /*1700004B*/ callconv(8) int32
          DefaultMaximumResponseHeadersLength()
  {
    .get int32 System.Net.HttpWebRequest/*0200000F*/::get_DefaultMaximumResponseHeadersLength() /* 060000AD */
    .set void System.Net.HttpWebRequest/*0200000F*/::set_DefaultMaximumResponseHeadersLength(int32) /* 060000AE */
  } // end of property HttpWebRequest::DefaultMaximumResponseHeadersLength
  .property /*1700004C*/ instance callconv(8) string
          Expect()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_Expect() /* 060000AF */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Expect(string) /* 060000B0 */
  } // end of property HttpWebRequest::Expect
  .property /*1700004D*/ instance callconv(8) bool
          HaveResponse()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_HaveResponse() /* 060000B1 */
  } // end of property HttpWebRequest::HaveResponse
  .property /*1700004E*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/
          Headers()
  {
    .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ System.Net.HttpWebRequest/*0200000F*/::get_Headers() /* 060000B2 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Headers(class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/) /* 060000B3 */
  } // end of property HttpWebRequest::Headers
  .property /*1700004F*/ instance callconv(8) string
          Host()
  {
    .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_Host() /* 060000B4 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Host(string) /* 060000B5 */
  } // end of property HttpWebRequest::Host
  .property /*17000050*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/
          IfModifiedSince()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ System.Net.HttpWebRequest/*0200000F*/::get_IfModifiedSince() /* 060000B6 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_IfModifiedSince(valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/) /* 060000B7 */
  } // end of property HttpWebRequest::IfModifiedSince
  .property /*17000051*/ instance callconv(8) bool
          KeepAlive()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_KeepAlive() /* 060000B8 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_KeepAlive(bool) /* 060000B9 */
  } // end of property HttpWebRequest::KeepAlive
  .property /*17000052*/ instance callconv(8) int32
          MaximumAutomaticRedirections()
  {
    .get instance int32 System.Net.HttpWebRequest/*0200000F*/::get_MaximumAutomaticRedirections() /* 060000BA */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_MaximumAutomaticRedirections(int32) /* 060000BB */
  } // end of property HttpWebRequest::MaximumAutomaticRedirections
  .property /*17000053*/ instance callconv(8) int32
          MaximumResponseHeadersLength()
  {
    .get instance int32 System.Net.HttpWebRequest/*0200000F*/::get_MaximumResponseHeadersLength() /* 060000BC */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_MaximumResponseHeadersLength(int32) /* 060000BD */
  } // end of property HttpWebRequest::MaximumResponseHeadersLength
  .property /*17000054*/ instance callconv(8) string
          MediaType()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_MediaType() /* 060000BE */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_MediaType(string) /* 060000BF */
  } // end of property HttpWebRequest::MediaType
  .property /*17000055*/ instance callconv(8) string
          Method()
  {
    .custom /*0C00007B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_Method() /* 060000C0 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Method(string) /* 060000C1 */
  } // end of property HttpWebRequest::Method
  .property /*17000056*/ instance callconv(8) bool
          Pipelined()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_Pipelined() /* 060000C2 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Pipelined(bool) /* 060000C3 */
  } // end of property HttpWebRequest::Pipelined
  .property /*17000057*/ instance callconv(8) bool
          PreAuthenticate()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_PreAuthenticate() /* 060000C4 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_PreAuthenticate(bool) /* 060000C5 */
  } // end of property HttpWebRequest::PreAuthenticate
  .property /*17000058*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Version/*01000030*/
          ProtocolVersion()
  {
    .custom /*0C00007D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Version/*01000030*/ System.Net.HttpWebRequest/*0200000F*/::get_ProtocolVersion() /* 060000C6 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ProtocolVersion(class [System.Runtime/*23000001*/]System.Version/*01000030*/) /* 060000C7 */
  } // end of property HttpWebRequest::ProtocolVersion
  .property /*17000059*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/
          Proxy()
  {
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ System.Net.HttpWebRequest/*0200000F*/::get_Proxy() /* 060000C8 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/) /* 060000C9 */
  } // end of property HttpWebRequest::Proxy
  .property /*1700005A*/ instance callconv(8) int32
          ReadWriteTimeout()
  {
    .get instance int32 System.Net.HttpWebRequest/*0200000F*/::get_ReadWriteTimeout() /* 060000CA */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ReadWriteTimeout(int32) /* 060000CB */
  } // end of property HttpWebRequest::ReadWriteTimeout
  .property /*1700005B*/ instance callconv(8) string
          Referer()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_Referer() /* 060000CC */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Referer(string) /* 060000CD */
  } // end of property HttpWebRequest::Referer
  .property /*1700005C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          RequestUri()
  {
    .custom /*0C000080:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.HttpWebRequest/*0200000F*/::get_RequestUri() /* 060000CE */
  } // end of property HttpWebRequest::RequestUri
  .property /*1700005D*/ instance callconv(8) bool
          SendChunked()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_SendChunked() /* 060000CF */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_SendChunked(bool) /* 060000D0 */
  } // end of property HttpWebRequest::SendChunked
  .property /*1700005E*/ instance callconv(8) class [System.Net.Security/*23000007*/]System.Net.Security.RemoteCertificateValidationCallback/*01000031*/
          ServerCertificateValidationCallback()
  {
    .get instance class [System.Net.Security/*23000007*/]System.Net.Security.RemoteCertificateValidationCallback/*01000031*/ System.Net.HttpWebRequest/*0200000F*/::get_ServerCertificateValidationCallback() /* 060000D1 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_ServerCertificateValidationCallback(class [System.Net.Security/*23000007*/]System.Net.Security.RemoteCertificateValidationCallback/*01000031*/) /* 060000D2 */
  } // end of property HttpWebRequest::ServerCertificateValidationCallback
  .property /*1700005F*/ instance callconv(8) class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*01000029*/
          ServicePoint()
  {
    .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*01000029*/ System.Net.HttpWebRequest/*0200000F*/::get_ServicePoint() /* 060000D3 */
  } // end of property HttpWebRequest::ServicePoint
  .property /*17000060*/ instance callconv(8) bool
          SupportsCookieContainer()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_SupportsCookieContainer() /* 060000D4 */
  } // end of property HttpWebRequest::SupportsCookieContainer
  .property /*17000061*/ instance callconv(8) int32
          Timeout()
  {
    .get instance int32 System.Net.HttpWebRequest/*0200000F*/::get_Timeout() /* 060000D5 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_Timeout(int32) /* 060000D6 */
  } // end of property HttpWebRequest::Timeout
  .property /*17000062*/ instance callconv(8) string
          TransferEncoding()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_TransferEncoding() /* 060000D7 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_TransferEncoding(string) /* 060000D8 */
  } // end of property HttpWebRequest::TransferEncoding
  .property /*17000063*/ instance callconv(8) bool
          UnsafeAuthenticatedConnectionSharing()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_UnsafeAuthenticatedConnectionSharing() /* 060000D9 */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_UnsafeAuthenticatedConnectionSharing(bool) /* 060000DA */
  } // end of property HttpWebRequest::UnsafeAuthenticatedConnectionSharing
  .property /*17000064*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .get instance bool System.Net.HttpWebRequest/*0200000F*/::get_UseDefaultCredentials() /* 060000DB */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_UseDefaultCredentials(bool) /* 060000DC */
  } // end of property HttpWebRequest::UseDefaultCredentials
  .property /*17000065*/ instance callconv(8) string
          UserAgent()
  {
    .get instance string System.Net.HttpWebRequest/*0200000F*/::get_UserAgent() /* 060000DD */
    .set instance void System.Net.HttpWebRequest/*0200000F*/::set_UserAgent(string) /* 060000DE */
  } // end of property HttpWebRequest::UserAgent
} // end of class System.Net.HttpWebRequest

.class /*02000010*/ public auto ansi beforefieldinit System.Net.HttpWebResponse
       extends System.Net.WebResponse/*02000019*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
{
  .custom /*0C000046:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000047:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000F2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000B9:0A000017*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string,
                                                                                                                        bool) /* 0A000017 */ = ( 01 00 61 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..aThis API supp
                                                                                                                                                 6F 72 74 73 20 74 68 65 20 2E 4E 45 54 20 69 6E   // orts the .NET in
                                                                                                                                                 66 72 61 73 74 72 75 63 74 75 72 65 20 61 6E 64   // frastructure and
                                                                                                                                                 20 69 73 20 6E 6F 74 20 69 6E 74 65 6E 64 65 64   //  is not intended
                                                                                                                                                 20 74 6F 20 62 65 20 75 73 65 64 20 64 69 72 65   //  to be used dire
                                                                                                                                                 63 74 6C 79 20 66 72 6F 6D 20 79 6F 75 72 20 63   // ctly from your c
                                                                                                                                                 6F 64 65 2E 01 00 00 )                            // ode....
    .custom /*0C0000BA:0A000018*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000034*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000033*/) /* 0A000018 */ = ( 01 00 01 00 00 00 00 00 ) 
    // Method begins at RVA 0x2329
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000152       */ call       instance void System.Net.WebResponse/*02000019*/::.ctor() /* 06000152 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpWebResponse::.ctor

  .method /*060000F3*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000BB:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 36 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E   // ..6Serialization
                                                                                                                                                   20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63   //  has been deprec
                                                                                                                                                   61 74 65 64 20 66 6F 72 20 48 74 74 70 57 65 62   // ated for HttpWeb
                                                                                                                                                   52 65 73 70 6F 6E 73 65 2E 00 00 )                // Response...
    // Method begins at RVA 0x2331
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000152       */ call       instance void System.Net.WebResponse/*02000019*/::.ctor() /* 06000152 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpWebResponse::.ctor

  .method /*060000F4*/ public hidebysig specialname 
          instance string  get_CharacterSet() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000BC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2339
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_CharacterSet

  .method /*060000F5*/ public hidebysig specialname 
          instance string  get_ContentEncoding() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x233c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_ContentEncoding

  .method /*060000F6*/ public hidebysig specialname virtual 
          instance int64  get_ContentLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x233f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_ContentLength

  .method /*060000F7*/ public hidebysig specialname virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2342
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_ContentType

  .method /*060000F8*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.CookieCollection/*01000035*/ 
          get_Cookies() cil managed
  // SIG: 20 00 12 80 D5
  {
    // Method begins at RVA 0x2345
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_Cookies

  .method /*060000F9*/ public hidebysig newslot specialname virtual 
          instance void  set_Cookies(class [System.Net.Primitives/*23000003*/]System.Net.CookieCollection/*01000035*/ 'value') cil managed
  // SIG: 20 01 01 12 80 D5
  {
    // Method begins at RVA 0x2348
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebResponse::set_Cookies

  .method /*060000FA*/ public hidebysig specialname virtual 
          instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x234a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_Headers

  .method /*060000FB*/ public hidebysig specialname virtual 
          instance bool  get_IsMutuallyAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x234d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_IsMutuallyAuthenticated

  .method /*060000FC*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ 
          get_LastModified() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x2350
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_LastModified

  .method /*060000FD*/ public hidebysig newslot specialname virtual 
          instance string  get_Method() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2353
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_Method

  .method /*060000FE*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Version/*01000030*/ 
          get_ProtocolVersion() cil managed
  // SIG: 20 00 12 80 C1
  {
    // Method begins at RVA 0x2356
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_ProtocolVersion

  .method /*060000FF*/ public hidebysig specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_ResponseUri() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x2359
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_ResponseUri

  .method /*06000100*/ public hidebysig specialname 
          instance string  get_Server() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x235c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_Server

  .method /*06000101*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Net.Primitives/*23000003*/]System.Net.HttpStatusCode/*01000036*/ 
          get_StatusCode() cil managed
  // SIG: 20 00 11 80 D9
  {
    // Method begins at RVA 0x235f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_StatusCode

  .method /*06000102*/ public hidebysig newslot specialname virtual 
          instance string  get_StatusDescription() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2362
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_StatusDescription

  .method /*06000103*/ public hidebysig specialname virtual 
          instance bool  get_SupportsHeaders() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2365
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::get_SupportsHeaders

  .method /*06000104*/ public hidebysig virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2368
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebResponse::Close

  .method /*06000105*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x236a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebResponse::Dispose

  .method /*06000106*/ family hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000BD:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 36 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E   // ..6Serialization
                                                                                                                                                   20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63   //  has been deprec
                                                                                                                                                   61 74 65 64 20 66 6F 72 20 48 74 74 70 57 65 62   // ated for HttpWeb
                                                                                                                                                   52 65 73 70 6F 6E 73 65 2E 00 00 )                // Response...
    // Method begins at RVA 0x236c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebResponse::GetObjectData

  .method /*06000107*/ public hidebysig instance string 
          GetResponseHeader(string headerName) cil managed
  // SIG: 20 01 0E 0E
  {
    // Method begins at RVA 0x236e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::GetResponseHeader

  .method /*06000108*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetResponseStream() cil managed
  // SIG: 20 00 12 80 8D
  {
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpWebResponse::GetResponseStream

  .method /*06000109*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000BE:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 36 53 65 72 69 61 6C 69 7A 61 74 69 6F 6E   // ..6Serialization
                                                                                                                                                   20 68 61 73 20 62 65 65 6E 20 64 65 70 72 65 63   //  has been deprec
                                                                                                                                                   61 74 65 64 20 66 6F 72 20 48 74 74 70 57 65 62   // ated for HttpWeb
                                                                                                                                                   52 65 73 70 6F 6E 73 65 2E 00 00 )                // Response...
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/::GetObjectData /*0100001B::0A000014*/ 
    // Method begins at RVA 0x2374
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HttpWebResponse::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000066*/ instance callconv(8) string
          CharacterSet()
  {
    .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.HttpWebResponse/*02000010*/::get_CharacterSet() /* 060000F4 */
  } // end of property HttpWebResponse::CharacterSet
  .property /*17000067*/ instance callconv(8) string
          ContentEncoding()
  {
    .get instance string System.Net.HttpWebResponse/*02000010*/::get_ContentEncoding() /* 060000F5 */
  } // end of property HttpWebResponse::ContentEncoding
  .property /*17000068*/ instance callconv(8) int64
          ContentLength()
  {
    .get instance int64 System.Net.HttpWebResponse/*02000010*/::get_ContentLength() /* 060000F6 */
  } // end of property HttpWebResponse::ContentLength
  .property /*17000069*/ instance callconv(8) string
          ContentType()
  {
    .get instance string System.Net.HttpWebResponse/*02000010*/::get_ContentType() /* 060000F7 */
  } // end of property HttpWebResponse::ContentType
  .property /*1700006A*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.CookieCollection/*01000035*/
          Cookies()
  {
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.CookieCollection/*01000035*/ System.Net.HttpWebResponse/*02000010*/::get_Cookies() /* 060000F8 */
    .set instance void System.Net.HttpWebResponse/*02000010*/::set_Cookies(class [System.Net.Primitives/*23000003*/]System.Net.CookieCollection/*01000035*/) /* 060000F9 */
  } // end of property HttpWebResponse::Cookies
  .property /*1700006B*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/
          Headers()
  {
    .get instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ System.Net.HttpWebResponse/*02000010*/::get_Headers() /* 060000FA */
  } // end of property HttpWebResponse::Headers
  .property /*1700006C*/ instance callconv(8) bool
          IsMutuallyAuthenticated()
  {
    .get instance bool System.Net.HttpWebResponse/*02000010*/::get_IsMutuallyAuthenticated() /* 060000FB */
  } // end of property HttpWebResponse::IsMutuallyAuthenticated
  .property /*1700006D*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/
          LastModified()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ System.Net.HttpWebResponse/*02000010*/::get_LastModified() /* 060000FC */
  } // end of property HttpWebResponse::LastModified
  .property /*1700006E*/ instance callconv(8) string
          Method()
  {
    .get instance string System.Net.HttpWebResponse/*02000010*/::get_Method() /* 060000FD */
  } // end of property HttpWebResponse::Method
  .property /*1700006F*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Version/*01000030*/
          ProtocolVersion()
  {
    .get instance class [System.Runtime/*23000001*/]System.Version/*01000030*/ System.Net.HttpWebResponse/*02000010*/::get_ProtocolVersion() /* 060000FE */
  } // end of property HttpWebResponse::ProtocolVersion
  .property /*17000070*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          ResponseUri()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.HttpWebResponse/*02000010*/::get_ResponseUri() /* 060000FF */
  } // end of property HttpWebResponse::ResponseUri
  .property /*17000071*/ instance callconv(8) string
          Server()
  {
    .get instance string System.Net.HttpWebResponse/*02000010*/::get_Server() /* 06000100 */
  } // end of property HttpWebResponse::Server
  .property /*17000072*/ instance callconv(8) valuetype [System.Net.Primitives/*23000003*/]System.Net.HttpStatusCode/*01000036*/
          StatusCode()
  {
    .get instance valuetype [System.Net.Primitives/*23000003*/]System.Net.HttpStatusCode/*01000036*/ System.Net.HttpWebResponse/*02000010*/::get_StatusCode() /* 06000101 */
  } // end of property HttpWebResponse::StatusCode
  .property /*17000073*/ instance callconv(8) string
          StatusDescription()
  {
    .get instance string System.Net.HttpWebResponse/*02000010*/::get_StatusDescription() /* 06000102 */
  } // end of property HttpWebResponse::StatusDescription
  .property /*17000074*/ instance callconv(8) bool
          SupportsHeaders()
  {
    .get instance bool System.Net.HttpWebResponse/*02000010*/::get_SupportsHeaders() /* 06000103 */
  } // end of property HttpWebResponse::SupportsHeaders
} // end of class System.Net.HttpWebResponse

.class /*02000011*/ interface public abstract auto ansi System.Net.IAuthenticationModule
{
  .custom /*0C000049:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600010A*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_AuthenticationType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method IAuthenticationModule::get_AuthenticationType

  .method /*0600010B*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_CanPreAuthenticate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method IAuthenticationModule::get_CanPreAuthenticate

  .method /*0600010C*/ public hidebysig newslot abstract virtual 
          instance class System.Net.Authorization/*02000007*/ 
          Authenticate(string challenge,
                       class System.Net.WebRequest/*02000017*/ 'request',
                       class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ credentials) cil managed
  // SIG: 20 03 12 1C 0E 12 5C 12 69
  {
    .param [0]/*080000A9*/ 
    .custom /*0C00009D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAuthenticationModule::Authenticate

  .method /*0600010D*/ public hidebysig newslot abstract virtual 
          instance class System.Net.Authorization/*02000007*/ 
          PreAuthenticate(class System.Net.WebRequest/*02000017*/ 'request',
                          class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ credentials) cil managed
  // SIG: 20 02 12 1C 12 5C 12 69
  {
    .param [0]/*080000AD*/ 
    .custom /*0C00009E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method IAuthenticationModule::PreAuthenticate

  .property /*17000075*/ instance callconv(8) string
          AuthenticationType()
  {
    .get instance string System.Net.IAuthenticationModule/*02000011*/::get_AuthenticationType() /* 0600010A */
  } // end of property IAuthenticationModule::AuthenticationType
  .property /*17000076*/ instance callconv(8) bool
          CanPreAuthenticate()
  {
    .get instance bool System.Net.IAuthenticationModule/*02000011*/::get_CanPreAuthenticate() /* 0600010B */
  } // end of property IAuthenticationModule::CanPreAuthenticate
} // end of class System.Net.IAuthenticationModule

.class /*02000012*/ interface public abstract auto ansi System.Net.ICredentialPolicy
{
  .custom /*0C00004A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600010E*/ public hidebysig newslot abstract virtual 
          instance bool  ShouldSendCredential(class [System.Runtime/*23000001*/]System.Uri/*01000020*/ challengeUri,
                                              class System.Net.WebRequest/*02000017*/ 'request',
                                              class [System.Net.Primitives/*23000003*/]System.Net.NetworkCredential/*01000037*/ credential,
                                              class System.Net.IAuthenticationModule/*02000011*/ authenticationModule) cil managed
  // SIG: 20 04 02 12 80 81 12 5C 12 80 DD 12 44
  {
    // Method begins at RVA 0x0
  } // end of method ICredentialPolicy::ShouldSendCredential

} // end of class System.Net.ICredentialPolicy

.class /*02000013*/ interface public abstract auto ansi System.Net.IWebRequestCreate
{
  .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*0600010F*/ public hidebysig newslot abstract virtual 
          instance class System.Net.WebRequest/*02000017*/ 
          Create(class [System.Runtime/*23000001*/]System.Uri/*01000020*/ uri) cil managed
  // SIG: 20 01 12 5C 12 80 81
  {
    // Method begins at RVA 0x0
  } // end of method IWebRequestCreate::Create

} // end of class System.Net.IWebRequestCreate

.class /*02000014*/ public auto ansi beforefieldinit System.Net.ProtocolViolationException
       extends [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
{
  .method /*06000110*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2376
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProtocolViolationException::.ctor

  .method /*06000111*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000BF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x237e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProtocolViolationException::.ctor

  .method /*06000112*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2386
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ProtocolViolationException::.ctor

  .method /*06000113*/ public hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x238e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ProtocolViolationException::GetObjectData

  .method /*06000114*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/::GetObjectData /*0100001B::0A000014*/ 
    // Method begins at RVA 0x2390
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ProtocolViolationException::System.Runtime.Serialization.ISerializable.GetObjectData

} // end of class System.Net.ProtocolViolationException

.class /*02000015*/ public auto ansi beforefieldinit System.Net.WebException
       extends [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
{
  .custom /*0C00004E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00004F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000115*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2392
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebException::.ctor

  .method /*06000116*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000C2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x239a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebException::.ctor

  .method /*06000117*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23a2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebException::.ctor

  .method /*06000118*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000039*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 E5
  {
    // Method begins at RVA 0x23aa
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebException::.ctor

  .method /*06000119*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*01000039*/ innerException,
                               valuetype System.Net.WebExceptionStatus/*02000016*/ status,
                               class System.Net.WebResponse/*02000019*/ response) cil managed
  // SIG: 20 04 01 0E 12 80 E5 11 58 12 64
  {
    // Method begins at RVA 0x23b2
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebException::.ctor

  .method /*0600011A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               valuetype System.Net.WebExceptionStatus/*02000016*/ status) cil managed
  // SIG: 20 02 01 0E 11 58
  {
    // Method begins at RVA 0x23ba
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.InvalidOperationException/*01000038*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebException::.ctor

  .method /*0600011B*/ public hidebysig specialname 
          instance class System.Net.WebResponse/*02000019*/ 
          get_Response() cil managed
  // SIG: 20 00 12 64
  {
    // Method begins at RVA 0x23c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebException::get_Response

  .method /*0600011C*/ public hidebysig specialname 
          instance valuetype System.Net.WebExceptionStatus/*02000016*/ 
          get_Status() cil managed
  // SIG: 20 00 11 58
  {
    // Method begins at RVA 0x23c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebException::get_Status

  .method /*0600011D*/ public hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebException::GetObjectData

  .method /*0600011E*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/::GetObjectData /*0100001B::0A000014*/ 
    // Method begins at RVA 0x23ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebException::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000077*/ instance callconv(8) class System.Net.WebResponse/*02000019*/
          Response()
  {
    .get instance class System.Net.WebResponse/*02000019*/ System.Net.WebException/*02000015*/::get_Response() /* 0600011B */
  } // end of property WebException::Response
  .property /*17000078*/ instance callconv(8) valuetype System.Net.WebExceptionStatus/*02000016*/
          Status()
  {
    .get instance valuetype System.Net.WebExceptionStatus/*02000016*/ System.Net.WebException/*02000015*/::get_Status() /* 0600011C */
  } // end of property WebException::Status
} // end of class System.Net.WebException

.class /*02000016*/ public auto ansi sealed System.Net.WebExceptionStatus
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*0400002A*/ public specialname rtspecialname int32 value__
  .field /*0400002B*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ Success = int32(0x00000000)
  .field /*0400002C*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ NameResolutionFailure = int32(0x00000001)
  .field /*0400002D*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ ConnectFailure = int32(0x00000002)
  .field /*0400002E*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ ReceiveFailure = int32(0x00000003)
  .field /*0400002F*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ SendFailure = int32(0x00000004)
  .field /*04000030*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ PipelineFailure = int32(0x00000005)
  .field /*04000031*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ RequestCanceled = int32(0x00000006)
  .field /*04000032*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ ProtocolError = int32(0x00000007)
  .field /*04000033*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ ConnectionClosed = int32(0x00000008)
  .field /*04000034*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ TrustFailure = int32(0x00000009)
  .field /*04000035*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ SecureChannelFailure = int32(0x0000000A)
  .field /*04000036*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ ServerProtocolViolation = int32(0x0000000B)
  .field /*04000037*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ KeepAliveFailure = int32(0x0000000C)
  .field /*04000038*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ Pending = int32(0x0000000D)
  .field /*04000039*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ Timeout = int32(0x0000000E)
  .field /*0400003A*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ ProxyNameResolutionFailure = int32(0x0000000F)
  .field /*0400003B*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ UnknownError = int32(0x00000010)
  .field /*0400003C*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ MessageLengthLimitExceeded = int32(0x00000011)
  .field /*0400003D*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ CacheEntryNotFound = int32(0x00000012)
  .field /*0400003E*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ RequestProhibitedByCachePolicy = int32(0x00000013)
  .field /*0400003F*/ public static literal valuetype System.Net.WebExceptionStatus/*02000016*/ RequestProhibitedByProxy = int32(0x00000014)
} // end of class System.Net.WebExceptionStatus

.class /*02000017*/ public abstract auto ansi beforefieldinit System.Net.WebRequest
       extends [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003A*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
{
  .custom /*0C000050:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000051:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600011F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C0000C4:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x23cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebRequest::.ctor

  .method /*06000120*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .custom /*0C0000C5:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x23d4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebRequest::.ctor

  .method /*06000121*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000003*/]System.Net.Security.AuthenticationLevel/*0100003B*/ 
          get_AuthenticationLevel() cil managed
  // SIG: 20 00 11 80 ED
  {
    // Method begins at RVA 0x23dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_AuthenticationLevel

  .method /*06000122*/ public hidebysig specialname 
          instance void  set_AuthenticationLevel(valuetype [System.Net.Primitives/*23000003*/]System.Net.Security.AuthenticationLevel/*0100003B*/ 'value') cil managed
  // SIG: 20 01 01 11 80 ED
  {
    // Method begins at RVA 0x23df
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_AuthenticationLevel

  .method /*06000123*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ 
          get_CachePolicy() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C0000C6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_CachePolicy

  .method /*06000124*/ public hidebysig newslot specialname virtual 
          instance void  set_CachePolicy(class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A1
  {
    .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_CachePolicy

  .method /*06000125*/ public hidebysig newslot specialname virtual 
          instance string  get_ConnectionGroupName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_ConnectionGroupName

  .method /*06000126*/ public hidebysig newslot specialname virtual 
          instance void  set_ConnectionGroupName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000C9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_ConnectionGroupName

  .method /*06000127*/ public hidebysig newslot specialname virtual 
          instance int64  get_ContentLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x23eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_ContentLength

  .method /*06000128*/ public hidebysig newslot specialname virtual 
          instance void  set_ContentLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x23ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_ContentLength

  .method /*06000129*/ public hidebysig newslot specialname virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000CA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_ContentType

  .method /*0600012A*/ public hidebysig newslot specialname virtual 
          instance void  set_ContentType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_ContentType

  .method /*0600012B*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 69
  {
    .custom /*0C0000CC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_Credentials

  .method /*0600012C*/ public hidebysig newslot specialname virtual 
          instance void  set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ 'value') cil managed
  // SIG: 20 01 01 12 69
  {
    .custom /*0C0000CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000D2*/ 
    .custom /*0C0000A8:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000027*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x23f8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_Credentials

  .method /*0600012D*/ public hidebysig specialname static 
          class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ 
          get_DefaultCachePolicy() cil managed
  // SIG: 00 00 12 80 A1
  {
    .custom /*0C0000CE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_DefaultCachePolicy

  .method /*0600012E*/ public hidebysig specialname static 
          void  set_DefaultCachePolicy(class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ 'value') cil managed
  // SIG: 00 01 01 12 80 A1
  {
    .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23fd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_DefaultCachePolicy

  .method /*0600012F*/ public hidebysig specialname static 
          class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 
          get_DefaultWebProxy() cil managed
  // SIG: 00 00 12 7D
  {
    .custom /*0C0000D0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_DefaultWebProxy

  .method /*06000130*/ public hidebysig specialname static 
          void  set_DefaultWebProxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 'value') cil managed
  // SIG: 00 01 01 12 7D
  {
    .custom /*0C0000D1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2402
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_DefaultWebProxy

  .method /*06000131*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x2404
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_Headers

  .method /*06000132*/ public hidebysig newslot specialname virtual 
          instance void  set_Headers(class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 'value') cil managed
  // SIG: 20 01 01 12 79
  {
    // Method begins at RVA 0x2407
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_Headers

  .method /*06000133*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100003C*/ 
          get_ImpersonationLevel() cil managed
  // SIG: 20 00 11 80 F1
  {
    // Method begins at RVA 0x2409
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_ImpersonationLevel

  .method /*06000134*/ public hidebysig specialname 
          instance void  set_ImpersonationLevel(valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100003C*/ 'value') cil managed
  // SIG: 20 01 01 11 80 F1
  {
    // Method begins at RVA 0x240c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_ImpersonationLevel

  .method /*06000135*/ public hidebysig newslot specialname virtual 
          instance string  get_Method() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x240e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_Method

  .method /*06000136*/ public hidebysig newslot specialname virtual 
          instance void  set_Method(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2411
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_Method

  .method /*06000137*/ public hidebysig newslot specialname virtual 
          instance bool  get_PreAuthenticate() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2413
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_PreAuthenticate

  .method /*06000138*/ public hidebysig newslot specialname virtual 
          instance void  set_PreAuthenticate(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2416
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_PreAuthenticate

  .method /*06000139*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 
          get_Proxy() cil managed
  // SIG: 20 00 12 7D
  {
    .custom /*0C0000D2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2418
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_Proxy

  .method /*0600013A*/ public hidebysig newslot specialname virtual 
          instance void  set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 'value') cil managed
  // SIG: 20 01 01 12 7D
  {
    .custom /*0C0000D3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x241b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_Proxy

  .method /*0600013B*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_RequestUri() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x241d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_RequestUri

  .method /*0600013C*/ public hidebysig newslot specialname virtual 
          instance int32  get_Timeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2420
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_Timeout

  .method /*0600013D*/ public hidebysig newslot specialname virtual 
          instance void  set_Timeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2423
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_Timeout

  .method /*0600013E*/ public hidebysig newslot specialname virtual 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2425
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::get_UseDefaultCredentials

  .method /*0600013F*/ public hidebysig newslot specialname virtual 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2428
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::set_UseDefaultCredentials

  .method /*06000140*/ public hidebysig newslot virtual 
          instance void  Abort() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x242a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::Abort

  .method /*06000141*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginGetRequestStream(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                                object state) cil managed
  // SIG: 20 02 12 80 85 12 80 89 1C
  {
    .custom /*0C0000D4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000DC*/ 
    .custom /*0C0000AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x242c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::BeginGetRequestStream

  .method /*06000142*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ 
          BeginGetResponse(class [System.Runtime/*23000001*/]System.AsyncCallback/*01000022*/ callback,
                           object state) cil managed
  // SIG: 20 02 12 80 85 12 80 89 1C
  {
    .custom /*0C0000D5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000DF*/ 
    .custom /*0C0000AB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x242f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::BeginGetResponse

  .method /*06000143*/ public hidebysig static 
          class System.Net.WebRequest/*02000017*/ 
          Create(string requestUriString) cil managed
  // SIG: 00 01 12 5C 0E
  {
    .custom /*0C0000D6:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x2432
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::Create

  .method /*06000144*/ public hidebysig static 
          class System.Net.WebRequest/*02000017*/ 
          Create(class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri) cil managed
  // SIG: 00 01 12 5C 12 80 81
  {
    .custom /*0C0000D7:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x2435
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::Create

  .method /*06000145*/ public hidebysig static 
          class System.Net.WebRequest/*02000017*/ 
          CreateDefault(class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri) cil managed
  // SIG: 00 01 12 5C 12 80 81
  {
    .custom /*0C0000D8:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x2438
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::CreateDefault

  .method /*06000146*/ public hidebysig static 
          class System.Net.HttpWebRequest/*0200000F*/ 
          CreateHttp(string requestUriString) cil managed
  // SIG: 00 01 12 3C 0E
  {
    .custom /*0C0000D9:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x243b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::CreateHttp

  .method /*06000147*/ public hidebysig static 
          class System.Net.HttpWebRequest/*0200000F*/ 
          CreateHttp(class [System.Runtime/*23000001*/]System.Uri/*01000020*/ requestUri) cil managed
  // SIG: 00 01 12 3C 12 80 81
  {
    .custom /*0C0000DA:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000013 */ = ( 01 00 5D 57 65 62 52 65 71 75 65 73 74 2C 20 48   // ..]WebRequest, H
                                                                                                                                                   74 74 70 57 65 62 52 65 71 75 65 73 74 2C 20 53   // ttpWebRequest, S
                                                                                                                                                   65 72 76 69 63 65 50 6F 69 6E 74 2C 20 61 6E 64   // ervicePoint, and
                                                                                                                                                   20 57 65 62 43 6C 69 65 6E 74 20 61 72 65 20 6F   //  WebClient are o
                                                                                                                                                   62 73 6F 6C 65 74 65 2E 20 55 73 65 20 48 74 74   // bsolete. Use Htt
                                                                                                                                                   70 43 6C 69 65 6E 74 20 69 6E 73 74 65 61 64 2E   // pClient instead.
                                                                                                                                                   02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49   // ..T..DiagnosticI
                                                                                                                                                   64 0A 53 59 53 4C 49 42 30 30 31 34 54 0E 09 55   // d.SYSLIB0014T..U
                                                                                                                                                   72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F   // rlFormat"https:/
                                                                                                                                                   2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77   // /aka.ms/dotnet-w
                                                                                                                                                   61 72 6E 69 6E 67 73 2F 7B 30 7D )                // arnings/{0}
    // Method begins at RVA 0x243e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::CreateHttp

  .method /*06000148*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          EndGetRequestStream(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult) cil managed
  // SIG: 20 01 12 80 8D 12 80 85
  {
    // Method begins at RVA 0x2441
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::EndGetRequestStream

  .method /*06000149*/ public hidebysig newslot virtual 
          instance class System.Net.WebResponse/*02000019*/ 
          EndGetResponse(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000021*/ asyncResult) cil managed
  // SIG: 20 01 12 64 12 80 85
  {
    // Method begins at RVA 0x2444
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::EndGetResponse

  .method /*0600014A*/ family hidebysig newslot virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    // Method begins at RVA 0x2447
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::GetObjectData

  .method /*0600014B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetRequestStream() cil managed
  // SIG: 20 00 12 80 8D
  {
    // Method begins at RVA 0x2449
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::GetRequestStream

  .method /*0600014C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/> 
          GetRequestStreamAsync() cil managed
  // SIG: 20 00 15 12 80 91 01 12 80 8D
  {
    // Method begins at RVA 0x244c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::GetRequestStreamAsync

  .method /*0600014D*/ public hidebysig newslot virtual 
          instance class System.Net.WebResponse/*02000019*/ 
          GetResponse() cil managed
  // SIG: 20 00 12 64
  {
    // Method begins at RVA 0x244f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::GetResponse

  .method /*0600014E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000024*/<class System.Net.WebResponse/*02000019*/> 
          GetResponseAsync() cil managed
  // SIG: 20 00 15 12 80 91 01 12 64
  {
    // Method begins at RVA 0x2452
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::GetResponseAsync

  .method /*0600014F*/ public hidebysig static 
          class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ 
          GetSystemWebProxy() cil managed
  // SIG: 00 00 12 7D
  {
    // Method begins at RVA 0x2455
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::GetSystemWebProxy

  .method /*06000150*/ public hidebysig static 
          bool  RegisterPrefix(string prefix,
                               class System.Net.IWebRequestCreate/*02000013*/ creator) cil managed
  // SIG: 00 02 02 0E 12 4C
  {
    // Method begins at RVA 0x2458
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebRequest::RegisterPrefix

  .method /*06000151*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/::GetObjectData /*0100001B::0A000014*/ 
    // Method begins at RVA 0x245b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebRequest::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000079*/ instance callconv(8) valuetype [System.Net.Primitives/*23000003*/]System.Net.Security.AuthenticationLevel/*0100003B*/
          AuthenticationLevel()
  {
    .get instance valuetype [System.Net.Primitives/*23000003*/]System.Net.Security.AuthenticationLevel/*0100003B*/ System.Net.WebRequest/*02000017*/::get_AuthenticationLevel() /* 06000121 */
    .set instance void System.Net.WebRequest/*02000017*/::set_AuthenticationLevel(valuetype [System.Net.Primitives/*23000003*/]System.Net.Security.AuthenticationLevel/*0100003B*/) /* 06000122 */
  } // end of property WebRequest::AuthenticationLevel
  .property /*1700007A*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/
          CachePolicy()
  {
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ System.Net.WebRequest/*02000017*/::get_CachePolicy() /* 06000123 */
    .set instance void System.Net.WebRequest/*02000017*/::set_CachePolicy(class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/) /* 06000124 */
  } // end of property WebRequest::CachePolicy
  .property /*1700007B*/ instance callconv(8) string
          ConnectionGroupName()
  {
    .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.WebRequest/*02000017*/::get_ConnectionGroupName() /* 06000125 */
    .set instance void System.Net.WebRequest/*02000017*/::set_ConnectionGroupName(string) /* 06000126 */
  } // end of property WebRequest::ConnectionGroupName
  .property /*1700007C*/ instance callconv(8) int64
          ContentLength()
  {
    .get instance int64 System.Net.WebRequest/*02000017*/::get_ContentLength() /* 06000127 */
    .set instance void System.Net.WebRequest/*02000017*/::set_ContentLength(int64) /* 06000128 */
  } // end of property WebRequest::ContentLength
  .property /*1700007D*/ instance callconv(8) string
          ContentType()
  {
    .custom /*0C00008D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.WebRequest/*02000017*/::get_ContentType() /* 06000129 */
    .set instance void System.Net.WebRequest/*02000017*/::set_ContentType(string) /* 0600012A */
  } // end of property WebRequest::ContentType
  .property /*1700007E*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/
          Credentials()
  {
    .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/ System.Net.WebRequest/*02000017*/::get_Credentials() /* 0600012B */
    .set instance void System.Net.WebRequest/*02000017*/::set_Credentials(class [System.Net.Primitives/*23000003*/]System.Net.ICredentials/*0100001A*/) /* 0600012C */
  } // end of property WebRequest::Credentials
  .property /*1700007F*/ callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/
          DefaultCachePolicy()
  {
    .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/ System.Net.WebRequest/*02000017*/::get_DefaultCachePolicy() /* 0600012D */
    .set void System.Net.WebRequest/*02000017*/::set_DefaultCachePolicy(class [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/) /* 0600012E */
  } // end of property WebRequest::DefaultCachePolicy
  .property /*17000080*/ callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/
          DefaultWebProxy()
  {
    .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ System.Net.WebRequest/*02000017*/::get_DefaultWebProxy() /* 0600012F */
    .set void System.Net.WebRequest/*02000017*/::set_DefaultWebProxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/) /* 06000130 */
  } // end of property WebRequest::DefaultWebProxy
  .property /*17000081*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/
          Headers()
  {
    .get instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ System.Net.WebRequest/*02000017*/::get_Headers() /* 06000131 */
    .set instance void System.Net.WebRequest/*02000017*/::set_Headers(class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/) /* 06000132 */
  } // end of property WebRequest::Headers
  .property /*17000082*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100003C*/
          ImpersonationLevel()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100003C*/ System.Net.WebRequest/*02000017*/::get_ImpersonationLevel() /* 06000133 */
    .set instance void System.Net.WebRequest/*02000017*/::set_ImpersonationLevel(valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100003C*/) /* 06000134 */
  } // end of property WebRequest::ImpersonationLevel
  .property /*17000083*/ instance callconv(8) string
          Method()
  {
    .get instance string System.Net.WebRequest/*02000017*/::get_Method() /* 06000135 */
    .set instance void System.Net.WebRequest/*02000017*/::set_Method(string) /* 06000136 */
  } // end of property WebRequest::Method
  .property /*17000084*/ instance callconv(8) bool
          PreAuthenticate()
  {
    .get instance bool System.Net.WebRequest/*02000017*/::get_PreAuthenticate() /* 06000137 */
    .set instance void System.Net.WebRequest/*02000017*/::set_PreAuthenticate(bool) /* 06000138 */
  } // end of property WebRequest::PreAuthenticate
  .property /*17000085*/ instance callconv(8) class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/
          Proxy()
  {
    .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/ System.Net.WebRequest/*02000017*/::get_Proxy() /* 06000139 */
    .set instance void System.Net.WebRequest/*02000017*/::set_Proxy(class [System.Net.Primitives/*23000003*/]System.Net.IWebProxy/*0100001F*/) /* 0600013A */
  } // end of property WebRequest::Proxy
  .property /*17000086*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          RequestUri()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.WebRequest/*02000017*/::get_RequestUri() /* 0600013B */
  } // end of property WebRequest::RequestUri
  .property /*17000087*/ instance callconv(8) int32
          Timeout()
  {
    .get instance int32 System.Net.WebRequest/*02000017*/::get_Timeout() /* 0600013C */
    .set instance void System.Net.WebRequest/*02000017*/::set_Timeout(int32) /* 0600013D */
  } // end of property WebRequest::Timeout
  .property /*17000088*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .get instance bool System.Net.WebRequest/*02000017*/::get_UseDefaultCredentials() /* 0600013E */
    .set instance void System.Net.WebRequest/*02000017*/::set_UseDefaultCredentials(bool) /* 0600013F */
  } // end of property WebRequest::UseDefaultCredentials
} // end of class System.Net.WebRequest

.class /*02000018*/ public abstract auto ansi sealed beforefieldinit System.Net.WebRequestMethods
       extends [System.Runtime/*23000001*/]System.Object/*01000016*/
{
  .custom /*0C000052:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200001D*/ abstract auto ansi sealed nested public beforefieldinit File
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C00005A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000051*/ public static literal string DownloadFile = "GET"
    .field /*04000052*/ public static literal string UploadFile = "PUT"
  } // end of class File

  .class /*0200001E*/ abstract auto ansi sealed nested public beforefieldinit Ftp
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000053*/ public static literal string AppendFile = "APPE"
    .field /*04000054*/ public static literal string DeleteFile = "DELE"
    .field /*04000055*/ public static literal string DownloadFile = "RETR"
    .field /*04000056*/ public static literal string GetDateTimestamp = "MDTM"
    .field /*04000057*/ public static literal string GetFileSize = "SIZE"
    .field /*04000058*/ public static literal string ListDirectory = "NLST"
    .field /*04000059*/ public static literal string ListDirectoryDetails = "LIST"
    .field /*0400005A*/ public static literal string MakeDirectory = "MKD"
    .field /*0400005B*/ public static literal string PrintWorkingDirectory = "PWD"
    .field /*0400005C*/ public static literal string RemoveDirectory = "RMD"
    .field /*0400005D*/ public static literal string Rename = "RENAME"
    .field /*0400005E*/ public static literal string UploadFile = "STOR"
    .field /*0400005F*/ public static literal string UploadFileWithUniqueName = "STOU"
  } // end of class Ftp

  .class /*0200001F*/ abstract auto ansi sealed nested public beforefieldinit Http
         extends [System.Runtime/*23000001*/]System.Object/*01000016*/
  {
    .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000060*/ public static literal string Connect = "CONNECT"
    .field /*04000061*/ public static literal string Get = "GET"
    .field /*04000062*/ public static literal string Head = "HEAD"
    .field /*04000063*/ public static literal string MkCol = "MKCOL"
    .field /*04000064*/ public static literal string Post = "POST"
    .field /*04000065*/ public static literal string Put = "PUT"
  } // end of class Http

} // end of class System.Net.WebRequestMethods

.class /*02000019*/ public abstract auto ansi beforefieldinit System.Net.WebResponse
       extends [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003A*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100002A*/,
                  [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
{
  .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100002A*/
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/
  .custom /*0C000038:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000152*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x245d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebResponse::.ctor

  .method /*06000153*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    // Method begins at RVA 0x2465
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Runtime/*23000001*/]System.MarshalByRefObject/*0100003A*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method WebResponse::.ctor

  .method /*06000154*/ public hidebysig newslot specialname virtual 
          instance int64  get_ContentLength() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x246d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebResponse::get_ContentLength

  .method /*06000155*/ public hidebysig newslot specialname virtual 
          instance void  set_ContentLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2470
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebResponse::set_ContentLength

  .method /*06000156*/ public hidebysig newslot specialname virtual 
          instance string  get_ContentType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2472
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebResponse::get_ContentType

  .method /*06000157*/ public hidebysig newslot specialname virtual 
          instance void  set_ContentType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2475
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebResponse::set_ContentType

  .method /*06000158*/ public hidebysig newslot specialname virtual 
          instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 79
  {
    // Method begins at RVA 0x2477
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebResponse::get_Headers

  .method /*06000159*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsFromCache() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x247a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebResponse::get_IsFromCache

  .method /*0600015A*/ public hidebysig newslot specialname virtual 
          instance bool  get_IsMutuallyAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x247d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebResponse::get_IsMutuallyAuthenticated

  .method /*0600015B*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ 
          get_ResponseUri() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x2480
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebResponse::get_ResponseUri

  .method /*0600015C*/ public hidebysig newslot specialname virtual 
          instance bool  get_SupportsHeaders() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2483
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebResponse::get_SupportsHeaders

  .method /*0600015D*/ public hidebysig newslot virtual 
          instance void  Close() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2486
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebResponse::Close

  .method /*0600015E*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2488
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebResponse::Dispose

  .method /*0600015F*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x248a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebResponse::Dispose

  .method /*06000160*/ family hidebysig newslot virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    // Method begins at RVA 0x248c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebResponse::GetObjectData

  .method /*06000161*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000023*/ 
          GetResponseStream() cil managed
  // SIG: 20 00 12 80 8D
  {
    // Method begins at RVA 0x248e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method WebResponse::GetResponseStream

  .method /*06000162*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001C*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001D*/ streamingContext) cil managed
  // SIG: 20 02 01 12 71 11 75
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001B*/::GetObjectData /*0100001B::0A000014*/ 
    // Method begins at RVA 0x2491
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method WebResponse::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*17000089*/ instance callconv(8) int64
          ContentLength()
  {
    .get instance int64 System.Net.WebResponse/*02000019*/::get_ContentLength() /* 06000154 */
    .set instance void System.Net.WebResponse/*02000019*/::set_ContentLength(int64) /* 06000155 */
  } // end of property WebResponse::ContentLength
  .property /*1700008A*/ instance callconv(8) string
          ContentType()
  {
    .get instance string System.Net.WebResponse/*02000019*/::get_ContentType() /* 06000156 */
    .set instance void System.Net.WebResponse/*02000019*/::set_ContentType(string) /* 06000157 */
  } // end of property WebResponse::ContentType
  .property /*1700008B*/ instance callconv(8) class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/
          Headers()
  {
    .get instance class [System.Net.WebHeaderCollection/*23000004*/]System.Net.WebHeaderCollection/*0100001E*/ System.Net.WebResponse/*02000019*/::get_Headers() /* 06000158 */
  } // end of property WebResponse::Headers
  .property /*1700008C*/ instance callconv(8) bool
          IsFromCache()
  {
    .get instance bool System.Net.WebResponse/*02000019*/::get_IsFromCache() /* 06000159 */
  } // end of property WebResponse::IsFromCache
  .property /*1700008D*/ instance callconv(8) bool
          IsMutuallyAuthenticated()
  {
    .get instance bool System.Net.WebResponse/*02000019*/::get_IsMutuallyAuthenticated() /* 0600015A */
  } // end of property WebResponse::IsMutuallyAuthenticated
  .property /*1700008E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*01000020*/
          ResponseUri()
  {
    .get instance class [System.Runtime/*23000001*/]System.Uri/*01000020*/ System.Net.WebResponse/*02000019*/::get_ResponseUri() /* 0600015B */
  } // end of property WebResponse::ResponseUri
  .property /*1700008F*/ instance callconv(8) bool
          SupportsHeaders()
  {
    .get instance bool System.Net.WebResponse/*02000019*/::get_SupportsHeaders() /* 0600015C */
  } // end of property WebResponse::SupportsHeaders
} // end of class System.Net.WebResponse

.class /*0200001A*/ public auto ansi sealed System.Net.Cache.HttpCacheAgeControl
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*04000040*/ public specialname rtspecialname int32 value__
  .field /*04000041*/ public static literal valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ None = int32(0x00000000)
  .field /*04000042*/ public static literal valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ MinFresh = int32(0x00000001)
  .field /*04000043*/ public static literal valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ MaxAge = int32(0x00000002)
  .field /*04000044*/ public static literal valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ MaxAgeAndMinFresh = int32(0x00000003)
  .field /*04000045*/ public static literal valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ MaxStale = int32(0x00000004)
  .field /*04000046*/ public static literal valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ MaxAgeAndMaxStale = int32(0x00000006)
} // end of class System.Net.Cache.HttpCacheAgeControl

.class /*0200001B*/ public auto ansi sealed System.Net.Cache.HttpRequestCacheLevel
       extends [System.Runtime/*23000001*/]System.Enum/*01000025*/
{
  .field /*04000047*/ public specialname rtspecialname int32 value__
  .field /*04000048*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ Default = int32(0x00000000)
  .field /*04000049*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ BypassCache = int32(0x00000001)
  .field /*0400004A*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ CacheOnly = int32(0x00000002)
  .field /*0400004B*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ CacheIfAvailable = int32(0x00000003)
  .field /*0400004C*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ Revalidate = int32(0x00000004)
  .field /*0400004D*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ Reload = int32(0x00000005)
  .field /*0400004E*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ NoCacheNoStore = int32(0x00000006)
  .field /*0400004F*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ CacheOrNextCacheOnly = int32(0x00000007)
  .field /*04000050*/ public static literal valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ Refresh = int32(0x00000008)
} // end of class System.Net.Cache.HttpRequestCacheLevel

.class /*0200001C*/ public auto ansi beforefieldinit System.Net.Cache.HttpRequestCachePolicy
       extends [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/
{
  .method /*06000163*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2493
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestCachePolicy::.ctor

  .method /*06000164*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ cacheSyncDate) cil managed
  // SIG: 20 01 01 11 80 AD
  {
    // Method begins at RVA 0x249b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestCachePolicy::.ctor

  .method /*06000165*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ cacheAgeControl,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ ageOrFreshOrStale) cil managed
  // SIG: 20 02 01 11 68 11 80 F5
  {
    // Method begins at RVA 0x24a3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestCachePolicy::.ctor

  .method /*06000166*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ cacheAgeControl,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ maxAge,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ freshOrStale) cil managed
  // SIG: 20 03 01 11 68 11 80 F5 11 80 F5
  {
    // Method begins at RVA 0x24ab
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestCachePolicy::.ctor

  .method /*06000167*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Cache.HttpCacheAgeControl/*0200001A*/ cacheAgeControl,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ maxAge,
                               valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ freshOrStale,
                               valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ cacheSyncDate) cil managed
  // SIG: 20 04 01 11 68 11 80 F5 11 80 F5 11 80 AD
  {
    // Method begins at RVA 0x24b3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestCachePolicy::.ctor

  .method /*06000168*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ level) cil managed
  // SIG: 20 01 01 11 6C
  {
    // Method begins at RVA 0x24bb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Net.Primitives/*23000003*/]System.Net.Cache.RequestCachePolicy/*01000028*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HttpRequestCachePolicy::.ctor

  .method /*06000169*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ 
          get_CacheSyncDate() cil managed
  // SIG: 20 00 11 80 AD
  {
    // Method begins at RVA 0x24c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestCachePolicy::get_CacheSyncDate

  .method /*0600016A*/ public hidebysig specialname 
          instance valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ 
          get_Level() cil managed
  // SIG: 20 00 11 6C
  {
    // Method begins at RVA 0x24c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestCachePolicy::get_Level

  .method /*0600016B*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ 
          get_MaxAge() cil managed
  // SIG: 20 00 11 80 F5
  {
    // Method begins at RVA 0x24c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestCachePolicy::get_MaxAge

  .method /*0600016C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ 
          get_MaxStale() cil managed
  // SIG: 20 00 11 80 F5
  {
    // Method begins at RVA 0x24cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestCachePolicy::get_MaxStale

  .method /*0600016D*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ 
          get_MinFresh() cil managed
  // SIG: 20 00 11 80 F5
  {
    // Method begins at RVA 0x24cf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestCachePolicy::get_MinFresh

  .method /*0600016E*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000DB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24d2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HttpRequestCachePolicy::ToString

  .property /*17000090*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/
          CacheSyncDate()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*0100002B*/ System.Net.Cache.HttpRequestCachePolicy/*0200001C*/::get_CacheSyncDate() /* 06000169 */
  } // end of property HttpRequestCachePolicy::CacheSyncDate
  .property /*17000091*/ instance callconv(8) valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/
          Level()
  {
    .get instance valuetype System.Net.Cache.HttpRequestCacheLevel/*0200001B*/ System.Net.Cache.HttpRequestCachePolicy/*0200001C*/::get_Level() /* 0600016A */
  } // end of property HttpRequestCachePolicy::Level
  .property /*17000092*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/
          MaxAge()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ System.Net.Cache.HttpRequestCachePolicy/*0200001C*/::get_MaxAge() /* 0600016B */
  } // end of property HttpRequestCachePolicy::MaxAge
  .property /*17000093*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/
          MaxStale()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ System.Net.Cache.HttpRequestCachePolicy/*0200001C*/::get_MaxStale() /* 0600016C */
  } // end of property HttpRequestCachePolicy::MaxStale
  .property /*17000094*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/
          MinFresh()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.TimeSpan/*0100003D*/ System.Net.Cache.HttpRequestCachePolicy/*0200001C*/::get_MinFresh() /* 0600016D */
  } // end of property HttpRequestCachePolicy::MinFresh
} // end of class System.Net.Cache.HttpRequestCachePolicy


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
