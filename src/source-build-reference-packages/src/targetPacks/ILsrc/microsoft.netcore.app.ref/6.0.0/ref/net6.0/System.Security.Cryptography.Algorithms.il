


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
// Time-date stamp:            0xc32bff11
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00008c00
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x0000abba
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
// Checksum:                       0x000146c8
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x0000ab67 [0x0000004f] address [size] of Import Directory:          
// 0x0000c000 [0x00000514] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00009600 [0x00002470] address [size] of Security Directory:        
// 0x0000e000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x0000aa5c [0x00000054] address [size] of Debug Directory:           
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
//              0x00008bc0 Virtual Size
//              0x00002000 Virtual Address
//              0x00008c00 Size of Raw Data
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
//              0x00000514 Virtual Size
//              0x0000c000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00008e00 Pointer to Raw Data
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
//              0x00009400 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000bbc
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x0000aba9 Import Name Table
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
// 0x00002904 [0x000080d8] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x0000a9dc [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00005694 Size
//              '#~' Name
//    Stream 2:
//              0x00005700 Offset
//              0x00001754 Size
//              '#Strings' Name
//    Stream 3:
//              0x00006e54 Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00006e58 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00006e68 Offset
//              0x00001270 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x0000020909a01f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 47728
// PE header size       : 512 (496 used)    ( 1.07%)
// PE additional info   : 10883             (22.80%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.15%)
// CLR meta-data size  : 32984              (69.11%)
// CLR additional info : 128                ( 0.27%)
// CLR method headers  : 644                ( 1.35%)
// Managed code         : 1583              ( 3.32%)
// Data                 : 2048              ( 4.29%)
// Unaccounted          : -1126             (-2.36%)

// Num.of PE sections   : 3
//   .text    - 35840
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 32984
//   Module        -    1 (10 bytes)
//   TypeDef       -   67 (938 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   50 (300 bytes)
//   MethodDef     -  671 (9394 bytes)    27 abstract, 0 native, 644 bodies
//   FieldDef      -   53 (318 bytes)     0 constant
//   MemberRef     -   29 (174 bytes)
//   ParamDef      - 1013 (6078 bytes)
//   Constant      -   28 (168 bytes)
//   CustomAttribute-  510 (3060 bytes)
//   InterfaceImpl -    9 (36 bytes)
//   PropertyMap   -   31 (124 bytes)
//   Property      -   95 (570 bytes)
//   MethodSemantic-  134 (804 bytes)
//   TypeSpec      -    2 (4 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    3 (60 bytes)
//   NestedClass   -    2 (8 bytes)
//   Strings       -  5972 bytes
//   Blobs         -  4720 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   204 bytes

// CLR additional info : 128

// CLR method headers : 644
//   Num.of method bodies  - 644
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 644

// Managed code : 1583
//   Ave method size - 2
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class Aes                            (public) (abstract) (auto) (ansi) 
// Class AesCcm                         (public) (auto) (ansi) (sealed) 
// Class AesGcm                         (public) (auto) (ansi) (sealed) 
// Class AesManaged                     (public) (auto) (ansi) (sealed) 
// Class AsymmetricKeyExchangeDeformatter (public) (abstract) (auto) (ansi) 
// Class AsymmetricKeyExchangeFormatter (public) (abstract) (auto) (ansi) 
// Class AsymmetricSignatureDeformatter (public) (abstract) (auto) (ansi) 
// Class AsymmetricSignatureFormatter   (public) (abstract) (auto) (ansi) 
// Class ChaCha20Poly1305               (public) (auto) (ansi) (sealed) 
// Class CryptoConfig                   (public) (auto) (ansi) 
// Class DeriveBytes                    (public) (abstract) (auto) (ansi) 
// Class DES                            (public) (abstract) (auto) (ansi) 
// Class DSA                            (public) (abstract) (auto) (ansi) 
// Class DSAParameters                  (public) (sequential) (ansi) (sealed) 
// Class DSASignatureDeformatter        (public) (auto) (ansi) 
// Class DSASignatureFormat             (public) (auto) (ansi) (sealed) 
// Class DSASignatureFormatter          (public) (auto) (ansi) 
// Class ECCurve                        (public) (sequential) (ansi) (sealed) 
// Class ECDiffieHellman                (public) (abstract) (auto) (ansi) 
// Class ECDiffieHellmanPublicKey       (public) (abstract) (auto) (ansi) 
// Class ECDsa                          (public) (abstract) (auto) (ansi) 
// Class ECParameters                   (public) (sequential) (ansi) (sealed) 
// Class ECPoint                        (public) (sequential) (ansi) (sealed) 
// Class HKDF                           (public) (abstract) (auto) (ansi) (sealed) 
// Class HMACMD5                        (public) (auto) (ansi) 
// Class HMACSHA1                       (public) (auto) (ansi) 
// Class HMACSHA256                     (public) (auto) (ansi) 
// Class HMACSHA384                     (public) (auto) (ansi) 
// Class HMACSHA512                     (public) (auto) (ansi) 
// Class IncrementalHash                (public) (auto) (ansi) (sealed) 
// Class MaskGenerationMethod           (public) (abstract) (auto) (ansi) 
// Class MD5                            (public) (abstract) (auto) (ansi) 
// Class PKCS1MaskGenerationMethod      (public) (auto) (ansi) 
// Class RandomNumberGenerator          (public) (abstract) (auto) (ansi) 
// Class RC2                            (public) (abstract) (auto) (ansi) 
// Class Rfc2898DeriveBytes             (public) (auto) (ansi) 
// Class Rijndael                       (public) (abstract) (auto) (ansi) 
// Class RijndaelManaged                (public) (auto) (ansi) (sealed) 
// Class RSA                            (public) (abstract) (auto) (ansi) 
// Class RSAEncryptionPadding           (public) (auto) (ansi) (sealed) 
// Class RSAEncryptionPaddingMode       (public) (auto) (ansi) (sealed) 
// Class RSAOAEPKeyExchangeDeformatter  (public) (auto) (ansi) 
// Class RSAOAEPKeyExchangeFormatter    (public) (auto) (ansi) 
// Class RSAParameters                  (public) (sequential) (ansi) (sealed) 
// Class RSAPKCS1KeyExchangeDeformatter (public) (auto) (ansi) 
// Class RSAPKCS1KeyExchangeFormatter   (public) (auto) (ansi) 
// Class RSAPKCS1SignatureDeformatter   (public) (auto) (ansi) 
// Class RSAPKCS1SignatureFormatter     (public) (auto) (ansi) 
// Class RSASignaturePadding            (public) (auto) (ansi) (sealed) 
// Class RSASignaturePaddingMode        (public) (auto) (ansi) (sealed) 
// Class SHA1                           (public) (abstract) (auto) (ansi) 
// Class SHA1Managed                    (public) (auto) (ansi) (sealed) 
// Class SHA256                         (public) (abstract) (auto) (ansi) 
// Class SHA256Managed                  (public) (auto) (ansi) (sealed) 
// Class SHA384                         (public) (abstract) (auto) (ansi) 
// Class SHA384Managed                  (public) (auto) (ansi) (sealed) 
// Class SHA512                         (public) (abstract) (auto) (ansi) 
// Class SHA512Managed                  (public) (auto) (ansi) (sealed) 
// Class SignatureDescription           (public) (auto) (ansi) 
// Class TripleDES                      (public) (abstract) (auto) (ansi) 
// Class ECCurveType                    (auto) (ansi) (sealed) (nested public) 
// Class NamedCurves                    (abstract) (auto) (ansi) (sealed) (nested public) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern /*23000001*/ System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000002*/ System.Security.Cryptography.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000003*/ System.Security.Cryptography.Encoding
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Security.Cryptography.Algorithms
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
  .custom /*0C00000A:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 27 53 79 73 74 65 6D 2E 53 65 63 75 72 69   // ..'System.Securi
                                                                                                                                                                        74 79 2E 43 72 79 70 74 6F 67 72 61 70 68 79 2E   // ty.Cryptography.
                                                                                                                                                                        41 6C 67 6F 72 69 74 68 6D 73 00 00 )             // Algorithms..
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000F:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C000010:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000011:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 27 53 79 73 74 65 6D 2E 53 65 63 75 72 69   // ..'System.Securi
                                                                                                                                                                       74 79 2E 43 72 79 70 74 6F 67 72 61 70 68 79 2E   // ty.Cryptography.
                                                                                                                                                                       41 6C 67 6F 72 69 74 68 6D 73 00 00 )             // Algorithms..
  .custom /*0C000012:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000013:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000014:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000015:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 27 53 79 73 74 65 6D 2E 53 65 63 75 72 69   // ..'System.Securi
                                                                                                                                                                 74 79 2E 43 72 79 70 74 6F 67 72 61 70 68 79 2E   // ty.Cryptography.
                                                                                                                                                                 41 6C 67 6F 72 69 74 68 6D 73 00 00 )             // Algorithms..
  .custom /*0C000016:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
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
.module System.Security.Cryptography.Algorithms.dll
// MVID: {15354db5-c607-46d4-abda-88d58824f83c}
.custom /*0C000003:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000017:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000018:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000018 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001A:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001C:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000018 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 17   |                  */ ldc.i4.1
    IL_0008:  /* 8D   | (01)000032       */ newarr     [System.Runtime/*23000001*/]System.Byte/*01000032*/
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
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000018 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001D:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001E:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000018 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000020:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000021:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000022:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000018       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000018 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.Aes
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/
{
  .custom /*0C000024:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000026:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000006*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Aes::.ctor

  .method /*06000007*/ public hidebysig static 
          class System.Security.Cryptography.Aes/*02000006*/ 
          Create() cil managed
  // SIG: 00 00 12 18
  {
    // Method begins at RVA 0x20a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Aes::Create

  .method /*06000008*/ public hidebysig static 
          class System.Security.Cryptography.Aes/*02000006*/ 
          Create(string algorithmName) cil managed
  // SIG: 00 01 12 18 0E
  {
    .custom /*0C00002D:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*08000001*/ 
    .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Aes::Create

} // end of class System.Security.Cryptography.Aes

.class /*02000007*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.AesCcm
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
{
  .custom /*0C000027:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000029:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C00002A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
  .custom /*0C00002B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
  .custom /*0C000002:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000009*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x20ab
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AesCcm::.ctor

  .method /*0600000A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20b3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AesCcm::.ctor

  .method /*0600000B*/ public hidebysig specialname static 
          bool  get_IsSupported() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x20bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCcm::get_IsSupported

  .method /*0600000C*/ public hidebysig specialname static 
          class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/ 
          get_NonceByteSizes() cil managed
  // SIG: 00 00 12 6D
  {
    // Method begins at RVA 0x20be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCcm::get_NonceByteSizes

  .method /*0600000D*/ public hidebysig specialname static 
          class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/ 
          get_TagByteSizes() cil managed
  // SIG: 00 00 12 6D
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesCcm::get_TagByteSizes

  .method /*0600000E*/ public hidebysig instance void 
          Decrypt(uint8[] nonce,
                  uint8[] ciphertext,
                  uint8[] tag,
                  uint8[] plaintext,
                  [opt] uint8[] associatedData) cil managed
  // SIG: 20 05 01 1D 05 1D 05 1D 05 1D 05 1D 05
  {
    .param [5]/*08000009*/  = nullref
    .custom /*0C000039:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20c4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCcm::Decrypt

  .method /*0600000F*/ public hidebysig instance void 
          Decrypt(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> nonce,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> ciphertext,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> tag,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> plaintext,
                  [opt] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> associatedData) cil managed
  // SIG: 20 05 01 15 11 69 01 05 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 15 11 69 01 05
  {
    .custom /*0C00004B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [5]/*0800000E*/  = nullref
    // Method begins at RVA 0x20c6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCcm::Decrypt

  .method /*06000010*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCcm::Dispose

  .method /*06000011*/ public hidebysig instance void 
          Encrypt(uint8[] nonce,
                  uint8[] plaintext,
                  uint8[] ciphertext,
                  uint8[] tag,
                  [opt] uint8[] associatedData) cil managed
  // SIG: 20 05 01 1D 05 1D 05 1D 05 1D 05 1D 05
  {
    .param [5]/*08000013*/  = nullref
    .custom /*0C000058:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ca
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCcm::Encrypt

  .method /*06000012*/ public hidebysig instance void 
          Encrypt(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> nonce,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> plaintext,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> ciphertext,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> tag,
                  [opt] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> associatedData) cil managed
  // SIG: 20 05 01 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 15 11 71 01 05 15 11 69 01 05
  {
    .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [5]/*08000018*/  = nullref
    // Method begins at RVA 0x20cc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesCcm::Encrypt

  .property /*17000001*/ callconv(8) bool
          IsSupported()
  {
    .get bool System.Security.Cryptography.AesCcm/*02000007*/::get_IsSupported() /* 0600000B */
  } // end of property AesCcm::IsSupported
  .property /*17000002*/ callconv(8) class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/
          NonceByteSizes()
  {
    .get class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/ System.Security.Cryptography.AesCcm/*02000007*/::get_NonceByteSizes() /* 0600000C */
  } // end of property AesCcm::NonceByteSizes
  .property /*17000003*/ callconv(8) class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/
          TagByteSizes()
  {
    .get class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/ System.Security.Cryptography.AesCcm/*02000007*/::get_TagByteSizes() /* 0600000D */
  } // end of property AesCcm::TagByteSizes
} // end of class System.Security.Cryptography.AesCcm

.class /*02000008*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.AesGcm
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
{
  .custom /*0C00002E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000030:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
  .custom /*0C000032:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
  .custom /*0C000019:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000013*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x20ce
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AesGcm::.ctor

  .method /*06000014*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    .custom /*0C000059:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x20d6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AesGcm::.ctor

  .method /*06000015*/ public hidebysig specialname static 
          bool  get_IsSupported() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x20de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesGcm::get_IsSupported

  .method /*06000016*/ public hidebysig specialname static 
          class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/ 
          get_NonceByteSizes() cil managed
  // SIG: 00 00 12 6D
  {
    // Method begins at RVA 0x20e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesGcm::get_NonceByteSizes

  .method /*06000017*/ public hidebysig specialname static 
          class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/ 
          get_TagByteSizes() cil managed
  // SIG: 00 00 12 6D
  {
    // Method begins at RVA 0x20e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesGcm::get_TagByteSizes

  .method /*06000018*/ public hidebysig instance void 
          Decrypt(uint8[] nonce,
                  uint8[] ciphertext,
                  uint8[] tag,
                  uint8[] plaintext,
                  [opt] uint8[] associatedData) cil managed
  // SIG: 20 05 01 1D 05 1D 05 1D 05 1D 05 1D 05
  {
    .param [5]/*0800001F*/  = nullref
    .custom /*0C00007A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20e7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesGcm::Decrypt

  .method /*06000019*/ public hidebysig instance void 
          Decrypt(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> nonce,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> ciphertext,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> tag,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> plaintext,
                  [opt] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> associatedData) cil managed
  // SIG: 20 05 01 15 11 69 01 05 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 15 11 69 01 05
  {
    .custom /*0C000066:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [5]/*08000024*/  = nullref
    // Method begins at RVA 0x20e9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesGcm::Decrypt

  .method /*0600001A*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20eb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesGcm::Dispose

  .method /*0600001B*/ public hidebysig instance void 
          Encrypt(uint8[] nonce,
                  uint8[] plaintext,
                  uint8[] ciphertext,
                  uint8[] tag,
                  [opt] uint8[] associatedData) cil managed
  // SIG: 20 05 01 1D 05 1D 05 1D 05 1D 05 1D 05
  {
    .param [5]/*08000029*/  = nullref
    .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ed
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesGcm::Encrypt

  .method /*0600001C*/ public hidebysig instance void 
          Encrypt(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> nonce,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> plaintext,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> ciphertext,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> tag,
                  [opt] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> associatedData) cil managed
  // SIG: 20 05 01 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 15 11 71 01 05 15 11 69 01 05
  {
    .custom /*0C00006C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [5]/*0800002E*/  = nullref
    // Method begins at RVA 0x20ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesGcm::Encrypt

  .property /*17000004*/ callconv(8) bool
          IsSupported()
  {
    .get bool System.Security.Cryptography.AesGcm/*02000008*/::get_IsSupported() /* 06000015 */
  } // end of property AesGcm::IsSupported
  .property /*17000005*/ callconv(8) class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/
          NonceByteSizes()
  {
    .get class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/ System.Security.Cryptography.AesGcm/*02000008*/::get_NonceByteSizes() /* 06000016 */
  } // end of property AesGcm::NonceByteSizes
  .property /*17000006*/ callconv(8) class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/
          TagByteSizes()
  {
    .get class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/ System.Security.Cryptography.AesGcm/*02000008*/::get_TagByteSizes() /* 06000017 */
  } // end of property AesGcm::TagByteSizes
} // end of class System.Security.Cryptography.AesGcm

.class /*02000009*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.AesManaged
       extends System.Security.Cryptography.Aes/*02000006*/
{
  .custom /*0C000034:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000035:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000036:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
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
  .custom /*0C000037:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000038:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600001D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20f1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000006       */ call       instance void System.Security.Cryptography.Aes/*02000006*/::.ctor() /* 06000006 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AesManaged::.ctor

  .method /*0600001E*/ public hidebysig specialname virtual 
          instance int32  get_BlockSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20f9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_BlockSize

  .method /*0600001F*/ public hidebysig specialname virtual 
          instance void  set_BlockSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x20fc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::set_BlockSize

  .method /*06000020*/ public hidebysig specialname virtual 
          instance int32  get_FeedbackSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_FeedbackSize

  .method /*06000021*/ public hidebysig specialname virtual 
          instance void  set_FeedbackSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2101
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::set_FeedbackSize

  .method /*06000022*/ public hidebysig specialname virtual 
          instance uint8[]  get_IV() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_IV

  .method /*06000023*/ public hidebysig specialname virtual 
          instance void  set_IV(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2106
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::set_IV

  .method /*06000024*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2108
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_Key

  .method /*06000025*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x210b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::set_Key

  .method /*06000026*/ public hidebysig specialname virtual 
          instance int32  get_KeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x210d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_KeySize

  .method /*06000027*/ public hidebysig specialname virtual 
          instance void  set_KeySize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2110
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::set_KeySize

  .method /*06000028*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[] 
          get_LegalBlockSizes() cil managed
  // SIG: 20 00 1D 12 6D
  {
    // Method begins at RVA 0x2112
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_LegalBlockSizes

  .method /*06000029*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[] 
          get_LegalKeySizes() cil managed
  // SIG: 20 00 1D 12 6D
  {
    // Method begins at RVA 0x2115
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_LegalKeySizes

  .method /*0600002A*/ public hidebysig specialname virtual 
          instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 80 81
  {
    // Method begins at RVA 0x2118
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_Mode

  .method /*0600002B*/ public hidebysig specialname virtual 
          instance void  set_Mode(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/ 'value') cil managed
  // SIG: 20 01 01 11 80 81
  {
    // Method begins at RVA 0x211b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::set_Mode

  .method /*0600002C*/ public hidebysig specialname virtual 
          instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/ 
          get_Padding() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x211d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::get_Padding

  .method /*0600002D*/ public hidebysig specialname virtual 
          instance void  set_Padding(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/ 'value') cil managed
  // SIG: 20 01 01 11 80 85
  {
    // Method begins at RVA 0x2120
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::set_Padding

  .method /*0600002E*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.ICryptoTransform/*01000022*/ 
          CreateDecryptor() cil managed
  // SIG: 20 00 12 80 89
  {
    // Method begins at RVA 0x2122
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::CreateDecryptor

  .method /*0600002F*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.ICryptoTransform/*01000022*/ 
          CreateDecryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 80 89 1D 05 1D 05
  {
    .param [2]/*08000037*/ 
    .custom /*0C0000BD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2125
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::CreateDecryptor

  .method /*06000030*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.ICryptoTransform/*01000022*/ 
          CreateEncryptor() cil managed
  // SIG: 20 00 12 80 89
  {
    // Method begins at RVA 0x2128
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::CreateEncryptor

  .method /*06000031*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.ICryptoTransform/*01000022*/ 
          CreateEncryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 80 89 1D 05 1D 05
  {
    .param [2]/*08000039*/ 
    .custom /*0C0000C2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x212b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AesManaged::CreateEncryptor

  .method /*06000032*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x212e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::Dispose

  .method /*06000033*/ public hidebysig virtual 
          instance void  GenerateIV() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2130
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::GenerateIV

  .method /*06000034*/ public hidebysig virtual 
          instance void  GenerateKey() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2132
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AesManaged::GenerateKey

  .property /*17000007*/ instance callconv(8) int32
          BlockSize()
  {
    .get instance int32 System.Security.Cryptography.AesManaged/*02000009*/::get_BlockSize() /* 0600001E */
    .set instance void System.Security.Cryptography.AesManaged/*02000009*/::set_BlockSize(int32) /* 0600001F */
  } // end of property AesManaged::BlockSize
  .property /*17000008*/ instance callconv(8) int32
          FeedbackSize()
  {
    .get instance int32 System.Security.Cryptography.AesManaged/*02000009*/::get_FeedbackSize() /* 06000020 */
    .set instance void System.Security.Cryptography.AesManaged/*02000009*/::set_FeedbackSize(int32) /* 06000021 */
  } // end of property AesManaged::FeedbackSize
  .property /*17000009*/ instance callconv(8) uint8[]
          IV()
  {
    .get instance uint8[] System.Security.Cryptography.AesManaged/*02000009*/::get_IV() /* 06000022 */
    .set instance void System.Security.Cryptography.AesManaged/*02000009*/::set_IV(uint8[]) /* 06000023 */
  } // end of property AesManaged::IV
  .property /*1700000A*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.AesManaged/*02000009*/::get_Key() /* 06000024 */
    .set instance void System.Security.Cryptography.AesManaged/*02000009*/::set_Key(uint8[]) /* 06000025 */
  } // end of property AesManaged::Key
  .property /*1700000B*/ instance callconv(8) int32
          KeySize()
  {
    .get instance int32 System.Security.Cryptography.AesManaged/*02000009*/::get_KeySize() /* 06000026 */
    .set instance void System.Security.Cryptography.AesManaged/*02000009*/::set_KeySize(int32) /* 06000027 */
  } // end of property AesManaged::KeySize
  .property /*1700000C*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[]
          LegalBlockSizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[] System.Security.Cryptography.AesManaged/*02000009*/::get_LegalBlockSizes() /* 06000028 */
  } // end of property AesManaged::LegalBlockSizes
  .property /*1700000D*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[]
          LegalKeySizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[] System.Security.Cryptography.AesManaged/*02000009*/::get_LegalKeySizes() /* 06000029 */
  } // end of property AesManaged::LegalKeySizes
  .property /*1700000E*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/
          Mode()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/ System.Security.Cryptography.AesManaged/*02000009*/::get_Mode() /* 0600002A */
    .set instance void System.Security.Cryptography.AesManaged/*02000009*/::set_Mode(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/) /* 0600002B */
  } // end of property AesManaged::Mode
  .property /*1700000F*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/
          Padding()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/ System.Security.Cryptography.AesManaged/*02000009*/::get_Padding() /* 0600002C */
    .set instance void System.Security.Cryptography.AesManaged/*02000009*/::set_Padding(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/) /* 0600002D */
  } // end of property AesManaged::Padding
} // end of class System.Security.Cryptography.AesManaged

.class /*0200000A*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.AsymmetricKeyExchangeDeformatter
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C00003C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00003E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000035*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2134
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsymmetricKeyExchangeDeformatter::.ctor

  .method /*06000036*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_Parameters() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricKeyExchangeDeformatter::get_Parameters

  .method /*06000037*/ public hidebysig newslot specialname abstract virtual 
          instance void  set_Parameters(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricKeyExchangeDeformatter::set_Parameters

  .method /*06000038*/ public hidebysig newslot abstract virtual 
          instance uint8[]  DecryptKeyExchange(uint8[] rgb) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    .custom /*0C0000BE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method AsymmetricKeyExchangeDeformatter::DecryptKeyExchange

  .method /*06000039*/ public hidebysig newslot abstract virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C0000BF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method AsymmetricKeyExchangeDeformatter::SetKey

  .property /*17000010*/ instance callconv(8) string
          Parameters()
  {
    .get instance string System.Security.Cryptography.AsymmetricKeyExchangeDeformatter/*0200000A*/::get_Parameters() /* 06000036 */
    .set instance void System.Security.Cryptography.AsymmetricKeyExchangeDeformatter/*0200000A*/::set_Parameters(string) /* 06000037 */
  } // end of property AsymmetricKeyExchangeDeformatter::Parameters
} // end of class System.Security.Cryptography.AsymmetricKeyExchangeDeformatter

.class /*0200000B*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.AsymmetricKeyExchangeFormatter
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C00003F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000041:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600003A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x213c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsymmetricKeyExchangeFormatter::.ctor

  .method /*0600003B*/ public hidebysig newslot specialname abstract virtual 
          instance string  get_Parameters() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0000C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method AsymmetricKeyExchangeFormatter::get_Parameters

  .method /*0600003C*/ public hidebysig newslot abstract virtual 
          instance uint8[]  CreateKeyExchange(uint8[] data) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricKeyExchangeFormatter::CreateKeyExchange

  .method /*0600003D*/ public hidebysig newslot abstract virtual 
          instance uint8[]  CreateKeyExchange(uint8[] data,
                                              class [System.Runtime/*23000001*/]System.Type/*01000024*/ symAlgType) cil managed
  // SIG: 20 02 1D 05 1D 05 12 80 91
  {
    .param [2]/*08000040*/ 
    .custom /*0C0000CE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method AsymmetricKeyExchangeFormatter::CreateKeyExchange

  .method /*0600003E*/ public hidebysig newslot abstract virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricKeyExchangeFormatter::SetKey

  .property /*17000011*/ instance callconv(8) string
          Parameters()
  {
    .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Security.Cryptography.AsymmetricKeyExchangeFormatter/*0200000B*/::get_Parameters() /* 0600003B */
  } // end of property AsymmetricKeyExchangeFormatter::Parameters
} // end of class System.Security.Cryptography.AsymmetricKeyExchangeFormatter

.class /*0200000C*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.AsymmetricSignatureDeformatter
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000042:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000043:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000044:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600003F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2144
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsymmetricSignatureDeformatter::.ctor

  .method /*06000040*/ public hidebysig newslot abstract virtual 
          instance void  SetHashAlgorithm(string strName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricSignatureDeformatter::SetHashAlgorithm

  .method /*06000041*/ public hidebysig newslot abstract virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricSignatureDeformatter::SetKey

  .method /*06000042*/ public hidebysig newslot abstract virtual 
          instance bool  VerifySignature(uint8[] rgbHash,
                                         uint8[] rgbSignature) cil managed
  // SIG: 20 02 02 1D 05 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricSignatureDeformatter::VerifySignature

  .method /*06000043*/ public hidebysig newslot virtual 
          instance bool  VerifySignature(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/ hash,
                                         uint8[] rgbSignature) cil managed
  // SIG: 20 02 02 12 80 95 1D 05
  {
    // Method begins at RVA 0x214c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsymmetricSignatureDeformatter::VerifySignature

} // end of class System.Security.Cryptography.AsymmetricSignatureDeformatter

.class /*0200000D*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.AsymmetricSignatureFormatter
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000045:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000047:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000044*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x214f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AsymmetricSignatureFormatter::.ctor

  .method /*06000045*/ public hidebysig newslot abstract virtual 
          instance uint8[]  CreateSignature(uint8[] rgbHash) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricSignatureFormatter::CreateSignature

  .method /*06000046*/ public hidebysig newslot virtual 
          instance uint8[]  CreateSignature(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/ hash) cil managed
  // SIG: 20 01 1D 05 12 80 95
  {
    // Method begins at RVA 0x2157
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AsymmetricSignatureFormatter::CreateSignature

  .method /*06000047*/ public hidebysig newslot abstract virtual 
          instance void  SetHashAlgorithm(string strName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricSignatureFormatter::SetHashAlgorithm

  .method /*06000048*/ public hidebysig newslot abstract virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x0
  } // end of method AsymmetricSignatureFormatter::SetKey

} // end of class System.Security.Cryptography.AsymmetricSignatureFormatter

.class /*0200000E*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.ChaCha20Poly1305
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
{
  .custom /*0C000048:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .custom /*0C000049:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
  .custom /*0C00004A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
  .method /*06000049*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key) cil managed
  // SIG: 20 01 01 1D 05
  {
    .custom /*0C0000D5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x215a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ChaCha20Poly1305::.ctor

  .method /*0600004A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    // Method begins at RVA 0x2162
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ChaCha20Poly1305::.ctor

  .method /*0600004B*/ public hidebysig specialname static 
          bool  get_IsSupported() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x216a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ChaCha20Poly1305::get_IsSupported

  .method /*0600004C*/ public hidebysig instance void 
          Decrypt(uint8[] nonce,
                  uint8[] ciphertext,
                  uint8[] tag,
                  uint8[] plaintext,
                  [opt] uint8[] associatedData) cil managed
  // SIG: 20 05 01 1D 05 1D 05 1D 05 1D 05 1D 05
  {
    .custom /*0C0000D6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [5]/*08000052*/  = nullref
    .custom /*0C0000D8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x216d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ChaCha20Poly1305::Decrypt

  .method /*0600004D*/ public hidebysig instance void 
          Decrypt(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> nonce,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> ciphertext,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> tag,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> plaintext,
                  [opt] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> associatedData) cil managed
  // SIG: 20 05 01 15 11 69 01 05 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 15 11 69 01 05
  {
    .param [5]/*08000057*/  = nullref
    // Method begins at RVA 0x216f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ChaCha20Poly1305::Decrypt

  .method /*0600004E*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2171
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ChaCha20Poly1305::Dispose

  .method /*0600004F*/ public hidebysig instance void 
          Encrypt(uint8[] nonce,
                  uint8[] plaintext,
                  uint8[] ciphertext,
                  uint8[] tag,
                  [opt] uint8[] associatedData) cil managed
  // SIG: 20 05 01 1D 05 1D 05 1D 05 1D 05 1D 05
  {
    .custom /*0C0000D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [5]/*0800005C*/  = nullref
    .custom /*0C0000E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2173
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ChaCha20Poly1305::Encrypt

  .method /*06000050*/ public hidebysig instance void 
          Encrypt(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> nonce,
                  valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> plaintext,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> ciphertext,
                  valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> tag,
                  [opt] valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> associatedData) cil managed
  // SIG: 20 05 01 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 15 11 71 01 05 15 11 69 01 05
  {
    .param [5]/*08000061*/  = nullref
    // Method begins at RVA 0x2175
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ChaCha20Poly1305::Encrypt

  .property /*17000012*/ callconv(8) bool
          IsSupported()
  {
    .get bool System.Security.Cryptography.ChaCha20Poly1305/*0200000E*/::get_IsSupported() /* 0600004B */
  } // end of property ChaCha20Poly1305::IsSupported
} // end of class System.Security.Cryptography.ChaCha20Poly1305

.class /*0200000F*/ public auto ansi beforefieldinit System.Security.Cryptography.CryptoConfig
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C00004C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000051*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2177
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method CryptoConfig::.ctor

  .method /*06000052*/ public hidebysig specialname static 
          bool  get_AllowOnlyFipsAlgorithms() cil managed
  // SIG: 00 00 02
  {
    // Method begins at RVA 0x217f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CryptoConfig::get_AllowOnlyFipsAlgorithms

  .method /*06000053*/ public hidebysig static 
          void  AddAlgorithm(class [System.Runtime/*23000001*/]System.Type/*01000024*/ 'algorithm',
                             string[] names) cil managed
  // SIG: 00 02 01 12 80 91 1D 0E
  {
    .custom /*0C0000D9:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .param [2]/*08000063*/ 
    .custom /*0C0000E4:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CryptoConfig::AddAlgorithm

  .method /*06000054*/ public hidebysig static 
          void  AddOID(string oid,
                       string[] names) cil managed
  // SIG: 00 02 01 0E 1D 0E
  {
    .custom /*0C0000DA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .param [2]/*08000065*/ 
    .custom /*0C0000E5:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2184
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method CryptoConfig::AddOID

  .method /*06000055*/ public hidebysig static 
          object  CreateFromName(string name) cil managed
  // SIG: 00 01 1C 0E
  {
    .custom /*0C0000DB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*08000066*/ 
    .custom /*0C0000E9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2186
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CryptoConfig::CreateFromName

  .method /*06000056*/ public hidebysig static 
          object  CreateFromName(string name,
                                 object[] args) cil managed
  // SIG: 00 02 1C 0E 1D 1C
  {
    .custom /*0C0000DC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .custom /*0C0000DD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [1]/*08000068*/ 
    .custom /*0C0000F0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000069*/ 
    .custom /*0C0000F3:0A000016*/ instance void [System.Runtime/*23000001*/]System.ParamArrayAttribute/*01000026*/::.ctor() /* 0A000016 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x2189
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CryptoConfig::CreateFromName

  .method /*06000057*/ public hidebysig static 
          uint8[]  EncodeOID(string str) cil managed
  // SIG: 00 01 1D 05 0E
  {
    .custom /*0C0000DE:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 53 45 6E 63 6F 64 65 4F 49 44 20 69 73 20   // ..SEncodeOID is 
                                                                                                                                                   6F 62 73 6F 6C 65 74 65 2E 20 55 73 65 20 74 68   // obsolete. Use th
                                                                                                                                                   65 20 41 53 4E 2E 31 20 66 75 6E 63 74 69 6F 6E   // e ASN.1 function
                                                                                                                                                   61 6C 69 74 79 20 70 72 6F 76 69 64 65 64 20 69   // ality provided i
                                                                                                                                                   6E 20 53 79 73 74 65 6D 2E 46 6F 72 6D 61 74 73   // n System.Formats
                                                                                                                                                   2E 41 73 6E 31 2E 02 00 54 0E 0C 44 69 61 67 6E   // .Asn1...T..Diagn
                                                                                                                                                   6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30   // osticId.SYSLIB00
                                                                                                                                                   33 31 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68   // 31T..UrlFormat"h
                                                                                                                                                   74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F   // ttps://aka.ms/do
                                                                                                                                                   74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30   // tnet-warnings/{0
                                                                                                                                                   7D )                                              // }
    .custom /*0C0000DF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x218c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CryptoConfig::EncodeOID

  .method /*06000058*/ public hidebysig static 
          string  MapNameToOID(string name) cil managed
  // SIG: 00 01 0E 0E
  {
    .custom /*0C0000E0:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .param [0]/*0800006B*/ 
    .custom /*0C0000F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x218f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method CryptoConfig::MapNameToOID

  .property /*17000013*/ callconv(8) bool
          AllowOnlyFipsAlgorithms()
  {
    .get bool System.Security.Cryptography.CryptoConfig/*0200000F*/::get_AllowOnlyFipsAlgorithms() /* 06000052 */
  } // end of property CryptoConfig::AllowOnlyFipsAlgorithms
} // end of class System.Security.Cryptography.CryptoConfig

.class /*02000010*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.DeriveBytes
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
{
  .custom /*0C00004E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000059*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2192
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DeriveBytes::.ctor

  .method /*0600005A*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x219a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeriveBytes::Dispose

  .method /*0600005B*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x219c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DeriveBytes::Dispose

  .method /*0600005C*/ public hidebysig newslot abstract virtual 
          instance uint8[]  GetBytes(int32 cb) cil managed
  // SIG: 20 01 1D 05 08
  {
    .custom /*0C0000E1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DeriveBytes::GetBytes

  .method /*0600005D*/ public hidebysig newslot abstract virtual 
          instance void  Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x0
  } // end of method DeriveBytes::Reset

} // end of class System.Security.Cryptography.DeriveBytes

.class /*02000011*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.DES
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/
{
  .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000051:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000052:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600005E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x219e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DES::.ctor

  .method /*0600005F*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x21a6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DES::get_Key

  .method /*06000060*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x21a9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DES::set_Key

  .method /*06000061*/ public hidebysig static 
          class System.Security.Cryptography.DES/*02000011*/ 
          Create() cil managed
  // SIG: 00 00 12 44
  {
    // Method begins at RVA 0x21ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DES::Create

  .method /*06000062*/ public hidebysig static 
          class System.Security.Cryptography.DES/*02000011*/ 
          Create(string algName) cil managed
  // SIG: 00 01 12 44 0E
  {
    .custom /*0C0000E3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*08000070*/ 
    .custom /*0C0000F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DES::Create

  .method /*06000063*/ public hidebysig static 
          bool  IsSemiWeakKey(uint8[] rgbKey) cil managed
  // SIG: 00 01 02 1D 05
  {
    // Method begins at RVA 0x21b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DES::IsSemiWeakKey

  .method /*06000064*/ public hidebysig static 
          bool  IsWeakKey(uint8[] rgbKey) cil managed
  // SIG: 00 01 02 1D 05
  {
    // Method begins at RVA 0x21b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DES::IsWeakKey

  .property /*17000014*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.DES/*02000011*/::get_Key() /* 0600005F */
    .set instance void System.Security.Cryptography.DES/*02000011*/::set_Key(uint8[]) /* 06000060 */
  } // end of property DES::Key
} // end of class System.Security.Cryptography.DES

.class /*02000012*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.DSA
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/
{
  .custom /*0C000055:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000065*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21b7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSA::.ctor

  .method /*06000066*/ public hidebysig static 
          class System.Security.Cryptography.DSA/*02000012*/ 
          Create() cil managed
  // SIG: 00 00 12 48
  {
    .custom /*0C0000E6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000E7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C0000E8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    // Method begins at RVA 0x21bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::Create

  .method /*06000067*/ public hidebysig static 
          class System.Security.Cryptography.DSA/*02000012*/ 
          Create(int32 keySizeInBits) cil managed
  // SIG: 00 01 12 48 08
  {
    .custom /*0C0000EA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000EB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C0000EC:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    // Method begins at RVA 0x21c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::Create

  .method /*06000068*/ public hidebysig static 
          class System.Security.Cryptography.DSA/*02000012*/ 
          Create(valuetype System.Security.Cryptography.DSAParameters/*02000013*/ parameters) cil managed
  // SIG: 00 01 12 48 11 4C
  {
    .custom /*0C0000ED:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000EE:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 03 69 6F 73 00 00 )                         // ...ios..
    .custom /*0C0000EF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 04 74 76 6F 73 00 00 )                      // ...tvos..
    // Method begins at RVA 0x21c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::Create

  .method /*06000069*/ public hidebysig static 
          class System.Security.Cryptography.DSA/*02000012*/ 
          Create(string algName) cil managed
  // SIG: 00 01 12 48 0E
  {
    .custom /*0C0000F1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0000F2:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*08000076*/ 
    .custom /*0C0000FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::Create

  .method /*0600006A*/ public hidebysig newslot abstract virtual 
          instance uint8[]  CreateSignature(uint8[] rgbHash) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    .custom /*0C0000F4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DSA::CreateSignature

  .method /*0600006B*/ public hidebysig instance uint8[] 
          CreateSignature(uint8[] rgbHash,
                          valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 02 1D 05 1D 05 11 54
  {
    .custom /*0C0000F5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::CreateSignature

  .method /*0600006C*/ family hidebysig newslot virtual 
          instance uint8[]  CreateSignatureCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                                valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 02 1D 05 15 11 69 01 05 11 54
  {
    .param [0]/*0800007B*/ 
    .custom /*0C0000FE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::CreateSignatureCore

  .method /*0600006D*/ public hidebysig newslot abstract virtual 
          instance valuetype System.Security.Cryptography.DSAParameters/*02000013*/ 
          ExportParameters(bool includePrivateParameters) cil managed
  // SIG: 20 01 11 4C 02
  {
    // Method begins at RVA 0x0
  } // end of method DSA::ExportParameters

  .method /*0600006E*/ public hidebysig virtual 
          instance void  FromXmlString(string xmlString) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000F7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSA::FromXmlString

  .method /*0600006F*/ public hidebysig instance int32 
          GetMaxSignatureSize(valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 01 08 11 54
  {
    // Method begins at RVA 0x21d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::GetMaxSignatureSize

  .method /*06000070*/ family hidebysig newslot virtual 
          instance uint8[]  HashData(uint8[] data,
                                     int32 offset,
                                     int32 count,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 04 1D 05 1D 05 08 08 11 80 9D
  {
    .custom /*0C0000F8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::HashData

  .method /*06000071*/ family hidebysig newslot virtual 
          instance uint8[]  HashData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 12 80 A1 11 80 9D
  {
    .custom /*0C0000FA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::HashData

  .method /*06000072*/ public hidebysig virtual 
          instance void  ImportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes,
                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                        [out] int32& bytesRead) cil managed
  // SIG: 20 03 01 15 11 69 01 05 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x21dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::ImportEncryptedPkcs8PrivateKey

  .method /*06000073*/ public hidebysig virtual 
          instance void  ImportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                        [out] int32& bytesRead) cil managed
  // SIG: 20 03 01 15 11 69 01 03 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x21df
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::ImportEncryptedPkcs8PrivateKey

  .method /*06000074*/ public hidebysig virtual 
          instance void  ImportFromEncryptedPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes) cil managed
  // SIG: 20 02 01 15 11 69 01 03 15 11 69 01 05
  {
    // Method begins at RVA 0x21e2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSA::ImportFromEncryptedPem

  .method /*06000075*/ public hidebysig virtual 
          instance void  ImportFromEncryptedPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password) cil managed
  // SIG: 20 02 01 15 11 69 01 03 15 11 69 01 03
  {
    // Method begins at RVA 0x21e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSA::ImportFromEncryptedPem

  .method /*06000076*/ public hidebysig virtual 
          instance void  ImportFromPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input) cil managed
  // SIG: 20 01 01 15 11 69 01 03
  {
    // Method begins at RVA 0x21e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSA::ImportFromPem

  .method /*06000077*/ public hidebysig newslot abstract virtual 
          instance void  ImportParameters(valuetype System.Security.Cryptography.DSAParameters/*02000013*/ parameters) cil managed
  // SIG: 20 01 01 11 4C
  {
    // Method begins at RVA 0x0
  } // end of method DSA::ImportParameters

  .method /*06000078*/ public hidebysig virtual 
          instance void  ImportPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                               [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::ImportPkcs8PrivateKey

  .method /*06000079*/ public hidebysig virtual 
          instance void  ImportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                    [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::ImportSubjectPublicKeyInfo

  .method /*0600007A*/ public hidebysig newslot virtual 
          instance uint8[]  SignData(uint8[] data,
                                     int32 offset,
                                     int32 count,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 04 1D 05 1D 05 08 08 11 80 9D
  {
    .custom /*0C0000FC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21ee
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::SignData

  .method /*0600007B*/ public hidebysig instance uint8[] 
          SignData(uint8[] data,
                   int32 offset,
                   int32 count,
                   valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                   valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 05 1D 05 1D 05 08 08 11 80 9D 11 54
  {
    .custom /*0C0000FD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::SignData

  .method /*0600007C*/ public hidebysig instance uint8[] 
          SignData(uint8[] data,
                   valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 1D 05 11 80 9D
  {
    .custom /*0C0000FF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::SignData

  .method /*0600007D*/ public hidebysig instance uint8[] 
          SignData(uint8[] data,
                   valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                   valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 1D 05 1D 05 11 80 9D 11 54
  {
    .custom /*0C000100:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21f7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::SignData

  .method /*0600007E*/ public hidebysig newslot virtual 
          instance uint8[]  SignData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 12 80 A1 11 80 9D
  {
    .custom /*0C000101:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::SignData

  .method /*0600007F*/ public hidebysig instance uint8[] 
          SignData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                   valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                   valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 1D 05 12 80 A1 11 80 9D 11 54
  {
    .custom /*0C000102:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::SignData

  .method /*06000080*/ family hidebysig newslot virtual 
          instance uint8[]  SignDataCore(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                         valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                         valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 1D 05 12 80 A1 11 80 9D 11 54
  {
    .custom /*0C000103:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2200
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::SignDataCore

  .method /*06000081*/ family hidebysig newslot virtual 
          instance uint8[]  SignDataCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                         valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                         valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 1D 05 15 11 69 01 05 11 80 9D 11 54
  {
    .param [0]/*080000AD*/ 
    .custom /*0C000110:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2203
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::SignDataCore

  .method /*06000082*/ public hidebysig virtual 
          instance string  ToXmlString(bool includePrivateParameters) cil managed
  // SIG: 20 01 0E 02
  {
    .custom /*0C000104:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2206
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::ToXmlString

  .method /*06000083*/ public hidebysig newslot virtual 
          instance bool  TryCreateSignature(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                            [out] int32& bytesWritten) cil managed
  // SIG: 20 03 02 15 11 69 01 05 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2209
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TryCreateSignature

  .method /*06000084*/ public hidebysig instance bool 
          TryCreateSignature(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                             valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                             valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat,
                             [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 54 10 08
  {
    // Method begins at RVA 0x220c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TryCreateSignature

  .method /*06000085*/ family hidebysig newslot virtual 
          instance bool  TryCreateSignatureCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                                valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat,
                                                [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 54 10 08
  {
    // Method begins at RVA 0x220f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TryCreateSignatureCore

  .method /*06000086*/ public hidebysig virtual 
          instance bool  TryExportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes,
                                                           class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PbeParameters/*01000029*/ pbeParameters,
                                                           valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                           [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 12 80 A5 15 11 71 01 05 10 08
  {
    .param [2]/*080000BE*/ 
    .custom /*0C000115:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2212
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TryExportEncryptedPkcs8PrivateKey

  .method /*06000087*/ public hidebysig virtual 
          instance bool  TryExportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                                                           class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PbeParameters/*01000029*/ pbeParameters,
                                                           valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                           [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 03 12 80 A5 15 11 71 01 05 10 08
  {
    .param [2]/*080000C2*/ 
    .custom /*0C000116:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2215
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TryExportEncryptedPkcs8PrivateKey

  .method /*06000088*/ public hidebysig virtual 
          instance bool  TryExportPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                  [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2218
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TryExportPkcs8PrivateKey

  .method /*06000089*/ public hidebysig virtual 
          instance bool  TryExportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                       [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x221b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TryExportSubjectPublicKeyInfo

  .method /*0600008A*/ family hidebysig newslot virtual 
          instance bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 10 08
  {
    // Method begins at RVA 0x221e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TryHashData

  .method /*0600008B*/ public hidebysig newslot virtual 
          instance bool  TrySignData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 10 08
  {
    // Method begins at RVA 0x2221
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TrySignData

  .method /*0600008C*/ public hidebysig instance bool 
          TrySignData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                      valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                      valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                      valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat,
                      [out] int32& bytesWritten) cil managed
  // SIG: 20 05 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 11 54 10 08
  {
    // Method begins at RVA 0x2224
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TrySignData

  .method /*0600008D*/ family hidebysig newslot virtual 
          instance bool  TrySignDataCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                         valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                         valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                         valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat,
                                         [out] int32& bytesWritten) cil managed
  // SIG: 20 05 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 11 54 10 08
  {
    // Method begins at RVA 0x2227
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::TrySignDataCore

  .method /*0600008E*/ public hidebysig instance bool 
          VerifyData(uint8[] data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 03 02 1D 05 1D 05 11 80 9D
  {
    .custom /*0C000105:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x222a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyData

  .method /*0600008F*/ public hidebysig instance bool 
          VerifyData(uint8[] data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 1D 05 1D 05 11 80 9D 11 54
  {
    .custom /*0C000106:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x222d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyData

  .method /*06000090*/ public hidebysig newslot virtual 
          instance bool  VerifyData(uint8[] data,
                                    int32 offset,
                                    int32 count,
                                    uint8[] signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 05 02 1D 05 08 08 1D 05 11 80 9D
  {
    .custom /*0C000107:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2230
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyData

  .method /*06000091*/ public hidebysig instance bool 
          VerifyData(uint8[] data,
                     int32 offset,
                     int32 count,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 06 02 1D 05 08 08 1D 05 11 80 9D 11 54
  {
    .custom /*0C000108:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2233
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyData

  .method /*06000092*/ public hidebysig newslot virtual 
          instance bool  VerifyData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                    uint8[] signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 03 02 12 80 A1 1D 05 11 80 9D
  {
    .custom /*0C000109:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2236
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyData

  .method /*06000093*/ public hidebysig instance bool 
          VerifyData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 12 80 A1 1D 05 11 80 9D 11 54
  {
    .custom /*0C00010A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2239
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyData

  .method /*06000094*/ public hidebysig newslot virtual 
          instance bool  VerifyData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 03 02 15 11 69 01 05 15 11 69 01 05 11 80 9D
  {
    // Method begins at RVA 0x223c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyData

  .method /*06000095*/ public hidebysig instance bool 
          VerifyData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                     valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 69 01 05 11 80 9D 11 54
  {
    // Method begins at RVA 0x223f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyData

  .method /*06000096*/ family hidebysig newslot virtual 
          instance bool  VerifyDataCore(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                        valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                        valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 12 80 A1 15 11 69 01 05 11 80 9D 11 54
  {
    .param [1]/*080000FB*/ 
    .custom /*0C000137:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2242
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyDataCore

  .method /*06000097*/ family hidebysig newslot virtual 
          instance bool  VerifyDataCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                        valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                        valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 69 01 05 11 80 9D 11 54
  {
    // Method begins at RVA 0x2245
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifyDataCore

  .method /*06000098*/ public hidebysig newslot abstract virtual 
          instance bool  VerifySignature(uint8[] rgbHash,
                                         uint8[] rgbSignature) cil managed
  // SIG: 20 02 02 1D 05 1D 05
  {
    .custom /*0C00010B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method DSA::VerifySignature

  .method /*06000099*/ public hidebysig instance bool 
          VerifySignature(uint8[] rgbHash,
                          uint8[] rgbSignature,
                          valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 02 1D 05 1D 05 11 54
  {
    .custom /*0C00010C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2248
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifySignature

  .method /*0600009A*/ public hidebysig newslot virtual 
          instance bool  VerifySignature(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature) cil managed
  // SIG: 20 02 02 15 11 69 01 05 15 11 69 01 05
  {
    // Method begins at RVA 0x224b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifySignature

  .method /*0600009B*/ public hidebysig instance bool 
          VerifySignature(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                          valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 02 15 11 69 01 05 15 11 69 01 05 11 54
  {
    // Method begins at RVA 0x224e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifySignature

  .method /*0600009C*/ family hidebysig newslot virtual 
          instance bool  VerifySignatureCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                             valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                             valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 02 15 11 69 01 05 15 11 69 01 05 11 54
  {
    // Method begins at RVA 0x2251
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSA::VerifySignatureCore

} // end of class System.Security.Cryptography.DSA

.class /*02000013*/ public sequential ansi sealed beforefieldinit System.Security.Cryptography.DSAParameters
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002A*/
{
  .custom /*0C000056:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ public int32 Counter
  .field /*04000005*/ public uint8[] G
  .field /*04000006*/ public uint8[] J
  .field /*04000007*/ public uint8[] P
  .field /*04000008*/ public uint8[] Q
  .field /*04000009*/ public uint8[] Seed
  .field /*0400000A*/ public uint8[] X
  .field /*0400000B*/ public uint8[] Y
} // end of class System.Security.Cryptography.DSAParameters

.class /*02000014*/ public auto ansi beforefieldinit System.Security.Cryptography.DSASignatureDeformatter
       extends System.Security.Cryptography.AsymmetricSignatureDeformatter/*0200000C*/
{
  .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00005C:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600009D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2254
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00003F       */ call       instance void System.Security.Cryptography.AsymmetricSignatureDeformatter/*0200000C*/::.ctor() /* 0600003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSASignatureDeformatter::.ctor

  .method /*0600009E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x225c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00003F       */ call       instance void System.Security.Cryptography.AsymmetricSignatureDeformatter/*0200000C*/::.ctor() /* 0600003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSASignatureDeformatter::.ctor

  .method /*0600009F*/ public hidebysig virtual 
          instance void  SetHashAlgorithm(string strName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2264
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSASignatureDeformatter::SetHashAlgorithm

  .method /*060000A0*/ public hidebysig virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x2266
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSASignatureDeformatter::SetKey

  .method /*060000A1*/ public hidebysig virtual 
          instance bool  VerifySignature(uint8[] rgbHash,
                                         uint8[] rgbSignature) cil managed
  // SIG: 20 02 02 1D 05 1D 05
  {
    // Method begins at RVA 0x2268
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSASignatureDeformatter::VerifySignature

} // end of class System.Security.Cryptography.DSASignatureDeformatter

.class /*02000015*/ public auto ansi sealed System.Security.Cryptography.DSASignatureFormat
       extends [System.Runtime/*23000001*/]System.Enum/*0100002B*/
{
  .field /*0400000C*/ public specialname rtspecialname int32 value__
  .field /*0400000D*/ public static literal valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ IeeeP1363FixedFieldConcatenation = int32(0x00000000)
  .field /*0400000E*/ public static literal valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ Rfc3279DerSequence = int32(0x00000001)
} // end of class System.Security.Cryptography.DSASignatureFormat

.class /*02000016*/ public auto ansi beforefieldinit System.Security.Cryptography.DSASignatureFormatter
       extends System.Security.Cryptography.AsymmetricSignatureFormatter/*0200000D*/
{
  .custom /*0C00005D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00005F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*060000A2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x226b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000044       */ call       instance void System.Security.Cryptography.AsymmetricSignatureFormatter/*0200000D*/::.ctor() /* 06000044 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSASignatureFormatter::.ctor

  .method /*060000A3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x2273
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000044       */ call       instance void System.Security.Cryptography.AsymmetricSignatureFormatter/*0200000D*/::.ctor() /* 06000044 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method DSASignatureFormatter::.ctor

  .method /*060000A4*/ public hidebysig virtual 
          instance uint8[]  CreateSignature(uint8[] rgbHash) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x227b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method DSASignatureFormatter::CreateSignature

  .method /*060000A5*/ public hidebysig virtual 
          instance void  SetHashAlgorithm(string strName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x227e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSASignatureFormatter::SetHashAlgorithm

  .method /*060000A6*/ public hidebysig virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x2280
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method DSASignatureFormatter::SetKey

} // end of class System.Security.Cryptography.DSASignatureFormatter

.class /*02000017*/ public sequential ansi sealed beforefieldinit System.Security.Cryptography.ECCurve
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002A*/
{
  .custom /*0C000060:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000061:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000062:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .class /*02000042*/ auto ansi sealed nested public ECCurveType
         extends [System.Runtime/*23000001*/]System.Enum/*0100002B*/
  {
    .custom /*0C0000D2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .field /*0400002F*/ public specialname rtspecialname int32 value__
    .field /*04000030*/ public static literal valuetype System.Security.Cryptography.ECCurve/*02000017*//ECCurveType/*02000042*/ Implicit = int32(0x00000000)
    .field /*04000031*/ public static literal valuetype System.Security.Cryptography.ECCurve/*02000017*//ECCurveType/*02000042*/ PrimeShortWeierstrass = int32(0x00000001)
    .field /*04000032*/ public static literal valuetype System.Security.Cryptography.ECCurve/*02000017*//ECCurveType/*02000042*/ PrimeTwistedEdwards = int32(0x00000002)
    .field /*04000033*/ public static literal valuetype System.Security.Cryptography.ECCurve/*02000017*//ECCurveType/*02000042*/ PrimeMontgomery = int32(0x00000003)
    .field /*04000034*/ public static literal valuetype System.Security.Cryptography.ECCurve/*02000017*//ECCurveType/*02000042*/ Characteristic2 = int32(0x00000004)
    .field /*04000035*/ public static literal valuetype System.Security.Cryptography.ECCurve/*02000017*//ECCurveType/*02000042*/ Named = int32(0x00000005)
  } // end of class ECCurveType

  .class /*02000043*/ abstract auto ansi sealed nested public beforefieldinit NamedCurves
         extends [System.Runtime/*23000001*/]System.Object/*01000018*/
  {
    .custom /*0C0000D3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .method /*0600028F*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP160r1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28d0
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP160r1

    .method /*06000290*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP160t1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28d3
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP160t1

    .method /*06000291*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP192r1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28d6
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP192r1

    .method /*06000292*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP192t1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28d9
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP192t1

    .method /*06000293*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP224r1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28dc
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP224r1

    .method /*06000294*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP224t1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28df
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP224t1

    .method /*06000295*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP256r1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28e2
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP256r1

    .method /*06000296*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP256t1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28e5
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP256t1

    .method /*06000297*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP320r1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28e8
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP320r1

    .method /*06000298*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP320t1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28eb
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP320t1

    .method /*06000299*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP384r1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28ee
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP384r1

    .method /*0600029A*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP384t1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28f1
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP384t1

    .method /*0600029B*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP512r1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28f4
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP512r1

    .method /*0600029C*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_brainpoolP512t1() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28f7
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_brainpoolP512t1

    .method /*0600029D*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_nistP256() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28fa
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_nistP256

    .method /*0600029E*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_nistP384() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x28fd
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_nistP384

    .method /*0600029F*/ public hidebysig specialname static 
            valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
            get_nistP521() cil managed
    // SIG: 00 00 11 5C
    {
      // Method begins at RVA 0x2900
      // Code size       2 (0x2)
      .maxstack  8
      IL_0000:  /* 14   |                  */ ldnull
      IL_0001:  /* 7A   |                  */ throw
    } // end of method NamedCurves::get_nistP521

    .property /*1700004F*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP160r1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP160r1() /* 0600028F */
    } // end of property NamedCurves::brainpoolP160r1
    .property /*17000050*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP160t1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP160t1() /* 06000290 */
    } // end of property NamedCurves::brainpoolP160t1
    .property /*17000051*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP192r1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP192r1() /* 06000291 */
    } // end of property NamedCurves::brainpoolP192r1
    .property /*17000052*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP192t1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP192t1() /* 06000292 */
    } // end of property NamedCurves::brainpoolP192t1
    .property /*17000053*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP224r1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP224r1() /* 06000293 */
    } // end of property NamedCurves::brainpoolP224r1
    .property /*17000054*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP224t1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP224t1() /* 06000294 */
    } // end of property NamedCurves::brainpoolP224t1
    .property /*17000055*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP256r1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP256r1() /* 06000295 */
    } // end of property NamedCurves::brainpoolP256r1
    .property /*17000056*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP256t1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP256t1() /* 06000296 */
    } // end of property NamedCurves::brainpoolP256t1
    .property /*17000057*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP320r1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP320r1() /* 06000297 */
    } // end of property NamedCurves::brainpoolP320r1
    .property /*17000058*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP320t1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP320t1() /* 06000298 */
    } // end of property NamedCurves::brainpoolP320t1
    .property /*17000059*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP384r1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP384r1() /* 06000299 */
    } // end of property NamedCurves::brainpoolP384r1
    .property /*1700005A*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP384t1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP384t1() /* 0600029A */
    } // end of property NamedCurves::brainpoolP384t1
    .property /*1700005B*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP512r1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP512r1() /* 0600029B */
    } // end of property NamedCurves::brainpoolP512r1
    .property /*1700005C*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            brainpoolP512t1()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_brainpoolP512t1() /* 0600029C */
    } // end of property NamedCurves::brainpoolP512t1
    .property /*1700005D*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            nistP256()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_nistP256() /* 0600029D */
    } // end of property NamedCurves::nistP256
    .property /*1700005E*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            nistP384()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_nistP384() /* 0600029E */
    } // end of property NamedCurves::nistP384
    .property /*1700005F*/ callconv(8) valuetype System.Security.Cryptography.ECCurve/*02000017*/
            nistP521()
    {
      .get valuetype System.Security.Cryptography.ECCurve/*02000017*/ System.Security.Cryptography.ECCurve/*02000017*//NamedCurves/*02000043*/::get_nistP521() /* 0600029F */
    } // end of property NamedCurves::nistP521
  } // end of class NamedCurves

  .field /*0400000F*/ private object _dummy
  .field /*04000010*/ private int32 _dummyPrimitive
  .field /*04000011*/ public uint8[] A
  .field /*04000012*/ public uint8[] B
  .field /*04000013*/ public uint8[] Cofactor
  .field /*04000014*/ public valuetype System.Security.Cryptography.ECCurve/*02000017*//ECCurveType/*02000042*/ CurveType
  .field /*04000015*/ public valuetype System.Security.Cryptography.ECPoint/*0200001C*/ G
  .field /*04000016*/ public valuetype [System.Runtime/*23000001*/]System.Nullable`1/*0100002C*/<valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/> Hash
  .field /*04000017*/ public uint8[] Order
  .field /*04000018*/ public uint8[] Polynomial
  .field /*04000019*/ public uint8[] Prime
  .field /*0400001A*/ public uint8[] Seed
  .method /*060000A7*/ public hidebysig specialname 
          instance bool  get_IsCharacteristic2() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2282
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECCurve::get_IsCharacteristic2

  .method /*060000A8*/ public hidebysig specialname 
          instance bool  get_IsExplicit() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2285
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECCurve::get_IsExplicit

  .method /*060000A9*/ public hidebysig specialname 
          instance bool  get_IsNamed() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2288
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECCurve::get_IsNamed

  .method /*060000AA*/ public hidebysig specialname 
          instance bool  get_IsPrime() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x228b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECCurve::get_IsPrime

  .method /*060000AB*/ public hidebysig specialname 
          instance class [System.Security.Cryptography.Encoding/*23000003*/]System.Security.Cryptography.Oid/*0100002D*/ 
          get_Oid() cil managed
  // SIG: 20 00 12 80 B5
  {
    .custom /*0C00010D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECCurve::get_Oid

  .method /*060000AC*/ public hidebysig static 
          valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
          CreateFromFriendlyName(string oidFriendlyName) cil managed
  // SIG: 00 01 11 5C 0E
  {
    .custom /*0C00010E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2291
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECCurve::CreateFromFriendlyName

  .method /*060000AD*/ public hidebysig static 
          valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
          CreateFromOid(class [System.Security.Cryptography.Encoding/*23000003*/]System.Security.Cryptography.Oid/*0100002D*/ curveOid) cil managed
  // SIG: 00 01 11 5C 12 80 B5
  {
    .custom /*0C00010F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2294
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECCurve::CreateFromOid

  .method /*060000AE*/ public hidebysig static 
          valuetype System.Security.Cryptography.ECCurve/*02000017*/ 
          CreateFromValue(string oidValue) cil managed
  // SIG: 00 01 11 5C 0E
  {
    .custom /*0C000111:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2297
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECCurve::CreateFromValue

  .method /*060000AF*/ public hidebysig instance void 
          Validate() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x229a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECCurve::Validate

  .property /*17000015*/ instance callconv(8) bool
          IsCharacteristic2()
  {
    .get instance bool System.Security.Cryptography.ECCurve/*02000017*/::get_IsCharacteristic2() /* 060000A7 */
  } // end of property ECCurve::IsCharacteristic2
  .property /*17000016*/ instance callconv(8) bool
          IsExplicit()
  {
    .get instance bool System.Security.Cryptography.ECCurve/*02000017*/::get_IsExplicit() /* 060000A8 */
  } // end of property ECCurve::IsExplicit
  .property /*17000017*/ instance callconv(8) bool
          IsNamed()
  {
    .get instance bool System.Security.Cryptography.ECCurve/*02000017*/::get_IsNamed() /* 060000A9 */
  } // end of property ECCurve::IsNamed
  .property /*17000018*/ instance callconv(8) bool
          IsPrime()
  {
    .get instance bool System.Security.Cryptography.ECCurve/*02000017*/::get_IsPrime() /* 060000AA */
  } // end of property ECCurve::IsPrime
  .property /*17000019*/ instance callconv(8) class [System.Security.Cryptography.Encoding/*23000003*/]System.Security.Cryptography.Oid/*0100002D*/
          Oid()
  {
    .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Security.Cryptography.Encoding/*23000003*/]System.Security.Cryptography.Oid/*0100002D*/ System.Security.Cryptography.ECCurve/*02000017*/::get_Oid() /* 060000AB */
  } // end of property ECCurve::Oid
} // end of class System.Security.Cryptography.ECCurve

.class /*02000018*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.ECDiffieHellman
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/
{
  .custom /*0C000063:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000065:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*060000B0*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x229c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellman::.ctor

  .method /*060000B1*/ public hidebysig specialname virtual 
          instance string  get_KeyExchangeAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::get_KeyExchangeAlgorithm

  .method /*060000B2*/ public hidebysig newslot specialname abstract virtual 
          instance class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/ 
          get_PublicKey() cil managed
  // SIG: 20 00 12 64
  {
    // Method begins at RVA 0x0
  } // end of method ECDiffieHellman::get_PublicKey

  .method /*060000B3*/ public hidebysig specialname virtual 
          instance string  get_SignatureAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000112:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::get_SignatureAlgorithm

  .method /*060000B4*/ public hidebysig static 
          class System.Security.Cryptography.ECDiffieHellman/*02000018*/ 
          Create() cil managed
  // SIG: 00 00 12 60
  {
    // Method begins at RVA 0x22aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::Create

  .method /*060000B5*/ public hidebysig static 
          class System.Security.Cryptography.ECDiffieHellman/*02000018*/ 
          Create(valuetype System.Security.Cryptography.ECCurve/*02000017*/ curve) cil managed
  // SIG: 00 01 12 60 11 5C
  {
    // Method begins at RVA 0x22ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::Create

  .method /*060000B6*/ public hidebysig static 
          class System.Security.Cryptography.ECDiffieHellman/*02000018*/ 
          Create(valuetype System.Security.Cryptography.ECParameters/*0200001B*/ parameters) cil managed
  // SIG: 00 01 12 60 11 6C
  {
    // Method begins at RVA 0x22b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::Create

  .method /*060000B7*/ public hidebysig static 
          class System.Security.Cryptography.ECDiffieHellman/*02000018*/ 
          Create(string 'algorithm') cil managed
  // SIG: 00 01 12 60 0E
  {
    .custom /*0C000113:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*0800011E*/ 
    .custom /*0C000145:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::Create

  .method /*060000B8*/ public hidebysig instance uint8[] 
          DeriveKeyFromHash(class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/ otherPartyPublicKey,
                            valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 12 64 11 80 9D
  {
    // Method begins at RVA 0x22b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::DeriveKeyFromHash

  .method /*060000B9*/ public hidebysig newslot virtual 
          instance uint8[]  DeriveKeyFromHash(class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/ otherPartyPublicKey,
                                              valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                              uint8[] secretPrepend,
                                              uint8[] secretAppend) cil managed
  // SIG: 20 04 1D 05 12 64 11 80 9D 1D 05 1D 05
  {
    .param [3]/*08000124*/ 
    .custom /*0C000147:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000125*/ 
    .custom /*0C000148:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::DeriveKeyFromHash

  .method /*060000BA*/ public hidebysig instance uint8[] 
          DeriveKeyFromHmac(class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/ otherPartyPublicKey,
                            valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                            uint8[] hmacKey) cil managed
  // SIG: 20 03 1D 05 12 64 11 80 9D 1D 05
  {
    .param [3]/*08000128*/ 
    .custom /*0C00014A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::DeriveKeyFromHmac

  .method /*060000BB*/ public hidebysig newslot virtual 
          instance uint8[]  DeriveKeyFromHmac(class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/ otherPartyPublicKey,
                                              valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                              uint8[] hmacKey,
                                              uint8[] secretPrepend,
                                              uint8[] secretAppend) cil managed
  // SIG: 20 05 1D 05 12 64 11 80 9D 1D 05 1D 05 1D 05
  {
    .custom /*0C000114:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [0]/*08000129*/ 
    .custom /*0C00014B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800012A*/ 
    .custom /*0C00014D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::DeriveKeyFromHmac

  .method /*060000BC*/ public hidebysig newslot virtual 
          instance uint8[]  DeriveKeyMaterial(class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/ otherPartyPublicKey) cil managed
  // SIG: 20 01 1D 05 12 64
  {
    // Method begins at RVA 0x22c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::DeriveKeyMaterial

  .method /*060000BD*/ public hidebysig newslot virtual 
          instance uint8[]  DeriveKeyTls(class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/ otherPartyPublicKey,
                                         uint8[] prfLabel,
                                         uint8[] prfSeed) cil managed
  // SIG: 20 03 1D 05 12 64 1D 05 1D 05
  {
    // Method begins at RVA 0x22c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::DeriveKeyTls

  .method /*060000BE*/ public hidebysig newslot virtual 
          instance uint8[]  ExportECPrivateKey() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x22c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ExportECPrivateKey

  .method /*060000BF*/ public hidebysig newslot virtual 
          instance valuetype System.Security.Cryptography.ECParameters/*0200001B*/ 
          ExportExplicitParameters(bool includePrivateParameters) cil managed
  // SIG: 20 01 11 6C 02
  {
    // Method begins at RVA 0x22cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ExportExplicitParameters

  .method /*060000C0*/ public hidebysig newslot virtual 
          instance valuetype System.Security.Cryptography.ECParameters/*0200001B*/ 
          ExportParameters(bool includePrivateParameters) cil managed
  // SIG: 20 01 11 6C 02
  {
    // Method begins at RVA 0x22ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ExportParameters

  .method /*060000C1*/ public hidebysig virtual 
          instance void  FromXmlString(string xmlString) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x22d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellman::FromXmlString

  .method /*060000C2*/ public hidebysig newslot virtual 
          instance void  GenerateKey(valuetype System.Security.Cryptography.ECCurve/*02000017*/ curve) cil managed
  // SIG: 20 01 01 11 5C
  {
    // Method begins at RVA 0x22d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellman::GenerateKey

  .method /*060000C3*/ public hidebysig newslot virtual 
          instance void  ImportECPrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                            [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    .custom /*0C000117:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ImportECPrivateKey

  .method /*060000C4*/ public hidebysig virtual 
          instance void  ImportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes,
                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                        [out] int32& bytesRead) cil managed
  // SIG: 20 03 01 15 11 69 01 05 15 11 69 01 05 10 08
  {
    .custom /*0C000118:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ImportEncryptedPkcs8PrivateKey

  .method /*060000C5*/ public hidebysig virtual 
          instance void  ImportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                        [out] int32& bytesRead) cil managed
  // SIG: 20 03 01 15 11 69 01 03 15 11 69 01 05 10 08
  {
    .custom /*0C000119:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ImportEncryptedPkcs8PrivateKey

  .method /*060000C6*/ public hidebysig virtual 
          instance void  ImportFromEncryptedPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes) cil managed
  // SIG: 20 02 01 15 11 69 01 03 15 11 69 01 05
  {
    .custom /*0C00011A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellman::ImportFromEncryptedPem

  .method /*060000C7*/ public hidebysig virtual 
          instance void  ImportFromEncryptedPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password) cil managed
  // SIG: 20 02 01 15 11 69 01 03 15 11 69 01 03
  {
    .custom /*0C00011B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22e0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellman::ImportFromEncryptedPem

  .method /*060000C8*/ public hidebysig virtual 
          instance void  ImportFromPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input) cil managed
  // SIG: 20 01 01 15 11 69 01 03
  {
    .custom /*0C00011C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22e2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellman::ImportFromPem

  .method /*060000C9*/ public hidebysig newslot virtual 
          instance void  ImportParameters(valuetype System.Security.Cryptography.ECParameters/*0200001B*/ parameters) cil managed
  // SIG: 20 01 01 11 6C
  {
    // Method begins at RVA 0x22e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellman::ImportParameters

  .method /*060000CA*/ public hidebysig virtual 
          instance void  ImportPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                               [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    .custom /*0C00011D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ImportPkcs8PrivateKey

  .method /*060000CB*/ public hidebysig virtual 
          instance void  ImportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                    [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    .custom /*0C00011E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ImportSubjectPublicKeyInfo

  .method /*060000CC*/ public hidebysig virtual 
          instance string  ToXmlString(bool includePrivateParameters) cil managed
  // SIG: 20 01 0E 02
  {
    // Method begins at RVA 0x22ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::ToXmlString

  .method /*060000CD*/ public hidebysig newslot virtual 
          instance bool  TryExportECPrivateKey(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                               [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C00011F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::TryExportECPrivateKey

  .method /*060000CE*/ public hidebysig virtual 
          instance bool  TryExportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes,
                                                           class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PbeParameters/*01000029*/ pbeParameters,
                                                           valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                           [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 12 80 A5 15 11 71 01 05 10 08
  {
    .custom /*0C000120:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*0800014D*/ 
    .custom /*0C00015D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::TryExportEncryptedPkcs8PrivateKey

  .method /*060000CF*/ public hidebysig virtual 
          instance bool  TryExportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                                                           class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PbeParameters/*01000029*/ pbeParameters,
                                                           valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                           [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 03 12 80 A5 15 11 71 01 05 10 08
  {
    .custom /*0C000121:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000151*/ 
    .custom /*0C00015E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x22f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::TryExportEncryptedPkcs8PrivateKey

  .method /*060000D0*/ public hidebysig virtual 
          instance bool  TryExportPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                  [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C000122:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::TryExportPkcs8PrivateKey

  .method /*060000D1*/ public hidebysig virtual 
          instance bool  TryExportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                       [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C000123:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x22fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellman::TryExportSubjectPublicKeyInfo

  .property /*1700001A*/ instance callconv(8) string
          KeyExchangeAlgorithm()
  {
    .get instance string System.Security.Cryptography.ECDiffieHellman/*02000018*/::get_KeyExchangeAlgorithm() /* 060000B1 */
  } // end of property ECDiffieHellman::KeyExchangeAlgorithm
  .property /*1700001B*/ instance callconv(8) class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/
          PublicKey()
  {
    .get instance class System.Security.Cryptography.ECDiffieHellmanPublicKey/*02000019*/ System.Security.Cryptography.ECDiffieHellman/*02000018*/::get_PublicKey() /* 060000B2 */
  } // end of property ECDiffieHellman::PublicKey
  .property /*1700001C*/ instance callconv(8) string
          SignatureAlgorithm()
  {
    .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Security.Cryptography.ECDiffieHellman/*02000018*/::get_SignatureAlgorithm() /* 060000B3 */
  } // end of property ECDiffieHellman::SignatureAlgorithm
} // end of class System.Security.Cryptography.ECDiffieHellman

.class /*02000019*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.ECDiffieHellmanPublicKey
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
{
  .custom /*0C000067:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
  .custom /*0C000023:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000D2*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22fe
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellmanPublicKey::.ctor

  .method /*060000D3*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] keyBlob) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2306
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellmanPublicKey::.ctor

  .method /*060000D4*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x230e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellmanPublicKey::Dispose

  .method /*060000D5*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2310
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDiffieHellmanPublicKey::Dispose

  .method /*060000D6*/ public hidebysig newslot virtual 
          instance valuetype System.Security.Cryptography.ECParameters/*0200001B*/ 
          ExportExplicitParameters() cil managed
  // SIG: 20 00 11 6C
  {
    // Method begins at RVA 0x2312
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellmanPublicKey::ExportExplicitParameters

  .method /*060000D7*/ public hidebysig newslot virtual 
          instance valuetype System.Security.Cryptography.ECParameters/*0200001B*/ 
          ExportParameters() cil managed
  // SIG: 20 00 11 6C
  {
    // Method begins at RVA 0x2315
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellmanPublicKey::ExportParameters

  .method /*060000D8*/ public hidebysig newslot virtual 
          instance uint8[]  ExportSubjectPublicKeyInfo() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2318
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellmanPublicKey::ExportSubjectPublicKeyInfo

  .method /*060000D9*/ public hidebysig newslot virtual 
          instance uint8[]  ToByteArray() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x231b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellmanPublicKey::ToByteArray

  .method /*060000DA*/ public hidebysig newslot virtual 
          instance string  ToXmlString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x231e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellmanPublicKey::ToXmlString

  .method /*060000DB*/ public hidebysig newslot virtual 
          instance bool  TryExportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                       [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C000124:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2321
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDiffieHellmanPublicKey::TryExportSubjectPublicKeyInfo

} // end of class System.Security.Cryptography.ECDiffieHellmanPublicKey

.class /*0200001A*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.ECDsa
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/
{
  .custom /*0C00006A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*060000DC*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2324
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ECDsa::.ctor

  .method /*060000DD*/ public hidebysig specialname virtual 
          instance string  get_KeyExchangeAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000125:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x232c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::get_KeyExchangeAlgorithm

  .method /*060000DE*/ public hidebysig specialname virtual 
          instance string  get_SignatureAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000126:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x232f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::get_SignatureAlgorithm

  .method /*060000DF*/ public hidebysig static 
          class System.Security.Cryptography.ECDsa/*0200001A*/ 
          Create() cil managed
  // SIG: 00 00 12 68
  {
    .custom /*0C000127:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2332
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::Create

  .method /*060000E0*/ public hidebysig static 
          class System.Security.Cryptography.ECDsa/*0200001A*/ 
          Create(valuetype System.Security.Cryptography.ECCurve/*02000017*/ curve) cil managed
  // SIG: 00 01 12 68 11 5C
  {
    .custom /*0C000128:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2335
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::Create

  .method /*060000E1*/ public hidebysig static 
          class System.Security.Cryptography.ECDsa/*0200001A*/ 
          Create(valuetype System.Security.Cryptography.ECParameters/*0200001B*/ parameters) cil managed
  // SIG: 00 01 12 68 11 6C
  {
    .custom /*0C000129:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2338
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::Create

  .method /*060000E2*/ public hidebysig static 
          class System.Security.Cryptography.ECDsa/*0200001A*/ 
          Create(string 'algorithm') cil managed
  // SIG: 00 01 12 68 0E
  {
    .custom /*0C00012A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00012B:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*0800015E*/ 
    .custom /*0C000164:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x233b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::Create

  .method /*060000E3*/ public hidebysig newslot virtual 
          instance uint8[]  ExportECPrivateKey() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C00012C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x233e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ExportECPrivateKey

  .method /*060000E4*/ public hidebysig newslot virtual 
          instance valuetype System.Security.Cryptography.ECParameters/*0200001B*/ 
          ExportExplicitParameters(bool includePrivateParameters) cil managed
  // SIG: 20 01 11 6C 02
  {
    // Method begins at RVA 0x2341
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ExportExplicitParameters

  .method /*060000E5*/ public hidebysig newslot virtual 
          instance valuetype System.Security.Cryptography.ECParameters/*0200001B*/ 
          ExportParameters(bool includePrivateParameters) cil managed
  // SIG: 20 01 11 6C 02
  {
    // Method begins at RVA 0x2344
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ExportParameters

  .method /*060000E6*/ public hidebysig virtual 
          instance void  FromXmlString(string xmlString) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00012D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2347
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDsa::FromXmlString

  .method /*060000E7*/ public hidebysig newslot virtual 
          instance void  GenerateKey(valuetype System.Security.Cryptography.ECCurve/*02000017*/ curve) cil managed
  // SIG: 20 01 01 11 5C
  {
    // Method begins at RVA 0x2349
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDsa::GenerateKey

  .method /*060000E8*/ public hidebysig instance int32 
          GetMaxSignatureSize(valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 01 08 11 54
  {
    // Method begins at RVA 0x234b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::GetMaxSignatureSize

  .method /*060000E9*/ family hidebysig newslot virtual 
          instance uint8[]  HashData(uint8[] data,
                                     int32 offset,
                                     int32 count,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 04 1D 05 1D 05 08 08 11 80 9D
  {
    .custom /*0C00012E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x234e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::HashData

  .method /*060000EA*/ family hidebysig newslot virtual 
          instance uint8[]  HashData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 12 80 A1 11 80 9D
  {
    .custom /*0C00012F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2351
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::HashData

  .method /*060000EB*/ public hidebysig newslot virtual 
          instance void  ImportECPrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                            [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x2354
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ImportECPrivateKey

  .method /*060000EC*/ public hidebysig virtual 
          instance void  ImportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes,
                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                        [out] int32& bytesRead) cil managed
  // SIG: 20 03 01 15 11 69 01 05 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x2357
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ImportEncryptedPkcs8PrivateKey

  .method /*060000ED*/ public hidebysig virtual 
          instance void  ImportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                        [out] int32& bytesRead) cil managed
  // SIG: 20 03 01 15 11 69 01 03 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x235a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ImportEncryptedPkcs8PrivateKey

  .method /*060000EE*/ public hidebysig virtual 
          instance void  ImportFromEncryptedPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes) cil managed
  // SIG: 20 02 01 15 11 69 01 03 15 11 69 01 05
  {
    // Method begins at RVA 0x235d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDsa::ImportFromEncryptedPem

  .method /*060000EF*/ public hidebysig virtual 
          instance void  ImportFromEncryptedPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password) cil managed
  // SIG: 20 02 01 15 11 69 01 03 15 11 69 01 03
  {
    // Method begins at RVA 0x235f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDsa::ImportFromEncryptedPem

  .method /*060000F0*/ public hidebysig virtual 
          instance void  ImportFromPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input) cil managed
  // SIG: 20 01 01 15 11 69 01 03
  {
    // Method begins at RVA 0x2361
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDsa::ImportFromPem

  .method /*060000F1*/ public hidebysig newslot virtual 
          instance void  ImportParameters(valuetype System.Security.Cryptography.ECParameters/*0200001B*/ parameters) cil managed
  // SIG: 20 01 01 11 6C
  {
    // Method begins at RVA 0x2363
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECDsa::ImportParameters

  .method /*060000F2*/ public hidebysig virtual 
          instance void  ImportPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                               [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x2365
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ImportPkcs8PrivateKey

  .method /*060000F3*/ public hidebysig virtual 
          instance void  ImportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                    [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    // Method begins at RVA 0x2368
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ImportSubjectPublicKeyInfo

  .method /*060000F4*/ public hidebysig newslot virtual 
          instance uint8[]  SignData(uint8[] data,
                                     int32 offset,
                                     int32 count,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 04 1D 05 1D 05 08 08 11 80 9D
  {
    .custom /*0C000130:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x236b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignData

  .method /*060000F5*/ public hidebysig instance uint8[] 
          SignData(uint8[] data,
                   int32 offset,
                   int32 count,
                   valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                   valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 05 1D 05 1D 05 08 08 11 80 9D 11 54
  {
    .custom /*0C000131:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x236e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignData

  .method /*060000F6*/ public hidebysig newslot virtual 
          instance uint8[]  SignData(uint8[] data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 1D 05 11 80 9D
  {
    .custom /*0C000132:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2371
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignData

  .method /*060000F7*/ public hidebysig instance uint8[] 
          SignData(uint8[] data,
                   valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                   valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 1D 05 1D 05 11 80 9D 11 54
  {
    .custom /*0C000133:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2374
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignData

  .method /*060000F8*/ public hidebysig newslot virtual 
          instance uint8[]  SignData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 12 80 A1 11 80 9D
  {
    .custom /*0C000134:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2377
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignData

  .method /*060000F9*/ public hidebysig instance uint8[] 
          SignData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                   valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                   valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 1D 05 12 80 A1 11 80 9D 11 54
  {
    .custom /*0C000135:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x237a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignData

  .method /*060000FA*/ family hidebysig newslot virtual 
          instance uint8[]  SignDataCore(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                         valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                         valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 1D 05 12 80 A1 11 80 9D 11 54
  {
    .custom /*0C000136:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x237d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignDataCore

  .method /*060000FB*/ family hidebysig newslot virtual 
          instance uint8[]  SignDataCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                         valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                         valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 1D 05 15 11 69 01 05 11 80 9D 11 54
  {
    .param [0]/*08000193*/ 
    .custom /*0C00017C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2380
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignDataCore

  .method /*060000FC*/ public hidebysig newslot abstract virtual 
          instance uint8[]  SignHash(uint8[] hash) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    .custom /*0C000138:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ECDsa::SignHash

  .method /*060000FD*/ public hidebysig instance uint8[] 
          SignHash(uint8[] hash,
                   valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 02 1D 05 1D 05 11 54
  {
    .custom /*0C000139:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2383
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignHash

  .method /*060000FE*/ family hidebysig newslot virtual 
          instance uint8[]  SignHashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                         valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 02 1D 05 15 11 69 01 05 11 54
  {
    .param [0]/*0800019A*/ 
    .custom /*0C00017E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2386
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::SignHashCore

  .method /*060000FF*/ public hidebysig virtual 
          instance string  ToXmlString(bool includePrivateParameters) cil managed
  // SIG: 20 01 0E 02
  {
    .custom /*0C00013A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2389
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::ToXmlString

  .method /*06000100*/ public hidebysig newslot virtual 
          instance bool  TryExportECPrivateKey(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                               [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x238c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TryExportECPrivateKey

  .method /*06000101*/ public hidebysig virtual 
          instance bool  TryExportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes,
                                                           class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PbeParameters/*01000029*/ pbeParameters,
                                                           valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                           [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 12 80 A5 15 11 71 01 05 10 08
  {
    .param [2]/*080001A1*/ 
    .custom /*0C000181:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x238f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TryExportEncryptedPkcs8PrivateKey

  .method /*06000102*/ public hidebysig virtual 
          instance bool  TryExportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                                                           class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PbeParameters/*01000029*/ pbeParameters,
                                                           valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                           [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 03 12 80 A5 15 11 71 01 05 10 08
  {
    .param [2]/*080001A5*/ 
    .custom /*0C000182:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2392
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TryExportEncryptedPkcs8PrivateKey

  .method /*06000103*/ public hidebysig virtual 
          instance bool  TryExportPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                  [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2395
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TryExportPkcs8PrivateKey

  .method /*06000104*/ public hidebysig virtual 
          instance bool  TryExportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                       [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2398
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TryExportSubjectPublicKeyInfo

  .method /*06000105*/ family hidebysig newslot virtual 
          instance bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 10 08
  {
    // Method begins at RVA 0x239b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TryHashData

  .method /*06000106*/ public hidebysig newslot virtual 
          instance bool  TrySignData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 10 08
  {
    // Method begins at RVA 0x239e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TrySignData

  .method /*06000107*/ public hidebysig instance bool 
          TrySignData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                      valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                      valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                      valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat,
                      [out] int32& bytesWritten) cil managed
  // SIG: 20 05 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 11 54 10 08
  {
    // Method begins at RVA 0x23a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TrySignData

  .method /*06000108*/ family hidebysig newslot virtual 
          instance bool  TrySignDataCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                         valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                         valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                         valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat,
                                         [out] int32& bytesWritten) cil managed
  // SIG: 20 05 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 11 54 10 08
  {
    // Method begins at RVA 0x23a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TrySignDataCore

  .method /*06000109*/ public hidebysig newslot virtual 
          instance bool  TrySignHash(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                     [out] int32& bytesWritten) cil managed
  // SIG: 20 03 02 15 11 69 01 05 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x23a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TrySignHash

  .method /*0600010A*/ public hidebysig instance bool 
          TrySignHash(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                      valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                      valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat,
                      [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 54 10 08
  {
    // Method begins at RVA 0x23aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TrySignHash

  .method /*0600010B*/ family hidebysig newslot virtual 
          instance bool  TrySignHashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                         valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                         valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat,
                                         [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 54 10 08
  {
    // Method begins at RVA 0x23ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::TrySignHashCore

  .method /*0600010C*/ public hidebysig instance bool 
          VerifyData(uint8[] data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 03 02 1D 05 1D 05 11 80 9D
  {
    .custom /*0C00013B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyData

  .method /*0600010D*/ public hidebysig instance bool 
          VerifyData(uint8[] data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 1D 05 1D 05 11 80 9D 11 54
  {
    .custom /*0C00013C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23b3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyData

  .method /*0600010E*/ public hidebysig newslot virtual 
          instance bool  VerifyData(uint8[] data,
                                    int32 offset,
                                    int32 count,
                                    uint8[] signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 05 02 1D 05 08 08 1D 05 11 80 9D
  {
    .custom /*0C00013D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23b6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyData

  .method /*0600010F*/ public hidebysig instance bool 
          VerifyData(uint8[] data,
                     int32 offset,
                     int32 count,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 06 02 1D 05 08 08 1D 05 11 80 9D 11 54
  {
    .custom /*0C00013E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23b9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyData

  .method /*06000110*/ public hidebysig instance bool 
          VerifyData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 03 02 12 80 A1 1D 05 11 80 9D
  {
    .custom /*0C00013F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyData

  .method /*06000111*/ public hidebysig instance bool 
          VerifyData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 12 80 A1 1D 05 11 80 9D 11 54
  {
    .custom /*0C000140:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23bf
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyData

  .method /*06000112*/ public hidebysig newslot virtual 
          instance bool  VerifyData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 03 02 15 11 69 01 05 15 11 69 01 05 11 80 9D
  {
    // Method begins at RVA 0x23c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyData

  .method /*06000113*/ public hidebysig instance bool 
          VerifyData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                     valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 69 01 05 11 80 9D 11 54
  {
    // Method begins at RVA 0x23c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyData

  .method /*06000114*/ family hidebysig newslot virtual 
          instance bool  VerifyDataCore(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                        valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                        valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 12 80 A1 15 11 69 01 05 11 80 9D 11 54
  {
    .param [1]/*080001E9*/ 
    .custom /*0C000193:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyDataCore

  .method /*06000115*/ family hidebysig newslot virtual 
          instance bool  VerifyDataCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                        valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                        valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 69 01 05 11 80 9D 11 54
  {
    // Method begins at RVA 0x23cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyDataCore

  .method /*06000116*/ public hidebysig newslot abstract virtual 
          instance bool  VerifyHash(uint8[] hash,
                                    uint8[] signature) cil managed
  // SIG: 20 02 02 1D 05 1D 05
  {
    .custom /*0C000141:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method ECDsa::VerifyHash

  .method /*06000117*/ public hidebysig instance bool 
          VerifyHash(uint8[] hash,
                     uint8[] signature,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 02 1D 05 1D 05 11 54
  {
    .custom /*0C000142:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyHash

  .method /*06000118*/ public hidebysig newslot virtual 
          instance bool  VerifyHash(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature) cil managed
  // SIG: 20 02 02 15 11 69 01 05 15 11 69 01 05
  {
    // Method begins at RVA 0x23d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyHash

  .method /*06000119*/ public hidebysig instance bool 
          VerifyHash(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                     valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                     valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 02 15 11 69 01 05 15 11 69 01 05 11 54
  {
    // Method begins at RVA 0x23d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyHash

  .method /*0600011A*/ family hidebysig newslot virtual 
          instance bool  VerifyHashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                        valuetype System.Security.Cryptography.DSASignatureFormat/*02000015*/ signatureFormat) cil managed
  // SIG: 20 03 02 15 11 69 01 05 15 11 69 01 05 11 54
  {
    // Method begins at RVA 0x23d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ECDsa::VerifyHashCore

  .property /*1700001D*/ instance callconv(8) string
          KeyExchangeAlgorithm()
  {
    .custom /*0C000072:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Security.Cryptography.ECDsa/*0200001A*/::get_KeyExchangeAlgorithm() /* 060000DD */
  } // end of property ECDsa::KeyExchangeAlgorithm
  .property /*1700001E*/ instance callconv(8) string
          SignatureAlgorithm()
  {
    .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance string System.Security.Cryptography.ECDsa/*0200001A*/::get_SignatureAlgorithm() /* 060000DE */
  } // end of property ECDsa::SignatureAlgorithm
} // end of class System.Security.Cryptography.ECDsa

.class /*0200001B*/ public sequential ansi sealed beforefieldinit System.Security.Cryptography.ECParameters
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002A*/
{
  .custom /*0C00006B:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .field /*0400001B*/ public valuetype System.Security.Cryptography.ECCurve/*02000017*/ Curve
  .field /*0400001C*/ public uint8[] D
  .custom /*0C00006D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
  .field /*0400001D*/ public valuetype System.Security.Cryptography.ECPoint/*0200001C*/ Q
  .method /*0600011B*/ public hidebysig instance void 
          Validate() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23da
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ECParameters::Validate

} // end of class System.Security.Cryptography.ECParameters

.class /*0200001C*/ public sequential ansi sealed beforefieldinit System.Security.Cryptography.ECPoint
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002A*/
{
  .custom /*0C00006E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*0400001E*/ public uint8[] X
  .field /*0400001F*/ public uint8[] Y
} // end of class System.Security.Cryptography.ECPoint

.class /*0200001D*/ public abstract auto ansi sealed beforefieldinit System.Security.Cryptography.HKDF
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000071:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600011C*/ public hidebysig static 
          uint8[]  DeriveKey(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithmName,
                             uint8[] ikm,
                             int32 outputLength,
                             [opt] uint8[] salt,
                             [opt] uint8[] info) cil managed
  // SIG: 00 05 1D 05 11 80 9D 1D 05 08 1D 05 1D 05
  {
    .custom /*0C000143:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000201*/  = nullref
    .custom /*0C0001A2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [5]/*08000202*/  = nullref
    .custom /*0C0001A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23dc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HKDF::DeriveKey

  .method /*0600011D*/ public hidebysig static 
          void  DeriveKey(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithmName,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> ikm,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> output,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> salt,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> info) cil managed
  // SIG: 00 05 01 11 80 9D 15 11 69 01 05 15 11 71 01 05 15 11 69 01 05 15 11 69 01 05
  {
    // Method begins at RVA 0x23df
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HKDF::DeriveKey

  .method /*0600011E*/ public hidebysig static 
          uint8[]  Expand(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithmName,
                          uint8[] prk,
                          int32 outputLength,
                          [opt] uint8[] info) cil managed
  // SIG: 00 04 1D 05 11 80 9D 1D 05 08 1D 05
  {
    .custom /*0C000144:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [4]/*0800020B*/  = nullref
    .custom /*0C0001A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HKDF::Expand

  .method /*0600011F*/ public hidebysig static 
          void  Expand(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithmName,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> prk,
                       valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> output,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> info) cil managed
  // SIG: 00 04 01 11 80 9D 15 11 69 01 05 15 11 71 01 05 15 11 69 01 05
  {
    // Method begins at RVA 0x23e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HKDF::Expand

  .method /*06000120*/ public hidebysig static 
          uint8[]  Extract(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithmName,
                           uint8[] ikm,
                           [opt] uint8[] salt) cil managed
  // SIG: 00 03 1D 05 11 80 9D 1D 05 1D 05
  {
    .custom /*0C000146:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000212*/  = nullref
    .custom /*0C0001AA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x23e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HKDF::Extract

  .method /*06000121*/ public hidebysig static 
          int32  Extract(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithmName,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> ikm,
                         valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> salt,
                         valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> prk) cil managed
  // SIG: 00 04 08 11 80 9D 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05
  {
    // Method begins at RVA 0x23e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HKDF::Extract

} // end of class System.Security.Cryptography.HKDF

.class /*0200001E*/ public auto ansi beforefieldinit System.Security.Cryptography.HMACMD5
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/
{
  .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000075:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000122*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23ec
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACMD5::.ctor

  .method /*06000123*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x23f4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACMD5::.ctor

  .method /*06000124*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x23fc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACMD5::get_Key

  .method /*06000125*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x23ff
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACMD5::set_Key

  .method /*06000126*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2401
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACMD5::Dispose

  .method /*06000127*/ family hidebysig virtual 
          instance void  HashCore(uint8[] rgb,
                                  int32 ib,
                                  int32 cb) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2403
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACMD5::HashCore

  .method /*06000128*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    .custom /*0C000149:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2405
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACMD5::HashCore

  .method /*06000129*/ public hidebysig static 
          uint8[]  HashData(uint8[] key,
                            uint8[] source) cil managed
  // SIG: 00 02 1D 05 1D 05 1D 05
  {
    // Method begins at RVA 0x2407
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACMD5::HashData

  .method /*0600012A*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 02 1D 05 15 11 69 01 05 15 11 69 01 05
  {
    .custom /*0C00014C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000220*/ 
    .custom /*0C0001B0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x240a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACMD5::HashData

  .method /*0600012B*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 03 08 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05
  {
    .custom /*0C00014E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x240d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACMD5::HashData

  .method /*0600012C*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2410
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACMD5::HashFinal

  .method /*0600012D*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2413
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACMD5::Initialize

  .method /*0600012E*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 04 02 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 10 08
  {
    .custom /*0C00014F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2415
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACMD5::TryHashData

  .method /*0600012F*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C000150:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2418
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACMD5::TryHashFinal

  .property /*1700001F*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.HMACMD5/*0200001E*/::get_Key() /* 06000124 */
    .set instance void System.Security.Cryptography.HMACMD5/*0200001E*/::set_Key(uint8[]) /* 06000125 */
  } // end of property HMACMD5::Key
} // end of class System.Security.Cryptography.HMACMD5

.class /*0200001F*/ public auto ansi beforefieldinit System.Security.Cryptography.HMACSHA1
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/
{
  .custom /*0C000077:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000079:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000130*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x241b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA1::.ctor

  .method /*06000131*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2423
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA1::.ctor

  .method /*06000132*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key,
                               bool useManagedSha1) cil managed
  // SIG: 20 02 01 1D 05 02
  {
    .custom /*0C000151:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
    .custom /*0C000152:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 80 83 48 4D 41 43 53 48 41 31 20 61 6C 77   // ....HMACSHA1 alw
                                                                                                                                                   61 79 73 20 75 73 65 73 20 74 68 65 20 61 6C 67   // ays uses the alg
                                                                                                                                                   6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65 6E 74   // orithm implement
                                                                                                                                                   61 74 69 6F 6E 20 70 72 6F 76 69 64 65 64 20 62   // ation provided b
                                                                                                                                                   79 20 74 68 65 20 70 6C 61 74 66 6F 72 6D 2E 20   // y the platform. 
                                                                                                                                                   55 73 65 20 61 20 63 6F 6E 73 74 72 75 63 74 6F   // Use a constructo
                                                                                                                                                   72 20 77 69 74 68 6F 75 74 20 74 68 65 20 75 73   // r without the us
                                                                                                                                                   65 4D 61 6E 61 67 65 64 53 68 61 31 20 70 61 72   // eManagedSha1 par
                                                                                                                                                   61 6D 65 74 65 72 2E 02 00 54 0E 0C 44 69 61 67   // ameter...T..Diag
                                                                                                                                                   6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30   // nosticId.SYSLIB0
                                                                                                                                                   30 33 30 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22   // 030T..UrlFormat"
                                                                                                                                                   68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64   // https://aka.ms/d
                                                                                                                                                   6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B   // otnet-warnings/{
                                                                                                                                                   30 7D )                                           // 0}
    // Method begins at RVA 0x242b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA1::.ctor

  .method /*06000133*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2433
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA1::get_Key

  .method /*06000134*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2436
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA1::set_Key

  .method /*06000135*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2438
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA1::Dispose

  .method /*06000136*/ family hidebysig virtual 
          instance void  HashCore(uint8[] rgb,
                                  int32 ib,
                                  int32 cb) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x243a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA1::HashCore

  .method /*06000137*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    .custom /*0C000153:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x243c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA1::HashCore

  .method /*06000138*/ public hidebysig static 
          uint8[]  HashData(uint8[] key,
                            uint8[] source) cil managed
  // SIG: 00 02 1D 05 1D 05 1D 05
  {
    // Method begins at RVA 0x243e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA1::HashData

  .method /*06000139*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 02 1D 05 15 11 69 01 05 15 11 69 01 05
  {
    .custom /*0C000154:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000237*/ 
    .custom /*0C0001B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2441
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA1::HashData

  .method /*0600013A*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 03 08 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05
  {
    .custom /*0C000155:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2444
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA1::HashData

  .method /*0600013B*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2447
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA1::HashFinal

  .method /*0600013C*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x244a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA1::Initialize

  .method /*0600013D*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 04 02 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 10 08
  {
    .custom /*0C000156:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x244c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA1::TryHashData

  .method /*0600013E*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C000157:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x244f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA1::TryHashFinal

  .property /*17000020*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.HMACSHA1/*0200001F*/::get_Key() /* 06000133 */
    .set instance void System.Security.Cryptography.HMACSHA1/*0200001F*/::set_Key(uint8[]) /* 06000134 */
  } // end of property HMACSHA1::Key
} // end of class System.Security.Cryptography.HMACSHA1

.class /*02000020*/ public auto ansi beforefieldinit System.Security.Cryptography.HMACSHA256
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/
{
  .custom /*0C00007B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00007D:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600013F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2452
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA256::.ctor

  .method /*06000140*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x245a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA256::.ctor

  .method /*06000141*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2462
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA256::get_Key

  .method /*06000142*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2465
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA256::set_Key

  .method /*06000143*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2467
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA256::Dispose

  .method /*06000144*/ family hidebysig virtual 
          instance void  HashCore(uint8[] rgb,
                                  int32 ib,
                                  int32 cb) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2469
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA256::HashCore

  .method /*06000145*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    .custom /*0C000158:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x246b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA256::HashCore

  .method /*06000146*/ public hidebysig static 
          uint8[]  HashData(uint8[] key,
                            uint8[] source) cil managed
  // SIG: 00 02 1D 05 1D 05 1D 05
  {
    // Method begins at RVA 0x246d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA256::HashData

  .method /*06000147*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 02 1D 05 15 11 69 01 05 15 11 69 01 05
  {
    .custom /*0C000159:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*0800024C*/ 
    .custom /*0C0001BF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2470
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA256::HashData

  .method /*06000148*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 03 08 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05
  {
    .custom /*0C00015A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2473
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA256::HashData

  .method /*06000149*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2476
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA256::HashFinal

  .method /*0600014A*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2479
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA256::Initialize

  .method /*0600014B*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 04 02 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 10 08
  {
    .custom /*0C00015B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x247b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA256::TryHashData

  .method /*0600014C*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C00015C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x247e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA256::TryHashFinal

  .property /*17000021*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.HMACSHA256/*02000020*/::get_Key() /* 06000141 */
    .set instance void System.Security.Cryptography.HMACSHA256/*02000020*/::set_Key(uint8[]) /* 06000142 */
  } // end of property HMACSHA256::Key
} // end of class System.Security.Cryptography.HMACSHA256

.class /*02000021*/ public auto ansi beforefieldinit System.Security.Cryptography.HMACSHA384
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/
{
  .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000080:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600014D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2481
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA384::.ctor

  .method /*0600014E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2489
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA384::.ctor

  .method /*0600014F*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2491
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA384::get_Key

  .method /*06000150*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2494
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA384::set_Key

  .method /*06000151*/ public hidebysig specialname 
          instance bool  get_ProduceLegacyHmacValues() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2496
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA384::get_ProduceLegacyHmacValues

  .method /*06000152*/ public hidebysig specialname 
          instance void  set_ProduceLegacyHmacValues(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2499
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA384::set_ProduceLegacyHmacValues

  .method /*06000153*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x249b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA384::Dispose

  .method /*06000154*/ family hidebysig virtual 
          instance void  HashCore(uint8[] rgb,
                                  int32 ib,
                                  int32 cb) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x249d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA384::HashCore

  .method /*06000155*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    .custom /*0C00015F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x249f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA384::HashCore

  .method /*06000156*/ public hidebysig static 
          uint8[]  HashData(uint8[] key,
                            uint8[] source) cil managed
  // SIG: 00 02 1D 05 1D 05 1D 05
  {
    // Method begins at RVA 0x24a1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA384::HashData

  .method /*06000157*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 02 1D 05 15 11 69 01 05 15 11 69 01 05
  {
    .custom /*0C000160:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000262*/ 
    .custom /*0C0001CB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24a4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA384::HashData

  .method /*06000158*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 03 08 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05
  {
    .custom /*0C000161:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA384::HashData

  .method /*06000159*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x24aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA384::HashFinal

  .method /*0600015A*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24ad
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA384::Initialize

  .method /*0600015B*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 04 02 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 10 08
  {
    .custom /*0C000162:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24af
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA384::TryHashData

  .method /*0600015C*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C000163:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA384::TryHashFinal

  .property /*17000022*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.HMACSHA384/*02000021*/::get_Key() /* 0600014F */
    .set instance void System.Security.Cryptography.HMACSHA384/*02000021*/::set_Key(uint8[]) /* 06000150 */
  } // end of property HMACSHA384::Key
  .property /*17000023*/ instance callconv(8) bool
          ProduceLegacyHmacValues()
  {
    .custom /*0C000084:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 53 50 72 6F 64 75 63 65 4C 65 67 61 63 79   // ..SProduceLegacy
                                                                                                                                                   48 6D 61 63 56 61 6C 75 65 73 20 69 73 20 6F 62   // HmacValues is ob
                                                                                                                                                   73 6F 6C 65 74 65 2E 20 50 72 6F 64 75 63 69 6E   // solete. Producin
                                                                                                                                                   67 20 6C 65 67 61 63 79 20 48 4D 41 43 20 76 61   // g legacy HMAC va
                                                                                                                                                   6C 75 65 73 20 69 73 20 6E 6F 74 20 73 75 70 70   // lues is not supp
                                                                                                                                                   6F 72 74 65 64 2E 02 00 54 0E 0C 44 69 61 67 6E   // orted...T..Diagn
                                                                                                                                                   6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30   // osticId.SYSLIB00
                                                                                                                                                   32 39 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68   // 29T..UrlFormat"h
                                                                                                                                                   74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F   // ttps://aka.ms/do
                                                                                                                                                   74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30   // tnet-warnings/{0
                                                                                                                                                   7D )                                              // }
    .get instance bool System.Security.Cryptography.HMACSHA384/*02000021*/::get_ProduceLegacyHmacValues() /* 06000151 */
    .set instance void System.Security.Cryptography.HMACSHA384/*02000021*/::set_ProduceLegacyHmacValues(bool) /* 06000152 */
  } // end of property HMACSHA384::ProduceLegacyHmacValues
} // end of class System.Security.Cryptography.HMACSHA384

.class /*02000022*/ public auto ansi beforefieldinit System.Security.Cryptography.HMACSHA512
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/
{
  .custom /*0C000081:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000082:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000083:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600015D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24b5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA512::.ctor

  .method /*0600015E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] key) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x24bd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HMAC/*0100002E*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method HMACSHA512::.ctor

  .method /*0600015F*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x24c5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA512::get_Key

  .method /*06000160*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x24c8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA512::set_Key

  .method /*06000161*/ public hidebysig specialname 
          instance bool  get_ProduceLegacyHmacValues() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x24ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA512::get_ProduceLegacyHmacValues

  .method /*06000162*/ public hidebysig specialname 
          instance void  set_ProduceLegacyHmacValues(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24cd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA512::set_ProduceLegacyHmacValues

  .method /*06000163*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x24cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA512::Dispose

  .method /*06000164*/ family hidebysig virtual 
          instance void  HashCore(uint8[] rgb,
                                  int32 ib,
                                  int32 cb) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x24d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA512::HashCore

  .method /*06000165*/ family hidebysig virtual 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    .custom /*0C000165:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24d3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA512::HashCore

  .method /*06000166*/ public hidebysig static 
          uint8[]  HashData(uint8[] key,
                            uint8[] source) cil managed
  // SIG: 00 02 1D 05 1D 05 1D 05
  {
    // Method begins at RVA 0x24d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA512::HashData

  .method /*06000167*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 02 1D 05 15 11 69 01 05 15 11 69 01 05
  {
    .custom /*0C000166:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*08000278*/ 
    .custom /*0C0001D4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA512::HashData

  .method /*06000168*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 03 08 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05
  {
    .custom /*0C000167:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA512::HashData

  .method /*06000169*/ family hidebysig virtual 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x24de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA512::HashFinal

  .method /*0600016A*/ public hidebysig virtual 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24e1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method HMACSHA512::Initialize

  .method /*0600016B*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key,
                            valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 04 02 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 10 08
  {
    .custom /*0C000168:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA512::TryHashData

  .method /*0600016C*/ family hidebysig virtual 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C000169:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x24e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method HMACSHA512::TryHashFinal

  .property /*17000024*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.HMACSHA512/*02000022*/::get_Key() /* 0600015F */
    .set instance void System.Security.Cryptography.HMACSHA512/*02000022*/::set_Key(uint8[]) /* 06000160 */
  } // end of property HMACSHA512::Key
  .property /*17000025*/ instance callconv(8) bool
          ProduceLegacyHmacValues()
  {
    .custom /*0C000087:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 53 50 72 6F 64 75 63 65 4C 65 67 61 63 79   // ..SProduceLegacy
                                                                                                                                                   48 6D 61 63 56 61 6C 75 65 73 20 69 73 20 6F 62   // HmacValues is ob
                                                                                                                                                   73 6F 6C 65 74 65 2E 20 50 72 6F 64 75 63 69 6E   // solete. Producin
                                                                                                                                                   67 20 6C 65 67 61 63 79 20 48 4D 41 43 20 76 61   // g legacy HMAC va
                                                                                                                                                   6C 75 65 73 20 69 73 20 6E 6F 74 20 73 75 70 70   // lues is not supp
                                                                                                                                                   6F 72 74 65 64 2E 02 00 54 0E 0C 44 69 61 67 6E   // orted...T..Diagn
                                                                                                                                                   6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30   // osticId.SYSLIB00
                                                                                                                                                   32 39 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68   // 29T..UrlFormat"h
                                                                                                                                                   74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F   // ttps://aka.ms/do
                                                                                                                                                   74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30   // tnet-warnings/{0
                                                                                                                                                   7D )                                              // }
    .get instance bool System.Security.Cryptography.HMACSHA512/*02000022*/::get_ProduceLegacyHmacValues() /* 06000161 */
    .set instance void System.Security.Cryptography.HMACSHA512/*02000022*/::set_ProduceLegacyHmacValues(bool) /* 06000162 */
  } // end of property HMACSHA512::ProduceLegacyHmacValues
} // end of class System.Security.Cryptography.HMACSHA512

.class /*02000023*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.IncrementalHash
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
{
  .method /*0600016D*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x24e9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method IncrementalHash::.ctor

  .method /*0600016E*/ public hidebysig specialname 
          instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ 
          get_AlgorithmName() cil managed
  // SIG: 20 00 11 80 9D
  {
    // Method begins at RVA 0x24f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::get_AlgorithmName

  .method /*0600016F*/ public hidebysig specialname 
          instance int32  get_HashLengthInBytes() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x24f4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::get_HashLengthInBytes

  .method /*06000170*/ public hidebysig instance void 
          AppendData(uint8[] data) cil managed
  // SIG: 20 01 01 1D 05
  {
    .custom /*0C00016A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method IncrementalHash::AppendData

  .method /*06000171*/ public hidebysig instance void 
          AppendData(uint8[] data,
                     int32 offset,
                     int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C00016B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method IncrementalHash::AppendData

  .method /*06000172*/ public hidebysig instance void 
          AppendData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    // Method begins at RVA 0x24fb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method IncrementalHash::AppendData

  .method /*06000173*/ public hidebysig static 
          class System.Security.Cryptography.IncrementalHash/*02000023*/ 
          CreateHash(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 00 01 12 80 8C 11 80 9D
  {
    .custom /*0C00016C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x24fd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::CreateHash

  .method /*06000174*/ public hidebysig static 
          class System.Security.Cryptography.IncrementalHash/*02000023*/ 
          CreateHMAC(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     uint8[] key) cil managed
  // SIG: 00 02 12 80 8C 11 80 9D 1D 05
  {
    .custom /*0C00016D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C00016E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    // Method begins at RVA 0x2500
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::CreateHMAC

  .method /*06000175*/ public hidebysig static 
          class System.Security.Cryptography.IncrementalHash/*02000023*/ 
          CreateHMAC(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> key) cil managed
  // SIG: 00 02 12 80 8C 11 80 9D 15 11 69 01 05
  {
    .custom /*0C00016F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .param [0]/*0800028C*/ 
    .custom /*0C0001DC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2503
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::CreateHMAC

  .method /*06000176*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2506
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method IncrementalHash::Dispose

  .method /*06000177*/ public hidebysig instance uint8[] 
          GetCurrentHash() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C000170:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2508
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::GetCurrentHash

  .method /*06000178*/ public hidebysig instance int32 
          GetCurrentHash(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 20 01 08 15 11 71 01 05
  {
    // Method begins at RVA 0x250b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::GetCurrentHash

  .method /*06000179*/ public hidebysig instance uint8[] 
          GetHashAndReset() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C000171:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x250e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::GetHashAndReset

  .method /*0600017A*/ public hidebysig instance int32 
          GetHashAndReset(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 20 01 08 15 11 71 01 05
  {
    // Method begins at RVA 0x2511
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::GetHashAndReset

  .method /*0600017B*/ public hidebysig instance bool 
          TryGetCurrentHash(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2514
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::TryGetCurrentHash

  .method /*0600017C*/ public hidebysig instance bool 
          TryGetHashAndReset(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                             [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2517
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method IncrementalHash::TryGetHashAndReset

  .property /*17000026*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/
          AlgorithmName()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ System.Security.Cryptography.IncrementalHash/*02000023*/::get_AlgorithmName() /* 0600016E */
  } // end of property IncrementalHash::AlgorithmName
  .property /*17000027*/ instance callconv(8) int32
          HashLengthInBytes()
  {
    .get instance int32 System.Security.Cryptography.IncrementalHash/*02000023*/::get_HashLengthInBytes() /* 0600016F */
  } // end of property IncrementalHash::HashLengthInBytes
} // end of class System.Security.Cryptography.IncrementalHash

.class /*02000024*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.MaskGenerationMethod
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C000085:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600017D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x251a
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MaskGenerationMethod::.ctor

  .method /*0600017E*/ public hidebysig newslot abstract virtual 
          instance uint8[]  GenerateMask(uint8[] rgbSeed,
                                         int32 cbReturn) cil managed
  // SIG: 20 02 1D 05 1D 05 08
  {
    .custom /*0C000172:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x0
  } // end of method MaskGenerationMethod::GenerateMask

} // end of class System.Security.Cryptography.MaskGenerationMethod

.class /*02000025*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.MD5
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/
{
  .custom /*0C000086:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600017F*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2522
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MD5::.ctor

  .method /*06000180*/ public hidebysig static 
          class System.Security.Cryptography.MD5/*02000025*/ 
          Create() cil managed
  // SIG: 00 00 12 80 94
  {
    .custom /*0C000173:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x252a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MD5::Create

  .method /*06000181*/ public hidebysig static 
          class System.Security.Cryptography.MD5/*02000025*/ 
          Create(string algName) cil managed
  // SIG: 00 01 12 80 94 0E
  {
    .custom /*0C000174:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C000175:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*08000297*/ 
    .custom /*0C0001DE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x252d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MD5::Create

  .method /*06000182*/ public hidebysig static 
          uint8[]  HashData(uint8[] source) cil managed
  // SIG: 00 01 1D 05 1D 05
  {
    .custom /*0C000176:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2530
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MD5::HashData

  .method /*06000183*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 01 1D 05 15 11 69 01 05
  {
    .param [0]/*0800029A*/ 
    .custom /*0C0001DF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2533
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MD5::HashData

  .method /*06000184*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 02 08 15 11 69 01 05 15 11 71 01 05
  {
    // Method begins at RVA 0x2536
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MD5::HashData

  .method /*06000185*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 69 01 05 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2539
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MD5::TryHashData

} // end of class System.Security.Cryptography.MD5

.class /*02000026*/ public auto ansi beforefieldinit System.Security.Cryptography.PKCS1MaskGenerationMethod
       extends System.Security.Cryptography.MaskGenerationMethod/*02000024*/
{
  .custom /*0C000088:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00008A:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000186*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    .custom /*0C000177:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 7E 50 4B 43 53 31 4D 61 73 6B 47 65 6E 65   // ..~PKCS1MaskGene
                                                                                                                                                                                            72 61 74 69 6F 6E 4D 65 74 68 6F 64 20 69 73 20   // rationMethod is 
                                                                                                                                                                                            6E 6F 74 20 74 72 69 6D 20 63 6F 6D 70 61 74 69   // not trim compati
                                                                                                                                                                                            62 6C 65 20 62 65 63 61 75 73 65 20 74 68 65 20   // ble because the 
                                                                                                                                                                                            61 6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D   // algorithm implem
                                                                                                                                                                                            65 6E 74 61 74 69 6F 6E 20 72 65 66 65 72 65 6E   // entation referen
                                                                                                                                                                                            63 65 64 20 62 79 20 48 61 73 68 4E 61 6D 65 20   // ced by HashName 
                                                                                                                                                                                            6D 69 67 68 74 20 62 65 20 72 65 6D 6F 76 65 64   // might be removed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    // Method begins at RVA 0x253c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00017D       */ call       instance void System.Security.Cryptography.MaskGenerationMethod/*02000024*/::.ctor() /* 0600017D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method PKCS1MaskGenerationMethod::.ctor

  .method /*06000187*/ public hidebysig specialname 
          instance string  get_HashName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2544
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PKCS1MaskGenerationMethod::get_HashName

  .method /*06000188*/ public hidebysig specialname 
          instance void  set_HashName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2547
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method PKCS1MaskGenerationMethod::set_HashName

  .method /*06000189*/ public hidebysig virtual 
          instance uint8[]  GenerateMask(uint8[] rgbSeed,
                                         int32 cbReturn) cil managed
  // SIG: 20 02 1D 05 1D 05 08
  {
    // Method begins at RVA 0x2549
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method PKCS1MaskGenerationMethod::GenerateMask

  .property /*17000028*/ instance callconv(8) string
          HashName()
  {
    .get instance string System.Security.Cryptography.PKCS1MaskGenerationMethod/*02000026*/::get_HashName() /* 06000187 */
    .set instance void System.Security.Cryptography.PKCS1MaskGenerationMethod/*02000026*/::set_HashName(string) /* 06000188 */
  } // end of property PKCS1MaskGenerationMethod::HashName
} // end of class System.Security.Cryptography.PKCS1MaskGenerationMethod

.class /*02000027*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.RandomNumberGenerator
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
{
  .custom /*0C00008B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*01000019*/
  .custom /*0C00002C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600018A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x254c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RandomNumberGenerator::.ctor

  .method /*0600018B*/ public hidebysig static 
          class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ 
          Create() cil managed
  // SIG: 00 00 12 80 9C
  {
    // Method begins at RVA 0x2554
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RandomNumberGenerator::Create

  .method /*0600018C*/ public hidebysig static 
          class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ 
          Create(string rngName) cil managed
  // SIG: 00 01 12 80 9C 0E
  {
    .custom /*0C000178:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .custom /*0C000179:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
    .param [0]/*080002A4*/ 
    .custom /*0C0001E0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2557
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RandomNumberGenerator::Create

  .method /*0600018D*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x255a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RandomNumberGenerator::Dispose

  .method /*0600018E*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x255c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RandomNumberGenerator::Dispose

  .method /*0600018F*/ public hidebysig static 
          void  Fill(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> data) cil managed
  // SIG: 00 01 01 15 11 71 01 05
  {
    .custom /*0C00017A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x255e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RandomNumberGenerator::Fill

  .method /*06000190*/ public hidebysig newslot abstract virtual 
          instance void  GetBytes(uint8[] data) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x0
  } // end of method RandomNumberGenerator::GetBytes

  .method /*06000191*/ public hidebysig newslot virtual 
          instance void  GetBytes(uint8[] data,
                                  int32 offset,
                                  int32 count) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    // Method begins at RVA 0x2560
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RandomNumberGenerator::GetBytes

  .method /*06000192*/ public hidebysig static 
          uint8[]  GetBytes(int32 count) cil managed
  // SIG: 00 01 1D 05 08
  {
    // Method begins at RVA 0x2562
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RandomNumberGenerator::GetBytes

  .method /*06000193*/ public hidebysig newslot virtual 
          instance void  GetBytes(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> data) cil managed
  // SIG: 20 01 01 15 11 71 01 05
  {
    .custom /*0C00017B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2565
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RandomNumberGenerator::GetBytes

  .method /*06000194*/ public hidebysig static 
          int32  GetInt32(int32 toExclusive) cil managed
  // SIG: 00 01 08 08
  {
    // Method begins at RVA 0x2567
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RandomNumberGenerator::GetInt32

  .method /*06000195*/ public hidebysig static 
          int32  GetInt32(int32 fromInclusive,
                          int32 toExclusive) cil managed
  // SIG: 00 02 08 08 08
  {
    // Method begins at RVA 0x256a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RandomNumberGenerator::GetInt32

  .method /*06000196*/ public hidebysig newslot virtual 
          instance void  GetNonZeroBytes(uint8[] data) cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x256d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RandomNumberGenerator::GetNonZeroBytes

  .method /*06000197*/ public hidebysig newslot virtual 
          instance void  GetNonZeroBytes(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> data) cil managed
  // SIG: 20 01 01 15 11 71 01 05
  {
    .custom /*0C00017D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x256f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RandomNumberGenerator::GetNonZeroBytes

} // end of class System.Security.Cryptography.RandomNumberGenerator

.class /*02000028*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.RC2
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/
{
  .custom /*0C00008D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00008E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00008F:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000090:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .field /*04000020*/ family int32 EffectiveKeySizeValue
  .method /*06000198*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2571
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RC2::.ctor

  .method /*06000199*/ public hidebysig newslot specialname virtual 
          instance int32  get_EffectiveKeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2579
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RC2::get_EffectiveKeySize

  .method /*0600019A*/ public hidebysig newslot specialname virtual 
          instance void  set_EffectiveKeySize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x257c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RC2::set_EffectiveKeySize

  .method /*0600019B*/ public hidebysig specialname virtual 
          instance int32  get_KeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x257e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RC2::get_KeySize

  .method /*0600019C*/ public hidebysig specialname virtual 
          instance void  set_KeySize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2581
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RC2::set_KeySize

  .method /*0600019D*/ public hidebysig static 
          class System.Security.Cryptography.RC2/*02000028*/ 
          Create() cil managed
  // SIG: 00 00 12 80 A0
  {
    .custom /*0C00017F:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 61 6E 64 72 6F 69 64 00 00 )             // ...android..
    // Method begins at RVA 0x2583
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RC2::Create

  .method /*0600019E*/ public hidebysig static 
          class System.Security.Cryptography.RC2/*02000028*/ 
          Create(string AlgName) cil managed
  // SIG: 00 01 12 80 A0 0E
  {
    .custom /*0C000180:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*080002B5*/ 
    .custom /*0C0001E1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2586
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RC2::Create

  .property /*17000029*/ instance callconv(8) int32
          EffectiveKeySize()
  {
    .get instance int32 System.Security.Cryptography.RC2/*02000028*/::get_EffectiveKeySize() /* 06000199 */
    .set instance void System.Security.Cryptography.RC2/*02000028*/::set_EffectiveKeySize(int32) /* 0600019A */
  } // end of property RC2::EffectiveKeySize
  .property /*1700002A*/ instance callconv(8) int32
          KeySize()
  {
    .get instance int32 System.Security.Cryptography.RC2/*02000028*/::get_KeySize() /* 0600019B */
    .set instance void System.Security.Cryptography.RC2/*02000028*/::set_KeySize(int32) /* 0600019C */
  } // end of property RC2::KeySize
} // end of class System.Security.Cryptography.RC2

.class /*02000029*/ public auto ansi beforefieldinit System.Security.Cryptography.Rfc2898DeriveBytes
       extends System.Security.Cryptography.DeriveBytes/*02000010*/
{
  .custom /*0C000091:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000093:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600019F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] password,
                               uint8[] salt,
                               int32 iterations) cil managed
  // SIG: 20 03 01 1D 05 1D 05 08
  {
    // Method begins at RVA 0x2589
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000059       */ call       instance void System.Security.Cryptography.DeriveBytes/*02000010*/::.ctor() /* 06000059 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::.ctor

  .method /*060001A0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(uint8[] password,
                               uint8[] salt,
                               int32 iterations,
                               valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 04 01 1D 05 1D 05 08 11 80 9D
  {
    // Method begins at RVA 0x2591
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000059       */ call       instance void System.Security.Cryptography.DeriveBytes/*02000010*/::.ctor() /* 06000059 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::.ctor

  .method /*060001A1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string password,
                               uint8[] salt) cil managed
  // SIG: 20 02 01 0E 1D 05
  {
    // Method begins at RVA 0x2599
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000059       */ call       instance void System.Security.Cryptography.DeriveBytes/*02000010*/::.ctor() /* 06000059 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::.ctor

  .method /*060001A2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string password,
                               uint8[] salt,
                               int32 iterations) cil managed
  // SIG: 20 03 01 0E 1D 05 08
  {
    // Method begins at RVA 0x25a1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000059       */ call       instance void System.Security.Cryptography.DeriveBytes/*02000010*/::.ctor() /* 06000059 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::.ctor

  .method /*060001A3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string password,
                               uint8[] salt,
                               int32 iterations,
                               valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 04 01 0E 1D 05 08 11 80 9D
  {
    // Method begins at RVA 0x25a9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000059       */ call       instance void System.Security.Cryptography.DeriveBytes/*02000010*/::.ctor() /* 06000059 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::.ctor

  .method /*060001A4*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string password,
                               int32 saltSize) cil managed
  // SIG: 20 02 01 0E 08
  {
    // Method begins at RVA 0x25b1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000059       */ call       instance void System.Security.Cryptography.DeriveBytes/*02000010*/::.ctor() /* 06000059 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::.ctor

  .method /*060001A5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string password,
                               int32 saltSize,
                               int32 iterations) cil managed
  // SIG: 20 03 01 0E 08 08
  {
    // Method begins at RVA 0x25b9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000059       */ call       instance void System.Security.Cryptography.DeriveBytes/*02000010*/::.ctor() /* 06000059 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::.ctor

  .method /*060001A6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string password,
                               int32 saltSize,
                               int32 iterations,
                               valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 04 01 0E 08 08 11 80 9D
  {
    // Method begins at RVA 0x25c1
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000059       */ call       instance void System.Security.Cryptography.DeriveBytes/*02000010*/::.ctor() /* 06000059 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::.ctor

  .method /*060001A7*/ public hidebysig specialname 
          instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ 
          get_HashAlgorithm() cil managed
  // SIG: 20 00 11 80 9D
  {
    // Method begins at RVA 0x25c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::get_HashAlgorithm

  .method /*060001A8*/ public hidebysig specialname 
          instance int32  get_IterationCount() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x25cc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::get_IterationCount

  .method /*060001A9*/ public hidebysig specialname 
          instance void  set_IterationCount(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x25cf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::set_IterationCount

  .method /*060001AA*/ public hidebysig specialname 
          instance uint8[]  get_Salt() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x25d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::get_Salt

  .method /*060001AB*/ public hidebysig specialname 
          instance void  set_Salt(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x25d4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::set_Salt

  .method /*060001AC*/ public hidebysig instance uint8[] 
          CryptDeriveKey(string algname,
                         string alghashname,
                         int32 keySize,
                         uint8[] rgbIV) cil managed
  // SIG: 20 04 1D 05 0E 0E 08 1D 05
  {
    .custom /*0C000183:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 73 52 66 63 32 38 39 38 44 65 72 69 76 65   // ..sRfc2898Derive
                                                                                                                                                   42 79 74 65 73 2E 43 72 79 70 74 44 65 72 69 76   // Bytes.CryptDeriv
                                                                                                                                                   65 4B 65 79 20 69 73 20 6F 62 73 6F 6C 65 74 65   // eKey is obsolete
                                                                                                                                                   20 61 6E 64 20 69 73 20 6E 6F 74 20 73 75 70 70   //  and is not supp
                                                                                                                                                   6F 72 74 65 64 2E 20 55 73 65 20 50 61 73 73 77   // orted. Use Passw
                                                                                                                                                   6F 72 64 44 65 72 69 76 65 42 79 74 65 73 2E 43   // ordDeriveBytes.C
                                                                                                                                                   72 79 70 74 44 65 72 69 76 65 4B 65 79 20 69 6E   // ryptDeriveKey in
                                                                                                                                                   73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61 67 6E   // stead...T..Diagn
                                                                                                                                                   6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42 30 30   // osticId.SYSLIB00
                                                                                                                                                   33 33 54 0E 09 55 72 6C 46 6F 72 6D 61 74 22 68   // 33T..UrlFormat"h
                                                                                                                                                   74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F 64 6F   // ttps://aka.ms/do
                                                                                                                                                   74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F 7B 30   // tnet-warnings/{0
                                                                                                                                                   7D )                                              // }
    // Method begins at RVA 0x25d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::CryptDeriveKey

  .method /*060001AD*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x25d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::Dispose

  .method /*060001AE*/ public hidebysig virtual 
          instance uint8[]  GetBytes(int32 cb) cil managed
  // SIG: 20 01 1D 05 08
  {
    // Method begins at RVA 0x25db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::GetBytes

  .method /*060001AF*/ public hidebysig static 
          uint8[]  Pbkdf2(uint8[] password,
                          uint8[] salt,
                          int32 iterations,
                          valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                          int32 outputLength) cil managed
  // SIG: 00 05 1D 05 1D 05 1D 05 08 11 80 9D 08
  {
    // Method begins at RVA 0x25de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::Pbkdf2

  .method /*060001B0*/ public hidebysig static 
          uint8[]  Pbkdf2(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> password,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> salt,
                          int32 iterations,
                          valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                          int32 outputLength) cil managed
  // SIG: 00 05 1D 05 15 11 69 01 05 15 11 69 01 05 08 11 80 9D 08
  {
    .custom /*0C000184:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080002DD*/ 
    .custom /*0C0001E2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::Pbkdf2

  .method /*060001B1*/ public hidebysig static 
          void  Pbkdf2(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> password,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> salt,
                       valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                       int32 iterations,
                       valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 00 05 01 15 11 69 01 05 15 11 69 01 05 15 11 71 01 05 08 11 80 9D
  {
    .custom /*0C000185:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25e4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::Pbkdf2

  .method /*060001B2*/ public hidebysig static 
          uint8[]  Pbkdf2(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                          valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> salt,
                          int32 iterations,
                          valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                          int32 outputLength) cil managed
  // SIG: 00 05 1D 05 15 11 69 01 03 15 11 69 01 05 08 11 80 9D 08
  {
    .custom /*0C000186:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [0]/*080002E8*/ 
    .custom /*0C0001E3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x25e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::Pbkdf2

  .method /*060001B3*/ public hidebysig static 
          void  Pbkdf2(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                       valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> salt,
                       valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                       int32 iterations,
                       valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 00 05 01 15 11 69 01 03 15 11 69 01 05 15 11 71 01 05 08 11 80 9D
  {
    .custom /*0C000187:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x25e9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::Pbkdf2

  .method /*060001B4*/ public hidebysig static 
          uint8[]  Pbkdf2(string password,
                          uint8[] salt,
                          int32 iterations,
                          valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                          int32 outputLength) cil managed
  // SIG: 00 05 1D 05 0E 1D 05 08 11 80 9D 08
  {
    // Method begins at RVA 0x25eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rfc2898DeriveBytes::Pbkdf2

  .method /*060001B5*/ public hidebysig virtual 
          instance void  Reset() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x25ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Rfc2898DeriveBytes::Reset

  .property /*1700002B*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/
          HashAlgorithm()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ System.Security.Cryptography.Rfc2898DeriveBytes/*02000029*/::get_HashAlgorithm() /* 060001A7 */
  } // end of property Rfc2898DeriveBytes::HashAlgorithm
  .property /*1700002C*/ instance callconv(8) int32
          IterationCount()
  {
    .get instance int32 System.Security.Cryptography.Rfc2898DeriveBytes/*02000029*/::get_IterationCount() /* 060001A8 */
    .set instance void System.Security.Cryptography.Rfc2898DeriveBytes/*02000029*/::set_IterationCount(int32) /* 060001A9 */
  } // end of property Rfc2898DeriveBytes::IterationCount
  .property /*1700002D*/ instance callconv(8) uint8[]
          Salt()
  {
    .get instance uint8[] System.Security.Cryptography.Rfc2898DeriveBytes/*02000029*/::get_Salt() /* 060001AA */
    .set instance void System.Security.Cryptography.Rfc2898DeriveBytes/*02000029*/::set_Salt(uint8[]) /* 060001AB */
  } // end of property Rfc2898DeriveBytes::Salt
} // end of class System.Security.Cryptography.Rfc2898DeriveBytes

.class /*0200002A*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.Rijndael
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/
{
  .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000096:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000097:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 45 54 68 65 20 52 69 6A 6E 64 61 65 6C 20   // ..EThe Rijndael 
                                                                                                                                                 61 6E 64 20 52 69 6A 6E 64 61 65 6C 4D 61 6E 61   // and RijndaelMana
                                                                                                                                                 67 65 64 20 74 79 70 65 73 20 61 72 65 20 6F 62   // ged types are ob
                                                                                                                                                 73 6F 6C 65 74 65 2E 20 55 73 65 20 41 65 73 20   // solete. Use Aes 
                                                                                                                                                 69 6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61   // instead...T..Dia
                                                                                                                                                 67 6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42   // gnosticId.SYSLIB
                                                                                                                                                 30 30 32 32 54 0E 09 55 72 6C 46 6F 72 6D 61 74   // 0022T..UrlFormat
                                                                                                                                                 22 68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F   // "https://aka.ms/
                                                                                                                                                 64 6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F   // dotnet-warnings/
                                                                                                                                                 7B 30 7D )                                        // {0}
  .custom /*0C000098:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C000099:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*060001B6*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x25f0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method Rijndael::.ctor

  .method /*060001B7*/ public hidebysig static 
          class System.Security.Cryptography.Rijndael/*0200002A*/ 
          Create() cil managed
  // SIG: 00 00 12 80 A8
  {
    // Method begins at RVA 0x25f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rijndael::Create

  .method /*060001B8*/ public hidebysig static 
          class System.Security.Cryptography.Rijndael/*0200002A*/ 
          Create(string algName) cil managed
  // SIG: 00 01 12 80 A8 0E
  {
    .custom /*0C000188:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*080002F8*/ 
    .custom /*0C0001E4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x25fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Rijndael::Create

} // end of class System.Security.Cryptography.Rijndael

.class /*0200002B*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.RijndaelManaged
       extends System.Security.Cryptography.Rijndael/*0200002A*/
{
  .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00009B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C00009C:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 45 54 68 65 20 52 69 6A 6E 64 61 65 6C 20   // ..EThe Rijndael 
                                                                                                                                                 61 6E 64 20 52 69 6A 6E 64 61 65 6C 4D 61 6E 61   // and RijndaelMana
                                                                                                                                                 67 65 64 20 74 79 70 65 73 20 61 72 65 20 6F 62   // ged types are ob
                                                                                                                                                 73 6F 6C 65 74 65 2E 20 55 73 65 20 41 65 73 20   // solete. Use Aes 
                                                                                                                                                 69 6E 73 74 65 61 64 2E 02 00 54 0E 0C 44 69 61   // instead...T..Dia
                                                                                                                                                 67 6E 6F 73 74 69 63 49 64 0A 53 59 53 4C 49 42   // gnosticId.SYSLIB
                                                                                                                                                 30 30 32 32 54 0E 09 55 72 6C 46 6F 72 6D 61 74   // 0022T..UrlFormat
                                                                                                                                                 22 68 74 74 70 73 3A 2F 2F 61 6B 61 2E 6D 73 2F   // "https://aka.ms/
                                                                                                                                                 64 6F 74 6E 65 74 2D 77 61 72 6E 69 6E 67 73 2F   // dotnet-warnings/
                                                                                                                                                 7B 30 7D )                                        // {0}
  .custom /*0C00009D:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .custom /*0C00009E:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*060001B9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x25fe
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0001B6       */ call       instance void System.Security.Cryptography.Rijndael/*0200002A*/::.ctor() /* 060001B6 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::.ctor

  .method /*060001BA*/ public hidebysig specialname virtual 
          instance int32  get_BlockSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2606
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::get_BlockSize

  .method /*060001BB*/ public hidebysig specialname virtual 
          instance void  set_BlockSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2609
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::set_BlockSize

  .method /*060001BC*/ public hidebysig specialname virtual 
          instance int32  get_FeedbackSize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x260b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::get_FeedbackSize

  .method /*060001BD*/ public hidebysig specialname virtual 
          instance void  set_FeedbackSize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x260e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::set_FeedbackSize

  .method /*060001BE*/ public hidebysig specialname virtual 
          instance uint8[]  get_IV() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2610
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::get_IV

  .method /*060001BF*/ public hidebysig specialname virtual 
          instance void  set_IV(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2613
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::set_IV

  .method /*060001C0*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2615
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::get_Key

  .method /*060001C1*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x2618
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::set_Key

  .method /*060001C2*/ public hidebysig specialname virtual 
          instance int32  get_KeySize() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x261a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::get_KeySize

  .method /*060001C3*/ public hidebysig specialname virtual 
          instance void  set_KeySize(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x261d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::set_KeySize

  .method /*060001C4*/ public hidebysig specialname virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[] 
          get_LegalKeySizes() cil managed
  // SIG: 20 00 1D 12 6D
  {
    // Method begins at RVA 0x261f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::get_LegalKeySizes

  .method /*060001C5*/ public hidebysig specialname virtual 
          instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 80 81
  {
    // Method begins at RVA 0x2622
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::get_Mode

  .method /*060001C6*/ public hidebysig specialname virtual 
          instance void  set_Mode(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/ 'value') cil managed
  // SIG: 20 01 01 11 80 81
  {
    // Method begins at RVA 0x2625
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::set_Mode

  .method /*060001C7*/ public hidebysig specialname virtual 
          instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/ 
          get_Padding() cil managed
  // SIG: 20 00 11 80 85
  {
    // Method begins at RVA 0x2627
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::get_Padding

  .method /*060001C8*/ public hidebysig specialname virtual 
          instance void  set_Padding(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/ 'value') cil managed
  // SIG: 20 01 01 11 80 85
  {
    // Method begins at RVA 0x262a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::set_Padding

  .method /*060001C9*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.ICryptoTransform/*01000022*/ 
          CreateDecryptor() cil managed
  // SIG: 20 00 12 80 89
  {
    // Method begins at RVA 0x262c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::CreateDecryptor

  .method /*060001CA*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.ICryptoTransform/*01000022*/ 
          CreateDecryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 80 89 1D 05 1D 05
  {
    .param [2]/*08000302*/ 
    .custom /*0C0001E5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x262f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::CreateDecryptor

  .method /*060001CB*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.ICryptoTransform/*01000022*/ 
          CreateEncryptor() cil managed
  // SIG: 20 00 12 80 89
  {
    // Method begins at RVA 0x2632
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::CreateEncryptor

  .method /*060001CC*/ public hidebysig virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.ICryptoTransform/*01000022*/ 
          CreateEncryptor(uint8[] rgbKey,
                          uint8[] rgbIV) cil managed
  // SIG: 20 02 12 80 89 1D 05 1D 05
  {
    .param [2]/*08000304*/ 
    .custom /*0C0001E6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2635
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RijndaelManaged::CreateEncryptor

  .method /*060001CD*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2638
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::Dispose

  .method /*060001CE*/ public hidebysig virtual 
          instance void  GenerateIV() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x263a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::GenerateIV

  .method /*060001CF*/ public hidebysig virtual 
          instance void  GenerateKey() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x263c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RijndaelManaged::GenerateKey

  .property /*1700002E*/ instance callconv(8) int32
          BlockSize()
  {
    .get instance int32 System.Security.Cryptography.RijndaelManaged/*0200002B*/::get_BlockSize() /* 060001BA */
    .set instance void System.Security.Cryptography.RijndaelManaged/*0200002B*/::set_BlockSize(int32) /* 060001BB */
  } // end of property RijndaelManaged::BlockSize
  .property /*1700002F*/ instance callconv(8) int32
          FeedbackSize()
  {
    .get instance int32 System.Security.Cryptography.RijndaelManaged/*0200002B*/::get_FeedbackSize() /* 060001BC */
    .set instance void System.Security.Cryptography.RijndaelManaged/*0200002B*/::set_FeedbackSize(int32) /* 060001BD */
  } // end of property RijndaelManaged::FeedbackSize
  .property /*17000030*/ instance callconv(8) uint8[]
          IV()
  {
    .get instance uint8[] System.Security.Cryptography.RijndaelManaged/*0200002B*/::get_IV() /* 060001BE */
    .set instance void System.Security.Cryptography.RijndaelManaged/*0200002B*/::set_IV(uint8[]) /* 060001BF */
  } // end of property RijndaelManaged::IV
  .property /*17000031*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.RijndaelManaged/*0200002B*/::get_Key() /* 060001C0 */
    .set instance void System.Security.Cryptography.RijndaelManaged/*0200002B*/::set_Key(uint8[]) /* 060001C1 */
  } // end of property RijndaelManaged::Key
  .property /*17000032*/ instance callconv(8) int32
          KeySize()
  {
    .get instance int32 System.Security.Cryptography.RijndaelManaged/*0200002B*/::get_KeySize() /* 060001C2 */
    .set instance void System.Security.Cryptography.RijndaelManaged/*0200002B*/::set_KeySize(int32) /* 060001C3 */
  } // end of property RijndaelManaged::KeySize
  .property /*17000033*/ instance callconv(8) class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[]
          LegalKeySizes()
  {
    .get instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.KeySizes/*0100001B*/[] System.Security.Cryptography.RijndaelManaged/*0200002B*/::get_LegalKeySizes() /* 060001C4 */
  } // end of property RijndaelManaged::LegalKeySizes
  .property /*17000034*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/
          Mode()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/ System.Security.Cryptography.RijndaelManaged/*0200002B*/::get_Mode() /* 060001C5 */
    .set instance void System.Security.Cryptography.RijndaelManaged/*0200002B*/::set_Mode(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.CipherMode/*01000020*/) /* 060001C6 */
  } // end of property RijndaelManaged::Mode
  .property /*17000035*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/
          Padding()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/ System.Security.Cryptography.RijndaelManaged/*0200002B*/::get_Padding() /* 060001C7 */
    .set instance void System.Security.Cryptography.RijndaelManaged/*0200002B*/::set_Padding(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PaddingMode/*01000021*/) /* 060001C8 */
  } // end of property RijndaelManaged::Padding
} // end of class System.Security.Cryptography.RijndaelManaged

.class /*0200002C*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.RSA
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/
{
  .custom /*0C00009F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000A1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*060001D0*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x263e
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSA::.ctor

  .method /*060001D1*/ public hidebysig specialname virtual 
          instance string  get_KeyExchangeAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000189:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2646
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::get_KeyExchangeAlgorithm

  .method /*060001D2*/ public hidebysig specialname virtual 
          instance string  get_SignatureAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2649
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::get_SignatureAlgorithm

  .method /*060001D3*/ public hidebysig static 
          class System.Security.Cryptography.RSA/*0200002C*/ 
          Create() cil managed
  // SIG: 00 00 12 80 B0
  {
    // Method begins at RVA 0x264c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::Create

  .method /*060001D4*/ public hidebysig static 
          class System.Security.Cryptography.RSA/*0200002C*/ 
          Create(int32 keySizeInBits) cil managed
  // SIG: 00 01 12 80 B0 08
  {
    // Method begins at RVA 0x264f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::Create

  .method /*060001D5*/ public hidebysig static 
          class System.Security.Cryptography.RSA/*0200002C*/ 
          Create(valuetype System.Security.Cryptography.RSAParameters/*02000031*/ parameters) cil managed
  // SIG: 00 01 12 80 B0 11 80 C4
  {
    // Method begins at RVA 0x2652
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::Create

  .method /*060001D6*/ public hidebysig static 
          class System.Security.Cryptography.RSA/*0200002C*/ 
          Create(string algName) cil managed
  // SIG: 00 01 12 80 B0 0E
  {
    .custom /*0C00018A:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*08000308*/ 
    .custom /*0C0001E7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2655
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::Create

  .method /*060001D7*/ public hidebysig newslot virtual 
          instance uint8[]  Decrypt(uint8[] data,
                                    class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ padding) cil managed
  // SIG: 20 02 1D 05 1D 05 12 80 B4
  {
    // Method begins at RVA 0x2658
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::Decrypt

  .method /*060001D8*/ public hidebysig newslot virtual 
          instance uint8[]  DecryptValue(uint8[] rgb) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x265b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::DecryptValue

  .method /*060001D9*/ public hidebysig newslot virtual 
          instance uint8[]  Encrypt(uint8[] data,
                                    class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ padding) cil managed
  // SIG: 20 02 1D 05 1D 05 12 80 B4
  {
    // Method begins at RVA 0x265e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::Encrypt

  .method /*060001DA*/ public hidebysig newslot virtual 
          instance uint8[]  EncryptValue(uint8[] rgb) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x2661
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::EncryptValue

  .method /*060001DB*/ public hidebysig newslot abstract virtual 
          instance valuetype System.Security.Cryptography.RSAParameters/*02000031*/ 
          ExportParameters(bool includePrivateParameters) cil managed
  // SIG: 20 01 11 80 C4 02
  {
    // Method begins at RVA 0x0
  } // end of method RSA::ExportParameters

  .method /*060001DC*/ public hidebysig newslot virtual 
          instance uint8[]  ExportRSAPrivateKey() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2664
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ExportRSAPrivateKey

  .method /*060001DD*/ public hidebysig newslot virtual 
          instance uint8[]  ExportRSAPublicKey() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2667
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ExportRSAPublicKey

  .method /*060001DE*/ public hidebysig virtual 
          instance void  FromXmlString(string xmlString) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x266a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSA::FromXmlString

  .method /*060001DF*/ family hidebysig newslot virtual 
          instance uint8[]  HashData(uint8[] data,
                                     int32 offset,
                                     int32 count,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 04 1D 05 1D 05 08 08 11 80 9D
  {
    // Method begins at RVA 0x266c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::HashData

  .method /*060001E0*/ family hidebysig newslot virtual 
          instance uint8[]  HashData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 20 02 1D 05 12 80 A1 11 80 9D
  {
    // Method begins at RVA 0x266f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::HashData

  .method /*060001E1*/ public hidebysig virtual 
          instance void  ImportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes,
                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                        [out] int32& bytesRead) cil managed
  // SIG: 20 03 01 15 11 69 01 05 15 11 69 01 05 10 08
  {
    .custom /*0C00018B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2672
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ImportEncryptedPkcs8PrivateKey

  .method /*060001E2*/ public hidebysig virtual 
          instance void  ImportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                                                        valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                        [out] int32& bytesRead) cil managed
  // SIG: 20 03 01 15 11 69 01 03 15 11 69 01 05 10 08
  {
    .custom /*0C00018C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2675
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ImportEncryptedPkcs8PrivateKey

  .method /*060001E3*/ public hidebysig virtual 
          instance void  ImportFromEncryptedPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes) cil managed
  // SIG: 20 02 01 15 11 69 01 03 15 11 69 01 05
  {
    .custom /*0C00018D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2678
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSA::ImportFromEncryptedPem

  .method /*060001E4*/ public hidebysig virtual 
          instance void  ImportFromEncryptedPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input,
                                                valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password) cil managed
  // SIG: 20 02 01 15 11 69 01 03 15 11 69 01 03
  {
    .custom /*0C00018E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x267a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSA::ImportFromEncryptedPem

  .method /*060001E5*/ public hidebysig virtual 
          instance void  ImportFromPem(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> input) cil managed
  // SIG: 20 01 01 15 11 69 01 03
  {
    .custom /*0C00018F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x267c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSA::ImportFromPem

  .method /*060001E6*/ public hidebysig newslot abstract virtual 
          instance void  ImportParameters(valuetype System.Security.Cryptography.RSAParameters/*02000031*/ parameters) cil managed
  // SIG: 20 01 01 11 80 C4
  {
    // Method begins at RVA 0x0
  } // end of method RSA::ImportParameters

  .method /*060001E7*/ public hidebysig virtual 
          instance void  ImportPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                               [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    .custom /*0C000190:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x267e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ImportPkcs8PrivateKey

  .method /*060001E8*/ public hidebysig newslot virtual 
          instance void  ImportRSAPrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                             [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    .custom /*0C000191:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2681
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ImportRSAPrivateKey

  .method /*060001E9*/ public hidebysig newslot virtual 
          instance void  ImportRSAPublicKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                            [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    .custom /*0C000192:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2684
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ImportRSAPublicKey

  .method /*060001EA*/ public hidebysig virtual 
          instance void  ImportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                                                    [out] int32& bytesRead) cil managed
  // SIG: 20 02 01 15 11 69 01 05 10 08
  {
    .custom /*0C000194:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x2687
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ImportSubjectPublicKeyInfo

  .method /*060001EB*/ public hidebysig newslot virtual 
          instance uint8[]  SignData(uint8[] data,
                                     int32 offset,
                                     int32 count,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 05 1D 05 1D 05 08 08 11 80 9D 12 80 D8
  {
    // Method begins at RVA 0x268a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::SignData

  .method /*060001EC*/ public hidebysig instance uint8[] 
          SignData(uint8[] data,
                   valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                   class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 03 1D 05 1D 05 11 80 9D 12 80 D8
  {
    // Method begins at RVA 0x268d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::SignData

  .method /*060001ED*/ public hidebysig newslot virtual 
          instance uint8[]  SignData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 03 1D 05 12 80 A1 11 80 9D 12 80 D8
  {
    // Method begins at RVA 0x2690
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::SignData

  .method /*060001EE*/ public hidebysig newslot virtual 
          instance uint8[]  SignHash(uint8[] hash,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 03 1D 05 1D 05 11 80 9D 12 80 D8
  {
    // Method begins at RVA 0x2693
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::SignHash

  .method /*060001EF*/ public hidebysig virtual 
          instance string  ToXmlString(bool includePrivateParameters) cil managed
  // SIG: 20 01 0E 02
  {
    // Method begins at RVA 0x2696
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::ToXmlString

  .method /*060001F0*/ public hidebysig newslot virtual 
          instance bool  TryDecrypt(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                    valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                    class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ padding,
                                    [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 12 80 B4 10 08
  {
    .custom /*0C000195:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [3]/*0800033D*/ 
    .custom /*0C0001E8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2699
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryDecrypt

  .method /*060001F1*/ public hidebysig newslot virtual 
          instance bool  TryEncrypt(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                    valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                    class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ padding,
                                    [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 12 80 B4 10 08
  {
    .custom /*0C000196:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [3]/*08000341*/ 
    .custom /*0C0001E9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x269c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryEncrypt

  .method /*060001F2*/ public hidebysig virtual 
          instance bool  TryExportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> passwordBytes,
                                                           class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PbeParameters/*01000029*/ pbeParameters,
                                                           valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                           [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 12 80 A5 15 11 71 01 05 10 08
  {
    .custom /*0C000197:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000344*/ 
    .custom /*0C0001EA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x269f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryExportEncryptedPkcs8PrivateKey

  .method /*060001F3*/ public hidebysig virtual 
          instance bool  TryExportEncryptedPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<char> password,
                                                           class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.PbeParameters/*01000029*/ pbeParameters,
                                                           valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                           [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 03 12 80 A5 15 11 71 01 05 10 08
  {
    .custom /*0C000198:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [2]/*08000348*/ 
    .custom /*0C0001EB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryExportEncryptedPkcs8PrivateKey

  .method /*060001F4*/ public hidebysig virtual 
          instance bool  TryExportPkcs8PrivateKey(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                  [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C000199:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x26a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryExportPkcs8PrivateKey

  .method /*060001F5*/ public hidebysig newslot virtual 
          instance bool  TryExportRSAPrivateKey(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C00019A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x26a8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryExportRSAPrivateKey

  .method /*060001F6*/ public hidebysig newslot virtual 
          instance bool  TryExportRSAPublicKey(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                               [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C00019B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x26ab
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryExportRSAPublicKey

  .method /*060001F7*/ public hidebysig virtual 
          instance bool  TryExportSubjectPublicKeyInfo(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                                       [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    .custom /*0C00019C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x26ae
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryExportSubjectPublicKeyInfo

  .method /*060001F8*/ family hidebysig newslot virtual 
          instance bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     [out] int32& bytesWritten) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 10 08
  {
    .custom /*0C00019D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    // Method begins at RVA 0x26b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TryHashData

  .method /*060001F9*/ public hidebysig newslot virtual 
          instance bool  TrySignData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding,
                                     [out] int32& bytesWritten) cil managed
  // SIG: 20 05 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 12 80 D8 10 08
  {
    .custom /*0C00019E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800035A*/ 
    .custom /*0C0001EC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TrySignData

  .method /*060001FA*/ public hidebysig newslot virtual 
          instance bool  TrySignHash(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                     valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                     class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding,
                                     [out] int32& bytesWritten) cil managed
  // SIG: 20 05 02 15 11 69 01 05 15 11 71 01 05 11 80 9D 12 80 D8 10 08
  {
    .custom /*0C00019F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800035F*/ 
    .custom /*0C0001ED:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::TrySignHash

  .method /*060001FB*/ public hidebysig instance bool 
          VerifyData(uint8[] data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 04 02 1D 05 1D 05 11 80 9D 12 80 D8
  {
    // Method begins at RVA 0x26ba
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::VerifyData

  .method /*060001FC*/ public hidebysig newslot virtual 
          instance bool  VerifyData(uint8[] data,
                                    int32 offset,
                                    int32 count,
                                    uint8[] signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                    class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 06 02 1D 05 08 08 1D 05 11 80 9D 12 80 D8
  {
    // Method begins at RVA 0x26bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::VerifyData

  .method /*060001FD*/ public hidebysig instance bool 
          VerifyData(class [System.Runtime/*23000001*/]System.IO.Stream/*01000028*/ data,
                     uint8[] signature,
                     valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                     class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 04 02 12 80 A1 1D 05 11 80 9D 12 80 D8
  {
    // Method begins at RVA 0x26c0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::VerifyData

  .method /*060001FE*/ public hidebysig newslot virtual 
          instance bool  VerifyData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> data,
                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                    class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 69 01 05 11 80 9D 12 80 D8
  {
    .custom /*0C0001A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [4]/*08000372*/ 
    .custom /*0C0001EE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26c3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::VerifyData

  .method /*060001FF*/ public hidebysig newslot virtual 
          instance bool  VerifyHash(uint8[] hash,
                                    uint8[] signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                    class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 04 02 1D 05 1D 05 11 80 9D 12 80 D8
  {
    // Method begins at RVA 0x26c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::VerifyHash

  .method /*06000200*/ public hidebysig newslot virtual 
          instance bool  VerifyHash(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> hash,
                                    valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> signature,
                                    valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm,
                                    class System.Security.Cryptography.RSASignaturePadding/*02000036*/ padding) cil managed
  // SIG: 20 04 02 15 11 69 01 05 15 11 69 01 05 11 80 9D 12 80 D8
  {
    .custom /*0C0001A1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 00 00 00 ) 
    .param [4]/*0800037A*/ 
    .custom /*0C0001EF:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSA::VerifyHash

  .property /*17000036*/ instance callconv(8) string
          KeyExchangeAlgorithm()
  {
    .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Security.Cryptography.RSA/*0200002C*/::get_KeyExchangeAlgorithm() /* 060001D1 */
  } // end of property RSA::KeyExchangeAlgorithm
  .property /*17000037*/ instance callconv(8) string
          SignatureAlgorithm()
  {
    .get instance string System.Security.Cryptography.RSA/*0200002C*/::get_SignatureAlgorithm() /* 060001D2 */
  } // end of property RSA::SignatureAlgorithm
} // end of class System.Security.Cryptography.RSA

.class /*0200002D*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.RSAEncryptionPadding
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/>/*1B000001*/
{
  .custom /*0C0000A2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000A4:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/>/*1B000001*/
  .custom /*0C000033:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*06000201*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26cc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAEncryptionPadding::.ctor

  .method /*06000202*/ public hidebysig specialname 
          instance valuetype System.Security.Cryptography.RSAEncryptionPaddingMode/*0200002E*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 80 B8
  {
    // Method begins at RVA 0x26d4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::get_Mode

  .method /*06000203*/ public hidebysig specialname 
          instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ 
          get_OaepHashAlgorithm() cil managed
  // SIG: 20 00 11 80 9D
  {
    // Method begins at RVA 0x26d7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::get_OaepHashAlgorithm

  .method /*06000204*/ public hidebysig specialname static 
          class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ 
          get_OaepSHA1() cil managed
  // SIG: 00 00 12 80 B4
  {
    // Method begins at RVA 0x26da
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::get_OaepSHA1

  .method /*06000205*/ public hidebysig specialname static 
          class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ 
          get_OaepSHA256() cil managed
  // SIG: 00 00 12 80 B4
  {
    // Method begins at RVA 0x26dd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::get_OaepSHA256

  .method /*06000206*/ public hidebysig specialname static 
          class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ 
          get_OaepSHA384() cil managed
  // SIG: 00 00 12 80 B4
  {
    // Method begins at RVA 0x26e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::get_OaepSHA384

  .method /*06000207*/ public hidebysig specialname static 
          class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ 
          get_OaepSHA512() cil managed
  // SIG: 00 00 12 80 B4
  {
    // Method begins at RVA 0x26e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::get_OaepSHA512

  .method /*06000208*/ public hidebysig specialname static 
          class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ 
          get_Pkcs1() cil managed
  // SIG: 00 00 12 80 B4
  {
    // Method begins at RVA 0x26e6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::get_Pkcs1

  .method /*06000209*/ public hidebysig static 
          class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ 
          CreateOaep(valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ hashAlgorithm) cil managed
  // SIG: 00 01 12 80 B4 11 80 9D
  {
    // Method begins at RVA 0x26e9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::CreateOaep

  .method /*0600020A*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0001A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800037C*/ 
    .custom /*0C0001F0:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::Equals

  .method /*0600020B*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ other) cil managed
  // SIG: 20 01 02 12 80 B4
  {
    .custom /*0C0001A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800037D*/ 
    .custom /*0C0001F1:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x26ef
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::Equals

  .method /*0600020C*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x26f2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::GetHashCode

  .method /*0600020D*/ public hidebysig specialname static 
          bool  op_Equality(class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ left,
                            class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ right) cil managed
  // SIG: 00 02 02 12 80 B4 12 80 B4
  {
    .custom /*0C0001A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::op_Equality

  .method /*0600020E*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ left,
                              class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ right) cil managed
  // SIG: 00 02 02 12 80 B4 12 80 B4
  {
    .custom /*0C0001A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x26f8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::op_Inequality

  .method /*0600020F*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x26fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAEncryptionPadding::ToString

  .property /*17000038*/ instance callconv(8) valuetype System.Security.Cryptography.RSAEncryptionPaddingMode/*0200002E*/
          Mode()
  {
    .get instance valuetype System.Security.Cryptography.RSAEncryptionPaddingMode/*0200002E*/ System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/::get_Mode() /* 06000202 */
  } // end of property RSAEncryptionPadding::Mode
  .property /*17000039*/ instance callconv(8) valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/
          OaepHashAlgorithm()
  {
    .get instance valuetype [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithmName/*01000027*/ System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/::get_OaepHashAlgorithm() /* 06000203 */
  } // end of property RSAEncryptionPadding::OaepHashAlgorithm
  .property /*1700003A*/ callconv(8) class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/
          OaepSHA1()
  {
    .get class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/::get_OaepSHA1() /* 06000204 */
  } // end of property RSAEncryptionPadding::OaepSHA1
  .property /*1700003B*/ callconv(8) class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/
          OaepSHA256()
  {
    .get class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/::get_OaepSHA256() /* 06000205 */
  } // end of property RSAEncryptionPadding::OaepSHA256
  .property /*1700003C*/ callconv(8) class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/
          OaepSHA384()
  {
    .get class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/::get_OaepSHA384() /* 06000206 */
  } // end of property RSAEncryptionPadding::OaepSHA384
  .property /*1700003D*/ callconv(8) class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/
          OaepSHA512()
  {
    .get class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/::get_OaepSHA512() /* 06000207 */
  } // end of property RSAEncryptionPadding::OaepSHA512
  .property /*1700003E*/ callconv(8) class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/
          Pkcs1()
  {
    .get class System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/ System.Security.Cryptography.RSAEncryptionPadding/*0200002D*/::get_Pkcs1() /* 06000208 */
  } // end of property RSAEncryptionPadding::Pkcs1
} // end of class System.Security.Cryptography.RSAEncryptionPadding

.class /*0200002E*/ public auto ansi sealed System.Security.Cryptography.RSAEncryptionPaddingMode
       extends [System.Runtime/*23000001*/]System.Enum/*0100002B*/
{
  .field /*04000021*/ public specialname rtspecialname int32 value__
  .field /*04000022*/ public static literal valuetype System.Security.Cryptography.RSAEncryptionPaddingMode/*0200002E*/ Pkcs1 = int32(0x00000000)
  .field /*04000023*/ public static literal valuetype System.Security.Cryptography.RSAEncryptionPaddingMode/*0200002E*/ Oaep = int32(0x00000001)
} // end of class System.Security.Cryptography.RSAEncryptionPaddingMode

.class /*0200002F*/ public auto ansi beforefieldinit System.Security.Cryptography.RSAOAEPKeyExchangeDeformatter
       extends System.Security.Cryptography.AsymmetricKeyExchangeDeformatter/*0200000A*/
{
  .custom /*0C0000A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000A6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000A7:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000210*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x26fe
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000035       */ call       instance void System.Security.Cryptography.AsymmetricKeyExchangeDeformatter/*0200000A*/::.ctor() /* 06000035 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeDeformatter::.ctor

  .method /*06000211*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x2706
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000035       */ call       instance void System.Security.Cryptography.AsymmetricKeyExchangeDeformatter/*0200000A*/::.ctor() /* 06000035 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeDeformatter::.ctor

  .method /*06000212*/ public hidebysig specialname virtual 
          instance string  get_Parameters() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C0001A9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x270e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAOAEPKeyExchangeDeformatter::get_Parameters

  .method /*06000213*/ public hidebysig specialname virtual 
          instance void  set_Parameters(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0001AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2711
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeDeformatter::set_Parameters

  .method /*06000214*/ public hidebysig virtual 
          instance uint8[]  DecryptKeyExchange(uint8[] rgbData) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x2713
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAOAEPKeyExchangeDeformatter::DecryptKeyExchange

  .method /*06000215*/ public hidebysig virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x2716
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeDeformatter::SetKey

  .property /*1700003F*/ instance callconv(8) string
          Parameters()
  {
    .custom /*0C0000CA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Security.Cryptography.RSAOAEPKeyExchangeDeformatter/*0200002F*/::get_Parameters() /* 06000212 */
    .set instance void System.Security.Cryptography.RSAOAEPKeyExchangeDeformatter/*0200002F*/::set_Parameters(string) /* 06000213 */
  } // end of property RSAOAEPKeyExchangeDeformatter::Parameters
} // end of class System.Security.Cryptography.RSAOAEPKeyExchangeDeformatter

.class /*02000030*/ public auto ansi beforefieldinit System.Security.Cryptography.RSAOAEPKeyExchangeFormatter
       extends System.Security.Cryptography.AsymmetricKeyExchangeFormatter/*0200000B*/
{
  .custom /*0C0000A8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000AA:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000216*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2718
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00003A       */ call       instance void System.Security.Cryptography.AsymmetricKeyExchangeFormatter/*0200000B*/::.ctor() /* 0600003A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeFormatter::.ctor

  .method /*06000217*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C0001AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2720
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00003A       */ call       instance void System.Security.Cryptography.AsymmetricKeyExchangeFormatter/*0200000B*/::.ctor() /* 0600003A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeFormatter::.ctor

  .method /*06000218*/ public hidebysig specialname 
          instance uint8[]  get_Parameter() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x2728
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAOAEPKeyExchangeFormatter::get_Parameter

  .method /*06000219*/ public hidebysig specialname 
          instance void  set_Parameter(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x272b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeFormatter::set_Parameter

  .method /*0600021A*/ public hidebysig specialname virtual 
          instance string  get_Parameters() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x272d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAOAEPKeyExchangeFormatter::get_Parameters

  .method /*0600021B*/ public hidebysig specialname 
          instance class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ 
          get_Rng() cil managed
  // SIG: 20 00 12 80 9C
  {
    // Method begins at RVA 0x2730
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAOAEPKeyExchangeFormatter::get_Rng

  .method /*0600021C*/ public hidebysig specialname 
          instance void  set_Rng(class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ 'value') cil managed
  // SIG: 20 01 01 12 80 9C
  {
    // Method begins at RVA 0x2733
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeFormatter::set_Rng

  .method /*0600021D*/ public hidebysig virtual 
          instance uint8[]  CreateKeyExchange(uint8[] rgbData) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    .custom /*0C0001AD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2735
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAOAEPKeyExchangeFormatter::CreateKeyExchange

  .method /*0600021E*/ public hidebysig virtual 
          instance uint8[]  CreateKeyExchange(uint8[] rgbData,
                                              class [System.Runtime/*23000001*/]System.Type/*01000024*/ symAlgType) cil managed
  // SIG: 20 02 1D 05 1D 05 12 80 91
  {
    .custom /*0C0001AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800038B*/ 
    .custom /*0C0001F2:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2738
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAOAEPKeyExchangeFormatter::CreateKeyExchange

  .method /*0600021F*/ public hidebysig virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C0001AF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x273b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAOAEPKeyExchangeFormatter::SetKey

  .property /*17000040*/ instance callconv(8) uint8[]
          Parameter()
  {
    .get instance uint8[] System.Security.Cryptography.RSAOAEPKeyExchangeFormatter/*02000030*/::get_Parameter() /* 06000218 */
    .set instance void System.Security.Cryptography.RSAOAEPKeyExchangeFormatter/*02000030*/::set_Parameter(uint8[]) /* 06000219 */
  } // end of property RSAOAEPKeyExchangeFormatter::Parameter
  .property /*17000041*/ instance callconv(8) string
          Parameters()
  {
    .get instance string System.Security.Cryptography.RSAOAEPKeyExchangeFormatter/*02000030*/::get_Parameters() /* 0600021A */
  } // end of property RSAOAEPKeyExchangeFormatter::Parameters
  .property /*17000042*/ instance callconv(8) class System.Security.Cryptography.RandomNumberGenerator/*02000027*/
          Rng()
  {
    .get instance class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ System.Security.Cryptography.RSAOAEPKeyExchangeFormatter/*02000030*/::get_Rng() /* 0600021B */
    .set instance void System.Security.Cryptography.RSAOAEPKeyExchangeFormatter/*02000030*/::set_Rng(class System.Security.Cryptography.RandomNumberGenerator/*02000027*/) /* 0600021C */
  } // end of property RSAOAEPKeyExchangeFormatter::Rng
} // end of class System.Security.Cryptography.RSAOAEPKeyExchangeFormatter

.class /*02000031*/ public sequential ansi sealed beforefieldinit System.Security.Cryptography.RSAParameters
       extends [System.Runtime/*23000001*/]System.ValueType/*0100002A*/
{
  .custom /*0C0000AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000AC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000024*/ public uint8[] D
  .field /*04000025*/ public uint8[] DP
  .field /*04000026*/ public uint8[] DQ
  .field /*04000027*/ public uint8[] Exponent
  .field /*04000028*/ public uint8[] InverseQ
  .field /*04000029*/ public uint8[] Modulus
  .field /*0400002A*/ public uint8[] P
  .field /*0400002B*/ public uint8[] Q
} // end of class System.Security.Cryptography.RSAParameters

.class /*02000032*/ public auto ansi beforefieldinit System.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter
       extends System.Security.Cryptography.AsymmetricKeyExchangeDeformatter/*0200000A*/
{
  .custom /*0C0000AD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000AE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000AF:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000220*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x273d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000035       */ call       instance void System.Security.Cryptography.AsymmetricKeyExchangeDeformatter/*0200000A*/::.ctor() /* 06000035 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeDeformatter::.ctor

  .method /*06000221*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C0001B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2745
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000035       */ call       instance void System.Security.Cryptography.AsymmetricKeyExchangeDeformatter/*0200000A*/::.ctor() /* 06000035 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeDeformatter::.ctor

  .method /*06000222*/ public hidebysig specialname virtual 
          instance string  get_Parameters() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x274d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1KeyExchangeDeformatter::get_Parameters

  .method /*06000223*/ public hidebysig specialname virtual 
          instance void  set_Parameters(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2750
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeDeformatter::set_Parameters

  .method /*06000224*/ public hidebysig specialname 
          instance class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ 
          get_RNG() cil managed
  // SIG: 20 00 12 80 9C
  {
    // Method begins at RVA 0x2752
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1KeyExchangeDeformatter::get_RNG

  .method /*06000225*/ public hidebysig specialname 
          instance void  set_RNG(class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ 'value') cil managed
  // SIG: 20 01 01 12 80 9C
  {
    // Method begins at RVA 0x2755
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeDeformatter::set_RNG

  .method /*06000226*/ public hidebysig virtual 
          instance uint8[]  DecryptKeyExchange(uint8[] rgbIn) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    .custom /*0C0001B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2757
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1KeyExchangeDeformatter::DecryptKeyExchange

  .method /*06000227*/ public hidebysig virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    .custom /*0C0001B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x275a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeDeformatter::SetKey

  .property /*17000043*/ instance callconv(8) string
          Parameters()
  {
    .get instance string System.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter/*02000032*/::get_Parameters() /* 06000222 */
    .set instance void System.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter/*02000032*/::set_Parameters(string) /* 06000223 */
  } // end of property RSAPKCS1KeyExchangeDeformatter::Parameters
  .property /*17000044*/ instance callconv(8) class System.Security.Cryptography.RandomNumberGenerator/*02000027*/
          RNG()
  {
    .get instance class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ System.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter/*02000032*/::get_RNG() /* 06000224 */
    .set instance void System.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter/*02000032*/::set_RNG(class System.Security.Cryptography.RandomNumberGenerator/*02000027*/) /* 06000225 */
  } // end of property RSAPKCS1KeyExchangeDeformatter::RNG
} // end of class System.Security.Cryptography.RSAPKCS1KeyExchangeDeformatter

.class /*02000033*/ public auto ansi beforefieldinit System.Security.Cryptography.RSAPKCS1KeyExchangeFormatter
       extends System.Security.Cryptography.AsymmetricKeyExchangeFormatter/*0200000B*/
{
  .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B1:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B2:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000228*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x275c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00003A       */ call       instance void System.Security.Cryptography.AsymmetricKeyExchangeFormatter/*0200000B*/::.ctor() /* 0600003A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeFormatter::.ctor

  .method /*06000229*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x2764
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00003A       */ call       instance void System.Security.Cryptography.AsymmetricKeyExchangeFormatter/*0200000B*/::.ctor() /* 0600003A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeFormatter::.ctor

  .method /*0600022A*/ public hidebysig specialname virtual 
          instance string  get_Parameters() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x276c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1KeyExchangeFormatter::get_Parameters

  .method /*0600022B*/ public hidebysig specialname 
          instance class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ 
          get_Rng() cil managed
  // SIG: 20 00 12 80 9C
  {
    .custom /*0C0001B4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x276f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1KeyExchangeFormatter::get_Rng

  .method /*0600022C*/ public hidebysig specialname 
          instance void  set_Rng(class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ 'value') cil managed
  // SIG: 20 01 01 12 80 9C
  {
    .custom /*0C0001B5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2772
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeFormatter::set_Rng

  .method /*0600022D*/ public hidebysig virtual 
          instance uint8[]  CreateKeyExchange(uint8[] rgbData) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x2774
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1KeyExchangeFormatter::CreateKeyExchange

  .method /*0600022E*/ public hidebysig virtual 
          instance uint8[]  CreateKeyExchange(uint8[] rgbData,
                                              class [System.Runtime/*23000001*/]System.Type/*01000024*/ symAlgType) cil managed
  // SIG: 20 02 1D 05 1D 05 12 80 91
  {
    .param [2]/*08000396*/ 
    .custom /*0C0001F3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2777
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1KeyExchangeFormatter::CreateKeyExchange

  .method /*0600022F*/ public hidebysig virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x277a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1KeyExchangeFormatter::SetKey

  .property /*17000045*/ instance callconv(8) string
          Parameters()
  {
    .get instance string System.Security.Cryptography.RSAPKCS1KeyExchangeFormatter/*02000033*/::get_Parameters() /* 0600022A */
  } // end of property RSAPKCS1KeyExchangeFormatter::Parameters
  .property /*17000046*/ instance callconv(8) class System.Security.Cryptography.RandomNumberGenerator/*02000027*/
          Rng()
  {
    .custom /*0C0000D4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Security.Cryptography.RandomNumberGenerator/*02000027*/ System.Security.Cryptography.RSAPKCS1KeyExchangeFormatter/*02000033*/::get_Rng() /* 0600022B */
    .set instance void System.Security.Cryptography.RSAPKCS1KeyExchangeFormatter/*02000033*/::set_Rng(class System.Security.Cryptography.RandomNumberGenerator/*02000027*/) /* 0600022C */
  } // end of property RSAPKCS1KeyExchangeFormatter::Rng
} // end of class System.Security.Cryptography.RSAPKCS1KeyExchangeFormatter

.class /*02000034*/ public auto ansi beforefieldinit System.Security.Cryptography.RSAPKCS1SignatureDeformatter
       extends System.Security.Cryptography.AsymmetricSignatureDeformatter/*0200000C*/
{
  .custom /*0C0000B3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B5:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000230*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x277c
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00003F       */ call       instance void System.Security.Cryptography.AsymmetricSignatureDeformatter/*0200000C*/::.ctor() /* 0600003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1SignatureDeformatter::.ctor

  .method /*06000231*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x2784
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00003F       */ call       instance void System.Security.Cryptography.AsymmetricSignatureDeformatter/*0200000C*/::.ctor() /* 0600003F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1SignatureDeformatter::.ctor

  .method /*06000232*/ public hidebysig virtual 
          instance void  SetHashAlgorithm(string strName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x278c
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1SignatureDeformatter::SetHashAlgorithm

  .method /*06000233*/ public hidebysig virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x278e
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1SignatureDeformatter::SetKey

  .method /*06000234*/ public hidebysig virtual 
          instance bool  VerifySignature(uint8[] rgbHash,
                                         uint8[] rgbSignature) cil managed
  // SIG: 20 02 02 1D 05 1D 05
  {
    // Method begins at RVA 0x2790
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1SignatureDeformatter::VerifySignature

} // end of class System.Security.Cryptography.RSAPKCS1SignatureDeformatter

.class /*02000035*/ public auto ansi beforefieldinit System.Security.Cryptography.RSAPKCS1SignatureFormatter
       extends System.Security.Cryptography.AsymmetricSignatureFormatter/*0200000D*/
{
  .custom /*0C0000B6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000B7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000B8:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000235*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2793
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000044       */ call       instance void System.Security.Cryptography.AsymmetricSignatureFormatter/*0200000D*/::.ctor() /* 06000044 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1SignatureFormatter::.ctor

  .method /*06000236*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x279b
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000044       */ call       instance void System.Security.Cryptography.AsymmetricSignatureFormatter/*0200000D*/::.ctor() /* 06000044 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1SignatureFormatter::.ctor

  .method /*06000237*/ public hidebysig virtual 
          instance uint8[]  CreateSignature(uint8[] rgbHash) cil managed
  // SIG: 20 01 1D 05 1D 05
  {
    // Method begins at RVA 0x27a3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSAPKCS1SignatureFormatter::CreateSignature

  .method /*06000238*/ public hidebysig virtual 
          instance void  SetHashAlgorithm(string strName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x27a6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1SignatureFormatter::SetHashAlgorithm

  .method /*06000239*/ public hidebysig virtual 
          instance void  SetKey(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 01 12 80 8D
  {
    // Method begins at RVA 0x27a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method RSAPKCS1SignatureFormatter::SetKey

} // end of class System.Security.Cryptography.RSAPKCS1SignatureFormatter

.class /*02000036*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.RSASignaturePadding
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
       implements class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<class System.Security.Cryptography.RSASignaturePadding/*02000036*/>/*1B000002*/
{
  .custom /*0C0000B9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000BA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000BB:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .interfaceimpl type class [System.Runtime/*23000001*/]System.IEquatable`1/*0100002F*/<class System.Security.Cryptography.RSASignaturePadding/*02000036*/>/*1B000002*/
  .custom /*0C00003A:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*0600023A*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27aa
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method RSASignaturePadding::.ctor

  .method /*0600023B*/ public hidebysig specialname 
          instance valuetype System.Security.Cryptography.RSASignaturePaddingMode/*02000037*/ 
          get_Mode() cil managed
  // SIG: 20 00 11 80 DC
  {
    // Method begins at RVA 0x27b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::get_Mode

  .method /*0600023C*/ public hidebysig specialname static 
          class System.Security.Cryptography.RSASignaturePadding/*02000036*/ 
          get_Pkcs1() cil managed
  // SIG: 00 00 12 80 D8
  {
    // Method begins at RVA 0x27b5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::get_Pkcs1

  .method /*0600023D*/ public hidebysig specialname static 
          class System.Security.Cryptography.RSASignaturePadding/*02000036*/ 
          get_Pss() cil managed
  // SIG: 00 00 12 80 D8
  {
    // Method begins at RVA 0x27b8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::get_Pss

  .method /*0600023E*/ public hidebysig virtual 
          instance bool  Equals(object obj) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C0001B7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080003A1*/ 
    .custom /*0C0001F4:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27bb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::Equals

  .method /*0600023F*/ public hidebysig newslot virtual final 
          instance bool  Equals(class System.Security.Cryptography.RSASignaturePadding/*02000036*/ other) cil managed
  // SIG: 20 01 02 12 80 D8
  {
    .custom /*0C0001B8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*080003A2*/ 
    .custom /*0C0001F5:0A000017*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000030*/::.ctor(bool) /* 0A000017 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27be
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::Equals

  .method /*06000240*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x27c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::GetHashCode

  .method /*06000241*/ public hidebysig specialname static 
          bool  op_Equality(class System.Security.Cryptography.RSASignaturePadding/*02000036*/ left,
                            class System.Security.Cryptography.RSASignaturePadding/*02000036*/ right) cil managed
  // SIG: 00 02 02 12 80 D8 12 80 D8
  {
    .custom /*0C0001B9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27c4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::op_Equality

  .method /*06000242*/ public hidebysig specialname static 
          bool  op_Inequality(class System.Security.Cryptography.RSASignaturePadding/*02000036*/ left,
                              class System.Security.Cryptography.RSASignaturePadding/*02000036*/ right) cil managed
  // SIG: 00 02 02 12 80 D8 12 80 D8
  {
    .custom /*0C0001BA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::op_Inequality

  .method /*06000243*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x27ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method RSASignaturePadding::ToString

  .property /*17000047*/ instance callconv(8) valuetype System.Security.Cryptography.RSASignaturePaddingMode/*02000037*/
          Mode()
  {
    .get instance valuetype System.Security.Cryptography.RSASignaturePaddingMode/*02000037*/ System.Security.Cryptography.RSASignaturePadding/*02000036*/::get_Mode() /* 0600023B */
  } // end of property RSASignaturePadding::Mode
  .property /*17000048*/ callconv(8) class System.Security.Cryptography.RSASignaturePadding/*02000036*/
          Pkcs1()
  {
    .get class System.Security.Cryptography.RSASignaturePadding/*02000036*/ System.Security.Cryptography.RSASignaturePadding/*02000036*/::get_Pkcs1() /* 0600023C */
  } // end of property RSASignaturePadding::Pkcs1
  .property /*17000049*/ callconv(8) class System.Security.Cryptography.RSASignaturePadding/*02000036*/
          Pss()
  {
    .get class System.Security.Cryptography.RSASignaturePadding/*02000036*/ System.Security.Cryptography.RSASignaturePadding/*02000036*/::get_Pss() /* 0600023D */
  } // end of property RSASignaturePadding::Pss
} // end of class System.Security.Cryptography.RSASignaturePadding

.class /*02000037*/ public auto ansi sealed System.Security.Cryptography.RSASignaturePaddingMode
       extends [System.Runtime/*23000001*/]System.Enum/*0100002B*/
{
  .field /*0400002C*/ public specialname rtspecialname int32 value__
  .field /*0400002D*/ public static literal valuetype System.Security.Cryptography.RSASignaturePaddingMode/*02000037*/ Pkcs1 = int32(0x00000000)
  .field /*0400002E*/ public static literal valuetype System.Security.Cryptography.RSASignaturePaddingMode/*02000037*/ Pss = int32(0x00000001)
} // end of class System.Security.Cryptography.RSASignaturePaddingMode

.class /*02000038*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.SHA1
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/
{
  .method /*06000244*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27cd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA1::.ctor

  .method /*06000245*/ public hidebysig static 
          class System.Security.Cryptography.SHA1/*02000038*/ 
          Create() cil managed
  // SIG: 00 00 12 80 E0
  {
    .custom /*0C0001BB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27d5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1::Create

  .method /*06000246*/ public hidebysig static 
          class System.Security.Cryptography.SHA1/*02000038*/ 
          Create(string hashName) cil managed
  // SIG: 00 01 12 80 E0 0E
  {
    .custom /*0C0001BC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001BD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*080003A7*/ 
    .custom /*0C0001F6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x27d8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1::Create

  .method /*06000247*/ public hidebysig static 
          uint8[]  HashData(uint8[] source) cil managed
  // SIG: 00 01 1D 05 1D 05
  {
    .custom /*0C0001BE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1::HashData

  .method /*06000248*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 01 1D 05 15 11 69 01 05
  {
    .param [0]/*080003AA*/ 
    .custom /*0C0001F7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27de
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1::HashData

  .method /*06000249*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 02 08 15 11 69 01 05 15 11 71 01 05
  {
    // Method begins at RVA 0x27e1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1::HashData

  .method /*0600024A*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 69 01 05 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x27e4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1::TryHashData

} // end of class System.Security.Cryptography.SHA1

.class /*02000039*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.SHA1Managed
       extends System.Security.Cryptography.SHA1/*02000038*/
{
  .custom /*0C0000C0:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
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
  .custom /*0C0000C1:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*0600024B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27e7
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000244       */ call       instance void System.Security.Cryptography.SHA1/*02000038*/::.ctor() /* 06000244 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA1Managed::.ctor

  .method /*0600024C*/ family hidebysig virtual final 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x27ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA1Managed::Dispose

  .method /*0600024D*/ family hidebysig virtual final 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C0001C0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27f1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA1Managed::HashCore

  .method /*0600024E*/ family hidebysig virtual final 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    // Method begins at RVA 0x27f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA1Managed::HashCore

  .method /*0600024F*/ family hidebysig virtual final 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C0001C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x27f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1Managed::HashFinal

  .method /*06000250*/ public hidebysig virtual final 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27f8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA1Managed::Initialize

  .method /*06000251*/ family hidebysig virtual final 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x27fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA1Managed::TryHashFinal

} // end of class System.Security.Cryptography.SHA1Managed

.class /*0200003A*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.SHA256
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/
{
  .method /*06000252*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x27fd
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA256::.ctor

  .method /*06000253*/ public hidebysig static 
          class System.Security.Cryptography.SHA256/*0200003A*/ 
          Create() cil managed
  // SIG: 00 00 12 80 E8
  {
    .custom /*0C0001C2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2805
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256::Create

  .method /*06000254*/ public hidebysig static 
          class System.Security.Cryptography.SHA256/*0200003A*/ 
          Create(string hashName) cil managed
  // SIG: 00 01 12 80 E8 0E
  {
    .custom /*0C0001C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001C4:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*080003B8*/ 
    .custom /*0C0001F8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2808
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256::Create

  .method /*06000255*/ public hidebysig static 
          uint8[]  HashData(uint8[] source) cil managed
  // SIG: 00 01 1D 05 1D 05
  {
    .custom /*0C0001C5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x280b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256::HashData

  .method /*06000256*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 01 1D 05 15 11 69 01 05
  {
    .param [0]/*080003BB*/ 
    .custom /*0C0001F9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x280e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256::HashData

  .method /*06000257*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 02 08 15 11 69 01 05 15 11 71 01 05
  {
    // Method begins at RVA 0x2811
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256::HashData

  .method /*06000258*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 69 01 05 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2814
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256::TryHashData

} // end of class System.Security.Cryptography.SHA256

.class /*0200003B*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.SHA256Managed
       extends System.Security.Cryptography.SHA256/*0200003A*/
{
  .custom /*0C0000C4:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
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
  .custom /*0C0000C5:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000259*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2817
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000252       */ call       instance void System.Security.Cryptography.SHA256/*0200003A*/::.ctor() /* 06000252 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA256Managed::.ctor

  .method /*0600025A*/ family hidebysig virtual final 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x281f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA256Managed::Dispose

  .method /*0600025B*/ family hidebysig virtual final 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C0001C6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2821
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA256Managed::HashCore

  .method /*0600025C*/ family hidebysig virtual final 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    // Method begins at RVA 0x2823
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA256Managed::HashCore

  .method /*0600025D*/ family hidebysig virtual final 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C0001C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2825
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256Managed::HashFinal

  .method /*0600025E*/ public hidebysig virtual final 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2828
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA256Managed::Initialize

  .method /*0600025F*/ family hidebysig virtual final 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x282a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA256Managed::TryHashFinal

} // end of class System.Security.Cryptography.SHA256Managed

.class /*0200003C*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.SHA384
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/
{
  .method /*06000260*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x282d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA384::.ctor

  .method /*06000261*/ public hidebysig static 
          class System.Security.Cryptography.SHA384/*0200003C*/ 
          Create() cil managed
  // SIG: 00 00 12 80 F0
  {
    .custom /*0C0001C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2835
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384::Create

  .method /*06000262*/ public hidebysig static 
          class System.Security.Cryptography.SHA384/*0200003C*/ 
          Create(string hashName) cil managed
  // SIG: 00 01 12 80 F0 0E
  {
    .custom /*0C0001C9:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001CA:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*080003C9*/ 
    .custom /*0C0001FA:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2838
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384::Create

  .method /*06000263*/ public hidebysig static 
          uint8[]  HashData(uint8[] source) cil managed
  // SIG: 00 01 1D 05 1D 05
  {
    .custom /*0C0001CC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x283b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384::HashData

  .method /*06000264*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 01 1D 05 15 11 69 01 05
  {
    .param [0]/*080003CC*/ 
    .custom /*0C0001FB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x283e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384::HashData

  .method /*06000265*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 02 08 15 11 69 01 05 15 11 71 01 05
  {
    // Method begins at RVA 0x2841
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384::HashData

  .method /*06000266*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 69 01 05 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2844
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384::TryHashData

} // end of class System.Security.Cryptography.SHA384

.class /*0200003D*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.SHA384Managed
       extends System.Security.Cryptography.SHA384/*0200003C*/
{
  .custom /*0C0000C6:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
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
  .custom /*0C0000C7:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000267*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2847
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)000260       */ call       instance void System.Security.Cryptography.SHA384/*0200003C*/::.ctor() /* 06000260 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA384Managed::.ctor

  .method /*06000268*/ family hidebysig virtual final 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x284f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA384Managed::Dispose

  .method /*06000269*/ family hidebysig virtual final 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C0001CD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2851
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA384Managed::HashCore

  .method /*0600026A*/ family hidebysig virtual final 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    // Method begins at RVA 0x2853
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA384Managed::HashCore

  .method /*0600026B*/ family hidebysig virtual final 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C0001CE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2855
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384Managed::HashFinal

  .method /*0600026C*/ public hidebysig virtual final 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2858
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA384Managed::Initialize

  .method /*0600026D*/ family hidebysig virtual final 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x285a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA384Managed::TryHashFinal

} // end of class System.Security.Cryptography.SHA384Managed

.class /*0200003E*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.SHA512
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/
{
  .method /*0600026E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x285d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA512::.ctor

  .method /*0600026F*/ public hidebysig static 
          class System.Security.Cryptography.SHA512/*0200003E*/ 
          Create() cil managed
  // SIG: 00 00 12 80 F8
  {
    .custom /*0C0001CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2865
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512::Create

  .method /*06000270*/ public hidebysig static 
          class System.Security.Cryptography.SHA512/*0200003E*/ 
          Create(string hashName) cil managed
  // SIG: 00 01 12 80 F8 0E
  {
    .custom /*0C0001D0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001D1:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*080003DA*/ 
    .custom /*0C0001FC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2868
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512::Create

  .method /*06000271*/ public hidebysig static 
          uint8[]  HashData(uint8[] source) cil managed
  // SIG: 00 01 1D 05 1D 05
  {
    .custom /*0C0001D2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x286b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512::HashData

  .method /*06000272*/ public hidebysig static 
          uint8[]  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 00 01 1D 05 15 11 69 01 05
  {
    .param [0]/*080003DD*/ 
    .custom /*0C0001FD:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x286e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512::HashData

  .method /*06000273*/ public hidebysig static 
          int32  HashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                          valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination) cil managed
  // SIG: 00 02 08 15 11 69 01 05 15 11 71 01 05
  {
    // Method begins at RVA 0x2871
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512::HashData

  .method /*06000274*/ public hidebysig static 
          bool  TryHashData(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source,
                            valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                            [out] int32& bytesWritten) cil managed
  // SIG: 00 03 02 15 11 69 01 05 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x2874
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512::TryHashData

} // end of class System.Security.Cryptography.SHA512

.class /*0200003F*/ public auto ansi sealed beforefieldinit System.Security.Cryptography.SHA512Managed
       extends System.Security.Cryptography.SHA512/*0200003E*/
{
  .custom /*0C0000C8:0A000014*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*0100001D*/::.ctor(string) /* 0A000014 */ = ( 01 00 59 44 65 72 69 76 65 64 20 63 72 79 70 74   // ..YDerived crypt
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
  .custom /*0C0000C9:0A000015*/ instance void [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableAttribute/*0100001F*/::.ctor(valuetype [System.Runtime/*23000001*/]System.ComponentModel.EditorBrowsableState/*0100001E*/) /* 0A000015 */ = ( 01 00 01 00 00 00 00 00 ) 
  .method /*06000275*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2877
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)00026E       */ call       instance void System.Security.Cryptography.SHA512/*0200003E*/::.ctor() /* 0600026E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SHA512Managed::.ctor

  .method /*06000276*/ family hidebysig virtual final 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x287f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA512Managed::Dispose

  .method /*06000277*/ family hidebysig virtual final 
          instance void  HashCore(uint8[] 'array',
                                  int32 ibStart,
                                  int32 cbSize) cil managed
  // SIG: 20 03 01 1D 05 08 08
  {
    .custom /*0C0001D3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2881
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA512Managed::HashCore

  .method /*06000278*/ family hidebysig virtual final 
          instance void  HashCore(valuetype [System.Runtime/*23000001*/]System.ReadOnlySpan`1/*0100001A*/<uint8> source) cil managed
  // SIG: 20 01 01 15 11 69 01 05
  {
    // Method begins at RVA 0x2883
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA512Managed::HashCore

  .method /*06000279*/ family hidebysig virtual final 
          instance uint8[]  HashFinal() cil managed
  // SIG: 20 00 1D 05
  {
    .custom /*0C0001D5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2885
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512Managed::HashFinal

  .method /*0600027A*/ public hidebysig virtual final 
          instance void  Initialize() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2888
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SHA512Managed::Initialize

  .method /*0600027B*/ family hidebysig virtual final 
          instance bool  TryHashFinal(valuetype [System.Runtime/*23000001*/]System.Span`1/*0100001C*/<uint8> destination,
                                      [out] int32& bytesWritten) cil managed
  // SIG: 20 02 02 15 11 71 01 05 10 08
  {
    // Method begins at RVA 0x288a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SHA512Managed::TryHashFinal

} // end of class System.Security.Cryptography.SHA512Managed

.class /*02000040*/ public auto ansi beforefieldinit System.Security.Cryptography.SignatureDescription
       extends [System.Runtime/*23000001*/]System.Object/*01000018*/
{
  .custom /*0C0000CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C0000CC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000CD:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*0600027C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x288d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SignatureDescription::.ctor

  .method /*0600027D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Security.SecurityElement/*01000031*/ el) cil managed
  // SIG: 20 01 01 12 80 C5
  {
    .custom /*0C0001D6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2895
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000018*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SignatureDescription::.ctor

  .method /*0600027E*/ public hidebysig specialname 
          instance string  get_DeformatterAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x289d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureDescription::get_DeformatterAlgorithm

  .method /*0600027F*/ public hidebysig specialname 
          instance void  set_DeformatterAlgorithm(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x28a0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureDescription::set_DeformatterAlgorithm

  .method /*06000280*/ public hidebysig specialname 
          instance string  get_DigestAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x28a2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureDescription::get_DigestAlgorithm

  .method /*06000281*/ public hidebysig specialname 
          instance void  set_DigestAlgorithm(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x28a5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureDescription::set_DigestAlgorithm

  .method /*06000282*/ public hidebysig specialname 
          instance string  get_FormatterAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x28a7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureDescription::get_FormatterAlgorithm

  .method /*06000283*/ public hidebysig specialname 
          instance void  set_FormatterAlgorithm(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x28aa
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureDescription::set_FormatterAlgorithm

  .method /*06000284*/ public hidebysig specialname 
          instance string  get_KeyAlgorithm() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x28ac
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureDescription::get_KeyAlgorithm

  .method /*06000285*/ public hidebysig specialname 
          instance void  set_KeyAlgorithm(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x28af
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SignatureDescription::set_KeyAlgorithm

  .method /*06000286*/ public hidebysig newslot virtual 
          instance class System.Security.Cryptography.AsymmetricSignatureDeformatter/*0200000C*/ 
          CreateDeformatter(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 12 30 12 80 8D
  {
    .custom /*0C0001D7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001D8:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 80 82 43 72 65 61 74 65 44 65 66 6F 72 6D   // ....CreateDeform
                                                                                                                                                                                            61 74 74 65 72 20 69 73 20 6E 6F 74 20 74 72 69   // atter is not tri
                                                                                                                                                                                            6D 20 63 6F 6D 70 61 74 69 62 6C 65 20 62 65 63   // m compatible bec
                                                                                                                                                                                            61 75 73 65 20 74 68 65 20 61 6C 67 6F 72 69 74   // ause the algorit
                                                                                                                                                                                            68 6D 20 69 6D 70 6C 65 6D 65 6E 74 61 74 69 6F   // hm implementatio
                                                                                                                                                                                            6E 20 72 65 66 65 72 65 6E 63 65 64 20 62 79 20   // n referenced by 
                                                                                                                                                                                            44 65 66 6F 72 6D 61 74 74 65 72 41 6C 67 6F 72   // DeformatterAlgor
                                                                                                                                                                                            69 74 68 6D 20 6D 69 67 68 74 20 62 65 20 72 65   // ithm might be re
                                                                                                                                                                                            6D 6F 76 65 64 2E 00 00 )                         // moved...
    // Method begins at RVA 0x28b1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureDescription::CreateDeformatter

  .method /*06000287*/ public hidebysig newslot virtual 
          instance class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.HashAlgorithm/*01000025*/ 
          CreateDigest() cil managed
  // SIG: 20 00 12 80 95
  {
    .custom /*0C0001D9:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 78 43 72 65 61 74 65 44 69 67 65 73 74 20   // ..xCreateDigest 
                                                                                                                                                                                            69 73 20 6E 6F 74 20 74 72 69 6D 20 63 6F 6D 70   // is not trim comp
                                                                                                                                                                                            61 74 69 62 6C 65 20 62 65 63 61 75 73 65 20 74   // atible because t
                                                                                                                                                                                            68 65 20 61 6C 67 6F 72 69 74 68 6D 20 69 6D 70   // he algorithm imp
                                                                                                                                                                                            6C 65 6D 65 6E 74 61 74 69 6F 6E 20 72 65 66 65   // lementation refe
                                                                                                                                                                                            72 65 6E 63 65 64 20 62 79 20 44 69 67 65 73 74   // renced by Digest
                                                                                                                                                                                            41 6C 67 6F 72 69 74 68 6D 20 6D 69 67 68 74 20   // Algorithm might 
                                                                                                                                                                                            62 65 20 72 65 6D 6F 76 65 64 2E 00 00 )          // be removed...
    // Method begins at RVA 0x28b4
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureDescription::CreateDigest

  .method /*06000288*/ public hidebysig newslot virtual 
          instance class System.Security.Cryptography.AsymmetricSignatureFormatter/*0200000D*/ 
          CreateFormatter(class [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.AsymmetricAlgorithm/*01000023*/ key) cil managed
  // SIG: 20 01 12 34 12 80 8D
  {
    .custom /*0C0001DA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .custom /*0C0001DB:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 7E 43 72 65 61 74 65 46 6F 72 6D 61 74 74   // ..~CreateFormatt
                                                                                                                                                                                            65 72 20 69 73 20 6E 6F 74 20 74 72 69 6D 20 63   // er is not trim c
                                                                                                                                                                                            6F 6D 70 61 74 69 62 6C 65 20 62 65 63 61 75 73   // ompatible becaus
                                                                                                                                                                                            65 20 74 68 65 20 61 6C 67 6F 72 69 74 68 6D 20   // e the algorithm 
                                                                                                                                                                                            69 6D 70 6C 65 6D 65 6E 74 61 74 69 6F 6E 20 72   // implementation r
                                                                                                                                                                                            65 66 65 72 65 6E 63 65 64 20 62 79 20 46 6F 72   // eferenced by For
                                                                                                                                                                                            6D 61 74 74 65 72 41 6C 67 6F 72 69 74 68 6D 20   // matterAlgorithm 
                                                                                                                                                                                            6D 69 67 68 74 20 62 65 20 72 65 6D 6F 76 65 64   // might be removed
                                                                                                                                                                                            2E 00 00 )                                        // ...
    // Method begins at RVA 0x28b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SignatureDescription::CreateFormatter

  .property /*1700004A*/ instance callconv(8) string
          DeformatterAlgorithm()
  {
    .get instance string System.Security.Cryptography.SignatureDescription/*02000040*/::get_DeformatterAlgorithm() /* 0600027E */
    .set instance void System.Security.Cryptography.SignatureDescription/*02000040*/::set_DeformatterAlgorithm(string) /* 0600027F */
  } // end of property SignatureDescription::DeformatterAlgorithm
  .property /*1700004B*/ instance callconv(8) string
          DigestAlgorithm()
  {
    .get instance string System.Security.Cryptography.SignatureDescription/*02000040*/::get_DigestAlgorithm() /* 06000280 */
    .set instance void System.Security.Cryptography.SignatureDescription/*02000040*/::set_DigestAlgorithm(string) /* 06000281 */
  } // end of property SignatureDescription::DigestAlgorithm
  .property /*1700004C*/ instance callconv(8) string
          FormatterAlgorithm()
  {
    .get instance string System.Security.Cryptography.SignatureDescription/*02000040*/::get_FormatterAlgorithm() /* 06000282 */
    .set instance void System.Security.Cryptography.SignatureDescription/*02000040*/::set_FormatterAlgorithm(string) /* 06000283 */
  } // end of property SignatureDescription::FormatterAlgorithm
  .property /*1700004D*/ instance callconv(8) string
          KeyAlgorithm()
  {
    .get instance string System.Security.Cryptography.SignatureDescription/*02000040*/::get_KeyAlgorithm() /* 06000284 */
    .set instance void System.Security.Cryptography.SignatureDescription/*02000040*/::set_KeyAlgorithm(string) /* 06000285 */
  } // end of property SignatureDescription::KeyAlgorithm
} // end of class System.Security.Cryptography.SignatureDescription

.class /*02000041*/ public abstract auto ansi beforefieldinit System.Security.Cryptography.TripleDES
       extends [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/
{
  .custom /*0C0000CF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C0000D0:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C0000D1:0A000012*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000015*/::.ctor(string) /* 0A000012 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .method /*06000289*/ family hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x28ba
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Security.Cryptography.Primitives/*23000002*/]System.Security.Cryptography.SymmetricAlgorithm/*01000016*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method TripleDES::.ctor

  .method /*0600028A*/ public hidebysig specialname virtual 
          instance uint8[]  get_Key() cil managed
  // SIG: 20 00 1D 05
  {
    // Method begins at RVA 0x28c2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDES::get_Key

  .method /*0600028B*/ public hidebysig specialname virtual 
          instance void  set_Key(uint8[] 'value') cil managed
  // SIG: 20 01 01 1D 05
  {
    // Method begins at RVA 0x28c5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method TripleDES::set_Key

  .method /*0600028C*/ public hidebysig static 
          class System.Security.Cryptography.TripleDES/*02000041*/ 
          Create() cil managed
  // SIG: 00 00 12 81 04
  {
    // Method begins at RVA 0x28c7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDES::Create

  .method /*0600028D*/ public hidebysig static 
          class System.Security.Cryptography.TripleDES/*02000041*/ 
          Create(string str) cil managed
  // SIG: 00 01 12 81 04 0E
  {
    .custom /*0C0001DD:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.RequiresUnreferencedCodeAttribute/*01000017*/::.ctor(string) /* 0A000013 */ = ( 01 00 6F 54 68 65 20 64 65 66 61 75 6C 74 20 61   // ..oThe default a
                                                                                                                                                                                            6C 67 6F 72 69 74 68 6D 20 69 6D 70 6C 65 6D 65   // lgorithm impleme
                                                                                                                                                                                            6E 74 61 74 69 6F 6E 73 20 6D 69 67 68 74 20 62   // ntations might b
                                                                                                                                                                                            65 20 72 65 6D 6F 76 65 64 2C 20 75 73 65 20 73   // e removed, use s
                                                                                                                                                                                            74 72 6F 6E 67 20 74 79 70 65 20 72 65 66 65 72   // trong type refer
                                                                                                                                                                                            65 6E 63 65 73 20 6C 69 6B 65 20 27 52 53 41 2E   // ences like 'RSA.
                                                                                                                                                                                            43 72 65 61 74 65 28 29 27 20 69 6E 73 74 65 61   // Create()' instea
                                                                                                                                                                                            64 2E 00 00 )                                     // d...
    .param [0]/*080003F3*/ 
    .custom /*0C0001FE:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x28ca
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDES::Create

  .method /*0600028E*/ public hidebysig static 
          bool  IsWeakKey(uint8[] rgbKey) cil managed
  // SIG: 00 01 02 1D 05
  {
    // Method begins at RVA 0x28cd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method TripleDES::IsWeakKey

  .property /*1700004E*/ instance callconv(8) uint8[]
          Key()
  {
    .get instance uint8[] System.Security.Cryptography.TripleDES/*02000041*/::get_Key() /* 0600028A */
    .set instance void System.Security.Cryptography.TripleDES/*02000041*/::set_Key(uint8[]) /* 0600028B */
  } // end of property TripleDES::Key
} // end of class System.Security.Cryptography.TripleDES


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
