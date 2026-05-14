


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
// Time-date stamp:            0xe2725426
// Ptr to symbol table:        0x00000000
// Number of symbols:          0x00000000
// Size of optional header:    0x00e0
// Characteristics:            0x2022
// ----- PE Optional Header (32 bit):
// Magic:                          0x010b
// Major linker version:           0x30
// Minor linker version:           0x00
// Size of code:                   0x00004800
// Size of init.data:              0x00000800
// Size of uninit.data:            0x00000000
// Addr. of entry point:           0x00006622
// Base of code:                   0x00002000
// Base of data:                   0x00008000
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
// Size of headers:                0x00000200
// Checksum:                       0x0000ce1c
// Subsystem:                      0x0003
// DLL characteristics:            0x8560
// Size of stack reserve:          0x00100000
// Size of stack commit:           0x00001000
// Size of heap reserve:           0x00100000
// Size of heap commit:            0x00001000
// Loader flags:                   0x00000000
// Directories:                    0x00000010
// 0x00000000 [0x00000000] address [size] of Export Directory:          
// 0x000065cf [0x0000004f] address [size] of Import Directory:          
// 0x00008000 [0x00000454] address [size] of Resource Directory:        
// 0x00000000 [0x00000000] address [size] of Exception Directory:       
// 0x00005200 [0x00002480] address [size] of Security Directory:        
// 0x0000a000 [0x0000000c] address [size] of Base Relocation Table:     
// 0x000064f4 [0x00000054] address [size] of Debug Directory:           
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
//              0x00004628 Virtual Size
//              0x00002000 Virtual Address
//              0x00004800 Size of Raw Data
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
//              0x00000454 Virtual Size
//              0x00008000 Virtual Address
//              0x00000600 Size of Raw Data
//              0x00004a00 Pointer to Raw Data
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
//              0x00005000 Pointer to Raw Data
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
//              Entry 1: Type 0x3 Offset 0x00000624
//              Entry 2: Type 0x0 Offset 0x00000000

// Import Address Table
//     DLL : mscoree.dll
//              0x00002000 Import Address Table
//              0x00006611 Import Name Table
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
// 0x00002434 [0x00004040] address [size] of Metadata Directory:        
// Flags:                              0x00000009
// Entry point token:                  0x00000000
// 0x00000000 [0x00000000] address [size] of Resources Directory:       
// 0x00006474 [0x00000080] address [size] of Strong Name Signature:     
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
//              0x00002340 Size
//              '#~' Name
//    Stream 2:
//              0x000023ac Offset
//              0x00001390 Size
//              '#Strings' Name
//    Stream 3:
//              0x0000373c Offset
//              0x00000004 Size
//              '#US' Name
//    Stream 4:
//              0x00003740 Offset
//              0x00000010 Size
//              '#GUID' Name
//    Stream 5:
//              0x00003750 Offset
//              0x000008f0 Size
//              '#Blob' Name

//    Metadata Stream Header:
//              0x00000000 Reserved
//                    0x02 Major
//                    0x00 Minor
//                    0x00 Heaps
//                    0x01 Rid
//      0x000002090bb41f57 MaskValid
//      0x000016003301fa00 Sorted

// Code Manager Table:
//  default


// Export Address Table Jumps:
// No data.

// File size            : 30336
// PE header size       : 512 (496 used)    ( 1.69%)
// PE additional info   : 10707             (35.29%)
// Num.of PE sections   : 3
// CLR header size     : 72                 ( 0.24%)
// CLR meta-data size  : 16448              (54.22%)
// CLR additional info : 128                ( 0.42%)
// CLR method headers  : 232                ( 0.76%)
// Managed code         : 764               ( 2.52%)
// Data                 : 2048              ( 6.75%)
// Unaccounted          : -575              (-1.90%)

// Num.of PE sections   : 3
//   .text    - 18432
//   .rsrc    - 1536
//   .reloc   - 512

// CLR meta-data size  : 16448
//   Module        -    1 (10 bytes)
//   TypeDef       -   33 (462 bytes)     0 interfaces, 0 explicit layout
//   TypeRef       -   50 (300 bytes)
//   MethodDef     -  236 (3304 bytes)    0 abstract, 0 native, 232 bodies
//   FieldDef      -   68 (408 bytes)     0 constant
//   MemberRef     -   31 (186 bytes)
//   ParamDef      -  235 (1410 bytes)
//   MethodImpl    -    3 (18 bytes)
//   Constant      -   59 (354 bytes)
//   CustomAttribute-  203 (1218 bytes)
//   InterfaceImpl -    9 (36 bytes)
//   PropertyMap   -   12 (48 bytes)
//   Property      -   61 (366 bytes)
//   MethodSemantic-  104 (624 bytes)
//   TypeSpec      -    4 (8 bytes)
//   Assembly      -    1 (22 bytes)
//   AssemblyRef   -    6 (120 bytes)
//   NestedClass   -    3 (12 bytes)
//   EventMap      -    1 (4 bytes)
//   Event         -    1 (6 bytes)
//   Strings       -  5007 bytes
//   Blobs         -  2288 bytes
//   UserStrings   -     4 bytes
//   Guids         -    16 bytes
//   Uncategorized -   217 bytes

// CLR additional info : 128

// CLR method headers : 232
//   Num.of method bodies  - 232
//   Num.of fat headers    - 0
//   Num.of tiny headers   - 232

// Managed code : 764
//   Ave method size - 3
// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class EmbeddedAttribute              (auto) (ansi) (sealed) 
// Class NullableAttribute              (auto) (ansi) (sealed) 
// Class NullableContextAttribute       (auto) (ansi) (sealed) 
// Class NullablePublicOnlyAttribute    (auto) (ansi) (sealed) 
// Class ContentDisposition             (public) (auto) (ansi) 
// Class ContentType                    (public) (auto) (ansi) 
// Class DispositionTypeNames           (public) (abstract) (auto) (ansi) (sealed) 
// Class MediaTypeNames                 (public) (abstract) (auto) (ansi) (sealed) 
// Class TransferEncoding               (public) (auto) (ansi) (sealed) 
// Class AlternateView                  (public) (auto) (ansi) 
// Class AlternateViewCollection        (public) (auto) (ansi) (sealed) 
// Class Attachment                     (public) (auto) (ansi) 
// Class AttachmentBase                 (public) (abstract) (auto) (ansi) 
// Class AttachmentCollection           (public) (auto) (ansi) (sealed) 
// Class DeliveryNotificationOptions    (public) (auto) (ansi) (sealed) 
// Class LinkedResource                 (public) (auto) (ansi) 
// Class LinkedResourceCollection       (public) (auto) (ansi) (sealed) 
// Class MailAddress                    (public) (auto) (ansi) 
// Class MailAddressCollection          (public) (auto) (ansi) 
// Class MailMessage                    (public) (auto) (ansi) 
// Class MailPriority                   (public) (auto) (ansi) (sealed) 
// Class SendCompletedEventHandler      (public) (auto) (ansi) (sealed) 
// Class SmtpClient                     (public) (auto) (ansi) 
// Class SmtpDeliveryFormat             (public) (auto) (ansi) (sealed) 
// Class SmtpDeliveryMethod             (public) (auto) (ansi) (sealed) 
// Class SmtpException                  (public) (auto) (ansi) 
// Class SmtpFailedRecipientException   (public) (auto) (ansi) 
// Class SmtpFailedRecipientsException  (public) (auto) (ansi) 
// Class SmtpStatusCode                 (public) (auto) (ansi) (sealed) 
// Class Application                    (abstract) (auto) (ansi) (sealed) (nested public) 
// Class Image                          (abstract) (auto) (ansi) (sealed) (nested public) 
// Class Text                           (abstract) (auto) (ansi) (sealed) (nested public) 
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
.assembly extern /*23000003*/ System.ComponentModel.EventBasedAsync
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000004*/ System.Security.Cryptography.X509Certificates
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000005*/ System.Net.Primitives
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A )                         // .?_....:
  .ver 6:0:0:0
}
.assembly extern /*23000006*/ System.Net.ServicePoint
{
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 )                         // .{...-.Q
  .ver 6:0:0:0
}
.assembly /*20000001*/ noplatform System.Net.Mail
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
  .custom /*0C000009:0A000007*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDefaultAliasAttribute/*01000008*/::.ctor(string) /* 0A000007 */ = ( 01 00 0F 53 79 73 74 65 6D 2E 4E 65 74 2E 4D 61   // ...System.Net.Ma
                                                                                                                                                                        69 6C 00 00 )                                     // il..
  .custom /*0C00000A:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 15 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                                                                                    41 73 73 65 6D 62 6C 79 00 00 00 )                // Assembly...
  .custom /*0C00000B:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 53 65 72 76 69 63 65 61 62 6C 65 04 54   // ...Serviceable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000C:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 50 72 65 66 65 72 49 6E 62 6F 78 04 54   // ...PreferInbox.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000D:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0B 49 73 54 72 69 6D 6D 61 62 6C 65 04 54   // ...IsTrimmable.T
                                                                                                                                                                    72 75 65 00 00 )                                  // rue..
  .custom /*0C00000E:0A000009*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCompanyAttribute/*0100000A*/::.ctor(string) /* 0A000009 */ = ( 01 00 15 4D 69 63 72 6F 73 6F 66 74 20 43 6F 72   // ...Microsoft Cor
                                                                                                                                                                   70 6F 72 61 74 69 6F 6E 00 00 )                   // poration..
  .custom /*0C00000F:0A00000A*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyCopyrightAttribute/*0100000B*/::.ctor(string) /* 0A00000A */ = ( 01 00 2E C2 A9 20 4D 69 63 72 6F 73 6F 66 74 20   // ..... Microsoft 
                                                                                                                                                                     43 6F 72 70 6F 72 61 74 69 6F 6E 2E 20 41 6C 6C   // Corporation. All
                                                                                                                                                                     20 72 69 67 68 74 73 20 72 65 73 65 72 76 65 64   //  rights reserved
                                                                                                                                                                     2E 00 00 )                                        // ...
  .custom /*0C000010:0A00000B*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyDescriptionAttribute/*0100000C*/::.ctor(string) /* 0A00000B */ = ( 01 00 0F 53 79 73 74 65 6D 2E 4E 65 74 2E 4D 61   // ...System.Net.Ma
                                                                                                                                                                       69 6C 00 00 )                                     // il..
  .custom /*0C000011:0A00000C*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyFileVersionAttribute/*0100000D*/::.ctor(string) /* 0A00000C */ = ( 01 00 0C 36 2E 30 2E 32 31 2E 35 32 32 31 30 00   // ...6.0.21.52210.
                                                                                                                                                                       00 ) 
  .custom /*0C000012:0A00000D*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyInformationalVersionAttribute/*0100000E*/::.ctor(string) /* 0A00000D */ = ( 01 00 2E 36 2E 30 2E 30 2B 34 38 32 32 65 33 63   // ...6.0.0+4822e3c
                                                                                                                                                                                33 61 61 37 37 65 62 38 32 62 32 66 62 33 33 63   // 3aa77eb82b2fb33c
                                                                                                                                                                                39 33 32 31 66 39 32 33 63 66 31 31 64 64 64 65   // 9321f923cf11ddde
                                                                                                                                                                                36 00 00 )                                        // 6..
  .custom /*0C000013:0A00000E*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyProductAttribute/*0100000F*/::.ctor(string) /* 0A00000E */ = ( 01 00 10 4D 69 63 72 6F 73 6F 66 74 C2 AE 20 2E   // ...Microsoft.. .
                                                                                                                                                                   4E 45 54 00 00 )                                  // NET..
  .custom /*0C000014:0A00000F*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyTitleAttribute/*01000010*/::.ctor(string) /* 0A00000F */ = ( 01 00 0F 53 79 73 74 65 6D 2E 4E 65 74 2E 4D 61   // ...System.Net.Ma
                                                                                                                                                                 69 6C 00 00 )                                     // il..
  .custom /*0C000015:0A000008*/ instance void [System.Runtime/*23000001*/]System.Reflection.AssemblyMetadataAttribute/*01000009*/::.ctor(string,
                                                                                                                                         string) /* 0A000008 */ = ( 01 00 0D 52 65 70 6F 73 69 74 6F 72 79 55 72 6C   // ...RepositoryUrl
                                                                                                                                                                    21 68 74 74 70 73 3A 2F 2F 67 69 74 68 75 62 2E   // !https://github.
                                                                                                                                                                    63 6F 6D 2F 64 6F 74 6E 65 74 2F 72 75 6E 74 69   // com/dotnet/runti
                                                                                                                                                                    6D 65 00 00 )                                     // me..
  .publickey = (00 24 00 00 04 80 00 00 94 00 00 00 06 02 00 00   // .$..............
                00 24 00 00 52 53 41 31 00 04 00 00 01 00 01 00   // .$..RSA1........
                4B 86 C4 CB 78 54 9B 34 BA B6 1A 3B 18 00 E2 3B   // K...xT.4...;...;
                FE B5 B3 EC 39 00 74 04 15 36 A7 E3 CB D9 7F 5F   // ....9.t..6....._
                04 CF 0F 85 71 55 A8 92 8E AA 29 EB FD 11 CF BB   // ....qU....).....
                AD 3B A7 0E FE A7 BD A3 22 6C 6A 8D 37 0A 4C D3   // .;......"lj.7.L.
                03 F7 14 48 6B 6E BC 22 59 85 A6 38 47 1E 6E F5   // ...Hkn."Y..8G.n.
                71 CC 92 A4 61 3C 00 B8 FA 65 D6 1C CE E0 CB E5   // q...a<...e......
                F3 63 30 C9 A0 1F 41 83 55 9F 1B EF 24 CC 29 17   // .c0...A.U...$.).
                C6 D9 13 E3 A5 41 33 3A 1D 05 D9 BE D2 2B 38 CB ) // .....A3:.....+8.
  .hash algorithm 0x00008004
  .ver 6:0:0:0
}
.module System.Net.Mail.dll
// MVID: {ff03666b-e4e8-40ef-9d93-a6b808533a48}
.custom /*0C000002:06000005*/ instance void System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::.ctor(bool) /* 06000005 */ = ( 01 00 00 00 00 ) 
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000009    //  ILONLY


// =============== CLASS MEMBERS DECLARATION ===================

.class /*02000002*/ private auto ansi sealed beforefieldinit Microsoft.CodeAnalysis.EmbeddedAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000016:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000017:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .method /*06000001*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2050
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method EmbeddedAttribute::.ctor

} // end of class Microsoft.CodeAnalysis.EmbeddedAttribute

.class /*02000003*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000019:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001A:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C00001B:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 84 6B 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ...k....T..Allow
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
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
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
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000001       */ stfld      uint8[] System.Runtime.CompilerServices.NullableAttribute/*02000003*/::NullableFlags /* 04000001 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableAttribute

.class /*02000004*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullableContextAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C00001E:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C00001F:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000020:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 4C 14 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ..L.....T..Allow
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
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000002       */ stfld      uint8 System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::Flag /* 04000002 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullableContextAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullableContextAttribute

.class /*02000005*/ private auto ansi sealed beforefieldinit System.Runtime.CompilerServices.NullablePublicOnlyAttribute
       extends [System.Runtime/*23000001*/]System.Attribute/*01000012*/
{
  .custom /*0C000022:0A000010*/ instance void [System.Runtime/*23000001*/]System.Runtime.CompilerServices.CompilerGeneratedAttribute/*01000011*/::.ctor() /* 0A000010 */ = ( 01 00 00 00 ) 
  .custom /*0C000023:06000001*/ instance void Microsoft.CodeAnalysis.EmbeddedAttribute/*02000002*/::.ctor() /* 06000001 */ = ( 01 00 00 00 ) 
  .custom /*0C000024:0A000011*/ instance void [System.Runtime/*23000001*/]System.AttributeUsageAttribute/*01000014*/::.ctor(valuetype [System.Runtime/*23000001*/]System.AttributeTargets/*01000013*/) /* 0A000011 */ = ( 01 00 02 00 00 00 02 00 54 02 0D 41 6C 6C 6F 77   // ........T..Allow
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
    IL_0001:  /* 28   | (0A)000019       */ call       instance void [System.Runtime/*23000001*/]System.Attribute/*01000012*/::.ctor() /* 0A000019 */
    IL_0006:  /* 02   |                  */ ldarg.0
    IL_0007:  /* 03   |                  */ ldarg.1
    IL_0008:  /* 7D   | (04)000003       */ stfld      bool System.Runtime.CompilerServices.NullablePublicOnlyAttribute/*02000005*/::IncludesInternals /* 04000003 */
    IL_000d:  /* 2A   |                  */ ret
  } // end of method NullablePublicOnlyAttribute::.ctor

} // end of class System.Runtime.CompilerServices.NullablePublicOnlyAttribute

.class /*02000006*/ public auto ansi beforefieldinit System.Net.Mime.ContentDisposition
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000026:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000027:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000006*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x209d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::.ctor

  .method /*06000007*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string disposition) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::.ctor

  .method /*06000008*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ 
          get_CreationDate() cil managed
  // SIG: 20 00 11 59
  {
    // Method begins at RVA 0x20ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::get_CreationDate

  .method /*06000009*/ public hidebysig specialname 
          instance void  set_CreationDate(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ 'value') cil managed
  // SIG: 20 01 01 11 59
  {
    // Method begins at RVA 0x20b0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::set_CreationDate

  .method /*0600000A*/ public hidebysig specialname 
          instance string  get_DispositionType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20b2
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::get_DispositionType

  .method /*0600000B*/ public hidebysig specialname 
          instance void  set_DispositionType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20b5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::set_DispositionType

  .method /*0600000C*/ public hidebysig specialname 
          instance string  get_FileName() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000037:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20b7
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::get_FileName

  .method /*0600000D*/ public hidebysig specialname 
          instance void  set_FileName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00003A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ba
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::set_FileName

  .method /*0600000E*/ public hidebysig specialname 
          instance bool  get_Inline() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x20bc
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::get_Inline

  .method /*0600000F*/ public hidebysig specialname 
          instance void  set_Inline(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x20bf
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::set_Inline

  .method /*06000010*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ 
          get_ModificationDate() cil managed
  // SIG: 20 00 11 59
  {
    // Method begins at RVA 0x20c1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::get_ModificationDate

  .method /*06000011*/ public hidebysig specialname 
          instance void  set_ModificationDate(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ 'value') cil managed
  // SIG: 20 01 01 11 59
  {
    // Method begins at RVA 0x20c4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::set_ModificationDate

  .method /*06000012*/ public hidebysig specialname 
          instance class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/ 
          get_Parameters() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x20c6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::get_Parameters

  .method /*06000013*/ public hidebysig specialname 
          instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ 
          get_ReadDate() cil managed
  // SIG: 20 00 11 59
  {
    // Method begins at RVA 0x20c9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::get_ReadDate

  .method /*06000014*/ public hidebysig specialname 
          instance void  set_ReadDate(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ 'value') cil managed
  // SIG: 20 01 01 11 59
  {
    // Method begins at RVA 0x20cc
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::set_ReadDate

  .method /*06000015*/ public hidebysig specialname 
          instance int64  get_Size() cil managed
  // SIG: 20 00 0A
  {
    // Method begins at RVA 0x20ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::get_Size

  .method /*06000016*/ public hidebysig specialname 
          instance void  set_Size(int64 'value') cil managed
  // SIG: 20 01 01 0A
  {
    // Method begins at RVA 0x20d1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentDisposition::set_Size

  .method /*06000017*/ public hidebysig virtual 
          instance bool  Equals(object rparam) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000051:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000009*/ 
    .custom /*0C000031:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x20d3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::Equals

  .method /*06000018*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x20d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::GetHashCode

  .method /*06000019*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20d9
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentDisposition::ToString

  .property /*17000001*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/
          CreationDate()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ System.Net.Mime.ContentDisposition/*02000006*/::get_CreationDate() /* 06000008 */
    .set instance void System.Net.Mime.ContentDisposition/*02000006*/::set_CreationDate(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/) /* 06000009 */
  } // end of property ContentDisposition::CreationDate
  .property /*17000002*/ instance callconv(8) string
          DispositionType()
  {
    .get instance string System.Net.Mime.ContentDisposition/*02000006*/::get_DispositionType() /* 0600000A */
    .set instance void System.Net.Mime.ContentDisposition/*02000006*/::set_DispositionType(string) /* 0600000B */
  } // end of property ContentDisposition::DispositionType
  .property /*17000003*/ instance callconv(8) string
          FileName()
  {
    .custom /*0C00001D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Mime.ContentDisposition/*02000006*/::get_FileName() /* 0600000C */
    .set instance void System.Net.Mime.ContentDisposition/*02000006*/::set_FileName(string) /* 0600000D */
  } // end of property ContentDisposition::FileName
  .property /*17000004*/ instance callconv(8) bool
          Inline()
  {
    .get instance bool System.Net.Mime.ContentDisposition/*02000006*/::get_Inline() /* 0600000E */
    .set instance void System.Net.Mime.ContentDisposition/*02000006*/::set_Inline(bool) /* 0600000F */
  } // end of property ContentDisposition::Inline
  .property /*17000005*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/
          ModificationDate()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ System.Net.Mime.ContentDisposition/*02000006*/::get_ModificationDate() /* 06000010 */
    .set instance void System.Net.Mime.ContentDisposition/*02000006*/::set_ModificationDate(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/) /* 06000011 */
  } // end of property ContentDisposition::ModificationDate
  .property /*17000006*/ instance callconv(8) class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/
          Parameters()
  {
    .get instance class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/ System.Net.Mime.ContentDisposition/*02000006*/::get_Parameters() /* 06000012 */
  } // end of property ContentDisposition::Parameters
  .property /*17000007*/ instance callconv(8) valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/
          ReadDate()
  {
    .get instance valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/ System.Net.Mime.ContentDisposition/*02000006*/::get_ReadDate() /* 06000013 */
    .set instance void System.Net.Mime.ContentDisposition/*02000006*/::set_ReadDate(valuetype [System.Runtime/*23000001*/]System.DateTime/*01000016*/) /* 06000014 */
  } // end of property ContentDisposition::ReadDate
  .property /*17000008*/ instance callconv(8) int64
          Size()
  {
    .get instance int64 System.Net.Mime.ContentDisposition/*02000006*/::get_Size() /* 06000015 */
    .set instance void System.Net.Mime.ContentDisposition/*02000006*/::set_Size(int64) /* 06000016 */
  } // end of property ContentDisposition::Size
} // end of class System.Net.Mime.ContentDisposition

.class /*02000007*/ public auto ansi beforefieldinit System.Net.Mime.ContentType
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C000029:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600001A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x20dc
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentType::.ctor

  .method /*0600001B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string contentType) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20e4
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method ContentType::.ctor

  .method /*0600001C*/ public hidebysig specialname 
          instance string  get_Boundary() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C00005A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ec
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentType::get_Boundary

  .method /*0600001D*/ public hidebysig specialname 
          instance void  set_Boundary(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00005D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20ef
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentType::set_Boundary

  .method /*0600001E*/ public hidebysig specialname 
          instance string  get_CharSet() cil managed
  // SIG: 20 00 0E
  {
    .custom /*0C000061:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentType::get_CharSet

  .method /*0600001F*/ public hidebysig specialname 
          instance void  set_CharSet(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C000062:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x20f4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentType::set_CharSet

  .method /*06000020*/ public hidebysig specialname 
          instance string  get_MediaType() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20f6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentType::get_MediaType

  .method /*06000021*/ public hidebysig specialname 
          instance void  set_MediaType(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x20f9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentType::set_MediaType

  .method /*06000022*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x20fb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentType::get_Name

  .method /*06000023*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800000E*/ 
    .custom /*0C00003F:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000019*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x20fe
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method ContentType::set_Name

  .method /*06000024*/ public hidebysig specialname 
          instance class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/ 
          get_Parameters() cil managed
  // SIG: 20 00 12 5D
  {
    // Method begins at RVA 0x2100
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentType::get_Parameters

  .method /*06000025*/ public hidebysig virtual 
          instance bool  Equals(object rparam) cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C00006A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800000F*/ 
    .custom /*0C000043:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2103
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentType::Equals

  .method /*06000026*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2106
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentType::GetHashCode

  .method /*06000027*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2109
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method ContentType::ToString

  .property /*17000009*/ instance callconv(8) string
          Boundary()
  {
    .custom /*0C000033:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Mime.ContentType/*02000007*/::get_Boundary() /* 0600001C */
    .set instance void System.Net.Mime.ContentType/*02000007*/::set_Boundary(string) /* 0600001D */
  } // end of property ContentType::Boundary
  .property /*1700000A*/ instance callconv(8) string
          CharSet()
  {
    .custom /*0C000034:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance string System.Net.Mime.ContentType/*02000007*/::get_CharSet() /* 0600001E */
    .set instance void System.Net.Mime.ContentType/*02000007*/::set_CharSet(string) /* 0600001F */
  } // end of property ContentType::CharSet
  .property /*1700000B*/ instance callconv(8) string
          MediaType()
  {
    .get instance string System.Net.Mime.ContentType/*02000007*/::get_MediaType() /* 06000020 */
    .set instance void System.Net.Mime.ContentType/*02000007*/::set_MediaType(string) /* 06000021 */
  } // end of property ContentType::MediaType
  .property /*1700000C*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Net.Mime.ContentType/*02000007*/::get_Name() /* 06000022 */
    .set instance void System.Net.Mime.ContentType/*02000007*/::set_Name(string) /* 06000023 */
  } // end of property ContentType::Name
  .property /*1700000D*/ instance callconv(8) class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/
          Parameters()
  {
    .get instance class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.StringDictionary/*01000017*/ System.Net.Mime.ContentType/*02000007*/::get_Parameters() /* 06000024 */
  } // end of property ContentType::Parameters
} // end of class System.Net.Mime.ContentType

.class /*02000008*/ public abstract auto ansi sealed beforefieldinit System.Net.Mime.DispositionTypeNames
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00002C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00002D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .field /*04000004*/ public static literal string Attachment = "attachment"
  .field /*04000005*/ public static literal string Inline = "inline"
} // end of class System.Net.Mime.DispositionTypeNames

.class /*02000009*/ public abstract auto ansi sealed beforefieldinit System.Net.Mime.MediaTypeNames
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00002F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000030:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .class /*0200001F*/ abstract auto ansi sealed nested public beforefieldinit Application
         extends [System.Runtime/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C000063:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000037*/ public static literal string Json = "application/json"
    .field /*04000038*/ public static literal string Octet = "application/octet-stream"
    .field /*04000039*/ public static literal string Pdf = "application/pdf"
    .field /*0400003A*/ public static literal string Rtf = "application/rtf"
    .field /*0400003B*/ public static literal string Soap = "application/soap+xml"
    .field /*0400003C*/ public static literal string Xml = "application/xml"
    .field /*0400003D*/ public static literal string Zip = "application/zip"
  } // end of class Application

  .class /*02000020*/ abstract auto ansi sealed nested public beforefieldinit Image
         extends [System.Runtime/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C000065:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*0400003E*/ public static literal string Gif = "image/gif"
    .field /*0400003F*/ public static literal string Jpeg = "image/jpeg"
    .field /*04000040*/ public static literal string Tiff = "image/tiff"
  } // end of class Image

  .class /*02000021*/ abstract auto ansi sealed nested public beforefieldinit Text
         extends [System.Runtime/*23000001*/]System.Object/*01000015*/
  {
    .custom /*0C000068:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
    .field /*04000041*/ public static literal string Html = "text/html"
    .field /*04000042*/ public static literal string Plain = "text/plain"
    .field /*04000043*/ public static literal string RichText = "text/richtext"
    .field /*04000044*/ public static literal string Xml = "text/xml"
  } // end of class Text

} // end of class System.Net.Mime.MediaTypeNames

.class /*0200000A*/ public auto ansi sealed System.Net.Mime.TransferEncoding
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*04000006*/ public specialname rtspecialname int32 value__
  .field /*04000007*/ public static literal valuetype System.Net.Mime.TransferEncoding/*0200000A*/ Unknown = int32(0xFFFFFFFF)
  .field /*04000008*/ public static literal valuetype System.Net.Mime.TransferEncoding/*0200000A*/ QuotedPrintable = int32(0x00000000)
  .field /*04000009*/ public static literal valuetype System.Net.Mime.TransferEncoding/*0200000A*/ Base64 = int32(0x00000001)
  .field /*0400000A*/ public static literal valuetype System.Net.Mime.TransferEncoding/*0200000A*/ SevenBit = int32(0x00000002)
  .field /*0400000B*/ public static literal valuetype System.Net.Mime.TransferEncoding/*0200000A*/ EightBit = int32(0x00000003)
} // end of class System.Net.Mime.TransferEncoding

.class /*0200000B*/ public auto ansi beforefieldinit System.Net.Mail.AlternateView
       extends System.Net.Mail.AttachmentBase/*0200000E*/
{
  .custom /*0C000035:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000036:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000028*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x210c
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AlternateView::.ctor

  .method /*06000029*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 20 02 01 12 6D 12 1C
  {
    .param [2]/*08000012*/ 
    .custom /*0C000049:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2115
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AlternateView::.ctor

  .method /*0600002A*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               string mediaType) cil managed
  // SIG: 20 02 01 12 6D 0E
  {
    .param [2]/*08000014*/ 
    .custom /*0C00004E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x211e
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AlternateView::.ctor

  .method /*0600002B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2127
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AlternateView::.ctor

  .method /*0600002C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 20 02 01 0E 12 1C
  {
    .param [2]/*08000017*/ 
    .custom /*0C000052:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2130
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AlternateView::.ctor

  .method /*0600002D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               string mediaType) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000019*/ 
    .custom /*0C000057:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2139
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method AlternateView::.ctor

  .method /*0600002E*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*0100001C*/ 
          get_BaseUri() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C000072:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2142
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AlternateView::get_BaseUri

  .method /*0600002F*/ public hidebysig specialname 
          instance void  set_BaseUri(class [System.Runtime/*23000001*/]System.Uri/*0100001C*/ 'value') cil managed
  // SIG: 20 01 01 12 71
  {
    .custom /*0C000073:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2145
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AlternateView::set_BaseUri

  .method /*06000030*/ public hidebysig specialname 
          instance class System.Net.Mail.LinkedResourceCollection/*02000012*/ 
          get_LinkedResources() cil managed
  // SIG: 20 00 12 48
  {
    // Method begins at RVA 0x2147
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AlternateView::get_LinkedResources

  .method /*06000031*/ public hidebysig static 
          class System.Net.Mail.AlternateView/*0200000B*/ 
          CreateAlternateViewFromString(string content) cil managed
  // SIG: 00 01 12 2C 0E
  {
    // Method begins at RVA 0x214a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AlternateView::CreateAlternateViewFromString

  .method /*06000032*/ public hidebysig static 
          class System.Net.Mail.AlternateView/*0200000B*/ 
          CreateAlternateViewFromString(string content,
                                        class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 00 02 12 2C 0E 12 1C
  {
    .param [2]/*0800001D*/ 
    .custom /*0C000060:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x214d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AlternateView::CreateAlternateViewFromString

  .method /*06000033*/ public hidebysig static 
          class System.Net.Mail.AlternateView/*0200000B*/ 
          CreateAlternateViewFromString(string content,
                                        class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ contentEncoding,
                                        string mediaType) cil managed
  // SIG: 00 03 12 2C 0E 12 75 0E
  {
    .param [2]/*0800001F*/ 
    .custom /*0C000064:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000020*/ 
    .custom /*0C000066:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2150
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AlternateView::CreateAlternateViewFromString

  .method /*06000034*/ family hidebysig virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2153
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AlternateView::Dispose

  .property /*1700000E*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*0100001C*/
          BaseUri()
  {
    .custom /*0C000040:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*0100001C*/ System.Net.Mail.AlternateView/*0200000B*/::get_BaseUri() /* 0600002E */
    .set instance void System.Net.Mail.AlternateView/*0200000B*/::set_BaseUri(class [System.Runtime/*23000001*/]System.Uri/*0100001C*/) /* 0600002F */
  } // end of property AlternateView::BaseUri
  .property /*1700000F*/ instance callconv(8) class System.Net.Mail.LinkedResourceCollection/*02000012*/
          LinkedResources()
  {
    .get instance class System.Net.Mail.LinkedResourceCollection/*02000012*/ System.Net.Mail.AlternateView/*0200000B*/::get_LinkedResources() /* 06000030 */
  } // end of property AlternateView::LinkedResources
} // end of class System.Net.Mail.AlternateView

.class /*0200000C*/ public auto ansi sealed beforefieldinit System.Net.Mail.AlternateViewCollection
       extends class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100001E*/<class System.Net.Mail.AlternateView/*0200000B*/>/*1B000001*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
{
  .custom /*0C000038:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000039:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
  .custom /*0C000001:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000035*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2155
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001B       */ call       instance void class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100001E*/<class System.Net.Mail.AlternateView/*0200000B*/>/*1B000001*/::.ctor() /* 0A00001B */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AlternateViewCollection::.ctor

  .method /*06000036*/ family hidebysig virtual 
          instance void  ClearItems() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x215d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AlternateViewCollection::ClearItems

  .method /*06000037*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x215f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AlternateViewCollection::Dispose

  .method /*06000038*/ family hidebysig virtual 
          instance void  InsertItem(int32 index,
                                    class System.Net.Mail.AlternateView/*0200000B*/ item) cil managed
  // SIG: 20 02 01 08 12 2C
  {
    // Method begins at RVA 0x2161
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AlternateViewCollection::InsertItem

  .method /*06000039*/ family hidebysig virtual 
          instance void  RemoveItem(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2163
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AlternateViewCollection::RemoveItem

  .method /*0600003A*/ family hidebysig virtual 
          instance void  SetItem(int32 index,
                                 class System.Net.Mail.AlternateView/*0200000B*/ item) cil managed
  // SIG: 20 02 01 08 12 2C
  {
    // Method begins at RVA 0x2165
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AlternateViewCollection::SetItem

} // end of class System.Net.Mail.AlternateViewCollection

.class /*0200000D*/ public auto ansi beforefieldinit System.Net.Mail.Attachment
       extends System.Net.Mail.AttachmentBase/*0200000E*/
{
  .custom /*0C00003B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00003C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600003B*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 20 02 01 12 6D 12 1C
  {
    .custom /*0C00007A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2167
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Attachment::.ctor

  .method /*0600003C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               string name) cil managed
  // SIG: 20 02 01 12 6D 0E
  {
    .custom /*0C00007B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*0800002A*/ 
    .custom /*0C00006E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2170
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Attachment::.ctor

  .method /*0600003D*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               string name,
                               string mediaType) cil managed
  // SIG: 20 03 01 12 6D 0E 0E
  {
    .param [1]/*0800002B*/ 
    .custom /*0C00006F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2179
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Attachment::.ctor

  .method /*0600003E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C00007C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2182
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Attachment::.ctor

  .method /*0600003F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 20 02 01 0E 12 1C
  {
    .custom /*0C00007D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x218b
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Attachment::.ctor

  .method /*06000040*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               string mediaType) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .custom /*0C00007E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000032*/ 
    .custom /*0C000075:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2194
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method Attachment::.ctor

  .method /*06000041*/ public hidebysig specialname 
          instance class System.Net.Mime.ContentDisposition/*02000006*/ 
          get_ContentDisposition() cil managed
  // SIG: 20 00 12 18
  {
    // Method begins at RVA 0x219d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Attachment::get_ContentDisposition

  .method /*06000042*/ public hidebysig specialname 
          instance string  get_Name() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21a0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Attachment::get_Name

  .method /*06000043*/ public hidebysig specialname 
          instance void  set_Name(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21a3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Attachment::set_Name

  .method /*06000044*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ 
          get_NameEncoding() cil managed
  // SIG: 20 00 12 75
  {
    // Method begins at RVA 0x21a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Attachment::get_NameEncoding

  .method /*06000045*/ public hidebysig specialname 
          instance void  set_NameEncoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 12 75
  {
    // Method begins at RVA 0x21a8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method Attachment::set_NameEncoding

  .method /*06000046*/ public hidebysig static 
          class System.Net.Mail.Attachment/*0200000D*/ 
          CreateAttachmentFromString(string content,
                                     class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 00 02 12 34 0E 12 1C
  {
    .custom /*0C000082:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21aa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Attachment::CreateAttachmentFromString

  .method /*06000047*/ public hidebysig static 
          class System.Net.Mail.Attachment/*0200000D*/ 
          CreateAttachmentFromString(string content,
                                     string name) cil managed
  // SIG: 00 02 12 34 0E 0E
  {
    .custom /*0C000083:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000038*/ 
    .custom /*0C000077:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21ad
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Attachment::CreateAttachmentFromString

  .method /*06000048*/ public hidebysig static 
          class System.Net.Mail.Attachment/*0200000D*/ 
          CreateAttachmentFromString(string content,
                                     string name,
                                     class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ contentEncoding,
                                     string mediaType) cil managed
  // SIG: 00 04 12 34 0E 0E 12 75 0E
  {
    .param [0]/*08000039*/ 
    .custom /*0C000078:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [1]/*0800003A*/ 
    .custom /*0C000079:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x21b0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method Attachment::CreateAttachmentFromString

  .property /*17000010*/ instance callconv(8) class System.Net.Mime.ContentDisposition/*02000006*/
          ContentDisposition()
  {
    .get instance class System.Net.Mime.ContentDisposition/*02000006*/ System.Net.Mail.Attachment/*0200000D*/::get_ContentDisposition() /* 06000041 */
  } // end of property Attachment::ContentDisposition
  .property /*17000011*/ instance callconv(8) string
          Name()
  {
    .get instance string System.Net.Mail.Attachment/*0200000D*/::get_Name() /* 06000042 */
    .set instance void System.Net.Mail.Attachment/*0200000D*/::set_Name(string) /* 06000043 */
  } // end of property Attachment::Name
  .property /*17000012*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/
          NameEncoding()
  {
    .get instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ System.Net.Mail.Attachment/*0200000D*/::get_NameEncoding() /* 06000044 */
    .set instance void System.Net.Mail.Attachment/*0200000D*/::set_NameEncoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/) /* 06000045 */
  } // end of property Attachment::NameEncoding
} // end of class System.Net.Mail.Attachment

.class /*0200000E*/ public abstract auto ansi beforefieldinit System.Net.Mail.AttachmentBase
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
{
  .custom /*0C00003D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00003E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
  .custom /*0C000018:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000049*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x21b3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::.ctor

  .method /*0600004A*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 20 02 01 12 6D 12 1C
  {
    .param [2]/*08000040*/ 
    .custom /*0C00007F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21bb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::.ctor

  .method /*0600004B*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               string mediaType) cil managed
  // SIG: 20 02 01 12 6D 0E
  {
    .param [2]/*08000042*/ 
    .custom /*0C000080:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21c3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::.ctor

  .method /*0600004C*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x21cb
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::.ctor

  .method /*0600004D*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 20 02 01 0E 12 1C
  {
    .param [2]/*08000045*/ 
    .custom /*0C000081:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21d3
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::.ctor

  .method /*0600004E*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               string mediaType) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000047*/ 
    .custom /*0C000084:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x21db
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::.ctor

  .method /*0600004F*/ public hidebysig specialname 
          instance string  get_ContentId() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x21e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttachmentBase::get_ContentId

  .method /*06000050*/ public hidebysig specialname 
          instance void  set_ContentId(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*08000048*/ 
    .custom /*0C000085:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000019*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x21e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::set_ContentId

  .method /*06000051*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ 
          get_ContentStream() cil managed
  // SIG: 20 00 12 6D
  {
    // Method begins at RVA 0x21e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttachmentBase::get_ContentStream

  .method /*06000052*/ public hidebysig specialname 
          instance class System.Net.Mime.ContentType/*02000007*/ 
          get_ContentType() cil managed
  // SIG: 20 00 12 1C
  {
    // Method begins at RVA 0x21eb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttachmentBase::get_ContentType

  .method /*06000053*/ public hidebysig specialname 
          instance void  set_ContentType(class System.Net.Mime.ContentType/*02000007*/ 'value') cil managed
  // SIG: 20 01 01 12 1C
  {
    // Method begins at RVA 0x21ee
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::set_ContentType

  .method /*06000054*/ public hidebysig specialname 
          instance valuetype System.Net.Mime.TransferEncoding/*0200000A*/ 
          get_TransferEncoding() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x21f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method AttachmentBase::get_TransferEncoding

  .method /*06000055*/ public hidebysig specialname 
          instance void  set_TransferEncoding(valuetype System.Net.Mime.TransferEncoding/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x21f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::set_TransferEncoding

  .method /*06000056*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21f5
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::Dispose

  .method /*06000057*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x21f7
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentBase::Dispose

  .property /*17000013*/ instance callconv(8) string
          ContentId()
  {
    .get instance string System.Net.Mail.AttachmentBase/*0200000E*/::get_ContentId() /* 0600004F */
    .set instance void System.Net.Mail.AttachmentBase/*0200000E*/::set_ContentId(string) /* 06000050 */
  } // end of property AttachmentBase::ContentId
  .property /*17000014*/ instance callconv(8) class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/
          ContentStream()
  {
    .get instance class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ System.Net.Mail.AttachmentBase/*0200000E*/::get_ContentStream() /* 06000051 */
  } // end of property AttachmentBase::ContentStream
  .property /*17000015*/ instance callconv(8) class System.Net.Mime.ContentType/*02000007*/
          ContentType()
  {
    .get instance class System.Net.Mime.ContentType/*02000007*/ System.Net.Mail.AttachmentBase/*0200000E*/::get_ContentType() /* 06000052 */
    .set instance void System.Net.Mail.AttachmentBase/*0200000E*/::set_ContentType(class System.Net.Mime.ContentType/*02000007*/) /* 06000053 */
  } // end of property AttachmentBase::ContentType
  .property /*17000016*/ instance callconv(8) valuetype System.Net.Mime.TransferEncoding/*0200000A*/
          TransferEncoding()
  {
    .get instance valuetype System.Net.Mime.TransferEncoding/*0200000A*/ System.Net.Mail.AttachmentBase/*0200000E*/::get_TransferEncoding() /* 06000054 */
    .set instance void System.Net.Mail.AttachmentBase/*0200000E*/::set_TransferEncoding(valuetype System.Net.Mime.TransferEncoding/*0200000A*/) /* 06000055 */
  } // end of property AttachmentBase::TransferEncoding
} // end of class System.Net.Mail.AttachmentBase

.class /*0200000F*/ public auto ansi sealed beforefieldinit System.Net.Mail.AttachmentCollection
       extends class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100001E*/<class System.Net.Mail.Attachment/*0200000D*/>/*1B000002*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
{
  .custom /*0C000041:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000042:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
  .custom /*0C00001C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000058*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x21f9
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001C       */ call       instance void class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100001E*/<class System.Net.Mail.Attachment/*0200000D*/>/*1B000002*/::.ctor() /* 0A00001C */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method AttachmentCollection::.ctor

  .method /*06000059*/ family hidebysig virtual 
          instance void  ClearItems() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2201
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentCollection::ClearItems

  .method /*0600005A*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2203
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentCollection::Dispose

  .method /*0600005B*/ family hidebysig virtual 
          instance void  InsertItem(int32 index,
                                    class System.Net.Mail.Attachment/*0200000D*/ item) cil managed
  // SIG: 20 02 01 08 12 34
  {
    // Method begins at RVA 0x2205
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentCollection::InsertItem

  .method /*0600005C*/ family hidebysig virtual 
          instance void  RemoveItem(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2207
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentCollection::RemoveItem

  .method /*0600005D*/ family hidebysig virtual 
          instance void  SetItem(int32 index,
                                 class System.Net.Mail.Attachment/*0200000D*/ item) cil managed
  // SIG: 20 02 01 08 12 34
  {
    // Method begins at RVA 0x2209
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method AttachmentCollection::SetItem

} // end of class System.Net.Mail.AttachmentCollection

.class /*02000010*/ public auto ansi sealed System.Net.Mail.DeliveryNotificationOptions
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .custom /*0C000044:0A000014*/ instance void [System.Runtime/*23000001*/]System.FlagsAttribute/*01000020*/::.ctor() /* 0A000014 */ = ( 01 00 00 00 ) 
  .field /*0400000C*/ public specialname rtspecialname int32 value__
  .field /*0400000D*/ public static literal valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/ None = int32(0x00000000)
  .field /*0400000E*/ public static literal valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/ OnSuccess = int32(0x00000001)
  .field /*0400000F*/ public static literal valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/ OnFailure = int32(0x00000002)
  .field /*04000010*/ public static literal valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/ Delay = int32(0x00000004)
  .field /*04000011*/ public static literal valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/ Never = int32(0x08000000)
} // end of class System.Net.Mail.DeliveryNotificationOptions

.class /*02000011*/ public auto ansi beforefieldinit System.Net.Mail.LinkedResource
       extends System.Net.Mail.AttachmentBase/*0200000E*/
{
  .custom /*0C000045:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000046:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600005E*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream) cil managed
  // SIG: 20 01 01 12 6D
  {
    // Method begins at RVA 0x220b
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method LinkedResource::.ctor

  .method /*0600005F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 20 02 01 12 6D 12 1C
  {
    .param [2]/*08000053*/ 
    .custom /*0C000086:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2214
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method LinkedResource::.ctor

  .method /*06000060*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/ contentStream,
                               string mediaType) cil managed
  // SIG: 20 02 01 12 6D 0E
  {
    .param [2]/*08000055*/ 
    .custom /*0C000087:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x221d
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method LinkedResource::.ctor

  .method /*06000061*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2226
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method LinkedResource::.ctor

  .method /*06000062*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 20 02 01 0E 12 1C
  {
    .param [2]/*08000058*/ 
    .custom /*0C000088:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x222f
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method LinkedResource::.ctor

  .method /*06000063*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string fileName,
                               string mediaType) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*0800005A*/ 
    .custom /*0C000089:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2238
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 14   |                  */ ldnull
    IL_0002:  /* 28   | (06)000049       */ call       instance void System.Net.Mail.AttachmentBase/*0200000E*/::.ctor(class [System.Runtime/*23000001*/]System.IO.Stream/*0100001B*/) /* 06000049 */
    IL_0007:  /* 2A   |                  */ ret
  } // end of method LinkedResource::.ctor

  .method /*06000064*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Uri/*0100001C*/ 
          get_ContentLink() cil managed
  // SIG: 20 00 12 71
  {
    .custom /*0C00008D:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2241
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkedResource::get_ContentLink

  .method /*06000065*/ public hidebysig specialname 
          instance void  set_ContentLink(class [System.Runtime/*23000001*/]System.Uri/*0100001C*/ 'value') cil managed
  // SIG: 20 01 01 12 71
  {
    .custom /*0C00008E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2244
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LinkedResource::set_ContentLink

  .method /*06000066*/ public hidebysig static 
          class System.Net.Mail.LinkedResource/*02000011*/ 
          CreateLinkedResourceFromString(string content) cil managed
  // SIG: 00 01 12 44 0E
  {
    // Method begins at RVA 0x2246
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkedResource::CreateLinkedResourceFromString

  .method /*06000067*/ public hidebysig static 
          class System.Net.Mail.LinkedResource/*02000011*/ 
          CreateLinkedResourceFromString(string content,
                                         class System.Net.Mime.ContentType/*02000007*/ contentType) cil managed
  // SIG: 00 02 12 44 0E 12 1C
  {
    .param [2]/*0800005E*/ 
    .custom /*0C00008A:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2249
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkedResource::CreateLinkedResourceFromString

  .method /*06000068*/ public hidebysig static 
          class System.Net.Mail.LinkedResource/*02000011*/ 
          CreateLinkedResourceFromString(string content,
                                         class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ contentEncoding,
                                         string mediaType) cil managed
  // SIG: 00 03 12 44 0E 12 75 0E
  {
    .param [2]/*08000060*/ 
    .custom /*0C00008B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [3]/*08000061*/ 
    .custom /*0C00008C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x224c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method LinkedResource::CreateLinkedResourceFromString

  .property /*17000017*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Uri/*0100001C*/
          ContentLink()
  {
    .custom /*0C000053:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Uri/*0100001C*/ System.Net.Mail.LinkedResource/*02000011*/::get_ContentLink() /* 06000064 */
    .set instance void System.Net.Mail.LinkedResource/*02000011*/::set_ContentLink(class [System.Runtime/*23000001*/]System.Uri/*0100001C*/) /* 06000065 */
  } // end of property LinkedResource::ContentLink
} // end of class System.Net.Mail.LinkedResource

.class /*02000012*/ public auto ansi sealed beforefieldinit System.Net.Mail.LinkedResourceCollection
       extends class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100001E*/<class System.Net.Mail.LinkedResource/*02000011*/>/*1B000003*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
{
  .custom /*0C000047:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000048:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
  .custom /*0C000021:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000069*/ assembly hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x224f
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001D       */ call       instance void class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100001E*/<class System.Net.Mail.LinkedResource/*02000011*/>/*1B000003*/::.ctor() /* 0A00001D */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method LinkedResourceCollection::.ctor

  .method /*0600006A*/ family hidebysig virtual 
          instance void  ClearItems() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2257
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LinkedResourceCollection::ClearItems

  .method /*0600006B*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2259
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LinkedResourceCollection::Dispose

  .method /*0600006C*/ family hidebysig virtual 
          instance void  InsertItem(int32 index,
                                    class System.Net.Mail.LinkedResource/*02000011*/ item) cil managed
  // SIG: 20 02 01 08 12 44
  {
    // Method begins at RVA 0x225b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LinkedResourceCollection::InsertItem

  .method /*0600006D*/ family hidebysig virtual 
          instance void  RemoveItem(int32 index) cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x225d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LinkedResourceCollection::RemoveItem

  .method /*0600006E*/ family hidebysig virtual 
          instance void  SetItem(int32 index,
                                 class System.Net.Mail.LinkedResource/*02000011*/ item) cil managed
  // SIG: 20 02 01 08 12 44
  {
    // Method begins at RVA 0x225f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method LinkedResourceCollection::SetItem

} // end of class System.Net.Mail.LinkedResourceCollection

.class /*02000013*/ public auto ansi beforefieldinit System.Net.Mail.MailAddress
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
{
  .custom /*0C00004A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*0600006F*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string address) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2261
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MailAddress::.ctor

  .method /*06000070*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string address,
                               string displayName) cil managed
  // SIG: 20 02 01 0E 0E
  {
    .param [2]/*08000069*/ 
    .custom /*0C00008F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2269
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MailAddress::.ctor

  .method /*06000071*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string address,
                               string displayName,
                               class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ displayNameEncoding) cil managed
  // SIG: 20 03 01 0E 0E 12 75
  {
    .custom /*0C000095:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800006A*/ 
    .custom /*0C000090:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2271
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MailAddress::.ctor

  .method /*06000072*/ public hidebysig specialname 
          instance string  get_Address() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2279
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::get_Address

  .method /*06000073*/ public hidebysig specialname 
          instance string  get_DisplayName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x227c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::get_DisplayName

  .method /*06000074*/ public hidebysig specialname 
          instance string  get_Host() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x227f
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::get_Host

  .method /*06000075*/ public hidebysig specialname 
          instance string  get_User() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2282
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::get_User

  .method /*06000076*/ public hidebysig virtual 
          instance bool  Equals(object 'value') cil managed
  // SIG: 20 01 02 1C
  {
    .custom /*0C000098:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800006D*/ 
    .custom /*0C000091:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2285
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::Equals

  .method /*06000077*/ public hidebysig virtual 
          instance int32  GetHashCode() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2288
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::GetHashCode

  .method /*06000078*/ public hidebysig static 
          bool  TryCreate(string address,
                          [out] class System.Net.Mail.MailAddress/*02000013*/& result) cil managed
  // SIG: 00 02 02 0E 10 12 4C
  {
    .param [2]/*0800006F*/ 
    .custom /*0C000092:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C000093:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::TryCreate

  .method /*06000079*/ public hidebysig static 
          bool  TryCreate(string address,
                          string displayName,
                          [out] class System.Net.Mail.MailAddress/*02000013*/& result) cil managed
  // SIG: 00 03 02 0E 0E 10 12 4C
  {
    .custom /*0C00009A:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000070*/ 
    .custom /*0C000094:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [3]/*08000072*/ 
    .custom /*0C000096:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x228e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::TryCreate

  .method /*0600007A*/ public hidebysig static 
          bool  TryCreate(string address,
                          string displayName,
                          class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ displayNameEncoding,
                          [out] class System.Net.Mail.MailAddress/*02000013*/& result) cil managed
  // SIG: 00 04 02 0E 0E 12 75 10 12 4C
  {
    .custom /*0C00009B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000073*/ 
    .custom /*0C000097:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [4]/*08000076*/ 
    .custom /*0C000099:0A000012*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.NotNullWhenAttribute/*01000018*/::.ctor(bool) /* 0A000012 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2291
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::TryCreate

  .method /*0600007B*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2294
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddress::ToString

  .property /*17000018*/ instance callconv(8) string
          Address()
  {
    .get instance string System.Net.Mail.MailAddress/*02000013*/::get_Address() /* 06000072 */
  } // end of property MailAddress::Address
  .property /*17000019*/ instance callconv(8) string
          DisplayName()
  {
    .get instance string System.Net.Mail.MailAddress/*02000013*/::get_DisplayName() /* 06000073 */
  } // end of property MailAddress::DisplayName
  .property /*1700001A*/ instance callconv(8) string
          Host()
  {
    .get instance string System.Net.Mail.MailAddress/*02000013*/::get_Host() /* 06000074 */
  } // end of property MailAddress::Host
  .property /*1700001B*/ instance callconv(8) string
          User()
  {
    .get instance string System.Net.Mail.MailAddress/*02000013*/::get_User() /* 06000075 */
  } // end of property MailAddress::User
} // end of class System.Net.Mail.MailAddress

.class /*02000014*/ public auto ansi beforefieldinit System.Net.Mail.MailAddressCollection
       extends class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100001E*/<class System.Net.Mail.MailAddress/*02000013*/>/*1B000004*/
{
  .custom /*0C00004C:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00004D:06000003*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8[]) /* 06000003 */ = ( 01 00 02 00 00 00 00 01 00 00 ) 
  .method /*0600007C*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2297
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001E       */ call       instance void class [System.Runtime/*23000001*/]System.Collections.ObjectModel.Collection`1/*0100001E*/<class System.Net.Mail.MailAddress/*02000013*/>/*1B000004*/::.ctor() /* 0A00001E */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MailAddressCollection::.ctor

  .method /*0600007D*/ public hidebysig instance void 
          Add(string addresses) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x229f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailAddressCollection::Add

  .method /*0600007E*/ family hidebysig virtual 
          instance void  InsertItem(int32 index,
                                    class System.Net.Mail.MailAddress/*02000013*/ item) cil managed
  // SIG: 20 02 01 08 12 4C
  {
    // Method begins at RVA 0x22a1
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailAddressCollection::InsertItem

  .method /*0600007F*/ family hidebysig virtual 
          instance void  SetItem(int32 index,
                                 class System.Net.Mail.MailAddress/*02000013*/ item) cil managed
  // SIG: 20 02 01 08 12 4C
  {
    // Method begins at RVA 0x22a3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailAddressCollection::SetItem

  .method /*06000080*/ public hidebysig virtual 
          instance string  ToString() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22a5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailAddressCollection::ToString

} // end of class System.Net.Mail.MailAddressCollection

.class /*02000015*/ public auto ansi beforefieldinit System.Net.Mail.MailMessage
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
{
  .custom /*0C00004F:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C000050:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
  .custom /*0C000025:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*06000081*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x22a8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MailMessage::.ctor

  .method /*06000082*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(class System.Net.Mail.MailAddress/*02000013*/ from,
                               class System.Net.Mail.MailAddress/*02000013*/ 'to') cil managed
  // SIG: 20 02 01 12 4C 12 4C
  {
    // Method begins at RVA 0x22b0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MailMessage::.ctor

  .method /*06000083*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string from,
                               string 'to') cil managed
  // SIG: 20 02 01 0E 0E
  {
    // Method begins at RVA 0x22b8
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MailMessage::.ctor

  .method /*06000084*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string from,
                               string 'to',
                               string subject,
                               string body) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    .param [3]/*08000082*/ 
    .custom /*0C00009C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*08000083*/ 
    .custom /*0C00009D:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22c0
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method MailMessage::.ctor

  .method /*06000085*/ public hidebysig specialname 
          instance class System.Net.Mail.AlternateViewCollection/*0200000C*/ 
          get_AlternateViews() cil managed
  // SIG: 20 00 12 30
  {
    // Method begins at RVA 0x22c8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_AlternateViews

  .method /*06000086*/ public hidebysig specialname 
          instance class System.Net.Mail.AttachmentCollection/*0200000F*/ 
          get_Attachments() cil managed
  // SIG: 20 00 12 3C
  {
    // Method begins at RVA 0x22cb
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_Attachments

  .method /*06000087*/ public hidebysig specialname 
          instance class System.Net.Mail.MailAddressCollection/*02000014*/ 
          get_Bcc() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x22ce
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_Bcc

  .method /*06000088*/ public hidebysig specialname 
          instance string  get_Body() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x22d1
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_Body

  .method /*06000089*/ public hidebysig specialname 
          instance void  set_Body(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*08000084*/ 
    .custom /*0C00009E:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000019*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22d4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_Body

  .method /*0600008A*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ 
          get_BodyEncoding() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000A0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d6
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_BodyEncoding

  .method /*0600008B*/ public hidebysig specialname 
          instance void  set_BodyEncoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C0000A1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22d9
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_BodyEncoding

  .method /*0600008C*/ public hidebysig specialname 
          instance valuetype System.Net.Mime.TransferEncoding/*0200000A*/ 
          get_BodyTransferEncoding() cil managed
  // SIG: 20 00 11 28
  {
    // Method begins at RVA 0x22db
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_BodyTransferEncoding

  .method /*0600008D*/ public hidebysig specialname 
          instance void  set_BodyTransferEncoding(valuetype System.Net.Mime.TransferEncoding/*0200000A*/ 'value') cil managed
  // SIG: 20 01 01 11 28
  {
    // Method begins at RVA 0x22de
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_BodyTransferEncoding

  .method /*0600008E*/ public hidebysig specialname 
          instance class System.Net.Mail.MailAddressCollection/*02000014*/ 
          get_CC() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x22e0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_CC

  .method /*0600008F*/ public hidebysig specialname 
          instance valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/ 
          get_DeliveryNotificationOptions() cil managed
  // SIG: 20 00 11 40
  {
    // Method begins at RVA 0x22e3
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_DeliveryNotificationOptions

  .method /*06000090*/ public hidebysig specialname 
          instance void  set_DeliveryNotificationOptions(valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/ 'value') cil managed
  // SIG: 20 01 01 11 40
  {
    // Method begins at RVA 0x22e6
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_DeliveryNotificationOptions

  .method /*06000091*/ public hidebysig specialname 
          instance class System.Net.Mail.MailAddress/*02000013*/ 
          get_From() cil managed
  // SIG: 20 00 12 4C
  {
    .custom /*0C0000A4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22e8
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_From

  .method /*06000092*/ public hidebysig specialname 
          instance void  set_From(class System.Net.Mail.MailAddress/*02000013*/ 'value') cil managed
  // SIG: 20 01 01 12 4C
  {
    .custom /*0C0000A5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*08000088*/ 
    .custom /*0C00009F:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000021*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x22eb
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_From

  .method /*06000093*/ public hidebysig specialname 
          instance class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.NameValueCollection/*01000022*/ 
          get_Headers() cil managed
  // SIG: 20 00 12 80 89
  {
    // Method begins at RVA 0x22ed
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_Headers

  .method /*06000094*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ 
          get_HeadersEncoding() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000A6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f0
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_HeadersEncoding

  .method /*06000095*/ public hidebysig specialname 
          instance void  set_HeadersEncoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C0000A7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22f3
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_HeadersEncoding

  .method /*06000096*/ public hidebysig specialname 
          instance bool  get_IsBodyHtml() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x22f5
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_IsBodyHtml

  .method /*06000097*/ public hidebysig specialname 
          instance void  set_IsBodyHtml(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x22f8
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_IsBodyHtml

  .method /*06000098*/ public hidebysig specialname 
          instance valuetype System.Net.Mail.MailPriority/*02000016*/ 
          get_Priority() cil managed
  // SIG: 20 00 11 58
  {
    // Method begins at RVA 0x22fa
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_Priority

  .method /*06000099*/ public hidebysig specialname 
          instance void  set_Priority(valuetype System.Net.Mail.MailPriority/*02000016*/ 'value') cil managed
  // SIG: 20 01 01 11 58
  {
    // Method begins at RVA 0x22fd
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_Priority

  .method /*0600009A*/ public hidebysig specialname 
          instance class System.Net.Mail.MailAddress/*02000013*/ 
          get_ReplyTo() cil managed
  // SIG: 20 00 12 4C
  {
    .custom /*0C0000AA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x22ff
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_ReplyTo

  .method /*0600009B*/ public hidebysig specialname 
          instance void  set_ReplyTo(class System.Net.Mail.MailAddress/*02000013*/ 'value') cil managed
  // SIG: 20 01 01 12 4C
  {
    .custom /*0C0000AB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2302
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_ReplyTo

  .method /*0600009C*/ public hidebysig specialname 
          instance class System.Net.Mail.MailAddressCollection/*02000014*/ 
          get_ReplyToList() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x2304
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_ReplyToList

  .method /*0600009D*/ public hidebysig specialname 
          instance class System.Net.Mail.MailAddress/*02000013*/ 
          get_Sender() cil managed
  // SIG: 20 00 12 4C
  {
    .custom /*0C0000AC:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2307
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_Sender

  .method /*0600009E*/ public hidebysig specialname 
          instance void  set_Sender(class System.Net.Mail.MailAddress/*02000013*/ 'value') cil managed
  // SIG: 20 01 01 12 4C
  {
    .custom /*0C0000AD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    .param [1]/*0800008D*/ 
    .custom /*0C0000A2:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000021*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x230a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_Sender

  .method /*0600009F*/ public hidebysig specialname 
          instance string  get_Subject() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x230c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_Subject

  .method /*060000A0*/ public hidebysig specialname 
          instance void  set_Subject(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*0800008E*/ 
    .custom /*0C0000A3:0A000013*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.AllowNullAttribute/*01000019*/::.ctor() /* 0A000013 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x230f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_Subject

  .method /*060000A1*/ public hidebysig specialname 
          instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ 
          get_SubjectEncoding() cil managed
  // SIG: 20 00 12 75
  {
    .custom /*0C0000AE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2311
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_SubjectEncoding

  .method /*060000A2*/ public hidebysig specialname 
          instance void  set_SubjectEncoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ 'value') cil managed
  // SIG: 20 01 01 12 75
  {
    .custom /*0C0000B0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2314
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::set_SubjectEncoding

  .method /*060000A3*/ public hidebysig specialname 
          instance class System.Net.Mail.MailAddressCollection/*02000014*/ 
          get_To() cil managed
  // SIG: 20 00 12 50
  {
    // Method begins at RVA 0x2316
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method MailMessage::get_To

  .method /*060000A4*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2319
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::Dispose

  .method /*060000A5*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x231b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method MailMessage::Dispose

  .property /*1700001C*/ instance callconv(8) class System.Net.Mail.AlternateViewCollection/*0200000C*/
          AlternateViews()
  {
    .get instance class System.Net.Mail.AlternateViewCollection/*0200000C*/ System.Net.Mail.MailMessage/*02000015*/::get_AlternateViews() /* 06000085 */
  } // end of property MailMessage::AlternateViews
  .property /*1700001D*/ instance callconv(8) class System.Net.Mail.AttachmentCollection/*0200000F*/
          Attachments()
  {
    .get instance class System.Net.Mail.AttachmentCollection/*0200000F*/ System.Net.Mail.MailMessage/*02000015*/::get_Attachments() /* 06000086 */
  } // end of property MailMessage::Attachments
  .property /*1700001E*/ instance callconv(8) class System.Net.Mail.MailAddressCollection/*02000014*/
          Bcc()
  {
    .get instance class System.Net.Mail.MailAddressCollection/*02000014*/ System.Net.Mail.MailMessage/*02000015*/::get_Bcc() /* 06000087 */
  } // end of property MailMessage::Bcc
  .property /*1700001F*/ instance callconv(8) string
          Body()
  {
    .get instance string System.Net.Mail.MailMessage/*02000015*/::get_Body() /* 06000088 */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_Body(string) /* 06000089 */
  } // end of property MailMessage::Body
  .property /*17000020*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/
          BodyEncoding()
  {
    .custom /*0C000067:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ System.Net.Mail.MailMessage/*02000015*/::get_BodyEncoding() /* 0600008A */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_BodyEncoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/) /* 0600008B */
  } // end of property MailMessage::BodyEncoding
  .property /*17000021*/ instance callconv(8) valuetype System.Net.Mime.TransferEncoding/*0200000A*/
          BodyTransferEncoding()
  {
    .get instance valuetype System.Net.Mime.TransferEncoding/*0200000A*/ System.Net.Mail.MailMessage/*02000015*/::get_BodyTransferEncoding() /* 0600008C */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_BodyTransferEncoding(valuetype System.Net.Mime.TransferEncoding/*0200000A*/) /* 0600008D */
  } // end of property MailMessage::BodyTransferEncoding
  .property /*17000022*/ instance callconv(8) class System.Net.Mail.MailAddressCollection/*02000014*/
          CC()
  {
    .get instance class System.Net.Mail.MailAddressCollection/*02000014*/ System.Net.Mail.MailMessage/*02000015*/::get_CC() /* 0600008E */
  } // end of property MailMessage::CC
  .property /*17000023*/ instance callconv(8) valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/
          DeliveryNotificationOptions()
  {
    .get instance valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/ System.Net.Mail.MailMessage/*02000015*/::get_DeliveryNotificationOptions() /* 0600008F */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_DeliveryNotificationOptions(valuetype System.Net.Mail.DeliveryNotificationOptions/*02000010*/) /* 06000090 */
  } // end of property MailMessage::DeliveryNotificationOptions
  .property /*17000024*/ instance callconv(8) class System.Net.Mail.MailAddress/*02000013*/
          From()
  {
    .custom /*0C000069:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Mail.MailAddress/*02000013*/ System.Net.Mail.MailMessage/*02000015*/::get_From() /* 06000091 */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_From(class System.Net.Mail.MailAddress/*02000013*/) /* 06000092 */
  } // end of property MailMessage::From
  .property /*17000025*/ instance callconv(8) class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.NameValueCollection/*01000022*/
          Headers()
  {
    .get instance class [System.Collections.Specialized/*23000002*/]System.Collections.Specialized.NameValueCollection/*01000022*/ System.Net.Mail.MailMessage/*02000015*/::get_Headers() /* 06000093 */
  } // end of property MailMessage::Headers
  .property /*17000026*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/
          HeadersEncoding()
  {
    .custom /*0C00006B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ System.Net.Mail.MailMessage/*02000015*/::get_HeadersEncoding() /* 06000094 */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_HeadersEncoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/) /* 06000095 */
  } // end of property MailMessage::HeadersEncoding
  .property /*17000027*/ instance callconv(8) bool
          IsBodyHtml()
  {
    .get instance bool System.Net.Mail.MailMessage/*02000015*/::get_IsBodyHtml() /* 06000096 */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_IsBodyHtml(bool) /* 06000097 */
  } // end of property MailMessage::IsBodyHtml
  .property /*17000028*/ instance callconv(8) valuetype System.Net.Mail.MailPriority/*02000016*/
          Priority()
  {
    .get instance valuetype System.Net.Mail.MailPriority/*02000016*/ System.Net.Mail.MailMessage/*02000015*/::get_Priority() /* 06000098 */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_Priority(valuetype System.Net.Mail.MailPriority/*02000016*/) /* 06000099 */
  } // end of property MailMessage::Priority
  .property /*17000029*/ instance callconv(8) class System.Net.Mail.MailAddress/*02000013*/
          ReplyTo()
  {
    .custom /*0C00006C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .custom /*0C00006D:0A000016*/ instance void [System.Runtime/*23000001*/]System.ObsoleteAttribute/*01000023*/::.ctor(string) /* 0A000016 */ = ( 01 00 5A 52 65 70 6C 79 54 6F 20 68 61 73 20 62   // ..ZReplyTo has b
                                                                                                                                                   65 65 6E 20 64 65 70 72 65 63 61 74 65 64 2E 20   // een deprecated. 
                                                                                                                                                   55 73 65 20 52 65 70 6C 79 54 6F 4C 69 73 74 20   // Use ReplyToList 
                                                                                                                                                   69 6E 73 74 65 61 64 2C 20 77 68 69 63 68 20 63   // instead, which c
                                                                                                                                                   61 6E 20 61 63 63 65 70 74 20 6D 75 6C 74 69 70   // an accept multip
                                                                                                                                                   6C 65 20 61 64 64 72 65 73 73 65 73 2E 00 00 )    // le addresses...
    .get instance class System.Net.Mail.MailAddress/*02000013*/ System.Net.Mail.MailMessage/*02000015*/::get_ReplyTo() /* 0600009A */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_ReplyTo(class System.Net.Mail.MailAddress/*02000013*/) /* 0600009B */
  } // end of property MailMessage::ReplyTo
  .property /*1700002A*/ instance callconv(8) class System.Net.Mail.MailAddressCollection/*02000014*/
          ReplyToList()
  {
    .get instance class System.Net.Mail.MailAddressCollection/*02000014*/ System.Net.Mail.MailMessage/*02000015*/::get_ReplyToList() /* 0600009C */
  } // end of property MailMessage::ReplyToList
  .property /*1700002B*/ instance callconv(8) class System.Net.Mail.MailAddress/*02000013*/
          Sender()
  {
    .custom /*0C000070:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class System.Net.Mail.MailAddress/*02000013*/ System.Net.Mail.MailMessage/*02000015*/::get_Sender() /* 0600009D */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_Sender(class System.Net.Mail.MailAddress/*02000013*/) /* 0600009E */
  } // end of property MailMessage::Sender
  .property /*1700002C*/ instance callconv(8) string
          Subject()
  {
    .get instance string System.Net.Mail.MailMessage/*02000015*/::get_Subject() /* 0600009F */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_Subject(string) /* 060000A0 */
  } // end of property MailMessage::Subject
  .property /*1700002D*/ instance callconv(8) class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/
          SubjectEncoding()
  {
    .custom /*0C000071:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .get instance class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/ System.Net.Mail.MailMessage/*02000015*/::get_SubjectEncoding() /* 060000A1 */
    .set instance void System.Net.Mail.MailMessage/*02000015*/::set_SubjectEncoding(class [System.Runtime/*23000001*/]System.Text.Encoding/*0100001D*/) /* 060000A2 */
  } // end of property MailMessage::SubjectEncoding
  .property /*1700002E*/ instance callconv(8) class System.Net.Mail.MailAddressCollection/*02000014*/
          To()
  {
    .get instance class System.Net.Mail.MailAddressCollection/*02000014*/ System.Net.Mail.MailMessage/*02000015*/::get_To() /* 060000A3 */
  } // end of property MailMessage::To
} // end of class System.Net.Mail.MailMessage

.class /*02000016*/ public auto ansi sealed System.Net.Mail.MailPriority
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*04000012*/ public specialname rtspecialname int32 value__
  .field /*04000013*/ public static literal valuetype System.Net.Mail.MailPriority/*02000016*/ Normal = int32(0x00000000)
  .field /*04000014*/ public static literal valuetype System.Net.Mail.MailPriority/*02000016*/ Low = int32(0x00000001)
  .field /*04000015*/ public static literal valuetype System.Net.Mail.MailPriority/*02000016*/ High = int32(0x00000002)
} // end of class System.Net.Mail.MailPriority

.class /*02000017*/ public auto ansi sealed System.Net.Mail.SendCompletedEventHandler
       extends [System.Runtime/*23000001*/]System.MulticastDelegate/*01000024*/
{
  .method /*060000A6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(object 'object',
                               native int 'method') runtime managed
  // SIG: 20 02 01 1C 18
  {
  } // end of method SendCompletedEventHandler::.ctor

  .method /*060000A7*/ public hidebysig newslot virtual 
          instance void  Invoke(object sender,
                                class [System.ComponentModel.EventBasedAsync/*23000003*/]System.ComponentModel.AsyncCompletedEventArgs/*01000025*/ e) runtime managed
  // SIG: 20 02 01 1C 12 80 95
  {
    .custom /*0C0000B1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  } // end of method SendCompletedEventHandler::Invoke

  .method /*060000A8*/ public hidebysig newslot virtual 
          instance class [System.Runtime/*23000001*/]System.IAsyncResult/*01000026*/ 
          BeginInvoke(object sender,
                      class [System.ComponentModel.EventBasedAsync/*23000003*/]System.ComponentModel.AsyncCompletedEventArgs/*01000025*/ e,
                      class [System.Runtime/*23000001*/]System.AsyncCallback/*01000027*/ callback,
                      object 'object') runtime managed
  // SIG: 20 04 12 80 99 1C 12 80 95 12 80 9D 1C
  {
    .param [1]/*08000095*/ 
    .custom /*0C0000A8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*08000096*/ 
    .custom /*0C0000A9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
  } // end of method SendCompletedEventHandler::BeginInvoke

  .method /*060000A9*/ public hidebysig newslot virtual 
          instance void  EndInvoke(class [System.Runtime/*23000001*/]System.IAsyncResult/*01000026*/ result) runtime managed
  // SIG: 20 01 01 12 80 99
  {
  } // end of method SendCompletedEventHandler::EndInvoke

} // end of class System.Net.Mail.SendCompletedEventHandler

.class /*02000018*/ public auto ansi beforefieldinit System.Net.Mail.SmtpClient
       extends [System.Runtime/*23000001*/]System.Object/*01000015*/
       implements [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
{
  .custom /*0C000054:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000055:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .custom /*0C000056:0A000017*/ instance void [System.Runtime/*23000001*/]System.Runtime.Versioning.UnsupportedOSPlatformAttribute/*01000028*/::.ctor(string) /* 0A000017 */ = ( 01 00 07 62 72 6F 77 73 65 72 00 00 )             // ...browser..
  .interfaceimpl type [System.Runtime/*23000001*/]System.IDisposable/*0100001F*/
  .custom /*0C000028:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000AA*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x231d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpClient::.ctor

  .method /*060000AB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string host) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2325
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpClient::.ctor

  .method /*060000AC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string host,
                               int32 port) cil managed
  // SIG: 20 02 01 0E 08
  {
    // Method begins at RVA 0x232d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001A       */ call       instance void [System.Runtime/*23000001*/]System.Object/*01000015*/::.ctor() /* 0A00001A */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpClient::.ctor

  .method /*060000AD*/ public hidebysig specialname 
          instance class [System.Security.Cryptography.X509Certificates/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ 
          get_ClientCertificates() cil managed
  // SIG: 20 00 12 80 A5
  {
    .custom /*0C0000B2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2335
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_ClientCertificates

  .method /*060000AE*/ public hidebysig specialname 
          instance class [System.Net.Primitives/*23000005*/]System.Net.ICredentialsByHost/*0100002A*/ 
          get_Credentials() cil managed
  // SIG: 20 00 12 80 A9
  {
    // Method begins at RVA 0x2338
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_Credentials

  .method /*060000AF*/ public hidebysig specialname 
          instance void  set_Credentials(class [System.Net.Primitives/*23000005*/]System.Net.ICredentialsByHost/*0100002A*/ 'value') cil managed
  // SIG: 20 01 01 12 80 A9
  {
    // Method begins at RVA 0x233b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_Credentials

  .method /*060000B0*/ public hidebysig specialname 
          instance valuetype System.Net.Mail.SmtpDeliveryFormat/*02000019*/ 
          get_DeliveryFormat() cil managed
  // SIG: 20 00 11 64
  {
    // Method begins at RVA 0x233d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_DeliveryFormat

  .method /*060000B1*/ public hidebysig specialname 
          instance void  set_DeliveryFormat(valuetype System.Net.Mail.SmtpDeliveryFormat/*02000019*/ 'value') cil managed
  // SIG: 20 01 01 11 64
  {
    // Method begins at RVA 0x2340
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_DeliveryFormat

  .method /*060000B2*/ public hidebysig specialname 
          instance valuetype System.Net.Mail.SmtpDeliveryMethod/*0200001A*/ 
          get_DeliveryMethod() cil managed
  // SIG: 20 00 11 68
  {
    // Method begins at RVA 0x2342
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_DeliveryMethod

  .method /*060000B3*/ public hidebysig specialname 
          instance void  set_DeliveryMethod(valuetype System.Net.Mail.SmtpDeliveryMethod/*0200001A*/ 'value') cil managed
  // SIG: 20 01 01 11 68
  {
    // Method begins at RVA 0x2345
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_DeliveryMethod

  .method /*060000B4*/ public hidebysig specialname 
          instance bool  get_EnableSsl() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2347
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_EnableSsl

  .method /*060000B5*/ public hidebysig specialname 
          instance void  set_EnableSsl(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x234a
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_EnableSsl

  .method /*060000B6*/ public hidebysig specialname 
          instance string  get_Host() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x234c
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_Host

  .method /*060000B7*/ public hidebysig specialname 
          instance void  set_Host(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    .param [1]/*080000A1*/ 
    .custom /*0C0000AF:0A000015*/ instance void [System.Runtime/*23000001*/]System.Diagnostics.CodeAnalysis.DisallowNullAttribute/*01000021*/::.ctor() /* 0A000015 */ = ( 01 00 00 00 ) 
    // Method begins at RVA 0x234f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_Host

  .method /*060000B8*/ public hidebysig specialname 
          instance string  get_PickupDirectoryLocation() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x2351
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_PickupDirectoryLocation

  .method /*060000B9*/ public hidebysig specialname 
          instance void  set_PickupDirectoryLocation(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2354
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_PickupDirectoryLocation

  .method /*060000BA*/ public hidebysig specialname 
          instance int32  get_Port() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2356
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_Port

  .method /*060000BB*/ public hidebysig specialname 
          instance void  set_Port(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2359
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_Port

  .method /*060000BC*/ public hidebysig specialname 
          instance class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*0100002B*/ 
          get_ServicePoint() cil managed
  // SIG: 20 00 12 80 AD
  {
    .custom /*0C0000BA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x235b
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_ServicePoint

  .method /*060000BD*/ public hidebysig specialname 
          instance string  get_TargetName() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x235e
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_TargetName

  .method /*060000BE*/ public hidebysig specialname 
          instance void  set_TargetName(string 'value') cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x2361
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_TargetName

  .method /*060000BF*/ public hidebysig specialname 
          instance int32  get_Timeout() cil managed
  // SIG: 20 00 08
  {
    // Method begins at RVA 0x2363
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_Timeout

  .method /*060000C0*/ public hidebysig specialname 
          instance void  set_Timeout(int32 'value') cil managed
  // SIG: 20 01 01 08
  {
    // Method begins at RVA 0x2366
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_Timeout

  .method /*060000C1*/ public hidebysig specialname 
          instance bool  get_UseDefaultCredentials() cil managed
  // SIG: 20 00 02
  {
    // Method begins at RVA 0x2368
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::get_UseDefaultCredentials

  .method /*060000C2*/ public hidebysig specialname 
          instance void  set_UseDefaultCredentials(bool 'value') cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x236b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::set_UseDefaultCredentials

  .method /*060000C3*/ public hidebysig specialname 
          instance void  add_SendCompleted(class System.Net.Mail.SendCompletedEventHandler/*02000017*/ 'value') cil managed
  // SIG: 20 01 01 12 5C
  {
    // Method begins at RVA 0x236d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::add_SendCompleted

  .method /*060000C4*/ public hidebysig specialname 
          instance void  remove_SendCompleted(class System.Net.Mail.SendCompletedEventHandler/*02000017*/ 'value') cil managed
  // SIG: 20 01 01 12 5C
  {
    // Method begins at RVA 0x236f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::remove_SendCompleted

  .method /*060000C5*/ public hidebysig newslot virtual final 
          instance void  Dispose() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2371
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::Dispose

  .method /*060000C6*/ family hidebysig newslot virtual 
          instance void  Dispose(bool disposing) cil managed
  // SIG: 20 01 01 02
  {
    // Method begins at RVA 0x2373
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::Dispose

  .method /*060000C7*/ family hidebysig instance void 
          OnSendCompleted(class [System.ComponentModel.EventBasedAsync/*23000003*/]System.ComponentModel.AsyncCompletedEventArgs/*01000025*/ e) cil managed
  // SIG: 20 01 01 12 80 95
  {
    .custom /*0C0000BD:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2375
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::OnSendCompleted

  .method /*060000C8*/ public hidebysig instance void 
          Send(class System.Net.Mail.MailMessage/*02000015*/ message) cil managed
  // SIG: 20 01 01 12 54
  {
    .custom /*0C0000BE:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2377
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::Send

  .method /*060000C9*/ public hidebysig instance void 
          Send(string from,
               string recipients,
               string subject,
               string body) cil managed
  // SIG: 20 04 01 0E 0E 0E 0E
  {
    .custom /*0C0000BF:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000AE*/ 
    .custom /*0C0000B3:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000AF*/ 
    .custom /*0C0000B4:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2379
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::Send

  .method /*060000CA*/ public hidebysig instance void 
          SendAsync(class System.Net.Mail.MailMessage/*02000015*/ message,
                    object userToken) cil managed
  // SIG: 20 02 01 12 54 1C
  {
    .custom /*0C0000C0:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000B1*/ 
    .custom /*0C0000B5:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x237b
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::SendAsync

  .method /*060000CB*/ public hidebysig instance void 
          SendAsync(string from,
                    string recipients,
                    string subject,
                    string body,
                    object userToken) cil managed
  // SIG: 20 05 01 0E 0E 0E 0E 1C
  {
    .param [1]/*080000B2*/ 
    .custom /*0C0000B6:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .param [2]/*080000B3*/ 
    .custom /*0C0000B7:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x237d
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::SendAsync

  .method /*060000CC*/ public hidebysig instance void 
          SendAsyncCancel() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x237f
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpClient::SendAsyncCancel

  .method /*060000CD*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002C*/ 
          SendMailAsync(class System.Net.Mail.MailMessage/*02000015*/ message) cil managed
  // SIG: 20 01 12 80 B1 12 54
  {
    .custom /*0C0000C1:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2381
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::SendMailAsync

  .method /*060000CE*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002C*/ 
          SendMailAsync(string from,
                        string recipients,
                        string subject,
                        string body) cil managed
  // SIG: 20 04 12 80 B1 0E 0E 0E 0E
  {
    .custom /*0C0000C2:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000BA*/ 
    .custom /*0C0000B8:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000BB*/ 
    .custom /*0C0000B9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2384
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::SendMailAsync

  .method /*060000CF*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002C*/ 
          SendMailAsync(class System.Net.Mail.MailMessage/*02000015*/ message,
                        valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002D*/ cancellationToken) cil managed
  // SIG: 20 02 12 80 B1 12 54 11 80 B5
  {
    .custom /*0C0000C3:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2387
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::SendMailAsync

  .method /*060000D0*/ public hidebysig instance class [System.Runtime/*23000001*/]System.Threading.Tasks.Task/*0100002C*/ 
          SendMailAsync(string from,
                        string recipients,
                        string subject,
                        string body,
                        valuetype [System.Runtime/*23000001*/]System.Threading.CancellationToken/*0100002D*/ cancellationToken) cil managed
  // SIG: 20 05 12 80 B1 0E 0E 0E 0E 11 80 B5
  {
    .custom /*0C0000C4:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    .param [3]/*080000C0*/ 
    .custom /*0C0000BB:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    .param [4]/*080000C1*/ 
    .custom /*0C0000BC:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x238a
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpClient::SendMailAsync

  .event /*14000001*/ System.Net.Mail.SendCompletedEventHandler/*02000017*/ SendCompleted
  {
    .addon instance void System.Net.Mail.SmtpClient/*02000018*/::add_SendCompleted(class System.Net.Mail.SendCompletedEventHandler/*02000017*/) /* 060000C3 */
    .removeon instance void System.Net.Mail.SmtpClient/*02000018*/::remove_SendCompleted(class System.Net.Mail.SendCompletedEventHandler/*02000017*/) /* 060000C4 */
  } // end of event SmtpClient::SendCompleted
  .property /*1700002F*/ instance callconv(8) class [System.Security.Cryptography.X509Certificates/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/
          ClientCertificates()
  {
    .custom /*0C000074:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Security.Cryptography.X509Certificates/*23000004*/]System.Security.Cryptography.X509Certificates.X509CertificateCollection/*01000029*/ System.Net.Mail.SmtpClient/*02000018*/::get_ClientCertificates() /* 060000AD */
  } // end of property SmtpClient::ClientCertificates
  .property /*17000030*/ instance callconv(8) class [System.Net.Primitives/*23000005*/]System.Net.ICredentialsByHost/*0100002A*/
          Credentials()
  {
    .get instance class [System.Net.Primitives/*23000005*/]System.Net.ICredentialsByHost/*0100002A*/ System.Net.Mail.SmtpClient/*02000018*/::get_Credentials() /* 060000AE */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_Credentials(class [System.Net.Primitives/*23000005*/]System.Net.ICredentialsByHost/*0100002A*/) /* 060000AF */
  } // end of property SmtpClient::Credentials
  .property /*17000031*/ instance callconv(8) valuetype System.Net.Mail.SmtpDeliveryFormat/*02000019*/
          DeliveryFormat()
  {
    .get instance valuetype System.Net.Mail.SmtpDeliveryFormat/*02000019*/ System.Net.Mail.SmtpClient/*02000018*/::get_DeliveryFormat() /* 060000B0 */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_DeliveryFormat(valuetype System.Net.Mail.SmtpDeliveryFormat/*02000019*/) /* 060000B1 */
  } // end of property SmtpClient::DeliveryFormat
  .property /*17000032*/ instance callconv(8) valuetype System.Net.Mail.SmtpDeliveryMethod/*0200001A*/
          DeliveryMethod()
  {
    .get instance valuetype System.Net.Mail.SmtpDeliveryMethod/*0200001A*/ System.Net.Mail.SmtpClient/*02000018*/::get_DeliveryMethod() /* 060000B2 */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_DeliveryMethod(valuetype System.Net.Mail.SmtpDeliveryMethod/*0200001A*/) /* 060000B3 */
  } // end of property SmtpClient::DeliveryMethod
  .property /*17000033*/ instance callconv(8) bool
          EnableSsl()
  {
    .get instance bool System.Net.Mail.SmtpClient/*02000018*/::get_EnableSsl() /* 060000B4 */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_EnableSsl(bool) /* 060000B5 */
  } // end of property SmtpClient::EnableSsl
  .property /*17000034*/ instance callconv(8) string
          Host()
  {
    .get instance string System.Net.Mail.SmtpClient/*02000018*/::get_Host() /* 060000B6 */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_Host(string) /* 060000B7 */
  } // end of property SmtpClient::Host
  .property /*17000035*/ instance callconv(8) string
          PickupDirectoryLocation()
  {
    .get instance string System.Net.Mail.SmtpClient/*02000018*/::get_PickupDirectoryLocation() /* 060000B8 */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_PickupDirectoryLocation(string) /* 060000B9 */
  } // end of property SmtpClient::PickupDirectoryLocation
  .property /*17000036*/ instance callconv(8) int32
          Port()
  {
    .get instance int32 System.Net.Mail.SmtpClient/*02000018*/::get_Port() /* 060000BA */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_Port(int32) /* 060000BB */
  } // end of property SmtpClient::Port
  .property /*17000037*/ instance callconv(8) class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*0100002B*/
          ServicePoint()
  {
    .custom /*0C000076:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 01 00 00 ) 
    .get instance class [System.Net.ServicePoint/*23000006*/]System.Net.ServicePoint/*0100002B*/ System.Net.Mail.SmtpClient/*02000018*/::get_ServicePoint() /* 060000BC */
  } // end of property SmtpClient::ServicePoint
  .property /*17000038*/ instance callconv(8) string
          TargetName()
  {
    .get instance string System.Net.Mail.SmtpClient/*02000018*/::get_TargetName() /* 060000BD */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_TargetName(string) /* 060000BE */
  } // end of property SmtpClient::TargetName
  .property /*17000039*/ instance callconv(8) int32
          Timeout()
  {
    .get instance int32 System.Net.Mail.SmtpClient/*02000018*/::get_Timeout() /* 060000BF */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_Timeout(int32) /* 060000C0 */
  } // end of property SmtpClient::Timeout
  .property /*1700003A*/ instance callconv(8) bool
          UseDefaultCredentials()
  {
    .get instance bool System.Net.Mail.SmtpClient/*02000018*/::get_UseDefaultCredentials() /* 060000C1 */
    .set instance void System.Net.Mail.SmtpClient/*02000018*/::set_UseDefaultCredentials(bool) /* 060000C2 */
  } // end of property SmtpClient::UseDefaultCredentials
} // end of class System.Net.Mail.SmtpClient

.class /*02000019*/ public auto ansi sealed System.Net.Mail.SmtpDeliveryFormat
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*04000016*/ public specialname rtspecialname int32 value__
  .field /*04000017*/ public static literal valuetype System.Net.Mail.SmtpDeliveryFormat/*02000019*/ SevenBit = int32(0x00000000)
  .field /*04000018*/ public static literal valuetype System.Net.Mail.SmtpDeliveryFormat/*02000019*/ International = int32(0x00000001)
} // end of class System.Net.Mail.SmtpDeliveryFormat

.class /*0200001A*/ public auto ansi sealed System.Net.Mail.SmtpDeliveryMethod
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*04000019*/ public specialname rtspecialname int32 value__
  .field /*0400001A*/ public static literal valuetype System.Net.Mail.SmtpDeliveryMethod/*0200001A*/ Network = int32(0x00000000)
  .field /*0400001B*/ public static literal valuetype System.Net.Mail.SmtpDeliveryMethod/*0200001A*/ SpecifiedPickupDirectory = int32(0x00000001)
  .field /*0400001C*/ public static literal valuetype System.Net.Mail.SmtpDeliveryMethod/*0200001A*/ PickupDirectoryFromIis = int32(0x00000002)
} // end of class System.Net.Mail.SmtpDeliveryMethod

.class /*0200001B*/ public auto ansi beforefieldinit System.Net.Mail.SmtpException
       extends [System.Runtime/*23000001*/]System.Exception/*0100002E*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/
{
  .custom /*0C000058:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C000059:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/
  .custom /*0C00002B:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000D1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x238d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100002E*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpException::.ctor

  .method /*060000D2*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ statusCode) cil managed
  // SIG: 20 01 01 11 78
  {
    // Method begins at RVA 0x2395
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100002E*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpException::.ctor

  .method /*060000D3*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ statusCode,
                               string message) cil managed
  // SIG: 20 02 01 11 78 0E
  {
    // Method begins at RVA 0x239d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100002E*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpException::.ctor

  .method /*060000D4*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ serializationInfo,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ streamingContext) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    .custom /*0C0000C5:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23a5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100002E*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpException::.ctor

  .method /*060000D5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23ad
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100002E*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpException::.ctor

  .method /*060000D6*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100002E*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 B9
  {
    // Method begins at RVA 0x23b5
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (0A)00001F       */ call       instance void [System.Runtime/*23000001*/]System.Exception/*0100002E*/::.ctor() /* 0A00001F */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpException::.ctor

  .method /*060000D7*/ public hidebysig specialname 
          instance valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ 
          get_StatusCode() cil managed
  // SIG: 20 00 11 78
  {
    // Method begins at RVA 0x23bd
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpException::get_StatusCode

  .method /*060000D8*/ public hidebysig specialname 
          instance void  set_StatusCode(valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ 'value') cil managed
  // SIG: 20 01 01 11 78
  {
    // Method begins at RVA 0x23c0
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpException::set_StatusCode

  .method /*060000D9*/ public hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ streamingContext) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    .custom /*0C0000C6:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23c2
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpException::GetObjectData

  .method /*060000DA*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ streamingContext) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/::GetObjectData /*0100002F::0A000018*/ 
    // Method begins at RVA 0x23c4
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpException::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*1700003B*/ instance callconv(8) valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/
          StatusCode()
  {
    .get instance valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ System.Net.Mail.SmtpException/*0200001B*/::get_StatusCode() /* 060000D7 */
    .set instance void System.Net.Mail.SmtpException/*0200001B*/::set_StatusCode(valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/) /* 060000D8 */
  } // end of property SmtpException::StatusCode
} // end of class System.Net.Mail.SmtpException

.class /*0200001C*/ public auto ansi beforefieldinit System.Net.Mail.SmtpFailedRecipientException
       extends System.Net.Mail.SmtpException/*0200001B*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/
{
  .custom /*0C00005B:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
  .custom /*0C00005C:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/
  .custom /*0C00002E:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000DB*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x23c6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000D1       */ call       instance void System.Net.Mail.SmtpException/*0200001B*/::.ctor() /* 060000D1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::.ctor

  .method /*060000DC*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ statusCode,
                               string failedRecipient) cil managed
  // SIG: 20 02 01 11 78 0E
  {
    // Method begins at RVA 0x23ce
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000D1       */ call       instance void System.Net.Mail.SmtpException/*0200001B*/::.ctor() /* 060000D1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::.ctor

  .method /*060000DD*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ statusCode,
                               string failedRecipient,
                               string serverResponse) cil managed
  // SIG: 20 03 01 11 78 0E 0E
  {
    // Method begins at RVA 0x23d6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000D1       */ call       instance void System.Net.Mail.SmtpException/*0200001B*/::.ctor() /* 060000D1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::.ctor

  .method /*060000DE*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ context) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    .custom /*0C0000C7:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x23de
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000D1       */ call       instance void System.Net.Mail.SmtpException/*0200001B*/::.ctor() /* 060000D1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::.ctor

  .method /*060000DF*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    // Method begins at RVA 0x23e6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000D1       */ call       instance void System.Net.Mail.SmtpException/*0200001B*/::.ctor() /* 060000D1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::.ctor

  .method /*060000E0*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100002E*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 B9
  {
    // Method begins at RVA 0x23ee
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000D1       */ call       instance void System.Net.Mail.SmtpException/*0200001B*/::.ctor() /* 060000D1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::.ctor

  .method /*060000E1*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               string failedRecipient,
                               class [System.Runtime/*23000001*/]System.Exception/*0100002E*/ innerException) cil managed
  // SIG: 20 03 01 0E 0E 12 80 B9
  {
    // Method begins at RVA 0x23f6
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000D1       */ call       instance void System.Net.Mail.SmtpException/*0200001B*/::.ctor() /* 060000D1 */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::.ctor

  .method /*060000E2*/ public hidebysig specialname 
          instance string  get_FailedRecipient() cil managed
  // SIG: 20 00 0E
  {
    // Method begins at RVA 0x23fe
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpFailedRecipientException::get_FailedRecipient

  .method /*060000E3*/ public hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ streamingContext) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    .custom /*0C0000C8:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
    // Method begins at RVA 0x2401
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::GetObjectData

  .method /*060000E4*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ streamingContext) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/::GetObjectData /*0100002F::0A000018*/ 
    // Method begins at RVA 0x2403
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientException::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*1700003C*/ instance callconv(8) string
          FailedRecipient()
  {
    .get instance string System.Net.Mail.SmtpFailedRecipientException/*0200001C*/::get_FailedRecipient() /* 060000E2 */
  } // end of property SmtpFailedRecipientException::FailedRecipient
} // end of class System.Net.Mail.SmtpFailedRecipientException

.class /*0200001D*/ public auto ansi beforefieldinit System.Net.Mail.SmtpFailedRecipientsException
       extends System.Net.Mail.SmtpFailedRecipientException/*0200001C*/
       implements [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/
{
  .custom /*0C00005E:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 01 00 00 ) 
  .custom /*0C00005F:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .interfaceimpl type [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/
  .custom /*0C000032:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 00 00 00 ) 
  .method /*060000E5*/ public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  // SIG: 20 00 01
  {
    // Method begins at RVA 0x2405
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000DB       */ call       instance void System.Net.Mail.SmtpFailedRecipientException/*0200001C*/::.ctor() /* 060000DB */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientsException::.ctor

  .method /*060000E6*/ family hidebysig specialname rtspecialname 
          instance void  .ctor(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ info,
                               valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ context) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    // Method begins at RVA 0x240d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000DB       */ call       instance void System.Net.Mail.SmtpFailedRecipientException/*0200001C*/::.ctor() /* 060000DB */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientsException::.ctor

  .method /*060000E7*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message) cil managed
  // SIG: 20 01 01 0E
  {
    .custom /*0C0000CA:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2415
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000DB       */ call       instance void System.Net.Mail.SmtpFailedRecipientException/*0200001C*/::.ctor() /* 060000DB */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientsException::.ctor

  .method /*060000E8*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class [System.Runtime/*23000001*/]System.Exception/*0100002E*/ innerException) cil managed
  // SIG: 20 02 01 0E 12 80 B9
  {
    .custom /*0C0000CB:06000004*/ instance void System.Runtime.CompilerServices.NullableContextAttribute/*02000004*/::.ctor(uint8) /* 06000004 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x241d
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000DB       */ call       instance void System.Net.Mail.SmtpFailedRecipientException/*0200001C*/::.ctor() /* 060000DB */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientsException::.ctor

  .method /*060000E9*/ public hidebysig specialname rtspecialname 
          instance void  .ctor(string message,
                               class System.Net.Mail.SmtpFailedRecipientException/*0200001C*/[] innerExceptions) cil managed
  // SIG: 20 02 01 0E 1D 12 70
  {
    .param [1]/*080000E6*/ 
    .custom /*0C0000C9:06000002*/ instance void System.Runtime.CompilerServices.NullableAttribute/*02000003*/::.ctor(uint8) /* 06000002 */ = ( 01 00 02 00 00 ) 
    // Method begins at RVA 0x2425
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  /* 02   |                  */ ldarg.0
    IL_0001:  /* 28   | (06)0000DB       */ call       instance void System.Net.Mail.SmtpFailedRecipientException/*0200001C*/::.ctor() /* 060000DB */
    IL_0006:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientsException::.ctor

  .method /*060000EA*/ public hidebysig specialname 
          instance class System.Net.Mail.SmtpFailedRecipientException/*0200001C*/[] 
          get_InnerExceptions() cil managed
  // SIG: 20 00 1D 12 70
  {
    // Method begins at RVA 0x242d
    // Code size       2 (0x2)
    .maxstack  8
    IL_0000:  /* 14   |                  */ ldnull
    IL_0001:  /* 7A   |                  */ throw
  } // end of method SmtpFailedRecipientsException::get_InnerExceptions

  .method /*060000EB*/ public hidebysig virtual 
          instance void  GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ serializationInfo,
                                       valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ streamingContext) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    // Method begins at RVA 0x2430
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientsException::GetObjectData

  .method /*060000EC*/ private hidebysig newslot virtual final 
          instance void  System.Runtime.Serialization.ISerializable.GetObjectData(class [System.Runtime/*23000001*/]System.Runtime.Serialization.SerializationInfo/*01000030*/ serializationInfo,
                                                                                  valuetype [System.Runtime/*23000001*/]System.Runtime.Serialization.StreamingContext/*01000031*/ streamingContext) cil managed
  // SIG: 20 02 01 12 80 C1 11 80 C5
  {
    .override [System.Runtime/*23000001*/]System.Runtime.Serialization.ISerializable/*0100002F*/::GetObjectData /*0100002F::0A000018*/ 
    // Method begins at RVA 0x2432
    // Code size       1 (0x1)
    .maxstack  8
    IL_0000:  /* 2A   |                  */ ret
  } // end of method SmtpFailedRecipientsException::System.Runtime.Serialization.ISerializable.GetObjectData

  .property /*1700003D*/ instance callconv(8) class System.Net.Mail.SmtpFailedRecipientException/*0200001C*/[]
          InnerExceptions()
  {
    .get instance class System.Net.Mail.SmtpFailedRecipientException/*0200001C*/[] System.Net.Mail.SmtpFailedRecipientsException/*0200001D*/::get_InnerExceptions() /* 060000EA */
  } // end of property SmtpFailedRecipientsException::InnerExceptions
} // end of class System.Net.Mail.SmtpFailedRecipientsException

.class /*0200001E*/ public auto ansi sealed System.Net.Mail.SmtpStatusCode
       extends [System.Runtime/*23000001*/]System.Enum/*0100001A*/
{
  .field /*0400001D*/ public specialname rtspecialname int32 value__
  .field /*0400001E*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ GeneralFailure = int32(0xFFFFFFFF)
  .field /*0400001F*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ SystemStatus = int32(0x000000D3)
  .field /*04000020*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ HelpMessage = int32(0x000000D6)
  .field /*04000021*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ ServiceReady = int32(0x000000DC)
  .field /*04000022*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ ServiceClosingTransmissionChannel = int32(0x000000DD)
  .field /*04000023*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ Ok = int32(0x000000FA)
  .field /*04000024*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ UserNotLocalWillForward = int32(0x000000FB)
  .field /*04000025*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ CannotVerifyUserWillAttemptDelivery = int32(0x000000FC)
  .field /*04000026*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ StartMailInput = int32(0x00000162)
  .field /*04000027*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ ServiceNotAvailable = int32(0x000001A5)
  .field /*04000028*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ MailboxBusy = int32(0x000001C2)
  .field /*04000029*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ LocalErrorInProcessing = int32(0x000001C3)
  .field /*0400002A*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ InsufficientStorage = int32(0x000001C4)
  .field /*0400002B*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ ClientNotPermitted = int32(0x000001C6)
  .field /*0400002C*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ CommandUnrecognized = int32(0x000001F4)
  .field /*0400002D*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ SyntaxError = int32(0x000001F5)
  .field /*0400002E*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ CommandNotImplemented = int32(0x000001F6)
  .field /*0400002F*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ BadCommandSequence = int32(0x000001F7)
  .field /*04000030*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ CommandParameterNotImplemented = int32(0x000001F8)
  .field /*04000031*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ MustIssueStartTlsFirst = int32(0x00000212)
  .field /*04000032*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ MailboxUnavailable = int32(0x00000226)
  .field /*04000033*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ UserNotLocalTryAlternatePath = int32(0x00000227)
  .field /*04000034*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ ExceededStorageAllocation = int32(0x00000228)
  .field /*04000035*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ MailboxNameNotAllowed = int32(0x00000229)
  .field /*04000036*/ public static literal valuetype System.Net.Mail.SmtpStatusCode/*0200001E*/ TransactionFailed = int32(0x0000022A)
} // end of class System.Net.Mail.SmtpStatusCode


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
