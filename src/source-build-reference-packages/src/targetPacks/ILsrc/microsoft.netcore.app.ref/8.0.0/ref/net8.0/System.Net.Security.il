
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
// Number of sections:         0x0003
// Time-date stamp:            0xcb8f5004
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00009c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000baf2
// Base of code:                   0x00002000
// Base of data:                   0x0000c000
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
// Size of headers:                0x00000200
// Checksum:                       0x00012131
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000baa0 [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x00000474] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x0000a600 [0x00002910] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000ba84 [0x0000001c] address [size] of Debug Directory:           
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
//              0x00009af8 Virtual Size
//              0x00002000 Virtual Address
//              0x00009c00 Size of Raw Data
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
//              0x0000c000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00009e00 Pointer to Raw Data
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
//              0x0000a400 Pointer to Raw Data
//              0x00000000 Pointer to Relocations
//              0x00000000 Pointer to Linenumbers
//                  0x0000 Number of Relocations
//                  0x0000 Number of Linenumbers
//              0x42000040 Characteristics
//                         CNT_INITIALIZED_DATA
//                         MEM_DISCARDABLE
//                         MEM_READ

// Base Relocation Table
//              0x0000b000 Page RVA
//              0x0000000c Block Size
//              0x00000002 Number of Entries
//              Entry 1: Type 0x3 Offset 0x00000af4
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000bae2 Import Name Table
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
// 0x0000244c [0x000095b8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000ba04 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00003d30 Size
//              '#~' Name
//    Stream 2:
//              0x00003d9c Offset
//              0x00004890 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000862c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00008630 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00008640 Offset
//              0x00000f78 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000000090ba01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 53008
// PE header size       : 512 (496 used)    ( 0.97%)
// PE additional info   : 11851             (22.36%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.14%)
// CLR meta-data size  : 38328              (72.31%)
// CLR additional info : 128                ( 0.24%)
// CLR method headers  : 326                ( 0.62%)
// Managed code         : 691               ( 1.30%)
// Data                 : 2048              ( 3.86%)
// Unaccounted          : -948              (-1.79%)

// Num.of PE sections   : 3
//   .text    - 39936
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 38328
//   Module        -    1 (10 bytes)
//   TypeDef       -   28 (392 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   76 (456 bytes)
//   MethodDef     -  318 (4452 bytes)    5 abstract, 0 native, 297 bodies
//   FieldDef      -  377 (2262 bytes)    3 constant
//   MemberRef     -   32 (192 bytes)
//   ParamDef      -  423 (2538 bytes)
//   MethodImpl    -    2 (12 bytes)
//   Constant      -  376 (2256 bytes)
//   CustomAttribute-  202 (1212 bytes)
//   InterfaceImpl -    3 (12 bytes)
//   PropertyMap   -   13 (52 bytes)
//   Property      -  109 (654 bytes)
//   MethodSemantic-  154 (924 bytes)
//   TypeSpec      -    1 (2 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    6 (120 bytes)
//   Strings       - 18576 bytes
//   Blobs         -  3960 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   204 bytes

// CLR additional info : 128

// CLR method headers : 326
//   Num.of method bodies  - 297
//   Num.of fat headers    - 1
//   Num.of tiny headers   - 296
//   Num.of fat sections   - 0
//   Num.of small sections - 1

// Managed code : 691
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class AuthenticationException        (public) (auto) (ansi) 
// Class InvalidCredentialException     (public) (auto) (ansi) 
// Class ExtendedProtectionPolicy       (public) (auto) (ansi) 
// Class PolicyEnforcement              (public) (auto) (ansi) (sealed) 
// Class ProtectionScenario             (public) (auto) (ansi) (sealed) 
// Class ServiceNameCollection          (public) (auto) (ansi) 
// Class AuthenticatedStream            (public) (abstract) (auto) (ansi) 
// Class CipherSuitesPolicy             (public) (auto) (ansi) (sealed) 
// Class EncryptionPolicy               (public) (auto) (ansi) (sealed) 
// Class LocalCertificateSelectionCallback (public) (auto) (ansi) (sealed) 
// Class NegotiateAuthentication        (public) (auto) (ansi) (sealed) 
// Class NegotiateAuthenticationClientOptions (public) (auto) (ansi) 
// Class NegotiateAuthenticationServerOptions (public) (auto) (ansi) 
// Class NegotiateAuthenticationStatusCode (public) (auto) (ansi) (sealed) 
// Class NegotiateStream                (public) (auto) (ansi) 
// Class ProtectionLevel                (public) (auto) (ansi) (sealed) 
// Class RemoteCertificateValidationCallback (public) (auto) (ansi) (sealed) 
// Class ServerCertificateSelectionCallback (public) (auto) (ansi) (sealed) 
// Class ServerOptionsSelectionCallback (public) (auto) (ansi) (sealed) 
// Class SslApplicationProtocol         (public) (sequential) (ansi) (sealed) 
// Class SslCertificateTrust            (public) (auto) (ansi) (sealed) 
// Class SslClientAuthenticationOptions (public) (auto) (ansi) 
// Class SslClientHelloInfo             (public) (sequential) (ansi) (sealed) 
// Class SslServerAuthenticationOptions (public) (auto) (ansi) 
// Class SslStream                      (public) (auto) (ansi) 
// Class SslStreamCertificateContext    (public) (auto) (ansi) 
// Class TlsCipherSuite                 (public) (auto) (ansi) (sealed) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000002*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000003*/ System.Collections.NonGeneric
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000004*/ System.Security.Cryptography
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly extern /*23000005*/ System.Memory
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 8:0:0:0
}
.assembly extern /*23000006*/ System.Collections
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 8:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.Security
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
                                                                                                                                                                           65 72 73 69 6F 6E 3D 76 38 2E 30 01 00 54 0E 14   // ersion=v8.0..T..
                                                                                                                                                                           46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C 61 79   // FrameworkDisplay
                                                                                                                                                                           4E 61 6D 65 08 2E 4E 45 54 20 38 2E 30 )          // Name..NET 8.0
  .custom /*0C000008:0A000005*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.ReferenceAssemblyAttribute/*01000006*/::.ctor() /* 0A000005 */ = ( 01 00 00 00 ) 
  .custom /*0C000009:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000A:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000B:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                        63 75 72 69 74 79 00 00 )                         // curity..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00000D:0A000009*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00000E:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                       63 75 72 69 74 79 00 00 )                         // curity..
  .custom /*0C000011:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 0C 38 2E 30 2E 32 33 2E 35 33 31 30 33 00   // ...8.0.23.53103.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 2E 38 2E 30 2E 30 2B 35 35 33 35 65 33 31   // ...8.0.0+5535e31
                                                                                                                                                                                61 37 31 32 33 34 33 61 36 33 66 35 64 37 64 37   // a712343a63f5d7d7
                                                                                                                                                                                39 36 63 64 38 37 34 65 35 36 33 65 35 61 63 31   // 96cd874e563e5ac1
                                                                                                                                                                                34 00 00 )                                        // 4..
  .custom /*0C000013:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A000010*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000011*/::.ctor(string) /* 0A000010 */ = ( 01 00 13 53 79 73 74 65 6D 2E 4E 65 74 2E 53 65   // ...System.Net.Se
                                                                                                                                                                 63 75 72 69 74 79 00 00 )                         // curity..
  .custom /*0C000015:0A000006*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000007*/::.ctor(string,
                                                                                                                                         string) /* 0A000006 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
  .ver 8:0:0:0
}
.module System.Net.Security.dll
// MVID: {2c9378f6-8f65-4325-8d87-aea84e210ef3}
.custom /*0C000002:0A000011*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.RefSafetyRulesAttribute/*01000012*/::.ctor(int32) /* 0A000011 */ = ( 01 00 0B 00 00 00 00 00 ) 
.custom /*0C000003:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*01000013*/::.ctor(bool) /* 0A000012 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY
// Image base: 0x00007F9972A83000


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ public auto ansi beforefieldinit System.Security.Authentication.AuthenticationException
       extends [System.Runtime/*23000001*/]System.SystemException/*01000016*/
{
  .custom /*0C000019:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00001A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000016*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AuthenticationException::.ctor

  .method /*06000002*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001A*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001B*/ streamingContext) cil managed
  // SIG: 20 02 01 12 69 11 6D
  {
    .custom /*0C000016:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000017:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000018:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    // Method begins at RVA 0x2058
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000016*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AuthenticationException::.ctor

  .method /*06000003*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2060
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000016*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AuthenticationException::.ctor

  .method /*06000004*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 71
  {
    // Method begins at RVA 0x2068
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Runtime/*23000001*/]System.SystemException/*01000016*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AuthenticationException::.ctor

} // end of class System.Security.Authentication.AuthenticationException

.class /*02000003*/ public auto ansi beforefieldinit System.Security.Authentication.InvalidCredentialException
       extends System.Security.Authentication.AuthenticationException/*02000002*/
{
  .custom /*0C00001B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00001C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000005*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2070
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000001       */ call       instance void System.Security.Authentication.AuthenticationException/*02000002*/::.ctor() /* 06000001 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidCredentialException::.ctor

  .method /*06000006*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001A*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001B*/ streamingContext) cil managed
  // SIG: 20 02 01 12 69 11 6D
  {
    .custom /*0C00001F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000020:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000021:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    // Method begins at RVA 0x2078
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000001       */ call       instance void System.Security.Authentication.AuthenticationException/*02000002*/::.ctor() /* 06000001 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidCredentialException::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2080
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000001       */ call       instance void System.Security.Authentication.AuthenticationException/*02000002*/::.ctor() /* 06000001 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidCredentialException::.ctor

  .method /*06000008*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100001C*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 71
  {
    // Method begins at RVA 0x2088
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000001       */ call       instance void System.Security.Authentication.AuthenticationException/*02000002*/::.ctor() /* 06000001 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method InvalidCredentialException::.ctor

} // end of class System.Security.Authentication.InvalidCredentialException

.class /*02000004*/ public auto ansi beforefieldinit System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001E*/
{
  .custom /*0C00001D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00001E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001E*/
  .custom /*0C000001:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000009*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001A*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001B*/ context) cil managed
  // SIG: 20 02 01 12 69 11 6D
  {
    .custom /*0C000026:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000027:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000028:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 72 54 68 69 73 20 41 50 49 20 73 75 70 70   // ..rThis API supp
                                                                                                                                                   6F 72 74 73 20 6F 62 73 6F 6C 65 74 65 20 66 6F   // orts obsolete fo
                                                                                                                                                   72 6D 61 74 74 65 72 2D 62 61 73 65 64 20 73 65   // rmatter-based se
                                                                                                                                                   72 69 61 6C 69 7A 61 74 69 6F 6E 2E 20 49 74 20   // rialization. It 
                                                                                                                                                   73 68 6F 75 6C 64 20 6E 6F 74 20 62 65 20 63 61   // should not be ca
                                                                                                                                                   6C 6C 65 64 20 6F 72 20 65 78 74 65 6E 64 65 64   // lled or extended
                                                                                                                                                   20 62 79 20 61 70 70 6C 69 63 61 74 69 6F 6E 20   //  by application 
                                                                                                                                                   63 6F 64 65 2E 02 00 54 0E 0C 44 69 61 67 6E 6F   // code...T..Diagno
                                                                                                                                                   73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30 35   // sticId.SYSLIB005
                                                                                                                                                   31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68 74   // 1T..UrlFormat"ht
                                                                                                                                                   74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F 74   // tps://aka.ms/dot
                                                                                                                                                   6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30 7D ) // net-warnings/{0}
    // Method begins at RVA 0x2090
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExtendedProtectionPolicy::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ policyEnforcement) cil managed
  // SIG: 20 01 01 11 14
  {
    // Method begins at RVA 0x2098
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExtendedProtectionPolicy::.ctor

  .method /*0600000B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ policyEnforcement,
                               class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ customChannelBinding) cil managed
  // SIG: 20 02 01 11 14 12 7D
  {
    .custom /*0C00002E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20a0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExtendedProtectionPolicy::.ctor

  .method /*0600000C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ policyEnforcement,
                               valuetype System.Security.Authentication.ExtendedProtection.ProtectionScenario/*02000006*/ protectionScenario,
                               class [System.Runtime/*23000001*/]System.Collections.ICollection/*01000020*/ customServiceNames) cil managed
  // SIG: 20 03 01 11 14 11 18 12 80 81
  {
    // Method begins at RVA 0x20a8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExtendedProtectionPolicy::.ctor

  .method /*0600000D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ policyEnforcement,
                               valuetype System.Security.Authentication.ExtendedProtection.ProtectionScenario/*02000006*/ protectionScenario,
                               class System.Security.Authentication.ExtendedProtection.ServiceNameCollection/*02000007*/ customServiceNames) cil managed
  // SIG: 20 03 01 11 14 11 18 12 1C
  {
    // Method begins at RVA 0x20b0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ExtendedProtectionPolicy::.ctor

  .method /*0600000E*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ 
          get_CustomChannelBinding() cil managed
  // SIG: 20 00 12 7D
  {
    // Method begins at RVA 0x20b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedProtectionPolicy::get_CustomChannelBinding

  .method /*0600000F*/ public hidebysig specialname 
          instance class System.Security.Authentication.ExtendedProtection.ServiceNameCollection/*02000007*/ 
          get_CustomServiceNames() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedProtectionPolicy::get_CustomServiceNames

  .method /*06000010*/ public hidebysig specialname static 
          bool  get_OSSupportsExtendedProtection() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedProtectionPolicy::get_OSSupportsExtendedProtection

  .method /*06000011*/ public hidebysig specialname 
          instance valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ 
          get_PolicyEnforcement() cil managed
  // SIG: 20 00 11 14
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedProtectionPolicy::get_PolicyEnforcement

  .method /*06000012*/ public hidebysig specialname 
          instance valuetype System.Security.Authentication.ExtendedProtection.ProtectionScenario/*02000006*/ 
          get_ProtectionScenario() cil managed
  // SIG: 20 00 11 18
  {
    // Method begins at RVA 0x20c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedProtectionPolicy::get_ProtectionScenario

  .method /*06000013*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*0100001A*/ info,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*0100001B*/ context) cil managed
  // SIG: 20 02 01 12 69 11 6D
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100001E*/::GetObjectData /*0100001E::0A000017*/ 
    // Method begins at RVA 0x20c7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ExtendedProtectionPolicy::System.Runtime.Serialization.ISerializable.GetObjectData

  .method /*06000014*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000037:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ExtendedProtectionPolicy::ToString

  .property /*17000001*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/
          CustomChannelBinding()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/::get_CustomChannelBinding() /* 0600000E */
  } // end of property ExtendedProtectionPolicy::CustomChannelBinding
  .property /*17000002*/ instance callconv(8) class System.Security.Authentication.ExtendedProtection.ServiceNameCollection/*02000007*/
          CustomServiceNames()
  {
    .get instance class System.Security.Authentication.ExtendedProtection.ServiceNameCollection/*02000007*/ System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/::get_CustomServiceNames() /* 0600000F */
  } // end of property ExtendedProtectionPolicy::CustomServiceNames
  .property /*17000003*/ callconv(8) bool
          OSSupportsExtendedProtection()
  {
    .get bool System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/::get_OSSupportsExtendedProtection() /* 06000010 */
  } // end of property ExtendedProtectionPolicy::OSSupportsExtendedProtection
  .property /*17000004*/ instance callconv(8) valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/
          PolicyEnforcement()
  {
    .get instance valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/::get_PolicyEnforcement() /* 06000011 */
  } // end of property ExtendedProtectionPolicy::PolicyEnforcement
  .property /*17000005*/ instance callconv(8) valuetype System.Security.Authentication.ExtendedProtection.ProtectionScenario/*02000006*/
          ProtectionScenario()
  {
    .get instance valuetype System.Security.Authentication.ExtendedProtection.ProtectionScenario/*02000006*/ System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/::get_ProtectionScenario() /* 06000012 */
  } // end of property ExtendedProtectionPolicy::ProtectionScenario
} // end of class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy

.class /*02000005*/ public auto ansi sealed System.Security.Authentication.ExtendedProtection.PolicyEnforcement
       extends [System.Runtime/*23000001*/]System.Enum/*01000021*/
{
  .field /*04000001*/ public specialname rtspecialname int32 value__
  .field /*04000002*/ public static literal valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ Never = int32(0x00000000)
  .field /*04000003*/ public static literal valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ WhenSupported = int32(0x00000001)
  .field /*04000004*/ public static literal valuetype System.Security.Authentication.ExtendedProtection.PolicyEnforcement/*02000005*/ Always = int32(0x00000002)
} // end of class System.Security.Authentication.ExtendedProtection.PolicyEnforcement

.class /*02000006*/ public auto ansi sealed System.Security.Authentication.ExtendedProtection.ProtectionScenario
       extends [System.Runtime/*23000001*/]System.Enum/*01000021*/
{
  .field /*04000005*/ public specialname rtspecialname int32 value__
  .field /*04000006*/ public static literal valuetype System.Security.Authentication.ExtendedProtection.ProtectionScenario/*02000006*/ TransportSelected = int32(0x00000000)
  .field /*04000007*/ public static literal valuetype System.Security.Authentication.ExtendedProtection.ProtectionScenario/*02000006*/ TrustedProxy = int32(0x00000001)
} // end of class System.Security.Authentication.ExtendedProtection.ProtectionScenario

.class /*02000007*/ public auto ansi beforefieldinit System.Security.Authentication.ExtendedProtection.ServiceNameCollection
       extends [System.Collections.NonGeneric/*23000003*/]System.Collections.ReadOnlyCollectionBase/*01000022*/
{
  .custom /*0C000022:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000023:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000015*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.ICollection/*01000020*/ items) cil managed
  // SIG: 20 01 01 12 80 81
  {
    // Method begins at RVA 0x20cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Collections.NonGeneric/*23000003*/]System.Collections.ReadOnlyCollectionBase/*01000022*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ServiceNameCollection::.ctor

  .method /*06000016*/ public hidebysig instance bool 
          Contains(string searchServiceName) cil managed
  // SIG: 20 01 02 0E
  {
    .custom /*0C00003A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServiceNameCollection::Contains

  .method /*06000017*/ public hidebysig instance class System.Security.Authentication.ExtendedProtection.ServiceNameCollection/*02000007*/ 
          Merge(class [System.Runtime/*23000001*/]System.Collections.IEnumerable/*01000023*/ serviceNames) cil managed
  // SIG: 20 01 12 1C 12 80 8D
  {
    // Method begins at RVA 0x20d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServiceNameCollection::Merge

  .method /*06000018*/ public hidebysig instance class System.Security.Authentication.ExtendedProtection.ServiceNameCollection/*02000007*/ 
          Merge(string serviceName) cil managed
  // SIG: 20 01 12 1C 0E
  {
    // Method begins at RVA 0x20da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ServiceNameCollection::Merge

} // end of class System.Security.Authentication.ExtendedProtection.ServiceNameCollection

.class /*02000008*/ public abstract auto ansi beforefieldinit System.Net.Security.AuthenticatedStream
       extends [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/
{
  .custom /*0C000024:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000025:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000019*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ innerStream,
                               bool leaveInnerStreamOpen) cil managed
  // SIG: 20 02 01 12 80 91 02
  {
    // Method begins at RVA 0x20dd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000020       */ call       instance void [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/::.ctor() /* 0A000020 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AuthenticatedStream::.ctor

  .method /*0600001A*/ family hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ 
          get_InnerStream() cil managed
  // SIG: 20 00 12 80 91
  {
    // Method begins at RVA 0x20e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticatedStream::get_InnerStream

  .method /*0600001B*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_IsAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method AuthenticatedStream::get_IsAuthenticated

  .method /*0600001C*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_IsEncrypted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method AuthenticatedStream::get_IsEncrypted

  .method /*0600001D*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_IsMutuallyAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method AuthenticatedStream::get_IsMutuallyAuthenticated

  .method /*0600001E*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_IsServer() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method AuthenticatedStream::get_IsServer

  .method /*0600001F*/ public hidebysig newslot specialname abstract virtual 
          instance bool  get_IsSigned() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x0
  } // end of method AuthenticatedStream::get_IsSigned

  .method /*06000020*/ public hidebysig specialname 
          instance bool  get_LeaveInnerStreamOpen() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticatedStream::get_LeaveInnerStreamOpen

  .method /*06000021*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20eb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AuthenticatedStream::Dispose

  .method /*06000022*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x20ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AuthenticatedStream::DisposeAsync

  .property /*17000006*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/
          InnerStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ System.Net.Security.AuthenticatedStream/*02000008*/::get_InnerStream() /* 0600001A */
  } // end of property AuthenticatedStream::InnerStream
  .property /*17000007*/ instance callconv(8) bool
          IsAuthenticated()
  {
    .get instance bool System.Net.Security.AuthenticatedStream/*02000008*/::get_IsAuthenticated() /* 0600001B */
  } // end of property AuthenticatedStream::IsAuthenticated
  .property /*17000008*/ instance callconv(8) bool
          IsEncrypted()
  {
    .get instance bool System.Net.Security.AuthenticatedStream/*02000008*/::get_IsEncrypted() /* 0600001C */
  } // end of property AuthenticatedStream::IsEncrypted
  .property /*17000009*/ instance callconv(8) bool
          IsMutuallyAuthenticated()
  {
    .get instance bool System.Net.Security.AuthenticatedStream/*02000008*/::get_IsMutuallyAuthenticated() /* 0600001D */
  } // end of property AuthenticatedStream::IsMutuallyAuthenticated
  .property /*1700000A*/ instance callconv(8) bool
          IsServer()
  {
    .get instance bool System.Net.Security.AuthenticatedStream/*02000008*/::get_IsServer() /* 0600001E */
  } // end of property AuthenticatedStream::IsServer
  .property /*1700000B*/ instance callconv(8) bool
          IsSigned()
  {
    .get instance bool System.Net.Security.AuthenticatedStream/*02000008*/::get_IsSigned() /* 0600001F */
  } // end of property AuthenticatedStream::IsSigned
  .property /*1700000C*/ instance callconv(8) bool
          LeaveInnerStreamOpen()
  {
    .get instance bool System.Net.Security.AuthenticatedStream/*02000008*/::get_LeaveInnerStreamOpen() /* 06000020 */
  } // end of property AuthenticatedStream::LeaveInnerStreamOpen
} // end of class System.Net.Security.AuthenticatedStream

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Net.Security.CipherSuitesPolicy
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000029:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00002B:0A000009*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
  .custom /*0C00002C:0A000009*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
  .method /*06000023*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000026*/<valuetype System.Net.Security.TlsCipherSuite/*0200001C*/> allowedCipherSuites) cil managed
  // SIG: 20 01 01 15 12 80 99 01 11 70
  {
    .custom /*0C00004F:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20f0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CipherSuitesPolicy::.ctor

  .method /*06000024*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000026*/<valuetype System.Net.Security.TlsCipherSuite/*0200001C*/> 
          get_AllowedCipherSuites() cil managed
  // SIG: 20 00 15 12 80 99 01 11 70
  {
    // Method begins at RVA 0x20f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CipherSuitesPolicy::get_AllowedCipherSuites

  .property /*1700000D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000026*/<valuetype System.Net.Security.TlsCipherSuite/*0200001C*/>
          AllowedCipherSuites()
  {
    .custom /*0C000032:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Collections.Generic.IEnumerable`1/*01000026*/<valuetype System.Net.Security.TlsCipherSuite/*0200001C*/> System.Net.Security.CipherSuitesPolicy/*02000009*/::get_AllowedCipherSuites() /* 06000024 */
  } // end of property CipherSuitesPolicy::AllowedCipherSuites
} // end of class System.Net.Security.CipherSuitesPolicy

.class /*0200000A*/ public auto ansi sealed System.Net.Security.EncryptionPolicy
       extends [System.Runtime/*23000001*/]System.Enum/*01000021*/
{
  .field /*04000008*/ public specialname rtspecialname int32 value__
  .field /*04000009*/ public static literal valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ RequireEncryption = int32(0x00000000)
  .field /*0400000A*/ public static literal valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ AllowNoEncryption = int32(0x00000001)
  .custom /*0C00002D:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 7A 45 6E 63 72 79 70 74 69 6F 6E 50 6F 6C   // ..zEncryptionPol
                                                                                                                                                 69 63 79 2E 4E 6F 45 6E 63 72 79 70 74 69 6F 6E   // icy.NoEncryption
                                                                                                                                                 20 61 6E 64 20 41 6C 6C 6F 77 45 6E 63 72 79 70   //  and AllowEncryp
                                                                                                                                                 74 69 6F 6E 20 73 69 67 6E 69 66 69 63 61 6E 74   // tion significant
                                                                                                                                                 6C 79 20 72 65 64 75 63 65 20 73 65 63 75 72 69   // ly reduce securi
                                                                                                                                                 74 79 20 61 6E 64 20 73 68 6F 75 6C 64 20 6E 6F   // ty and should no
                                                                                                                                                 74 20 62 65 20 75 73 65 64 20 69 6E 20 70 72 6F   // t be used in pro
                                                                                                                                                 64 75 63 74 69 6F 6E 20 63 6F 64 65 2E 02 00 54   // duction code...T
                                                                                                                                                 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53   // ..DiagnosticId.S
                                                                                                                                                 59 53 4C 49 42 30 30 34 30 54 0E 09 55 72 6C 46   // YSLIB0040T..UrlF
                                                                                                                                                 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B   // ormat"https://ak
                                                                                                                                                 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E   // a.ms/dotnet-warn
                                                                                                                                                 69 6E 67 73 2F 7B 30 7D )                         // ings/{0}
  .field /*0400000B*/ public static literal valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ NoEncryption = int32(0x00000002)
  .custom /*0C00002F:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000019*/::.ctor(string) /* 0A000016 */ = ( 01 00 7A 45 6E 63 72 79 70 74 69 6F 6E 50 6F 6C   // ..zEncryptionPol
                                                                                                                                                 69 63 79 2E 4E 6F 45 6E 63 72 79 70 74 69 6F 6E   // icy.NoEncryption
                                                                                                                                                 20 61 6E 64 20 41 6C 6C 6F 77 45 6E 63 72 79 70   //  and AllowEncryp
                                                                                                                                                 74 69 6F 6E 20 73 69 67 6E 69 66 69 63 61 6E 74   // tion significant
                                                                                                                                                 6C 79 20 72 65 64 75 63 65 20 73 65 63 75 72 69   // ly reduce securi
                                                                                                                                                 74 79 20 61 6E 64 20 73 68 6F 75 6C 64 20 6E 6F   // ty and should no
                                                                                                                                                 74 20 62 65 20 75 73 65 64 20 69 6E 20 70 72 6F   // t be used in pro
                                                                                                                                                 64 75 63 74 69 6F 6E 20 63 6F 64 65 2E 02 00 54   // duction code...T
                                                                                                                                                 0E 0C 44 69 61 67 6E 6F 73 74 69 63 49 64 0A 53   // ..DiagnosticId.S
                                                                                                                                                 59 53 4C 49 42 30 30 34 30 54 0E 09 55 72 6C 46   // YSLIB0040T..UrlF
                                                                                                                                                 6F 72 6D 61 74 22 68 74 74 70 73 3A 2F 2F 61 6B   // ormat"https://ak
                                                                                                                                                 61 2E 6D 73 2F 64 6F 74 6E 65 74 2D 77 61 72 6E   // a.ms/dotnet-warn
                                                                                                                                                 69 6E 67 73 2F 7B 30 7D )                         // ings/{0}
} // end of class System.Net.Security.EncryptionPolicy

.class /*0200000B*/ public auto ansi sealed System.Net.Security.LocalCertificateSelectionCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000027*/
{
  .method /*06000025*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method LocalCertificateSelectionCallback::.ctor

  .method /*06000026*/ public hidebysig newslot virtual 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ 
          Invoke(object sender,
                 string targetHost,
                 class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ localCertificates,
                 class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ remoteCertificate,
                 string[] acceptableIssuers) runtime managed
  // SIG: 20 05 12 80 A1 1C 0E 12 80 A5 12 80 A1 1D 0E
  {
    .custom /*0C000051:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000025*/ 
    .custom /*0C000050:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method LocalCertificateSelectionCallback::Invoke

  .method /*06000027*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginInvoke(object sender,
                      string targetHost,
                      class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ localCertificates,
                      class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ remoteCertificate,
                      string[] acceptableIssuers,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ callback,
                      object 'object') runtime managed
  // SIG: 20 07 12 80 A9 1C 0E 12 80 A5 12 80 A1 1D 0E 12 80 AD 1C
  {
    .custom /*0C000052:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [0]/*08000027*/ 
    .custom /*0C000053:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800002B*/ 
    .custom /*0C000056:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*0800002D*/ 
    .custom /*0C000057:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
    .param [7]/*0800002E*/ 
    .custom /*0C000058:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  } // end of method LocalCertificateSelectionCallback::BeginInvoke

  .method /*06000028*/ public hidebysig newslot virtual 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ result) runtime managed
  // SIG: 20 01 12 80 A1 12 80 A9
  {
    .param [0]/*0800002F*/ 
    .custom /*0C000059:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method LocalCertificateSelectionCallback::EndInvoke

} // end of class System.Net.Security.LocalCertificateSelectionCallback

.class /*0200000C*/ public auto ansi sealed beforefieldinit System.Net.Security.NegotiateAuthentication
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100002C*/
{
  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/ clientOptions) cil managed
  // SIG: 20 01 01 12 34
  {
    .custom /*0C000054:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20fb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthentication::.ctor

  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/ serverOptions) cil managed
  // SIG: 20 01 01 12 38
  {
    .custom /*0C000055:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2103
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthentication::.ctor

  .method /*0600002B*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ 
          get_ImpersonationLevel() cil managed
  // SIG: 20 00 11 80 B5
  {
    // Method begins at RVA 0x210b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_ImpersonationLevel

  .method /*0600002C*/ public hidebysig specialname 
          instance bool  get_IsAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x210e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_IsAuthenticated

  .method /*0600002D*/ public hidebysig specialname 
          instance bool  get_IsEncrypted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2111
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_IsEncrypted

  .method /*0600002E*/ public hidebysig specialname 
          instance bool  get_IsMutuallyAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2114
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_IsMutuallyAuthenticated

  .method /*0600002F*/ public hidebysig specialname 
          instance bool  get_IsServer() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2117
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_IsServer

  .method /*06000030*/ public hidebysig specialname 
          instance bool  get_IsSigned() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x211a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_IsSigned

  .method /*06000031*/ public hidebysig specialname 
          instance string  get_Package() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00005A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_Package

  .method /*06000032*/ public hidebysig specialname 
          instance valuetype System.Net.Security.ProtectionLevel/*02000011*/ 
          get_ProtectionLevel() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x2120
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_ProtectionLevel

  .method /*06000033*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Security.Principal.IIdentity/*0100002E*/ 
          get_RemoteIdentity() cil managed
  // SIG: 20 00 12 80 B9
  {
    .custom /*0C00005B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2123
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_RemoteIdentity

  .method /*06000034*/ public hidebysig specialname 
          instance string  get_TargetName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00005D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2126
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::get_TargetName

  .method /*06000035*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2129
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthentication::Dispose

  .method /*06000036*/ public hidebysig instance uint8[] 
          GetOutgoingBlob(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002F*/<uint8> incomingBlob,
                          [out] valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/& statusCode) cil managed
  // SIG: 20 02 1D 05 15 11 80 BD 01 05 10 11 3C
  {
    .param [0]/*08000033*/ 
    .custom /*0C00005C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::GetOutgoingBlob

  .method /*06000037*/ public hidebysig instance string 
          GetOutgoingBlob(string incomingBlob,
                          [out] valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/& statusCode) cil managed
  // SIG: 20 02 0E 0E 10 11 3C
  {
    .custom /*0C00005E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::GetOutgoingBlob

  .method /*06000038*/ public hidebysig instance valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ 
          Unwrap(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002F*/<uint8> input,
                 class [System.Memory/*23000005*/]System.Buffers.IBufferWriter`1/*01000030*/<uint8> outputWriter,
                 [out] bool& wasEncrypted) cil managed
  // SIG: 20 03 11 3C 15 11 80 BD 01 05 15 12 80 C1 01 05 10 02
  {
    .param [2]/*08000039*/ 
    .custom /*0C00005F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2131
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::Unwrap

  .method /*06000039*/ public hidebysig instance valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ 
          UnwrapInPlace(valuetype [System.Runtime/*23000001*/]System.Span`1/*01000031*/<uint8> input,
                        [out] int32& unwrappedOffset,
                        [out] int32& unwrappedLength,
                        [out] bool& wasEncrypted) cil managed
  // SIG: 20 04 11 3C 15 11 80 C5 01 05 10 08 10 08 10 02
  {
    // Method begins at RVA 0x2134
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::UnwrapInPlace

  .method /*0600003A*/ public hidebysig instance valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ 
          Wrap(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100002F*/<uint8> input,
               class [System.Memory/*23000005*/]System.Buffers.IBufferWriter`1/*01000030*/<uint8> outputWriter,
               bool requestEncryption,
               [out] bool& isEncrypted) cil managed
  // SIG: 20 04 11 3C 15 11 80 BD 01 05 15 12 80 C1 01 05 02 10 02
  {
    .param [2]/*08000040*/ 
    .custom /*0C000062:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2137
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthentication::Wrap

  .property /*1700000E*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/
          ImpersonationLevel()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ System.Net.Security.NegotiateAuthentication/*0200000C*/::get_ImpersonationLevel() /* 0600002B */
  } // end of property NegotiateAuthentication::ImpersonationLevel
  .property /*1700000F*/ instance callconv(8) bool
          IsAuthenticated()
  {
    .get instance bool System.Net.Security.NegotiateAuthentication/*0200000C*/::get_IsAuthenticated() /* 0600002C */
  } // end of property NegotiateAuthentication::IsAuthenticated
  .property /*17000010*/ instance callconv(8) bool
          IsEncrypted()
  {
    .get instance bool System.Net.Security.NegotiateAuthentication/*0200000C*/::get_IsEncrypted() /* 0600002D */
  } // end of property NegotiateAuthentication::IsEncrypted
  .property /*17000011*/ instance callconv(8) bool
          IsMutuallyAuthenticated()
  {
    .get instance bool System.Net.Security.NegotiateAuthentication/*0200000C*/::get_IsMutuallyAuthenticated() /* 0600002E */
  } // end of property NegotiateAuthentication::IsMutuallyAuthenticated
  .property /*17000012*/ instance callconv(8) bool
          IsServer()
  {
    .get instance bool System.Net.Security.NegotiateAuthentication/*0200000C*/::get_IsServer() /* 0600002F */
  } // end of property NegotiateAuthentication::IsServer
  .property /*17000013*/ instance callconv(8) bool
          IsSigned()
  {
    .get instance bool System.Net.Security.NegotiateAuthentication/*0200000C*/::get_IsSigned() /* 06000030 */
  } // end of property NegotiateAuthentication::IsSigned
  .property /*17000014*/ instance callconv(8) string
          Package()
  {
    .custom /*0C000038:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Net.Security.NegotiateAuthentication/*0200000C*/::get_Package() /* 06000031 */
  } // end of property NegotiateAuthentication::Package
  .property /*17000015*/ instance callconv(8) valuetype System.Net.Security.ProtectionLevel/*02000011*/
          ProtectionLevel()
  {
    .get instance valuetype System.Net.Security.ProtectionLevel/*02000011*/ System.Net.Security.NegotiateAuthentication/*0200000C*/::get_ProtectionLevel() /* 06000032 */
  } // end of property NegotiateAuthentication::ProtectionLevel
  .property /*17000016*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Security.Principal.IIdentity/*0100002E*/
          RemoteIdentity()
  {
    .custom /*0C00003D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Security.Principal.IIdentity/*0100002E*/ System.Net.Security.NegotiateAuthentication/*0200000C*/::get_RemoteIdentity() /* 06000033 */
  } // end of property NegotiateAuthentication::RemoteIdentity
  .property /*17000017*/ instance callconv(8) string
          TargetName()
  {
    .custom /*0C000040:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Security.NegotiateAuthentication/*0200000C*/::get_TargetName() /* 06000034 */
  } // end of property NegotiateAuthentication::TargetName
} // end of class System.Net.Security.NegotiateAuthentication

.class /*0200000D*/ public auto ansi beforefieldinit System.Net.Security.NegotiateAuthenticationClientOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000030:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000031:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600003B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationClientOptions::.ctor

  .method /*0600003C*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ 
          get_AllowedImpersonationLevel() cil managed
  // SIG: 20 00 11 80 B5
  {
    // Method begins at RVA 0x2142
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationClientOptions::get_AllowedImpersonationLevel

  .method /*0600003D*/ public hidebysig specialname 
          instance void  set_AllowedImpersonationLevel(valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 B5
  {
    // Method begins at RVA 0x2145
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationClientOptions::set_AllowedImpersonationLevel

  .method /*0600003E*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ 
          get_Binding() cil managed
  // SIG: 20 00 12 7D
  {
    .custom /*0C000060:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2147
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationClientOptions::get_Binding

  .method /*0600003F*/ public hidebysig specialname 
          instance void  set_Binding(class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ 'value') cil managed
  // SIG: 20 01 01 12 7D
  {
    .custom /*0C000061:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationClientOptions::set_Binding

  .method /*06000040*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ 
          get_Credential() cil managed
  // SIG: 20 00 12 80 C9
  {
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationClientOptions::get_Credential

  .method /*06000041*/ public hidebysig specialname 
          instance void  set_Credential(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    // Method begins at RVA 0x214f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationClientOptions::set_Credential

  .method /*06000042*/ public hidebysig specialname 
          instance string  get_Package() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2151
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationClientOptions::get_Package

  .method /*06000043*/ public hidebysig specialname 
          instance void  set_Package(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2154
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationClientOptions::set_Package

  .method /*06000044*/ public hidebysig specialname 
          instance valuetype System.Net.Security.ProtectionLevel/*02000011*/ 
          get_RequiredProtectionLevel() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x2156
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationClientOptions::get_RequiredProtectionLevel

  .method /*06000045*/ public hidebysig specialname 
          instance void  set_RequiredProtectionLevel(valuetype System.Net.Security.ProtectionLevel/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 11 44
  {
    // Method begins at RVA 0x2159
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationClientOptions::set_RequiredProtectionLevel

  .method /*06000046*/ public hidebysig specialname 
          instance bool  get_RequireMutualAuthentication() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x215b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationClientOptions::get_RequireMutualAuthentication

  .method /*06000047*/ public hidebysig specialname 
          instance void  set_RequireMutualAuthentication(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x215e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationClientOptions::set_RequireMutualAuthentication

  .method /*06000048*/ public hidebysig specialname 
          instance string  get_TargetName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000063:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2160
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationClientOptions::get_TargetName

  .method /*06000049*/ public hidebysig specialname 
          instance void  set_TargetName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000064:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2163
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationClientOptions::set_TargetName

  .property /*17000018*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/
          AllowedImpersonationLevel()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::get_AllowedImpersonationLevel() /* 0600003C */
    .set instance void System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::set_AllowedImpersonationLevel(valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/) /* 0600003D */
  } // end of property NegotiateAuthenticationClientOptions::AllowedImpersonationLevel
  .property /*17000019*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/
          Binding()
  {
    .custom /*0C000046:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::get_Binding() /* 0600003E */
    .set instance void System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::set_Binding(class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/) /* 0600003F */
  } // end of property NegotiateAuthenticationClientOptions::Binding
  .property /*1700001A*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/
          Credential()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::get_Credential() /* 06000040 */
    .set instance void System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::set_Credential(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/) /* 06000041 */
  } // end of property NegotiateAuthenticationClientOptions::Credential
  .property /*1700001B*/ instance callconv(8) string
          Package()
  {
    .get instance string System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::get_Package() /* 06000042 */
    .set instance void System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::set_Package(string) /* 06000043 */
  } // end of property NegotiateAuthenticationClientOptions::Package
  .property /*1700001C*/ instance callconv(8) valuetype System.Net.Security.ProtectionLevel/*02000011*/
          RequiredProtectionLevel()
  {
    .get instance valuetype System.Net.Security.ProtectionLevel/*02000011*/ System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::get_RequiredProtectionLevel() /* 06000044 */
    .set instance void System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::set_RequiredProtectionLevel(valuetype System.Net.Security.ProtectionLevel/*02000011*/) /* 06000045 */
  } // end of property NegotiateAuthenticationClientOptions::RequiredProtectionLevel
  .property /*1700001D*/ instance callconv(8) bool
          RequireMutualAuthentication()
  {
    .get instance bool System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::get_RequireMutualAuthentication() /* 06000046 */
    .set instance void System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::set_RequireMutualAuthentication(bool) /* 06000047 */
  } // end of property NegotiateAuthenticationClientOptions::RequireMutualAuthentication
  .property /*1700001E*/ instance callconv(8) string
          TargetName()
  {
    .custom /*0C00004C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::get_TargetName() /* 06000048 */
    .set instance void System.Net.Security.NegotiateAuthenticationClientOptions/*0200000D*/::set_TargetName(string) /* 06000049 */
  } // end of property NegotiateAuthenticationClientOptions::TargetName
} // end of class System.Net.Security.NegotiateAuthenticationClientOptions

.class /*0200000E*/ public auto ansi beforefieldinit System.Net.Security.NegotiateAuthenticationServerOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000033:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000034:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600004A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2165
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationServerOptions::.ctor

  .method /*0600004B*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ 
          get_Binding() cil managed
  // SIG: 20 00 12 7D
  {
    .custom /*0C000065:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationServerOptions::get_Binding

  .method /*0600004C*/ public hidebysig specialname 
          instance void  set_Binding(class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ 'value') cil managed
  // SIG: 20 01 01 12 7D
  {
    .custom /*0C000066:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2170
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationServerOptions::set_Binding

  .method /*0600004D*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ 
          get_Credential() cil managed
  // SIG: 20 00 12 80 C9
  {
    // Method begins at RVA 0x2172
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationServerOptions::get_Credential

  .method /*0600004E*/ public hidebysig specialname 
          instance void  set_Credential(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ 'value') cil managed
  // SIG: 20 01 01 12 80 C9
  {
    // Method begins at RVA 0x2175
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationServerOptions::set_Credential

  .method /*0600004F*/ public hidebysig specialname 
          instance string  get_Package() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2177
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationServerOptions::get_Package

  .method /*06000050*/ public hidebysig specialname 
          instance void  set_Package(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x217a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationServerOptions::set_Package

  .method /*06000051*/ public hidebysig specialname 
          instance class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ 
          get_Policy() cil managed
  // SIG: 20 00 12 10
  {
    .custom /*0C000067:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x217c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationServerOptions::get_Policy

  .method /*06000052*/ public hidebysig specialname 
          instance void  set_Policy(class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ 'value') cil managed
  // SIG: 20 01 01 12 10
  {
    .custom /*0C000068:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x217f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationServerOptions::set_Policy

  .method /*06000053*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ 
          get_RequiredImpersonationLevel() cil managed
  // SIG: 20 00 11 80 B5
  {
    // Method begins at RVA 0x2181
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationServerOptions::get_RequiredImpersonationLevel

  .method /*06000054*/ public hidebysig specialname 
          instance void  set_RequiredImpersonationLevel(valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ 'value') cil managed
  // SIG: 20 01 01 11 80 B5
  {
    // Method begins at RVA 0x2184
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationServerOptions::set_RequiredImpersonationLevel

  .method /*06000055*/ public hidebysig specialname 
          instance valuetype System.Net.Security.ProtectionLevel/*02000011*/ 
          get_RequiredProtectionLevel() cil managed
  // SIG: 20 00 11 44
  {
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateAuthenticationServerOptions::get_RequiredProtectionLevel

  .method /*06000056*/ public hidebysig specialname 
          instance void  set_RequiredProtectionLevel(valuetype System.Net.Security.ProtectionLevel/*02000011*/ 'value') cil managed
  // SIG: 20 01 01 11 44
  {
    // Method begins at RVA 0x2189
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateAuthenticationServerOptions::set_RequiredProtectionLevel

  .property /*1700001F*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/
          Binding()
  {
    .custom /*0C00004D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::get_Binding() /* 0600004B */
    .set instance void System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::set_Binding(class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/) /* 0600004C */
  } // end of property NegotiateAuthenticationServerOptions::Binding
  .property /*17000020*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/
          Credential()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::get_Credential() /* 0600004D */
    .set instance void System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::set_Credential(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/) /* 0600004E */
  } // end of property NegotiateAuthenticationServerOptions::Credential
  .property /*17000021*/ instance callconv(8) string
          Package()
  {
    .get instance string System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::get_Package() /* 0600004F */
    .set instance void System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::set_Package(string) /* 06000050 */
  } // end of property NegotiateAuthenticationServerOptions::Package
  .property /*17000022*/ instance callconv(8) class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/
          Policy()
  {
    .custom /*0C00004E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::get_Policy() /* 06000051 */
    .set instance void System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::set_Policy(class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/) /* 06000052 */
  } // end of property NegotiateAuthenticationServerOptions::Policy
  .property /*17000023*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/
          RequiredImpersonationLevel()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::get_RequiredImpersonationLevel() /* 06000053 */
    .set instance void System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::set_RequiredImpersonationLevel(valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/) /* 06000054 */
  } // end of property NegotiateAuthenticationServerOptions::RequiredImpersonationLevel
  .property /*17000024*/ instance callconv(8) valuetype System.Net.Security.ProtectionLevel/*02000011*/
          RequiredProtectionLevel()
  {
    .get instance valuetype System.Net.Security.ProtectionLevel/*02000011*/ System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::get_RequiredProtectionLevel() /* 06000055 */
    .set instance void System.Net.Security.NegotiateAuthenticationServerOptions/*0200000E*/::set_RequiredProtectionLevel(valuetype System.Net.Security.ProtectionLevel/*02000011*/) /* 06000056 */
  } // end of property NegotiateAuthenticationServerOptions::RequiredProtectionLevel
} // end of class System.Net.Security.NegotiateAuthenticationServerOptions

.class /*0200000F*/ public auto ansi sealed System.Net.Security.NegotiateAuthenticationStatusCode
       extends [System.Runtime/*23000001*/]System.Enum/*01000021*/
{
  .field /*0400000C*/ public specialname rtspecialname int32 value__
  .field /*0400000D*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ Completed = int32(0x00000000)
  .field /*0400000E*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ ContinueNeeded = int32(0x00000001)
  .field /*0400000F*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ GenericFailure = int32(0x00000002)
  .field /*04000010*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ BadBinding = int32(0x00000003)
  .field /*04000011*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ Unsupported = int32(0x00000004)
  .field /*04000012*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ MessageAltered = int32(0x00000005)
  .field /*04000013*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ ContextExpired = int32(0x00000006)
  .field /*04000014*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ CredentialsExpired = int32(0x00000007)
  .field /*04000015*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ InvalidCredentials = int32(0x00000008)
  .field /*04000016*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ InvalidToken = int32(0x00000009)
  .field /*04000017*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ UnknownCredentials = int32(0x0000000A)
  .field /*04000018*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ QopNotSupported = int32(0x0000000B)
  .field /*04000019*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ OutOfSequence = int32(0x0000000C)
  .field /*0400001A*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ SecurityQosFailed = int32(0x0000000D)
  .field /*0400001B*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ TargetUnknown = int32(0x0000000E)
  .field /*0400001C*/ public static literal valuetype System.Net.Security.NegotiateAuthenticationStatusCode/*0200000F*/ ImpersonationValidationFailed = int32(0x0000000F)
} // end of class System.Net.Security.NegotiateAuthenticationStatusCode

.class /*02000010*/ public auto ansi beforefieldinit System.Net.Security.NegotiateStream
       extends System.Net.Security.AuthenticatedStream/*02000008*/
{
  .custom /*0C000035:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000036:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*06000057*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ innerStream) cil managed
  // SIG: 20 01 01 12 80 91
  {
    // Method begins at RVA 0x218b
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000019       */ call       instance void System.Net.Security.AuthenticatedStream/*02000008*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/,
                                                                                                                                bool) /* 06000019 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::.ctor

  .method /*06000058*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ innerStream,
                               bool leaveInnerStreamOpen) cil managed
  // SIG: 20 02 01 12 80 91 02
  {
    // Method begins at RVA 0x2195
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000019       */ call       instance void System.Net.Security.AuthenticatedStream/*02000008*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/,
                                                                                                                                bool) /* 06000019 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::.ctor

  .method /*06000059*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x219f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_CanRead

  .method /*0600005A*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_CanSeek

  .method /*0600005B*/ public hidebysig specialname virtual 
          instance bool  get_CanTimeout() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_CanTimeout

  .method /*0600005C*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_CanWrite

  .method /*0600005D*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ 
          get_ImpersonationLevel() cil managed
  // SIG: 20 00 11 80 B5
  {
    // Method begins at RVA 0x21ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_ImpersonationLevel

  .method /*0600005E*/ public hidebysig specialname virtual 
          instance bool  get_IsAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_IsAuthenticated

  .method /*0600005F*/ public hidebysig specialname virtual 
          instance bool  get_IsEncrypted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_IsEncrypted

  .method /*06000060*/ public hidebysig specialname virtual 
          instance bool  get_IsMutuallyAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_IsMutuallyAuthenticated

  .method /*06000061*/ public hidebysig specialname virtual 
          instance bool  get_IsServer() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_IsServer

  .method /*06000062*/ public hidebysig specialname virtual 
          instance bool  get_IsSigned() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x21ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_IsSigned

  .method /*06000063*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_Length

  .method /*06000064*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x21c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_Position

  .method /*06000065*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x21c3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::set_Position

  .method /*06000066*/ public hidebysig specialname virtual 
          instance int32  get_ReadTimeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_ReadTimeout

  .method /*06000067*/ public hidebysig specialname virtual 
          instance void  set_ReadTimeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::set_ReadTimeout

  .method /*06000068*/ public hidebysig newslot specialname virtual 
          instance class [System.Runtime/*23000001*/]System.Security.Principal.IIdentity/*0100002E*/ 
          get_RemoteIdentity() cil managed
  // SIG: 20 00 12 80 B9
  {
    // Method begins at RVA 0x21ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_RemoteIdentity

  .method /*06000069*/ public hidebysig specialname virtual 
          instance int32  get_WriteTimeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x21cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::get_WriteTimeout

  .method /*0600006A*/ public hidebysig specialname virtual 
          instance void  set_WriteTimeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x21d0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::set_WriteTimeout

  .method /*0600006B*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsClient() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21d2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsClient

  .method /*0600006C*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsClient(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                              class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ binding,
                                              string targetName) cil managed
  // SIG: 20 03 01 12 80 C9 12 7D 0E
  {
    .param [2]/*08000057*/ 
    .custom /*0C000069:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsClient

  .method /*0600006D*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsClient(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                              class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ binding,
                                              string targetName,
                                              valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                              valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ allowedImpersonationLevel) cil managed
  // SIG: 20 05 01 12 80 C9 12 7D 0E 11 44 11 80 B5
  {
    .param [2]/*0800005A*/ 
    .custom /*0C00006A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsClient

  .method /*0600006E*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsClient(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                              string targetName) cil managed
  // SIG: 20 02 01 12 80 C9 0E
  {
    // Method begins at RVA 0x21d8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsClient

  .method /*0600006F*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsClient(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                              string targetName,
                                              valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                              valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ allowedImpersonationLevel) cil managed
  // SIG: 20 04 01 12 80 C9 0E 11 44 11 80 B5
  {
    // Method begins at RVA 0x21da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsClient

  .method /*06000070*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync() cil managed
  // SIG: 20 00 12 80 CD
  {
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsClientAsync

  .method /*06000071*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ binding,
                                    string targetName) cil managed
  // SIG: 20 03 12 80 CD 12 80 C9 12 7D 0E
  {
    .param [2]/*08000065*/ 
    .custom /*0C00006D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsClientAsync

  .method /*06000072*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ binding,
                                    string targetName,
                                    valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                    valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ allowedImpersonationLevel) cil managed
  // SIG: 20 05 12 80 CD 12 80 C9 12 7D 0E 11 44 11 80 B5
  {
    .param [2]/*08000068*/ 
    .custom /*0C00006F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsClientAsync

  .method /*06000073*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    string targetName) cil managed
  // SIG: 20 02 12 80 CD 12 80 C9 0E
  {
    // Method begins at RVA 0x21e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsClientAsync

  .method /*06000074*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    string targetName,
                                    valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                    valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ allowedImpersonationLevel) cil managed
  // SIG: 20 04 12 80 CD 12 80 C9 0E 11 44 11 80 B5
  {
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsClientAsync

  .method /*06000075*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsServer() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21eb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsServer

  .method /*06000076*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsServer(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                              valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                              valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ requiredImpersonationLevel) cil managed
  // SIG: 20 03 01 12 80 C9 11 44 11 80 B5
  {
    // Method begins at RVA 0x21ed
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsServer

  .method /*06000077*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsServer(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                              class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ policy,
                                              valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                              valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ requiredImpersonationLevel) cil managed
  // SIG: 20 04 01 12 80 C9 12 10 11 44 11 80 B5
  {
    .param [2]/*08000076*/ 
    .custom /*0C000070:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsServer

  .method /*06000078*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsServer(class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ policy) cil managed
  // SIG: 20 01 01 12 10
  {
    .custom /*0C000071:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::AuthenticateAsServer

  .method /*06000079*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync() cil managed
  // SIG: 20 00 12 80 CD
  {
    // Method begins at RVA 0x21f3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsServerAsync

  .method /*0600007A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                    valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ requiredImpersonationLevel) cil managed
  // SIG: 20 03 12 80 CD 12 80 C9 11 44 11 80 B5
  {
    // Method begins at RVA 0x21f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsServerAsync

  .method /*0600007B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ policy,
                                    valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                    valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ requiredImpersonationLevel) cil managed
  // SIG: 20 04 12 80 CD 12 80 C9 12 10 11 44 11 80 B5
  {
    .param [2]/*0800007E*/ 
    .custom /*0C000073:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsServerAsync

  .method /*0600007C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync(class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ policy) cil managed
  // SIG: 20 01 12 80 CD 12 10
  {
    .param [1]/*08000081*/ 
    .custom /*0C000074:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::AuthenticateAsServerAsync

  .method /*0600007D*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsClient(class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 02 12 80 A9 12 80 AD 1C
  {
    .custom /*0C000072:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000082*/ 
    .custom /*0C000076:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsClient

  .method /*0600007E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsClient(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ binding,
                                    string targetName,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 05 12 80 A9 12 80 C9 12 7D 0E 12 80 AD 1C
  {
    .param [2]/*08000086*/ 
    .custom /*0C000079:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000088*/ 
    .custom /*0C00007A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000089*/ 
    .custom /*0C00007B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2202
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsClient

  .method /*0600007F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsClient(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    class [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExtendedProtection.ChannelBinding/*0100001F*/ binding,
                                    string targetName,
                                    valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                    valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ allowedImpersonationLevel,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 07 12 80 A9 12 80 C9 12 7D 0E 11 44 11 80 B5 12 80 AD 1C
  {
    .param [2]/*0800008B*/ 
    .custom /*0C00007C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*0800008F*/ 
    .custom /*0C00007D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [7]/*08000090*/ 
    .custom /*0C00007E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2205
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsClient

  .method /*06000080*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsClient(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    string targetName,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 04 12 80 A9 12 80 C9 0E 12 80 AD 1C
  {
    .param [3]/*08000093*/ 
    .custom /*0C000080:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000094*/ 
    .custom /*0C000081:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2208
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsClient

  .method /*06000081*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsClient(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    string targetName,
                                    valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                    valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ allowedImpersonationLevel,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 06 12 80 A9 12 80 C9 0E 11 44 11 80 B5 12 80 AD 1C
  {
    .param [5]/*08000099*/ 
    .custom /*0C000084:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*0800009A*/ 
    .custom /*0C000085:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x220b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsClient

  .method /*06000082*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsServer(class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 02 12 80 A9 12 80 AD 1C
  {
    .custom /*0C000075:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800009B*/ 
    .custom /*0C000086:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x220e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsServer

  .method /*06000083*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsServer(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                    valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ requiredImpersonationLevel,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 05 12 80 A9 12 80 C9 11 44 11 80 B5 12 80 AD 1C
  {
    .param [4]/*080000A1*/ 
    .custom /*0C000089:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000A2*/ 
    .custom /*0C00008A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2211
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsServer

  .method /*06000084*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsServer(class [System.Net.Primitives/*23000002*/]System.Net.NetworkCredential/*01000032*/ credential,
                                    class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ policy,
                                    valuetype System.Net.Security.ProtectionLevel/*02000011*/ requiredProtectionLevel,
                                    valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ requiredImpersonationLevel,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 06 12 80 A9 12 80 C9 12 10 11 44 11 80 B5 12 80 AD 1C
  {
    .custom /*0C000077:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000A3*/ 
    .custom /*0C00008B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [1]/*080000A4*/ 
    .custom /*0C00008D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2214
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsServer

  .method /*06000085*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsServer(class System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy/*02000004*/ policy,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 03 12 80 A9 12 10 12 80 AD 1C
  {
    .custom /*0C000078:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*080000AA*/ 
    .custom /*0C000090:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2217
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginAuthenticateAsServer

  .method /*06000086*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 80 A9 1D 05 08 08 12 80 AD 1C
  {
    .param [4]/*080000B1*/ 
    .custom /*0C000092:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000B2*/ 
    .custom /*0C000093:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginRead

  .method /*06000087*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 80 A9 1D 05 08 08 12 80 AD 1C
  {
    .param [4]/*080000B6*/ 
    .custom /*0C000094:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*080000B7*/ 
    .custom /*0C000095:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::BeginWrite

  .method /*06000088*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2220
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::Dispose

  .method /*06000089*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x2222
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::DisposeAsync

  .method /*0600008A*/ public hidebysig newslot virtual 
          instance void  EndAuthenticateAsClient(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ asyncResult) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x2225
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::EndAuthenticateAsClient

  .method /*0600008B*/ public hidebysig newslot virtual 
          instance void  EndAuthenticateAsServer(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ asyncResult) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x2227
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::EndAuthenticateAsServer

  .method /*0600008C*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ asyncResult) cil managed
  // SIG: 20 01 08 12 80 A9
  {
    // Method begins at RVA 0x2229
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::EndRead

  .method /*0600008D*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ asyncResult) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x222c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::EndWrite

  .method /*0600008E*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x222e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::Flush

  .method /*0600008F*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 CD 11 80 D1
  {
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::FlushAsync

  .method /*06000090*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x2233
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::Read

  .method /*06000091*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000035*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 D5 01 08 1D 05 08 08 11 80 D1
  {
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::ReadAsync

  .method /*06000092*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000036*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000037*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 D9 01 08 15 11 80 DD 01 05 11 80 D1
  {
    .custom /*0C00007F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080000C6*/  = nullref
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::ReadAsync

  .method /*06000093*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000038*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 E1
  {
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::Seek

  .method /*06000094*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x223f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::SetLength

  .method /*06000095*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2241
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method NegotiateStream::Write

  .method /*06000096*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 CD 1D 05 08 08 11 80 D1
  {
    // Method begins at RVA 0x2243
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::WriteAsync

  .method /*06000097*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000039*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 02 11 80 95 15 11 80 E5 01 05 11 80 D1
  {
    .custom /*0C000082:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080000D2*/  = nullref
    // Method begins at RVA 0x2246
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method NegotiateStream::WriteAsync

  .property /*17000025*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_CanRead() /* 06000059 */
  } // end of property NegotiateStream::CanRead
  .property /*17000026*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_CanSeek() /* 0600005A */
  } // end of property NegotiateStream::CanSeek
  .property /*17000027*/ instance callconv(8) bool
          CanTimeout()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_CanTimeout() /* 0600005B */
  } // end of property NegotiateStream::CanTimeout
  .property /*17000028*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_CanWrite() /* 0600005C */
  } // end of property NegotiateStream::CanWrite
  .property /*17000029*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/
          ImpersonationLevel()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.Security.Principal.TokenImpersonationLevel/*0100002D*/ System.Net.Security.NegotiateStream/*02000010*/::get_ImpersonationLevel() /* 0600005D */
  } // end of property NegotiateStream::ImpersonationLevel
  .property /*1700002A*/ instance callconv(8) bool
          IsAuthenticated()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_IsAuthenticated() /* 0600005E */
  } // end of property NegotiateStream::IsAuthenticated
  .property /*1700002B*/ instance callconv(8) bool
          IsEncrypted()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_IsEncrypted() /* 0600005F */
  } // end of property NegotiateStream::IsEncrypted
  .property /*1700002C*/ instance callconv(8) bool
          IsMutuallyAuthenticated()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_IsMutuallyAuthenticated() /* 06000060 */
  } // end of property NegotiateStream::IsMutuallyAuthenticated
  .property /*1700002D*/ instance callconv(8) bool
          IsServer()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_IsServer() /* 06000061 */
  } // end of property NegotiateStream::IsServer
  .property /*1700002E*/ instance callconv(8) bool
          IsSigned()
  {
    .get instance bool System.Net.Security.NegotiateStream/*02000010*/::get_IsSigned() /* 06000062 */
  } // end of property NegotiateStream::IsSigned
  .property /*1700002F*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.Net.Security.NegotiateStream/*02000010*/::get_Length() /* 06000063 */
  } // end of property NegotiateStream::Length
  .property /*17000030*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.Net.Security.NegotiateStream/*02000010*/::get_Position() /* 06000064 */
    .set instance void System.Net.Security.NegotiateStream/*02000010*/::set_Position(int64) /* 06000065 */
  } // end of property NegotiateStream::Position
  .property /*17000031*/ instance callconv(8) int32
          ReadTimeout()
  {
    .get instance int32 System.Net.Security.NegotiateStream/*02000010*/::get_ReadTimeout() /* 06000066 */
    .set instance void System.Net.Security.NegotiateStream/*02000010*/::set_ReadTimeout(int32) /* 06000067 */
  } // end of property NegotiateStream::ReadTimeout
  .property /*17000032*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Security.Principal.IIdentity/*0100002E*/
          RemoteIdentity()
  {
    .get instance class [System.Runtime/*23000001*/]System.Security.Principal.IIdentity/*0100002E*/ System.Net.Security.NegotiateStream/*02000010*/::get_RemoteIdentity() /* 06000068 */
  } // end of property NegotiateStream::RemoteIdentity
  .property /*17000033*/ instance callconv(8) int32
          WriteTimeout()
  {
    .get instance int32 System.Net.Security.NegotiateStream/*02000010*/::get_WriteTimeout() /* 06000069 */
    .set instance void System.Net.Security.NegotiateStream/*02000010*/::set_WriteTimeout(int32) /* 0600006A */
  } // end of property NegotiateStream::WriteTimeout
} // end of class System.Net.Security.NegotiateStream

.class /*02000011*/ public auto ansi sealed System.Net.Security.ProtectionLevel
       extends [System.Runtime/*23000001*/]System.Enum/*01000021*/
{
  .field /*0400001D*/ public specialname rtspecialname int32 value__
  .field /*0400001E*/ public static literal valuetype System.Net.Security.ProtectionLevel/*02000011*/ None = int32(0x00000000)
  .field /*0400001F*/ public static literal valuetype System.Net.Security.ProtectionLevel/*02000011*/ Sign = int32(0x00000001)
  .field /*04000020*/ public static literal valuetype System.Net.Security.ProtectionLevel/*02000011*/ EncryptAndSign = int32(0x00000002)
} // end of class System.Net.Security.ProtectionLevel

.class /*02000012*/ public auto ansi sealed System.Net.Security.RemoteCertificateValidationCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000027*/
{
  .method /*06000098*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method RemoteCertificateValidationCallback::.ctor

  .method /*06000099*/ public hidebysig newslot virtual 
          instance bool  Invoke(object sender,
                                class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ certificate,
                                class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100003A*/ chain,
                                valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*0100003B*/ sslPolicyErrors) runtime managed
  // SIG: 20 04 02 1C 12 80 A1 12 80 E9 11 80 ED
  {
    .custom /*0C000083:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000D5*/ 
    .custom /*0C000096:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method RemoteCertificateValidationCallback::Invoke

  .method /*0600009A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginInvoke(object sender,
                      class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ certificate,
                      class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Chain/*0100003A*/ chain,
                      valuetype [System.Net.Primitives/*23000002*/]System.Net.Security.SslPolicyErrors/*0100003B*/ sslPolicyErrors,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ callback,
                      object 'object') runtime managed
  // SIG: 20 06 12 80 A9 1C 12 80 A1 12 80 E9 11 80 ED 12 80 AD 1C
  {
    .param [1]/*080000D9*/ 
    .custom /*0C000097:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000DA*/ 
    .custom /*0C000098:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080000DB*/ 
    .custom /*0C000099:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method RemoteCertificateValidationCallback::BeginInvoke

  .method /*0600009B*/ public hidebysig newslot virtual 
          instance bool  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ result) runtime managed
  // SIG: 20 01 02 12 80 A9
  {
  } // end of method RemoteCertificateValidationCallback::EndInvoke

} // end of class System.Net.Security.RemoteCertificateValidationCallback

.class /*02000013*/ public auto ansi sealed System.Net.Security.ServerCertificateSelectionCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000027*/
{
  .method /*0600009C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ServerCertificateSelectionCallback::.ctor

  .method /*0600009D*/ public hidebysig newslot virtual 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ 
          Invoke(object sender,
                 string hostName) runtime managed
  // SIG: 20 02 12 80 A1 1C 0E
  {
    .custom /*0C000087:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000E3*/ 
    .custom /*0C00009A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method ServerCertificateSelectionCallback::Invoke

  .method /*0600009E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginInvoke(object sender,
                      string hostName,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 A9 1C 0E 12 80 AD 1C
  {
    .param [1]/*080000E4*/ 
    .custom /*0C00009B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000E5*/ 
    .custom /*0C00009C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method ServerCertificateSelectionCallback::BeginInvoke

  .method /*0600009F*/ public hidebysig newslot virtual 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ result) runtime managed
  // SIG: 20 01 12 80 A1 12 80 A9
  {
    .param [0]/*080000E8*/ 
    .custom /*0C00009D:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
  } // end of method ServerCertificateSelectionCallback::EndInvoke

} // end of class System.Net.Security.ServerCertificateSelectionCallback

.class /*02000014*/ public auto ansi sealed System.Net.Security.ServerOptionsSelectionCallback
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000027*/
{
  .method /*060000A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method ServerOptionsSelectionCallback::.ctor

  .method /*060000A1*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000036*/<class System.Net.Security.SslServerAuthenticationOptions/*02000019*/> 
          Invoke(class System.Net.Security.SslStream/*0200001A*/ 'stream',
                 valuetype System.Net.Security.SslClientHelloInfo/*02000018*/ clientHelloInfo,
                 object state,
                 valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) runtime managed
  // SIG: 20 04 15 11 80 D9 01 12 64 12 68 11 60 1C 11 80 D1
  {
    .custom /*0C000088:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    .param [0]/*080000EC*/ 
    .custom /*0C00009F:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000018 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
    .param [3]/*080000EF*/ 
    .custom /*0C0000A1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method ServerOptionsSelectionCallback::Invoke

  .method /*060000A2*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginInvoke(class System.Net.Security.SslStream/*0200001A*/ 'stream',
                      valuetype System.Net.Security.SslClientHelloInfo/*02000018*/ clientHelloInfo,
                      object state,
                      valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ callback,
                      object 'object') runtime managed
  // SIG: 20 06 12 80 A9 12 68 11 60 1C 11 80 D1 12 80 AD 1C
  {
    .param [1]/*080000F1*/ 
    .custom /*0C0000A2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000F3*/ 
    .custom /*0C0000A3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
  } // end of method ServerOptionsSelectionCallback::BeginInvoke

  .method /*060000A3*/ public hidebysig newslot virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000036*/<class System.Net.Security.SslServerAuthenticationOptions/*02000019*/> 
          EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ result) runtime managed
  // SIG: 20 01 15 11 80 D9 01 12 64 12 80 A9
  {
    .param [0]/*080000F7*/ 
    .custom /*0C0000A4:0A000018*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8[]) /* 0A000018 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  } // end of method ServerOptionsSelectionCallback::EndInvoke

} // end of class System.Net.Security.ServerOptionsSelectionCallback

.class /*02000015*/ public sequential ansi sealed beforefieldinit System.Net.Security.SslApplicationProtocol
       extends [System.Runtime/*23000001*/]System.ValueType/*0100003D*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100003E*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/>/*1B000001*/
{
  .custom /*0C000039:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003C*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
  .field /*04000021*/ private initonly object _dummy
  .field /*04000022*/ private initonly int32 _dummyPrimitive
  .field /*04000023*/ public static initonly valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ Http11
  .field /*04000024*/ public static initonly valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ Http2
  .field /*04000025*/ public static initonly valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ Http3
  .method /*060000A4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] protocol) cil managed
  // SIG: 20 01 01 1D 05
  {
    .custom /*0C00008C:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2249
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::.ctor

  .method /*060000A5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string protocol) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00008E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x224c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::.ctor

  .method /*060000A6*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000039*/<uint8> 
          get_Protocol() cil managed
  // SIG: 20 00 15 11 80 E5 01 05
  {
    // Method begins at RVA 0x224f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::get_Protocol

  .method /*060000A7*/ public hidebysig newslot virtual final 
          instance bool  Equals(valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ other) cil managed
  // SIG: 20 01 02 11 54
  {
    // Method begins at RVA 0x2252
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::Equals

  .method /*060000A8*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00008F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080000FC*/ 
    .custom /*0C0000A5:0A00001A*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*0100003F*/::.ctor(bool) /* 0A00001A */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2255
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::Equals

  .method /*060000A9*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2258
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::GetHashCode

  .method /*060000AA*/ public hidebysig specialname static 
          bool  op_Equality(valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ left,
                            valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ right) cil managed
  // SIG: 00 02 02 11 54 11 54
  {
    // Method begins at RVA 0x225b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::op_Equality

  .method /*060000AB*/ public hidebysig specialname static 
          bool  op_Inequality(valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ left,
                              valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ right) cil managed
  // SIG: 00 02 02 11 54 11 54
  {
    // Method begins at RVA 0x225e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::op_Inequality

  .method /*060000AC*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000091:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2261
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslApplicationProtocol::ToString

  .property /*17000034*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000039*/<uint8>
          Protocol()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000039*/<uint8> System.Net.Security.SslApplicationProtocol/*02000015*/::get_Protocol() /* 060000A6 */
  } // end of property SslApplicationProtocol::Protocol
} // end of class System.Net.Security.SslApplicationProtocol

.class /*02000016*/ public auto ansi sealed beforefieldinit System.Net.Security.SslCertificateTrust
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C00003B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003C:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000AD*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2264
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SslCertificateTrust::.ctor

  .method /*060000AE*/ public hidebysig static 
          class System.Net.Security.SslCertificateTrust/*02000016*/ 
          CreateForX509Collection(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2Collection/*01000040*/ trustList,
                                  [opt] bool sendTrustInHandshake) cil managed
  // SIG: 00 02 12 58 12 81 01 02
  {
    .param [2]/*08000102*/  = bool(false)
    // Method begins at RVA 0x226c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslCertificateTrust::CreateForX509Collection

  .method /*060000AF*/ public hidebysig static 
          class System.Net.Security.SslCertificateTrust/*02000016*/ 
          CreateForX509Store(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Store/*01000041*/ store,
                             [opt] bool sendTrustInHandshake) cil managed
  // SIG: 00 02 12 58 12 81 05 02
  {
    .param [2]/*08000104*/  = bool(false)
    // Method begins at RVA 0x226f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslCertificateTrust::CreateForX509Store

} // end of class System.Net.Security.SslCertificateTrust

.class /*02000017*/ public auto ansi beforefieldinit System.Net.Security.SslClientAuthenticationOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C00003E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003F:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000B0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2272
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::.ctor

  .method /*060000B1*/ public hidebysig specialname 
          instance bool  get_AllowRenegotiation() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x227a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_AllowRenegotiation

  .method /*060000B2*/ public hidebysig specialname 
          instance void  set_AllowRenegotiation(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x227d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_AllowRenegotiation

  .method /*060000B3*/ public hidebysig specialname 
          instance bool  get_AllowTlsResume() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x227f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_AllowTlsResume

  .method /*060000B4*/ public hidebysig specialname 
          instance void  set_AllowTlsResume(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2282
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_AllowTlsResume

  .method /*060000B5*/ public hidebysig specialname 
          instance class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/> 
          get_ApplicationProtocols() cil managed
  // SIG: 20 00 15 12 81 09 01 11 54
  {
    // Method begins at RVA 0x2284
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_ApplicationProtocols

  .method /*060000B6*/ public hidebysig specialname 
          instance void  set_ApplicationProtocols(class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 09 01 11 54
  {
    // Method begins at RVA 0x2287
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_ApplicationProtocols

  .method /*060000B7*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/ 
          get_CertificateChainPolicy() cil managed
  // SIG: 20 00 12 81 0D
  {
    // Method begins at RVA 0x2289
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_CertificateChainPolicy

  .method /*060000B8*/ public hidebysig specialname 
          instance void  set_CertificateChainPolicy(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/ 'value') cil managed
  // SIG: 20 01 01 12 81 0D
  {
    // Method begins at RVA 0x228c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_CertificateChainPolicy

  .method /*060000B9*/ public hidebysig specialname 
          instance valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/ 
          get_CertificateRevocationCheckMode() cil managed
  // SIG: 20 00 11 81 11
  {
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_CertificateRevocationCheckMode

  .method /*060000BA*/ public hidebysig specialname 
          instance void  set_CertificateRevocationCheckMode(valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/ 'value') cil managed
  // SIG: 20 01 01 11 81 11
  {
    // Method begins at RVA 0x2291
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_CertificateRevocationCheckMode

  .method /*060000BB*/ public hidebysig specialname 
          instance class System.Net.Security.CipherSuitesPolicy/*02000009*/ 
          get_CipherSuitesPolicy() cil managed
  // SIG: 20 00 12 24
  {
    // Method begins at RVA 0x2293
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_CipherSuitesPolicy

  .method /*060000BC*/ public hidebysig specialname 
          instance void  set_CipherSuitesPolicy(class System.Net.Security.CipherSuitesPolicy/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x2296
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_CipherSuitesPolicy

  .method /*060000BD*/ public hidebysig specialname 
          instance class System.Net.Security.SslStreamCertificateContext/*0200001B*/ 
          get_ClientCertificateContext() cil managed
  // SIG: 20 00 12 6C
  {
    // Method begins at RVA 0x2298
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_ClientCertificateContext

  .method /*060000BE*/ public hidebysig specialname 
          instance void  set_ClientCertificateContext(class System.Net.Security.SslStreamCertificateContext/*0200001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6C
  {
    // Method begins at RVA 0x229b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_ClientCertificateContext

  .method /*060000BF*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ 
          get_ClientCertificates() cil managed
  // SIG: 20 00 12 80 A5
  {
    // Method begins at RVA 0x229d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_ClientCertificates

  .method /*060000C0*/ public hidebysig specialname 
          instance void  set_ClientCertificates(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A5
  {
    // Method begins at RVA 0x22a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_ClientCertificates

  .method /*060000C1*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ 
          get_EnabledSslProtocols() cil managed
  // SIG: 20 00 11 81 15
  {
    // Method begins at RVA 0x22a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_EnabledSslProtocols

  .method /*060000C2*/ public hidebysig specialname 
          instance void  set_EnabledSslProtocols(valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ 'value') cil managed
  // SIG: 20 01 01 11 81 15
  {
    // Method begins at RVA 0x22a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_EnabledSslProtocols

  .method /*060000C3*/ public hidebysig specialname 
          instance valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ 
          get_EncryptionPolicy() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x22a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_EncryptionPolicy

  .method /*060000C4*/ public hidebysig specialname 
          instance void  set_EncryptionPolicy(valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x22aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_EncryptionPolicy

  .method /*060000C5*/ public hidebysig specialname 
          instance class System.Net.Security.LocalCertificateSelectionCallback/*0200000B*/ 
          get_LocalCertificateSelectionCallback() cil managed
  // SIG: 20 00 12 2C
  {
    // Method begins at RVA 0x22ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_LocalCertificateSelectionCallback

  .method /*060000C6*/ public hidebysig specialname 
          instance void  set_LocalCertificateSelectionCallback(class System.Net.Security.LocalCertificateSelectionCallback/*0200000B*/ 'value') cil managed
  // SIG: 20 01 01 12 2C
  {
    // Method begins at RVA 0x22af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_LocalCertificateSelectionCallback

  .method /*060000C7*/ public hidebysig specialname 
          instance class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ 
          get_RemoteCertificateValidationCallback() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x22b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_RemoteCertificateValidationCallback

  .method /*060000C8*/ public hidebysig specialname 
          instance void  set_RemoteCertificateValidationCallback(class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ 'value') cil managed
  // SIG: 20 01 01 12 48
  {
    // Method begins at RVA 0x22b4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_RemoteCertificateValidationCallback

  .method /*060000C9*/ public hidebysig specialname 
          instance string  get_TargetHost() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientAuthenticationOptions::get_TargetHost

  .method /*060000CA*/ public hidebysig specialname 
          instance void  set_TargetHost(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22b9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslClientAuthenticationOptions::set_TargetHost

  .property /*17000035*/ instance callconv(8) bool
          AllowRenegotiation()
  {
    .get instance bool System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_AllowRenegotiation() /* 060000B1 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_AllowRenegotiation(bool) /* 060000B2 */
  } // end of property SslClientAuthenticationOptions::AllowRenegotiation
  .property /*17000036*/ instance callconv(8) bool
          AllowTlsResume()
  {
    .get instance bool System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_AllowTlsResume() /* 060000B3 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_AllowTlsResume(bool) /* 060000B4 */
  } // end of property SslClientAuthenticationOptions::AllowTlsResume
  .property /*17000037*/ instance callconv(8) class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/>
          ApplicationProtocols()
  {
    .get instance class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/> System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_ApplicationProtocols() /* 060000B5 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_ApplicationProtocols(class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/>) /* 060000B6 */
  } // end of property SslClientAuthenticationOptions::ApplicationProtocols
  .property /*17000038*/ instance callconv(8) class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/
          CertificateChainPolicy()
  {
    .get instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_CertificateChainPolicy() /* 060000B7 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_CertificateChainPolicy(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/) /* 060000B8 */
  } // end of property SslClientAuthenticationOptions::CertificateChainPolicy
  .property /*17000039*/ instance callconv(8) valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/
          CertificateRevocationCheckMode()
  {
    .get instance valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_CertificateRevocationCheckMode() /* 060000B9 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_CertificateRevocationCheckMode(valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/) /* 060000BA */
  } // end of property SslClientAuthenticationOptions::CertificateRevocationCheckMode
  .property /*1700003A*/ instance callconv(8) class System.Net.Security.CipherSuitesPolicy/*02000009*/
          CipherSuitesPolicy()
  {
    .get instance class System.Net.Security.CipherSuitesPolicy/*02000009*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_CipherSuitesPolicy() /* 060000BB */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_CipherSuitesPolicy(class System.Net.Security.CipherSuitesPolicy/*02000009*/) /* 060000BC */
  } // end of property SslClientAuthenticationOptions::CipherSuitesPolicy
  .property /*1700003B*/ instance callconv(8) class System.Net.Security.SslStreamCertificateContext/*0200001B*/
          ClientCertificateContext()
  {
    .get instance class System.Net.Security.SslStreamCertificateContext/*0200001B*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_ClientCertificateContext() /* 060000BD */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_ClientCertificateContext(class System.Net.Security.SslStreamCertificateContext/*0200001B*/) /* 060000BE */
  } // end of property SslClientAuthenticationOptions::ClientCertificateContext
  .property /*1700003C*/ instance callconv(8) class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/
          ClientCertificates()
  {
    .get instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_ClientCertificates() /* 060000BF */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_ClientCertificates(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/) /* 060000C0 */
  } // end of property SslClientAuthenticationOptions::ClientCertificates
  .property /*1700003D*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/
          EnabledSslProtocols()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_EnabledSslProtocols() /* 060000C1 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_EnabledSslProtocols(valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/) /* 060000C2 */
  } // end of property SslClientAuthenticationOptions::EnabledSslProtocols
  .property /*1700003E*/ instance callconv(8) valuetype System.Net.Security.EncryptionPolicy/*0200000A*/
          EncryptionPolicy()
  {
    .get instance valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_EncryptionPolicy() /* 060000C3 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_EncryptionPolicy(valuetype System.Net.Security.EncryptionPolicy/*0200000A*/) /* 060000C4 */
  } // end of property SslClientAuthenticationOptions::EncryptionPolicy
  .property /*1700003F*/ instance callconv(8) class System.Net.Security.LocalCertificateSelectionCallback/*0200000B*/
          LocalCertificateSelectionCallback()
  {
    .get instance class System.Net.Security.LocalCertificateSelectionCallback/*0200000B*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_LocalCertificateSelectionCallback() /* 060000C5 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_LocalCertificateSelectionCallback(class System.Net.Security.LocalCertificateSelectionCallback/*0200000B*/) /* 060000C6 */
  } // end of property SslClientAuthenticationOptions::LocalCertificateSelectionCallback
  .property /*17000040*/ instance callconv(8) class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/
          RemoteCertificateValidationCallback()
  {
    .get instance class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_RemoteCertificateValidationCallback() /* 060000C7 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_RemoteCertificateValidationCallback(class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/) /* 060000C8 */
  } // end of property SslClientAuthenticationOptions::RemoteCertificateValidationCallback
  .property /*17000041*/ instance callconv(8) string
          TargetHost()
  {
    .get instance string System.Net.Security.SslClientAuthenticationOptions/*02000017*/::get_TargetHost() /* 060000C9 */
    .set instance void System.Net.Security.SslClientAuthenticationOptions/*02000017*/::set_TargetHost(string) /* 060000CA */
  } // end of property SslClientAuthenticationOptions::TargetHost
} // end of class System.Net.Security.SslClientAuthenticationOptions

.class /*02000018*/ public sequential ansi sealed beforefieldinit System.Net.Security.SslClientHelloInfo
       extends [System.Runtime/*23000001*/]System.ValueType/*0100003D*/
{
  .custom /*0C000041:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000042:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000043:0A000019*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.IsReadOnlyAttribute/*0100003C*/::.ctor() /* 0A000019 */ = ( 01 00 00 00 ) 
  .field /*04000026*/ private initonly object _dummy
  .field /*04000027*/ private initonly int32 _dummyPrimitive
  .method /*060000CB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string serverName,
                               valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ sslProtocols) cil managed
  // SIG: 20 02 01 0E 11 81 15
  {
    // Method begins at RVA 0x22bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientHelloInfo::.ctor

  .method /*060000CC*/ public hidebysig specialname 
          instance string  get_ServerName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientHelloInfo::get_ServerName

  .method /*060000CD*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ 
          get_SslProtocols() cil managed
  // SIG: 20 00 11 81 15
  {
    // Method begins at RVA 0x22c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslClientHelloInfo::get_SslProtocols

  .property /*17000042*/ instance callconv(8) string
          ServerName()
  {
    .get instance string System.Net.Security.SslClientHelloInfo/*02000018*/::get_ServerName() /* 060000CC */
  } // end of property SslClientHelloInfo::ServerName
  .property /*17000043*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/
          SslProtocols()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ System.Net.Security.SslClientHelloInfo/*02000018*/::get_SslProtocols() /* 060000CD */
  } // end of property SslClientHelloInfo::SslProtocols
} // end of class System.Net.Security.SslClientHelloInfo

.class /*02000019*/ public auto ansi beforefieldinit System.Net.Security.SslServerAuthenticationOptions
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000044:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000045:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000CE*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22c4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::.ctor

  .method /*060000CF*/ public hidebysig specialname 
          instance bool  get_AllowRenegotiation() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_AllowRenegotiation

  .method /*060000D0*/ public hidebysig specialname 
          instance void  set_AllowRenegotiation(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_AllowRenegotiation

  .method /*060000D1*/ public hidebysig specialname 
          instance bool  get_AllowTlsResume() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_AllowTlsResume

  .method /*060000D2*/ public hidebysig specialname 
          instance void  set_AllowTlsResume(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22d4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_AllowTlsResume

  .method /*060000D3*/ public hidebysig specialname 
          instance class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/> 
          get_ApplicationProtocols() cil managed
  // SIG: 20 00 15 12 81 09 01 11 54
  {
    // Method begins at RVA 0x22d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_ApplicationProtocols

  .method /*060000D4*/ public hidebysig specialname 
          instance void  set_ApplicationProtocols(class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/> 'value') cil managed
  // SIG: 20 01 01 15 12 81 09 01 11 54
  {
    // Method begins at RVA 0x22d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_ApplicationProtocols

  .method /*060000D5*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/ 
          get_CertificateChainPolicy() cil managed
  // SIG: 20 00 12 81 0D
  {
    // Method begins at RVA 0x22db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_CertificateChainPolicy

  .method /*060000D6*/ public hidebysig specialname 
          instance void  set_CertificateChainPolicy(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/ 'value') cil managed
  // SIG: 20 01 01 12 81 0D
  {
    // Method begins at RVA 0x22de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_CertificateChainPolicy

  .method /*060000D7*/ public hidebysig specialname 
          instance valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/ 
          get_CertificateRevocationCheckMode() cil managed
  // SIG: 20 00 11 81 11
  {
    // Method begins at RVA 0x22e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_CertificateRevocationCheckMode

  .method /*060000D8*/ public hidebysig specialname 
          instance void  set_CertificateRevocationCheckMode(valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/ 'value') cil managed
  // SIG: 20 01 01 11 81 11
  {
    // Method begins at RVA 0x22e3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_CertificateRevocationCheckMode

  .method /*060000D9*/ public hidebysig specialname 
          instance class System.Net.Security.CipherSuitesPolicy/*02000009*/ 
          get_CipherSuitesPolicy() cil managed
  // SIG: 20 00 12 24
  {
    // Method begins at RVA 0x22e5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_CipherSuitesPolicy

  .method /*060000DA*/ public hidebysig specialname 
          instance void  set_CipherSuitesPolicy(class System.Net.Security.CipherSuitesPolicy/*02000009*/ 'value') cil managed
  // SIG: 20 01 01 12 24
  {
    // Method begins at RVA 0x22e8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_CipherSuitesPolicy

  .method /*060000DB*/ public hidebysig specialname 
          instance bool  get_ClientCertificateRequired() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22ea
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_ClientCertificateRequired

  .method /*060000DC*/ public hidebysig specialname 
          instance void  set_ClientCertificateRequired(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22ed
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_ClientCertificateRequired

  .method /*060000DD*/ public hidebysig specialname 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ 
          get_EnabledSslProtocols() cil managed
  // SIG: 20 00 11 81 15
  {
    // Method begins at RVA 0x22ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_EnabledSslProtocols

  .method /*060000DE*/ public hidebysig specialname 
          instance void  set_EnabledSslProtocols(valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ 'value') cil managed
  // SIG: 20 01 01 11 81 15
  {
    // Method begins at RVA 0x22f2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_EnabledSslProtocols

  .method /*060000DF*/ public hidebysig specialname 
          instance valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ 
          get_EncryptionPolicy() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x22f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_EncryptionPolicy

  .method /*060000E0*/ public hidebysig specialname 
          instance void  set_EncryptionPolicy(valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x22f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_EncryptionPolicy

  .method /*060000E1*/ public hidebysig specialname 
          instance class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ 
          get_RemoteCertificateValidationCallback() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x22f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_RemoteCertificateValidationCallback

  .method /*060000E2*/ public hidebysig specialname 
          instance void  set_RemoteCertificateValidationCallback(class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ 'value') cil managed
  // SIG: 20 01 01 12 48
  {
    // Method begins at RVA 0x22fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_RemoteCertificateValidationCallback

  .method /*060000E3*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ 
          get_ServerCertificate() cil managed
  // SIG: 20 00 12 80 A1
  {
    // Method begins at RVA 0x22fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_ServerCertificate

  .method /*060000E4*/ public hidebysig specialname 
          instance void  set_ServerCertificate(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A1
  {
    // Method begins at RVA 0x2301
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_ServerCertificate

  .method /*060000E5*/ public hidebysig specialname 
          instance class System.Net.Security.SslStreamCertificateContext/*0200001B*/ 
          get_ServerCertificateContext() cil managed
  // SIG: 20 00 12 6C
  {
    // Method begins at RVA 0x2303
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_ServerCertificateContext

  .method /*060000E6*/ public hidebysig specialname 
          instance void  set_ServerCertificateContext(class System.Net.Security.SslStreamCertificateContext/*0200001B*/ 'value') cil managed
  // SIG: 20 01 01 12 6C
  {
    // Method begins at RVA 0x2306
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_ServerCertificateContext

  .method /*060000E7*/ public hidebysig specialname 
          instance class System.Net.Security.ServerCertificateSelectionCallback/*02000013*/ 
          get_ServerCertificateSelectionCallback() cil managed
  // SIG: 20 00 12 4C
  {
    // Method begins at RVA 0x2308
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslServerAuthenticationOptions::get_ServerCertificateSelectionCallback

  .method /*060000E8*/ public hidebysig specialname 
          instance void  set_ServerCertificateSelectionCallback(class System.Net.Security.ServerCertificateSelectionCallback/*02000013*/ 'value') cil managed
  // SIG: 20 01 01 12 4C
  {
    // Method begins at RVA 0x230b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslServerAuthenticationOptions::set_ServerCertificateSelectionCallback

  .property /*17000044*/ instance callconv(8) bool
          AllowRenegotiation()
  {
    .get instance bool System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_AllowRenegotiation() /* 060000CF */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_AllowRenegotiation(bool) /* 060000D0 */
  } // end of property SslServerAuthenticationOptions::AllowRenegotiation
  .property /*17000045*/ instance callconv(8) bool
          AllowTlsResume()
  {
    .get instance bool System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_AllowTlsResume() /* 060000D1 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_AllowTlsResume(bool) /* 060000D2 */
  } // end of property SslServerAuthenticationOptions::AllowTlsResume
  .property /*17000046*/ instance callconv(8) class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/>
          ApplicationProtocols()
  {
    .get instance class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/> System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_ApplicationProtocols() /* 060000D3 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_ApplicationProtocols(class [System.Collections/*23000006*/]System.Collections.Generic.List`1/*01000042*/<valuetype System.Net.Security.SslApplicationProtocol/*02000015*/>) /* 060000D4 */
  } // end of property SslServerAuthenticationOptions::ApplicationProtocols
  .property /*17000047*/ instance callconv(8) class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/
          CertificateChainPolicy()
  {
    .get instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_CertificateChainPolicy() /* 060000D5 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_CertificateChainPolicy(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509ChainPolicy/*01000043*/) /* 060000D6 */
  } // end of property SslServerAuthenticationOptions::CertificateChainPolicy
  .property /*17000048*/ instance callconv(8) valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/
          CertificateRevocationCheckMode()
  {
    .get instance valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_CertificateRevocationCheckMode() /* 060000D7 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_CertificateRevocationCheckMode(valuetype [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509RevocationMode/*01000044*/) /* 060000D8 */
  } // end of property SslServerAuthenticationOptions::CertificateRevocationCheckMode
  .property /*17000049*/ instance callconv(8) class System.Net.Security.CipherSuitesPolicy/*02000009*/
          CipherSuitesPolicy()
  {
    .get instance class System.Net.Security.CipherSuitesPolicy/*02000009*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_CipherSuitesPolicy() /* 060000D9 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_CipherSuitesPolicy(class System.Net.Security.CipherSuitesPolicy/*02000009*/) /* 060000DA */
  } // end of property SslServerAuthenticationOptions::CipherSuitesPolicy
  .property /*1700004A*/ instance callconv(8) bool
          ClientCertificateRequired()
  {
    .get instance bool System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_ClientCertificateRequired() /* 060000DB */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_ClientCertificateRequired(bool) /* 060000DC */
  } // end of property SslServerAuthenticationOptions::ClientCertificateRequired
  .property /*1700004B*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/
          EnabledSslProtocols()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_EnabledSslProtocols() /* 060000DD */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_EnabledSslProtocols(valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/) /* 060000DE */
  } // end of property SslServerAuthenticationOptions::EnabledSslProtocols
  .property /*1700004C*/ instance callconv(8) valuetype System.Net.Security.EncryptionPolicy/*0200000A*/
          EncryptionPolicy()
  {
    .get instance valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_EncryptionPolicy() /* 060000DF */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_EncryptionPolicy(valuetype System.Net.Security.EncryptionPolicy/*0200000A*/) /* 060000E0 */
  } // end of property SslServerAuthenticationOptions::EncryptionPolicy
  .property /*1700004D*/ instance callconv(8) class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/
          RemoteCertificateValidationCallback()
  {
    .get instance class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_RemoteCertificateValidationCallback() /* 060000E1 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_RemoteCertificateValidationCallback(class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/) /* 060000E2 */
  } // end of property SslServerAuthenticationOptions::RemoteCertificateValidationCallback
  .property /*1700004E*/ instance callconv(8) class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/
          ServerCertificate()
  {
    .get instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_ServerCertificate() /* 060000E3 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_ServerCertificate(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/) /* 060000E4 */
  } // end of property SslServerAuthenticationOptions::ServerCertificate
  .property /*1700004F*/ instance callconv(8) class System.Net.Security.SslStreamCertificateContext/*0200001B*/
          ServerCertificateContext()
  {
    .get instance class System.Net.Security.SslStreamCertificateContext/*0200001B*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_ServerCertificateContext() /* 060000E5 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_ServerCertificateContext(class System.Net.Security.SslStreamCertificateContext/*0200001B*/) /* 060000E6 */
  } // end of property SslServerAuthenticationOptions::ServerCertificateContext
  .property /*17000050*/ instance callconv(8) class System.Net.Security.ServerCertificateSelectionCallback/*02000013*/
          ServerCertificateSelectionCallback()
  {
    .get instance class System.Net.Security.ServerCertificateSelectionCallback/*02000013*/ System.Net.Security.SslServerAuthenticationOptions/*02000019*/::get_ServerCertificateSelectionCallback() /* 060000E7 */
    .set instance void System.Net.Security.SslServerAuthenticationOptions/*02000019*/::set_ServerCertificateSelectionCallback(class System.Net.Security.ServerCertificateSelectionCallback/*02000013*/) /* 060000E8 */
  } // end of property SslServerAuthenticationOptions::ServerCertificateSelectionCallback
} // end of class System.Net.Security.SslServerAuthenticationOptions

.class /*0200001A*/ public auto ansi beforefieldinit System.Net.Security.SslStream
       extends System.Net.Security.AuthenticatedStream/*02000008*/
{
  .custom /*0C000047:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000048:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*060000E9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ innerStream) cil managed
  // SIG: 20 01 01 12 80 91
  {
    // Method begins at RVA 0x230d
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000019       */ call       instance void System.Net.Security.AuthenticatedStream/*02000008*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/,
                                                                                                                                bool) /* 06000019 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method SslStream::.ctor

  .method /*060000EA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ innerStream,
                               bool leaveInnerStreamOpen) cil managed
  // SIG: 20 02 01 12 80 91 02
  {
    // Method begins at RVA 0x2317
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000019       */ call       instance void System.Net.Security.AuthenticatedStream/*02000008*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/,
                                                                                                                                bool) /* 06000019 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method SslStream::.ctor

  .method /*060000EB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ innerStream,
                               bool leaveInnerStreamOpen,
                               class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ userCertificateValidationCallback) cil managed
  // SIG: 20 03 01 12 80 91 02 12 48
  {
    .param [3]/*08000126*/ 
    .custom /*0C0000AA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2321
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000019       */ call       instance void System.Net.Security.AuthenticatedStream/*02000008*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/,
                                                                                                                                bool) /* 06000019 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method SslStream::.ctor

  .method /*060000EC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ innerStream,
                               bool leaveInnerStreamOpen,
                               class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ userCertificateValidationCallback,
                               class System.Net.Security.LocalCertificateSelectionCallback/*0200000B*/ userCertificateSelectionCallback) cil managed
  // SIG: 20 04 01 12 80 91 02 12 48 12 2C
  {
    .custom /*0C00009E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000127*/ 
    .custom /*0C0000AB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x232b
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000019       */ call       instance void System.Net.Security.AuthenticatedStream/*02000008*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/,
                                                                                                                                bool) /* 06000019 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method SslStream::.ctor

  .method /*060000ED*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/ innerStream,
                               bool leaveInnerStreamOpen,
                               class System.Net.Security.RemoteCertificateValidationCallback/*02000012*/ userCertificateValidationCallback,
                               class System.Net.Security.LocalCertificateSelectionCallback/*0200000B*/ userCertificateSelectionCallback,
                               valuetype System.Net.Security.EncryptionPolicy/*0200000A*/ encryptionPolicy) cil managed
  // SIG: 20 05 01 12 80 91 02 12 48 12 2C 11 28
  {
    .custom /*0C0000A0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800012B*/ 
    .custom /*0C0000AC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2335
    // Code size       9 (0x9)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 16   |                  */ ldc.i4.0
    IL_0003:  /* 28   | (06)000019       */ call       instance void System.Net.Security.AuthenticatedStream/*02000008*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*01000024*/,
                                                                                                                                bool) /* 06000019 */
    IL_0008:  /* 2A   |                  */ ret
  } // end of method SslStream::.ctor

  .method /*060000EE*/ public hidebysig specialname virtual 
          instance bool  get_CanRead() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x233f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_CanRead

  .method /*060000EF*/ public hidebysig specialname virtual 
          instance bool  get_CanSeek() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2342
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_CanSeek

  .method /*060000F0*/ public hidebysig specialname virtual 
          instance bool  get_CanTimeout() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2345
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_CanTimeout

  .method /*060000F1*/ public hidebysig specialname virtual 
          instance bool  get_CanWrite() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2348
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_CanWrite

  .method /*060000F2*/ public hidebysig newslot specialname virtual 
          instance bool  get_CheckCertRevocationStatus() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x234b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_CheckCertRevocationStatus

  .method /*060000F3*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.CipherAlgorithmType/*01000046*/ 
          get_CipherAlgorithm() cil managed
  // SIG: 20 00 11 81 19
  {
    // Method begins at RVA 0x234e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_CipherAlgorithm

  .method /*060000F4*/ public hidebysig newslot specialname virtual 
          instance int32  get_CipherStrength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2351
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_CipherStrength

  .method /*060000F5*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.HashAlgorithmType/*01000047*/ 
          get_HashAlgorithm() cil managed
  // SIG: 20 00 11 81 1D
  {
    // Method begins at RVA 0x2354
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_HashAlgorithm

  .method /*060000F6*/ public hidebysig newslot specialname virtual 
          instance int32  get_HashStrength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2357
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_HashStrength

  .method /*060000F7*/ public hidebysig specialname virtual 
          instance bool  get_IsAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x235a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_IsAuthenticated

  .method /*060000F8*/ public hidebysig specialname virtual 
          instance bool  get_IsEncrypted() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x235d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_IsEncrypted

  .method /*060000F9*/ public hidebysig specialname virtual 
          instance bool  get_IsMutuallyAuthenticated() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2360
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_IsMutuallyAuthenticated

  .method /*060000FA*/ public hidebysig specialname virtual 
          instance bool  get_IsServer() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2363
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_IsServer

  .method /*060000FB*/ public hidebysig specialname virtual 
          instance bool  get_IsSigned() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2366
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_IsSigned

  .method /*060000FC*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExchangeAlgorithmType/*01000048*/ 
          get_KeyExchangeAlgorithm() cil managed
  // SIG: 20 00 11 81 21
  {
    // Method begins at RVA 0x2369
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_KeyExchangeAlgorithm

  .method /*060000FD*/ public hidebysig newslot specialname virtual 
          instance int32  get_KeyExchangeStrength() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x236c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_KeyExchangeStrength

  .method /*060000FE*/ public hidebysig specialname virtual 
          instance int64  get_Length() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x236f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_Length

  .method /*060000FF*/ public hidebysig newslot specialname virtual 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ 
          get_LocalCertificate() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C0000A6:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2372
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_LocalCertificate

  .method /*06000100*/ public hidebysig specialname 
          instance valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ 
          get_NegotiatedApplicationProtocol() cil managed
  // SIG: 20 00 11 54
  {
    // Method begins at RVA 0x2375
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_NegotiatedApplicationProtocol

  .method /*06000101*/ public hidebysig newslot specialname virtual 
          instance valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ 
          get_NegotiatedCipherSuite() cil managed
  // SIG: 20 00 11 70
  {
    // Method begins at RVA 0x2378
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_NegotiatedCipherSuite

  .method /*06000102*/ public hidebysig specialname virtual 
          instance int64  get_Position() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x237b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_Position

  .method /*06000103*/ public hidebysig specialname virtual 
          instance void  set_Position(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x237e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::set_Position

  .method /*06000104*/ public hidebysig specialname virtual 
          instance int32  get_ReadTimeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2380
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_ReadTimeout

  .method /*06000105*/ public hidebysig specialname virtual 
          instance void  set_ReadTimeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2383
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::set_ReadTimeout

  .method /*06000106*/ public hidebysig newslot specialname virtual 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ 
          get_RemoteCertificate() cil managed
  // SIG: 20 00 12 80 A1
  {
    .custom /*0C0000A7:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2385
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_RemoteCertificate

  .method /*06000107*/ public hidebysig newslot specialname virtual 
          instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ 
          get_SslProtocol() cil managed
  // SIG: 20 00 11 81 15
  {
    // Method begins at RVA 0x2388
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_SslProtocol

  .method /*06000108*/ public hidebysig specialname 
          instance string  get_TargetHostName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x238b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_TargetHostName

  .method /*06000109*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000049*/ 
          get_TransportContext() cil managed
  // SIG: 20 00 12 81 25
  {
    // Method begins at RVA 0x238e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_TransportContext

  .method /*0600010A*/ public hidebysig specialname virtual 
          instance int32  get_WriteTimeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2391
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::get_WriteTimeout

  .method /*0600010B*/ public hidebysig specialname virtual 
          instance void  set_WriteTimeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2394
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::set_WriteTimeout

  .method /*0600010C*/ public hidebysig instance void 
          AuthenticateAsClient(class System.Net.Security.SslClientAuthenticationOptions/*02000017*/ sslClientAuthenticationOptions) cil managed
  // SIG: 20 01 01 12 5C
  {
    // Method begins at RVA 0x2396
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::AuthenticateAsClient

  .method /*0600010D*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsClient(string targetHost) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2398
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::AuthenticateAsClient

  .method /*0600010E*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsClient(string targetHost,
                                              class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ clientCertificates,
                                              bool checkCertificateRevocation) cil managed
  // SIG: 20 03 01 0E 12 80 A5 02
  {
    .param [2]/*08000136*/ 
    .custom /*0C0000B1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x239a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::AuthenticateAsClient

  .method /*0600010F*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsClient(string targetHost,
                                              class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ clientCertificates,
                                              valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ enabledSslProtocols,
                                              bool checkCertificateRevocation) cil managed
  // SIG: 20 04 01 0E 12 80 A5 11 81 15 02
  {
    .param [2]/*08000139*/ 
    .custom /*0C0000B3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x239c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::AuthenticateAsClient

  .method /*06000110*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync(class System.Net.Security.SslClientAuthenticationOptions/*02000017*/ sslClientAuthenticationOptions,
                                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 CD 12 5C 11 80 D1
  {
    .param [2]/*0800013D*/  = nullref
    // Method begins at RVA 0x239e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsClientAsync

  .method /*06000111*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync(string targetHost) cil managed
  // SIG: 20 01 12 80 CD 0E
  {
    // Method begins at RVA 0x23a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsClientAsync

  .method /*06000112*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync(string targetHost,
                                    class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ clientCertificates,
                                    bool checkCertificateRevocation) cil managed
  // SIG: 20 03 12 80 CD 0E 12 80 A5 02
  {
    .param [2]/*08000140*/ 
    .custom /*0C0000B5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsClientAsync

  .method /*06000113*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsClientAsync(string targetHost,
                                    class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ clientCertificates,
                                    valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ enabledSslProtocols,
                                    bool checkCertificateRevocation) cil managed
  // SIG: 20 04 12 80 CD 0E 12 80 A5 11 81 15 02
  {
    .param [2]/*08000143*/ 
    .custom /*0C0000B6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsClientAsync

  .method /*06000114*/ public hidebysig instance void 
          AuthenticateAsServer(class System.Net.Security.SslServerAuthenticationOptions/*02000019*/ sslServerAuthenticationOptions) cil managed
  // SIG: 20 01 01 12 64
  {
    // Method begins at RVA 0x23aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::AuthenticateAsServer

  .method /*06000115*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsServer(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate) cil managed
  // SIG: 20 01 01 12 80 A1
  {
    // Method begins at RVA 0x23ac
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::AuthenticateAsServer

  .method /*06000116*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsServer(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate,
                                              bool clientCertificateRequired,
                                              bool checkCertificateRevocation) cil managed
  // SIG: 20 03 01 12 80 A1 02 02
  {
    // Method begins at RVA 0x23ae
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::AuthenticateAsServer

  .method /*06000117*/ public hidebysig newslot virtual 
          instance void  AuthenticateAsServer(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate,
                                              bool clientCertificateRequired,
                                              valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ enabledSslProtocols,
                                              bool checkCertificateRevocation) cil managed
  // SIG: 20 04 01 12 80 A1 02 11 81 15 02
  {
    // Method begins at RVA 0x23b0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::AuthenticateAsServer

  .method /*06000118*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync(class System.Net.Security.ServerOptionsSelectionCallback/*02000014*/ optionsCallback,
                                    object state,
                                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 03 12 80 CD 12 50 1C 11 80 D1
  {
    .param [2]/*08000150*/ 
    .custom /*0C0000B7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000151*/  = nullref
    // Method begins at RVA 0x23b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsServerAsync

  .method /*06000119*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync(class System.Net.Security.SslServerAuthenticationOptions/*02000019*/ sslServerAuthenticationOptions,
                                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 CD 12 64 11 80 D1
  {
    .param [2]/*08000153*/  = nullref
    // Method begins at RVA 0x23b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsServerAsync

  .method /*0600011A*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate) cil managed
  // SIG: 20 01 12 80 CD 12 80 A1
  {
    // Method begins at RVA 0x23b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsServerAsync

  .method /*0600011B*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate,
                                    bool clientCertificateRequired,
                                    bool checkCertificateRevocation) cil managed
  // SIG: 20 03 12 80 CD 12 80 A1 02 02
  {
    // Method begins at RVA 0x23bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsServerAsync

  .method /*0600011C*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          AuthenticateAsServerAsync(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate,
                                    bool clientCertificateRequired,
                                    valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ enabledSslProtocols,
                                    bool checkCertificateRevocation) cil managed
  // SIG: 20 04 12 80 CD 12 80 A1 02 11 81 15 02
  {
    // Method begins at RVA 0x23be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::AuthenticateAsServerAsync

  .method /*0600011D*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsClient(string targetHost,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 03 12 80 A9 0E 12 80 AD 1C
  {
    .param [2]/*0800015D*/ 
    .custom /*0C0000B8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800015E*/ 
    .custom /*0C0000B9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::BeginAuthenticateAsClient

  .method /*0600011E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsClient(string targetHost,
                                    class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ clientCertificates,
                                    bool checkCertificateRevocation,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 05 12 80 A9 0E 12 80 A5 02 12 80 AD 1C
  {
    .custom /*0C0000A8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*0800015F*/ 
    .custom /*0C0000BA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000160*/ 
    .custom /*0C0000BB:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::BeginAuthenticateAsClient

  .method /*0600011F*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsClient(string targetHost,
                                    class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ clientCertificates,
                                    valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ enabledSslProtocols,
                                    bool checkCertificateRevocation,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 06 12 80 A9 0E 12 80 A5 11 81 15 02 12 80 AD 1C
  {
    .custom /*0C0000A9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000165*/ 
    .custom /*0C0000BC:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    .param [1]/*08000166*/ 
    .custom /*0C0000BD:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::BeginAuthenticateAsClient

  .method /*06000120*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsServer(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 03 12 80 A9 12 80 A1 12 80 AD 1C
  {
    .param [2]/*0800016D*/ 
    .custom /*0C0000BE:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*0800016E*/ 
    .custom /*0C0000BF:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::BeginAuthenticateAsServer

  .method /*06000121*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsServer(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate,
                                    bool clientCertificateRequired,
                                    bool checkCertificateRevocation,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 05 12 80 A9 12 80 A1 02 02 12 80 AD 1C
  {
    .param [4]/*08000172*/ 
    .custom /*0C0000C0:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000173*/ 
    .custom /*0C0000C1:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::BeginAuthenticateAsServer

  .method /*06000122*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginAuthenticateAsServer(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ serverCertificate,
                                    bool clientCertificateRequired,
                                    valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ enabledSslProtocols,
                                    bool checkCertificateRevocation,
                                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                                    object asyncState) cil managed
  // SIG: 20 06 12 80 A9 12 80 A1 02 11 81 15 02 12 80 AD 1C
  {
    .param [5]/*08000178*/ 
    .custom /*0C0000C2:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [6]/*08000179*/ 
    .custom /*0C0000C3:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23d0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::BeginAuthenticateAsServer

  .method /*06000123*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginRead(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                    object asyncState) cil managed
  // SIG: 20 05 12 80 A9 1D 05 08 08 12 80 AD 1C
  {
    .param [4]/*0800017D*/ 
    .custom /*0C0000C4:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*0800017E*/ 
    .custom /*0C0000C5:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::BeginRead

  .method /*06000124*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ 
          BeginWrite(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     class [System.Runtime/*23000001*/]System.AsyncCallback/*0100002B*/ asyncCallback,
                     object asyncState) cil managed
  // SIG: 20 05 12 80 A9 1D 05 08 08 12 80 AD 1C
  {
    .param [4]/*08000182*/ 
    .custom /*0C0000C6:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000183*/ 
    .custom /*0C0000C7:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::BeginWrite

  .method /*06000125*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x23d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::Dispose

  .method /*06000126*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          DisposeAsync() cil managed
  // SIG: 20 00 11 80 95
  {
    // Method begins at RVA 0x23db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::DisposeAsync

  .method /*06000127*/ public hidebysig newslot virtual 
          instance void  EndAuthenticateAsClient(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ asyncResult) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x23de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::EndAuthenticateAsClient

  .method /*06000128*/ public hidebysig newslot virtual 
          instance void  EndAuthenticateAsServer(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ asyncResult) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x23e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::EndAuthenticateAsServer

  .method /*06000129*/ public hidebysig virtual 
          instance int32  EndRead(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ asyncResult) cil managed
  // SIG: 20 01 08 12 80 A9
  {
    // Method begins at RVA 0x23e2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::EndRead

  .method /*0600012A*/ public hidebysig virtual 
          instance void  EndWrite(class [System.Runtime/*23000001*/]System.IAsyncResult/*0100002A*/ asyncResult) cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x23e5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::EndWrite

  .method /*0600012B*/ family hidebysig virtual 
          instance void  Finalize() cil managed
  // SIG: 20 00 01
  {
    .override [System.Runtime/*23000001*/]System.Object/*0100001D*/::Finalize /*0100001D::0A00001B*/ 
    // Method begins at RVA 0x23e8
    // Code size       10 (0xa)
    .maxstack  1
    .try
    {
      IL_0000:  /* DE   | 07               */ leave.s    IL_0009

    }  // end .try
    finally
    {
      IL_0002:  /* 02   |                  */ ldarg.0
      IL_0003:  /* 28   | (0A)00001B       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::Finalize() /* 0A00001B */
      IL_0008:  /* DC   |                  */ endfinally
    }  // end handler
    // HEX: 02 00 00 00 02 02 00 07 00 00 00 00
    IL_0009:  /* 2A   |                  */ ret
  } // end of method SslStream::Finalize

  .method /*0600012C*/ public hidebysig virtual 
          instance void  Flush() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2410
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::Flush

  .method /*0600012D*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          FlushAsync(valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 CD 11 80 D1
  {
    // Method begins at RVA 0x2412
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::FlushAsync

  .method /*0600012E*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          NegotiateClientCertificateAsync([opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 01 12 80 CD 11 80 D1
  {
    .custom /*0C0000AD:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100004A*/::.ctor(string) /* 0A00001C */ = ( 01 00 07 66 72 65 65 62 73 64 00 00 )             // ...freebsd..
    .custom /*0C0000AE:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100004A*/::.ctor(string) /* 0A00001C */ = ( 01 00 05 6C 69 6E 75 78 00 00 )                   // ...linux..
    .custom /*0C0000AF:0A00001C*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.SupportedOSPlatformAttribute/*0100004A*/::.ctor(string) /* 0A00001C */ = ( 01 00 07 77 69 6E 64 6F 77 73 00 00 )             // ...windows..
    .param [1]/*0800018A*/  = nullref
    // Method begins at RVA 0x2415
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::NegotiateClientCertificateAsync

  .method /*0600012F*/ public hidebysig virtual 
          instance int32  Read(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 08 1D 05 08 08
  {
    // Method begins at RVA 0x2418
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::Read

  .method /*06000130*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task`1/*01000035*/<int32> 
          ReadAsync(uint8[] buffer,
                    int32 offset,
                    int32 count,
                    valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 04 15 12 80 D5 01 08 1D 05 08 08 11 80 D1
  {
    // Method begins at RVA 0x241b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::ReadAsync

  .method /*06000131*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask`1/*01000036*/<int32> 
          ReadAsync(valuetype [System.Runtime/*23000001*/]System.Memory`1/*01000037*/<uint8> buffer,
                    [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 02 15 11 80 D9 01 08 15 11 80 DD 01 05 11 80 D1
  {
    .custom /*0C0000B0:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000193*/  = nullref
    // Method begins at RVA 0x241e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::ReadAsync

  .method /*06000132*/ public hidebysig virtual 
          instance int32  ReadByte() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2421
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::ReadByte

  .method /*06000133*/ public hidebysig virtual 
          instance int64  Seek(int64 offset,
                               valuetype [System.Runtime/*23000001*/]System.IO.SeekOrigin/*01000038*/ origin) cil managed
  // SIG: 20 02 0A 0A 11 80 E1
  {
    // Method begins at RVA 0x2424
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::Seek

  .method /*06000134*/ public hidebysig virtual 
          instance void  SetLength(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x2427
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::SetLength

  .method /*06000135*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          ShutdownAsync() cil managed
  // SIG: 20 00 12 80 CD
  {
    // Method begins at RVA 0x2429
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::ShutdownAsync

  .method /*06000136*/ public hidebysig instance void 
          Write(uint8[] buffer) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x242c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::Write

  .method /*06000137*/ public hidebysig virtual 
          instance void  Write(uint8[] buffer,
                               int32 offset,
                               int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x242e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SslStream::Write

  .method /*06000138*/ public hidebysig virtual 
          instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*01000033*/ 
          WriteAsync(uint8[] buffer,
                     int32 offset,
                     int32 count,
                     valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 04 12 80 CD 1D 05 08 08 11 80 D1
  {
    // Method begins at RVA 0x2430
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::WriteAsync

  .method /*06000139*/ public hidebysig virtual 
          instance valuetype [System.Runtime/*23000001*/]System.Threading.Tasks.ValueTask/*01000025*/ 
          WriteAsync(valuetype [System.Runtime/*23000001*/]System.ReadOnlyMemory`1/*01000039*/<uint8> buffer,
                     [opt] valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*01000034*/ cancellationToken) cil managed
  // SIG: 20 02 11 80 95 15 11 80 E5 01 05 11 80 D1
  {
    .custom /*0C0000B2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 00 00 00 ) 
    .param [2]/*080001A0*/  = nullref
    // Method begins at RVA 0x2433
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStream::WriteAsync

  .property /*17000051*/ instance callconv(8) bool
          CanRead()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_CanRead() /* 060000EE */
  } // end of property SslStream::CanRead
  .property /*17000052*/ instance callconv(8) bool
          CanSeek()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_CanSeek() /* 060000EF */
  } // end of property SslStream::CanSeek
  .property /*17000053*/ instance callconv(8) bool
          CanTimeout()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_CanTimeout() /* 060000F0 */
  } // end of property SslStream::CanTimeout
  .property /*17000054*/ instance callconv(8) bool
          CanWrite()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_CanWrite() /* 060000F1 */
  } // end of property SslStream::CanWrite
  .property /*17000055*/ instance callconv(8) bool
          CheckCertRevocationStatus()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_CheckCertRevocationStatus() /* 060000F2 */
  } // end of property SslStream::CheckCertRevocationStatus
  .property /*17000056*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.CipherAlgorithmType/*01000046*/
          CipherAlgorithm()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.CipherAlgorithmType/*01000046*/ System.Net.Security.SslStream/*0200001A*/::get_CipherAlgorithm() /* 060000F3 */
  } // end of property SslStream::CipherAlgorithm
  .property /*17000057*/ instance callconv(8) int32
          CipherStrength()
  {
    .get instance int32 System.Net.Security.SslStream/*0200001A*/::get_CipherStrength() /* 060000F4 */
  } // end of property SslStream::CipherStrength
  .property /*17000058*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.HashAlgorithmType/*01000047*/
          HashAlgorithm()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.HashAlgorithmType/*01000047*/ System.Net.Security.SslStream/*0200001A*/::get_HashAlgorithm() /* 060000F5 */
  } // end of property SslStream::HashAlgorithm
  .property /*17000059*/ instance callconv(8) int32
          HashStrength()
  {
    .get instance int32 System.Net.Security.SslStream/*0200001A*/::get_HashStrength() /* 060000F6 */
  } // end of property SslStream::HashStrength
  .property /*1700005A*/ instance callconv(8) bool
          IsAuthenticated()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_IsAuthenticated() /* 060000F7 */
  } // end of property SslStream::IsAuthenticated
  .property /*1700005B*/ instance callconv(8) bool
          IsEncrypted()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_IsEncrypted() /* 060000F8 */
  } // end of property SslStream::IsEncrypted
  .property /*1700005C*/ instance callconv(8) bool
          IsMutuallyAuthenticated()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_IsMutuallyAuthenticated() /* 060000F9 */
  } // end of property SslStream::IsMutuallyAuthenticated
  .property /*1700005D*/ instance callconv(8) bool
          IsServer()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_IsServer() /* 060000FA */
  } // end of property SslStream::IsServer
  .property /*1700005E*/ instance callconv(8) bool
          IsSigned()
  {
    .get instance bool System.Net.Security.SslStream/*0200001A*/::get_IsSigned() /* 060000FB */
  } // end of property SslStream::IsSigned
  .property /*1700005F*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExchangeAlgorithmType/*01000048*/
          KeyExchangeAlgorithm()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.ExchangeAlgorithmType/*01000048*/ System.Net.Security.SslStream/*0200001A*/::get_KeyExchangeAlgorithm() /* 060000FC */
  } // end of property SslStream::KeyExchangeAlgorithm
  .property /*17000060*/ instance callconv(8) int32
          KeyExchangeStrength()
  {
    .get instance int32 System.Net.Security.SslStream/*0200001A*/::get_KeyExchangeStrength() /* 060000FD */
  } // end of property SslStream::KeyExchangeStrength
  .property /*17000061*/ instance callconv(8) int64
          Length()
  {
    .get instance int64 System.Net.Security.SslStream/*0200001A*/::get_Length() /* 060000FE */
  } // end of property SslStream::Length
  .property /*17000062*/ instance callconv(8) class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/
          LocalCertificate()
  {
    .custom /*0C00006B:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ System.Net.Security.SslStream/*0200001A*/::get_LocalCertificate() /* 060000FF */
  } // end of property SslStream::LocalCertificate
  .property /*17000063*/ instance callconv(8) valuetype System.Net.Security.SslApplicationProtocol/*02000015*/
          NegotiatedApplicationProtocol()
  {
    .get instance valuetype System.Net.Security.SslApplicationProtocol/*02000015*/ System.Net.Security.SslStream/*0200001A*/::get_NegotiatedApplicationProtocol() /* 06000100 */
  } // end of property SslStream::NegotiatedApplicationProtocol
  .property /*17000064*/ instance callconv(8) valuetype System.Net.Security.TlsCipherSuite/*0200001C*/
          NegotiatedCipherSuite()
  {
    .custom /*0C00006C:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
    .get instance valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ System.Net.Security.SslStream/*0200001A*/::get_NegotiatedCipherSuite() /* 06000101 */
  } // end of property SslStream::NegotiatedCipherSuite
  .property /*17000065*/ instance callconv(8) int64
          Position()
  {
    .get instance int64 System.Net.Security.SslStream/*0200001A*/::get_Position() /* 06000102 */
    .set instance void System.Net.Security.SslStream/*0200001A*/::set_Position(int64) /* 06000103 */
  } // end of property SslStream::Position
  .property /*17000066*/ instance callconv(8) int32
          ReadTimeout()
  {
    .get instance int32 System.Net.Security.SslStream/*0200001A*/::get_ReadTimeout() /* 06000104 */
    .set instance void System.Net.Security.SslStream/*0200001A*/::set_ReadTimeout(int32) /* 06000105 */
  } // end of property SslStream::ReadTimeout
  .property /*17000067*/ instance callconv(8) class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/
          RemoteCertificate()
  {
    .custom /*0C00006E:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate/*01000028*/ System.Net.Security.SslStream/*0200001A*/::get_RemoteCertificate() /* 06000106 */
  } // end of property SslStream::RemoteCertificate
  .property /*17000068*/ instance callconv(8) valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/
          SslProtocol()
  {
    .get instance valuetype [System.Net.Primitives/*23000002*/]System.Security.Authentication.SslProtocols/*01000045*/ System.Net.Security.SslStream/*0200001A*/::get_SslProtocol() /* 06000107 */
  } // end of property SslStream::SslProtocol
  .property /*17000069*/ instance callconv(8) string
          TargetHostName()
  {
    .get instance string System.Net.Security.SslStream/*0200001A*/::get_TargetHostName() /* 06000108 */
  } // end of property SslStream::TargetHostName
  .property /*1700006A*/ instance callconv(8) class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000049*/
          TransportContext()
  {
    .get instance class [System.Net.Primitives/*23000002*/]System.Net.TransportContext/*01000049*/ System.Net.Security.SslStream/*0200001A*/::get_TransportContext() /* 06000109 */
  } // end of property SslStream::TransportContext
  .property /*1700006B*/ instance callconv(8) int32
          WriteTimeout()
  {
    .get instance int32 System.Net.Security.SslStream/*0200001A*/::get_WriteTimeout() /* 0600010A */
    .set instance void System.Net.Security.SslStream/*0200001A*/::set_WriteTimeout(int32) /* 0600010B */
  } // end of property SslStream::WriteTimeout
} // end of class System.Net.Security.SslStream

.class /*0200001B*/ public auto ansi beforefieldinit System.Net.Security.SslStreamCertificateContext
       extends [System.Runtime/*23000001*/]System.Object/*0100001D*/
{
  .custom /*0C000049:0A000013*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableContextAttribute/*01000014*/::.ctor(uint8) /* 0A000013 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004A:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 00 00 00 ) 
  .method /*0600013A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2436
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void [System.Runtime/*23000001*/]System.Object/*0100001D*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SslStreamCertificateContext::.ctor

  .method /*0600013B*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100004B*/<class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100004C*/> 
          get_IntermediateCertificates() cil managed
  // SIG: 20 00 15 12 81 2D 01 12 81 31
  {
    // Method begins at RVA 0x243e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStreamCertificateContext::get_IntermediateCertificates

  .method /*0600013C*/ public hidebysig specialname 
          instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100004C*/ 
          get_TargetCertificate() cil managed
  // SIG: 20 00 12 81 31
  {
    // Method begins at RVA 0x2441
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStreamCertificateContext::get_TargetCertificate

  .method /*0600013D*/ public hidebysig static 
          class System.Net.Security.SslStreamCertificateContext/*0200001B*/ 
          Create(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100004C*/ target,
                 class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2Collection/*01000040*/ additionalCertificates,
                 bool offline) cil managed
  // SIG: 00 03 12 6C 12 81 31 12 81 01 02
  {
    .custom /*0C0000B4:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*01000018*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*01000017*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .param [2]/*080001A2*/ 
    .custom /*0C0000C8:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2444
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStreamCertificateContext::Create

  .method /*0600013E*/ public hidebysig static 
          class System.Net.Security.SslStreamCertificateContext/*0200001B*/ 
          Create(class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100004C*/ target,
                 class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2Collection/*01000040*/ additionalCertificates,
                 [opt] bool offline,
                 [opt] class System.Net.Security.SslCertificateTrust/*02000016*/ trust) cil managed
  // SIG: 00 04 12 6C 12 81 31 12 81 01 02 12 58
  {
    .param [2]/*080001A5*/ 
    .custom /*0C0000C9:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    .param [3]/*080001A6*/  = bool(false)
    .param [4]/*080001A7*/  = nullref
    .custom /*0C0000CA:0A000014*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.NullableAttribute/*01000015*/::.ctor(uint8) /* 0A000014 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2447
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SslStreamCertificateContext::Create

  .property /*1700006C*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100004B*/<class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100004C*/>
          IntermediateCertificates()
  {
    .get instance class [System.Runtime/*23000001*/]System.Collections.ObjectModel.ReadOnlyCollection`1/*0100004B*/<class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100004C*/> System.Net.Security.SslStreamCertificateContext/*0200001B*/::get_IntermediateCertificates() /* 0600013B */
  } // end of property SslStreamCertificateContext::IntermediateCertificates
  .property /*1700006D*/ instance callconv(8) class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100004C*/
          TargetCertificate()
  {
    .get instance class [System.Security.Cryptography/*23000004*/]System.Security.Cryptography.X509Certificates.X509Certificate2/*0100004C*/ System.Net.Security.SslStreamCertificateContext/*0200001B*/::get_TargetCertificate() /* 0600013C */
  } // end of property SslStreamCertificateContext::TargetCertificate
} // end of class System.Net.Security.SslStreamCertificateContext

.class /*0200001C*/ public auto ansi sealed System.Net.Security.TlsCipherSuite
       extends [System.Runtime/*23000001*/]System.Enum/*01000021*/
{
  .custom /*0C00004B:0A000008*/ instance void [System.Runtime/*23000001*/]System.CLSCompliantAttribute/*01000009*/::.ctor(bool) /* 0A000008 */ = ( 01 00 00 00 00 ) 
  .field /*04000028*/ public specialname rtspecialname uint16 value__
  .field /*04000029*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_NULL_WITH_NULL_NULL = uint16(0x0000)
  .field /*0400002A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_NULL_MD5 = uint16(0x0001)
  .field /*0400002B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_NULL_SHA = uint16(0x0002)
  .field /*0400002C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_EXPORT_WITH_RC4_40_MD5 = uint16(0x0003)
  .field /*0400002D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_RC4_128_MD5 = uint16(0x0004)
  .field /*0400002E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_RC4_128_SHA = uint16(0x0005)
  .field /*0400002F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_EXPORT_WITH_RC2_CBC_40_MD5 = uint16(0x0006)
  .field /*04000030*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_IDEA_CBC_SHA = uint16(0x0007)
  .field /*04000031*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_EXPORT_WITH_DES40_CBC_SHA = uint16(0x0008)
  .field /*04000032*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_DES_CBC_SHA = uint16(0x0009)
  .field /*04000033*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_3DES_EDE_CBC_SHA = uint16(0x000A)
  .field /*04000034*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_EXPORT_WITH_DES40_CBC_SHA = uint16(0x000B)
  .field /*04000035*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_DES_CBC_SHA = uint16(0x000C)
  .field /*04000036*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_3DES_EDE_CBC_SHA = uint16(0x000D)
  .field /*04000037*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_EXPORT_WITH_DES40_CBC_SHA = uint16(0x000E)
  .field /*04000038*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_DES_CBC_SHA = uint16(0x000F)
  .field /*04000039*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_3DES_EDE_CBC_SHA = uint16(0x0010)
  .field /*0400003A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_EXPORT_WITH_DES40_CBC_SHA = uint16(0x0011)
  .field /*0400003B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_DES_CBC_SHA = uint16(0x0012)
  .field /*0400003C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_3DES_EDE_CBC_SHA = uint16(0x0013)
  .field /*0400003D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_EXPORT_WITH_DES40_CBC_SHA = uint16(0x0014)
  .field /*0400003E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_DES_CBC_SHA = uint16(0x0015)
  .field /*0400003F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_3DES_EDE_CBC_SHA = uint16(0x0016)
  .field /*04000040*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_EXPORT_WITH_RC4_40_MD5 = uint16(0x0017)
  .field /*04000041*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_RC4_128_MD5 = uint16(0x0018)
  .field /*04000042*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_EXPORT_WITH_DES40_CBC_SHA = uint16(0x0019)
  .field /*04000043*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_DES_CBC_SHA = uint16(0x001A)
  .field /*04000044*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_3DES_EDE_CBC_SHA = uint16(0x001B)
  .field /*04000045*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_WITH_DES_CBC_SHA = uint16(0x001E)
  .field /*04000046*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_WITH_3DES_EDE_CBC_SHA = uint16(0x001F)
  .field /*04000047*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_WITH_RC4_128_SHA = uint16(0x0020)
  .field /*04000048*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_WITH_IDEA_CBC_SHA = uint16(0x0021)
  .field /*04000049*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_WITH_DES_CBC_MD5 = uint16(0x0022)
  .field /*0400004A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_WITH_3DES_EDE_CBC_MD5 = uint16(0x0023)
  .field /*0400004B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_WITH_RC4_128_MD5 = uint16(0x0024)
  .field /*0400004C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_WITH_IDEA_CBC_MD5 = uint16(0x0025)
  .field /*0400004D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_EXPORT_WITH_DES_CBC_40_SHA = uint16(0x0026)
  .field /*0400004E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_EXPORT_WITH_RC2_CBC_40_SHA = uint16(0x0027)
  .field /*0400004F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_EXPORT_WITH_RC4_40_SHA = uint16(0x0028)
  .field /*04000050*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_EXPORT_WITH_DES_CBC_40_MD5 = uint16(0x0029)
  .field /*04000051*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_EXPORT_WITH_RC2_CBC_40_MD5 = uint16(0x002A)
  .field /*04000052*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_KRB5_EXPORT_WITH_RC4_40_MD5 = uint16(0x002B)
  .field /*04000053*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_NULL_SHA = uint16(0x002C)
  .field /*04000054*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_NULL_SHA = uint16(0x002D)
  .field /*04000055*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_NULL_SHA = uint16(0x002E)
  .field /*04000056*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_128_CBC_SHA = uint16(0x002F)
  .field /*04000057*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_AES_128_CBC_SHA = uint16(0x0030)
  .field /*04000058*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_AES_128_CBC_SHA = uint16(0x0031)
  .field /*04000059*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_AES_128_CBC_SHA = uint16(0x0032)
  .field /*0400005A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_128_CBC_SHA = uint16(0x0033)
  .field /*0400005B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_AES_128_CBC_SHA = uint16(0x0034)
  .field /*0400005C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_256_CBC_SHA = uint16(0x0035)
  .field /*0400005D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_AES_256_CBC_SHA = uint16(0x0036)
  .field /*0400005E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_AES_256_CBC_SHA = uint16(0x0037)
  .field /*0400005F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_AES_256_CBC_SHA = uint16(0x0038)
  .field /*04000060*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_256_CBC_SHA = uint16(0x0039)
  .field /*04000061*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_AES_256_CBC_SHA = uint16(0x003A)
  .field /*04000062*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_NULL_SHA256 = uint16(0x003B)
  .field /*04000063*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_128_CBC_SHA256 = uint16(0x003C)
  .field /*04000064*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_256_CBC_SHA256 = uint16(0x003D)
  .field /*04000065*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_AES_128_CBC_SHA256 = uint16(0x003E)
  .field /*04000066*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_AES_128_CBC_SHA256 = uint16(0x003F)
  .field /*04000067*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_AES_128_CBC_SHA256 = uint16(0x0040)
  .field /*04000068*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_CAMELLIA_128_CBC_SHA = uint16(0x0041)
  .field /*04000069*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_CAMELLIA_128_CBC_SHA = uint16(0x0042)
  .field /*0400006A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_CAMELLIA_128_CBC_SHA = uint16(0x0043)
  .field /*0400006B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_CAMELLIA_128_CBC_SHA = uint16(0x0044)
  .field /*0400006C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_CAMELLIA_128_CBC_SHA = uint16(0x0045)
  .field /*0400006D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_CAMELLIA_128_CBC_SHA = uint16(0x0046)
  .field /*0400006E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_128_CBC_SHA256 = uint16(0x0067)
  .field /*0400006F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_AES_256_CBC_SHA256 = uint16(0x0068)
  .field /*04000070*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_AES_256_CBC_SHA256 = uint16(0x0069)
  .field /*04000071*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_AES_256_CBC_SHA256 = uint16(0x006A)
  .field /*04000072*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_256_CBC_SHA256 = uint16(0x006B)
  .field /*04000073*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_AES_128_CBC_SHA256 = uint16(0x006C)
  .field /*04000074*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_AES_256_CBC_SHA256 = uint16(0x006D)
  .field /*04000075*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_CAMELLIA_256_CBC_SHA = uint16(0x0084)
  .field /*04000076*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_CAMELLIA_256_CBC_SHA = uint16(0x0085)
  .field /*04000077*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_CAMELLIA_256_CBC_SHA = uint16(0x0086)
  .field /*04000078*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_CAMELLIA_256_CBC_SHA = uint16(0x0087)
  .field /*04000079*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_CAMELLIA_256_CBC_SHA = uint16(0x0088)
  .field /*0400007A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_CAMELLIA_256_CBC_SHA = uint16(0x0089)
  .field /*0400007B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_RC4_128_SHA = uint16(0x008A)
  .field /*0400007C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_3DES_EDE_CBC_SHA = uint16(0x008B)
  .field /*0400007D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_128_CBC_SHA = uint16(0x008C)
  .field /*0400007E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_256_CBC_SHA = uint16(0x008D)
  .field /*0400007F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_RC4_128_SHA = uint16(0x008E)
  .field /*04000080*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_3DES_EDE_CBC_SHA = uint16(0x008F)
  .field /*04000081*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_AES_128_CBC_SHA = uint16(0x0090)
  .field /*04000082*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_AES_256_CBC_SHA = uint16(0x0091)
  .field /*04000083*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_RC4_128_SHA = uint16(0x0092)
  .field /*04000084*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_3DES_EDE_CBC_SHA = uint16(0x0093)
  .field /*04000085*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_AES_128_CBC_SHA = uint16(0x0094)
  .field /*04000086*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_AES_256_CBC_SHA = uint16(0x0095)
  .field /*04000087*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_SEED_CBC_SHA = uint16(0x0096)
  .field /*04000088*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_SEED_CBC_SHA = uint16(0x0097)
  .field /*04000089*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_SEED_CBC_SHA = uint16(0x0098)
  .field /*0400008A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_SEED_CBC_SHA = uint16(0x0099)
  .field /*0400008B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_SEED_CBC_SHA = uint16(0x009A)
  .field /*0400008C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_SEED_CBC_SHA = uint16(0x009B)
  .field /*0400008D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_128_GCM_SHA256 = uint16(0x009C)
  .field /*0400008E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_256_GCM_SHA384 = uint16(0x009D)
  .field /*0400008F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_128_GCM_SHA256 = uint16(0x009E)
  .field /*04000090*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_256_GCM_SHA384 = uint16(0x009F)
  .field /*04000091*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_AES_128_GCM_SHA256 = uint16(0x00A0)
  .field /*04000092*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_AES_256_GCM_SHA384 = uint16(0x00A1)
  .field /*04000093*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_AES_128_GCM_SHA256 = uint16(0x00A2)
  .field /*04000094*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_AES_256_GCM_SHA384 = uint16(0x00A3)
  .field /*04000095*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_AES_128_GCM_SHA256 = uint16(0x00A4)
  .field /*04000096*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_AES_256_GCM_SHA384 = uint16(0x00A5)
  .field /*04000097*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_AES_128_GCM_SHA256 = uint16(0x00A6)
  .field /*04000098*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_AES_256_GCM_SHA384 = uint16(0x00A7)
  .field /*04000099*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_128_GCM_SHA256 = uint16(0x00A8)
  .field /*0400009A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_256_GCM_SHA384 = uint16(0x00A9)
  .field /*0400009B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_AES_128_GCM_SHA256 = uint16(0x00AA)
  .field /*0400009C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_AES_256_GCM_SHA384 = uint16(0x00AB)
  .field /*0400009D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_AES_128_GCM_SHA256 = uint16(0x00AC)
  .field /*0400009E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_AES_256_GCM_SHA384 = uint16(0x00AD)
  .field /*0400009F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_128_CBC_SHA256 = uint16(0x00AE)
  .field /*040000A0*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_256_CBC_SHA384 = uint16(0x00AF)
  .field /*040000A1*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_NULL_SHA256 = uint16(0x00B0)
  .field /*040000A2*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_NULL_SHA384 = uint16(0x00B1)
  .field /*040000A3*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_AES_128_CBC_SHA256 = uint16(0x00B2)
  .field /*040000A4*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_AES_256_CBC_SHA384 = uint16(0x00B3)
  .field /*040000A5*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_NULL_SHA256 = uint16(0x00B4)
  .field /*040000A6*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_NULL_SHA384 = uint16(0x00B5)
  .field /*040000A7*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_AES_128_CBC_SHA256 = uint16(0x00B6)
  .field /*040000A8*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_AES_256_CBC_SHA384 = uint16(0x00B7)
  .field /*040000A9*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_NULL_SHA256 = uint16(0x00B8)
  .field /*040000AA*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_NULL_SHA384 = uint16(0x00B9)
  .field /*040000AB*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0x00BA)
  .field /*040000AC*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0x00BB)
  .field /*040000AD*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0x00BC)
  .field /*040000AE*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0x00BD)
  .field /*040000AF*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0x00BE)
  .field /*040000B0*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0x00BF)
  .field /*040000B1*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_CAMELLIA_256_CBC_SHA256 = uint16(0x00C0)
  .field /*040000B2*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_CAMELLIA_256_CBC_SHA256 = uint16(0x00C1)
  .field /*040000B3*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_CAMELLIA_256_CBC_SHA256 = uint16(0x00C2)
  .field /*040000B4*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_CAMELLIA_256_CBC_SHA256 = uint16(0x00C3)
  .field /*040000B5*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_CAMELLIA_256_CBC_SHA256 = uint16(0x00C4)
  .field /*040000B6*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_CAMELLIA_256_CBC_SHA256 = uint16(0x00C5)
  .field /*040000B7*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_AES_128_GCM_SHA256 = uint16(0x1301)
  .field /*040000B8*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_AES_256_GCM_SHA384 = uint16(0x1302)
  .field /*040000B9*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_CHACHA20_POLY1305_SHA256 = uint16(0x1303)
  .field /*040000BA*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_AES_128_CCM_SHA256 = uint16(0x1304)
  .field /*040000BB*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_AES_128_CCM_8_SHA256 = uint16(0x1305)
  .field /*040000BC*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_NULL_SHA = uint16(0xC001)
  .field /*040000BD*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_RC4_128_SHA = uint16(0xC002)
  .field /*040000BE*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_3DES_EDE_CBC_SHA = uint16(0xC003)
  .field /*040000BF*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA = uint16(0xC004)
  .field /*040000C0*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA = uint16(0xC005)
  .field /*040000C1*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_NULL_SHA = uint16(0xC006)
  .field /*040000C2*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_RC4_128_SHA = uint16(0xC007)
  .field /*040000C3*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA = uint16(0xC008)
  .field /*040000C4*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA = uint16(0xC009)
  .field /*040000C5*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA = uint16(0xC00A)
  .field /*040000C6*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_NULL_SHA = uint16(0xC00B)
  .field /*040000C7*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_RC4_128_SHA = uint16(0xC00C)
  .field /*040000C8*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_3DES_EDE_CBC_SHA = uint16(0xC00D)
  .field /*040000C9*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_AES_128_CBC_SHA = uint16(0xC00E)
  .field /*040000CA*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_AES_256_CBC_SHA = uint16(0xC00F)
  .field /*040000CB*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_NULL_SHA = uint16(0xC010)
  .field /*040000CC*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_RC4_128_SHA = uint16(0xC011)
  .field /*040000CD*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_3DES_EDE_CBC_SHA = uint16(0xC012)
  .field /*040000CE*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA = uint16(0xC013)
  .field /*040000CF*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA = uint16(0xC014)
  .field /*040000D0*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_anon_WITH_NULL_SHA = uint16(0xC015)
  .field /*040000D1*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_anon_WITH_RC4_128_SHA = uint16(0xC016)
  .field /*040000D2*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_anon_WITH_3DES_EDE_CBC_SHA = uint16(0xC017)
  .field /*040000D3*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_anon_WITH_AES_128_CBC_SHA = uint16(0xC018)
  .field /*040000D4*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_anon_WITH_AES_256_CBC_SHA = uint16(0xC019)
  .field /*040000D5*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_WITH_3DES_EDE_CBC_SHA = uint16(0xC01A)
  .field /*040000D6*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_RSA_WITH_3DES_EDE_CBC_SHA = uint16(0xC01B)
  .field /*040000D7*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_DSS_WITH_3DES_EDE_CBC_SHA = uint16(0xC01C)
  .field /*040000D8*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_WITH_AES_128_CBC_SHA = uint16(0xC01D)
  .field /*040000D9*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_RSA_WITH_AES_128_CBC_SHA = uint16(0xC01E)
  .field /*040000DA*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_DSS_WITH_AES_128_CBC_SHA = uint16(0xC01F)
  .field /*040000DB*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_WITH_AES_256_CBC_SHA = uint16(0xC020)
  .field /*040000DC*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_RSA_WITH_AES_256_CBC_SHA = uint16(0xC021)
  .field /*040000DD*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_SRP_SHA_DSS_WITH_AES_256_CBC_SHA = uint16(0xC022)
  .field /*040000DE*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 = uint16(0xC023)
  .field /*040000DF*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 = uint16(0xC024)
  .field /*040000E0*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA256 = uint16(0xC025)
  .field /*040000E1*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA384 = uint16(0xC026)
  .field /*040000E2*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA256 = uint16(0xC027)
  .field /*040000E3*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA384 = uint16(0xC028)
  .field /*040000E4*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_AES_128_CBC_SHA256 = uint16(0xC029)
  .field /*040000E5*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_AES_256_CBC_SHA384 = uint16(0xC02A)
  .field /*040000E6*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 = uint16(0xC02B)
  .field /*040000E7*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 = uint16(0xC02C)
  .field /*040000E8*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_AES_128_GCM_SHA256 = uint16(0xC02D)
  .field /*040000E9*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_AES_256_GCM_SHA384 = uint16(0xC02E)
  .field /*040000EA*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256 = uint16(0xC02F)
  .field /*040000EB*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384 = uint16(0xC030)
  .field /*040000EC*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_AES_128_GCM_SHA256 = uint16(0xC031)
  .field /*040000ED*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_AES_256_GCM_SHA384 = uint16(0xC032)
  .field /*040000EE*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_RC4_128_SHA = uint16(0xC033)
  .field /*040000EF*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_3DES_EDE_CBC_SHA = uint16(0xC034)
  .field /*040000F0*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_AES_128_CBC_SHA = uint16(0xC035)
  .field /*040000F1*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_AES_256_CBC_SHA = uint16(0xC036)
  .field /*040000F2*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_AES_128_CBC_SHA256 = uint16(0xC037)
  .field /*040000F3*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_AES_256_CBC_SHA384 = uint16(0xC038)
  .field /*040000F4*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_NULL_SHA = uint16(0xC039)
  .field /*040000F5*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_NULL_SHA256 = uint16(0xC03A)
  .field /*040000F6*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_NULL_SHA384 = uint16(0xC03B)
  .field /*040000F7*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_ARIA_128_CBC_SHA256 = uint16(0xC03C)
  .field /*040000F8*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_ARIA_256_CBC_SHA384 = uint16(0xC03D)
  .field /*040000F9*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_ARIA_128_CBC_SHA256 = uint16(0xC03E)
  .field /*040000FA*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_ARIA_256_CBC_SHA384 = uint16(0xC03F)
  .field /*040000FB*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_ARIA_128_CBC_SHA256 = uint16(0xC040)
  .field /*040000FC*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_ARIA_256_CBC_SHA384 = uint16(0xC041)
  .field /*040000FD*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_ARIA_128_CBC_SHA256 = uint16(0xC042)
  .field /*040000FE*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_ARIA_256_CBC_SHA384 = uint16(0xC043)
  .field /*040000FF*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_ARIA_128_CBC_SHA256 = uint16(0xC044)
  .field /*04000100*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_ARIA_256_CBC_SHA384 = uint16(0xC045)
  .field /*04000101*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_ARIA_128_CBC_SHA256 = uint16(0xC046)
  .field /*04000102*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_ARIA_256_CBC_SHA384 = uint16(0xC047)
  .field /*04000103*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_ARIA_128_CBC_SHA256 = uint16(0xC048)
  .field /*04000104*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_ARIA_256_CBC_SHA384 = uint16(0xC049)
  .field /*04000105*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_ARIA_128_CBC_SHA256 = uint16(0xC04A)
  .field /*04000106*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_ARIA_256_CBC_SHA384 = uint16(0xC04B)
  .field /*04000107*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_ARIA_128_CBC_SHA256 = uint16(0xC04C)
  .field /*04000108*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_ARIA_256_CBC_SHA384 = uint16(0xC04D)
  .field /*04000109*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_ARIA_128_CBC_SHA256 = uint16(0xC04E)
  .field /*0400010A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_ARIA_256_CBC_SHA384 = uint16(0xC04F)
  .field /*0400010B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_ARIA_128_GCM_SHA256 = uint16(0xC050)
  .field /*0400010C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_ARIA_256_GCM_SHA384 = uint16(0xC051)
  .field /*0400010D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_ARIA_128_GCM_SHA256 = uint16(0xC052)
  .field /*0400010E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_ARIA_256_GCM_SHA384 = uint16(0xC053)
  .field /*0400010F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_ARIA_128_GCM_SHA256 = uint16(0xC054)
  .field /*04000110*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_ARIA_256_GCM_SHA384 = uint16(0xC055)
  .field /*04000111*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_ARIA_128_GCM_SHA256 = uint16(0xC056)
  .field /*04000112*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_ARIA_256_GCM_SHA384 = uint16(0xC057)
  .field /*04000113*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_ARIA_128_GCM_SHA256 = uint16(0xC058)
  .field /*04000114*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_ARIA_256_GCM_SHA384 = uint16(0xC059)
  .field /*04000115*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_ARIA_128_GCM_SHA256 = uint16(0xC05A)
  .field /*04000116*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_ARIA_256_GCM_SHA384 = uint16(0xC05B)
  .field /*04000117*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_ARIA_128_GCM_SHA256 = uint16(0xC05C)
  .field /*04000118*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_ARIA_256_GCM_SHA384 = uint16(0xC05D)
  .field /*04000119*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_ARIA_128_GCM_SHA256 = uint16(0xC05E)
  .field /*0400011A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_ARIA_256_GCM_SHA384 = uint16(0xC05F)
  .field /*0400011B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_ARIA_128_GCM_SHA256 = uint16(0xC060)
  .field /*0400011C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_ARIA_256_GCM_SHA384 = uint16(0xC061)
  .field /*0400011D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_ARIA_128_GCM_SHA256 = uint16(0xC062)
  .field /*0400011E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_ARIA_256_GCM_SHA384 = uint16(0xC063)
  .field /*0400011F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_ARIA_128_CBC_SHA256 = uint16(0xC064)
  .field /*04000120*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_ARIA_256_CBC_SHA384 = uint16(0xC065)
  .field /*04000121*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_ARIA_128_CBC_SHA256 = uint16(0xC066)
  .field /*04000122*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_ARIA_256_CBC_SHA384 = uint16(0xC067)
  .field /*04000123*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_ARIA_128_CBC_SHA256 = uint16(0xC068)
  .field /*04000124*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_ARIA_256_CBC_SHA384 = uint16(0xC069)
  .field /*04000125*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_ARIA_128_GCM_SHA256 = uint16(0xC06A)
  .field /*04000126*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_ARIA_256_GCM_SHA384 = uint16(0xC06B)
  .field /*04000127*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_ARIA_128_GCM_SHA256 = uint16(0xC06C)
  .field /*04000128*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_ARIA_256_GCM_SHA384 = uint16(0xC06D)
  .field /*04000129*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_ARIA_128_GCM_SHA256 = uint16(0xC06E)
  .field /*0400012A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_ARIA_256_GCM_SHA384 = uint16(0xC06F)
  .field /*0400012B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_ARIA_128_CBC_SHA256 = uint16(0xC070)
  .field /*0400012C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_ARIA_256_CBC_SHA384 = uint16(0xC071)
  .field /*0400012D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0xC072)
  .field /*0400012E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_CAMELLIA_256_CBC_SHA384 = uint16(0xC073)
  .field /*0400012F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0xC074)
  .field /*04000130*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_CAMELLIA_256_CBC_SHA384 = uint16(0xC075)
  .field /*04000131*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0xC076)
  .field /*04000132*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_CAMELLIA_256_CBC_SHA384 = uint16(0xC077)
  .field /*04000133*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0xC078)
  .field /*04000134*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_CAMELLIA_256_CBC_SHA384 = uint16(0xC079)
  .field /*04000135*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC07A)
  .field /*04000136*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC07B)
  .field /*04000137*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC07C)
  .field /*04000138*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC07D)
  .field /*04000139*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC07E)
  .field /*0400013A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_RSA_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC07F)
  .field /*0400013B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC080)
  .field /*0400013C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_DSS_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC081)
  .field /*0400013D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC082)
  .field /*0400013E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_DSS_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC083)
  .field /*0400013F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC084)
  .field /*04000140*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DH_anon_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC085)
  .field /*04000141*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC086)
  .field /*04000142*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC087)
  .field /*04000143*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC088)
  .field /*04000144*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_ECDSA_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC089)
  .field /*04000145*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC08A)
  .field /*04000146*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC08B)
  .field /*04000147*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC08C)
  .field /*04000148*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDH_RSA_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC08D)
  .field /*04000149*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC08E)
  .field /*0400014A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC08F)
  .field /*0400014B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC090)
  .field /*0400014C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC091)
  .field /*0400014D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_CAMELLIA_128_GCM_SHA256 = uint16(0xC092)
  .field /*0400014E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_CAMELLIA_256_GCM_SHA384 = uint16(0xC093)
  .field /*0400014F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0xC094)
  .field /*04000150*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_CAMELLIA_256_CBC_SHA384 = uint16(0xC095)
  .field /*04000151*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0xC096)
  .field /*04000152*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_CAMELLIA_256_CBC_SHA384 = uint16(0xC097)
  .field /*04000153*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0xC098)
  .field /*04000154*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_CAMELLIA_256_CBC_SHA384 = uint16(0xC099)
  .field /*04000155*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_CAMELLIA_128_CBC_SHA256 = uint16(0xC09A)
  .field /*04000156*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_CAMELLIA_256_CBC_SHA384 = uint16(0xC09B)
  .field /*04000157*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_128_CCM = uint16(0xC09C)
  .field /*04000158*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_256_CCM = uint16(0xC09D)
  .field /*04000159*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_128_CCM = uint16(0xC09E)
  .field /*0400015A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_256_CCM = uint16(0xC09F)
  .field /*0400015B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_128_CCM_8 = uint16(0xC0A0)
  .field /*0400015C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_WITH_AES_256_CCM_8 = uint16(0xC0A1)
  .field /*0400015D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_128_CCM_8 = uint16(0xC0A2)
  .field /*0400015E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_AES_256_CCM_8 = uint16(0xC0A3)
  .field /*0400015F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_128_CCM = uint16(0xC0A4)
  .field /*04000160*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_256_CCM = uint16(0xC0A5)
  .field /*04000161*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_AES_128_CCM = uint16(0xC0A6)
  .field /*04000162*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_AES_256_CCM = uint16(0xC0A7)
  .field /*04000163*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_128_CCM_8 = uint16(0xC0A8)
  .field /*04000164*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_AES_256_CCM_8 = uint16(0xC0A9)
  .field /*04000165*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_DHE_WITH_AES_128_CCM_8 = uint16(0xC0AA)
  .field /*04000166*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_DHE_WITH_AES_256_CCM_8 = uint16(0xC0AB)
  .field /*04000167*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_128_CCM = uint16(0xC0AC)
  .field /*04000168*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_256_CCM = uint16(0xC0AD)
  .field /*04000169*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_128_CCM_8 = uint16(0xC0AE)
  .field /*0400016A*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_AES_256_CCM_8 = uint16(0xC0AF)
  .field /*0400016B*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECCPWD_WITH_AES_128_GCM_SHA256 = uint16(0xC0B0)
  .field /*0400016C*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECCPWD_WITH_AES_256_GCM_SHA384 = uint16(0xC0B1)
  .field /*0400016D*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECCPWD_WITH_AES_128_CCM_SHA256 = uint16(0xC0B2)
  .field /*0400016E*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECCPWD_WITH_AES_256_CCM_SHA384 = uint16(0xC0B3)
  .field /*0400016F*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 = uint16(0xCCA8)
  .field /*04000170*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256 = uint16(0xCCA9)
  .field /*04000171*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_RSA_WITH_CHACHA20_POLY1305_SHA256 = uint16(0xCCAA)
  .field /*04000172*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_PSK_WITH_CHACHA20_POLY1305_SHA256 = uint16(0xCCAB)
  .field /*04000173*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_CHACHA20_POLY1305_SHA256 = uint16(0xCCAC)
  .field /*04000174*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_DHE_PSK_WITH_CHACHA20_POLY1305_SHA256 = uint16(0xCCAD)
  .field /*04000175*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_RSA_PSK_WITH_CHACHA20_POLY1305_SHA256 = uint16(0xCCAE)
  .field /*04000176*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_AES_128_GCM_SHA256 = uint16(0xD001)
  .field /*04000177*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_AES_256_GCM_SHA384 = uint16(0xD002)
  .field /*04000178*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_AES_128_CCM_8_SHA256 = uint16(0xD003)
  .field /*04000179*/ public static literal valuetype System.Net.Security.TlsCipherSuite/*0200001C*/ TLS_ECDHE_PSK_WITH_AES_128_CCM_SHA256 = uint16(0xD005)
} // end of class System.Net.Security.TlsCipherSuite


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
