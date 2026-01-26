


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
// Time-date stamp:            0xa578fc88
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00003a00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x000058a6
// Base of code:                   0x00002000
// Base of data:                   0x00006000
// Image base:                     0x10000000
// Section alignment:              0x00002000
// File alignment:                 0x00000200
// Major OS version:               0x0004
// Minor OS version:               0x0000
// Major image version:            0x0000
// Minor image version:            0x0000
// Major subsystem version:        0x0004
// Minor subsystem version:        0x0000
// Size of image:                  0x0000a000
// Size of headers:                0x00000200
// Checksum:                       0x00013595
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x00005851 [0x0000004f] address [size] of Import Directory:          
// 0x00006000 [0x000004e4] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00004400 [0x00002470] address [size] of Security Directory:        
// 0x00008000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x00005754 [0x00000054] address [size] of Debug Directory:           
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
//              0x000038ac Virtual Size
//              0x00002000 Virtual Address
//              0x00003a00 Size of Raw Data
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
//              0x000004e4 Virtual Size
//              0x00006000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00003c00 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x40000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_READ

//              .reloc
//              0x0000000c Virtual Size
//              0x00008000 Virtual Address
//              0x00000200 Size of Raw Data
//              0x00004200 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x00005000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x000008a8
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00005893 Import Name Table
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
// 0x00002384 [0x00003350] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x000056d4 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00001cd4 Size
//              '#~' Name
//    Stream 2:
//              0x00001d40 Offset
//              0x00000df8 Size
//              '#Strings' Name
//    Stream 3:
//              0x00002b38 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00002b3c Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00002b4c Offset
//              0x00000804 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000000901a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 26736
// PE header size       : 512 (496 used)    ( 1.92%)
// PE additional info   : 10835             (40.53%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.27%)
// CLR meta-data size  : 13136              (49.13%)
// CLR additional info : 128                ( 0.48%)
// CLR method headers  : 219                ( 0.82%)
// Managed code         : 600               ( 2.24%)
// Data                 : 2048              ( 7.66%)
// Unaccounted          : -814              (-3.04%)

// Num.of PE sections   : 3
//   .text    - 14848
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 13136
//   Module        -    1 (10 bytes)
//   TypeDef       -   23 (322 bytes)     1 interfaces, 0 explicit layout
//   TypeRef       -   56 (336 bytes)
//   MethodDef     -  222 (3108 bytes)    3 abstract, 0 native, 219 bodies
//   FieldDef      -   20 (120 bytes)     0 constant
//   MemberRef     -   38 (228 bytes)
//   ParamDef      -  211 (1266 bytes)
//   Constant      -   11 (66 bytes)
//   CustomAttribute-  146 (876 bytes)
//   InterfaceImpl -    2 (8 bytes)
//   PropertyMap   -    9 (36 bytes)
//   Property      -   55 (330 bytes)
//   MethodSemantic-   81 (486 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    4 (80 bytes)
//   Strings       -  3576 bytes
//   Blobs         -  2052 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   194 bytes

// CLR additional info : 128

// CLR method headers : 219
//   Num.of method bodies  - 219
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 219

// Managed code : 600
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class AesCryptoServiceProvider       (public) (auto) (ansi) (sealed) 
// Class CspKeyContainerInfo            (public) (auto) (ansi) (sealed) 
// Class CspParameters                  (public) (auto) (ansi) (sealed) 
// Class CspProviderFlags               (public) (auto) (ansi) (sealed) 
// Class DESCryptoServiceProvider       (public) (auto) (ansi) (sealed) 
// Class DSACryptoServiceProvider       (public) (auto) (ansi) (sealed) 
// Interface ICspAsymmetricAlgorithm        (public) (abstract) (auto) (ansi) 
// Class KeyNumber                      (public) (auto) (ansi) (sealed) 
// Class MD5CryptoServiceProvider       (public) (auto) (ansi) (sealed) 
// Class PasswordDeriveBytes            (public) (auto) (ansi) 
// Class RC2CryptoServiceProvider       (public) (auto) (ansi) (sealed) 
// Class RNGCryptoServiceProvider       (public) (auto) (ansi) (sealed) 
// Class RSACryptoServiceProvider       (public) (auto) (ansi) (sealed) 
// Class SHA1CryptoServiceProvider      (public) (auto) (ansi) (sealed) 
// Class SHA256CryptoServiceProvider    (public) (auto) (ansi) (sealed) 
// Class SHA384CryptoServiceProvider    (public) (auto) (ansi) (sealed) 
// Class SHA512CryptoServiceProvider    (public) (auto) (ansi) (sealed) 
// Class TripleDESCryptoServiceProvider (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Security.Cryptography.Algorithms
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Security.Cryptography.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Runtime.InteropServices
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Security.Cryptography.Csp
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

  .custom /*0C000006:0A000004*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.TargetFrameworkAttribute/*01000005*/::.ctor(string) /* 0A000004 */ = ( 01 00 18 2E 4E 45 54 43 6F 72 65 41 70 70 2C 56   // ....NETCoreApp,V
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 36 2E 30 01 00 54 0E 14   // ersion=v6.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 00 )                                  // Name.
  .custom /*0C000007:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000008:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000009*/::.ctor(string) /* 0A000008 */ = ( 01 00 20 53 79 73 74 65 6D 2E 53 65 63 75 72 69   // .. System.Securi
                                                                                                                                                                        74 79 2E 43 72 79 70 74 6F 67 72 61 70 68 79 2E   // ty.Cryptography.
                                                                                                                                                                        43 73 70 00 00 )                                  // Csp..
  .custom /*0C00000B:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
                                                                                                                                         string) /* 0A000009 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 20 53 79 73 74 65 6D 2E 53 65 63 75 72 69   // .. System.Securi
                                                                                                                                                                       74 79 2E 43 72 79 70 74 6F 67 72 61 70 68 79 2E   // ty.Cryptography.
                                                                                                                                                                       43 73 70 00 00 )                                  // Csp..
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 20 53 79 73 74 65 6D 2E 53 65 63 75 72 69   // .. System.Securi
                                                                                                                                                                 74 79 2E 43 72 79 70 74 6F 67 72 61 70 68 79 2E   // ty.Cryptography.
                                                                                                                                                                 43 73 70 00 00 )                                  // Csp..
  .custom /*0C000016:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*0100000A*/::.ctor(string,
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
.module System.Security.Cryptography.Csp.dll
// MVID: {25fe88b7-fbf2-4326-85b3-5f5cb1f43bd3}
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
  .custom /*0C000017:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
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
    // Method begins at RVA 0x2058
    // Code size       23 (0x17)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000018 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000038       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000038*/
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
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000018 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C00001D:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000018 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000013*/
{
  .custom /*0C000020:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000012*/::.ctor() /* 0A000011 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000012*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000015*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000014*/) /* 0A000012 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000013*/::.ctor() /* 0A000018 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.AesCryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.Aes/*01000019*/
{
  .custom /*0C000023:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000024:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000025:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C000026:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.Aes/*01000019*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::.ctor

  .method /*06000007*/ public hidebysig specialname virtual 
          instance int32  get_BlockSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_BlockSize

  .method /*06000008*/ public hidebysig specialname virtual 
          instance void  set_BlockSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::set_BlockSize

  .method /*06000009*/ public hidebysig specialname virtual 
          instance int32  get_FeedbackSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_FeedbackSize

  .method /*0600000A*/ public hidebysig specialname virtual 
          instance void  set_FeedbackSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20ad
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::set_FeedbackSize

  .method /*0600000B*/ public hidebysig specialname virtual 
          instance uint8[]  get_IV() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x20af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_IV

  .method /*0600000C*/ public hidebysig specialname virtual 
          instance void  set_IV(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x20b2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::set_IV

  .method /*0600000D*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x20b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_Key

  .method /*0600000E*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x20b7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::set_Key

  .method /*0600000F*/ public hidebysig specialname virtual 
          instance int32  get_KeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_KeySize

  .method /*06000010*/ public hidebysig specialname virtual 
          instance void  set_KeySize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20bc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::set_KeySize

  .method /*06000011*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] 
          get_LegalBlockSizes() cil managed
  // SIG: 20 00 1D 12 69
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_LegalBlockSizes

  .method /*06000012*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] 
          get_LegalKeySizes() cil managed
  // SIG: 20 00 1D 12 69
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_LegalKeySizes

  .method /*06000013*/ public hidebysig specialname virtual 
          instance valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_Mode

  .method /*06000014*/ public hidebysig specialname virtual 
          instance void  set_Mode(valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 11 6D
  {
    // Method begins at RVA 0x20c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::set_Mode

  .method /*06000015*/ public hidebysig specialname virtual 
          instance valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/ 
          get_Padding() cil managed
  // SIG: 20 00 11 71
  {
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::get_Padding

  .method /*06000016*/ public hidebysig specialname virtual 
          instance void  set_Padding(valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/ 'value') cil managed
  // SIG: 20 01 01 11 71
  {
    // Method begins at RVA 0x20cc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::set_Padding

  .method /*06000017*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateDecryptor() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::CreateDecryptor

  .method /*06000018*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateDecryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 75 1D 05 1D 05
  {
    .param [2]/*08000009*/ 
    .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::CreateDecryptor

  .method /*06000019*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateEncryptor() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::CreateEncryptor

  .method /*0600001A*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateEncryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 75 1D 05 1D 05
  {
    .param [2]/*0800000B*/ 
    .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCryptoServiceProvider::CreateEncryptor

  .method /*0600001B*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::Dispose

  .method /*0600001C*/ public hidebysig virtual 
          instance void  GenerateIV() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20dc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::GenerateIV

  .method /*0600001D*/ public hidebysig virtual 
          instance void  GenerateKey() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCryptoServiceProvider::GenerateKey

  .property /*17000001*/ instance callconv(8) int32
          BlockSize()
  {
    .get instance int32 System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_BlockSize() /* 06000007 */
    .set instance void System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::set_BlockSize(int32) /* 06000008 */
  } // end of property AesCryptoServiceProvider::BlockSize
  .property /*17000002*/ instance callconv(8) int32
          FeedbackSize()
  {
    .get instance int32 System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_FeedbackSize() /* 06000009 */
    .set instance void System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::set_FeedbackSize(int32) /* 0600000A */
  } // end of property AesCryptoServiceProvider::FeedbackSize
  .property /*17000003*/ instance callconv(8) uint8[]
          IV()
  {
    .get instance uint8[] System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_IV() /* 0600000B */
    .set instance void System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::set_IV(uint8[]) /* 0600000C */
  } // end of property AesCryptoServiceProvider::IV
  .property /*17000004*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_Key() /* 0600000D */
    .set instance void System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::set_Key(uint8[]) /* 0600000E */
  } // end of property AesCryptoServiceProvider::Key
  .property /*17000005*/ instance callconv(8) int32
          KeySize()
  {
    .get instance int32 System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_KeySize() /* 0600000F */
    .set instance void System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::set_KeySize(int32) /* 06000010 */
  } // end of property AesCryptoServiceProvider::KeySize
  .property /*17000006*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[]
          LegalBlockSizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_LegalBlockSizes() /* 06000011 */
  } // end of property AesCryptoServiceProvider::LegalBlockSizes
  .property /*17000007*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[]
          LegalKeySizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_LegalKeySizes() /* 06000012 */
  } // end of property AesCryptoServiceProvider::LegalKeySizes
  .property /*17000008*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/
          Mode()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/ System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_Mode() /* 06000013 */
    .set instance void System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::set_Mode(valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/) /* 06000014 */
  } // end of property AesCryptoServiceProvider::Mode
  .property /*17000009*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/
          Padding()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/ System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::get_Padding() /* 06000015 */
    .set instance void System.Security.Cryptography.AesCryptoServiceProvider/*02000006*/::set_Padding(valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/) /* 06000016 */
  } // end of property AesCryptoServiceProvider::Padding
} // end of class System.Security.Cryptography.AesCryptoServiceProvider

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.CspKeyContainerInfo
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
{
  .custom /*0C000027:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000029:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .method /*0600001E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Security.Cryptography.CspParameters/*02000008*/ parameters) cil managed
  // SIG: 20 01 01 12 20
  {
    .custom /*0C000058:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20e0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CspKeyContainerInfo::.ctor

  .method /*0600001F*/ public hidebysig specialname 
          instance bool  get_Accessible() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_Accessible

  .method /*06000020*/ public hidebysig specialname 
          instance bool  get_Exportable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_Exportable

  .method /*06000021*/ public hidebysig specialname 
          instance bool  get_HardwareDevice() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_HardwareDevice

  .method /*06000022*/ public hidebysig specialname 
          instance string  get_KeyContainerName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_KeyContainerName

  .method /*06000023*/ public hidebysig specialname 
          instance valuetype System.Security.Cryptography.KeyNumber/*0200000D*/ 
          get_KeyNumber() cil managed
  // SIG: 20 00 11 34
  {
    // Method begins at RVA 0x20f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_KeyNumber

  .method /*06000024*/ public hidebysig specialname 
          instance bool  get_MachineKeyStore() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_MachineKeyStore

  .method /*06000025*/ public hidebysig specialname 
          instance bool  get_Protected() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_Protected

  .method /*06000026*/ public hidebysig specialname 
          instance string  get_ProviderName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_ProviderName

  .method /*06000027*/ public hidebysig specialname 
          instance int32  get_ProviderType() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_ProviderType

  .method /*06000028*/ public hidebysig specialname 
          instance bool  get_RandomlyGenerated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_RandomlyGenerated

  .method /*06000029*/ public hidebysig specialname 
          instance bool  get_Removable() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_Removable

  .method /*0600002A*/ public hidebysig specialname 
          instance string  get_UniqueKeyContainerName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00005C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspKeyContainerInfo::get_UniqueKeyContainerName

  .property /*1700000A*/ instance callconv(8) bool
          Accessible()
  {
    .get instance bool System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_Accessible() /* 0600001F */
  } // end of property CspKeyContainerInfo::Accessible
  .property /*1700000B*/ instance callconv(8) bool
          Exportable()
  {
    .get instance bool System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_Exportable() /* 06000020 */
  } // end of property CspKeyContainerInfo::Exportable
  .property /*1700000C*/ instance callconv(8) bool
          HardwareDevice()
  {
    .get instance bool System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_HardwareDevice() /* 06000021 */
  } // end of property CspKeyContainerInfo::HardwareDevice
  .property /*1700000D*/ instance callconv(8) string
          KeyContainerName()
  {
    .get instance string System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_KeyContainerName() /* 06000022 */
  } // end of property CspKeyContainerInfo::KeyContainerName
  .property /*1700000E*/ instance callconv(8) valuetype System.Security.Cryptography.KeyNumber/*0200000D*/
          KeyNumber()
  {
    .get instance valuetype System.Security.Cryptography.KeyNumber/*0200000D*/ System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_KeyNumber() /* 06000023 */
  } // end of property CspKeyContainerInfo::KeyNumber
  .property /*1700000F*/ instance callconv(8) bool
          MachineKeyStore()
  {
    .get instance bool System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_MachineKeyStore() /* 06000024 */
  } // end of property CspKeyContainerInfo::MachineKeyStore
  .property /*17000010*/ instance callconv(8) bool
          Protected()
  {
    .get instance bool System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_Protected() /* 06000025 */
  } // end of property CspKeyContainerInfo::Protected
  .property /*17000011*/ instance callconv(8) string
          ProviderName()
  {
    .get instance string System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_ProviderName() /* 06000026 */
  } // end of property CspKeyContainerInfo::ProviderName
  .property /*17000012*/ instance callconv(8) int32
          ProviderType()
  {
    .get instance int32 System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_ProviderType() /* 06000027 */
  } // end of property CspKeyContainerInfo::ProviderType
  .property /*17000013*/ instance callconv(8) bool
          RandomlyGenerated()
  {
    .get instance bool System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_RandomlyGenerated() /* 06000028 */
  } // end of property CspKeyContainerInfo::RandomlyGenerated
  .property /*17000014*/ instance callconv(8) bool
          Removable()
  {
    .get instance bool System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_Removable() /* 06000029 */
  } // end of property CspKeyContainerInfo::Removable
  .property /*17000015*/ instance callconv(8) string
          UniqueKeyContainerName()
  {
    .custom /*0C00004C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Security.Cryptography.CspKeyContainerInfo/*02000007*/::get_UniqueKeyContainerName() /* 0600002A */
  } // end of property CspKeyContainerInfo::UniqueKeyContainerName
} // end of class System.Security.Cryptography.CspKeyContainerInfo

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.CspParameters
       extends [System.Runtime/*23000001*/]System.Object/*0100001F*/
{
  .custom /*0C00002A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002C:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .field /*04000004*/ public string KeyContainerName
  .field /*04000005*/ public int32 KeyNumber
  .field /*04000006*/ public string ProviderName
  .field /*04000007*/ public int32 ProviderType
  .method /*0600002B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x210c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CspParameters::.ctor

  .method /*0600002C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dwTypeIn) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2114
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CspParameters::.ctor

  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dwTypeIn,
                               string strProviderNameIn) cil managed
  // SIG: 20 02 01 08 0E
  {
    // Method begins at RVA 0x211c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CspParameters::.ctor

  .method /*0600002E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dwTypeIn,
                               string strProviderNameIn,
                               string strContainerNameIn) cil managed
  // SIG: 20 03 01 08 0E 0E
  {
    // Method begins at RVA 0x2124
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001F*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CspParameters::.ctor

  .method /*0600002F*/ public hidebysig specialname 
          instance valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ 
          get_Flags() cil managed
  // SIG: 20 00 11 24
  {
    // Method begins at RVA 0x212c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspParameters::get_Flags

  .method /*06000030*/ public hidebysig specialname 
          instance void  set_Flags(valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 11 24
  {
    // Method begins at RVA 0x212f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CspParameters::set_Flags

  .method /*06000031*/ public hidebysig specialname 
          instance class [System.Runtime.InteropServices/*23000004*/]System.Security.SecureString/*01000020*/ 
          get_KeyPassword() cil managed
  // SIG: 20 00 12 80 81
  {
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspParameters::get_KeyPassword

  .method /*06000032*/ public hidebysig specialname 
          instance void  set_KeyPassword(class [System.Runtime.InteropServices/*23000004*/]System.Security.SecureString/*01000020*/ 'value') cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x2134
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CspParameters::set_KeyPassword

  .method /*06000033*/ public hidebysig specialname 
          instance native int  get_ParentWindowHandle() cil managed
  // SIG: 20 00 18
  {
    // Method begins at RVA 0x2136
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CspParameters::get_ParentWindowHandle

  .method /*06000034*/ public hidebysig specialname 
          instance void  set_ParentWindowHandle(native int 'value') cil managed
  // SIG: 20 01 01 18
  {
    // Method begins at RVA 0x2139
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CspParameters::set_ParentWindowHandle

  .property /*17000016*/ instance callconv(8) valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/
          Flags()
  {
    .get instance valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ System.Security.Cryptography.CspParameters/*02000008*/::get_Flags() /* 0600002F */
    .set instance void System.Security.Cryptography.CspParameters/*02000008*/::set_Flags(valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/) /* 06000030 */
  } // end of property CspParameters::Flags
  .property /*17000017*/ instance callconv(8) class [System.Runtime.InteropServices/*23000004*/]System.Security.SecureString/*01000020*/
          KeyPassword()
  {
    .custom /*0C000053:0A000006*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000007*/::.ctor(bool) /* 0A000006 */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Runtime.InteropServices/*23000004*/]System.Security.SecureString/*01000020*/ System.Security.Cryptography.CspParameters/*02000008*/::get_KeyPassword() /* 06000031 */
    .set instance void System.Security.Cryptography.CspParameters/*02000008*/::set_KeyPassword(class [System.Runtime.InteropServices/*23000004*/]System.Security.SecureString/*01000020*/) /* 06000032 */
  } // end of property CspParameters::KeyPassword
  .property /*17000018*/ instance callconv(8) native int
          ParentWindowHandle()
  {
    .get instance native int System.Security.Cryptography.CspParameters/*02000008*/::get_ParentWindowHandle() /* 06000033 */
    .set instance void System.Security.Cryptography.CspParameters/*02000008*/::set_ParentWindowHandle(native int) /* 06000034 */
  } // end of property CspParameters::ParentWindowHandle
} // end of class System.Security.Cryptography.CspParameters

.class /*02000009*/ public auto ansi sealed System.Security.Cryptography.CspProviderFlags
       extends [System.Runtime/*23000001*/]System.Enum/*01000022*/
{
  .custom /*0C00002D:0A000016*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000021*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
  .field /*04000008*/ public specialname rtspecialname int32 value__
  .field /*04000009*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ NoFlags = int32(0x00000000)
  .field /*0400000A*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ UseMachineKeyStore = int32(0x00000001)
  .field /*0400000B*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ UseDefaultKeyContainer = int32(0x00000002)
  .field /*0400000C*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ UseNonExportableKey = int32(0x00000004)
  .field /*0400000D*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ UseExistingKey = int32(0x00000008)
  .field /*0400000E*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ UseArchivableKey = int32(0x00000010)
  .field /*0400000F*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ UseUserProtectedKey = int32(0x00000020)
  .field /*04000010*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ NoPrompt = int32(0x00000040)
  .field /*04000011*/ public static literal valuetype System.Security.Cryptography.CspProviderFlags/*02000009*/ CreateEphemeralKey = int32(0x00000080)
} // end of class System.Security.Cryptography.CspProviderFlags

.class /*0200000A*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.DESCryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DES/*01000023*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000031:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C000032:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000035*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DES/*01000023*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DESCryptoServiceProvider::.ctor

  .method /*06000036*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateDecryptor() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2143
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DESCryptoServiceProvider::CreateDecryptor

  .method /*06000037*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateDecryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 75 1D 05 1D 05
  {
    .param [2]/*08000018*/ 
    .custom /*0C000054:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2146
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DESCryptoServiceProvider::CreateDecryptor

  .method /*06000038*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateEncryptor() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2149
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DESCryptoServiceProvider::CreateEncryptor

  .method /*06000039*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateEncryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 75 1D 05 1D 05
  {
    .param [2]/*0800001A*/ 
    .custom /*0C000056:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DESCryptoServiceProvider::CreateEncryptor

  .method /*0600003A*/ public hidebysig virtual 
          instance void  GenerateIV() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DESCryptoServiceProvider::GenerateIV

  .method /*0600003B*/ public hidebysig virtual 
          instance void  GenerateKey() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2151
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DESCryptoServiceProvider::GenerateKey

} // end of class System.Security.Cryptography.DESCryptoServiceProvider

.class /*0200000B*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.DSACryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DSA/*01000024*/
       implements System.Security.Cryptography.ICspAsymmetricAlgorithm/*0200000C*/
{
  .custom /*0C000033:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Security.Cryptography.ICspAsymmetricAlgorithm/*0200000C*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600003C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2153
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DSA/*01000024*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::.ctor

  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dwKeySize) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x215b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DSA/*01000024*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::.ctor

  .method /*0600003E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dwKeySize,
                               class System.Security.Cryptography.CspParameters/*02000008*/ parameters) cil managed
  // SIG: 20 02 01 08 12 20
  {
    .custom /*0C00005F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000060:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2163
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DSA/*01000024*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::.ctor

  .method /*0600003F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Security.Cryptography.CspParameters/*02000008*/ parameters) cil managed
  // SIG: 20 01 01 12 20
  {
    .custom /*0C000061:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000062:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x216b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DSA/*01000024*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::.ctor

  .method /*06000040*/ public hidebysig newslot specialname virtual final 
          instance class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/ 
          get_CspKeyContainerInfo() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x2173
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::get_CspKeyContainerInfo

  .method /*06000041*/ public hidebysig specialname virtual 
          instance string  get_KeyExchangeAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000063:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2176
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::get_KeyExchangeAlgorithm

  .method /*06000042*/ public hidebysig specialname virtual 
          instance int32  get_KeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2179
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::get_KeySize

  .method /*06000043*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] 
          get_LegalKeySizes() cil managed
  // SIG: 20 00 1D 12 69
  {
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::get_LegalKeySizes

  .method /*06000044*/ public hidebysig specialname 
          instance bool  get_PersistKeyInCsp() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::get_PersistKeyInCsp

  .method /*06000045*/ public hidebysig specialname 
          instance void  set_PersistKeyInCsp(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2182
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::set_PersistKeyInCsp

  .method /*06000046*/ public hidebysig specialname 
          instance bool  get_PublicOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2184
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::get_PublicOnly

  .method /*06000047*/ public hidebysig specialname virtual 
          instance string  get_SignatureAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2187
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::get_SignatureAlgorithm

  .method /*06000048*/ public hidebysig specialname static 
          bool  get_UseMachineKeyStore() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x218a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::get_UseMachineKeyStore

  .method /*06000049*/ public hidebysig specialname static 
          void  set_UseMachineKeyStore(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x218d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::set_UseMachineKeyStore

  .method /*0600004A*/ public hidebysig virtual 
          instance uint8[]  CreateSignature(uint8[] rgbHash) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::CreateSignature

  .method /*0600004B*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2192
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::Dispose

  .method /*0600004C*/ public hidebysig newslot virtual final 
          instance uint8[]  ExportCspBlob(bool includePrivateParameters) cil managed
  // SIG: 20 01 1D 05 02
  {
    // Method begins at RVA 0x2194
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::ExportCspBlob

  .method /*0600004D*/ public hidebysig virtual 
          instance valuetype [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DSAParameters/*01000025*/ 
          ExportParameters(bool includePrivateParameters) cil managed
  // SIG: 20 01 11 80 95 02
  {
    // Method begins at RVA 0x2197
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::ExportParameters

  .method /*0600004E*/ family hidebysig virtual 
          instance uint8[]  HashData(uint8[] data,
                                     int32 offset,
                                     int32 count,
                                     valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.HashAlgorithmName/*01000026*/ hashAlgorithm) cil managed
  // SIG: 20 04 1D 05 1D 05 08 08 11 80 99
  {
    // Method begins at RVA 0x219a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::HashData

  .method /*0600004F*/ family hidebysig virtual 
          instance uint8[]  HashData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.HashAlgorithmName/*01000026*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 12 80 9D 11 80 99
  {
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::HashData

  .method /*06000050*/ public hidebysig newslot virtual final 
          instance void  ImportCspBlob(uint8[] keyBlob) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x21a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::ImportCspBlob

  .method /*06000051*/ public hidebysig virtual 
          instance void  ImportParameters(valuetype [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DSAParameters/*01000025*/ parameters) cil managed
  // SIG: 20 01 01 11 80 95
  {
    // Method begins at RVA 0x21a2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSACryptoServiceProvider::ImportParameters

  .method /*06000052*/ public hidebysig instance uint8[] 
          SignData(uint8[] buffer) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x21a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::SignData

  .method /*06000053*/ public hidebysig instance uint8[] 
          SignData(uint8[] buffer,
                   int32 offset,
                   int32 count) cil managed
  // SIG: 20 03 1D 05 1D 05 08 08
  {
    // Method begins at RVA 0x21a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::SignData

  .method /*06000054*/ public hidebysig instance uint8[] 
          SignData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ inputStream) cil managed
  // SIG: 20 01 1D 05 12 80 9D
  {
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::SignData

  .method /*06000055*/ public hidebysig instance uint8[] 
          SignHash(uint8[] rgbHash,
                   string str) cil managed
  // SIG: 20 02 1D 05 1D 05 0E
  {
    .param [2]/*08000033*/ 
    .custom /*0C00005D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::SignHash

  .method /*06000056*/ public hidebysig instance bool 
          VerifyData(uint8[] rgbData,
                     uint8[] rgbSignature) cil managed
  // SIG: 20 02 02 1D 05 1D 05
  {
    // Method begins at RVA 0x21b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::VerifyData

  .method /*06000057*/ public hidebysig instance bool 
          VerifyHash(uint8[] rgbHash,
                     string str,
                     uint8[] rgbSignature) cil managed
  // SIG: 20 03 02 1D 05 0E 1D 05
  {
    .param [2]/*08000037*/ 
    .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::VerifyHash

  .method /*06000058*/ public hidebysig virtual 
          instance bool  VerifySignature(uint8[] rgbHash,
                                         uint8[] rgbSignature) cil managed
  // SIG: 20 02 02 1D 05 1D 05
  {
    // Method begins at RVA 0x21b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSACryptoServiceProvider::VerifySignature

  .property /*17000019*/ instance callconv(8) class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/
          CspKeyContainerInfo()
  {
    .custom /*0C000055:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .get instance class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/ System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::get_CspKeyContainerInfo() /* 06000040 */
  } // end of property DSACryptoServiceProvider::CspKeyContainerInfo
  .property /*1700001A*/ instance callconv(8) string
          KeyExchangeAlgorithm()
  {
    .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::get_KeyExchangeAlgorithm() /* 06000041 */
  } // end of property DSACryptoServiceProvider::KeyExchangeAlgorithm
  .property /*1700001B*/ instance callconv(8) int32
          KeySize()
  {
    .get instance int32 System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::get_KeySize() /* 06000042 */
  } // end of property DSACryptoServiceProvider::KeySize
  .property /*1700001C*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[]
          LegalKeySizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::get_LegalKeySizes() /* 06000043 */
  } // end of property DSACryptoServiceProvider::LegalKeySizes
  .property /*1700001D*/ instance callconv(8) bool
          PersistKeyInCsp()
  {
    .get instance bool System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::get_PersistKeyInCsp() /* 06000044 */
    .set instance void System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::set_PersistKeyInCsp(bool) /* 06000045 */
  } // end of property DSACryptoServiceProvider::PersistKeyInCsp
  .property /*1700001E*/ instance callconv(8) bool
          PublicOnly()
  {
    .get instance bool System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::get_PublicOnly() /* 06000046 */
  } // end of property DSACryptoServiceProvider::PublicOnly
  .property /*1700001F*/ instance callconv(8) string
          SignatureAlgorithm()
  {
    .get instance string System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::get_SignatureAlgorithm() /* 06000047 */
  } // end of property DSACryptoServiceProvider::SignatureAlgorithm
  .property /*17000020*/ callconv(8) bool
          UseMachineKeyStore()
  {
    .get bool System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::get_UseMachineKeyStore() /* 06000048 */
    .set void System.Security.Cryptography.DSACryptoServiceProvider/*0200000B*/::set_UseMachineKeyStore(bool) /* 06000049 */
  } // end of property DSACryptoServiceProvider::UseMachineKeyStore
} // end of class System.Security.Cryptography.DSACryptoServiceProvider

.class /*0200000C*/ interface public abstract auto ansi System.Security.Cryptography.ICspAsymmetricAlgorithm
{
  .custom /*0C000036:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .method /*06000059*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/ 
          get_CspKeyContainerInfo() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x0
  } // end of method ICspAsymmetricAlgorithm::get_CspKeyContainerInfo

  .method /*0600005A*/ public hidebysig newslot abstract virtual 
          instance uint8[]  ExportCspBlob(bool includePrivateParameters) cil managed
  // SIG: 20 01 1D 05 02
  {
    // Method begins at RVA 0x0
  } // end of method ICspAsymmetricAlgorithm::ExportCspBlob

  .method /*0600005B*/ public hidebysig newslot abstract virtual 
          instance void  ImportCspBlob(uint8[] rawData) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method ICspAsymmetricAlgorithm::ImportCspBlob

  .property /*17000021*/ instance callconv(8) class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/
          CspKeyContainerInfo()
  {
    .get instance class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/ System.Security.Cryptography.ICspAsymmetricAlgorithm/*0200000C*/::get_CspKeyContainerInfo() /* 06000059 */
  } // end of property ICspAsymmetricAlgorithm::CspKeyContainerInfo
} // end of class System.Security.Cryptography.ICspAsymmetricAlgorithm

.class /*0200000D*/ public auto ansi sealed System.Security.Cryptography.KeyNumber
       extends [System.Runtime/*23000001*/]System.Enum/*01000022*/
{
  .field /*04000012*/ public specialname rtspecialname int32 value__
  .field /*04000013*/ public static literal valuetype System.Security.Cryptography.KeyNumber/*0200000D*/ Exchange = int32(0x00000001)
  .field /*04000014*/ public static literal valuetype System.Security.Cryptography.KeyNumber/*0200000D*/ Signature = int32(0x00000002)
} // end of class System.Security.Cryptography.KeyNumber

.class /*0200000E*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.MD5CryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.MD5/*01000028*/
{
  .custom /*0C000037:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C000038:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600005C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21b9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.MD5/*01000028*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MD5CryptoServiceProvider::.ctor

  .method /*0600005D*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21c1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MD5CryptoServiceProvider::Dispose

  .method /*0600005E*/ family hidebysig virtual 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C00006D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MD5CryptoServiceProvider::HashCore

  .method /*0600005F*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000029*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 80 A5 01 05
  {
    // Method begins at RVA 0x21c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MD5CryptoServiceProvider::HashCore

  .method /*06000060*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C00006F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MD5CryptoServiceProvider::HashFinal

  .method /*06000061*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MD5CryptoServiceProvider::Initialize

  .method /*06000062*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002A*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 80 A9 01 05 10 08
  {
    // Method begins at RVA 0x21cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MD5CryptoServiceProvider::TryHashFinal

} // end of class System.Security.Cryptography.MD5CryptoServiceProvider

.class /*0200000F*/ public auto ansi beforefieldinit System.Security.Cryptography.PasswordDeriveBytes
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/
{
  .custom /*0C000039:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003B:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] password,
                               uint8[] salt) cil managed
  // SIG: 20 02 01 1D 05 1D 05
  {
    .param [2]/*08000045*/ 
    .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21cf
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::.ctor

  .method /*06000064*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] password,
                               uint8[] salt,
                               class System.Security.Cryptography.CspParameters/*02000008*/ cspParams) cil managed
  // SIG: 20 03 01 1D 05 1D 05 12 20
  {
    .custom /*0C000071:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000046*/ 
    .custom /*0C000065:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::.ctor

  .method /*06000065*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] password,
                               uint8[] salt,
                               string hashName,
                               int32 iterations) cil managed
  // SIG: 20 04 01 1D 05 1D 05 0E 08
  {
    .custom /*0C000073:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002C*/::.ctor(string) /* 0A000017 */ = ( 01 00 5E 54 68 65 20 68 61 73 68 20 69 6D 70 6C   // ..^The hash impl
                                                                                                                                                                                            65 6D 65 6E 74 61 74 69 6F 6E 20 6D 69 67 68 74   // ementation might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 2E 20 45 6E 73   //  be removed. Ens
                                                                                                                                                                                            75 72 65 20 74 68 65 20 72 65 66 65 72 65 6E 63   // ure the referenc
                                                                                                                                                                                            65 64 20 68 61 73 68 20 61 6C 67 6F 72 69 74 68   // ed hash algorith
                                                                                                                                                                                            6D 20 69 73 20 6E 6F 74 20 74 72 69 6D 6D 65 64   // m is not trimmed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param [2]/*0800004A*/ 
    .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21df
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::.ctor

  .method /*06000066*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] password,
                               uint8[] salt,
                               string hashName,
                               int32 iterations,
                               class System.Security.Cryptography.CspParameters/*02000008*/ cspParams) cil managed
  // SIG: 20 05 01 1D 05 1D 05 0E 08 12 20
  {
    .custom /*0C000074:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002C*/::.ctor(string) /* 0A000017 */ = ( 01 00 5E 54 68 65 20 68 61 73 68 20 69 6D 70 6C   // ..^The hash impl
                                                                                                                                                                                            65 6D 65 6E 74 61 74 69 6F 6E 20 6D 69 67 68 74   // ementation might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 2E 20 45 6E 73   //  be removed. Ens
                                                                                                                                                                                            75 72 65 20 74 68 65 20 72 65 66 65 72 65 6E 63   // ure the referenc
                                                                                                                                                                                            65 64 20 68 61 73 68 20 61 6C 67 6F 72 69 74 68   // ed hash algorith
                                                                                                                                                                                            6D 20 69 73 20 6E 6F 74 20 74 72 69 6D 6D 65 64   // m is not trimmed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param [2]/*0800004E*/ 
    .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000051*/ 
    .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::.ctor

  .method /*06000067*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string strPassword,
                               uint8[] rgbSalt) cil managed
  // SIG: 20 02 01 0E 1D 05
  {
    .param [2]/*08000053*/ 
    .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ef
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::.ctor

  .method /*06000068*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string strPassword,
                               uint8[] rgbSalt,
                               class System.Security.Cryptography.CspParameters/*02000008*/ cspParams) cil managed
  // SIG: 20 03 01 0E 1D 05 12 20
  {
    .custom /*0C000075:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000054*/ 
    .custom /*0C00006A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::.ctor

  .method /*06000069*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string strPassword,
                               uint8[] rgbSalt,
                               string strHashName,
                               int32 iterations) cil managed
  // SIG: 20 04 01 0E 1D 05 0E 08
  {
    .custom /*0C000076:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002C*/::.ctor(string) /* 0A000017 */ = ( 01 00 5E 54 68 65 20 68 61 73 68 20 69 6D 70 6C   // ..^The hash impl
                                                                                                                                                                                            65 6D 65 6E 74 61 74 69 6F 6E 20 6D 69 67 68 74   // ementation might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 2E 20 45 6E 73   //  be removed. Ens
                                                                                                                                                                                            75 72 65 20 74 68 65 20 72 65 66 65 72 65 6E 63   // ure the referenc
                                                                                                                                                                                            65 64 20 68 61 73 68 20 61 6C 67 6F 72 69 74 68   // ed hash algorith
                                                                                                                                                                                            6D 20 69 73 20 6E 6F 74 20 74 72 69 6D 6D 65 64   // m is not trimmed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param [2]/*08000058*/ 
    .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ff
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::.ctor

  .method /*0600006A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string strPassword,
                               uint8[] rgbSalt,
                               string strHashName,
                               int32 iterations,
                               class System.Security.Cryptography.CspParameters/*02000008*/ cspParams) cil managed
  // SIG: 20 05 01 0E 1D 05 0E 08 12 20
  {
    .custom /*0C000077:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002C*/::.ctor(string) /* 0A000017 */ = ( 01 00 5E 54 68 65 20 68 61 73 68 20 69 6D 70 6C   // ..^The hash impl
                                                                                                                                                                                            65 6D 65 6E 74 61 74 69 6F 6E 20 6D 69 67 68 74   // ementation might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 2E 20 45 6E 73   //  be removed. Ens
                                                                                                                                                                                            75 72 65 20 74 68 65 20 72 65 66 65 72 65 6E 63   // ure the referenc
                                                                                                                                                                                            65 64 20 68 61 73 68 20 61 6C 67 6F 72 69 74 68   // ed hash algorith
                                                                                                                                                                                            6D 20 69 73 20 6E 6F 74 20 74 72 69 6D 6D 65 64   // m is not trimmed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    .param [2]/*0800005C*/ 
    .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800005F*/ 
    .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2207
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.DeriveBytes/*0100002B*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::.ctor

  .method /*0600006B*/ public hidebysig specialname 
          instance string  get_HashName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PasswordDeriveBytes::get_HashName

  .method /*0600006C*/ public hidebysig specialname 
          instance void  set_HashName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000078:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*0100002C*/::.ctor(string) /* 0A000017 */ = ( 01 00 5E 54 68 65 20 68 61 73 68 20 69 6D 70 6C   // ..^The hash impl
                                                                                                                                                                                            65 6D 65 6E 74 61 74 69 6F 6E 20 6D 69 67 68 74   // ementation might
                                                                                                                                                                                            20 62 65 20 72 65 6D 6F 76 65 64 2E 20 45 6E 73   //  be removed. Ens
                                                                                                                                                                                            75 72 65 20 74 68 65 20 72 65 66 65 72 65 6E 63   // ure the referenc
                                                                                                                                                                                            65 64 20 68 61 73 68 20 61 6C 67 6F 72 69 74 68   // ed hash algorith
                                                                                                                                                                                            6D 20 69 73 20 6E 6F 74 20 74 72 69 6D 6D 65 64   // m is not trimmed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    // Method begins at RVA 0x2212
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::set_HashName

  .method /*0600006D*/ public hidebysig specialname 
          instance int32  get_IterationCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PasswordDeriveBytes::get_IterationCount

  .method /*0600006E*/ public hidebysig specialname 
          instance void  set_IterationCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2217
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::set_IterationCount

  .method /*0600006F*/ public hidebysig specialname 
          instance uint8[]  get_Salt() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C00007B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2219
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PasswordDeriveBytes::get_Salt

  .method /*06000070*/ public hidebysig specialname 
          instance void  set_Salt(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    .custom /*0C00007C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::set_Salt

  .method /*06000071*/ public hidebysig instance uint8[] 
          CryptDeriveKey(string algname,
                         string alghashname,
                         int32 keySize,
                         uint8[] rgbIV) cil managed
  // SIG: 20 04 1D 05 0E 0E 08 1D 05
  {
    .custom /*0C00007D:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .param [1]/*08000063*/ 
    .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [2]/*08000064*/ 
    .custom /*0C000072:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PasswordDeriveBytes::CryptDeriveKey

  .method /*06000072*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2221
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::Dispose

  .method /*06000073*/ public hidebysig virtual 
          instance uint8[]  GetBytes(int32 cb) cil managed
  // SIG: 20 01 1D 05 08
  {
    .custom /*0C00007E:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 7F 52 66 63 32 38 39 38 44 65 72 69 76 65   // ...Rfc2898Derive
                                                                                                                                                   42 79 74 65 73 20 72 65 70 6C 61 63 65 73 20 50   // Bytes replaces P
                                                                                                                                                   61 73 73 77 6F 72 64 44 65 72 69 76 65 42 79 74   // asswordDeriveByt
                                                                                                                                                   65 73 20 66 6F 72 20 64 65 72 69 76 69 6E 67 20   // es for deriving 
                                                                                                                                                   6B 65 79 20 6D 61 74 65 72 69 61 6C 20 66 72 6F   // key material fro
                                                                                                                                                   6D 20 61 20 70 61 73 73 77 6F 72 64 20 61 6E 64   // m a password and
                                                                                                                                                   20 69 73 20 70 72 65 66 65 72 72 65 64 20 69 6E   //  is preferred in
                                                                                                                                                   20 6E 65 77 20 61 70 70 6C 69 63 61 74 69 6F 6E   //  new application
                                                                                                                                                   73 2E 00 00 )                                     // s...
    // Method begins at RVA 0x2223
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PasswordDeriveBytes::GetBytes

  .method /*06000074*/ public hidebysig virtual 
          instance void  Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2226
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PasswordDeriveBytes::Reset

  .property /*17000022*/ instance callconv(8) string
          HashName()
  {
    .get instance string System.Security.Cryptography.PasswordDeriveBytes/*0200000F*/::get_HashName() /* 0600006B */
    .set instance void System.Security.Cryptography.PasswordDeriveBytes/*0200000F*/::set_HashName(string) /* 0600006C */
  } // end of property PasswordDeriveBytes::HashName
  .property /*17000023*/ instance callconv(8) int32
          IterationCount()
  {
    .get instance int32 System.Security.Cryptography.PasswordDeriveBytes/*0200000F*/::get_IterationCount() /* 0600006D */
    .set instance void System.Security.Cryptography.PasswordDeriveBytes/*0200000F*/::set_IterationCount(int32) /* 0600006E */
  } // end of property PasswordDeriveBytes::IterationCount
  .property /*17000024*/ instance callconv(8) uint8[]
          Salt()
  {
    .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance uint8[] System.Security.Cryptography.PasswordDeriveBytes/*0200000F*/::get_Salt() /* 0600006F */
    .set instance void System.Security.Cryptography.PasswordDeriveBytes/*0200000F*/::set_Salt(uint8[]) /* 06000070 */
  } // end of property PasswordDeriveBytes::Salt
} // end of class System.Security.Cryptography.PasswordDeriveBytes

.class /*02000010*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.RC2CryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RC2/*0100002D*/
{
  .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C00003F:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000075*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2228
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RC2/*0100002D*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RC2CryptoServiceProvider::.ctor

  .method /*06000076*/ public hidebysig specialname virtual 
          instance int32  get_EffectiveKeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RC2CryptoServiceProvider::get_EffectiveKeySize

  .method /*06000077*/ public hidebysig specialname virtual 
          instance void  set_EffectiveKeySize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2233
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RC2CryptoServiceProvider::set_EffectiveKeySize

  .method /*06000078*/ public hidebysig specialname 
          instance bool  get_UseSalt() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2235
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RC2CryptoServiceProvider::get_UseSalt

  .method /*06000079*/ public hidebysig specialname 
          instance void  set_UseSalt(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    .custom /*0C00007F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2238
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RC2CryptoServiceProvider::set_UseSalt

  .method /*0600007A*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateDecryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 75 1D 05 1D 05
  {
    .param [2]/*0800006C*/ 
    .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RC2CryptoServiceProvider::CreateDecryptor

  .method /*0600007B*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateEncryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 75 1D 05 1D 05
  {
    .param [2]/*0800006E*/ 
    .custom /*0C00007A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x223d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RC2CryptoServiceProvider::CreateEncryptor

  .method /*0600007C*/ public hidebysig virtual 
          instance void  GenerateIV() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2240
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RC2CryptoServiceProvider::GenerateIV

  .method /*0600007D*/ public hidebysig virtual 
          instance void  GenerateKey() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2242
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RC2CryptoServiceProvider::GenerateKey

  .property /*17000025*/ instance callconv(8) int32
          EffectiveKeySize()
  {
    .get instance int32 System.Security.Cryptography.RC2CryptoServiceProvider/*02000010*/::get_EffectiveKeySize() /* 06000076 */
    .set instance void System.Security.Cryptography.RC2CryptoServiceProvider/*02000010*/::set_EffectiveKeySize(int32) /* 06000077 */
  } // end of property RC2CryptoServiceProvider::EffectiveKeySize
  .property /*17000026*/ instance callconv(8) bool
          UseSalt()
  {
    .get instance bool System.Security.Cryptography.RC2CryptoServiceProvider/*02000010*/::get_UseSalt() /* 06000078 */
    .set instance void System.Security.Cryptography.RC2CryptoServiceProvider/*02000010*/::set_UseSalt(bool) /* 06000079 */
  } // end of property RC2CryptoServiceProvider::UseSalt
} // end of class System.Security.Cryptography.RC2CryptoServiceProvider

.class /*02000011*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.RNGCryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RandomNumberGenerator/*0100002E*/
{
  .custom /*0C000040:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000041:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000042:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 7F 52 4E 47 43 72 79 70 74 6F 53 65 72 76   // ...RNGCryptoServ
                                                                                                                                                 69 63 65 50 72 6F 76 69 64 65 72 20 69 73 20 6F   // iceProvider is o
                                                                                                                                                 62 73 6F 6C 65 74 65 2E 20 54 6F 20 67 65 6E 65   // bsolete. To gene
                                                                                                                                                 72 61 74 65 20 61 20 72 61 6E 64 6F 6D 20 6E 75   // rate a random nu
                                                                                                                                                 6D 62 65 72 2C 20 75 73 65 20 6F 6E 65 20 6F 66   // mber, use one of
                                                                                                                                                 20 74 68 65 20 52 61 6E 64 6F 6D 4E 75 6D 62 65   //  the RandomNumbe
                                                                                                                                                 72 47 65 6E 65 72 61 74 6F 72 20 73 74 61 74 69   // rGenerator stati
                                                                                                                                                 63 20 6D 65 74 68 6F 64 73 20 69 6E 73 74 65 61   // c methods instea
                                                                                                                                                 64 2E 02 00 54 0E 0C 44 69 61 67 6E 6F 73 74 69   // d...T..Diagnosti
                                                                                                                                                 63 49 64 0A 53 59 53 4C 49 42 30 30 32 33 54 0E   // cId.SYSLIB0023T.
                                                                                                                                                 09 55 72 6C 46 6F 72 6D 61 74 22 68 74 74 70 73   // .UrlFormat"https
                                                                                                                                                 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74 6E 65 74   // ://aka.ms/dotnet
                                                                                                                                                 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D )          // -warnings/{0}
  .custom /*0C000043:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600007E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2244
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RandomNumberGenerator/*0100002E*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::.ctor

  .method /*0600007F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] rgb) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x224c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RandomNumberGenerator/*0100002E*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::.ctor

  .method /*06000080*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Security.Cryptography.CspParameters/*02000008*/ cspParams) cil managed
  // SIG: 20 01 01 12 20
  {
    .custom /*0C000080:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2254
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RandomNumberGenerator/*0100002E*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::.ctor

  .method /*06000081*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string str) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x225c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RandomNumberGenerator/*0100002E*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::.ctor

  .method /*06000082*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2264
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::Dispose

  .method /*06000083*/ public hidebysig virtual 
          instance void  GetBytes(uint8[] data) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2266
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::GetBytes

  .method /*06000084*/ public hidebysig virtual 
          instance void  GetBytes(uint8[] data,
                                  int32 offset,
                                  int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2268
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::GetBytes

  .method /*06000085*/ public hidebysig virtual 
          instance void  GetBytes(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002A*/<uint8> data) cil managed
  // SIG: 20 01 01 15 11 80 A9 01 05
  {
    .custom /*0C000081:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x226a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::GetBytes

  .method /*06000086*/ public hidebysig virtual 
          instance void  GetNonZeroBytes(uint8[] data) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x226c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::GetNonZeroBytes

  .method /*06000087*/ public hidebysig virtual 
          instance void  GetNonZeroBytes(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002A*/<uint8> data) cil managed
  // SIG: 20 01 01 15 11 80 A9 01 05
  {
    .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x226e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RNGCryptoServiceProvider::GetNonZeroBytes

} // end of class System.Security.Cryptography.RNGCryptoServiceProvider

.class /*02000012*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.RSACryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSA/*0100002F*/
       implements System.Security.Cryptography.ICspAsymmetricAlgorithm/*0200000C*/
{
  .custom /*0C000044:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000045:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type System.Security.Cryptography.ICspAsymmetricAlgorithm/*0200000C*/
  .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000088*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2270
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSA/*0100002F*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::.ctor

  .method /*06000089*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dwKeySize) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2278
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSA/*0100002F*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::.ctor

  .method /*0600008A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(int32 dwKeySize,
                               class System.Security.Cryptography.CspParameters/*02000008*/ parameters) cil managed
  // SIG: 20 02 01 08 12 20
  {
    .custom /*0C000083:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000084:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2280
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSA/*0100002F*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::.ctor

  .method /*0600008B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Security.Cryptography.CspParameters/*02000008*/ parameters) cil managed
  // SIG: 20 01 01 12 20
  {
    .custom /*0C000085:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000086:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    // Method begins at RVA 0x2288
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000021       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSA/*0100002F*/::.ctor() /* 0A000021 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::.ctor

  .method /*0600008C*/ public hidebysig newslot specialname virtual final 
          instance class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/ 
          get_CspKeyContainerInfo() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x2290
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::get_CspKeyContainerInfo

  .method /*0600008D*/ public hidebysig specialname virtual 
          instance string  get_KeyExchangeAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000087:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::get_KeyExchangeAlgorithm

  .method /*0600008E*/ public hidebysig specialname virtual 
          instance int32  get_KeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2296
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::get_KeySize

  .method /*0600008F*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] 
          get_LegalKeySizes() cil managed
  // SIG: 20 00 1D 12 69
  {
    // Method begins at RVA 0x2299
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::get_LegalKeySizes

  .method /*06000090*/ public hidebysig specialname 
          instance bool  get_PersistKeyInCsp() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x229c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::get_PersistKeyInCsp

  .method /*06000091*/ public hidebysig specialname 
          instance void  set_PersistKeyInCsp(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x229f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::set_PersistKeyInCsp

  .method /*06000092*/ public hidebysig specialname 
          instance bool  get_PublicOnly() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::get_PublicOnly

  .method /*06000093*/ public hidebysig specialname virtual 
          instance string  get_SignatureAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::get_SignatureAlgorithm

  .method /*06000094*/ public hidebysig specialname static 
          bool  get_UseMachineKeyStore() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x22a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::get_UseMachineKeyStore

  .method /*06000095*/ public hidebysig specialname static 
          void  set_UseMachineKeyStore(bool 'value') cil managed
  // SIG: 00 01 01 02
  {
    // Method begins at RVA 0x22aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::set_UseMachineKeyStore

  .method /*06000096*/ public hidebysig instance uint8[] 
          Decrypt(uint8[] rgb,
                  bool fOAEP) cil managed
  // SIG: 20 02 1D 05 1D 05 02
  {
    // Method begins at RVA 0x22ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::Decrypt

  .method /*06000097*/ public hidebysig virtual 
          instance uint8[]  Decrypt(uint8[] data,
                                    class [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSAEncryptionPadding/*01000030*/ padding) cil managed
  // SIG: 20 02 1D 05 1D 05 12 80 C1
  {
    // Method begins at RVA 0x22af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::Decrypt

  .method /*06000098*/ public hidebysig virtual 
          instance uint8[]  DecryptValue(uint8[] rgb) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x22b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::DecryptValue

  .method /*06000099*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22b5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::Dispose

  .method /*0600009A*/ public hidebysig instance uint8[] 
          Encrypt(uint8[] rgb,
                  bool fOAEP) cil managed
  // SIG: 20 02 1D 05 1D 05 02
  {
    // Method begins at RVA 0x22b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::Encrypt

  .method /*0600009B*/ public hidebysig virtual 
          instance uint8[]  Encrypt(uint8[] data,
                                    class [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSAEncryptionPadding/*01000030*/ padding) cil managed
  // SIG: 20 02 1D 05 1D 05 12 80 C1
  {
    // Method begins at RVA 0x22ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::Encrypt

  .method /*0600009C*/ public hidebysig virtual 
          instance uint8[]  EncryptValue(uint8[] rgb) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x22bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::EncryptValue

  .method /*0600009D*/ public hidebysig newslot virtual final 
          instance uint8[]  ExportCspBlob(bool includePrivateParameters) cil managed
  // SIG: 20 01 1D 05 02
  {
    // Method begins at RVA 0x22c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::ExportCspBlob

  .method /*0600009E*/ public hidebysig virtual 
          instance valuetype [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSAParameters/*01000031*/ 
          ExportParameters(bool includePrivateParameters) cil managed
  // SIG: 20 01 11 80 C5 02
  {
    // Method begins at RVA 0x22c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::ExportParameters

  .method /*0600009F*/ family hidebysig virtual 
          instance uint8[]  HashData(uint8[] data,
                                     int32 offset,
                                     int32 count,
                                     valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.HashAlgorithmName/*01000026*/ hashAlgorithm) cil managed
  // SIG: 20 04 1D 05 1D 05 08 08 11 80 99
  {
    // Method begins at RVA 0x22c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::HashData

  .method /*060000A0*/ family hidebysig virtual 
          instance uint8[]  HashData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.HashAlgorithmName/*01000026*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 12 80 9D 11 80 99
  {
    // Method begins at RVA 0x22c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::HashData

  .method /*060000A1*/ public hidebysig newslot virtual final 
          instance void  ImportCspBlob(uint8[] keyBlob) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x22cc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::ImportCspBlob

  .method /*060000A2*/ public hidebysig virtual 
          instance void  ImportParameters(valuetype [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSAParameters/*01000031*/ parameters) cil managed
  // SIG: 20 01 01 11 80 C5
  {
    // Method begins at RVA 0x22ce
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSACryptoServiceProvider::ImportParameters

  .method /*060000A3*/ public hidebysig instance uint8[] 
          SignData(uint8[] buffer,
                   int32 offset,
                   int32 count,
                   object halg) cil managed
  // SIG: 20 04 1D 05 1D 05 08 08 1C
  {
    // Method begins at RVA 0x22d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::SignData

  .method /*060000A4*/ public hidebysig instance uint8[] 
          SignData(uint8[] buffer,
                   object halg) cil managed
  // SIG: 20 02 1D 05 1D 05 1C
  {
    // Method begins at RVA 0x22d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::SignData

  .method /*060000A5*/ public hidebysig instance uint8[] 
          SignData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000027*/ inputStream,
                   object halg) cil managed
  // SIG: 20 02 1D 05 12 80 9D 1C
  {
    // Method begins at RVA 0x22d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::SignData

  .method /*060000A6*/ public hidebysig virtual 
          instance uint8[]  SignHash(uint8[] hash,
                                     valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.HashAlgorithmName/*01000026*/ hashAlgorithm,
                                     class [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSASignaturePadding/*01000032*/ padding) cil managed
  // SIG: 20 03 1D 05 1D 05 11 80 99 12 80 C9
  {
    // Method begins at RVA 0x22d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::SignHash

  .method /*060000A7*/ public hidebysig instance uint8[] 
          SignHash(uint8[] rgbHash,
                   string str) cil managed
  // SIG: 20 02 1D 05 1D 05 0E
  {
    .param [2]/*080000A1*/ 
    .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::SignHash

  .method /*060000A8*/ public hidebysig instance bool 
          VerifyData(uint8[] buffer,
                     object halg,
                     uint8[] signature) cil managed
  // SIG: 20 03 02 1D 05 1C 1D 05
  {
    // Method begins at RVA 0x22df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::VerifyData

  .method /*060000A9*/ public hidebysig virtual 
          instance bool  VerifyHash(uint8[] hash,
                                    uint8[] signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.HashAlgorithmName/*01000026*/ hashAlgorithm,
                                    class [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.RSASignaturePadding/*01000032*/ padding) cil managed
  // SIG: 20 04 02 1D 05 1D 05 11 80 99 12 80 C9
  {
    // Method begins at RVA 0x22e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::VerifyHash

  .method /*060000AA*/ public hidebysig instance bool 
          VerifyHash(uint8[] rgbHash,
                     string str,
                     uint8[] rgbSignature) cil managed
  // SIG: 20 03 02 1D 05 0E 1D 05
  {
    // Method begins at RVA 0x22e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSACryptoServiceProvider::VerifyHash

  .property /*17000027*/ instance callconv(8) class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/
          CspKeyContainerInfo()
  {
    .custom /*0C00005A:0A000015*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100001E*/::.ctor(string) /* 0A000015 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .get instance class System.Security.Cryptography.CspKeyContainerInfo/*02000007*/ System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::get_CspKeyContainerInfo() /* 0600008C */
  } // end of property RSACryptoServiceProvider::CspKeyContainerInfo
  .property /*17000028*/ instance callconv(8) string
          KeyExchangeAlgorithm()
  {
    .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::get_KeyExchangeAlgorithm() /* 0600008D */
  } // end of property RSACryptoServiceProvider::KeyExchangeAlgorithm
  .property /*17000029*/ instance callconv(8) int32
          KeySize()
  {
    .get instance int32 System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::get_KeySize() /* 0600008E */
  } // end of property RSACryptoServiceProvider::KeySize
  .property /*1700002A*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[]
          LegalKeySizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::get_LegalKeySizes() /* 0600008F */
  } // end of property RSACryptoServiceProvider::LegalKeySizes
  .property /*1700002B*/ instance callconv(8) bool
          PersistKeyInCsp()
  {
    .get instance bool System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::get_PersistKeyInCsp() /* 06000090 */
    .set instance void System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::set_PersistKeyInCsp(bool) /* 06000091 */
  } // end of property RSACryptoServiceProvider::PersistKeyInCsp
  .property /*1700002C*/ instance callconv(8) bool
          PublicOnly()
  {
    .get instance bool System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::get_PublicOnly() /* 06000092 */
  } // end of property RSACryptoServiceProvider::PublicOnly
  .property /*1700002D*/ instance callconv(8) string
          SignatureAlgorithm()
  {
    .get instance string System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::get_SignatureAlgorithm() /* 06000093 */
  } // end of property RSACryptoServiceProvider::SignatureAlgorithm
  .property /*1700002E*/ callconv(8) bool
          UseMachineKeyStore()
  {
    .get bool System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::get_UseMachineKeyStore() /* 06000094 */
    .set void System.Security.Cryptography.RSACryptoServiceProvider/*02000012*/::set_UseMachineKeyStore(bool) /* 06000095 */
  } // end of property RSACryptoServiceProvider::UseMachineKeyStore
} // end of class System.Security.Cryptography.RSACryptoServiceProvider

.class /*02000013*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.SHA1CryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.SHA1/*01000033*/
{
  .custom /*0C000046:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C000047:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060000AB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22e8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000022       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.SHA1/*01000033*/::.ctor() /* 0A000022 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA1CryptoServiceProvider::.ctor

  .method /*060000AC*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22f0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA1CryptoServiceProvider::Dispose

  .method /*060000AD*/ family hidebysig virtual 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C000089:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22f2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA1CryptoServiceProvider::HashCore

  .method /*060000AE*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000029*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 80 A5 01 05
  {
    // Method begins at RVA 0x22f4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA1CryptoServiceProvider::HashCore

  .method /*060000AF*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C00008A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1CryptoServiceProvider::HashFinal

  .method /*060000B0*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA1CryptoServiceProvider::Initialize

  .method /*060000B1*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002A*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 80 A9 01 05 10 08
  {
    // Method begins at RVA 0x22fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1CryptoServiceProvider::TryHashFinal

} // end of class System.Security.Cryptography.SHA1CryptoServiceProvider

.class /*02000014*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.SHA256CryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.SHA256/*01000034*/
{
  .custom /*0C000048:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C000049:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060000B2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22fe
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000023       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.SHA256/*01000034*/::.ctor() /* 0A000023 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA256CryptoServiceProvider::.ctor

  .method /*060000B3*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2306
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA256CryptoServiceProvider::Dispose

  .method /*060000B4*/ family hidebysig virtual 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2308
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA256CryptoServiceProvider::HashCore

  .method /*060000B5*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000029*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 80 A5 01 05
  {
    // Method begins at RVA 0x230a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA256CryptoServiceProvider::HashCore

  .method /*060000B6*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C00008C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256CryptoServiceProvider::HashFinal

  .method /*060000B7*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x230f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA256CryptoServiceProvider::Initialize

  .method /*060000B8*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002A*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 80 A9 01 05 10 08
  {
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256CryptoServiceProvider::TryHashFinal

} // end of class System.Security.Cryptography.SHA256CryptoServiceProvider

.class /*02000015*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.SHA384CryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.SHA384/*01000035*/
{
  .custom /*0C00004A:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C00004B:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060000B9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2314
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000024       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.SHA384/*01000035*/::.ctor() /* 0A000024 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA384CryptoServiceProvider::.ctor

  .method /*060000BA*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x231c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA384CryptoServiceProvider::Dispose

  .method /*060000BB*/ family hidebysig virtual 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C00008D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x231e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA384CryptoServiceProvider::HashCore

  .method /*060000BC*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000029*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 80 A5 01 05
  {
    // Method begins at RVA 0x2320
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA384CryptoServiceProvider::HashCore

  .method /*060000BD*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2322
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384CryptoServiceProvider::HashFinal

  .method /*060000BE*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2325
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA384CryptoServiceProvider::Initialize

  .method /*060000BF*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002A*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 80 A9 01 05 10 08
  {
    // Method begins at RVA 0x2327
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384CryptoServiceProvider::TryHashFinal

} // end of class System.Security.Cryptography.SHA384CryptoServiceProvider

.class /*02000016*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.SHA512CryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.SHA512/*01000036*/
{
  .custom /*0C00004D:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C00004E:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060000C0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x232a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000025       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.SHA512/*01000036*/::.ctor() /* 0A000025 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA512CryptoServiceProvider::.ctor

  .method /*060000C1*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2332
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA512CryptoServiceProvider::Dispose

  .method /*060000C2*/ family hidebysig virtual 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C00008F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2334
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA512CryptoServiceProvider::HashCore

  .method /*060000C3*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*01000029*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 80 A5 01 05
  {
    // Method begins at RVA 0x2336
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA512CryptoServiceProvider::HashCore

  .method /*060000C4*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C000090:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2338
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512CryptoServiceProvider::HashFinal

  .method /*060000C5*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x233b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA512CryptoServiceProvider::Initialize

  .method /*060000C6*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100002A*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 80 A9 01 05 10 08
  {
    // Method begins at RVA 0x233d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512CryptoServiceProvider::TryHashFinal

} // end of class System.Security.Cryptography.SHA512CryptoServiceProvider

.class /*02000017*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.TripleDESCryptoServiceProvider
       extends [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.TripleDES/*01000037*/
{
  .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000051:0A000013*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000016*/::.ctor(string) /* 0A000013 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
                                                                                                                                                 6F 67 72 61 70 68 69 63 20 74 79 70 65 73 20 61   // ographic types a
                                                                                                                                                 72 65 20 6F 62 73 6F 6C 65 74 65 2E 20 55 73 65   // re obsolete. Use
                                                                                                                                                 20 74 68 65 20 43 72 65 61 74 65 20 6D 65 74 68   //  the Create meth
                                                                                                                                                 6F 64 20 6F 6E 20 74 68 65 20 62 61 73 65 20 74   // od on the base t
                                                                                                                                                 79 70 65 20 69 6E 73 74 65 61 64 2E 02 00 54 0E   // ype instead...T.
                                                                                                                                                 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53 59   // .DiagnosticId.SY
                                                                                                                                                 53 4C 49 42 30 30 32 31 54 0E 09 55 72 6C 46 6F   // SLIB0021T..UrlFo
                                                                                                                                                 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B 61   // rmat"https://aka
                                                                                                                                                 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E 69   // .ms/dotnet-warni
                                                                                                                                                 6E 67 73 2F 7B 30 7D )                            // ngs/{0}
  .custom /*0C000052:0A000014*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000014 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*060000C7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2340
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000026       */ call       instance void [System.Security.Cryptography.Algorithms/*23000002*/]System.Security.Cryptography.TripleDES/*01000037*/::.ctor() /* 0A000026 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::.ctor

  .method /*060000C8*/ public hidebysig specialname virtual 
          instance int32  get_BlockSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2348
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_BlockSize

  .method /*060000C9*/ public hidebysig specialname virtual 
          instance void  set_BlockSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x234b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::set_BlockSize

  .method /*060000CA*/ public hidebysig specialname virtual 
          instance int32  get_FeedbackSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x234d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_FeedbackSize

  .method /*060000CB*/ public hidebysig specialname virtual 
          instance void  set_FeedbackSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2350
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::set_FeedbackSize

  .method /*060000CC*/ public hidebysig specialname virtual 
          instance uint8[]  get_IV() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2352
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_IV

  .method /*060000CD*/ public hidebysig specialname virtual 
          instance void  set_IV(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2355
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::set_IV

  .method /*060000CE*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2357
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_Key

  .method /*060000CF*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x235a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::set_Key

  .method /*060000D0*/ public hidebysig specialname virtual 
          instance int32  get_KeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x235c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_KeySize

  .method /*060000D1*/ public hidebysig specialname virtual 
          instance void  set_KeySize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x235f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::set_KeySize

  .method /*060000D2*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] 
          get_LegalBlockSizes() cil managed
  // SIG: 20 00 1D 12 69
  {
    // Method begins at RVA 0x2361
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_LegalBlockSizes

  .method /*060000D3*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] 
          get_LegalKeySizes() cil managed
  // SIG: 20 00 1D 12 69
  {
    // Method begins at RVA 0x2364
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_LegalKeySizes

  .method /*060000D4*/ public hidebysig specialname virtual 
          instance valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 6D
  {
    // Method begins at RVA 0x2367
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_Mode

  .method /*060000D5*/ public hidebysig specialname virtual 
          instance void  set_Mode(valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/ 'value') cil managed
  // SIG: 20 01 01 11 6D
  {
    // Method begins at RVA 0x236a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::set_Mode

  .method /*060000D6*/ public hidebysig specialname virtual 
          instance valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/ 
          get_Padding() cil managed
  // SIG: 20 00 11 71
  {
    // Method begins at RVA 0x236c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::get_Padding

  .method /*060000D7*/ public hidebysig specialname virtual 
          instance void  set_Padding(valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/ 'value') cil managed
  // SIG: 20 01 01 11 71
  {
    // Method begins at RVA 0x236f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::set_Padding

  .method /*060000D8*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateDecryptor() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::CreateDecryptor

  .method /*060000D9*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateDecryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 75 1D 05 1D 05
  {
    .param [2]/*080000D0*/ 
    .custom /*0C000091:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2374
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::CreateDecryptor

  .method /*060000DA*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateEncryptor() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::CreateEncryptor

  .method /*060000DB*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.ICryptoTransform/*0100001D*/ 
          CreateEncryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 75 1D 05 1D 05
  {
    .param [2]/*080000D2*/ 
    .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDESCryptoServiceProvider::CreateEncryptor

  .method /*060000DC*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x237d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::Dispose

  .method /*060000DD*/ public hidebysig virtual 
          instance void  GenerateIV() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x237f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::GenerateIV

  .method /*060000DE*/ public hidebysig virtual 
          instance void  GenerateKey() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2381
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDESCryptoServiceProvider::GenerateKey

  .property /*1700002F*/ instance callconv(8) int32
          BlockSize()
  {
    .get instance int32 System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_BlockSize() /* 060000C8 */
    .set instance void System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::set_BlockSize(int32) /* 060000C9 */
  } // end of property TripleDESCryptoServiceProvider::BlockSize
  .property /*17000030*/ instance callconv(8) int32
          FeedbackSize()
  {
    .get instance int32 System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_FeedbackSize() /* 060000CA */
    .set instance void System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::set_FeedbackSize(int32) /* 060000CB */
  } // end of property TripleDESCryptoServiceProvider::FeedbackSize
  .property /*17000031*/ instance callconv(8) uint8[]
          IV()
  {
    .get instance uint8[] System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_IV() /* 060000CC */
    .set instance void System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::set_IV(uint8[]) /* 060000CD */
  } // end of property TripleDESCryptoServiceProvider::IV
  .property /*17000032*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_Key() /* 060000CE */
    .set instance void System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::set_Key(uint8[]) /* 060000CF */
  } // end of property TripleDESCryptoServiceProvider::Key
  .property /*17000033*/ instance callconv(8) int32
          KeySize()
  {
    .get instance int32 System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_KeySize() /* 060000D0 */
    .set instance void System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::set_KeySize(int32) /* 060000D1 */
  } // end of property TripleDESCryptoServiceProvider::KeySize
  .property /*17000034*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[]
          LegalBlockSizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_LegalBlockSizes() /* 060000D2 */
  } // end of property TripleDESCryptoServiceProvider::LegalBlockSizes
  .property /*17000035*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[]
          LegalKeySizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.KeySizes/*0100001A*/[] System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_LegalKeySizes() /* 060000D3 */
  } // end of property TripleDESCryptoServiceProvider::LegalKeySizes
  .property /*17000036*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/
          Mode()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/ System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_Mode() /* 060000D4 */
    .set instance void System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::set_Mode(valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.CipherMode/*0100001B*/) /* 060000D5 */
  } // end of property TripleDESCryptoServiceProvider::Mode
  .property /*17000037*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/
          Padding()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/ System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::get_Padding() /* 060000D6 */
    .set instance void System.Security.Cryptography.TripleDESCryptoServiceProvider/*02000017*/::set_Padding(valuetype [System.Security.Cryptography.Primitives/*23000003*/]System.Security.Cryptography.PaddingMode/*0100001C*/) /* 060000D7 */
  } // end of property TripleDESCryptoServiceProvider::Padding
} // end of class System.Security.Cryptography.TripleDESCryptoServiceProvider


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
